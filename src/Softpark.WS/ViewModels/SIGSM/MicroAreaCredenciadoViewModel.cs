using DataTables.AspNet.WebApi2;
using System.ComponentModel.DataAnnotations;

namespace Softpark.WS.ViewModels.SIGSM
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ListagemMicroAreaCredenciadoViewModel
    {
        [ColumnDef(0, Sorting = SortingDirections.Ascending)]
        public string Unidade { get; set; }

        [ColumnDef(1, Title = "Profissional")]
        public string Nome { get; set; }

        [ColumnDef(2, Title = "Micro Área")]
        public string MicroArea { get; set; }

        [ColumnDef(3, Title = "", Sortable = false)]
        public string btn { get; set; }
    }
    
    public class CadastroMicroAreaCredenciadoViewModel
    {
        public int? id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Selecione um Credenciado"),
            RegularExpression("^([1-9]|[1-9][0-9]+)([:])([1-9]|[1-9][0-9]+)$", ErrorMessage = "Selecione um Credenciado")]
        public string ItemVinc { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Selecione uma Micro Área Associada"),
            RegularExpression("^([1-9]|[1-9][0-9]+)$", ErrorMessage = "Selecione uma Micro Área Associada")]
        public int idMicroAreaUnidade { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}