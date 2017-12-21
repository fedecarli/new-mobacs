using System.ComponentModel;

namespace DataTables.AspNet.WebApi2
{
    public enum PaginationTypes
    {
        [Description("two_button")]
        TwoButton,
        [Description("full_numbers")]
        FullNumbers,
        [Description("simple_numbers")]
        SimpleNumbers,
        [Description("numbers")]
        Numbers,
        [Description("simple")]
        Simple,
        [Description("full")]
        Full,
        [Description("first_last_numbers")]
        FirstLastNumbers
    }
}
