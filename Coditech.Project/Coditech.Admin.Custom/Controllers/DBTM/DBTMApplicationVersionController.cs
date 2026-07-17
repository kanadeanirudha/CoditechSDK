using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc;
namespace Coditech.Admin.Controllers
{
    public class DBTMApplicationVersionController : BaseController
    {
        private readonly IDBTMApplicationVersionAgent _dBTMApplicationVersionAgent;
        private const string createEdit = "~/Views/DBTM/DBTMApplicationVersion/CreateEdit.cshtml";

        public DBTMApplicationVersionController(IDBTMApplicationVersionAgent dBTMApplicationVersionAgent)
        {
            _dBTMApplicationVersionAgent = dBTMApplicationVersionAgent;
        }

        public virtual ActionResult List(DataTableViewModel dataTableModel)
        {
            DBTMApplicationVersionListViewModel list = _dBTMApplicationVersionAgent.GetDBTMApplicationVersionList(dataTableModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMApplicationVersion/_List.cshtml", list);
            }
            return View($"~/Views/DBTM/DBTMApplicationVersion/List.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            return View(createEdit, new DBTMApplicationVersionViewModel());
        }

        [HttpPost]
        public virtual ActionResult Create(DBTMApplicationVersionViewModel dBTMApplicationVersionViewModel)
        {
            if (ModelState.IsValid)
            {
                dBTMApplicationVersionViewModel = _dBTMApplicationVersionAgent.CreateDBTMApplicationVersion(dBTMApplicationVersionViewModel);
                if (!dBTMApplicationVersionViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    if (string.Equals(dBTMApplicationVersionViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction(AdminConstants.ActionRedirectToEdit, new { dBTMApplicationVersionId = dBTMApplicationVersionViewModel.DBTMApplicationVersionId });
                    }
                    else if (string.Equals(dBTMApplicationVersionViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction(AdminConstants.ActionRedirectToList);
                    }
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMApplicationVersionViewModel.ErrorMessage));
            return View(createEdit, dBTMApplicationVersionViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(long dBTApplicationVersionId)
        {
            DBTMApplicationVersionViewModel dBTMApplicationVersionViewModel = _dBTMApplicationVersionAgent.GetDBTMApplicationVersion(dBTApplicationVersionId);
            return ActionView(createEdit, dBTMApplicationVersionViewModel);
        }

        [HttpPost]
        public virtual ActionResult Edit(DBTMApplicationVersionViewModel dBTMApplicationVersionViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_dBTMApplicationVersionAgent.UpdateDBTMApplicationVersion(dBTMApplicationVersionViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                if (string.Equals(dBTMApplicationVersionViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction(AdminConstants.ActionRedirectToEdit, new { dBTMApplicationVersionId = dBTMApplicationVersionViewModel.DBTMApplicationVersionId });
                }
                else if (string.Equals(dBTMApplicationVersionViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction(AdminConstants.ActionRedirectToList);
                }
            }
            return View(createEdit, dBTMApplicationVersionViewModel);
        }

        public virtual ActionResult Delete(string dBTMApplicationVersionIds)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(dBTMApplicationVersionIds))
            {
                status = _dBTMApplicationVersionAgent.DeleteDBTMApplicationVersion(dBTMApplicationVersionIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction<DBTMApplicationVersionController>(x => x.List(null));
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction<DBTMApplicationVersionController>(x => x.List(null));
        }

        #region Protected

        #endregion
    }
}