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
    
    public partial class Domicilio_DCardiaco
    {
        public int id_domicilio_dcardiaco { get; set; }
        public Nullable<int> id_domicilio_individual { get; set; }
        public Nullable<int> tp_Doenca_Cardiaca { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<int> ativo { get; set; }
        public Nullable<int> id_user_cadastro { get; set; }
        public Nullable<int> id_unidade_cadastro { get; set; }
        public Nullable<System.DateTime> data_cadastro { get; set; }
        public Nullable<int> id_user_alteracao { get; set; }
        public Nullable<int> id_unidade_alteracao { get; set; }
        public Nullable<System.DateTime> data_alteracao { get; set; }
    }
}
