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
        private readonly IDBTMBatchActivityAgent _dBTMBatchActivityAgent;
        private readonly IDBTMTestAgent _dBTMTestAgent;
        private readonly IDBTMBatchAgent _dBTMBatchAgent;

        public DBTMGeneralBatchMasterController(IGeneralBatchAgent generalBatchAgent, IDBTMBatchActivityAgent dBTMBatchActivityAgent, IDBTMTestAgent dBTMTestAgent, IDBTMBatchAgent dBTMBatchAgent)
        {
            _generalBatchAgent = generalBatchAgent;
            _dBTMBatchActivityAgent = dBTMBatchActivityAgent;
            _dBTMTestAgent = dBTMTestAgent;
            _dBTMBatchAgent = dBTMBatchAgent;
        }

        [HttpGet]
        public ActionResult Create()
        {
            GeneralBatchViewModel generalBatchViewModel = new GeneralBatchViewModel();
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
            BindDBTMBatchActivity(generalBatchViewModel);
            BindDBTMBatchUserList(generalBatchViewModel);
            return View(createEditBatch, generalBatchViewModel);
        }

        [HttpPost]
        public ActionResult Create(GeneralBatchViewModel generalBatchViewModel)
        {
            if (generalBatchViewModel?.CustomDropdownSelectedValue1?.Count > 0 && generalBatchViewModel?.CustomDropdownSelectedValue2?.Count > 0)
            {
                if (ModelState.IsValid)
                {
                    BindDuration(generalBatchViewModel);
                    generalBatchViewModel = _generalBatchAgent.CreateGeneralBatch(generalBatchViewModel);
                    if (!generalBatchViewModel.HasError)
                    {
                        SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                        return RedirectToAction<GeneralBatchMasterController>(x => x.List(new DataTableViewModel { SelectedCentreCode = generalBatchViewModel.CentreCode }));
                    }
                }
                SetNotificationMessage(GetErrorNotificationMessage(generalBatchViewModel.ErrorMessage));
            }
            else
            {
                SetNotificationMessage(GetErrorNotificationMessage("Please Select Activity and User"));
            }
            BindDBTMBatchActivity(generalBatchViewModel);
            BindDBTMBatchUserList(generalBatchViewModel);
            return View(createEditBatch, generalBatchViewModel);
        }

        [HttpGet]
        public ActionResult UpdateGeneralBatch(int generalBatchMasterId)
        {
            GeneralBatchViewModel generalBatchViewModel = _generalBatchAgent.GetGeneralBatch(generalBatchMasterId);
            generalBatchViewModel.SelectedWeekDays = !string.IsNullOrEmpty(generalBatchViewModel.WeekDays) ? generalBatchViewModel.WeekDays.Split(',').ToList() : new List<string>();
            generalBatchViewModel.SchedulerWeekDaysList = CoditechDropdownHelper.GeneralDropdownList(new DropdownViewModel()
            {
                DropdownType = DropdownTypeEnum.SchedulerWeeks.ToString(),
                DropdownSelectedValue = generalBatchViewModel.WeekDays
            }).DropdownList;
            BindDBTMBatchActivity(generalBatchViewModel);
            BindDBTMBatchUserList(generalBatchViewModel);
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
                    return RedirectToAction("UpdateGeneralBatch", new { generalBatchMasterId = generalBatchViewModel.GeneralBatchMasterId });
                }
            }
            else
            {
                SetNotificationMessage(GetErrorNotificationMessage("Please Select Activity."));
            }
            BindDBTMBatchActivity(generalBatchViewModel);
            return View(createEditBatch, generalBatchViewModel);
        }

        protected void BindDBTMBatchActivity(GeneralBatchViewModel generalBatchViewModel)
        {
            generalBatchViewModel.CustomDropdownList1 = generalBatchViewModel.CustomDropdownList1 ?? new List<SelectListItem>();
            DataTableViewModel dataTableModel = new DataTableViewModel() { PageSize = int.MaxValue };
            DBTMTestListViewModel dBTMBatchActivityList = _dBTMTestAgent.GetDBTMTestList(dataTableModel);
            if (dBTMBatchActivityList?.DBTMTestList != null)
            {
                foreach (var item in dBTMBatchActivityList.DBTMTestList)
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
            DataTableViewModel dataTableViewModel = new DataTableViewModel() {PageIndex = int.MaxValue };
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
    }
}


