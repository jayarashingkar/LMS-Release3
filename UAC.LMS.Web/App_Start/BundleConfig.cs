using System.Web.Optimization;

namespace UAC.LMS.Web.App_Start
{
    public class BundleConfig
    {
        // comment here 

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));


            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/fuelux.min.js",
                      "~/Scripts/bootstrapValidator.min.js",
                      "~/Scripts/bootbox.min.js",
                      "~/Scripts/bootstrap-select.min.js",
                      "~/Scripts/bootstrap-datepicker.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/fuelux.min.css",
                      "~/Content/bootstrapValidator.min.css",
                      "~/Content/bootstrap-select.min.css",
                      "~/Content/bootstrap-datepicker.min.css",
                      "~/Content/navmenu.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/Utilities").Include(
                      "~/Scripts/Utilities.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/GridUtil").Include(
                      "~/Scripts/GridUtil.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/BusinessUnit").Include(
                      "~/Scripts/BusinessUnit.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/Courses").Include(
                      "~/Scripts/Courses.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/Departments").Include(
                      "~/Scripts/Departments.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/EmployeeCourseList").Include(
                      "~/Scripts/EmployeeCourseList.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/Employees").Include(
                      "~/Scripts/Employees.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/JobTitles").Include(
                      "~/Scripts/JobTitles.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/UserLogins").Include(
                      "~/Scripts/UserLogins.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/SecuityConfig").Include(
                      "~/Scripts/SecuityConfig.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/PrintRoster").Include(
                      "~/Scripts/PrintRoster.js"
                      ));

            //added 11/15/2016
            bundles.Add(new ScriptBundle("~/bundles/Audit").Include(
                      "~/Scripts/Audit.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/CourseHistory").Include(
                "~/Scripts/CourseHistory.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/OverduesReport").Include(
            "~/Scripts/overduesreport.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/TrainingnotTakenReport").Include(
                "~/Scripts/trainingnottakenreport.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/ImportEmployees").Include(
               "~/Scripts/ImportEmployees.js"
               ));

            
            //BundleTable.EnableOptimizations = true;
        }
    }
}