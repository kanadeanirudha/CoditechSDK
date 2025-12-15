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
    public class DBTMGeneralBatchMasterController : BaseController
    {
        private readonly IGeneralBatchAgent _generalBatchAgent;
        private const string createEditBatch = "~/Views/GeneralMaster/GeneralBatchMaster/CreateEditGeneralBatch.cshtml";
        private readonly IDBTMTestAgent _dBTMTestAgent;
        private readonly IDBTMBatchAgent _dBTMBatchAgent;

        public DBTMGeneralBatchMasterController(IGeneralBatchAgent generalBatchAgent, IDBTMTestAgent dBTMTestAgent, IDBTMBatchAgent dBTMBatchAgent)
        {
            _generalBatchAgent = generalBatchAgent;
            _dBTMTestAgent = dBTMTestAgent;
            _dBTMBatchAgent = dBTMBatchAgent;
        }
        [HttpGet, HttpPost]
        public ActionResult List(DataTableViewModel dataTableModel)
        {
            GeneralBatchListViewModel list = new GeneralBatchListViewModel();
            GetListOnlyIfSingleCentre(dataTableModel);

            {
                list = _generalBatchAgent.GetBatchList(dataTableModel);
            }
            list.SelectedCentreCode = dataTableModel.SelectedCentreCode;
            list.Custom4 = dataTableModel.SelectedParameter4;

            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/GeneralMaster/GeneralBatchMaster/_List.cshtml", list);
            }

            return View("~/Views/GeneralMaster/GeneralBatchMaster/List.cshtml", list);
        }

        [HttpGet]
        public ActionResult Create(string custom4)
        {

            GeneralBatchViewModel generalBatchViewModel = new GeneralBatchViewModel();
            generalBatchViewModel.Custom4 = custom4;
            BindDropdown(generalBatchViewModel);
            return View("~/Views/GeneralMaster/GeneralBatchMaster/CreateEditGeneralBatch.cshtml", generalBatchViewModel);
        }

        [HttpPost]
        public ActionResult Create(GeneralBatchViewModel generalBatchViewModel)
        {
            if ((generalBatchViewModel?.CustomDropdownSelectedValue1?.Count ?? 0) == 0 &&
                (generalBatchViewModel?.CustomDropdownSelectedValue2?.Count ?? 0) == 0)
            {
                generalBatchViewModel.ErrorMessage = "Please select at least one Activity and one Batch User.";
            }
            else if ((generalBatchViewModel?.CustomDropdownSelectedValue1?.Count ?? 0) == 0)
            {
                generalBatchViewModel.ErrorMessage = "Please select at least one Activity.";
            }
            else if ((generalBatchViewModel?.CustomDropdownSelectedValue2?.Count ?? 0) == 0)
            {
                generalBatchViewModel.ErrorMessage = "Please select at least one Batch User.";
            }
            else if (ModelState.IsValid)
            {
                BindDuration(generalBatchViewModel);
                generalBatchViewModel = _generalBatchAgent.CreateGeneralBatch(generalBatchViewModel);
                if (!generalBatchViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    if (string.Equals(generalBatchViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("UpdateGeneralBatch", new { generalBatchMasterId = generalBatchViewModel.GeneralBatchMasterId, generalBatchViewModel.Custom4 });
                    }
                    else if (string.Equals(generalBatchViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("List", new DataTableViewModel { SelectedCentreCode = generalBatchViewModel.CentreCode, SelectedParameter4 = Convert.ToString(generalBatchViewModel.Custom4) });

                    }
                }
            }
            BindDropdown(generalBatchViewModel);
            SetNotificationMessage(GetErrorNotificationMessage(generalBatchViewModel.ErrorMessage));
            return View("~/Views/GeneralMaster/GeneralBatchMaster/CreateEditGeneralBatch.cshtml", generalBatchViewModel);
        }

        [HttpGet, HttpPost]
        public virtual ActionResult GetGeneralBatchUserList(DataTableViewModel dataTableViewModel)
        {
            GeneralBatchUserListViewModel list = _generalBatchAgent.GetGeneralBatchUserList(Convert.ToInt32(dataTableViewModel.SelectedParameter1), Convert.ToString(dataTableViewModel.SelectedParameter2), dataTableViewModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/GeneralMaster/GeneralBatchMaster/GeneralBatchUser/_AssociatedBatchList.cshtml", list);
            }
            list.SelectedParameter1 = dataTableViewModel.SelectedParameter1;
            list.SelectedParameter2 = dataTableViewModel.SelectedParameter2;
            list.Custom4 = dataTableViewModel.SelectedParameter4;
            return View($"~/Views/GeneralMaster/GeneralBatchMaster/GeneralBatchUser/AssociatedBatchList.cshtml", list);
        }
        [HttpGet]
        public ActionResult UpdateGeneralBatch(int generalBatchMasterId, string custom4)
        {
            GeneralBatchViewModel generalBatchViewModel = _generalBatchAgent.GetGeneralBatch(generalBatchMasterId);
            BindDropdown(generalBatchViewModel);
            generalBatchViewModel.Custom4 = custom4;
            return ActionView(createEditBatch, generalBatchViewModel);
        }

        [HttpPost]
        public ActionResult UpdateGeneralBatch(GeneralBatchViewModel generalBatchViewModel)
        {
            if (generalBatchViewModel?.CustomDropdownSelectedValue1?.Count > 0)
            {
                if (ModelState.IsValid)
                {
                    BindDuration(generalBatchViewModel);
                    SetNotificationMessage(_generalBatchAgent.UpdateGeneralBatch(generalBatchViewModel).HasError
                    ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                    : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                    if (string.Equals(generalBatchViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("UpdateGeneralBatch", new { generalBatchMasterId = generalBatchViewModel.GeneralBatchMasterId, generalBatchViewModel.Custom4 });
                    }
                    else if (string.Equals(generalBatchViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("List", new DataTableViewModel { SelectedCentreCode = generalBatchViewModel.CentreCode, SelectedParameter4 = Convert.ToString(generalBatchViewModel.Custom4) });

                    }
                }
            }
            else
            {
                SetNotificationMessage(GetErrorNotificationMessage("Please Select Activity."));
            }
            BindDropdown(generalBatchViewModel);
            return View(createEditBatch, generalBatchViewModel);
        }

        //Calendar
        [HttpGet]
        public ActionResult DBTMCalendar()
        {
            return View("~/Views/DBTM/DBTMDashboard/DBTMCalendar.cshtml");
        }

        [HttpGet]
        public IActionResult GetCalendarData(DateTime startDate, DateTime endDate)
        {
            UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
            string centreCode = userModel.SelectedCentreCode;

            GeneralBatchListViewModel batches = _dBTMBatchAgent.GetCalendarBatches(
                centreCode,
                userModel.UserMasterId,
                startDate,
                endDate
            );

            var frequencyMap = new Dictionary<string, Func<DateTime, DateTime>>
            {
                { "Daily", d => d.AddDays(1) },
                { "Weekly", d => d.AddDays(7) },
                { "Monthly", d => d.AddMonths(1) }
            };

            var events = batches.GeneralBatchList.SelectMany(batch =>
            {
                var eventsForBatch = new List<object>();

                if (batch.BatchStartDate.HasValue &&
                    batch.BatchStartTime.HasValue &&
                    batch.Duration.HasValue)
                {
                    DateTime currentDate = batch.BatchStartDate.Value;
                    DateTime batchEndDate = batch.BatchExpireDate ?? batch.BatchStartDate.Value;

                    var increment = frequencyMap.ContainsKey(batch.BatchFrequency) ? frequencyMap[batch.BatchFrequency] : (d => d.AddDays(1));

                    while (currentDate <= batchEndDate)
                    {
                        DateTime startDateTime = currentDate.Date + batch.BatchStartTime.Value;
                        DateTime endDateTime = startDateTime + batch.Duration.Value;

                        if (startDateTime <= endDate && endDateTime >= startDate)
                        {
                            eventsForBatch.Add(new
                            {
                                id = batch.GeneralBatchMasterId,
                                title = batch.BatchName,
                                start = startDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                                end = endDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                                allDay = false,
                                backgroundColor = batch.IsActive ? "#00a65a" : "#f39c12",
                                description = $"Frequency: {batch.BatchFrequency} | Duration: {batch.Duration}"
                            });
                        }
                        if (batch.BatchExpireDate == null) break;
                        currentDate = increment(currentDate);
                    }
                }
                return eventsForBatch;
            });
            return Json(events);
        }

        #region Protected Methods
        protected void BindFrequency(GeneralBatchViewModel generalBatchViewModel)
        {
            generalBatchViewModel.SelectedWeekDays = !string.IsNullOrEmpty(generalBatchViewModel.WeekDays) ? generalBatchViewModel.WeekDays.Split(',').ToList() : new List<string>();
            generalBatchViewModel.SchedulerWeekDaysList = CoditechDropdownHelper.GeneralDropdownList(new DropdownViewModel()
            {
                DropdownType = DropdownTypeEnum.SchedulerWeeks.ToString(),
                DropdownSelectedValue = generalBatchViewModel.WeekDays
            }).DropdownList;
            if (string.IsNullOrEmpty(generalBatchViewModel.BatchFrequency))
            {
                generalBatchViewModel.BatchFrequency = SchedulerFrequencyEnum.Daily.ToString();
            }
        }
        protected void BindDBTMBatchActivity(GeneralBatchViewModel generalBatchViewModel)
        {
            generalBatchViewModel.CustomDropdownList1 = generalBatchViewModel.CustomDropdownList1 ?? new List<SelectListItem>();
            DataTableViewModel dataTableModel = new DataTableViewModel() { PageSize = int.MaxValue };
            DBTMTestListViewModel dBTMBatchActivityList = _dBTMTestAgent.GetDBTMTestList(dataTableModel);
            if (dBTMBatchActivityList?.DBTMTestList != null)
            {
                foreach (var item in dBTMBatchActivityList.DBTMTestList.Where(x => x.IsActive).OrderBy(x => x.PerformanceMatrix) .ThenBy(x => x.TestName))
                {
                    generalBatchViewModel.CustomDropdownList1.Add(new SelectListItem
                    {
                        Text = item.TestName,
                        Value = item.DBTMTestMasterId.ToString(),
                    });
                }
            }
        }
        protected void BindDuration(GeneralBatchViewModel model)
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
        protected void BindDBTMBatchUserList(GeneralBatchViewModel generalBatchViewModel)
        {
            UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
            string CentreCode = userModel.SelectedCentreCode;
            long GeneralTrainerMasterId = userModel.Custom1 == CustomConstants.DBTMTrainer ? (JsonConvert.DeserializeObject<DBTMCustomUserModel>(userModel.Custom3 ?? string.Empty)?.GeneralTrainerMasterId ?? 0) : 0;
            generalBatchViewModel.CustomDropdownList2 = generalBatchViewModel.CustomDropdownList2 ?? new List<SelectListItem>();
            DataTableViewModel dataTableViewModel = new DataTableViewModel() { PageIndex = int.MaxValue };
            GeneralBatchUserListViewModel list = _dBTMBatchAgent.GetBatchUserListByCentreCodeAndGeneralTrainerMasterId(CentreCode, GeneralTrainerMasterId, generalBatchViewModel.GeneralBatchMasterId);
            if (list?.GeneralBatchUserList != null)
            {
                foreach (var item in list.GeneralBatchUserList)
                {
                    generalBatchViewModel.CustomDropdownList2.Add(new SelectListItem
                    {
                        Text = $"{item.FirstName} {item.LastName}",
                        Value = item.EntityId.ToString(),
                    });
                }
            }
        }
        protected void BindDropdown(GeneralBatchViewModel generalBatchViewModel)
        {
            BindFrequency(generalBatchViewModel);
            BindDBTMBatchActivity(generalBatchViewModel);
            BindDBTMBatchUserList(generalBatchViewModel);

        }
        public virtual ActionResult Cancel(string SelectedCentreCode, string custom4)
        {
            DataTableViewModel dataTableViewModel = new DataTableViewModel() { SelectedCentreCode = SelectedCentreCode, SelectedParameter4 = custom4 };
            return RedirectToAction("List", dataTableViewModel);
        }
        public virtual ActionResult Delete(string generalBatchMasterIds, string selectedCentreCode, string custom4)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(generalBatchMasterIds))
            {
                status = _generalBatchAgent.DeleteGeneralBatch(generalBatchMasterIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(string.IsNullOrEmpty(message) ? GeneralResources.DeleteErrorMessage : message)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction("List", new DataTableViewModel { SelectedCentreCode = selectedCentreCode, SelectedParameter4 = custom4 });
            }
            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction("List", new DataTableViewModel { SelectedCentreCode = selectedCentreCode, SelectedParameter4 = custom4 });
        }
        #endregion
    }
}