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
    
    public partial class ASSMED_CadastroTitular
    {
        public int NumContrato { get; set; }
        public decimal Codigo { get; set; }
        public Nullable<decimal> CodTitular { get; set; }
        public Nullable<int> CodGrauPar { get; set; }
        public string MesmaMoradia { get; set; }
        public Nullable<int> CodUsu { get; set; }
        public Nullable<System.DateTime> DtSistema { get; set; }
        public string NumIP { get; set; }
    
        public virtual ASSMED_Cadastro ASSMED_Cadastro { get; set; }
        public virtual ASSMED_Parentesco ASSMED_Parentesco { get; set; }
    }
}
