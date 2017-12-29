namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TP_Sexo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TP_Sexo()
        {
            IdentificacaoUsuarioCidadao = new HashSet<IdentificacaoUsuarioCidadao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int codigo { get; set; }

        [Required]
        [StringLength(60)]
        public string descricao { get; set; }

        [StringLength(512)]
        public string observacao { get; set; }

        [StringLength(1)]
        public string sigla { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IdentificacaoUsuarioCidadao> IdentificacaoUsuarioCidadao { get; set; }
    }
}
