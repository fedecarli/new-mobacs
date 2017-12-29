namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AS_CredenciadosVinc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AS_CredenciadosVinc()
        {
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

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemVinc { get; set; }

        public int? CodRegime { get; set; }

        public int? CodTabProfissao { get; set; }

        public int? CodSetor { get; set; }

        [StringLength(7)]
        public string CNESLocal { get; set; }

        [StringLength(15)]
        public string Matricula { get; set; }

        [StringLength(10)]
        public string CodProfTab { get; set; }

        public DateTime? DtInicio { get; set; }

        public DateTime? DtFinal { get; set; }

        public int? CodUsu { get; set; }

        public int? CodConv { get; set; }

        public int? CodINE { get; set; }

        public virtual AS_Credenciados AS_Credenciados { get; set; }

        public virtual Setores Setores { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_MicroArea_CredenciadoVinc> SIGSM_MicroArea_CredenciadoVinc { get; set; }
    }
}
