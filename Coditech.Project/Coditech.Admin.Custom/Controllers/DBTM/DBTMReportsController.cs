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

        [HttpGet]
        public ActionResult GetBatchWiseMultipleReportsFile(string dBTMTestMasterIds, int generalBatchMasterId, DateTime FromDate, DateTime ToDate, string reportType)
        {
            DBTMReportsListViewModel datalist = _dBTMReportsAgent.BatchWiseMultipleReportsFile(dBTMTestMasterIds, generalBatchMasterId, FromDate, ToDate, reportType);
            return PartialView("~/Views/Shared/_DBTMMultiReports.cshtml", datalist);
        }

        [HttpGet]
        public JsonResult CheckBatchReportAvailability(string dBTMTestMasterIds, int generalBatchMasterId, DateTime fromDate, DateTime toDate)
        {
            DBTMReportsListViewModel reportData = _dBTMReportsAgent.BatchWiseMultipleReports(dBTMTestMasterIds, generalBatchMasterId, fromDate, toDate);
            if (reportData == null || reportData.DataTableList == null || reportData.DataTableList.Count == 0)
            {
                return Json(new { success = false, message = "No data available for download." });
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public ActionResult DownloadBatchReport(string dBTMTestMasterIds, int generalBatchMasterId, DateTime fromDate, DateTime toDate, string reportType)
        {
            DBTMReportsListViewModel reportData = _dBTMReportsAgent.BatchWiseMultipleReportsFile(dBTMTestMasterIds, generalBatchMasterId, fromDate, toDate, reportType);
            if (reportData == null || string.IsNullOrEmpty(reportData.FilePath) || !System.IO.File.Exists(reportData.FilePath))
            {
                return Content("Report not found.");
            }
            var fileBytes = System.IO.File.ReadAllBytes(reportData.FilePath);
            var fileName = reportData.FileName;
            _dBTMReportsAgent.DeleteReportsFile(fileName);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
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
        public ActionResult ViewActivityDetailPopup(long dBTMDeviceDataId)
        {
            DBTMReportVerticalDataViewModel model = _dBTMReportsAgent.GetActivityVerticalDetails(dBTMDeviceDataId);
            if (model == null || model.DataTable == null || model.DataTable.Rows.Count == 0)
                return Content("No activity details found.");
            return PartialView("~/Views/DBTM/DBTMReports/_DBTMReportVerticalDetailPopup.cshtml", model);
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
            if (string.IsNullOrWhiteSpace(reportType) || reportType == "undefined")
            {
                reportType = "excel";
            }
            DBTMReportsListViewModel reportData = _dBTMReportsAgent.TestWiseMultipleReportsFile(dBTMTestMasterIds, dBTMTraineeDetailId, fromDate, toDate, reportType);
            if (reportData == null || string.IsNullOrEmpty(reportData.FilePath) || !System.IO.File.Exists(reportData.FilePath))
            {
                return Content("Report not found.");
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(reportData.FilePath);
            string fileName = reportData.FileName;
            _dBTMReportsAgent.DeleteReportsFile(fileName);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        //TestWise Graph Reports
        [HttpGet]
        public ActionResult TestWiseGraphReports()
        {
            DBTMGraphListViewModel dBTMReportsViewModel = new DBTMGraphListViewModel();
            dBTMReportsViewModel.FromDate = DateTime.Today;
            dBTMReportsViewModel.ToDate = DateTime.Today;
            BindDBTMGraph(dBTMReportsViewModel);
            return View("~/Views/DBTM/DBTMReports/TestWiseGraphReports.cshtml", dBTMReportsViewModel);
        }

        [HttpGet]
        public ActionResult GetTestWiseGraphReports( int dBTMTestMasterId, long dBTMTraineeDetailId, string dBTMGraphMasterIds, string graphMode, DateTime FromDate, DateTime ToDate)
        {
            if (!string.IsNullOrEmpty(graphMode) &&
                graphMode.Equals("InstantaneousChart", StringComparison.OrdinalIgnoreCase))
            {
                ToDate = FromDate;
            }
            if (!string.IsNullOrEmpty(dBTMGraphMasterIds))
            {
                List<GraphModel> graphModels = _dBTMReportsAgent.TestWiseGraphReportsV2( dBTMTestMasterId, dBTMTraineeDetailId, dBTMGraphMasterIds, graphMode, FromDate, ToDate );

                if (graphModels != null && graphModels.Any(x => x.IsRecordFound))
                {
                    return PartialView("~/Views/Shared/Charts/_MultipleGraphs.cshtml", graphModels);
                }
            }
            return Content("No Record Found.");
        }

        [HttpGet]
        public JsonResult GetGraphListByDBTMTestMasterId(int dBTMTestMasterId, string graphMode)
        {
            var list = new List<SelectListItem>();
            DBTMGraphMasterListViewModel graphList = _dBTMTestAgent.DBTMGraph(dBTMTestMasterId);

            if (graphList?.DBTMGraphMasterList != null)
            {
                list = graphList.DBTMGraphMasterList
                                .Where(g => g.GraphMode == graphMode && g.IsActive )
                                .Select(g => new SelectListItem
                                {
                                    Text = $"{g.GraphName}",
                                    Value = g.DBTMGraphMasterId.ToString()
                                }).ToList();
            }
            return Json(list);
        }

        [HttpGet]
        public IActionResult GetActivityPerformedDates(int dBTMTestMasterId, long dBTMTraineeDetailId )
        {
            if (dBTMTestMasterId <= 0 || dBTMTraineeDetailId <= 0)
                return Json(new List<string>());

            List<DateTime> dates = _dBTMReportsAgent.GetActivityPerformedDates(dBTMTestMasterId, dBTMTraineeDetailId);
            List<string> result = dates.Select(d => d.ToString("yyyy-MM-dd")).ToList();

            return Json(result);
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

        public ActionResult GetNameWiseReportsFile(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate, string reportType)
        {
            DBTMReportsListViewModel datalist = _dBTMReportsAgent.TestWiseMultipleReportsFile(dBTMTestMasterIds, dBTMTraineeDetailId, FromDate, ToDate, reportType);
            return PartialView("~/Views/Shared/_DBTMMultiReports.cshtml", datalist);
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
                foreach (var item in dBTMBatchActivityList.DBTMTestList.Where(x => x.IsActive).OrderBy(x => x.PerformanceMatrix).ThenBy(x => x.TestName))
                {
                    dBTMReportsViewModel.CustomDropdownList1.Add(new SelectListItem
                    {
                        Text = item.TestName,
                        Value = item.DBTMTestMasterId.ToString(),
                    });
                }
            }
        }

        protected virtual void BindDBTMGraph(DBTMGraphListViewModel dBTMTestViewModel)
        {
            dBTMTestViewModel.DBTMGraphMasterList = dBTMTestViewModel.DBTMGraphMasterList ?? new List<SelectListItem>();
            if (dBTMTestViewModel.DBTMTestMasterId > 0)
            {
                DBTMGraphMasterListViewModel dBTMGraphMasterList = _dBTMTestAgent.DBTMGraph(dBTMTestViewModel.DBTMTestMasterId);
                if (dBTMGraphMasterList?.DBTMGraphMasterList != null)
                {
                    foreach (var item in dBTMGraphMasterList.DBTMGraphMasterList)
                    {
                        dBTMTestViewModel.DBTMGraphMasterList.Add(new SelectListItem
                        {
                            Text = $"{item.GraphName} ({item.GraphMode})",
                            Value = item.DBTMGraphMasterId.ToString(),
                            Selected = dBTMTestViewModel.DBTMSelectedGraph != null &&
                                       dBTMTestViewModel.DBTMSelectedGraph.Contains(item.DBTMGraphMasterId.ToString())
                        });
                    }
                }
            }
        }
        #endregion
    }
}