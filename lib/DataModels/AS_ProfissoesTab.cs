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
    
    public partial class AS_ProfissoesTab
    {
        public int NumContrato { get; set; }
        public decimal CodProfissao { get; set; }
        public int CodTabProfissao { get; set; }
        public string CodProfTab { get; set; }
        public string DesProfTab { get; set; }
    
        public virtual AS_Profissoes AS_Profissoes { get; set; }
    }
}
