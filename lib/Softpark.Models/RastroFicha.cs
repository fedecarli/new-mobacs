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
    
    public partial class RastroFicha
    {
        public System.Guid token { get; set; }
        public int CodUsu { get; set; }
        public System.DateTime DataModificacao { get; set; }
        public string DadoAnterior { get; set; }
        public string DadoAtual { get; set; }
    
        public virtual OrigemVisita OrigemVisita { get; set; }
    }
}
