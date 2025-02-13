using Coditech.Admin.Agents;
using Coditech.Admin.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
namespace Coditech.Admin.Controllers
{
    public class CustomDashboardController : BaseController
    {
        private readonly IDashboardAgent _dashboardAgent;
        private readonly ICustomDashboardAgent _customDashboardAgent;

        public CustomDashboardController(IDashboardAgent dashboardAgent, ICustomDashboardAgent dBTMDashboardAgent)
        {
            _dashboardAgent = dashboardAgent;
            _customDashboardAgent = dBTMDashboardAgent;
        }

        [HttpGet]
        public IActionResult Index(short numberOfDaysRecord)
        {
            DashboardViewModel dashboardViewModel = _dashboardAgent.GetDashboardDetails(numberOfDaysRecord);
            //if (IsNotNull(dashboardViewModel) && !string.IsNullOrEmpty(dashboardViewModel.DashboardFormEnumCode))
            //{
            //    if (dashboardViewModel.DashboardFormEnumCode.Equals(DashboardFormCustomEnum.CustomDashboard.ToString(), StringComparison.InvariantCultureIgnoreCase))
            //    {
            //        CustomDashboardViewModel customDashboardViewModel = _customDashboardAgent.GetDBTMDashboardDetails();
            //        return View("~/Views/DBTM/DBTMDashboard/DBTMCentreDashboard.cshtml", customDashboardViewModel);
            //    }
            //    else if (dashboardViewModel.DashboardFormEnumCode.Equals(DashboardFormCustomEnum.CustomDashboard.ToString(), StringComparison.InvariantCultureIgnoreCase))
            //    {
            //        CustomDashboardViewModel customDashboardViewModel = _customDashboardAgent.GetDBTMDashboardDetails();
            //        return View("~/Views/DBTM/DBTMDashboard/DBTMTrainerDashboard.cshtml", customDashboardViewModel);
            //    }
            //}
            return View("~/Views/Dashboard/GeneralDashboard.cshtml");
        }


        public ActionResult Index()
        {
            // Info.  
            return this.View();
        }

        /// <summary>  
        /// GET: /Home/GetCalendarData  
        /// </summary>  
        /// <returns>Return data</returns>  

        [HttpGet]
        public ActionResult GetCalendarData()
        {
            // Initialization.  
            JsonResult result = new JsonResult(null);

            try
            {
                // Loading.  
                List<CalendarViewModel> data = this.LoadData();

                // Processing.  
                result = this.Json(data, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            // Return info.  
            return result;
        }

        /// <summary>  
        /// Load data method.  
        /// </summary>  
        /// <returns>Returns - Data</returns>  
        private List<CalendarViewModel> LoadData()
        {
            // Initialization.  
            List<CalendarViewModel> lst = new List<CalendarViewModel>();

            try
            {
                // Initialization.  
                string line = string.Empty;
                //  string srcFilePath = "";
                var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                //var fullPath = Path.Combine(rootPath, srcFilePath);
                var fullPath = Path.Combine(rootPath);
                string filePath = new Uri(fullPath).LocalPath;
                StreamReader sr = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read));

                // Read file.  
                while ((line = sr.ReadLine()) != null)
                {
                    // Initialization.  
                    CalendarViewModel infoObj = new CalendarViewModel();
                    string[] info = line.Split(',');

                    // Setting.  
                    infoObj.CalendarId = Convert.ToInt32(info[0].ToString());
                    infoObj.Title = info[1].ToString();
                    infoObj.Desc = info[2].ToString();
                    infoObj.Start_Date = info[3].ToString();
                    infoObj.End_Date = info[4].ToString();

                    // Adding.  
                    lst.Add(infoObj);
                }

                // Closing.  
                sr.Dispose();
                sr.Close();
            }
            catch (Exception ex)
            {
                // info.  
                Console.Write(ex);
            }

            // info.  
            return lst;
        }
    }
}
