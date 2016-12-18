using System.Web;
using System.Web.Optimization;

namespace BlueCarGpsWeb
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/jquery-ui-1.9.1.custom.min.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-treeview.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/jquery.datetimepicker.full.js",
                      "~/Scripts/layout.js",
                      "~/Scripts/pseudo-data.js",
                      "~/Scripts/map.js",
                      "~/Scripts/jquery.qrcode.min.js",
                      "~/Scripts/echarts.min.js",
                      "~/Scripts/jquery.ztree.all-3.5.js",
                      "~/Scripts/jquery.combo.select.js",
                      "~/Scripts/jquery-increment-number.js",
                      "~/Scripts/highcharts/highcharts.js",
                      "~/Scripts/highcharts/modules/exporting.js",
                      //"~/Scripts/highcharts/highcharts-3d.js",
                      "~/Scripts/jquery-popModal.js",
                      "~/Scripts/jquery.file.upload/jquery.fileupload.js",
                      "~/Scripts/jquery.file.upload/jquery.iframe-transport.js",
                      "~/Scripts/jquery.file.upload/upload.file.data.js",
                      "~/Scripts/imgAreaSelect/jquery.imgareaselect.js",
                      "~/Scripts/imgAreaSelect/jquery.imgareaselect.pack.js"
                      ));


            bundles.Add(new ScriptBundle("~/bundles/vue").Include(
                     "~/Scripts/vue.js",
                     "~/Scripts/vue.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/jquery-ui.css",
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-treeview.css",
                      "~/Content/jquery-tree.css",
                      "~/Content/font-awesome.css",
                      "~/Content/layout.css",
                      "~/Content/site.css",
                      "~/Content/pagination.css",
                      "~/Content/combo.select.css",
                      "~/Content/jquery.datetimepicker.css",
                      "~/Content/highcharts.css",
                      "~/Content/jquery-popModal.css",
                      "~/Content/animate.css",
                      "~/Content/imgAreaSelect/imgareaselect-animated.css",
                      "~/Content/imgAreaSelect/imgareaselect-default.css",
                      "~/Content/imgAreaSelect/imgareaselect-deprecated.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/minitor").Include(
                      "~/Content/minitor.css"));

            bundles.Add(new StyleBundle("~/Content/shop").Include(
                      "~/Content/shop.css"));

            bundles.Add(new StyleBundle("~/Content/dashboard").Include(
                     "~/Content/dashboard.css"));
        }
    }
}
