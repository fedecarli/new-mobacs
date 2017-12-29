namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SIGSM_ServicoSerializador_Agenda
    {
        public Guid Id { get; set; }

        public Guid IdTransmissao { get; set; }

        public bool Executando { get; set; }

        public DateTime? ExecutadoEm { get; set; }

        [Column(TypeName = "text")]
        public string LogMessage { get; set; }
    }
}
