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
    
    public partial class ASSMED_CadastroQuestRespM
    {
        public int NumContrato { get; set; }
        public decimal Codigo { get; set; }
        public int ItemCadQuest { get; set; }
        public int ItemCadQuestResp { get; set; }
        public int ItemResposta { get; set; }
    
        public virtual ASSMED_CadastroQuestResp ASSMED_CadastroQuestResp { get; set; }
    }
}
