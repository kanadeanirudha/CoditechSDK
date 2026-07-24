using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc;
namespace Coditech.Admin.Controllers
{
    public class DBTMPrintQRController : BaseController
    {
        private readonly IDBTMBatchAgent _dBTMBatchQRAgent;
        private readonly IDBTMPrintQRAgent _dBTMPrintQRAgent;
        private const string createDBTMPrintQR = "~/Views/DBTM/DBTMPrintQR/CreateDBTMPrintQR.cshtml";

        public DBTMPrintQRController(IDBTMPrintQRAgent dBTMPrintQRAgent)
        {
            _dBTMPrintQRAgent = dBTMPrintQRAgent;
        }

        public ActionResult List(DataTableViewModel dataTableModel)
        {
            GetListOnlyIfSingleCentre(dataTableModel);

            DBTMPrintQRListViewModel model = new DBTMPrintQRListViewModel
            {
                SelectedCentreCode = "SVTDKSZO"
            };
            return View("~/Views/DBTM/DBTMPrintQR/DBTMPrintQRList.cshtml", model);
        }

        public virtual ActionResult GetDBTMPrintQRTraineeList(DataTableViewModel dataTableViewModel)
        {
            DBTMPrintQRListViewModel list = _dBTMPrintQRAgent.GetDBTMPrintQRTraineeList(Convert.ToInt32(dataTableViewModel.SelectedParameter1), dataTableViewModel);

            list.SelectedParameter1 = dataTableViewModel.SelectedParameter1;

            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMPrintQR/_DBTMPrintQRList.cshtml", list);
            }

            return View("~/Views/DBTM/DBTMPrintQR/DBTMPrintQRList.cshtml", list);
        }

        [HttpPost]
        public ActionResult GetDBTMPrintQR(string personIds)
        {
            DBTMPrintQRListViewModel model = _dBTMPrintQRAgent.GetDBTMPrintQR(personIds);
            return Json(model);
        }

        #region Protected
        #endregion
    }
}
