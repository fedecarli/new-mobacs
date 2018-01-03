using DataTables.AspNet.WebApi2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Softpark.WS.ViewModels
{
    class ListagemAssociacaoViewModel
    {
        [ColumnDef(0, Title = "Micro Área", Sortable = false)]
        public string MicroArea { get; set; }

        [ColumnDef(1, Title = "Munícipe", Sortable = false)]
        public string Municipe { get; set; }

        [ColumnDef(2, Title = "Endereço", Sortable = false)]
        public string Endereco { get; set; }

        [ColumnDef(3, Sortable = false, Title = "", FnRender = "renderVincSelection")]
        public string check { get; set; }
    }

    class ListagemDownloadViewModel
    {
        [ColumnDef(0, Title = "Micro Área", Sortable = false)]
        public string MicroArea { get; set; }

        [ColumnDef(1, Title = "Munícipe", Sortable = false)]
        public string Municipe { get; set; }

        [ColumnDef(2, Title = "Endereço", Sortable = false)]
        public string Endereco { get; set; }

        [ColumnDef(3, Sortable = false, Title = "", FnRender = "renderDownSelection")]
        public string check { get; set; }
    }

    public class AssociacaoViewModel
    {
        public decimal Codigo { get; set; }
        public bool Relacionar { get; set; }
    }

    public class AssociacoesList
    {
        public AssociacaoViewModel[] associacoes { get; set; }
    }

    public class DownloadViewModel
    {
        public int Vinculo { get; set; }
        public bool Baixar { get; set; }
    }

    public class DownloadsList
    {
        public DownloadViewModel[] downloads { get; set; }
    }
}