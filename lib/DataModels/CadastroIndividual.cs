//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CadastroIndividual
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CadastroIndividual()
        {
            this.CadastroIndividual_recusa = new HashSet<CadastroIndividual_recusa>();
        }
    
        public long idAuto { get; set; }
        public System.Guid id { get; set; }
        public Nullable<System.Guid> condicoesDeSaude { get; set; }
        public Nullable<System.Guid> emSituacaoDeRua { get; set; }
        public bool fichaAtualizada { get; set; }
        public Nullable<System.Guid> identificacaoUsuarioCidadao { get; set; }
        public Nullable<System.Guid> informacoesSocioDemograficas { get; set; }
        public bool statusTermoRecusaCadastroIndividualAtencaoBasica { get; set; }
        public int tpCdsOrigem { get; set; }
        public Nullable<System.Guid> uuidFichaOriginadora { get; set; }
        public Nullable<System.Guid> saidaCidadaoCadastro { get; set; }
        public System.Guid headerTransport { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public Nullable<bool> Erro { get; set; }
        public Nullable<System.DateTime> DataRegistro { get; set; }
        public string Justificativa { get; set; }
    
        public virtual CondicoesDeSaude CondicoesDeSaude1 { get; set; }
        public virtual EmSituacaoDeRua EmSituacaoDeRua1 { get; set; }
        public virtual IdentificacaoUsuarioCidadao IdentificacaoUsuarioCidadao1 { get; set; }
        public virtual InformacoesSocioDemograficas InformacoesSocioDemograficas1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CadastroIndividual_recusa> CadastroIndividual_recusa { get; set; }
        public virtual SaidaCidadaoCadastro SaidaCidadaoCadastro1 { get; set; }
        public virtual UnicaLotacaoTransport UnicaLotacaoTransport { get; set; }
    }
}
