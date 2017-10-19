using System;

namespace Softpark.DomainModels.Attributes
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    internal class TableAttribute : Attribute
    {
        public string Schema { get; set; } = "dbo";
        public string Tabela { get; }

        public TableAttribute(string tabela)
        {
            Tabela = tabela;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    internal class ColumnAttribute : Attribute
    {
        public string ColumnName { get; }
        public bool IsKey { get; set; }
        public int Order { get; set; }

        public ColumnAttribute(string coluna)
        {
            ColumnName = coluna;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    internal class HasOneAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    internal class CollectionOfAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    internal class BelongsToAttribute : Attribute
    {
        public BelongsToAttribute(string referenceKey)
        {
            ReferenceKey = referenceKey;
        }

        public string ReferenceKey { get; private set; }
    }
}
