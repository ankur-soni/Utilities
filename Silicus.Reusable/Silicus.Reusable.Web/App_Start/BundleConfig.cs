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
                      "~/Content/site.css",
                      "~/Content/sweetalert.css"));

            bundles.Add(new ScriptBundle("~/bundles/requiredJS").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/modernizr-*",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/jquery.blockUI.js",
                         "~/Scripts/Custom/common.js",
                         "~/Scripts/sweetalert.min.js"
                      ));
        }
    }
}
