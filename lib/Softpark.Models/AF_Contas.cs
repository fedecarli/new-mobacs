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
    
    public partial class AF_Contas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AF_Contas()
        {
            this.AF_ChequesEmi = new HashSet<AF_ChequesEmi>();
            this.AF_ChequesRec = new HashSet<AF_ChequesRec>();
            this.AF_ContasMensagens = new HashSet<AF_ContasMensagens>();
            this.AF_Lancamentos = new HashSet<AF_Lancamentos>();
        }
    
        public int NumContrato { get; set; }
        public int CodConta { get; set; }
        public string CodBanco { get; set; }
        public string CodAgencia { get; set; }
        public string NumContaCor { get; set; }
        public string DesConta { get; set; }
        public string TelContato { get; set; }
        public string NomeContato { get; set; }
        public string NumConvenio { get; set; }
        public string Carteira { get; set; }
        public string LocPag { get; set; }
        public string Cedente { get; set; }
        public string DigAgencia { get; set; }
        public string DigConta { get; set; }
        public string DigBanco { get; set; }
        public Nullable<int> CodPCon { get; set; }
        public Nullable<int> CodSetor { get; set; }
    
        public virtual AF_Bancos AF_Bancos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AF_ChequesEmi> AF_ChequesEmi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AF_ChequesRec> AF_ChequesRec { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AF_ContasMensagens> AF_ContasMensagens { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AF_Lancamentos> AF_Lancamentos { get; set; }
    }
}
