namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ASSMED_TipoDocPessoal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ASSMED_TipoDocPessoal()
        {
            Documentos = new HashSet<Documentos>();
            ASSMED_CadastroDocPessoal = new HashSet<ASSMED_CadastroDocPessoal>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumContrato { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodTpDocP { get; set; }

        [StringLength(50)]
        public string DesTpDocP { get; set; }

        [StringLength(1)]
        public string Numero { get; set; }

        [StringLength(1)]
        public string Serie { get; set; }

        [StringLength(1)]
        public string Secao { get; set; }

        [StringLength(1)]
        public string Zona { get; set; }

        [StringLength(1)]
        public string DtValidade { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documentos> Documentos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_CadastroDocPessoal> ASSMED_CadastroDocPessoal { get; set; }
    }
}
