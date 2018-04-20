using System.Web;
using System.Web.Optimization;

namespace eggWeb
{
    public class BundleConfig
    {
        // 如需「搭配」的詳細資訊，請瀏覽 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Scripts").Include(
                        "~/Scripts/jquery-2.1.1.min.js",
                        "~/Scripts/materialize.min.js",
                        "~/Scripts/init.js"));
 
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/materialize.min.css",
                      "~/Content/style.css"));

            bundles.Add(new ScriptBundle("~/bundles/vue").Include(
                    "~/Scripts/vue.min.js",
                    "~/Scripts/vue-resource.min.js"));
 
        }
    }
}
