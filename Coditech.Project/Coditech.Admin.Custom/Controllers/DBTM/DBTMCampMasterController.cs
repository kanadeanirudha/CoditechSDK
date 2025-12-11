using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.Helper.Utilities;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Coditech.Admin.Controllers
{
    public class DBTMCampMasterController : BaseController
    {
        private readonly IDBTMCampAgent _dBTMCampAgent;
        private const string createEdit = "~/Views/DBTM/DBTMCampMaster/CreateEdit.cshtml";

        public DBTMCampMasterController(IDBTMCampAgent dBTMCampAgent)
        {
            _dBTMCampAgent = dBTMCampAgent;
        }

        public virtual ActionResult List(DataTableViewModel dataTableModel)
        {
            DBTMCampListViewModel list = new DBTMCampListViewModel();
            GetListOnlyIfSingleCentre(dataTableModel);
            if (!string.IsNullOrEmpty(dataTableModel.SelectedCentreCode))
            {
                list = _dBTMCampAgent.GetDBTMCampList(dataTableModel);
            }
            list.SelectedCentreCode = dataTableModel.SelectedCentreCode;
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMCampMaster/_List.cshtml", list);
            }
            return View($"~/Views/DBTM/DBTMCampMaster/List.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            DBTMCampMasterViewModel dBTMCampMasterViewModel = new DBTMCampMasterViewModel();
            return View(createEdit, dBTMCampMasterViewModel);
        }

        [HttpPost]
        public virtual ActionResult Create(DBTMCampMasterViewModel dBTMCampMasterViewModel)
        {
            if (ModelState.IsValid)
            {
                dBTMCampMasterViewModel = _dBTMCampAgent.CreateDBTMCamp(dBTMCampMasterViewModel);
                if (!dBTMCampMasterViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    if (string.Equals(dBTMCampMasterViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction(AdminConstants.ActionRedirectToEdit, new { dBTMCampMasterId = dBTMCampMasterViewModel.DBTMCampMasterId });
                    }
                    else if (string.Equals(dBTMCampMasterViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction(AdminConstants.ActionRedirectToList);
                    }
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMCampMasterViewModel.ErrorMessage));
            return View(createEdit, dBTMCampMasterViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(long dBTMCampMasterId)
        {
            DBTMCampMasterViewModel dBTMCampMasterViewModel = _dBTMCampAgent.GetDBTMCamp(dBTMCampMasterId);
            return ActionView(createEdit, dBTMCampMasterViewModel);
        }

        [HttpPost]
        public virtual ActionResult Edit(DBTMCampMasterViewModel dBTMCampMasterViewModel)
        {
            if (ModelState.IsValid)
            {
                dBTMCampMasterViewModel = _dBTMCampAgent.UpdateDBTMCamp(dBTMCampMasterViewModel);
                SetNotificationMessage(dBTMCampMasterViewModel.HasError
                ? GetErrorNotificationMessage(dBTMCampMasterViewModel.ErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                if (string.Equals(dBTMCampMasterViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction( AdminConstants.ActionRedirectToEdit, new { dBTMCampMasterId = dBTMCampMasterViewModel.DBTMCampMasterId });
                }
                else if (string.Equals(dBTMCampMasterViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction(AdminConstants.ActionRedirectToList);
                }
            }
            return View(createEdit, dBTMCampMasterViewModel);
        }

        public virtual ActionResult Cancel(string SelectedCentreCode)
        {
            DataTableViewModel dataTableViewModel = new DataTableViewModel() { SelectedCentreCode = SelectedCentreCode };
            return RedirectToAction("List", dataTableViewModel);
        }

        public virtual ActionResult Delete(string dBTMCampMasterIds)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(dBTMCampMasterIds))
            {
                status = _dBTMCampAgent.DeleteDBTMCamp(dBTMCampMasterIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction<DBTMCampMasterController>(x => x.List(null));
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction<DBTMCampMasterController>(x => x.List(null));
        }

        public virtual ActionResult GetDBTMCampUserList(DataTableViewModel dataTableViewModel)
        {
            DBTMCampUserListViewModel list = _dBTMCampAgent.GetDBTMCampUserList(Convert.ToInt64(dataTableViewModel.SelectedParameter1), Convert.ToString(dataTableViewModel.SelectedParameter2), dataTableViewModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMCampMaster/DBTMCampUser/_AssociatedCampList.cshtml", list);
            }
            list.SelectedParameter1 = dataTableViewModel.SelectedParameter1;
            list.SelectedParameter2 = dataTableViewModel.SelectedParameter2;
            return View($"~/Views/DBTM/DBTMCampMaster/DBTMCampUser/AssociatedCampList.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult GetAssociateUnAssociateCampwiseUser(DBTMCampUserViewModel dBTMCampUserViewModel)
        {
            return PartialView("~/Views/DBTM/DBTMCampMaster/DBTMCampUser/_AssociateUnAssociateCampwiseUser.cshtml", dBTMCampUserViewModel);
        }

        [HttpPost]
        public virtual ActionResult AssociateUnAssociateCampwiseUser(DBTMCampUserViewModel dBTMCampUserViewModel)
        {
            SetNotificationMessage(_dBTMCampAgent.AssociateUnAssociateCampwiseUser(dBTMCampUserViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
            return RedirectToAction("GetDBTMCampUserList", new DataTableViewModel { SelectedParameter1 = dBTMCampUserViewModel.DBTMCampMasterId.ToString(), SelectedParameter2 = dBTMCampUserViewModel.UserType });
        }

        #region Protected
        #endregion
    }
}