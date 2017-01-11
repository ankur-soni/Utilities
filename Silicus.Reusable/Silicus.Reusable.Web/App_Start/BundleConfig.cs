using System.Configuration;
using System.Web;
using System.Web.Optimization;

namespace Silicus.Reusable.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/icon-outlines-style.css",
                      "~/Content/animate.css",
                      "~/Content/custom-style.css",
                      "~/Content/perfect-scrollbar.css",
                      "~/Content/style.css",
                      "~/Content/plugins/sweetalert/sweetalert.css",
                      "~/Content/font-awesome/css/font-awesome.css",
                      "~/Content/plugins/switchery/switchery.css",
                      "~/Content/plugins/selectize/selectize.css"));


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/requiredJS").Include(
                         "~/Scripts/plugins/validate/jquery.validate.min.js",
                         "~/Scripts/modernizr-*",
                         "~/Scripts/bootstrap.js",
                         "~/Scripts/respond.js",
                         "~/Scripts/jquery.unobtrusive-ajax.min.js",
                          "~/Scripts/Custom/common.js",
                          "~/Scripts/plugins/sweetalert/sweetalert.min.js",
                          "~/Scripts/Custom/layout.js",
                          "~/Scripts/plugins/perfect-scrollbar/perfect-scrollbar.js",
                          "~/Scripts/plugins/perfect-scrollbar/perfect-scrollbar.jquery.js",
                          "~/Scripts/jquery.validate.unobtrusive.js"
                       ));

            //// Loading latest version of Kendo
            bundles.Add(new ScriptBundle("~/bundles/kendoJS").Include(
                "~/Scripts/plugins/Kendo/" + ConfigurationManager.AppSettings["KendoVersion"] + "/jquery.min.js",
                "~/Scripts/plugins/kendo/" + ConfigurationManager.AppSettings["KendoVersion"] + "/kendo.all.min.js",
                "~/Scripts/plugins/kendo/" + ConfigurationManager.AppSettings["KendoVersion"] + "/kendo.aspnetmvc.min.js"));

            bundles.Add(new StyleBundle("~/Content/kendoCSS").Include(
            "~/Content/plugins/kendo/" + ConfigurationManager.AppSettings["KendoVersion"] + "/kendo.common.min.css",
            "~/Content/plugins/kendo/" + ConfigurationManager.AppSettings["KendoVersion"] + "/kendo.custom.css"));
        }
    }
}
