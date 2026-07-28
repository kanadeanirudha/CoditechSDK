using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Coditech.Resources;
using DocumentFormat.OpenXml.Wordprocessing;
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
            BindDBTMGraph(dBTMTestViewModel);
            SetNotificationMessage(GetErrorNotificationMessage(dBTMTestViewModel.ErrorMessage));
            return View(createEdit, dBTMTestViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(int dBTMTestMasterId)
        {
            DBTMTestViewModel dBTMTestViewModel = _dBTMTestAgent.GetDBTMTest(dBTMTestMasterId);
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

        // Get Activity List View Sequence
        public virtual ActionResult ActivityListViewSequenceList(DataTableViewModel dataTableViewModel)
        {
            DBTMActivityListViewSequenceListViewModel list = _dBTMTestAgent.GetActivityListViewSequenceList(Convert.ToInt16(dataTableViewModel.SelectedParameter1), dataTableViewModel);
            list.SelectedParameter1 = dataTableViewModel.SelectedParameter1;
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMTestMaster/ActivityListViewSequence/_ActivityListViewSequenceList.cshtml", list);
            }

            return View($"~/Views/DBTM/DBTMTestMaster/ActivityListViewSequence/ActivityListViewSequenceList.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult ActivityListViewSequence(int dBTMTestParameterListViewSequenceId)
        {
            DBTMActivityListViewSequenceViewModel dBTMTestViewModel = _dBTMTestAgent.GetActivityListViewSequence(dBTMTestParameterListViewSequenceId);
            if (string.IsNullOrEmpty(dBTMTestViewModel.DisplayOn))
            {
                dBTMTestViewModel.DisplayOn = "Both";
            }
            return View("~/Views/DBTM/DBTMTestMaster/ActivityListViewSequence/DBTMActivityListViewSequence.cshtml", dBTMTestViewModel);
        }

        [HttpPost]
        public virtual ActionResult ActivityListViewSequence(DBTMActivityListViewSequenceViewModel dBTMTestViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_dBTMTestAgent.UpdateActivityListViewSequence(dBTMTestViewModel).HasError
                    ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                    : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                if (string.Equals(dBTMTestViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("ActivityListViewSequence", new { dBTMTestParameterListViewSequenceId = dBTMTestViewModel.DBTMTestParameterListViewSequenceId });
                }
                else if (string.Equals(dBTMTestViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("ActivityListViewSequenceList", new DataTableViewModel() { SelectedParameter1 = Convert.ToString(dBTMTestViewModel.DBTMTestMasterId) });
                }
            }
            if (string.IsNullOrEmpty(dBTMTestViewModel.DisplayOn))
            {
                dBTMTestViewModel.DisplayOn = "Both";
            }
            return View("~/Views/DBTM/DBTMTestMaster/ActivityListViewSequence/DBTMActivityListViewSequence.cshtml", dBTMTestViewModel);
        }

        [HttpGet]
        public ActionResult UpdateSequenceNumber(int dBTMTestMasterId)
        {
            DBTMActivityListViewSequenceListViewModel listViewModel = _dBTMTestAgent.GetActivityListViewSequenceList(dBTMTestMasterId, new DataTableViewModel());

            var modelList = listViewModel.DBTMActivityListViewSequenceList
                .Select(x => new DBTMActivityListViewSequenceModel
                {
                    DBTMTestParameterListViewSequenceId = x.DBTMTestParameterListViewSequenceId,
                    DBTMTestMasterId = x.DBTMTestMasterId,
                    ParameterCode = x.ParameterCode,
                    IsCalculatedParameter = x.IsCalculatedParameter,
                    SequenceNumber = x.SequenceNumber
                })
                .ToList();

            DBTMActivityListViewSequenceViewModel viewModel = new DBTMActivityListViewSequenceViewModel
            {
                DBTMTestMasterId = dBTMTestMasterId,
                DBTMActivityListViewSequenceList = modelList
            };
            return PartialView("~/Views/DBTM/DBTMTestMaster/ActivityListViewSequence/_AddSequenceNumberPopUp.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult UpdateSequenceNumber(DBTMActivityListViewSequenceViewModel listViewModel)
        {
            ModelState.Remove(nameof(DBTMActivityVerticalViewSequenceViewModel.Recursion));
            if (ModelState.IsValid)
            {
                listViewModel = _dBTMTestAgent.UpdateSequenceNumber(listViewModel);
                if (!listViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage("Sequence Number Saved Successfully."));
                    return Json(new { success = true });
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage("Failed to Save Sequence Number."));
            return Json(new { success = false });
        }

        // Create Activity List View Sequence
        [HttpGet]
        public virtual ActionResult CreateActivityListViewSequence(int dBTMTestMasterId)
        {
            DBTMActivityListViewSequenceListViewModel listViewModel = _dBTMTestAgent.GetActivityListViewSequenceList(dBTMTestMasterId, new DataTableViewModel());
            int maxSequence = 0;
            if (listViewModel?.DBTMActivityListViewSequenceList != null && listViewModel.DBTMActivityListViewSequenceList.Count > 0)
            {
                maxSequence = listViewModel.DBTMActivityListViewSequenceList.Max(x => x.SequenceNumber);
            }
            var newViewModel = new DBTMActivityListViewSequenceViewModel
            {
                DBTMTestMasterId = dBTMTestMasterId,
                SequenceNumber = (short)(maxSequence + 1),
            };
            if (string.IsNullOrEmpty(newViewModel.DisplayOn))
            {
                newViewModel.DisplayOn = "Both";
            }
            return View("~/Views/DBTM/DBTMTestMaster/ActivityListViewSequence/DBTMActivityListViewSequence.cshtml", newViewModel);
        }

        [HttpPost]
        public virtual ActionResult CreateActivityListViewSequence(DBTMActivityListViewSequenceViewModel dBTMActivityListViewSequenceViewModel)
        {
            if (ModelState.IsValid)
            {
                dBTMActivityListViewSequenceViewModel = _dBTMTestAgent.CreateActivityListViewSequence(dBTMActivityListViewSequenceViewModel);
                if (!dBTMActivityListViewSequenceViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    if (string.Equals(dBTMActivityListViewSequenceViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("ActivityListViewSequence", new { dBTMTestParameterListViewSequenceId = dBTMActivityListViewSequenceViewModel.DBTMTestParameterListViewSequenceId });
                    }
                    else if (string.Equals(dBTMActivityListViewSequenceViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("ActivityListViewSequenceList", new DataTableViewModel() { SelectedParameter1 = Convert.ToString(dBTMActivityListViewSequenceViewModel.DBTMTestMasterId) });
                    }
                }
            }
            if (string.IsNullOrEmpty(dBTMActivityListViewSequenceViewModel.DisplayOn))
            {
                dBTMActivityListViewSequenceViewModel.DisplayOn = "Both";
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMActivityListViewSequenceViewModel.ErrorMessage));
            return View("~/Views/DBTM/DBTMTestMaster/ActivityListViewSequence/DBTMActivityListViewSequence.cshtml", dBTMActivityListViewSequenceViewModel);
        }

        public virtual ActionResult DeleteActivityListViewSequence(string dBTMTestParameterListViewSequenceIds, string SelectedParameter1)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(dBTMTestParameterListViewSequenceIds))
            {
                status = _dBTMTestAgent.DeleteActivityListViewSequence(dBTMTestParameterListViewSequenceIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction("ActivityListViewSequenceList", new DataTableViewModel() { SelectedParameter1 = SelectedParameter1 });
            }
            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction("ActivityListViewSequenceList", new DataTableViewModel() { SelectedParameter1 = SelectedParameter1 });
        }

        #region Activity Vertical View Sequence
        // Get Activity Vertical View Sequence
        public virtual ActionResult ActivityVerticalViewSequenceList(DataTableViewModel dataTableViewModel)
        {
            DBTMActivityVerticalViewSequenceListViewModel list = _dBTMTestAgent.GetActivityVerticalViewSequenceList(Convert.ToInt16(dataTableViewModel.SelectedParameter1), dataTableViewModel);
            list.SelectedParameter1 = dataTableViewModel.SelectedParameter1;
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMTestMaster/ActivityVerticalViewSequence/_ActivityVerticalViewSequenceList.cshtml", list);
            }

            return View($"~/Views/DBTM/DBTMTestMaster/ActivityVerticalViewSequence/ActivityVerticalViewSequenceList.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult ActivityVerticalViewSequence(int dBTMTestParameterVerticalViewSequenceId)
        {
            DBTMActivityVerticalViewSequenceViewModel dBTMTestViewModel = _dBTMTestAgent.GetActivityVerticalViewSequence(dBTMTestParameterVerticalViewSequenceId);
            return View("~/Views/DBTM/DBTMTestMaster/ActivityVerticalViewSequence/DBTMActivityVerticalViewSequence.cshtml", dBTMTestViewModel);
        }

        [HttpPost]
        public virtual ActionResult ActivityVerticalViewSequence(DBTMActivityVerticalViewSequenceViewModel dBTMTestViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_dBTMTestAgent.UpdateActivityVerticalViewSequence(dBTMTestViewModel).HasError
                    ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                    : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                if (string.Equals(dBTMTestViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("ActivityVerticalViewSequence", new { dBTMTestParameterVerticalViewSequenceId = dBTMTestViewModel.DBTMTestParameterVerticalViewSequenceId });
                }
                else if (string.Equals(dBTMTestViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("ActivityVerticalViewSequenceList", new DataTableViewModel() { SelectedParameter1 = Convert.ToString(dBTMTestViewModel.DBTMTestMasterId) });
                }
            }
            return View("~/Views/DBTM/DBTMTestMaster/ActivityListViewSequence/DBTMActivityListViewSequence.cshtml", dBTMTestViewModel);
        }

        [HttpGet]
        public ActionResult UpdateVerticalSequenceNumber(int dBTMTestMasterId)
        {
            DBTMActivityVerticalViewSequenceListViewModel listViewModel = _dBTMTestAgent.GetActivityVerticalViewSequenceList(dBTMTestMasterId, new DataTableViewModel());

            var modelList = listViewModel.DBTMActivityVerticalViewSequenceList
                .Select(x => new DBTMActivityVerticalViewSequenceModel
                {
                    DBTMTestParameterVerticalViewSequenceId = x.DBTMTestParameterVerticalViewSequenceId,
                    DBTMTestMasterId = x.DBTMTestMasterId,
                    ParameterCode = x.ParameterCode,
                    IsCalculatedParameter = x.IsCalculatedParameter,
                    SequenceNumber = x.SequenceNumber
                })
                .ToList();

            DBTMActivityVerticalViewSequenceViewModel viewModel = new DBTMActivityVerticalViewSequenceViewModel
            {
                DBTMTestMasterId = dBTMTestMasterId,
                DBTMActivityVerticalViewSequenceList = modelList
            };
            return PartialView("~/Views/DBTM/DBTMTestMaster/ActivityVerticalViewSequence/_AddVerticalSequenceNumberPopUp.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult UpdateVerticalSequenceNumber(DBTMActivityVerticalViewSequenceViewModel listViewModel)
        {
            ModelState.Remove(nameof(DBTMActivityVerticalViewSequenceViewModel.Recursion));
            if (ModelState.IsValid)
            {
                listViewModel = _dBTMTestAgent.UpdateVerticalSequenceNumber(listViewModel);
                if (!listViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage("Sequence Number Saved Successfully."));
                    return Json(new { success = true });
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage("Failed to Save Sequence Number."));
            return Json(new { success = false });
        }

        // Create Activity Vertical View Sequence
        [HttpGet]
        public virtual ActionResult CreateActivityVerticalViewSequence(int dBTMTestMasterId)
        {
            DBTMActivityVerticalViewSequenceListViewModel listViewModel = _dBTMTestAgent.GetActivityVerticalViewSequenceList(dBTMTestMasterId, new DataTableViewModel());
            int maxSequence = 0;
            if (listViewModel?.DBTMActivityVerticalViewSequenceList != null && listViewModel.DBTMActivityVerticalViewSequenceList.Count > 0)
            {
                maxSequence = listViewModel.DBTMActivityVerticalViewSequenceList.Max(x => x.SequenceNumber);
            }
            var newViewModel = new DBTMActivityVerticalViewSequenceViewModel
            {
                DBTMTestMasterId = dBTMTestMasterId,
                SequenceNumber = (short)(maxSequence + 1),
            };
            if (string.IsNullOrEmpty(listViewModel.DisplayOn))
            {
                newViewModel.DisplayOn = "Both";
            }
            return View("~/Views/DBTM/DBTMTestMaster/ActivityVerticalViewSequence/DBTMActivityVerticalViewSequence.cshtml", newViewModel);
        }

        [HttpPost]
        public virtual ActionResult CreateActivityVerticalViewSequence(DBTMActivityVerticalViewSequenceViewModel dBTMActivityVerticalViewSequenceViewModel)
        {
            if (ModelState.IsValid)
            {
                dBTMActivityVerticalViewSequenceViewModel = _dBTMTestAgent.CreateActivityVerticalViewSequence(dBTMActivityVerticalViewSequenceViewModel);
                if (!dBTMActivityVerticalViewSequenceViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    if (string.Equals(dBTMActivityVerticalViewSequenceViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("ActivityVerticalViewSequence", new { dBTMTestParameterVerticalViewSequenceId = dBTMActivityVerticalViewSequenceViewModel.DBTMTestParameterVerticalViewSequenceId });
                    }
                    else if (string.Equals(dBTMActivityVerticalViewSequenceViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("ActivityVerticalViewSequenceList", new DataTableViewModel() { SelectedParameter1 = Convert.ToString(dBTMActivityVerticalViewSequenceViewModel.DBTMTestMasterId) });
                    }
                }
            }
            if (string.IsNullOrEmpty(dBTMActivityVerticalViewSequenceViewModel.DisplayOn))
            {
                dBTMActivityVerticalViewSequenceViewModel.DisplayOn = "Both";
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMActivityVerticalViewSequenceViewModel.ErrorMessage));
            return View("~/Views/DBTM/DBTMTestMaster/ActivityVerticalViewSequence/DBTMActivityVerticalViewSequence.cshtml", dBTMActivityVerticalViewSequenceViewModel);
        }

        public virtual ActionResult DeleteActivityVerticalViewSequence(string dBTMTestParameterVerticalViewSequenceIds, string SelectedParameter1)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(dBTMTestParameterVerticalViewSequenceIds))
            {
                status = _dBTMTestAgent.DeleteActivityVerticalViewSequence(dBTMTestParameterVerticalViewSequenceIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction("ActivityVerticalViewSequenceList", new DataTableViewModel { SelectedParameter1 = SelectedParameter1 });
            }
            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction("ActivityVerticalViewSequenceList", new DataTableViewModel { SelectedParameter1 = SelectedParameter1 });
        }
        public virtual ActionResult DBTMTestWisePerformanceStandardList(int dBTMTestMasterId, short dBTMTestwisePerformanceStandardCategoryId)
        {
            DBTMTestWisePerformanceStandardListViewModel list = new DBTMTestWisePerformanceStandardListViewModel();
            if (dBTMTestwisePerformanceStandardCategoryId > 0)
                list = _dBTMTestAgent.DBTMTestWisePerformanceStandardList(dBTMTestMasterId, dBTMTestwisePerformanceStandardCategoryId);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMTestMaster/DBTMTestWisePerformanceStandard/_DBTMTestWisePerformanceStandardList.cshtml", list);
            }
            return View("~/Views/DBTM/DBTMTestMaster/DBTMTestWisePerformanceStandard/DBTMTestWisePerformanceStandardList.cshtml", list);
        }
        [HttpPost]
        public virtual ActionResult SaveDBTMTestWisePerformanceStandard(DBTMTestWisePerformanceStandardViewModel dBTMTestWisePerformanceStandardViewModel)
        {
            if (ModelState.IsValid)
            {
                if (dBTMTestWisePerformanceStandardViewModel.DBTMTestWisePerformanceStandardId > 0)
                {
                    dBTMTestWisePerformanceStandardViewModel = _dBTMTestAgent.UpdateDBTMTestWisePerformanceStandard(dBTMTestWisePerformanceStandardViewModel);
                }
                else
                {
                    dBTMTestWisePerformanceStandardViewModel = _dBTMTestAgent.CreateDBTMTestWisePerformanceStandard(dBTMTestWisePerformanceStandardViewModel);
                }
                if (!dBTMTestWisePerformanceStandardViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                    return Json(new { success = true });
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage));
            return Json(new { success = false });
        }
        #endregion
        #region Protected

        protected virtual void BindDBTMGraph(DBTMTestViewModel dBTMTestViewModel)
        {
            dBTMTestViewModel.DBTMGraphMasterList = dBTMTestViewModel.DBTMGraphMasterList ?? new List<SelectListItem>();
            if (dBTMTestViewModel.DBTMTestMasterId > 0)
            {
                DBTMGraphMasterListViewModel dBTMGraphMasterList = _dBTMTestAgent.DBTMGraph(dBTMTestViewModel.DBTMTestMasterId);
                if (dBTMGraphMasterList?.DBTMGraphMasterList != null)
                {
                    foreach (var item in dBTMGraphMasterList.DBTMGraphMasterList)
                    {
                        dBTMTestViewModel.DBTMGraphMasterList.Add(new SelectListItem
                        {
                            Text = $"{item.GraphName} ({item.GraphMode})",
                            Value = item.DBTMGraphMasterId.ToString(),
                            Selected = dBTMTestViewModel.DBTMSelectedGraph != null &&
                                       dBTMTestViewModel.DBTMSelectedGraph.Contains(item.DBTMGraphMasterId.ToString())
                        });
                    }
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
        [HttpGet]
        public virtual ActionResult DBTMTestwisePerformanceStandardCategoryList(short dBTMTestwisePerformanceStandardCategoryId)
        {
            DropdownViewModel categoryDropdown = new DropdownViewModel()
            {
                DropdownType = DropdownCustomTypeEnum.DBTMTestwisePerformanceStandardCategory.ToString(),
                DropdownName = "DBTMTestwisePerformanceStandardCategoryId",
                Parameter = dBTMTestwisePerformanceStandardCategoryId.ToString(),
                IsCustomDropdown = true
            };
            return PartialView("~/Views/Shared/Control/_DropdownList.cshtml", categoryDropdown);
        }

        #endregion
    }
}
