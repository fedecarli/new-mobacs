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
    
    public partial class VW_ATENDIMENTO_INDIVIDUAL
    {
        public long PK { get; set; }
        public long ficha_ID { get; set; }
        public string DATA { get; set; }
        public Nullable<decimal> Profissional_ID { get; set; }
        public string Profissional_nome { get; set; }
        public Nullable<int> Digitador_ID { get; set; }
        public string Digitador_nome { get; set; }
        public Nullable<int> Status_ID { get; set; }
        public string Status_Nome { get; set; }
        public Nullable<int> Unidade_CNES { get; set; }
        public Nullable<int> Unidade_ID { get; set; }
        public string GUID_ESUS { get; set; }
    }
}
