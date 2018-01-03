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
    
    public partial class IdentificacaoUsuarioCidadao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IdentificacaoUsuarioCidadao()
        {
            this.CadastroIndividual = new HashSet<CadastroIndividual>();
            this.ASSMED_Cadastro = new HashSet<ASSMED_Cadastro>();
            this.Documentos = new HashSet<Documentos>();
        }
    
        public System.Guid id { get; set; }
        public string nomeSocial { get; set; }
        public string codigoIbgeMunicipioNascimento { get; set; }
        public bool desconheceNomeMae { get; set; }
        public string emailCidadao { get; set; }
        public int nacionalidadeCidadao { get; set; }
        public string nomeCidadao { get; set; }
        public string nomeMaeCidadao { get; set; }
        public string cnsCidadao { get; set; }
        public string cnsResponsavelFamiliar { get; set; }
        public string telefoneCelular { get; set; }
        public string numeroNisPisPasep { get; set; }
        public Nullable<int> paisNascimento { get; set; }
        public int racaCorCidadao { get; set; }
        public int sexoCidadao { get; set; }
        public bool statusEhResponsavel { get; set; }
        public Nullable<int> etnia { get; set; }
        public Nullable<int> num_contrato { get; set; }
        public string nomePaiCidadao { get; set; }
        public bool desconheceNomePai { get; set; }
        public string portariaNaturalizacao { get; set; }
        public string microarea { get; set; }
        public bool stForaArea { get; set; }
        public Nullable<System.DateTime> dataNascimentoCidadao { get; set; }
        public Nullable<System.DateTime> dtNaturalizacao { get; set; }
        public Nullable<System.DateTime> dtEntradaBrasil { get; set; }
        public string RG { get; set; }
        public string CPF { get; set; }
        public Nullable<bool> beneficiarioBolsaFamilia { get; set; }
        public string EstadoCivil { get; set; }
        public string ComplementoRG { get; set; }
        public Nullable<decimal> Codigo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CadastroIndividual> CadastroIndividual { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_Cadastro> ASSMED_Cadastro { get; set; }
        public virtual ASSMED_Cadastro ASSMED_Cadastro1 { get; set; }
        public virtual Etnia Etnia1 { get; set; }
        public virtual TP_Raca_Cor TP_Raca_Cor { get; set; }
        public virtual TP_Sexo TP_Sexo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documentos> Documentos { get; set; }
    }
}