using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Coditech.Admin.Controllers
{
    public class DBTMBatchActivityController : BaseController
    {
        private readonly IDBTMBatchActivityAgent _dBTMBatchActivityAgent;
        private const string createDBTMBatchActivity = "~/Views/DBTM/DBTMBatchActivity/CreateDBTMBatchActivity.cshtml";

        public DBTMBatchActivityController(IDBTMBatchActivityAgent dBTMBatchActivityAgent)
        {
            _dBTMBatchActivityAgent = dBTMBatchActivityAgent;
        }

        public virtual ActionResult List(DataTableViewModel dataTableModel)
        {
            GeneralBatchListViewModel list = new GeneralBatchListViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SelectedCentreCode))
            {
                list = _dBTMBatchActivityAgent.GetBatchList(dataTableModel);
            }
            list.SelectedCentreCode = dataTableModel.SelectedCentreCode;
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMBatchActivity/_List.cshtml", list);
            }
            return View($"~/Views/DBTM/DBTMBatchActivity/List.cshtml", list);
        }

        public virtual ActionResult GetDBTMBatchActivityList(DataTableViewModel dataTableViewModel)
        {
            DBTMBatchActivityListViewModel list = _dBTMBatchActivityAgent.GetDBTMBatchActivityList(Convert.ToInt32(dataTableViewModel.SelectedParameter1), dataTableViewModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMBatchActivity/_DBTMBatchActivityList.cshtml", list);
            }
            list.SelectedParameter1 = dataTableViewModel.SelectedParameter1;

            return View($"~/Views/DBTM/DBTMBatchActivity/DBTMBatchActivityList.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult CreateDBTMBatchActivity(int generalBatchMasterId)
        {
            DBTMBatchActivityViewModel dBTMBatchActivityViewModel = _dBTMBatchActivityAgent.DBTMBatchActivity(generalBatchMasterId);
            return View(createDBTMBatchActivity, dBTMBatchActivityViewModel);
        }

        [HttpPost]
        public virtual ActionResult CreateDBTMBatchActivity(DBTMBatchActivityViewModel dBTMBatchActivityViewModel)
        {
            if (ModelState.IsValid)
            {
                dBTMBatchActivityViewModel = _dBTMBatchActivityAgent.CreateDBTMBatchActivity(dBTMBatchActivityViewModel);
                if (!dBTMBatchActivityViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    return RedirectToAction("GetDBTMBatchActivityList", new { SelectedParameter1 = dBTMBatchActivityViewModel.GeneralBatchMasterId });
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMBatchActivityViewModel.ErrorMessage));
            return View(createDBTMBatchActivity, dBTMBatchActivityViewModel);
        }

        public virtual ActionResult Delete(string dBTMBatchActivityIds,string generalBatchMasterId)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(dBTMBatchActivityIds))
            {
                status = _dBTMBatchActivityAgent.DeleteDBTMBatchActivity(dBTMBatchActivityIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction("GetDBTMBatchActivityList", new { SelectedParameter1 = generalBatchMasterId });

            }
            
            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction("GetDBTMBatchActivityList", new { SelectedParameter1 = generalBatchMasterId });
        }

        #region Protected
        #endregion
    }
}