using Softpark.Infrastructure.Extras;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Softpark.Models
{
    public partial class FichaVisitaDomiciliarChild
    {
        [NotMapped]
        public virtual DateTime? DataNascimento
        {
            get { return dtNascimento.HasValue ? dtNascimento.Value.FromUnix(true) : (DateTime?)null; }
            set { dtNascimento = value.HasValue ? value.Value.ToUnix() : (long?)null; }
        }
    }

    public partial class UnicaLotacaoTransport
    {
        [NotMapped]
        public virtual DateTime DataDeAtendimento
        {
            get { return dataAtendimento.FromUnix(true); }
            set { dataAtendimento = value.ToUnix(); }
        }
    }
}
