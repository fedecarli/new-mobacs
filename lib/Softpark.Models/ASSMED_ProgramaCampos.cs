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
    
    public partial class ASSMED_ProgramaCampos
    {
        public string NomProg { get; set; }
        public string NomeCampo { get; set; }
        public string Chave { get; set; }
        public string ChavePai { get; set; }
        public string Aparece { get; set; }
        public string CampoPes { get; set; }
        public Nullable<int> Linha { get; set; }
        public Nullable<int> Ordem { get; set; }
        public Nullable<int> TamNoForm { get; set; }
        public string TituloCampo { get; set; }
        public string TipoCampo { get; set; }
        public Nullable<int> Tamanho { get; set; }
        public string TipoCompo { get; set; }
        public string PropComponente { get; set; }
        public string DesCompo { get; set; }
        public string Comentario { get; set; }
        public Nullable<int> TamProc { get; set; }
        public Nullable<int> TamLista { get; set; }
        public string CampodeLink { get; set; }
    
        public virtual ASSMED_Programa ASSMED_Programa { get; set; }
    }
}
