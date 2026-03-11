using Coditech.Admin.Agents;
using Coditech.Admin.Helpers;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
namespace Coditech.Admin.Controllers
{
    public class DBTMCampMasterController : BaseController
    {
        private readonly IDBTMCampAgent _dBTMCampAgent;
        private readonly IDBTMBatchAgent _dBTMBatchAgent;
        private readonly IDBTMTestAgent _dBTMTestAgent;
        private const string createEdit = "~/Views/DBTM/DBTMCampMaster/CreateEdit.cshtml";

        public DBTMCampMasterController(IDBTMCampAgent dBTMCampAgent, IDBTMTestAgent dBTMTestAgent, IDBTMBatchAgent dBTMBatchAgent)
        {
            _dBTMCampAgent = dBTMCampAgent;
            _dBTMBatchAgent = dBTMBatchAgent;
            _dBTMTestAgent = dBTMTestAgent;
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
            UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
            dBTMCampMasterViewModel.CentreCode = userModel?.SelectedCentreCode;
            BindDropdown(dBTMCampMasterViewModel);
            return View(createEdit, dBTMCampMasterViewModel);
        }

        [HttpPost]
        public virtual ActionResult Create(DBTMCampMasterViewModel dBTMCampMasterViewModel)
        {
            if (ModelState.IsValid)
            {
                BindDuration(dBTMCampMasterViewModel);
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
            BindDropdown(dBTMCampMasterViewModel);
            SetNotificationMessage(GetErrorNotificationMessage(dBTMCampMasterViewModel.ErrorMessage));
            return View(createEdit, dBTMCampMasterViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(int dBTMCampMasterId)
        {
            DBTMCampMasterViewModel dBTMCampMasterViewModel = _dBTMCampAgent.GetDBTMCamp(dBTMCampMasterId);
            BindDropdown(dBTMCampMasterViewModel);
            if (dBTMCampMasterViewModel.Duration.HasValue)
            {
                dBTMCampMasterViewModel.DurationHours =
                    dBTMCampMasterViewModel.Duration.Value.Hours.ToString("D2");

                dBTMCampMasterViewModel.DurationMinutes =
                    dBTMCampMasterViewModel.Duration.Value.Minutes.ToString("D2");
            }
            return ActionView(createEdit, dBTMCampMasterViewModel);
        }

        [HttpPost]
        public virtual ActionResult Edit(DBTMCampMasterViewModel dBTMCampMasterViewModel)
        {
            if (ModelState.IsValid)
            {
                BindDuration(dBTMCampMasterViewModel);
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
            BindDropdown(dBTMCampMasterViewModel);
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
            DBTMCampUserListViewModel list = _dBTMCampAgent.GetDBTMCampUserList(Convert.ToInt16(dataTableViewModel.SelectedParameter1), Convert.ToString(dataTableViewModel.SelectedParameter2), dataTableViewModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMCampMaster/DBTMCampUser/_AssociatedCampList.cshtml", list);
            }
            list.SelectedParameter1 = dataTableViewModel.SelectedParameter1;
            list.SelectedParameter2 = dataTableViewModel.SelectedParameter2;
            return View($"~/Views/DBTM/DBTMCampMaster/DBTMCampUser/AssociatedCampList.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult GetAssociateUnAssociateCampwiseUser( long DBTMCampUserId, int DBTMCampMasterId, string CampName, string FirstName, string LastName, long EntityId)
        {
            DBTMCampUserViewModel model = new DBTMCampUserViewModel
            {
                DBTMCampUserId = DBTMCampUserId,
                DBTMCampMasterId = DBTMCampMasterId,
                CampName = CampName,
                FirstName = FirstName,
                LastName = LastName,
                EntityId = EntityId
            };
            return PartialView("~/Views/DBTM/DBTMCampMaster/DBTMCampUser/_AssociateUnAssociateCampwiseUser.cshtml", model);
        }

        [HttpPost]
        public virtual ActionResult AssociateUnAssociateCampwiseUser(DBTMCampUserViewModel dBTMCampUserViewModel)
        {
            SetNotificationMessage(_dBTMCampAgent.AssociateUnAssociateCampwiseUser(dBTMCampUserViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
            return RedirectToAction("GetDBTMCampUserList", new DataTableViewModel { SelectedParameter1 = dBTMCampUserViewModel.DBTMCampMasterId.ToString(), SelectedParameter2 = dBTMCampUserViewModel.UserType });
        }
        [HttpGet]
        public ActionResult GetActivityByCentreCode(string centreCode, List<string> selectedActivities)
        {
            DBTMCampMasterViewModel model = new DBTMCampMasterViewModel();
            model.CustomDropdownSelectedValue1 = selectedActivities;
            if (!string.IsNullOrEmpty(centreCode))
            {
                DBTMCentreWiseTestListViewModel response = _dBTMTestAgent.GetTestsByCentreCode(centreCode);
                model.CustomDropdownList1 = response?.DBTMCentreWiseTestList?.OrderBy(x => x.TestName)
                    .Select(x => new SelectListItem
                    {
                        Text = x.TestName,
                        Value = x.DBTMTestMasterId.ToString(),
                        Selected = selectedActivities?.Contains(x.DBTMTestMasterId.ToString()) == true
                    }).ToList();
            }
            return PartialView("~/Views/DBTM/DBTMCampMaster/_ActivityDropdown.cshtml", model);
        }
        #region Protected
        protected void BindDropdown(DBTMCampMasterViewModel generalBatchViewModel)
        {
            BindFrequency(generalBatchViewModel);
            if (!string.IsNullOrEmpty(generalBatchViewModel.CentreCode))
            {
                BindDBTMCampActivity(generalBatchViewModel);
            }
            BindDBTMCampUserList(generalBatchViewModel);
        }
        protected void BindDuration(DBTMCampMasterViewModel model)
        {
            if (!string.IsNullOrEmpty(model.DurationHours) && !string.IsNullOrEmpty(model.DurationMinutes))
            {
                string durationString = $"{model.DurationHours}:{model.DurationMinutes}:00";
                if (TimeSpan.TryParse(durationString, out var duration))
                {
                    model.Duration = duration;
                }
            }
        }
        protected void BindFrequency(DBTMCampMasterViewModel generalBatchViewModel)
        {
            generalBatchViewModel.SelectedWeekDays = !string.IsNullOrEmpty(generalBatchViewModel.WeekDays) ? generalBatchViewModel.WeekDays.Split(',').ToList() : new List<string>();
            generalBatchViewModel.SchedulerWeekDaysList = CoditechDropdownHelper.GeneralDropdownList(new DropdownViewModel()
            {
                DropdownType = DropdownTypeEnum.SchedulerWeeks.ToString(),
                DropdownSelectedValue = generalBatchViewModel.WeekDays
            }).DropdownList;
            if (string.IsNullOrEmpty(generalBatchViewModel.CampFrequency))
            {
                generalBatchViewModel.CampFrequency = SchedulerFrequencyEnum.Daily.ToString();
            }
        }

        protected void BindDBTMCampActivity(DBTMCampMasterViewModel model)
        {
            model.CustomDropdownList1 = model.CustomDropdownList1 ?? new List<SelectListItem>();
            string centreCode = model?.CentreCode;
            if (string.IsNullOrEmpty(centreCode))
                return;
            DBTMCentreWiseTestListViewModel response = _dBTMTestAgent.GetTestsByCentreCode(centreCode);
            if (!response?.DBTMCentreWiseTestList?.Any() ?? true)
                return;
            model.CustomDropdownList1 = response.DBTMCentreWiseTestList
                .OrderBy(x => x.TestName)
                .Select(x => new SelectListItem
                {
                    Text = x.TestName,
                    Value = x.DBTMTestMasterId.ToString(),
                    Selected = model.CustomDropdownSelectedValue1
                    ?.Contains(x.DBTMTestMasterId.ToString()) == true
                }).ToList();
        }
        protected void BindDBTMCampUserList(DBTMCampMasterViewModel generalBatchViewModel)
        {
            UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
            string CentreCode = userModel.SelectedCentreCode;
            long GeneralTrainerMasterId = userModel.Custom1 == CustomConstants.DBTMTrainer ? (JsonConvert.DeserializeObject<DBTMCustomUserModel>(userModel.Custom3 ?? string.Empty)?.GeneralTrainerMasterId ?? 0) : 0;
            generalBatchViewModel.CustomDropdownList2 = generalBatchViewModel.CustomDropdownList2 ?? new List<SelectListItem>();
            DataTableViewModel dataTableViewModel = new DataTableViewModel() { PageIndex = int.MaxValue };
            DBTMCampUserListViewModel list = _dBTMCampAgent.GetCampUserListByCentreCodeAndGeneralTrainerMasterId(CentreCode, GeneralTrainerMasterId, generalBatchViewModel.DBTMCampMasterId);
            if (list?.DBTMCampUserList != null)
            {
                foreach (var item in list.DBTMCampUserList)
                {
                    generalBatchViewModel.CustomDropdownList2.Add(new SelectListItem
                    {
                        Text = $"{item.FirstName} {item.LastName}",
                        Value = item.EntityId.ToString(),
                    });
                }
            }
        }
        #endregion
    }
}