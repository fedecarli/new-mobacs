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
    
    public partial class AS_AcomodacoesCarac
    {
        public int NumContrato { get; set; }
        public int CodAcomodacao { get; set; }
        public int CodCarLeito { get; set; }
        public Nullable<int> CodUsu { get; set; }
    
        public virtual AS_Acomodacoes AS_Acomodacoes { get; set; }
        public virtual AS_CaracLeito AS_CaracLeito { get; set; }
    }
}
