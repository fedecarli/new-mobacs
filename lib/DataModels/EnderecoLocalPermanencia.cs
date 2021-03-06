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
    
    public partial class EnderecoLocalPermanencia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EnderecoLocalPermanencia()
        {
            this.CadastroDomiciliar = new HashSet<CadastroDomiciliar>();
            this.ASSMED_Endereco = new HashSet<ASSMED_Endereco>();
        }
    
        public System.Guid id { get; set; }
        public string bairro { get; set; }
        public string cep { get; set; }
        public string codigoIbgeMunicipio { get; set; }
        public string complemento { get; set; }
        public string nomeLogradouro { get; set; }
        public string numero { get; set; }
        public string numeroDneUf { get; set; }
        public string telefoneContato { get; set; }
        public string telefoneResidencia { get; set; }
        public string tipoLogradouroNumeroDne { get; set; }
        public bool stSemNumero { get; set; }
        public string pontoReferencia { get; set; }
        public string microarea { get; set; }
        public bool stForaArea { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CadastroDomiciliar> CadastroDomiciliar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASSMED_Endereco> ASSMED_Endereco { get; set; }
    }
}
