using System.Web;
using System.Web.Optimization;

#pragma warning disable 1591
namespace Softpark.WS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.11.0.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/zendesk.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                      "~/Scripts/jquery-ui.js",
                      "~/Scripts/jquery-ui-timepicker-addon.js",
                      "~/Scripts/plugins/metisMenu/metisMenu.min.js",

                      "~/Scripts/plugins/dataTables/jquery.dataTables.js",
                      "~/Scripts/jquery.dataTables.odata.js",
                      "~/Scripts/plugins/dataTables/dataTables.bootstrap.js",
                      "~/Scripts/plugins/dataTables/fnSetFilteringDelay.js",

                      "~/Scripts/sb-admin-2.js",

                      "~/Scripts/jquery.mask.min.js",
                      "~/Scripts/jquery.maskMoney.js",
                      "~/Scripts/main.js",

                      "~/Scripts/plugins/tinymce/tinymce.min.js",
                      "~/Scripts/jquery.validate.min.js",
                      "~/Scripts/plugins/linq/JSLINQ.js",
                      "~/Scripts/esusfw.js",
                      "~/Scripts/components/carga.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome-4.3.0/css/font-awesome.min.css",
                      "~/Content/login.css"));

            bundles.Add(new StyleBundle("~/Content/site").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/sigsm").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome-4.3.0/css/font-awesome.min.css",
                      "~/Content/jquery-ui.min.css",
                      "~/Content/plugins/metisMenu/metisMenu.min.css",
                      "~/Content/plugins/social-buttons.css",
                      "~/Content/plugins/dataTables.bootstrap.css",
                      "~/Content/sb-admin-2.css",
                      "~/Content/main.css",
                      "~/ClienteCSS.ashx"));
        }
    }
}
