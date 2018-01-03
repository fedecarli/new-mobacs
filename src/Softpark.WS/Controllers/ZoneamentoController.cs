using DataTables.AspNet.WebApi2;
using Softpark.Models;
using Softpark.WS.ViewModels.SIGSM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Softpark.WS.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ZoneamentoController : BaseAjaxController
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        private List<ColumnDef> tblColumns => ColumnDef.From<TableList>();

        public class TableList
        {
            [ColumnDef(0, Title = "#")]
            public decimal Codigo { get; set; }

            [ColumnDef(1, Title = "Munícipe")]
            public string Nome { get; set; }

            [ColumnDef(2, Title = "Endereço")]
            public string Endereco { get; set; }

            [ColumnDef(3)]
            public string CEP { get; set; }

            [ColumnDef(4, Title = "Micro Área / Unidade", FnRender = "renderMaSelection")]
            public string MicroArea { get; set; }
        }

        public ZoneamentoController() : base(new DomainContainer()) { }

        /// GET: Zoneamento
        public async Task<ActionResult> Index()
        {
            ViewBag.MicroArea = (await Domain.SIGSM_MicroAreas.ToListAsync())
                .Select(x => new SelectListItem { Text = x.Codigo + " - " + x.Descricao, Value = x.Codigo });

            return View(tblColumns);
        }

        [HttpGet]
        public async Task<JsonResult> List([Bind(Include = "iDisplayStart,iDisplayLength,iSortingCols,iSortCol_0,sSortDir_0,sSearch,sEcho")] DataTableParameters request)
        {
            if (request == null) request = new DataTableParameters(Request.QueryString);

            var micros = new string[100];

            for (int i = 0; i < 99; i++)
                micros[i] = i.ToString().PadLeft(2, '0');

            var vincs = from zon in Domain.VW_Cadastros_Zoneamento
                        join cad in Domain.ASSMED_Cadastro
                        on zon.Codigo equals cad.Codigo
                        let dom = cad.ASSMED_Endereco.FirstOrDefault(x => x.ItemEnd == zon.ItemEnd)
                        select new TableList
                        {
                            Codigo = cad.Codigo,
                            Nome = cad.Nome,
                            Endereco = dom == null ? "" : (
                                (dom.TipoEnd == null ? "" : (dom.TipoEnd + " ")) + (dom.Logradouro == null ? "" : dom.Logradouro) +
                            (dom.Numero == null || 0 == dom.Numero.Trim().Length ? "" : (", " + dom.Numero)) +
                            (dom.Complemento == null || 0 == dom.Complemento.Trim().Length ? "" : (", " + dom.Complemento)) +
                            (dom.Bairro == null || 0 == dom.Bairro.Trim().Length ? "" : (", " + dom.Bairro))).Trim(),
                            CEP = dom == null ? "" : dom.CEP == null ? "" : dom.CEP.Replace(".", "").Replace("-", ""),
                            MicroArea = zon.MicroArea == null || !micros.Contains(zon.MicroArea) ? "" : zon.MicroArea
                        };
            
            Expression<Func<TableList, object>> sort;
            if (request.iSortCol_0 < 0 || request.iSortCol_0 > tblColumns.Max(x => x.Position))
                request.iSortCol_0 = 0;

            var col = tblColumns[request.iSortCol_0].Name;

            if (col == "Nome")
                sort = ((a) => a.Nome);
            else if (col == "Endereco")
                sort = ((a) => a.Endereco);
            else if (col == "CEP")
                sort = ((a) => a.CEP);
            else if (col == "MicroArea")
                sort = ((a) => (a.MicroArea == null ? "999" : ("0" + a.MicroArea)));
            else
                sort = ((a) => a.Codigo);

            var comp = request.Compose(vincs, sort,
                x => x.Codigo.ToString() == request.sSearch.Trim() ||
                x.Nome.Contains(request.sSearch.Trim()) ||
                x.Endereco.Contains(request.sSearch.Trim()) ||
                x.CEP.Equals(request.sSearch.Trim().Replace(".", "").Replace("-", "")) ||
                x.MicroArea.Equals(request.sSearch.Trim()));
            
            return Json(await comp.Result(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Required(ErrorMessage = "Nenhum zoneamento alterado."), Bind(Include = "zoneamentos")] ZonasViewModel zonas)
        {
            if (!ModelState.IsValid)
                return Json(ModelState.ToDictionary(x => x.Key, x => new { Errors = x.Value.Errors.Select(y => y.ErrorMessage).ToArray() }));

            var zoneamentos = zonas.zoneamentos;

            var cods = zoneamentos.Select(x => x.Codigo);

            var cads = await (from zon in Domain.VW_Cadastros_Zoneamento
                              join cad in Domain.ASSMED_Cadastro
                              on zon.Codigo equals cad.Codigo
                              where cods.Contains(cad.Codigo)
                              select cad).ToListAsync();

            foreach (var cad in cads)
            {
                var zona = zoneamentos.Single(x => x.Codigo == cad.Codigo);
                cad.MicroArea = zona.MicroArea;
                var dom = cad.ASSMED_Endereco.OrderByDescending(x => x.ItemEnd).FirstOrDefault();
                if (dom != null)
                {
                    dom.MicroArea = zona.MicroArea;
                }
            }

            await Domain.SaveChangesAsync();

            return Json(true);
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}