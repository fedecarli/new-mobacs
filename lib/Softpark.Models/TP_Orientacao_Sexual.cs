
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public partial class TP_Orientacao_Sexual
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public TP_Orientacao_Sexual()
    {

        this.InformacoesSocioDemograficas = new HashSet<InformacoesSocioDemograficas>();

    }


    public int codigo { get; set; }

    public string descricao { get; set; }

    public string observacoes { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<InformacoesSocioDemograficas> InformacoesSocioDemograficas { get; set; }

}

}
