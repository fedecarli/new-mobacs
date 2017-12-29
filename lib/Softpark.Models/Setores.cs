namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Setores
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Setores()
        {
            AS_CredenciadosVinc = new HashSet<AS_CredenciadosVinc>();
            AS_SetoresPar = new HashSet<AS_SetoresPar>();
            SIGSM_MicroArea_Unidade = new HashSet<SIGSM_MicroArea_Unidade>();
            SetoresINEs = new HashSet<SetoresINEs>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodSetor { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumContrato { get; set; }

        [StringLength(60)]
        public string DesSetor { get; set; }

        public decimal? Codigo { get; set; }

        [StringLength(20)]
        public string Latitude { get; set; }

        [StringLength(20)]
        public string Longitude { get; set; }

        public bool almoxarifado { get; set; }

        [StringLength(100)]
        public string DesSetorRes { get; set; }

        public int? blindado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AS_CredenciadosVinc> AS_CredenciadosVinc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AS_SetoresPar> AS_SetoresPar { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_MicroArea_Unidade> SIGSM_MicroArea_Unidade { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SetoresINEs> SetoresINEs { get; set; }
    }
}
