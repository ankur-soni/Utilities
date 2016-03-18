using System.Diagnostics.CodeAnalysis;
using System.Web.Optimization;
using System.Configuration;
namespace Silicus.ProjectTracker.Web
{
    [ExcludeFromCodeCoverage]
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725

        string BootstrapVersion = ConfigurationManager.AppSettings["BootstrapVersion"];
        string KendoVersion = ConfigurationManager.AppSettings["KendoVersion"];
        string KnockoutVersion = ConfigurationManager.AppSettings["KnockoutVersion"];
        string JQueryVersion = ConfigurationManager.AppSettings["JQueryVersion"];

        public static void RegisterBundles(BundleCollection bundles)
        {
            // Loading a kendo compatible jquery version 
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            "~/Scripts/Kendo/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/notification").Include(
            "~/Scripts/toastr.min.js",
            "~/Scripts/notifications.js"
            ));

            //// Loading latest version of Kendo
            bundles.Add(new ScriptBundle("~/bundles/kendoJS").Include(
                "~/Scripts/Kendo/" + ConfigurationManager.AppSettings["KendoVersion"] + "/jquery.min.js",
                "~/Scripts/kendo/" + ConfigurationManager.AppSettings["KendoVersion"] + "/kendo.all.min.js",
                //"~/Scripts/kendo/" + ConfigurationManager.AppSettings["KendoVersion"] + "/kendo.dataviz.min.js",
                "~/Scripts/kendo/" + ConfigurationManager.AppSettings["KendoVersion"] + "/kendo.aspnetmvc.min.js"));

            bundles.Add(new StyleBundle("~/Content/kendoCSS").Include(
            "~/Content/css/kendo/" + ConfigurationManager.AppSettings["KendoVersion"] + "/kendo.common.min.css",
            "~/Content/css/kendo/" + ConfigurationManager.AppSettings["KendoVersion"] + "/kendo.default.min.css"));

            //// Loading latest version of Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/BootstrapJS").Include(
                "~/Scripts/Bootstrap/" + ConfigurationManager.AppSettings["BootstrapVersion"] + "/bootstrap.min.js",
                "~/Scripts/Bootstrap/" + ConfigurationManager.AppSettings["BootstrapVersion"] + "/responsiveTabs.js"));

            bundles.Add(new StyleBundle("~/Content/BootstrapCSS").Include(
            "~/Content/css/Bootstrap/" + ConfigurationManager.AppSettings["BootstrapVersion"] + "/bootstrap.min.css",
            "~/Content/css/Bootstrap/" + ConfigurationManager.AppSettings["BootstrapVersion"] + "/bootstrap-theme.min.css"));

            //// Loading latest version of Knockout
            //bundles.Add(new ScriptBundle("~/bundles/KnockoutJS").Include(
            // "~/Scripts/Knockout/" + ConfigurationManager.AppSettings["KnockoutVersion"] + "/.js",
            // "~/Scripts/Knockout/" + ConfigurationManager.AppSettings["KnockoutVersion"] + "/.js"));

            //bundles.Add(new StyleBundle("~/Content/KnockoutCSS").Include(
            //"~/Content/css/Knockout/" + ConfigurationManager.AppSettings["KnockoutVersion"] + "/.css",
            //"~/Content/css/Knockout/" + ConfigurationManager.AppSettings["KnockoutVersion"] + "/.css"));

            //// Loading latest version of JQuery
            bundles.Add(new ScriptBundle("~/bundles/JQuery").Include(
                "~/Scripts/JQuery/" + ConfigurationManager.AppSettings["JQueryVersion"] + "/jquery-{version}.min.js",
                "~/Scripts/JQuery/" + ConfigurationManager.AppSettings["JQueryVersion"] + "/jquery-ui.min.js"));

            bundles.Add(new StyleBundle("~/Content/JQueryUI").Include(
            "~/Content/css/JQueryUI/" + ConfigurationManager.AppSettings["JQueryVersion"] + "/jquery-ui.min.css",
            "~/Content/css/JQueryUI/" + ConfigurationManager.AppSettings["JQueryVersion"] + "/jquery.ui.structure.min.css"));

            //// Loading JQuery Validation
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/JQuery/" + ConfigurationManager.AppSettings["JQueryVersion"] + "/jquery.unobtrusive*",
                "~/Scripts/JQuery/" + ConfigurationManager.AppSettings["JQueryVersion"] + "/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //Common JS in the Project
            bundles.Add(new ScriptBundle("~/bundles/CommonJS").Include(
                        "~/Scripts/toastr.min.js",
                        "~/Scripts/prefixfree.min.js",
                        "~/Scripts/commonjs.js"));

            bundles.Add(new ScriptBundle("~/bundles/ProjectTracker").Include(
            "~/Scripts/projecttracker.js"
            ));
        }

    }
}