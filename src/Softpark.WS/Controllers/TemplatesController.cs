using Softpark.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Softpark.WS.Controllers
{
    public class TemplatesController : Controller
    {
        private DomainContainer db = new DomainContainer();

        public class Pessoa
        {
            public ASSMED_Cadastro ASSMED_Cadastro { get; set; }
            public AS_CredenciadosUsu AS_CredenciadosUsu { get; set; }
            public AS_Credenciados AS_Credenciados { get; set; }
            public AS_CredenciadosVinc AS_CredenciadosVinc { get; set; }
        }

        [Route("Cabecalho/{id:int}/{idSetor:int?}")]
        public async Task<ActionResult> Cabecalho([Required] int id, int? idSetor = null)
        {
            IEnumerable<Pessoa> pessoa =
            await (from ca in db.ASSMED_Cadastro
                   join asusu in db.AS_CredenciadosUsu
                   on new { CodUsu = ca.CodUsu, NumContrato = ca.NumContrato } equals new { CodUsu = asusu.CodUsuD, NumContrato = asusu.NumContrato }
                   join cred in db.AS_Credenciados
                   on new { CodCred = asusu.CodCred, NumContrato = asusu.NumContrato } equals
                   new { CodCred = cred.CodCred, NumContrato = cred.NumContrato }
                   join vinc in db.AS_CredenciadosVinc on
                   new { NumContrato = cred.NumContrato, CodCred = cred.CodCred } equals new { NumContrato = vinc.NumContrato, CodCred = vinc.CodCred }
                   where ca.CodUsu == id && ca.Codigo == cred.Codigo && ca.NumContrato == 22
                   && vinc.CodSetor == (idSetor ?? vinc.CodSetor)
                   select new Pessoa { ASSMED_Cadastro = ca, AS_CredenciadosUsu = asusu, AS_Credenciados = cred, AS_CredenciadosVinc = vinc }).ToListAsync();

            ViewBag.idSetor = idSetor;
            AS_SetoresPar setor = null;
            if (idSetor != null)
                setor = db.AS_SetoresPar.Single(x => x.CodSetor == idSetor);

            AS_ProfissoesTab[] profs = pessoa.SelectMany(x => x.AS_CredenciadosVinc.AS_TabProfissao.AS_ProfissoesTab).ToArray();

            VW_Profissional[] profiss = new VW_Profissional[0];

            if (setor != null)
                profiss = db.VW_Profissional.AsEnumerable().Where(x => x.CNES.Trim() == setor.CNES.Trim()).ToArray();
            else
                profiss = db.VW_Profissional.ToArray();

            SetoresINEs[] ines = new SetoresINEs[0];
            if (setor != null)
                ines = db.SetoresINEs.Where(x => x.CodSetor == idSetor).ToArray();
            else
                ines = db.SetoresINEs.ToArray();

            var setores = db.AS_SetoresPar.ToArray();

            ViewBag.CBOs = profs;
            ViewBag.INEs = ines;
            ViewBag.CNESs = setores;
            ViewBag.Profissional = profiss.FirstOrDefault();
            ViewBag.OwnCBOs = profiss.Where(x => x.CBO != null).GroupBy(x => x.CBO).Select(x => x.Key.Trim()).ToArray();

            return View(pessoa?.FirstOrDefault()?.ASSMED_Cadastro);
        }
    }
}