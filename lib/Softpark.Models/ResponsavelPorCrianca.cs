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
    
    public partial class ResponsavelPorCrianca
    {
        public System.Guid id_informacoes_sociodemograficas { get; set; }
        public int id_tp_crianca { get; set; }
    
        public virtual InformacoesSocioDemograficas InformacoesSocioDemograficas { get; set; }
    }
}
