using Coditech.Admin.Agents;
using Coditech.Admin.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Coditech.Admin.Controllers
{
    public class DBTMReportsController : BaseController
    {
        private readonly IDBTMReportsAgent _dBTMReportsAgent;
        private const string reports = "~/Views/DBTM/DBTMReports/BatchWiseReports.cshtml";

        public DBTMReportsController(IDBTMReportsAgent dBTMReportsAgent)
        {
            _dBTMReportsAgent = dBTMReportsAgent;
        }

        [HttpGet]
        public virtual ActionResult BatchWiseReports()
        {
            return View(reports, new DBTMBatchWiseReportsListViewModel());
        }

        [HttpGet]
        public virtual ActionResult GetBatchWiseReports(int generalBatchMasterId)
        {
            DBTMBatchWiseReportsListViewModel dBTMBatchWiseReportsViewModel = _dBTMReportsAgent.BatchWiseReports(generalBatchMasterId);
            return PartialView("~/Views/Shared/_DBTMBatchWiseReports.cshtml", dBTMBatchWiseReportsViewModel);
        }
    }
}