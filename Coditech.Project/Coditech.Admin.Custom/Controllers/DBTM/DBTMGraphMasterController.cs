using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.Helper.Utilities;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Coditech.Admin.Controllers
{
    public class DBTMGraphMasterController : BaseController
    {
        private readonly IDBTMGraphAgent _dBTMGraphAgent;
        private const string createEdit = "~/Views/DBTM/DBTMGraphMaster/CreateEdit.cshtml";

        public DBTMGraphMasterController(IDBTMGraphAgent dBTMGraphAgent)
        {
            _dBTMGraphAgent = dBTMGraphAgent;
        }

        public virtual ActionResult List(DataTableViewModel dataTableModel)
        {
            DBTMGraphMasterListViewModel list = _dBTMGraphAgent.GetDBTMGraphList(dataTableModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMGraphMaster/_List.cshtml", list);
            }
            return View($"~/Views/DBTM/DBTMGraphMaster/List.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            DBTMGraphMasterViewModel dBTMGraphMasterViewModel = new DBTMGraphMasterViewModel();
            BindDBTMGraphTestCode(dBTMGraphMasterViewModel);
            return View(createEdit, dBTMGraphMasterViewModel);
        }

        [HttpPost]
        public virtual ActionResult Create(DBTMGraphMasterViewModel dBTMGraphMasterViewModel)
        {
            if (ModelState.IsValid)
            {
                dBTMGraphMasterViewModel = _dBTMGraphAgent.CreateDBTMGraph(dBTMGraphMasterViewModel);
                if (!dBTMGraphMasterViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    if (string.Equals(dBTMGraphMasterViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction(AdminConstants.ActionRedirectToEdit, new { graphCode = dBTMGraphMasterViewModel.GraphCode });
                    }
                    else if (string.Equals(dBTMGraphMasterViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction(AdminConstants.ActionRedirectToList);
                    }
                }
            }
            BindDBTMGraphTestCode(dBTMGraphMasterViewModel);
            SetNotificationMessage(GetErrorNotificationMessage(dBTMGraphMasterViewModel.ErrorMessage));
            return View(createEdit, dBTMGraphMasterViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(string graphCode)
        {
            DBTMGraphMasterViewModel dBTMGraphMasterViewModel = _dBTMGraphAgent.GetDBTMGraph(graphCode);
            BindDBTMGraphTestCode(dBTMGraphMasterViewModel);
            return ActionView(createEdit, dBTMGraphMasterViewModel);
        }

        [HttpPost]
        public virtual ActionResult Edit(DBTMGraphMasterViewModel dBTMGraphMasterViewModel)
        {
            if (ModelState.IsValid)
            {
                dBTMGraphMasterViewModel = _dBTMGraphAgent.UpdateDBTMGraph(dBTMGraphMasterViewModel);
                SetNotificationMessage(dBTMGraphMasterViewModel.HasError
                ? GetErrorNotificationMessage(dBTMGraphMasterViewModel.ErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                if (string.Equals(dBTMGraphMasterViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction(AdminConstants.ActionRedirectToEdit, new { graphCode = dBTMGraphMasterViewModel.GraphCode });
                }
                else if (string.Equals(dBTMGraphMasterViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction(AdminConstants.ActionRedirectToList);
                }
            }
            BindDBTMGraphTestCode(dBTMGraphMasterViewModel);
            return View(createEdit, dBTMGraphMasterViewModel);
        }

        public virtual ActionResult Delete(string graphCode)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(graphCode))
            {
                status = _dBTMGraphAgent.DeleteDBTMGraph(graphCode, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction<DBTMGraphMasterController>(x => x.List(null));
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction<DBTMGraphMasterController>(x => x.List(null));
        }

        protected virtual void BindDBTMGraphTestCode(DBTMGraphMasterViewModel dBTMGraphMasterViewModel)
        {
            dBTMGraphMasterViewModel.DBTMTestList = dBTMGraphMasterViewModel.DBTMTestList ?? new List<SelectListItem>();
            DBTMTestListViewModel dBTMGraphMasterList = _dBTMGraphAgent.DBTMGraphTestCode();

            if (dBTMGraphMasterList?.DBTMTestList != null)
            {
                foreach (var item in dBTMGraphMasterList.DBTMTestList)
                {
                    dBTMGraphMasterViewModel.DBTMTestList.Add(new SelectListItem
                    {
                        Text = item.TestName,
                        Value = Convert.ToString(item.DBTMTestMasterId)
                    });
                }
            }
        }

        [HttpGet]
        public ActionResult GetDBTMGraphByDBTMTestMaster(int dBTMTestMasterId)
        {
            DropdownViewModel dBTMGraphByDBTMTestMaster = new DropdownViewModel()
            {
                DropdownType = DropdownCustomTypeEnum.DBTMGraph.ToString(),
                DropdownName = "DBTMGraphMasterId",
                Parameter = dBTMTestMasterId.ToString(),
                IsCustomDropdown = true
            };
            return PartialView("~/Views/Shared/Control/_DropdownList.cshtml", dBTMGraphByDBTMTestMaster);
        }
        #region Protected
        #endregion
    }
}