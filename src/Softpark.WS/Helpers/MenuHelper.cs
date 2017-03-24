using Softpark.Models;
using Softpark.WS.Controllers;
using System.Linq;

namespace System.Web.Mvc
{
    public static class MenuHelper
    {
        private static DomainContainer db => DomainContainer.Current;

        public static MvcHtmlString Menu(this HtmlHelper helper, int? id_pai = null)
        {
            var html = string.Empty;

            try
            {
                var usuario = helper.ViewContext.HttpContext.User.Usuario();

                var rsPags = from g in (from m in db.Menu
                                        join Gm in db.Grupo_Menu on m.id_menu equals Gm.id_menu
                                        join Gu in db.Grupo_Usuario on Gm.id_grupo equals Gu.id_grupo
                                        where (Gm.ler == 1 || Gm.cadastrar == 1 || Gm.atualizar == 1 || Gm.excluir == 1 || Gm.imprimir == 1) &&
                                        Gu.CodUsu == usuario.CodUsu && m.id_sistema == 99 && m.id_menu_pai == id_pai && m.ativo == 1
                                        group new { m, Gm, Gu } by new { m.id_menu_pai, m.id_menu, m.descricao, m.icone, m.link, m.ordem })
                             orderby g.Key.ordem
                             select g.Key;

                if (rsPags.Any())
                {
                    var ul = new TagBuilder("ul");
                    ul.AddCssClass("nav");

                    if (id_pai == null)
                        ul.GenerateId("side-menu");
                    else
                        ul.AddCssClass("nav-second-level");

                    foreach (var item in rsPags)
                    {
                        var li = new TagBuilder("li");

                        var a = new TagBuilder("a");
                        a.MergeAttribute("href", string.IsNullOrEmpty(item.link) ? "#nogo" : item.link);
                        if (id_pai == null)
                            a.AddCssClass("mFirst");

                        var i = new TagBuilder("i");
                        i.AddCssClass("fa");
                        i.AddCssClass(item.icone);
                        i.AddCssClass(string.IsNullOrEmpty(item.link) ? "fa-fw" : "fa-angle-right");
                        if (string.IsNullOrEmpty(item.link))
                            i.MergeAttribute("style", "font-size: 18px !important");

                        var span = new TagBuilder("span");
                        if (!string.IsNullOrEmpty(item.link))
                        {
                            span.AddCssClass("fa");
                            span.AddCssClass("arrow");
                        }

                        a.InnerHtml = $"{i} {item.descricao} {span}";

                        li.InnerHtml = $"{a} {helper.Menu(item.id_menu)}";

                        ul.InnerHtml = li.ToString();
                    }

                    html = ul.ToString();
                }
            }
            finally { }

            return new MvcHtmlString(html);
        }
    }
}