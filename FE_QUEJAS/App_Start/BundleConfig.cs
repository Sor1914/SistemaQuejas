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
            ScriptBundleValidate(bundles);
        }

        public static void StyleBundle(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/css")
                     .Include("~/Content/bootstrap.css"));
        }

        public static void ScriptBundle(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js")
                     .Include("~/Scripts/jquery-3.3.1.js"));
        }
        public static void ScriptBundleValidate(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/jsValidate")
                     .Include("~/Scripts/jquery.validate.js").Include("~/Scripts/jquery.validate.unobtrusive.js"));
        }

        public static void StyleBundleCss(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/cssLocal")
                     .Include("~/Content/Site.css"));
        }
    }
}
