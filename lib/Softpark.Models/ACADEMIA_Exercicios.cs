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
    
    public partial class ACADEMIA_Exercicios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ACADEMIA_Exercicios()
        {
            this.ACADEMIA_FichaTreinoExer = new HashSet<ACADEMIA_FichaTreinoExer>();
        }
    
        public int NumContrato { get; set; }
        public int CodExerc { get; set; }
        public string DesExerc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ACADEMIA_FichaTreinoExer> ACADEMIA_FichaTreinoExer { get; set; }
    }
}
