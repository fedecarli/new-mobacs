namespace Softpark.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class VW_MenuSistema
    {
        public int id_menu { get; set; }
        public int? id_pai_indireto { get; set; }
        public int? id_pai_direto { get; set; }
        public string link { get; set; }
        public string sublink { get; set; }
        public string icone { get; set; }
        public string descricao { get; set; }
        public int ordem { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
