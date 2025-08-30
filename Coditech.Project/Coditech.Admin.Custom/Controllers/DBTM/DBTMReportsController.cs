using Coditech.Admin.Agents;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Coditech.Admin.Controllers
{
    public class DBTMReportsController : BaseController
    {
        private readonly IDBTMReportsAgent _dBTMReportsAgent;
        private const string batchreports = "~/Views/DBTM/DBTMReports/BatchWiseReports.cshtml";
        private const string testreports = "~/Views/DBTM/DBTMReports/TestWiseReports.cshtml";
        public DBTMReportsController(IDBTMReportsAgent dBTMReportsAgent)
        {
            _dBTMReportsAgent = dBTMReportsAgent;
        }

        [HttpGet]
        public ActionResult BatchWiseReports()
        {
            DBTMReportsListViewModel dBTMReportsViewModel = new DBTMReportsListViewModel();
            dBTMReportsViewModel.FromDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
            dBTMReportsViewModel.ToDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            return View(batchreports, dBTMReportsViewModel);
        }

        [HttpGet]
        public ActionResult GetBatchWiseReports(int generalBatchMasterId, int dBTMTestMasterId, DateTime FromDate, DateTime ToDate)
        {
            DBTMReportsListViewModel dBTMReportsViewModel = _dBTMReportsAgent.BatchWiseReports(generalBatchMasterId, dBTMTestMasterId, FromDate, ToDate);
            dBTMReportsViewModel.IsRecordFound = dBTMReportsViewModel?.DataTable?.Rows?.Count > 0;
            return PartialView("~/Views/Shared/_DBTMReports.cshtml", dBTMReportsViewModel);
        }

        [HttpGet]
        public ActionResult TestWiseReports()
        {
            DBTMReportsListViewModel dBTMReportsViewModel = new DBTMReportsListViewModel();
            dBTMReportsViewModel.FromDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
            dBTMReportsViewModel.ToDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            return View(testreports, dBTMReportsViewModel);
        }

        [HttpGet]
        public ActionResult GetTestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate)
        {
            DBTMReportsListViewModel dBTMReportsViewModel = _dBTMReportsAgent.TestWiseReports(dBTMTestMasterId, dBTMTraineeDetailId, FromDate, ToDate);
            return PartialView("~/Views/Shared/_DBTMReports.cshtml", dBTMReportsViewModel);
        }

        [HttpGet]
        public ActionResult TestWiseGraphReports()
        {
            DBTMGraphListViewModel dBTMReportsViewModel = new DBTMGraphListViewModel();
            dBTMReportsViewModel.FromDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
            dBTMReportsViewModel.ToDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            return View("~/Views/DBTM/DBTMReports/TestWiseGraphReports.cshtml", dBTMReportsViewModel);
        }

        [HttpGet]
        public ActionResult GetTestWiseGraphReports(int dBTMTestMasterId, long dBTMTraineeDetailId, byte dBTMGraphMasterId, DateTime FromDate, DateTime ToDate)
        {
            GraphModel graphModel = _dBTMReportsAgent.TestWiseGraphReports(dBTMTestMasterId, dBTMTraineeDetailId, dBTMGraphMasterId, FromDate, ToDate);
            if (graphModel.IsRecordFound)
            {
                if (graphModel.GraphType == "LineChart")
                {
                    return PartialView("~/Views/Shared/Charts/_LineChart.cshtml", graphModel.LineChartModel);
                }
                else if (graphModel.GraphType == "BarChart")
                {
                    return PartialView("~/Views/Shared/Charts/_BarChart.cshtml", graphModel.BarChartModel);
                }
                else if (graphModel.GraphType == "PieChart")
                {
                    return PartialView("~/Views/Shared/Charts/_PieChart.cshtml", graphModel.PieChartModel);
                }
            }
            return Content("No Record Found.");
        }

        public ActionResult GetTestByGeneralBatchMasterId(int generalBatchMasterId)
        {
            DropdownViewModel testDropdownn = new DropdownViewModel
            {
                DropdownType = DropdownCustomTypeEnum.DBTMBatchActivity.ToString(),
                DropdownName = "DBTMTestMasterId",
                Parameter = $"{generalBatchMasterId}~true",
                IsCustomDropdown = true
            };
            return PartialView("~/Views/Shared/Control/_DropdownList.cshtml", testDropdownn);
        }

        [HttpGet]
        public ActionResult GetGraphListByDBTMTestMasterId(int dBTMTestMasterId)
        {
            DropdownViewModel dBTMGraphByDBTMTestMaster = new DropdownViewModel()
            {
                DropdownType = DropdownCustomTypeEnum.DBTMGraph.ToString(),
                DropdownName = "DBTMGraphMasterId",
                Parameter = dBTMTestMasterId.ToString(),
                IsCustomDropdown = true
            };
            return PartialView("~/Views/Shared/Control/_DropdownList.cshtml", dBTMGraphByDBTMTestMaster);
        }
    }
}