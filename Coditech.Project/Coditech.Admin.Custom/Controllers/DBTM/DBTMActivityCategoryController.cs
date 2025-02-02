using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Resources;

using Microsoft.AspNetCore.Mvc;

namespace Coditech.Admin.Controllers
{
    public class DBTMActivityCategoryController : BaseController
    {
        private readonly IDBTMActivityCategoryAgent _dBTMActivityCategoryAgent;
        private const string createEdit = "~/Views/DBTM/DBTMActivityCategory/CreateEdit.cshtml";

        public DBTMActivityCategoryController(IDBTMActivityCategoryAgent dBTMActivityCategoryAgent)
        {
            _dBTMActivityCategoryAgent = dBTMActivityCategoryAgent;
        }

        public virtual ActionResult List(DataTableViewModel dataTableModel)
        {
            DBTMActivityCategoryListViewModel list = _dBTMActivityCategoryAgent.GetDBTMActivityCategoryList(dataTableModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMActivityCategory/_List.cshtml", list);
            }
            return View($"~/Views/DBTM/DBTMActivityCategory/List.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            return View(createEdit, new DBTMActivityCategoryViewModel());
        }

        [HttpPost]
        public virtual ActionResult Create(DBTMActivityCategoryViewModel dBTMActivityCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                dBTMActivityCategoryViewModel = _dBTMActivityCategoryAgent.CreateDBTMActivityCategory(dBTMActivityCategoryViewModel);
                if (!dBTMActivityCategoryViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    return RedirectToAction("List", CreateActionDataTable());
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMActivityCategoryViewModel.ErrorMessage));
            return View(createEdit, dBTMActivityCategoryViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(short dBTMActivityCategoryId)
        {
            DBTMActivityCategoryViewModel dBTMActivityCategoryViewModel = _dBTMActivityCategoryAgent.GetDBTMActivityCategory(dBTMActivityCategoryId);
            return ActionView(createEdit, dBTMActivityCategoryViewModel);
        }

        [HttpPost]
        public virtual ActionResult Edit(DBTMActivityCategoryViewModel dBTMActivityCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_dBTMActivityCategoryAgent.UpdateDBTMActivityCategory(dBTMActivityCategoryViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                return RedirectToAction("Edit", new { DBTMActivityCategoryId = dBTMActivityCategoryViewModel.DBTMActivityCategoryId });
            }
            return View(createEdit, dBTMActivityCategoryViewModel);
        }

        public virtual ActionResult Delete(string dBTMActivityCategoryIds)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(dBTMActivityCategoryIds))
            {
                status = _dBTMActivityCategoryAgent.DeleteDBTMActivityCategory(dBTMActivityCategoryIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction<DBTMActivityCategoryController>(x => x.List(null));
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction<DBTMActivityCategoryController>(x => x.List(null));
        }

        #region Protected

        #endregion
    }
}