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
    
    public partial class AS_TabelaProcAud
    {
        public int NumContrato { get; set; }
        public decimal CodProc { get; set; }
        public int CodTab { get; set; }
        public System.DateTime DtAtual { get; set; }
        public string Sexo { get; set; }
        public Nullable<int> IdadeIni { get; set; }
        public Nullable<int> IdadeFim { get; set; }
        public Nullable<int> Validade { get; set; }
    
        public virtual AS_TabelaProc AS_TabelaProc { get; set; }
    }
}
