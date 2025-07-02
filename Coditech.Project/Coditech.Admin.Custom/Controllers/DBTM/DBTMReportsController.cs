using Coditech.Admin.Agents;
using Coditech.Admin.ViewModel;
using Coditech.Common.Helper.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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
        public virtual ActionResult BatchWiseReports()
        {
            DBTMReportsListViewModel dBTMReportsViewModel = new DBTMReportsListViewModel();
            dBTMReportsViewModel.FromDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
            dBTMReportsViewModel.ToDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());           
            return View(batchreports, dBTMReportsViewModel);
        }

        [HttpGet]
        public virtual ActionResult GetBatchWiseReports(int generalBatchMasterId, int dBTMTestMasterId, DateTime FromDate, DateTime ToDate)
        {
            DBTMReportsListViewModel dBTMReportsViewModel = _dBTMReportsAgent.BatchWiseReports(generalBatchMasterId,dBTMTestMasterId,FromDate, ToDate);
            dBTMReportsViewModel.IsRecordFound = dBTMReportsViewModel?.DataTable?.Rows?.Count > 0;
            return PartialView("~/Views/Shared/_DBTMReports.cshtml", dBTMReportsViewModel);
        }

        [HttpGet]
        public virtual ActionResult TestWiseReports()
        {
            DBTMReportsListViewModel dBTMReportsViewModel = new DBTMReportsListViewModel();
            dBTMReportsViewModel.FromDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
            dBTMReportsViewModel.ToDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            return View(testreports, dBTMReportsViewModel);
        }

        [HttpGet]
        public virtual ActionResult GetTestWiseReports(int dBTMTestMasterId,long dBTMTraineeDetailId,DateTime FromDate,DateTime ToDate)
        {
            DBTMReportsListViewModel dBTMReportsViewModel = _dBTMReportsAgent.TestWiseReports(dBTMTestMasterId,dBTMTraineeDetailId,FromDate,ToDate);
            return PartialView("~/Views/Shared/_DBTMReports.cshtml", dBTMReportsViewModel);
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
    }
}