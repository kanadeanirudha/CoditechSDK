using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Coditech.Resources;

using Microsoft.AspNetCore.Mvc;

namespace Coditech.Admin.Controllers
{
    public class DBTMTraineeDetailsController : BaseController
    {
        private readonly IDBTMTraineeDetailsAgent _dBTMTraineeDetailsAgent;
        private const string createEditTraineeDetails = "~/Views/DBTM/DBTMTraineeDetails/DBTMTraineeDetails.cshtml";
        private const string createEditAssociatedTrainer = "~/Views/GeneralMaster/GeneralTrainerMaster/GeneralTraineeAssociatedToTrainer/CreateEditAssociatedTrainer.cshtml";
        public DBTMTraineeDetailsController(IDBTMTraineeDetailsAgent dBTMTraineeDetailsAgent)
        {
            _dBTMTraineeDetailsAgent = dBTMTraineeDetailsAgent;
        }

        #region DBTMTraineeDetails

        public virtual ActionResult List(DataTableViewModel dataTableViewModel)
        {
            DBTMTraineeDetailsListViewModel list = new DBTMTraineeDetailsListViewModel();
            GetListOnlyIfSingleCentre(dataTableViewModel);
           if (!string.IsNullOrEmpty(dataTableViewModel.SelectedCentreCode))
            {
                UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);

                if (userModel?.Custom1 == CustomConstants.DBTMTrainer)
                {
                    if (!string.IsNullOrEmpty(dataTableViewModel.SelectedParameter1))
                        list = _dBTMTraineeDetailsAgent.GetDBTMTraineeDetailsList(dataTableViewModel, "");
                }
                else
                    list = _dBTMTraineeDetailsAgent.GetDBTMTraineeDetailsList(dataTableViewModel);
            }
            list.SelectedCentreCode = dataTableViewModel.SelectedCentreCode;
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMTraineeDetails/_List.cshtml", list);
            }
            return View($"~/Views/DBTM/DBTMTraineeDetails/List.cshtml", list);
        }

        public ActionResult ActiveMemberList(DataTableViewModel dataTableViewModel)
        {
            DBTMTraineeDetailsListViewModel list = new DBTMTraineeDetailsListViewModel();
            GetListOnlyIfSingleCentre(dataTableViewModel);
            dataTableViewModel.SelectedParameter2 = "Active";
            if (!string.IsNullOrEmpty(dataTableViewModel.SelectedCentreCode))
            {
                UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);

                if (userModel?.Custom1 == CustomConstants.DBTMTrainer)
                {
                    if (!string.IsNullOrEmpty(dataTableViewModel.SelectedParameter1))
                        list = _dBTMTraineeDetailsAgent.GetDBTMTraineeDetailsList(dataTableViewModel, "Active");
                }
                else
                    list = _dBTMTraineeDetailsAgent.GetDBTMTraineeDetailsList(dataTableViewModel, "Active");
            }
            list.SelectedCentreCode = dataTableViewModel.SelectedCentreCode;
            list.ListType = "Active";
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMTraineeDetails/_List.cshtml", list);
            }
            return View($"~/Views/DBTM/DBTMTraineeDetails/List.cshtml", list);
        }

        public ActionResult InActiveMemberList(DataTableViewModel dataTableViewModel)
        {
            DBTMTraineeDetailsListViewModel list = new DBTMTraineeDetailsListViewModel();
            GetListOnlyIfSingleCentre(dataTableViewModel);
            dataTableViewModel.SelectedParameter2 = "InActive";
            if (!string.IsNullOrEmpty(dataTableViewModel.SelectedCentreCode))
            {
                UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);

                if (userModel?.Custom1 == CustomConstants.DBTMTrainer)
                {
                    if (!string.IsNullOrEmpty(dataTableViewModel.SelectedParameter1))
                        list = _dBTMTraineeDetailsAgent.GetDBTMTraineeDetailsList(dataTableViewModel, "InActive");
                }
                else
                    list = _dBTMTraineeDetailsAgent.GetDBTMTraineeDetailsList(dataTableViewModel, "InActive");
            }
            list.SelectedCentreCode = dataTableViewModel.SelectedCentreCode;
            list.ListType = "InActive";
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMTraineeDetails/_List.cshtml", list);
            }
            return View($"~/Views/DBTM/DBTMTraineeDetails/List.cshtml", list);
        }

        [HttpGet]
        public ActionResult CreateDBTMTrainee()
        {
            DBTMTraineeDetailsCreateEditViewModel viewModel = new DBTMTraineeDetailsCreateEditViewModel();
            viewModel.UserType = UserTypeEnum.Trainee.ToString();
            return View(createEditTraineeDetails, viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult CreateDBTMTrainee(DBTMTraineeDetailsCreateEditViewModel dBTMTraineeDetailsCreateEditViewModel)
        {
            if (ModelState.IsValid)
            {
                dBTMTraineeDetailsCreateEditViewModel = _dBTMTraineeDetailsAgent.CreateDBTMTraineeDetails(dBTMTraineeDetailsCreateEditViewModel);
                if (!dBTMTraineeDetailsCreateEditViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    return RedirectToAction("List", new { selectedCentreCode = dBTMTraineeDetailsCreateEditViewModel.SelectedCentreCode });
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMTraineeDetailsCreateEditViewModel.ErrorMessage));
            return View(createEditTraineeDetails, dBTMTraineeDetailsCreateEditViewModel);
        }

        [HttpGet]
        public virtual ActionResult UpdateDBTMTraineePersonalDetails(long dBTMTraineeDetailId, long personId)
        {
            DBTMTraineeDetailsCreateEditViewModel dBTMTraineeDetailsCreateEditViewModel = _dBTMTraineeDetailsAgent.GetDBTMTraineePersonalDetails(dBTMTraineeDetailId, personId);
            dBTMTraineeDetailsCreateEditViewModel.UserType = UserTypeEnum.Trainee.ToString();
            return ActionView(createEditTraineeDetails, dBTMTraineeDetailsCreateEditViewModel);
        }

        [HttpPost]
        public virtual ActionResult UpdateDBTMTraineePersonalDetails(DBTMTraineeDetailsCreateEditViewModel dBTMTraineeDetailsCreateEditViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_dBTMTraineeDetailsAgent.UpdateDBTMTraineePersonalDetails(dBTMTraineeDetailsCreateEditViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                return RedirectToAction("UpdateDBTMTraineePersonalDetails", new { dBTMTraineeDetailId = dBTMTraineeDetailsCreateEditViewModel.DBTMTraineeDetailId, personId = dBTMTraineeDetailsCreateEditViewModel.PersonId });
            }
            return View(createEditTraineeDetails, dBTMTraineeDetailsCreateEditViewModel);
        }

        [HttpGet]
        public virtual ActionResult MemberOtherDetails(long dBTMTraineeDetailId)
        {
            DBTMTraineeDetailsViewModel dBTMTraineeDetailsViewModel = _dBTMTraineeDetailsAgent.GetDBTMTraineeOtherDetails(dBTMTraineeDetailId);
            return View("~/Views/DBTM/DBTMTraineeDetails/UpdateDBTMTraineeOtherDetails.cshtml", dBTMTraineeDetailsViewModel);
        }

        [HttpPost]
        public virtual ActionResult MemberOtherDetails(DBTMTraineeDetailsViewModel dBTMTraineeDetailsViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_dBTMTraineeDetailsAgent.UpdateDBTMTraineeOtherDetails(dBTMTraineeDetailsViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                return RedirectToAction("MemberOtherDetails", new { dBTMTraineeDetailId = dBTMTraineeDetailsViewModel.DBTMTraineeDetailId });
            }
            return View("~/Views/DBTM/DBTMTraineeDetails/UpdateDBTMTraineeOtherDetails.cshtml", dBTMTraineeDetailsViewModel);
        }

        public virtual ActionResult Delete(string dBTMTraineeDetailIds, string selectedCentreCode)
        {
            string message = string.Empty;
            bool status = false;

            if (!string.IsNullOrEmpty(dBTMTraineeDetailIds))
            {
                status = _dBTMTraineeDetailsAgent.DeleteDBTMTraineeDetails(dBTMTraineeDetailIds, out message);

                SetNotificationMessage(!status
                    ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                    : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction("List", new DataTableViewModel { SelectedCentreCode = selectedCentreCode });
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction("List", new DataTableViewModel { SelectedCentreCode = selectedCentreCode });
        }
        #endregion DBTMTraineeDetails

        #region TraineeAssociatedToTrainer
        public virtual ActionResult GetAssociatedTrainerList(DataTableViewModel dataTableViewModel)
        {
            GeneralTraineeAssociatedToTrainerListViewModel list = _dBTMTraineeDetailsAgent.GetAssociatedTrainerList(Convert.ToInt64(dataTableViewModel.SelectedParameter1), Convert.ToInt64(dataTableViewModel.SelectedParameter2), dataTableViewModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/GeneralMaster/GeneralTrainerMaster/GeneralTraineeAssociatedToTrainer/_AssociatedTrainerList.cshtml", list);
            }
            list.SelectedParameter1 = dataTableViewModel.SelectedParameter1;
            list.SelectedParameter2 = dataTableViewModel.SelectedParameter2;

            return View($"~/Views/GeneralMaster/GeneralTrainerMaster/GeneralTraineeAssociatedToTrainer/AssociatedTrainerList.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult InsertAssociatedTrainer(long dBTMTraineeDetailId, long personId)
        {
            GeneralTraineeAssociatedToTrainerViewModel viewModel = _dBTMTraineeDetailsAgent.AssociatedTrainer(dBTMTraineeDetailId, personId);
            return View(createEditAssociatedTrainer, viewModel);
        }

        [HttpPost]
        public virtual ActionResult InsertAssociatedTrainer(GeneralTraineeAssociatedToTrainerViewModel generalTraineeAssociatedToTrainerViewModel)
        {
            if (ModelState.IsValid)
            {
                generalTraineeAssociatedToTrainerViewModel = _dBTMTraineeDetailsAgent.InsertAssociatedTrainer(generalTraineeAssociatedToTrainerViewModel);
                if (!generalTraineeAssociatedToTrainerViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    return RedirectToAction("GetAssociatedTrainerList", new { SelectedParameter1 = generalTraineeAssociatedToTrainerViewModel.EntityId, SelectedParameter2 = generalTraineeAssociatedToTrainerViewModel.PersonId });
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(generalTraineeAssociatedToTrainerViewModel.ErrorMessage));
            return View(createEditAssociatedTrainer, generalTraineeAssociatedToTrainerViewModel);
        }

        [HttpGet]
        public virtual ActionResult UpdateAssociatedTrainer(long generalTraineeAssociatedToTrainerId, long dBTMTraineeDetailId, long personId)
        {
            GeneralTraineeAssociatedToTrainerViewModel generalTraineeAssociatedToTrainerViewModel = _dBTMTraineeDetailsAgent.GetAssociatedTrainer(generalTraineeAssociatedToTrainerId);
            generalTraineeAssociatedToTrainerViewModel.DBTMTraineeDetailId = dBTMTraineeDetailId;
            generalTraineeAssociatedToTrainerViewModel.PersonId = personId;
            return ActionView(createEditAssociatedTrainer, generalTraineeAssociatedToTrainerViewModel);
        }

        [HttpPost]
        public virtual ActionResult UpdateAssociatedTrainer(GeneralTraineeAssociatedToTrainerViewModel generalTraineeAssociatedToTrainerViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_dBTMTraineeDetailsAgent.UpdateAssociatedTrainer(generalTraineeAssociatedToTrainerViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                return RedirectToAction("GetAssociatedTrainerList", new { SelectedParameter1 = generalTraineeAssociatedToTrainerViewModel.EntityId, SelectedParameter2 = generalTraineeAssociatedToTrainerViewModel.PersonId });
            }
            return View(createEditAssociatedTrainer, generalTraineeAssociatedToTrainerViewModel);
        }

        public virtual ActionResult DeleteAssociatedTrainer(string generalTraineeAssociatedToTrainerIds, string selectedCentreCode, short selectedDepartmentId)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(generalTraineeAssociatedToTrainerIds))
            {
                status = _dBTMTraineeDetailsAgent.DeleteAssociatedTrainer(generalTraineeAssociatedToTrainerIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction("GetAssociatedTrainerList", new { SelectedParameter1 = selectedCentreCode, SelectedParameter2 = selectedDepartmentId });
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction("GetAssociatedTrainerList", new { SelectedParameter1 = selectedCentreCode, SelectedParameter2 = selectedDepartmentId });
        }

        public virtual ActionResult GetTrainerList(string selectedCentreCode, string selectedDepartmentId, long entityId)
        {
            DropdownViewModel departmentDropdown = new DropdownViewModel()
            {
                DropdownType = DropdownTypeEnum.UnAssociatedTrainerList.ToString(),
                DropdownName = "GeneralTrainerMasterId",
                Parameter = $"{selectedCentreCode}~{selectedDepartmentId}~{entityId}~{UserTypeEnum.Trainee.ToString()}~false",
            };
            return PartialView("~/Views/Shared/Control/_DropdownList.cshtml", departmentDropdown);
        }

        public virtual ActionResult Cancel(string SelectedCentreCode, short selectedDepartmentId)
        {
            DataTableViewModel dataTableViewModel = new DataTableViewModel() { SelectedCentreCode = SelectedCentreCode, SelectedDepartmentId = selectedDepartmentId };
            return RedirectToAction("GetAssociatedTrainerList", dataTableViewModel);
        }
        #endregion TraineeAssociatedToTrainer

        public virtual ActionResult DBTMTraineeDetailsCancel(string SelectedCentreCode)
        {
            DataTableViewModel dataTableViewModel = new DataTableViewModel() { SelectedCentreCode = SelectedCentreCode };
            return RedirectToAction("List", dataTableViewModel);
        }

        #region Trainee Activities List
        public virtual ActionResult TraineeActivitiesList(DataTableViewModel dataTableModel)
        {
            DBTMActivitiesListViewModel list = _dBTMTraineeDetailsAgent.GetTraineeActivitiesList(Convert.ToString(dataTableModel.SelectedParameter1), 7, dataTableModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMActivities/_List.cshtml", list);
            }
            list.SelectedParameter1 = dataTableModel.SelectedParameter1;

            return View($"~/Views/DBTM/DBTMActivities/List.cshtml", list);
        }

        //Trainee Activities Details List
        public virtual ActionResult TraineeActivitiesDetailsList(DataTableViewModel dataTableModel)
        {
            DBTMActivitiesDetailsListViewModel list = _dBTMTraineeDetailsAgent.GetTraineeActivitiesDetailsList(Convert.ToInt64(dataTableModel.SelectedParameter1), dataTableModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMActivities/_DBTMActivitiesDetailsList.cshtml", list);
            }
            list.SelectedParameter1 = dataTableModel.SelectedParameter1;

            return View($"~/Views/DBTM/DBTMActivities/DBTMActivitiesDetailsList.cshtml", list);
        }

        public ActionResult GetTrainerByCentreCode(string centreCode)
        {
            DropdownViewModel trainerDropdown = new DropdownViewModel()
            {
                DropdownType = DropdownCustomTypeEnum.CentrewiseDBTMTrainer.ToString(),
                DropdownName = "GeneralTrainerMasterId",
                Parameter = centreCode,
                IsCustomDropdown = true,
                SelectedText = "All",
                SelectedValue = "0"
            };
            return PartialView("~/Views/Shared/Control/_DropdownList.cshtml", trainerDropdown);
        }
        #endregion
    }
}