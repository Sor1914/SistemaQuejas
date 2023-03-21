using System.Web;
using System.Web.Optimization;

namespace FE_QUEJAS
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            StyleBundle(bundles);
            StyleBundleCss(bundles);
            ScriptBundle(bundles);
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
    "~/Scripts/jquery-3.3.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.bundle.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));
        }

        public static void StyleBundle(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/css")
                     .Include("~/Content/bootstrap.css"));
        }

        public static void ScriptBundle(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js")
                     .Include("~/Scripts/jquery-3.3.1.js")
                     .Include("~/Scripts/jquery.validate.js")
                     .Include("~/Scripts/jquery.validate.unobtrusive.js")
                      );
        }

        public static void StyleBundleCss(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/cssLocal")
                     .Include("~/Content/Site.css"));
        }


    }
}
