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
    
    public partial class SIGSM_Atendimento_Odontologico_Procedimentos
    {
        public long id { get; set; }
        public Nullable<long> id_atendimento_usuario { get; set; }
        public Nullable<long> id_atendimento { get; set; }
        public string descricao { get; set; }
        public Nullable<int> qtde { get; set; }
        public string sigtap { get; set; }
    
        public virtual SIGSM_Atendimento_Odontologico SIGSM_Atendimento_Odontologico { get; set; }
        public virtual SIGSM_Atendimento_Odontologico_Paciente SIGSM_Atendimento_Odontologico_Paciente { get; set; }
    }
}
