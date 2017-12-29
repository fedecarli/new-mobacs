namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SIGSM_Transmissao_Processos_Log
    {
        public Guid Id { get; set; }

        public Guid IdProcesso { get; set; }

        [Required]
        public string Erro { get; set; }

        public DateTime DataLog { get; set; }

        public virtual SIGSM_Transmissao_Processos SIGSM_Transmissao_Processos { get; set; }
    }
}
