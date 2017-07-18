namespace Softpark.Infrastructure
{
    /// <summary>
    /// Entidade com três chaves
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <typeparam name="TN"></typeparam>
    public interface IEntity<T, TE, TN> : IEntity<T, TE>, IEntity
    {
        /// <summary>
        /// terceira chave
        /// </summary>
        TN Idc { get; }
    }

    /// <summary>
    /// Entidade com duas chaves
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    public interface IEntity<T, TE> : IEntity<T>, IEntity
    {
        /// <summary>
        /// segunda chave
        /// </summary>
        TE Idb { get; }
    }

    /// <summary>
    /// Entidade simples
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial interface IEntity<T> : IBaseEntity<T>, IEntity
    {
    }

    /// <summary>
    /// entidade base
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseEntity<T> : IEntity
    {
        /// <summary>
        /// chave
        /// </summary>
        T Id { get; }

        /// <summary>
        /// equalizador
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Equals(object obj);
    }

    /// <summary>
    /// entidade básica
    /// </summary>
    public interface IEntity
    {
    }
}