namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("api.CadastroIndividual")]
    public partial class CadastroIndividual
    {
        public Guid id { get; set; }

        public Guid? condicoesDeSaude { get; set; }

        public Guid? emSituacaoDeRua { get; set; }

        public bool fichaAtualizada { get; set; }

        public Guid? identificacaoUsuarioCidadao { get; set; }

        public Guid? informacoesSocioDemograficas { get; set; }

        public bool statusTermoRecusaCadastroIndividualAtencaoBasica { get; set; }

        public int tpCdsOrigem { get; set; }

        public Guid? uuidFichaOriginadora { get; set; }

        public Guid? saidaCidadaoCadastro { get; set; }

        public Guid headerTransport { get; set; }

        [StringLength(20)]
        public string latitude { get; set; }

        [StringLength(20)]
        public string longitude { get; set; }

        public DateTime? DataRegistro { get; set; }

        public virtual CondicoesDeSaude CondicoesDeSaude1 { get; set; }

        public virtual EmSituacaoDeRua EmSituacaoDeRua1 { get; set; }

        public virtual IdentificacaoUsuarioCidadao IdentificacaoUsuarioCidadao1 { get; set; }

        public virtual InformacoesSocioDemograficas InformacoesSocioDemograficas1 { get; set; }

        public virtual SaidaCidadaoCadastro SaidaCidadaoCadastro1 { get; set; }

        public virtual UnicaLotacaoTransport UnicaLotacaoTransport { get; set; }
    }
}
