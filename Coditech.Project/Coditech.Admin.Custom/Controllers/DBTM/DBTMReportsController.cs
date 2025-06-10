using Coditech.Admin.Agents;
using Coditech.Admin.ViewModel;
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
        public virtual ActionResult BatchWiseReports()
        {
            return View(batchreports, new DBTMBatchWiseReportsListViewModel());
        }

        [HttpGet]
        public virtual ActionResult GetBatchWiseReports(int generalBatchMasterId)
        {
            DBTMBatchWiseReportsListViewModel dBTMBatchWiseReportsViewModel = _dBTMReportsAgent.BatchWiseReports(generalBatchMasterId);
            return PartialView("~/Views/Shared/_DBTMReports.cshtml", dBTMBatchWiseReportsViewModel);
        }

        [HttpGet]
        public virtual ActionResult TestWiseReports()
        {
            DBTMTestWiseReportsListViewModel dTMTestWiseReportsListViewModel = new DBTMTestWiseReportsListViewModel();
            dTMTestWiseReportsListViewModel.FromDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
            dTMTestWiseReportsListViewModel.ToDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            return View(testreports, dTMTestWiseReportsListViewModel);
        }

        [HttpGet]
        public virtual ActionResult GetTestWiseReports(int dBTMTestMasterId,long dBTMTraineeDetailId,DateTime FromDate,DateTime ToDate)
        {
            DBTMTestWiseReportsListViewModel dBTMTestWiseReportsViewModel = _dBTMReportsAgent.TestWiseReports(dBTMTestMasterId,dBTMTraineeDetailId,FromDate,ToDate);
            return PartialView("~/Views/Shared/_DBTMReports.cshtml", dBTMTestWiseReportsViewModel);
        }
    }
}