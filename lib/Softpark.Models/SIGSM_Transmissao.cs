namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SIGSM_Transmissao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SIGSM_Transmissao()
        {
            SIGSM_Transmissao_Processos = new HashSet<SIGSM_Transmissao_Processos>();
        }

        public Guid Id { get; set; }

        public DateTime DataSolicitacao { get; set; }

        public int CodUsu { get; set; }

        public DateTime DataLote { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_Transmissao_Processos> SIGSM_Transmissao_Processos { get; set; }
    }
}
