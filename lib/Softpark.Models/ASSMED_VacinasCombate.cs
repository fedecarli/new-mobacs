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
    
    public partial class ASSMED_VacinasCombate
    {
        public int NumContrato { get; set; }
        public int CodVacina { get; set; }
        public int CodDoenca { get; set; }
        public Nullable<int> CodUsu { get; set; }
        public Nullable<System.DateTime> DtSistema { get; set; }
        public string NumIP { get; set; }
    
        public virtual ASSMED_DOENCAS ASSMED_DOENCAS { get; set; }
        public virtual ASSMED_Vacinas ASSMED_Vacinas { get; set; }
    }
}
