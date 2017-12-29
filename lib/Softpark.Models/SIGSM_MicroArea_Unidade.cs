namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SIGSM_MicroArea_Unidade
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SIGSM_MicroArea_Unidade()
        {
            SIGSM_MicroArea_CredenciadoVinc = new HashSet<SIGSM_MicroArea_CredenciadoVinc>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(2)]
        public string MicroArea { get; set; }

        public int NumContrato { get; set; }

        public int CodSetor { get; set; }

        public virtual ASSMED_Contratos ASSMED_Contratos { get; set; }

        public virtual Setores Setores { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_MicroArea_CredenciadoVinc> SIGSM_MicroArea_CredenciadoVinc { get; set; }

        public virtual SIGSM_MicroAreas SIGSM_MicroAreas { get; set; }
    }
}
