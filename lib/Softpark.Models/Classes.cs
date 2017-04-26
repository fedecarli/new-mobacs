using Softpark.Infrastructure.Extras;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Softpark.Models
{
    public partial class FichaVisitaDomiciliarChild
    {
        [NotMapped]
        public virtual long? DataNascimento
        {
            get { return dtNascimento.HasValue ? dtNascimento.Value.ToUnix() : (long?)null; }
            set { dtNascimento = value.HasValue ? value.Value.FromUnix() : (DateTime?)null; }
        }
    }

    public partial class UnicaLotacaoTransport
    {
        [NotMapped]
        public virtual long DataDeAtendimento
        {
            get { return dataAtendimento.ToUnix(); }
            set { dataAtendimento = value.FromUnix(); }
        }
    }
}
