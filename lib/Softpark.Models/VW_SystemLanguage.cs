namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_SystemLanguage
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short langid { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(3)]
        public string dateformat { get; set; }

        [Key]
        [Column(Order = 2)]
        public byte datefirst { get; set; }

        public int? upgrade { get; set; }

        [Key]
        [Column(Order = 3)]
        public string name { get; set; }

        [Key]
        [Column(Order = 4)]
        public string alias { get; set; }

        [StringLength(372)]
        public string months { get; set; }

        [StringLength(132)]
        public string shortmonths { get; set; }

        [StringLength(217)]
        public string days { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int lcid { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short msglangid { get; set; }
    }
}
