namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ASSMED_Contratos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ASSMED_Contratos()
        {
            SIGSM_MicroArea_Unidade = new HashSet<SIGSM_MicroArea_Unidade>();
            SIGSM_MicroArea_CredenciadoVinc = new HashSet<SIGSM_MicroArea_CredenciadoVinc>();
            SIGSM_MicroArea_CredenciadoCidadao = new HashSet<SIGSM_MicroArea_CredenciadoCidadao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumContrato { get; set; }

        public DateTime? DtInicio { get; set; }

        [StringLength(80)]
        public string NomeContratante { get; set; }

        [StringLength(1)]
        public string AuditaEntrada { get; set; }

        [StringLength(1)]
        public string AuditaProg { get; set; }

        [StringLength(20)]
        public string DesMarca { get; set; }

        [StringLength(20)]
        public string DesCateg { get; set; }

        [StringLength(1)]
        public string CtrRef { get; set; }

        public int? CodCustoComisao { get; set; }

        public int? CodCustoICMS { get; set; }

        public int? CodCustoIPI { get; set; }

        public int? CodTabSai { get; set; }

        [StringLength(60)]
        public string Pasta { get; set; }

        [StringLength(60)]
        public string Logo { get; set; }

        [StringLength(20)]
        public string PastaCli { get; set; }

        [StringLength(20)]
        public string PastaImagens { get; set; }

        [StringLength(7)]
        public string CodigoIbgeMunicipio { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_MicroArea_Unidade> SIGSM_MicroArea_Unidade { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_MicroArea_CredenciadoVinc> SIGSM_MicroArea_CredenciadoVinc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_MicroArea_CredenciadoCidadao> SIGSM_MicroArea_CredenciadoCidadao { get; set; }
    }
}
