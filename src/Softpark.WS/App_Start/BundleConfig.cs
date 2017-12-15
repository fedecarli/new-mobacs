using System;
using System.Web.Optimization;

namespace Softpark.WS
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            string version = DateTime.Now.ToString("yyyyMMddHHmmss");

#if !DEBUG
            version = DateTime.Now.ToString("yyyyMMdd");
#endif

            bundles.Add(new ScriptBundle("~/bundles/common.js").Include("~/Scripts/fichas.js"));

            bundles.Add(new ScriptBundle("~/bundles/sigsm.js").Include(
                "~/Scripts/json2.min.js",
                "~/../js/jquery-1.11.0.js",
                "~/../js/bootstrap.min.js",
                "~/../js/jquery-ui.js",
                "~/../js/plugins/metisMenu/metisMenu.min.js",
                "~/../js/plugins/dataTables/jquery.dataTables.js",
                "~/../js/plugins/dataTables/dataTables.bootstrap.js",
                "~/../js/plugins/dataTables/fnSetFilteringDelay.js",
                "~/../js/sb-admin-2.js",
                "~/../js/jquery.mask.min.js",
                "~/../js/jquery.maskMoney.js",
                "~/../../_inc/js/jquery-ui-timepicker-addon.js",
                "~/../../_inc/js/main.js",
                "~/../js/main.js",
                "~/../js/plugins/tinymce/tinymce.min.js",
                "~/Scripts/bootstrap-select.min.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js",
                "~/../js/plugins/linq/JSLINQ.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new StyleBundle("~/bundles/sigsm.css").Include(
                "~/../css/bootstrap.min.css",
                "~/../css/jquery-ui.css",
                "~/../css/plugins/metisMenu/metisMenu.min.css",
                "~/../css/plugins/social-buttons.css",
                "~/../css/plugins/dataTables.bootstrap.css",
                "~/../css/sb-admin-2.css",
                "~/../font-awesome-4.3.0/css/font-awesome.min.css",
                "~/../css/main.css",
                "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/Home/ajax/default.asp").Include(
                      "~/../ajax/default.asp"));
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
