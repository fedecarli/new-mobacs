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
    
    public partial class CadastroDomiciliar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CadastroDomiciliar()
        {
            this.AnimalNoDomicilio = new HashSet<AnimalNoDomicilio>();
            this.FamiliaRow = new HashSet<FamiliaRow>();
        }
    
        public long idAuto { get; set; }
        public System.Guid id { get; set; }
        public Nullable<System.Guid> condicaoMoradia { get; set; }
        public Nullable<System.Guid> enderecoLocalPermanencia { get; set; }
        public bool fichaAtualizada { get; set; }
        public Nullable<int> quantosAnimaisNoDomicilio { get; set; }
        public bool stAnimaisNoDomicilio { get; set; }
        public bool statusTermoRecusa { get; set; }
        public int tpCdsOrigem { get; set; }
        public Nullable<System.Guid> uuidFichaOriginadora { get; set; }
        public int tipoDeImovel { get; set; }
        public Nullable<System.Guid> instituicaoPermanencia { get; set; }
        public System.Guid headerTransport { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public Nullable<bool> Erro { get; set; }
        public Nullable<System.DateTime> DataRegistro { get; set; }
        public string Justificativa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnimalNoDomicilio> AnimalNoDomicilio { get; set; }
        public virtual CondicaoMoradia CondicaoMoradia1 { get; set; }
        public virtual EnderecoLocalPermanencia EnderecoLocalPermanencia1 { get; set; }
        public virtual InstituicaoPermanencia InstituicaoPermanencia1 { get; set; }
        public virtual TP_Imovel TP_Imovel { get; set; }
        public virtual UnicaLotacaoTransport UnicaLotacaoTransport { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FamiliaRow> FamiliaRow { get; set; }
    }
}
