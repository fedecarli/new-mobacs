using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Softpark.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Softpark.WS.ViewModels;

namespace Softpark.WS.Controllers.Api
{
    ///// <summary>
    ///// 
    ///// </summary>
    //[System.Web.Mvc.OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
    //[System.Web.Mvc.SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    //public class ProfileController : BaseApiController
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    protected ProfileController() : base(new DomainContainer()) { }

    //    [HttpGet, Route("api/Login/ConsultarLogin/{login}/{senha}")]
    //    public IHttpActionResult ConsultarLogin([FromUri, Required(AllowEmptyStrings = false, ErrorMessage = "Informe um Login.")] string login,
    //        [FromUri, Required(AllowEmptyStrings = false, ErrorMessage = "Informe a senha.")] string senha)
    //    {
    //        if (!ModelState.IsValid || Domain.ASSMED_Usuario.Count(x => x.Login == login) != 1)
    //            return BadRequest("Usuário ou Senha inválidos.");

    //        var usuario = Domain.ASSMED_Usuario.Single(x => x.Login == login);

    //        var cp = (Domain.SIGSM_ServicoSerializador_Config.SingleOrDefault(x => x.Configuracao == "authCryptAlg")?.Valor ?? "MD5").ToUpper();

    //        HashAlgorithm hashAlgorithm;

    //        switch (cp)
    //        {
    //            case "SHA512":
    //                hashAlgorithm = SHA512.Create();
    //                break;

    //            case "SHA256":
    //                hashAlgorithm = SHA256.Create();
    //                break;

    //            case "SHA1":
    //                hashAlgorithm = SHA1.Create();
    //                break;

    //            default:
    //                hashAlgorithm = MD5.Create();
    //                break;
    //        }

    //        var pass = Encoding.ASCII.GetBytes(senha);

    //        var crypted = hashAlgorithm.ComputeHash(pass);

    //        var compare = crypted.Aggregate("", (a, b) => a + b.ToString("X2"));

    //        if (compare != usuario.Senha)
    //            return BadRequest("Usuário ou Senha inválidos.");

    //        var ines = Domain.Database.SqlQuery<SetoresINEs>("SELECT CodINE, NumContrato, CodSetor, Numero, Descricao FROM SetoresINEs");

    //        var profs = (from ac in Domain.ASSMED_Cadastro
    //                     join acdp in Domain.ASSMED_CadastroDocPessoal
    //                     on ac.Codigo equals acdp.Codigo
    //                     join acr in Domain.AS_Credenciados
    //                     on ac.Codigo equals acr.Codigo
    //                     join acv in Domain.AS_CredenciadosVinc
    //                     on acr.CodCred equals acv.CodCred
    //                     join acu in Domain.AS_CredenciadosUsu
    //                     on acr.CodCred equals acu.CodCred
    //                     join asu in Domain.ASSMED_Usuario
    //                     on acu.CodUsuD equals asu.CodUsu
    //                     join apt in Domain.AS_ProfissoesTab
    //                     on acv.CodProfTab.Trim() equals apt.CodProfTab.Trim()
    //                     join asp in Domain.AS_SetoresPar
    //                     on acv.CNESLocal.Trim() equals asp.CNES.Trim()
    //                     join set in Domain.Setores
    //                     on asp.CodSetor equals set.CodSetor
    //                     let ine = Domain.SetoresINEs.Where(x => x.CodSetor == set.CodSetor && x.Numero != null).DefaultIfEmpty().FirstOrDefault()
    //                     where asu.CodUsu == usuario.CodUsu &&
    //                     apt.CodProfTab != null &&
    //                     acdp.Numero != null &&
    //                     acdp.CodTpDocP == 6 &&
    //                     acv.CNESLocal != null
    //                     select new LoginViewModel
    //                     {
    //                         CBO = apt.CodProfTab.Trim(),
    //                         CNES = acv.CNESLocal.Trim(),
    //                         CNS = acdp.Numero.ToString(),
    //                         CodUsuario = asu.CodUsu,
    //                         Email = asu.Email,
    //                         INE = ine.Numero != null ? ine.Numero.Trim() : null,
    //                         Login = asu.Login,
    //                         NomeUsuario = asu.Nome,
    //                         Mensagem = string.Empty,
    //                         Senha = asu.Senha
    //                     }).Distinct().ToList()
    //                    .Where(prof => null != Domain.GetProfissionalMobile(prof.CNES, prof.INE, prof.CBO, prof.CNS));

    //        return Ok(profs.ToArray());
    //    }

    //    [HttpPost, Route("api/LogMobile")]
    //    public void LogMobile([FromBody]string value)
    //    {
    //        LogMobileModel logMLogMobileModel = new LogMobileModel();

    //        logMLogMobileModel.LogDescricao = value;
    //        logMLogMobileModel.DtLog = DateTime.Now;

    //        ServiceLogMobile serviceLogMobile = new ServiceLogMobile();
    //        serviceLogMobile.GravaLogMobile(logMLogMobileModel);
    //    }

    //    // POST: api/Pesquisa/PesquisaResponsavel
    //    [HttpPost, Route("api/Pesquisa/PesquisaResponsavel")]
    //    public IHttpActionResult PesquisaResponsavel([FromBody] UnicoPessoasModel model)
    //    {
    //        ServicePesquisa ServicePesquisa = new ServicePesquisa();
    //        var resposta = ServicePesquisa.BuscaResponsavel(model);

    //        return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, resposta));
    //    }
    //}
}
