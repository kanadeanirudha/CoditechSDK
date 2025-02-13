using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Resources;

using Microsoft.AspNetCore.Mvc;

namespace Coditech.Admin.Controllers
{
    public class DBTMDeviceMasterController : BaseController
    {
        private readonly IDBTMDeviceAgent _dBTMDeviceAgent;
        private const string createEdit = "~/Views/DBTM/DBTMDeviceMaster/CreateEdit.cshtml";

        public DBTMDeviceMasterController(IDBTMDeviceAgent dBTMDeviceAgent)
        {
            _dBTMDeviceAgent = dBTMDeviceAgent;
        }

        public virtual ActionResult List(DataTableViewModel dataTableModel)
        {
            DBTMDeviceListViewModel list = _dBTMDeviceAgent.GetDBTMDeviceList(dataTableModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMDeviceMaster/_List.cshtml", list);
            }
            return View($"~/Views/DBTM/DBTMDeviceMaster/List.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            return View(createEdit, new DBTMDeviceViewModel());
        }

        [HttpPost]
        public virtual ActionResult Create(DBTMDeviceViewModel dBTMDeviceViewModel)
        {
            if (ModelState.IsValid)
            {
                dBTMDeviceViewModel = _dBTMDeviceAgent.CreateDBTMDevice(dBTMDeviceViewModel);
                if (!dBTMDeviceViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    return RedirectToAction("List", CreateActionDataTable());
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMDeviceViewModel.ErrorMessage));
            return View(createEdit, dBTMDeviceViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(long dBTMDeviceId)
        {
            DBTMDeviceViewModel dBTMDeviceViewModel = _dBTMDeviceAgent.GetDBTMDevice(dBTMDeviceId);
            return ActionView(createEdit, dBTMDeviceViewModel);
        }

        [HttpPost]
        public virtual ActionResult Edit(DBTMDeviceViewModel dBTMDeviceViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_dBTMDeviceAgent.UpdateDBTMDevice(dBTMDeviceViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                return RedirectToAction("Edit", new { dBTMDeviceId = dBTMDeviceViewModel.DBTMDeviceMasterId });
            }
            return View(createEdit, dBTMDeviceViewModel);
        }

        public virtual ActionResult Delete(string dBTMDeviceIds)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(dBTMDeviceIds))
            {
                status = _dBTMDeviceAgent.DeleteDBTMDevice(dBTMDeviceIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction<DBTMDeviceMasterController>(x => x.List(null));
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction<DBTMDeviceMasterController>(x => x.List(null));
        }

        public virtual ActionResult Cancel()
        {
            DataTableViewModel dataTableViewModel = new DataTableViewModel();
            return RedirectToAction("List", dataTableViewModel);
        }
        #region Protected

        #endregion
    }
}