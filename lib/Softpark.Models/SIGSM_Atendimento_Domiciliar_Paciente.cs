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
    
    public partial class SIGSM_Atendimento_Domiciliar_Paciente
    {
        public long id { get; set; }
        public Nullable<int> id_tipo_atendimento { get; set; }
        public Nullable<System.DateTime> data_nascimento { get; set; }
        public decimal id_identificacao_usuario { get; set; }
        public Nullable<int> NumContrato { get; set; }
        public Nullable<int> local_atendimento { get; set; }
        public string turno { get; set; }
        public Nullable<long> id_atendimento_domiciliar { get; set; }
        public string numero_cartao_sus { get; set; }
        public string modalidade_ad { get; set; }
        public string condicao { get; set; }
        public string procedimentos { get; set; }
        public string cid { get; set; }
        public string ciap { get; set; }
        public Nullable<int> conduta_motivo_saida { get; set; }
        public Nullable<bool> acompanhamento_pos_obito { get; set; }
        public Nullable<int> numero { get; set; }
        public string sexo { get; set; }
        public string nome { get; set; }
    
        public virtual ASSMED_Cadastro ASSMED_Cadastro { get; set; }
    }
}
