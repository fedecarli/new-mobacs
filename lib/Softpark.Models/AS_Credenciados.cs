namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AS_Credenciados
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AS_Credenciados()
        {
            AS_CredenciadosUsu = new HashSet<AS_CredenciadosUsu>();
            AS_CredenciadosVinc = new HashSet<AS_CredenciadosVinc>();
            SIGSM_MicroArea_CredenciadoVinc = new HashSet<SIGSM_MicroArea_CredenciadoVinc>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumContrato { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodCred { get; set; }

        public decimal? Codigo { get; set; }

        [StringLength(7)]
        public string CNES { get; set; }

        [StringLength(10)]
        public string NumConselho { get; set; }

        public int? CodOrgao { get; set; }

        [StringLength(2)]
        public string UF { get; set; }

        [StringLength(5)]
        public string Tratamento { get; set; }

        public virtual UF UF1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AS_CredenciadosUsu> AS_CredenciadosUsu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AS_CredenciadosVinc> AS_CredenciadosVinc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_MicroArea_CredenciadoVinc> SIGSM_MicroArea_CredenciadoVinc { get; set; }
    }
}
