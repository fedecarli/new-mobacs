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
    
    public partial class TP_Acesso_Domicilio
    {
        public int id_tp_acesso_domicilio { get; set; }
        public string descricao { get; set; }
        public Nullable<int> id_user_cadastro { get; set; }
        public Nullable<int> ativo { get; set; }
        public Nullable<System.DateTime> data_cadastro { get; set; }
        public Nullable<int> id_user_alteracao { get; set; }
        public Nullable<System.DateTime> data_alteracao { get; set; }
        public int codigo { get; set; }
    }
}
