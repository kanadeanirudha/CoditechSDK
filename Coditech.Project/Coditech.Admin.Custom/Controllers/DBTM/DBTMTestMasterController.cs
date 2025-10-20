using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.Helper.Utilities;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Coditech.Admin.Controllers
{
    public class DBTMTestMasterController : BaseController
    {
        private readonly IDBTMTestAgent _dBTMTestAgent;
        private const string createEdit = "~/Views/DBTM/DBTMTestMaster/CreateEdit.cshtml";

        public DBTMTestMasterController(IDBTMTestAgent dBTMTestAgent)
        {
            _dBTMTestAgent = dBTMTestAgent;
        }

        public virtual ActionResult List(DataTableViewModel dataTableModel)
        {
            DBTMTestListViewModel list = _dBTMTestAgent.GetDBTMTestList(dataTableModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMTestMaster/_List.cshtml", list);
            }
            return View($"~/Views/DBTM/DBTMTestMaster/List.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            DBTMTestViewModel dBTMTestViewModel = new DBTMTestViewModel();
            BindDBTMTestParameter(dBTMTestViewModel);
            BindDBTMTestCalculation(dBTMTestViewModel);
            BindDBTMGraph(dBTMTestViewModel);
            return View(createEdit, dBTMTestViewModel);
        }

        [HttpPost]
        public virtual ActionResult Create(DBTMTestViewModel dBTMTestViewModel)
        {
            if (ModelState.IsValid)
            {
                dBTMTestViewModel = _dBTMTestAgent.CreateDBTMTest(dBTMTestViewModel);
                if (!dBTMTestViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    if (string.Equals(dBTMTestViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction(AdminConstants.ActionRedirectToEdit, new { dBTMTestMasterId = dBTMTestViewModel.DBTMTestMasterId });
                    }
                    else if (string.Equals(dBTMTestViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction(AdminConstants.ActionRedirectToList);
                    }
                }
            }
            BindDBTMTestParameter(dBTMTestViewModel);
            BindDBTMTestCalculation(dBTMTestViewModel);
            BindDBTMGraph(dBTMTestViewModel);
            SetNotificationMessage(GetErrorNotificationMessage(dBTMTestViewModel.ErrorMessage));
            return View(createEdit, dBTMTestViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(int dBTMTestMasterId)
        {
            DBTMTestViewModel dBTMTestViewModel = _dBTMTestAgent.GetDBTMTest(dBTMTestMasterId);
            BindDBTMTestParameter(dBTMTestViewModel);
            BindDBTMTestCalculation(dBTMTestViewModel);
            BindDBTMGraph(dBTMTestViewModel);
            return ActionView(createEdit, dBTMTestViewModel);
        }

        [HttpPost]
        public virtual ActionResult Edit(DBTMTestViewModel dBTMTestViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_dBTMTestAgent.UpdateDBTMTest(dBTMTestViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                if (string.Equals(dBTMTestViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction(AdminConstants.ActionRedirectToEdit, new { dBTMTestMasterId = dBTMTestViewModel.DBTMTestMasterId });
                }
                else if (string.Equals(dBTMTestViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction(AdminConstants.ActionRedirectToList);
                }
            }
            BindDBTMTestParameter(dBTMTestViewModel);
            BindDBTMTestCalculation(dBTMTestViewModel);
            BindDBTMGraph(dBTMTestViewModel);
            return View(createEdit, dBTMTestViewModel);
        }

        public virtual ActionResult Delete(string dBTMTestMasterIds)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(dBTMTestMasterIds))
            {
                status = _dBTMTestAgent.DeleteDBTMTest(dBTMTestMasterIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction<DBTMTestMasterController>(x => x.List(null));
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction<DBTMTestMasterController>(x => x.List(null));
        }

        #region Protected
        protected virtual void BindDBTMTestParameter(DBTMTestViewModel dBTMTestViewModel)
        {
            dBTMTestViewModel.DBTMTestParameterList = dBTMTestViewModel.DBTMTestParameterList ?? new List<SelectListItem>();
            DBTMTestParameterListViewModel parameterList = _dBTMTestAgent.DBTMTestParameter();

            if (parameterList?.DBTMTestParameterList != null)
            {
                foreach (var item in parameterList.DBTMTestParameterList)
                {
                    dBTMTestViewModel.DBTMTestParameterList.Add(new SelectListItem
                    {
                        Text = item.ParameterName,
                        Value = item.DBTMTestParameterId.ToString()
                    });
                }
            }
        }
        protected virtual void BindDBTMTestCalculation(DBTMTestViewModel dBTMTestViewModel)
        {
            dBTMTestViewModel.DBTMTestCalculationList = dBTMTestViewModel.DBTMTestCalculationList ?? new List<SelectListItem>();
            DBTMTestCalculationListViewModel parameterList = _dBTMTestAgent.DBTMTestCalculation();

            if (parameterList?.DBTMTestCalculationList != null)
            {
                foreach (var item in parameterList.DBTMTestCalculationList)
                {
                    dBTMTestViewModel.DBTMTestCalculationList.Add(new SelectListItem
                    {
                        Text = item.CalculationName,
                        Value = item.DBTMTestCalculationId.ToString()
                    });
                }
            }
        }
        protected virtual void BindDBTMGraph(DBTMTestViewModel dBTMTestViewModel)
        {
            dBTMTestViewModel.DBTMGraphMasterList = dBTMTestViewModel.DBTMGraphMasterList ?? new List<SelectListItem>();
            DBTMGraphMasterListViewModel dBTMGraphMasterList = _dBTMTestAgent.DBTMGraph();

            if (dBTMGraphMasterList?.DBTMGraphMasterList != null)
            {
                foreach (var item in dBTMGraphMasterList.DBTMGraphMasterList)
                {
                    dBTMTestViewModel.DBTMGraphMasterList.Add(new SelectListItem
                    {
                        Text = item.GraphName,
                        Value = item.DBTMGraphMasterId.ToString()
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

        #endregion
    }
}