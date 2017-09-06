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
    
    public partial class ASSMED_Cadastro
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ASSMED_Cadastro()
        {
            this.IdentificacaoUsuarioCidadao1 = new HashSet<IdentificacaoUsuarioCidadao>();
            this.ASSMED_CadastroDocPessoal = new HashSet<ASSMED_CadastroDocPessoal>();
            this.ASSMED_CadEmails = new HashSet<ASSMED_CadEmails>();
            this.ASSMED_CadTelefones = new HashSet<ASSMED_CadTelefones>();
            this.ASSMED_Endereco = new HashSet<ASSMED_Endereco>();
        }
    
        public int NumContrato { get; set; }
        public decimal Codigo { get; set; }
        public string Tipo { get; set; }
        public string Nome { get; set; }
        public Nullable<System.DateTime> DtSistema { get; set; }
        public Nullable<int> CodUsu { get; set; }
        public string NumIP { get; set; }
        public string NomeSocial { get; set; }
        public Nullable<int> CodMunicipe { get; set; }
        public Nullable<System.DateTime> DtAtualizacao { get; set; }
        public Nullable<int> CodTpHomologacao { get; set; }
        public string Justificativa { get; set; }
        public string MotivoHomologacao { get; set; }
        public Nullable<System.Guid> IdFicha { get; set; }
    
        public virtual IdentificacaoUsuarioCidadao IdentificacaoUsuarioCidadao { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IdentificacaoUsuarioCidadao> IdentificacaoUsuarioCidadao1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_CadastroDocPessoal> ASSMED_CadastroDocPessoal { get; set; }
        public virtual ASSMED_PesFisica ASSMED_PesFisica { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_CadEmails> ASSMED_CadEmails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_CadTelefones> ASSMED_CadTelefones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_Endereco> ASSMED_Endereco { get; set; }
    }
}
