
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
    public partial class SaidaCidadaoCadastro
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public SaidaCidadaoCadastro()
    {

        this.CadastroIndividual = new HashSet<CadastroIndividual>();

    }


    public System.Guid id { get; set; }

    public Nullable<int> motivoSaidaCidadao { get; set; }

    public string numeroDO { get; set; }

    public Nullable<System.DateTime> dataObito { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<CadastroIndividual> CadastroIndividual { get; set; }

    public virtual TP_Motivo_Saida TP_Motivo_Saida { get; set; }

}

}
