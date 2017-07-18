
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    using System;
    using System.Collections.Generic;
    
public partial class FichaVisitaDomiciliarChild
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public FichaVisitaDomiciliarChild()
    {

        this.SIGSM_MotivoVisita = new HashSet<SIGSM_MotivoVisita>();

    }


    public string uuidFicha { get; set; }

    public long turno { get; set; }

    public string numProntuario { get; set; }

    public string cnsCidadao { get; set; }

    public Nullable<long> sexo { get; set; }

    public bool statusVisitaCompartilhadaOutroProfissional { get; set; }

    public long desfecho { get; set; }

    public string microarea { get; set; }

    public bool stForaArea { get; set; }

    public long tipoDeImovel { get; set; }

    public Nullable<decimal> pesoAcompanhamentoNutricional { get; set; }

    public Nullable<decimal> alturaAcompanhamentoNutricional { get; set; }

    public string latitude { get; set; }

    public string longitude { get; set; }

    public System.Guid childId { get; set; }

    public Nullable<System.DateTime> dtNascimento { get; set; }

    public string Justificativa { get; set; }

    public Nullable<System.DateTime> DataRegistro { get; set; }



    public virtual FichaVisitaDomiciliarMaster FichaVisitaDomiciliarMaster { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<SIGSM_MotivoVisita> SIGSM_MotivoVisita { get; set; }

}

}
