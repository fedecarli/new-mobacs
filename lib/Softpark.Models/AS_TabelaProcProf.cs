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
    
    public partial class AS_TabelaProcProf
    {
        public int NumContrato { get; set; }
        public decimal CodProc { get; set; }
        public int CodTab { get; set; }
        public decimal CodProfissao { get; set; }
        public int CodTabProfissao { get; set; }
        public System.DateTime DtAtual { get; set; }
        public Nullable<int> CodUsu { get; set; }
    
        public virtual AS_ProfissoesTab AS_ProfissoesTab { get; set; }
        public virtual AS_TabelaProc AS_TabelaProc { get; set; }
    }
}
