using System.ComponentModel;

namespace DataTables.AspNet.WebApi2
{
    public enum SortingDirections
    {
        [Description("asc")]
        Assending,
        [Description("desc")]
        Descending,
        Both = 0
    }
}
