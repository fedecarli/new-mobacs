using Softpark.WS.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Softpark.WS.ViewModels.SIGSM
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ZonasViewModel
    {
        [Required]
        public ICollection<ZoneamentoViewModel> zoneamentos { get; set; }
    }

    public class ZoneamentoViewModel
    {
        [Required(ErrorMessage = "Informe uma Micro Área válida."), MicroAreaValidation(ErrorMessage = "A Micro Área '{0}' é inválida.")]
        public string MicroArea { get; set; }

        [Required(ErrorMessage = "Informe um Cadastro Não Zoneado."), ZoneamentoValidation(ErrorMessage = "Este cadastro não pode ser zoneado.")]
        public decimal Codigo { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}