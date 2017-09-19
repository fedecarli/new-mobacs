using Softpark.DomainModels.Attributes;
using System.Linq;
using System.Reflection;

namespace Softpark.DomainModels
{
    public interface IEntidade
    {
        string Tabela { get; }
        string Schema { get; }
    }

    internal abstract class AEntidade<T> : IEntidade where T : IEntidade
    {
        public AEntidade()
        {
            var t = typeof(T);

            var ti = t.GetTypeInfo();

            var ta = ti.GetCustomAttribute<TableAttribute>();

            Tabela = ta.Tabela;
            Schema = ta.Schema;
        }

        public string Tabela { get; }

        public string Schema { get; }

        public static AEntidade<T> GetInstance()
        {
            return typeof(T).GetTypeInfo()
                .Assembly.DefinedTypes
                .FirstOrDefault(x => x.IsClass && !x.IsAbstract && x.ImplementedInterfaces
                .Any(y => y.GetTypeInfo() == typeof(T).GetTypeInfo()))
                ?.DeclaredConstructors.FirstOrDefault()?
                .Invoke(null) as AEntidade<T>;
        }
    }

    public interface IDataProxy<T> where T : IEntidade
    {
    }
}
