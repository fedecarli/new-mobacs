using System.ComponentModel;

namespace DataTables.AspNet.WebApi2
{
    public enum SortingDirections
    {
        [Description("asc")]
        Ascending,
        [Description("desc")]
        Descending,
        Both = 0
    }
}
