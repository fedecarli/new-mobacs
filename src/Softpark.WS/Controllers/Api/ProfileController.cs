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
        /// <param name="login"></param>
        /// <param name="senha"></param>
        /// <returns></returns>
        [HttpGet, Route("api/Login/ConsultarLogin/{login}/{senha}")]
        [ResponseType(typeof(LoginViewModel[]))]
        public async Task<IHttpActionResult> ConsultarLogin([FromUri, Required(AllowEmptyStrings = false, ErrorMessage = "Informe um Login.")] string login,
            [FromUri, Required(AllowEmptyStrings = false, ErrorMessage = "Informe a senha.")] string senha)
        {
            var lvm = new[]{ new LoginViewModel {
                CBO = null,
                CNES = null,
                CNS = null,
                CodUsuario = 0,
                Email = null,
                INE = null,
                Login = null,
                NomeUsuario = null,
                Mensagem = "Usuário ou Senha inválidos",
                Senha = null,
                Sucesso = false
            }};

            if (!ModelState.IsValid)
                return Ok(lvm);

            var acesso = Domain.VW_UsuariosACS().Where(x => x.CNS == login || x.CodCred.ToString() == login ||
            x.Matricula == login || x.Codigo.ToString() == login || x.CodUsu.ToString() == login || x.Email == login);

            if (acesso.Count() != 1)
                return Ok(lvm);

            var ua = acesso.Single();

            var usuario = Domain.ASSMED_Usuario.Single(x => x.CodUsu == ua.CodUsu);

            var cp = (Domain.SIGSM_ServicoSerializador_Config.SingleOrDefault(x => x.Configuracao == "authCryptAlg")?.Valor ?? "MD5").ToUpper();

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

            var pass = Encoding.ASCII.GetBytes(senha);

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
                Senha = ua.Senha,
                Sucesso = true
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
    }
}
