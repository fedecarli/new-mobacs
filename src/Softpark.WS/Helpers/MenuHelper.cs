using Softpark.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Softpark.WS.Helpers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class MenuHelper
    {
        public static MvcHtmlString ActionLinkButton(this AjaxHelper ajax,
            string action, AjaxOptions options, object htmlAttributes, string content)
        {
            var str = ajax.ActionLink("{content}", action, new { }, options, htmlAttributes).ToHtmlString();

            str = str.Replace("{content}", content);

            return new MvcHtmlString(str);
        }

        public static HtmlString CarregaMenu(this HtmlHelper helper, UrlHelper url, int idUsuario, int? idPai, int idSistema, DomainContainer domain)
        {
            var menus = domain.VW_MenuSistema(idUsuario, idPai, idSistema).ToArray();

            if (menus.Length == 0) return new HtmlString("");

            var html = "<ul class=\"nav" +
                (idPai == null ? "\" id=\"side-menu\">" : " nav-second-level\">");

            foreach (var item in menus)
            {
                item.link = url.Content("~/../" + item.link);

                html +=
                    $"<li><a href=\"{item.link ?? "#nogo"}\"" +
                    $" class=\"{(idPai == null ? "mFirst" : "")}\">" +
                    $"<i class=\"fa {item.icone} {(item.link == null ? "fa-fw\" style=\"font-size: 18px !important;\"" : "fa-angle-right\"")}></i>" +
                    $" {item.descricao}{(item.link == null ? "<i class=\"da arrow fa\"></i>" : "")}</a>" +
                    helper.CarregaMenu(url, idUsuario, item.id_menu, idSistema, domain) +
                    "</li>";
            }

            html += "</ul>";

            return new HtmlString(html);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}