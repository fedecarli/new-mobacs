
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
    
public partial class AS_Profissoes
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public AS_Profissoes()
    {

        this.AS_ProfissoesTab = new HashSet<AS_ProfissoesTab>();

    }


    public int NumContrato { get; set; }

    public decimal CodProfissao { get; set; }

    public string DesProfissao { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<AS_ProfissoesTab> AS_ProfissoesTab { get; set; }

}

}
