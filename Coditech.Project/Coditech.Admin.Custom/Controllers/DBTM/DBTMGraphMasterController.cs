using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
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
        public virtual ActionResult GraphVerticalViewSequenceList( DataTableViewModel dataTableViewModel)
        {
            DBTMGraphVerticalViewSequenceListViewModel list = _dBTMGraphAgent.GetGraphVerticalViewSequenceList( Convert.ToInt32(dataTableViewModel.SelectedParameter1), dataTableViewModel);
            list.SelectedParameter1 = dataTableViewModel.SelectedParameter1;
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView( "~/Views/DBTM/DBTMGraphMaster/GraphVerticalViewSequence/_GraphVerticalViewSequenceList.cshtml", list);
            }
            return View( "~/Views/DBTM/DBTMGraphMaster/GraphVerticalViewSequence/GraphVerticalViewSequenceList.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult GraphVerticalViewSequence( int dBTMGraphVerticalViewSequenceId)
        {
            DBTMGraphVerticalViewSequenceViewModel model = _dBTMGraphAgent.GetGraphVerticalViewSequence( dBTMGraphVerticalViewSequenceId);
            return View( "~/Views/DBTM/DBTMGraphMaster/GraphVerticalViewSequence/DBTMGraphVerticalViewSequence.cshtml",  model);
        }
        [HttpPost]
        public virtual ActionResult GraphVerticalViewSequence( DBTMGraphVerticalViewSequenceViewModel model)
        {
            if (ModelState.IsValid)
            {
                model = _dBTMGraphAgent.UpdateGraphVerticalViewSequence(model);
                SetNotificationMessage( model.HasError ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage) : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                if (string.Equals( model.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("GraphVerticalViewSequence", new { dBTMGraphVerticalViewSequenceId = model.DBTMGraphVerticalViewSequenceId });
                }
                else if (string.Equals( model.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction( "GraphVerticalViewSequenceList", new DataTableViewModel { SelectedParameter1 = Convert.ToString( model.DBTMGraphMasterId) });
                }
            }
            return View( "~/Views/DBTM/DBTMGraphMaster/GraphVerticalViewSequence/DBTMGraphVerticalViewSequence.cshtml", model);
        }

        [HttpGet]
        public ActionResult UpdateGraphVerticalSequenceNumber( int dBTMGraphMasterId)
        {
            DBTMGraphVerticalViewSequenceListViewModel listViewModel = _dBTMGraphAgent.GetGraphVerticalViewSequenceList( dBTMGraphMasterId, new DataTableViewModel());
            var modelList = listViewModel.DBTMGraphVerticalViewSequenceList
                .Select(x => new DBTMGraphVerticalViewSequenceModel
                {
                    DBTMGraphVerticalViewSequenceId = x.DBTMGraphVerticalViewSequenceId,
                    DBTMGraphMasterId = x.DBTMGraphMasterId,
                    ParameterCode = x.ParameterCode,
                    IsCalculatedParameter = x.IsCalculatedParameter,
                    SequenceNumber = x.SequenceNumber
                })
                .ToList();

            DBTMGraphVerticalViewSequenceViewModel viewModel = new DBTMGraphVerticalViewSequenceViewModel
                {
                    DBTMGraphMasterId = dBTMGraphMasterId,
                    DBTMGraphVerticalViewSequenceList = modelList
                };
            return PartialView("~/Views/DBTM/DBTMGraphMaster/GraphVerticalViewSequence/_AddGraphVerticalSequenceNumberPopUp.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult UpdateGraphVerticalSequenceNumber( DBTMGraphVerticalViewSequenceViewModel listViewModel)
        {
            ModelState.Remove( nameof(DBTMGraphVerticalViewSequenceViewModel.Recursion));
            if (ModelState.IsValid)
            {
                listViewModel = _dBTMGraphAgent.UpdateGraphVerticalSequenceNumber( listViewModel);
                if (!listViewModel.HasError)
                {
                    SetNotificationMessage( GetSuccessNotificationMessage( "Sequence Number Saved Successfully."));
                    return Json(new { success = true });
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage("Failed to Save Sequence Number."));
            return Json(new { success = false });
        }

        [HttpGet]
        public virtual ActionResult CreateGraphVerticalViewSequence(int dBTMGraphMasterId)
        {
            DBTMGraphVerticalViewSequenceListViewModel listViewModel = _dBTMGraphAgent.GetGraphVerticalViewSequenceList( dBTMGraphMasterId, new DataTableViewModel());
            int maxSequence = 0;
            if (listViewModel?.DBTMGraphVerticalViewSequenceList != null &&
                listViewModel.DBTMGraphVerticalViewSequenceList.Count > 0)
            {
                maxSequence = listViewModel.DBTMGraphVerticalViewSequenceList.Max(x => x.SequenceNumber);
            }
            DBTMGraphVerticalViewSequenceViewModel newViewModel = new DBTMGraphVerticalViewSequenceViewModel
                {
                    DBTMGraphMasterId = dBTMGraphMasterId,
                    SequenceNumber = (short)(maxSequence + 1)
                };
            if (string.IsNullOrEmpty(newViewModel.DisplayOn))
            {
                newViewModel.DisplayOn = "Both";
            }
            return View("~/Views/DBTM/DBTMGraphMaster/GraphVerticalViewSequence/DBTMGraphVerticalViewSequence.cshtml", newViewModel);
        }

        [HttpPost]
        public virtual ActionResult CreateGraphVerticalViewSequence( DBTMGraphVerticalViewSequenceViewModel model)
        {
            if (ModelState.IsValid)
            {
                model = _dBTMGraphAgent.CreateGraphVerticalViewSequence(model);
                if (!model.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    if (string.Equals(model.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction( "GraphVerticalViewSequence", new { dBTMGraphVerticalViewSequenceId = model.DBTMGraphVerticalViewSequenceId });
                    }
                    else if (string.Equals(model.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("GraphVerticalViewSequenceList", new DataTableViewModel() { SelectedParameter1 = Convert.ToString(model.DBTMGraphMasterId) });
                    }
                }
            }
            if (string.IsNullOrEmpty(model.DisplayOn))
            {
                model.DisplayOn = "Both";
            }
            SetNotificationMessage( GetErrorNotificationMessage(model.ErrorMessage));
            return View( "~/Views/DBTM/DBTMGraphMaster/GraphVerticalViewSequence/DBTMGraphVerticalViewSequence.cshtml", model);
        }

        public virtual ActionResult DeleteGraphVerticalViewSequence(string dBTMGraphVerticalViewSequenceIds, string SelectedParameter1)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(dBTMGraphVerticalViewSequenceIds))
            {
                status = _dBTMGraphAgent.DeleteGraphVerticalViewSequence( dBTMGraphVerticalViewSequenceIds, out message);
                SetNotificationMessage(!status ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage) : GetSuccessNotificationMessage( GeneralResources.DeleteMessage));
                return RedirectToAction( "GraphVerticalViewSequenceList", new DataTableViewModel { SelectedParameter1 = SelectedParameter1 });
            }
            SetNotificationMessage(GetErrorNotificationMessage( GeneralResources.DeleteErrorMessage));
            return RedirectToAction( "GraphVerticalViewSequenceList", new DataTableViewModel { SelectedParameter1 = SelectedParameter1 });
        }

        #region Protected
        #endregion
    }
}