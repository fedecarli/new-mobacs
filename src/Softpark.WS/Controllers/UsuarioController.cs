using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Softpark.Models;
using System.Configuration;
using Softpark.WS.ViewModels;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Threading;
using System.Web.Security;
using System.Security.Principal;
using System.Linq;
using System.IO;
using Softpark.Infrastructure.Extensions;
using System.Web.Script.Serialization;
using System.IdentityModel;

namespace Softpark.WS.Controllers
{
    public static class Identidade
    {
        private static ASSMED_Usuario _usuario;

        public static ASSMED_Usuario Usuario(this IPrincipal principal)
        {
            return _usuario ?? (_usuario = DomainContainer.Current.ASSMED_Usuario.Find(principal?.Identity?.Name));
        }
    }

    [Authorize]
    public class UsuarioController : Controller
    {
        private DomainContainer db = new DomainContainer();

        private static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        private static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public UsuarioController()
        {
            ViewData.Add("nomeInicialSistema", ConfigurationManager.AppSettings["nomeInicialSistema"]);
            ViewData.Add("logoInicialSistema", ConfigurationManager.AppSettings["logoInicialSistema"]);

            var xIp = System.Web.HttpContext.Current.Request.UserHostAddress;
            var nav = System.Web.HttpContext.Current.Request.UserAgent;

            var xMessNav = "";
            if (nav.Contains("MSIE"))
            {
                if (nav.Contains("MSIE 8.0"))
                    xMessNav = "Seu Navegador não é compatível, utilize o <a href='https://www.google.com.br/chrome/browser/desktop/index.html'><img src='img/chrome.png' style='width: 17px;margin-top: -3px;'/> Google Chrome</a> ou <img src='img/iexplorer.png' style='width: 16px;margin-top: -3px;'/> Internet Explorer 10 ou superior!";
                else if (nav.Contains("MSIE 7.0"))
                    xMessNav = "Seu Navegador não é compatível, utilize o <a href='https://www.google.com.br/chrome/browser/desktop/index.html'><img src='img/chrome.png' style='width: 17px;margin-top: -3px;'/> Google Chrome</a> ou <img src='img/iexplorer.png' style='width: 16px;margin-top: -3px;'/> Internet Explorer 10 ou superior!";
                else if (nav.Contains("MSIE6.0"))
                    xMessNav = "Seu Navegador não é compatível, utilize o <a href='https://www.google.com.br/chrome/browser/desktop/index.html'><img src='img/chrome.png' style='width: 17px;margin-top: -3px;'/> Google Chrome</a> ou <img src='img/iexplorer.png' style='width: 16px;margin-top: -3px;'/> Internet Explorer 10 ou superior!";
                else
                    xMessNav = "Para uma melhor visualização, utilize o <a href='https://www.google.com.br/chrome/browser/desktop/index.html'><img src='img/chrome.png' style='width: 17px;margin-top: -3px;'/> Google Chrome</a> ou <img src='img/iexplorer.png' style='width: 16px;margin-top: -3px;'/> Internet Explorer 10 ou superior!";
            }

            ViewData.Add("xMessNav", xMessNav);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Acesso(Uri ReturnUrl = null)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View(new UsuarioLoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Acesso([System.Web.Http.FromBody] UsuarioLoginViewModel model, [System.Web.Http.FromUri] Uri ReturnUrl = null)
        {
            try
            {
                ViewBag.ReturnUrl = ReturnUrl;

                if (ModelState.IsValid)
                {
                    var cancelation = new CancellationTokenSource();
                    var usuario = await db.ASSMED_Usuario.SingleOrDefaultAsync(x => x.Email == model.Login);

                    using (var md5 = MD5.Create())
                    {
                        if (usuario == null || !VerifyMd5Hash(md5, model.Senha, usuario.Senha))
                        {
                            ModelState.AddModelError("", "Login ou Senha inválidos.");
                        }
                        else
                        {
                            var serializer = new JavaScriptSerializer();

                            var contentModel = serializer.Serialize(model.ForToken());
                            
                            var postModel = new UsuarioLoginUnidadeViewModelPost
                            {
                                IdUsuario = usuario.CodUsu,
                                Setores = await (from setor in db.Setores
                                                 join us in db.ASSMED_UsuarioSetor
                                                 on setor.CodSetor equals us.CodSetor
                                                 where us.CodUsuD == usuario.CodUsu
                                                 select new SetorDDLViewModel { Value = setor.CodSetor, Text = setor.DesSetor }).ToArrayAsync(),
                                Token = contentModel.Encrypt(usuario.Senha),
                                NomeUsuario = usuario.Nome
                            };

                            if (postModel.Setores.Count == 1)
                            {
                                postModel.Setor = postModel.Setores.First().Value;

                                return await AcessoUnidade(postModel, ReturnUrl);
                            }

                            return View("AcessoUnidade", postModel);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e);
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AcessoUnidade([System.Web.Http.FromBody] UsuarioLoginUnidadeViewModel postModel, [System.Web.Http.FromUri] Uri ReturnUrl = null)
        {
            var viewModel = new UsuarioLoginUnidadeViewModelPost
            {
                IdUsuario = postModel.IdUsuario,
                Setor = postModel.Setor,
                Token = postModel.Token
            };

            try
            {
                ViewBag.ReturnUrl = ReturnUrl;

                var usuario = await db.ASSMED_Usuario.SingleOrDefaultAsync(x => x.CodUsu == postModel.IdUsuario);

                viewModel.Setores = await (from setor in db.Setores
                                           join us in db.ASSMED_UsuarioSetor
                                           on setor.CodSetor equals us.CodSetor
                                           where us.CodUsuD == usuario.CodUsu
                                           select new SetorDDLViewModel { Value = setor.CodSetor, Text = setor.DesSetor }).ToArrayAsync();

                viewModel.NomeUsuario = usuario.Nome;

                if (ModelState.IsValid)
                {
                    var cancelation = new CancellationTokenSource();

                    var tokenUser = postModel.Token.Decrypt(usuario.Senha);

                    var serializer = new JavaScriptSerializer();

                    var model = serializer.Deserialize<UsuarioLoginViewModelToken>(tokenUser);

                    if (model == null || !model.IsValid())
                        return Redirect(ReturnUrl.ToString());

                    using (var md5 = MD5.Create())
                    {
                        if (usuario == null || !VerifyMd5Hash(md5, model.Senha, usuario.Senha))
                        {
                            ModelState.AddModelError("", "Login ou Senha inválidos.");
                        }
                        if(viewModel.Setores.All(x => x.Value != postModel.Setor))
                        {
                            ModelState.AddModelError("", "Selecione um setor.");
                        }
                        else
                        {
                            FormsAuthentication.SetAuthCookie(usuario.Email, false);

                            Session.Add("CodSetor", postModel.Setor.ToString());
                            
                            if (ReturnUrl == null)
                                return RedirectToAction("Index", "Home");

                            return Redirect(ReturnUrl.ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e);
            }

            return View(viewModel);
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Auth([System.Web.Http.FromBody] UsuarioLoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var cancelation = new CancellationTokenSource();
                    var usuario = await db.ASSMED_Usuario.SingleOrDefaultAsync(x => x.Email == model.Login);

                    using (var md5 = MD5.Create())
                    {
                        if (usuario == null || !VerifyMd5Hash(md5, model.Senha, usuario.Senha))
                        {
                            ModelState.AddModelError("", "Login ou Senha inválidos.");
                        }
                        else
                        {
                            var serializer = new JavaScriptSerializer();

                            var contentModel = serializer.Serialize(model.ForToken());

                            FormsAuthentication.SetAuthCookie(usuario.Email, true);

                            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e);
            }

            return Json(new { Success = false, Message = ModelState.SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage).Aggregate((x, y) => x + "\n" + y) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Sair()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // GET: Usuario
        public async Task<ActionResult> Index()
        {
            return View(await db.ASSMED_Usuario.ToListAsync());
        }

        // GET: Usuario/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ASSMED_Usuario aSSMED_Usuario = await db.ASSMED_Usuario.FindAsync(id);
            if (aSSMED_Usuario == null)
            {
                return HttpNotFound();
            }
            return View(aSSMED_Usuario);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CodUsu,Login,Nome,Senha,DtSistema,Email,NumIP,CEP,NomeCidade,UF,NumLog,Ativo,tipo_end,logradouro,numero,bairro,municipio,complemento,fone,id_user_cadastro,data_cadastro,id_user_alteracao,data_alteracao,cpf")] ASSMED_Usuario aSSMED_Usuario)
        {
            if (ModelState.IsValid)
            {
                db.ASSMED_Usuario.Add(aSSMED_Usuario);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(aSSMED_Usuario);
        }

        // GET: Usuario/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ASSMED_Usuario aSSMED_Usuario = await db.ASSMED_Usuario.FindAsync(id);
            if (aSSMED_Usuario == null)
            {
                return HttpNotFound();
            }
            return View(aSSMED_Usuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CodUsu,Login,Nome,Senha,DtSistema,Email,NumIP,CEP,NomeCidade,UF,NumLog,Ativo,tipo_end,logradouro,numero,bairro,municipio,complemento,fone,id_user_cadastro,data_cadastro,id_user_alteracao,data_alteracao,cpf")] ASSMED_Usuario aSSMED_Usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aSSMED_Usuario).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(aSSMED_Usuario);
        }

        // GET: Usuario/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ASSMED_Usuario aSSMED_Usuario = await db.ASSMED_Usuario.FindAsync(id);
            if (aSSMED_Usuario == null)
            {
                return HttpNotFound();
            }
            return View(aSSMED_Usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ASSMED_Usuario aSSMED_Usuario = await db.ASSMED_Usuario.FindAsync(id);
            db.ASSMED_Usuario.Remove(aSSMED_Usuario);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
