using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Resources;

using Microsoft.AspNetCore.Mvc;

namespace Coditech.Admin.Controllers
{
    public class DBTMPrivacySettingController : BaseController
    {
        private readonly IDBTMPrivacySettingAgent _dBTMPrivacySettingAgent;
        private const string createEdit = "~/Views/DBTM/DBTMPrivacySetting/CreateEdit.cshtml";

        public DBTMPrivacySettingController(IDBTMPrivacySettingAgent dBTMPrivacySettingAgent)
        {
            _dBTMPrivacySettingAgent = dBTMPrivacySettingAgent;
        }
        public virtual ActionResult List(DataTableViewModel dataTableModel)
        {
            DBTMPrivacySettingListViewModel list = new DBTMPrivacySettingListViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SelectedCentreCode))
            {
                list = _dBTMPrivacySettingAgent.GetDBTMPrivacySettingList(dataTableModel);
            }
            list.SelectedCentreCode = dataTableModel.SelectedCentreCode;
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMPrivacySetting/_List.cshtml", list);
            }
            return View($"~/Views/DBTM/DBTMPrivacySetting/List.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            DBTMPrivacySettingViewModel dBTMPrivacySettingViewModel = new DBTMPrivacySettingViewModel();
            return View(createEdit, dBTMPrivacySettingViewModel);
        }

        [HttpPost]
        public virtual ActionResult Create(DBTMPrivacySettingViewModel dBTMPrivacySettingViewModel)
        {
            if (ModelState.IsValid)
            {
                dBTMPrivacySettingViewModel = _dBTMPrivacySettingAgent.CreateDBTMPrivacySetting(dBTMPrivacySettingViewModel);
                if (!dBTMPrivacySettingViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    return RedirectToAction("List", new DataTableViewModel { SelectedCentreCode = dBTMPrivacySettingViewModel.CentreCode });
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMPrivacySettingViewModel.ErrorMessage));
            return View(createEdit, dBTMPrivacySettingViewModel);
        }
        [HttpGet]
        public virtual ActionResult Edit(int dBTMPrivacySettingId)
        {
            DBTMPrivacySettingViewModel dBTMPrivacySettingViewModel = _dBTMPrivacySettingAgent.GetDBTMPrivacySetting(dBTMPrivacySettingId);
            return ActionView(createEdit, dBTMPrivacySettingViewModel);
        }

        [HttpPost]
        public virtual ActionResult Edit(DBTMPrivacySettingViewModel dBTMPrivacySettingViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_dBTMPrivacySettingAgent.UpdateDBTMPrivacySetting(dBTMPrivacySettingViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                return RedirectToAction("Edit", new { dBTMPrivacySettingId = dBTMPrivacySettingViewModel.DBTMPrivacySettingId });
            }
            return View(createEdit, dBTMPrivacySettingViewModel);
        }

        public virtual ActionResult Delete(string dBTMPrivacySettingIds)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(dBTMPrivacySettingIds))
            {
                status = _dBTMPrivacySettingAgent.DeleteDBTMPrivacySetting(dBTMPrivacySettingIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction("List", CreateActionDataTable());
            }
            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction("List", CreateActionDataTable());
        }
        public virtual ActionResult Cancel(string SelectedCentreCode)
        {
            DataTableViewModel dataTableViewModel = new DataTableViewModel() { SelectedCentreCode = SelectedCentreCode };
            return RedirectToAction("List", dataTableViewModel);
        }
    }
}
