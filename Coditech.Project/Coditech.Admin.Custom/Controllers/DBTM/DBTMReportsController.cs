using Coditech.Admin.Agents;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Coditech.Admin.Controllers
{
    public class DBTMReportsController : BaseController
    {
        private readonly IDBTMReportsAgent _dBTMReportsAgent;
        private readonly IDBTMTestAgent _dBTMTestAgent;
        private const string namereports = "~/Views/DBTM/DBTMReports/NameWiseReports.cshtml";
        private const string testwisemultireports = "~/Views/DBTM/DBTMReports/TestWiseMultiReports.cshtml";
        private const string batchwisemultireports = "~/Views/DBTM/DBTMReports/BatchWiseMultiReports.cshtml";
        public DBTMReportsController(IDBTMReportsAgent dBTMReportsAgent, IDBTMTestAgent dBTMTestAgent)
        {
            _dBTMReportsAgent = dBTMReportsAgent;
            _dBTMTestAgent = dBTMTestAgent;
        }

        //Batchwise Reports
        [HttpGet]
        public ActionResult BatchWiseReports()
        {
            DBTMReportsListViewModel dBTMReportsViewModel = new DBTMReportsListViewModel();
            dBTMReportsViewModel.FromDate = DateTime.Today;
            dBTMReportsViewModel.ToDate = DateTime.Today;
            dBTMReportsViewModel.CustomDropdownList1 = new List<SelectListItem>();
            return View(batchwisemultireports, dBTMReportsViewModel);
        }

        [HttpGet]
        public ActionResult GetBatchWiseReports(string dBTMTestMasterIds, int generalBatchMasterId, DateTime FromDate, DateTime ToDate)
        {
            DBTMReportsListViewModel dBTMReportsViewModel = _dBTMReportsAgent.BatchWiseMultipleReports(dBTMTestMasterIds, generalBatchMasterId, FromDate, ToDate);
            return PartialView("~/Views/Shared/_DBTMMultiReports.cshtml", dBTMReportsViewModel);
        }

        //Test Wise Reports 
        [HttpGet]
        public ActionResult TestWiseReports()
        {
            DBTMReportsListViewModel dBTMReportsViewModel = new DBTMReportsListViewModel();
            dBTMReportsViewModel.FromDate = DateTime.Today;
            dBTMReportsViewModel.ToDate = DateTime.Today;
            BindDBTMBatchActivity(dBTMReportsViewModel);
            return View(testwisemultireports, dBTMReportsViewModel);
        }

        [HttpGet]
        public ActionResult GetTestWiseReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate)
        {
            DBTMReportsListViewModel dBTMReportsViewModel = _dBTMReportsAgent.TestWiseMultipleReports(dBTMTestMasterIds, dBTMTraineeDetailId, FromDate, ToDate);
            return PartialView("~/Views/Shared/_DBTMMultiReports.cshtml", dBTMReportsViewModel);
        }

        [HttpGet]
        public ActionResult GetTestWiseReportsFile(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate, string reportType)
        {
            DBTMReportsListViewModel datalist = _dBTMReportsAgent.TestWiseMultipleReportsFile(dBTMTestMasterIds, dBTMTraineeDetailId, FromDate, ToDate, reportType);
            return PartialView("~/Views/Shared/_DBTMMultiReports.cshtml", datalist);
        }

        [HttpGet]
        public JsonResult CheckReportAvailability(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate)
        {
            DBTMReportsListViewModel reportData = _dBTMReportsAgent.TestWiseMultipleReports(dBTMTestMasterIds, dBTMTraineeDetailId, fromDate, toDate);
            if (reportData == null || reportData.DataTableList == null || reportData.DataTableList.Count == 0)
            {
                return Json(new { success = false, message = "No data available for download." });
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public ActionResult DownloadReport(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, string reportType)
        {
            DBTMReportsListViewModel reportData = _dBTMReportsAgent.TestWiseMultipleReportsFile(dBTMTestMasterIds, dBTMTraineeDetailId, fromDate, toDate, reportType);
            if (reportData == null || string.IsNullOrEmpty(reportData.FilePath) || !System.IO.File.Exists(reportData.FilePath))
            { 
                return Content("Report not found.");
            }
            var fileBytes = System.IO.File.ReadAllBytes(reportData.FilePath);
            var fileName = reportData.FileName;
            System.IO.File.Delete(reportData.FilePath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        //TestWise Graph Reports
        [HttpGet]
        public ActionResult TestWiseGraphReports()
        {
            DBTMGraphListViewModel dBTMReportsViewModel = new DBTMGraphListViewModel();
            dBTMReportsViewModel.FromDate = DateTime.Today;
            dBTMReportsViewModel.ToDate = DateTime.Today;
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

        //NameWise Reports
        [HttpGet]
        public ActionResult NameWiseReports()
        {
            DBTMReportsListViewModel dBTMReportsViewModel = new DBTMReportsListViewModel();
            dBTMReportsViewModel.FromDate = DateTime.Today;
            dBTMReportsViewModel.ToDate = DateTime.Today;
            BindDBTMBatchActivity(dBTMReportsViewModel);
            return View(namereports, dBTMReportsViewModel);
        }

        [HttpGet]
        public ActionResult GetNameWiseReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate)
        {
            DBTMReportsListViewModel dBTMReportsViewModel = _dBTMReportsAgent.NameWiseReports(dBTMTestMasterIds, dBTMTraineeDetailId, FromDate, ToDate);
            return PartialView("~/Views/Shared/_DBTMMultiReports.cshtml", dBTMReportsViewModel);
        }

        #region Protected Methods
        public ActionResult GetMultiTestByGeneralBatchMasterId(int generalBatchMasterId)
        {
            DBTMReportsListViewModel batchreports = new DBTMReportsListViewModel();
            BindDBTMBatchActivity(batchreports);
            DropdownViewModel testDropdown = new DropdownViewModel
            {
                DropdownType = DropdownCustomTypeEnum.BatchWiseMultiReports.ToString(),
                DropdownName = "DBTMTestMasterId",
                Parameter = $"{generalBatchMasterId}~true",
                IsCustomDropdown = true
            };
            ViewBag.CustomDropdownList1 = batchreports.CustomDropdownList1;
            return PartialView("~/Views/Shared/Control/_DropdownList.cshtml", testDropdown);
        }

        protected void BindDBTMBatchActivity(DBTMReportsListViewModel dBTMReportsViewModel)
        {
            dBTMReportsViewModel.CustomDropdownList1 = dBTMReportsViewModel.CustomDropdownList1 ?? new List<SelectListItem>();
            DataTableViewModel dataTableModel = new DataTableViewModel() { PageSize = int.MaxValue };
            DBTMTestListViewModel dBTMBatchActivityList = _dBTMTestAgent.GetDBTMTestList(dataTableModel);
            dBTMReportsViewModel.CustomDropdownList1.Add(new SelectListItem
            {
                Text = "All",
                Value = "0"
            });
            if (dBTMBatchActivityList?.DBTMTestList != null)
            {
                foreach (var item in dBTMBatchActivityList.DBTMTestList)
                {
                    dBTMReportsViewModel.CustomDropdownList1.Add(new SelectListItem
                    {
                        Text = item.TestName,
                        Value = item.DBTMTestMasterId.ToString(),
                    });
                }
            }
        }
        #endregion
    }
}