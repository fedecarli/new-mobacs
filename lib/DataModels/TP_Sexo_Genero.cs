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
    
    public partial class TP_Sexo_Genero
    {
        public int id_tp_sexo_genero { get; set; }
        public string descricao { get; set; }
        public Nullable<int> id_user_cadastro { get; set; }
        public Nullable<int> ativo { get; set; }
        public Nullable<System.DateTime> data_cadastro { get; set; }
        public Nullable<int> id_user_alteracao { get; set; }
        public Nullable<System.DateTime> data_alteracao { get; set; }
    }
}
