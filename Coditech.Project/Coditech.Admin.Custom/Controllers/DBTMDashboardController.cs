using Coditech.Admin.Agents;
using Coditech.Admin.ViewModel;
using Coditech.Common.Helper.Utilities;
using Microsoft.AspNetCore.Mvc;

using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.Admin.Controllers
{
    public class DBTMDashboardController : BaseController
    {
        private readonly IDashboardAgent _dashboardAgent;
        private readonly IDBTMDashboardAgent _dBTMDashboardAgent;

        public DBTMDashboardController(IDashboardAgent dashboardAgent, IDBTMDashboardAgent dBTMDashboardAgent)
        {
            _dashboardAgent = dashboardAgent;
            _dBTMDashboardAgent = dBTMDashboardAgent;
        }

        [HttpGet]
        public IActionResult Index(short numberOfDaysRecord)
        {
            DashboardViewModel dashboardViewModel = _dashboardAgent.GetDashboardDetails(numberOfDaysRecord);
            if (IsNotNull(dashboardViewModel) && !string.IsNullOrEmpty(dashboardViewModel.DashboardFormEnumCode))
            {
                if (dashboardViewModel.DashboardFormEnumCode.Equals(DashboardFormCustomEnum.DBTMCentreDashboard.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    DBTMDashboardViewModel dBTMDashboardViewModel = _dBTMDashboardAgent.GetDBTMDashboardDetails();
                    return View("~/Views/DBTM/DBTMDashboard/DBTMCentreDashboard.cshtml", dBTMDashboardViewModel);
                }
                else if (dashboardViewModel.DashboardFormEnumCode.Equals(DashboardFormCustomEnum.DBTMTrainerDashboard.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    DBTMDashboardViewModel dBTMDashboardViewModel = _dBTMDashboardAgent.GetDBTMDashboardDetails();
                    return View("~/Views/DBTM/DBTMDashboard/DBTMTrainerDashboard.cshtml", dBTMDashboardViewModel);
                }
            }
            return View("~/Views/Dashboard/GeneralDashboard.cshtml");
        }
    }
}
