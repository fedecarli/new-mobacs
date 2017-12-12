using System;

namespace Softpark.WS.ViewModels
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class LogMobileViewModel
    {
        public int IdLog { get; set; }
        public Guid Token { get; set; }
        public string LogDescricao { get; set; }
        public DateTime DtLog { get; set; }
        public string TipoChamada { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}