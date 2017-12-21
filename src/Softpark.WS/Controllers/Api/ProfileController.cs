using System;
using System.Linq;
using System.Web.Http;
using Softpark.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Softpark.WS.ViewModels;
using System.Web.Http.Description;
using System.Threading.Tasks;
using System.Data.Entity;
using Newtonsoft.Json;
using System.IO;

namespace Softpark.WS.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [System.Web.Mvc.OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
    [System.Web.Mvc.SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class ProfileController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public ProfileController() : base(new DomainContainer()) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ul"></param>
        /// <returns></returns>
        [HttpPost, Route("api/Login/ConsultarLogin")]
        [ResponseType(typeof(LoginViewModel[]))]
        public async Task<IHttpActionResult> ConsultarLogin([FromBody, Required] UsuarioLoginViewModel ul)
        {
            var lvm = new[]{ new LoginViewModel {
                Mensagem = "Usuário ou Senha inválidos"
            }};

            if (!ModelState.IsValid)
                return Ok(lvm);

            var acesso = Domain.VW_UsuariosACS().Where(x => x.CNS == ul.login || x.Login == ul.login);

            if (acesso.Count() != 1)
                return Ok(lvm);

            var ua = acesso.Single();

            var usuario = await Domain.ASSMED_Usuario.SingleAsync(x => x.CodUsu == ua.CodUsu);

            var cp = ((await Domain.SIGSM_ServicoSerializador_Config.SingleOrDefaultAsync(x => x.Configuracao == "authCryptAlg"))?.Valor ?? "MD5").ToUpper();

            HashAlgorithm hashAlgorithm;

            switch (cp)
            {
                case "SHA512":
                    hashAlgorithm = SHA512.Create();
                    break;

                case "SHA256":
                    hashAlgorithm = SHA256.Create();
                    break;

                case "SHA1":
                    hashAlgorithm = SHA1.Create();
                    break;

                default:
                    hashAlgorithm = MD5.Create();
                    break;
            }
            
            var pass = Encoding.ASCII.GetBytes(DecryptStringAES(ul.senha));

            var crypted = hashAlgorithm.ComputeHash(pass);

            var compare = crypted.Aggregate("", (a, b) => a + b.ToString("X2"));

            if (compare.ToUpper() != usuario.Senha?.ToUpper())
                return Ok(lvm);

            var ines = Domain.Database.SqlQuery<SetoresINEs>("SELECT CodINE, NumContrato, CodSetor, Numero, Descricao FROM SetoresINEs");

            lvm = new[] {new LoginViewModel
            {
                CBO = ua.CBO,
                CNES = ua.CNES,
                CNS = ua.CNS,
                CodUsuario = ua.CodUsu,
                Email = ua.Email,
                INE = ua.INE,
                Login = ua.Login,
                NomeUsuario = ua.Nome,
                Mensagem = string.Empty,
                Sucesso = true,
                IBGE = (await Domain.ASSMED_Contratos.SingleAsync()).CodigoIbgeMunicipio
            }};

            return Ok(lvm);
        }

        private static log4net.ILog Log { get; set; } = log4net.LogManager.GetLogger(typeof(ProfileController));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost, Route("api/LogMobile")]
        public void LogMobile([FromBody] string value)
        {
            Log.Info("-----");
            Log.Info("POST api/LogMobile");

            var serializer = new JsonSerializer();

            Log.Info(JsonConvert.SerializeObject(new { LogDescricao = value, DtLog = DateTime.Now }));
        }

        private static string DecryptStringAES(string text)
        {
            var keybytes = Encoding.UTF8.GetBytes("7CCB5E624FD29283");
            var iv = Encoding.UTF8.GetBytes("7061737323313233");

            //DECRYPT FROM CRIPTOJS
            var encrypted = Convert.FromBase64String(text);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);

            return decriptedFromJavascript;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
    }
}
