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
    
    public partial class AE_MovimentaItemOLD
    {
        public int NumContrato { get; set; }
        public int AnoMov { get; set; }
        public decimal NumMov { get; set; }
        public int Item { get; set; }
        public Nullable<int> CodUnidade { get; set; }
        public Nullable<System.DateTime> Validade { get; set; }
        public Nullable<int> CodProd { get; set; }
        public string Referencia { get; set; }
        public Nullable<decimal> Qtd { get; set; }
        public Nullable<decimal> FatorUnidade { get; set; }
        public Nullable<decimal> QtdSai { get; set; }
        public Nullable<int> SitTrib { get; set; }
        public Nullable<decimal> ValorUnitario { get; set; }
        public Nullable<decimal> ValorDesconto { get; set; }
        public Nullable<decimal> BCICMS { get; set; }
        public Nullable<decimal> VlrICMS { get; set; }
        public Nullable<decimal> AliquotaICMS { get; set; }
        public Nullable<int> CodUsu { get; set; }
        public Nullable<decimal> VlrIPI { get; set; }
        public Nullable<decimal> AliquotaIPI { get; set; }
        public Nullable<decimal> Comissao { get; set; }
        public Nullable<decimal> BCICMSSub { get; set; }
        public Nullable<int> CFOP { get; set; }
        public Nullable<decimal> VlrICMSSub { get; set; }
        public Nullable<decimal> AliqICMSSub { get; set; }
        public string NumIP { get; set; }
        public Nullable<System.DateTime> DtSistema { get; set; }
        public Nullable<decimal> posologia { get; set; }
        public Nullable<System.DateTime> prox_retirada { get; set; }
        public Nullable<decimal> Origem_NumMov { get; set; }
        public Nullable<int> Origem_Item { get; set; }
        public string Lista { get; set; }
        public string Controlado { get; set; }
        public Nullable<decimal> QtdSaiBkp { get; set; }
        public Nullable<decimal> Origem_NumMovBkp { get; set; }
        public Nullable<int> Origem_ItemBkp { get; set; }
        public Nullable<int> Bkp { get; set; }
    }
}
