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
    
    public partial class AS_EXAMES_LOCAGEND
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AS_EXAMES_LOCAGEND()
        {
            this.Terceiros_tipoExames = new HashSet<Terceiros_tipoExames>();
        }
    
        public int IdLocAgend { get; set; }
        public string Descricao { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public string NomeCidade { get; set; }
        public string UF { get; set; }
        public string Numero { get; set; }
        public string Cep { get; set; }
        public Nullable<int> NumContrato { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Terceiros_tipoExames> Terceiros_tipoExames { get; set; }
    }
}
