namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ASSMED_Usuario
    {
        public int CodUsu { get; set; }

        [StringLength(80)]
        public string Login { get; set; }

        [StringLength(100)]
        public string Nome { get; set; }

        [StringLength(80)]
        public string Senha { get; set; }

        public DateTime? DtSistema { get; set; }

        [Key]
        [StringLength(80)]
        public string Email { get; set; }

        [StringLength(60)]
        public string NumIP { get; set; }

        [StringLength(10)]
        public string CEP { get; set; }

        [StringLength(80)]
        public string NomeCidade { get; set; }

        [StringLength(2)]
        public string UF { get; set; }

        [StringLength(8)]
        public string NumLog { get; set; }

        public int? Ativo { get; set; }

        [StringLength(10)]
        public string tipo_end { get; set; }

        [StringLength(250)]
        public string logradouro { get; set; }

        [StringLength(10)]
        public string numero { get; set; }

        [StringLength(250)]
        public string bairro { get; set; }

        [StringLength(250)]
        public string municipio { get; set; }

        [StringLength(100)]
        public string complemento { get; set; }

        [StringLength(16)]
        public string fone { get; set; }

        public int? id_user_cadastro { get; set; }

        public DateTime? data_cadastro { get; set; }

        public int? id_user_alteracao { get; set; }

        public DateTime? data_alteracao { get; set; }

        [StringLength(14)]
        public string cpf { get; set; }
    }
}
