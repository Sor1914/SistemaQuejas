using System.Web;
using System.Web.Optimization;

namespace FE_QUEJAS
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {            
            StyleBundleCss(bundles);
            ScriptBundle(bundles);        
      
        }


        public static void ScriptBundle(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js")
                     .Include("~/Scripts/jquery-3.3.1.min.js")
                     .Include("~/Scripts/jquery.validate.min.js")
                     .Include("~/Scripts/jquery.validate.unobtrusive.min.js")
                     .Include("~/Scripts/sidebars.js")
                      );
        }

        public static void StyleBundleCss(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/css")
                     .Include("~/Content/Site.css")                     
                     .Include("~/Content/bootstrap.min.css")
                     .Include("~/fonts/font-awesome.min.css")
                     .Include("~/Content/sidebars.css"));
        }


    }
}
