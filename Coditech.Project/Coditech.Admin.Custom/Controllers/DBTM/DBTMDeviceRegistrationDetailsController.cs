using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Resources;

using Microsoft.AspNetCore.Mvc;

namespace Coditech.Admin.Controllers
{
    public class DBTMDeviceRegistrationDetailsController : BaseController
    {
        private readonly IDBTMDeviceRegistrationDetailsAgent _dBTMDeviceRegistrationDetailsAgent;
        private const string createEdit = "~/Views/DBTM/DBTMDeviceRegistrationDetails/AddNewDevice.cshtml";

        public DBTMDeviceRegistrationDetailsController(IDBTMDeviceRegistrationDetailsAgent dBTMDeviceRegistrationDetailsAgent)
        {
            _dBTMDeviceRegistrationDetailsAgent = dBTMDeviceRegistrationDetailsAgent;
        }

        public virtual ActionResult List(DataTableViewModel dataTableModel)
        {
            DBTMDeviceRegistrationDetailsListViewModel list = _dBTMDeviceRegistrationDetailsAgent.GetDBTMDeviceRegistrationDetailsList(dataTableModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMDeviceRegistrationDetails/_List.cshtml", list);
            }
            return View($"~/Views/DBTM/DBTMDeviceRegistrationDetails/List.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            return View(createEdit, new DBTMDeviceRegistrationDetailsViewModel());
        }

        [HttpPost]
        public virtual ActionResult Create(DBTMDeviceRegistrationDetailsViewModel dBTMDeviceRegistrationDetailsViewModel)
        {
            if (ModelState.IsValid)
            {
                dBTMDeviceRegistrationDetailsViewModel = _dBTMDeviceRegistrationDetailsAgent.CreateRegistrationDetails(dBTMDeviceRegistrationDetailsViewModel);
                if (!dBTMDeviceRegistrationDetailsViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    return RedirectToAction("List", CreateActionDataTable());
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMDeviceRegistrationDetailsViewModel.ErrorMessage));
            return View(createEdit, dBTMDeviceRegistrationDetailsViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(long dBTMDeviceRegistrationDetailId)
        {
            DBTMDeviceRegistrationDetailsViewModel dBTMDeviceRegistrationDetailsViewModel = _dBTMDeviceRegistrationDetailsAgent.GetRegistrationDetails(dBTMDeviceRegistrationDetailId);
            return ActionView(createEdit, dBTMDeviceRegistrationDetailsViewModel);
        }

        [HttpPost]
        public virtual ActionResult Edit(DBTMDeviceRegistrationDetailsViewModel dBTMDeviceRegistrationDetailsViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_dBTMDeviceRegistrationDetailsAgent.UpdateRegistrationDetails(dBTMDeviceRegistrationDetailsViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                return RedirectToAction("Edit", new { dBTMDeviceRegistrationDetailId = dBTMDeviceRegistrationDetailsViewModel.DBTMDeviceRegistrationDetailId });
            }
            return View(createEdit, dBTMDeviceRegistrationDetailsViewModel);
        }

        public virtual ActionResult Delete(string dBTMDeviceRegistrationDetailIds)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(dBTMDeviceRegistrationDetailIds))
            {
                status = _dBTMDeviceRegistrationDetailsAgent.DeleteRegistrationDetails(dBTMDeviceRegistrationDetailIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction<DBTMDeviceRegistrationDetailsController>(x => x.List(null));
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction<DBTMDeviceRegistrationDetailsController>(x => x.List(null));
        }

        #region Protected

        #endregion
    }
}