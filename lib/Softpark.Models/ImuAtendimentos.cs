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
    
    public partial class ImuAtendimentos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ImuAtendimentos()
        {
            this.ImuAtendimentos_Doses = new HashSet<ImuAtendimentos_Doses>();
        }
    
        public int ID { get; set; }
        public Nullable<System.DateTime> DataHora { get; set; }
        public Nullable<int> ID_Setor { get; set; }
        public Nullable<int> ID_Campanha { get; set; }
        public Nullable<int> ID_Credenciado { get; set; }
        public string grupoAtendimento { get; set; }
        public string EstrategiaVacinacao { get; set; }
        public Nullable<int> ID_Paciente { get; set; }
        public Nullable<int> Gestante { get; set; }
        public Nullable<int> Hanseniase { get; set; }
        public string Diagnostico { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImuAtendimentos_Doses> ImuAtendimentos_Doses { get; set; }
        public virtual ImuCampanhas ImuCampanhas { get; set; }
    }
}
