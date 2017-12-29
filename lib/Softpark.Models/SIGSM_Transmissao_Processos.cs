namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SIGSM_Transmissao_Processos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SIGSM_Transmissao_Processos()
        {
            SIGSM_Transmissao_Processos_Log = new HashSet<SIGSM_Transmissao_Processos_Log>();
        }

        public Guid Id { get; set; }

        public Guid IdTransmissao { get; set; }

        public int IdStatusGeracao { get; set; }

        public DateTime? InicioDoProcesso { get; set; }

        public DateTime? FimDoProcesso { get; set; }

        [StringLength(255)]
        public string ArquivoGerado { get; set; }

        [StringLength(100)]
        public string TipoFicha { get; set; }

        [Required]
        [StringLength(10)]
        public string SerializarEm { get; set; }

        [Required]
        [StringLength(7)]
        public string CNES { get; set; }

        public virtual SIGSM_Transmissao SIGSM_Transmissao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_Transmissao_Processos_Log> SIGSM_Transmissao_Processos_Log { get; set; }

        public virtual SIGSM_Transmissao_StatusGeracao SIGSM_Transmissao_StatusGeracao { get; set; }
    }
}
