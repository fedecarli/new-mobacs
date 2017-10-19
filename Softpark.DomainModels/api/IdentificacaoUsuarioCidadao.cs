using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softpark.DomainModels.api
{
    public interface IIdentificacaoUsuarioCidadao : IEntidade { }

    internal class IdentificacaoUsuarioCidadao : AEntidade<IIdentificacaoUsuarioCidadao>, IIdentificacaoUsuarioCidadao
    {
    }
}
