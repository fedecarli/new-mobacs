namespace Softpark.Infrastructure
{
    public interface IEntity<T, TE, TN> : IEntity<T, TE>, IEntity
    {
        TN Idc { get; }
    }

    public interface IEntity<T, TE> : IEntity<T>, IEntity
    {
        TE Idb { get; }
    }

    public partial interface IEntity<T> : IBaseEntity<T>, IEntity
    {
    }

    public interface IBaseEntity<T> : IEntity
    {
        T Id { get; }

        bool Equals(object obj);
    }

    public interface IEntity
    {
    }
}