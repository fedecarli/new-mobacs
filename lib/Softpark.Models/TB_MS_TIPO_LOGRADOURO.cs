namespace Softpark.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("VW_TB_MS_TIPO_LOGRADOURO")]
    public partial class TB_MS_TIPO_LOGRADOURO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        
        [StringLength(3)]
        public string CO_TIPO_LOGRADOURO { get; set; }
        
        [Index(IsUnique = true)]
        [StringLength(100)]
        public string DS_TIPO_LOGRADOURO { get; set; }

        [StringLength(15)]
        public string DS_TIPO_LOGRADOURO_ABREV { get; set; }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_Endereco> ASSMED_Endereco { get; set; }
    }
}
