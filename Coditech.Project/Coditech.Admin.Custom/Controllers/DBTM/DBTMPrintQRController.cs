using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Coditech.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            UserModel user = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
            DBTMPrintQRListViewModel model = new DBTMPrintQRListViewModel();
            model.SelectedCentreCode = user.SelectedCentreCode;
            if (!string.IsNullOrWhiteSpace(user.Custom3))
            {
                DBTMCustomUserModel customUser = JsonConvert.DeserializeObject<DBTMCustomUserModel>(user.Custom3);
                if (customUser != null)
                {
                    model.GeneralTrainerMasterId = customUser.GeneralTrainerMasterId ?? 0;
                }
            }
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

        [HttpGet]
        public JsonResult CheckPrintQRAvailability(string personIds)
        {
            if (string.IsNullOrWhiteSpace(personIds))
            {
                return Json(new { success = false, message = "No trainee selected." });
            }
            return Json(new { success = true });
        }

        public ActionResult DownloadPrintQR(string personIds)
        {
            DBTMPrintQRListViewModel model = _dBTMPrintQRAgent.DownloadPrintQR(personIds);
            byte[] bytes = System.IO.File.ReadAllBytes(model.FilePath);
            System.IO.File.Delete(model.FilePath);
            Response.Cookies.Append("PrintQRDownload", "Completed", new CookieOptions { Path = "/" });
            return File(bytes, "application/pdf", model.FileName);
        }

        #region Protected
        #endregion
    }
}
