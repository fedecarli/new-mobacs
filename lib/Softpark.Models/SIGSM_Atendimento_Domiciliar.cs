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
    
    public partial class SIGSM_Atendimento_Domiciliar
    {
        public long id { get; set; }
        public int digitado_por { get; set; }
        public System.DateTime data_entrada { get; set; }
        public Nullable<int> conferido_por { get; set; }
        public long numero_folha { get; set; }
        public Nullable<int> codigo_cnes_unidade { get; set; }
        public Nullable<int> codigo_equipe_ine { get; set; }
        public System.DateTime data_atendimento { get; set; }
        public Nullable<int> id_status { get; set; }
        public Nullable<int> num_contrato { get; set; }
        public bool enviado { get; set; }
        public Nullable<System.DateTime> data_envio { get; set; }
        public Nullable<int> usuario_envio { get; set; }
        public string guid_esus { get; set; }
    
        public virtual SIGSM_Status SIGSM_Status { get; set; }
    }
}
