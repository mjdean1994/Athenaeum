using System.Web;
using System.Web.Optimization;

namespace Athenaeum
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/Unify").Include(
                        "~/Scripts/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/Toastr").Include(
                        "~/Scripts/toastr.min.js"));

            bundles.Add(new StyleBundle("~/Content/Toastr").Include(
                        "~/Content/toastr.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/FontAwesome").Include(
                        "~/Content/font-awesome.min.css"));

            bundles.Add(new StyleBundle("~/Content/UnifyStyles").Include(
                        "~/Content/Unify/*.css"));

            bundles.Add(new StyleBundle("~/Content/Bootstrap").Include(
                        "~/Content/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/Custom").Include(
                        "~/Content/Site.css"));
        }
    }
}
