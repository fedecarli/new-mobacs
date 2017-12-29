namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.Documentos")]
    public partial class Documentos
    {
        public Guid Id { get; set; }

        public Guid IdIdentificacaoUsuarioCidadao { get; set; }

        public int NumContrato { get; set; }

        public int IdTipoDocumento { get; set; }

        [Required]
        [StringLength(60)]
        public string TipoArquivo { get; set; }

        public long Tamanho { get; set; }

        public DateTime Data { get; set; }

        public byte[] Arquivo { get; set; }

        public virtual IdentificacaoUsuarioCidadao IdentificacaoUsuarioCidadao { get; set; }

        public virtual ASSMED_TipoDocPessoal ASSMED_TipoDocPessoal { get; set; }
    }
}
