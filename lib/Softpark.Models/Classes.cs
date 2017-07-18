using Softpark.Infrastructure.Extras;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Softpark.Models
{
    /// <summary>
    /// Ficha de Visita
    /// </summary>
    public partial class FichaVisitaDomiciliarChild
    {
        /// <summary>
        /// Data de Nascimento em Epoch
        /// </summary>
        [NotMapped]
        public virtual long? DataNascimento
        {
            get { return dtNascimento.HasValue ? dtNascimento.Value.ToUnix() : (long?)null; }
            set { dtNascimento = value.HasValue ? value.Value.FromUnix() : (DateTime?)null; }
        }
    }

    /// <summary>
    /// Cabeçalho
    /// </summary>
    public partial class UnicaLotacaoTransport
    {
        /// <summary>
        /// Data de atendiemnto em Epoch
        /// </summary>
        [NotMapped]
        public virtual long DataDeAtendimento
        {
            get { return dataAtendimento.ToUnix(); }
            set { dataAtendimento = value.FromUnix(); }
        }
    }
}
