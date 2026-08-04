using Coditech.Admin.Agents;
using Coditech.Admin.Helpers;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
namespace Coditech.Admin.Controllers
{
    public class DBTMTraineeDetailsController : BaseController
    {
        private readonly IDBTMTraineeDetailsAgent _dBTMTraineeDetailsAgent;
        private readonly IDBTMNewRegistrationAgent _dBTMNewRegistrationAgent;
        private readonly IDBTMOrganisationCentrewiseJoiningCodeAgent _dBTMOrganisationCentrewiseJoiningCodeAgent;
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        private const string createEditTraineeDetails = "~/Views/DBTM/DBTMTraineeDetails/DBTMTraineeDetails.cshtml";
        private const string createEditAssociatedTrainer = "~/Views/GeneralMaster/GeneralTrainerMaster/GeneralTraineeAssociatedToTrainer/CreateEditAssociatedTrainer.cshtml";
        public DBTMTraineeDetailsController(IDBTMTraineeDetailsAgent dBTMTraineeDetailsAgent, IDBTMNewRegistrationAgent dBTMNewRegistrationAgent, IDBTMOrganisationCentrewiseJoiningCodeAgent dBTMOrganisationCentrewiseJoiningCodeAgent, IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider)
        {
            _dBTMTraineeDetailsAgent = dBTMTraineeDetailsAgent;
            _dBTMNewRegistrationAgent = dBTMNewRegistrationAgent;
            _dBTMOrganisationCentrewiseJoiningCodeAgent = dBTMOrganisationCentrewiseJoiningCodeAgent;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        #region DBTMTraineeDetails

        public virtual ActionResult List(DataTableViewModel dataTableViewModel)
        {
            if (string.IsNullOrEmpty(dataTableViewModel.SelectedParameter1))
            {
                dataTableViewModel.SelectedParameter1 = "0";
            }
            DBTMTraineeDetailsListViewModel list = new DBTMTraineeDetailsListViewModel();
            GetListOnlyIfSingleCentre(dataTableViewModel);

            if (!string.IsNullOrEmpty(dataTableViewModel.SelectedCentreCode) && !string.IsNullOrEmpty(dataTableViewModel.SelectedParameter1))
            {
                UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);

                if (userModel?.Custom1 == CustomConstants.DBTMTrainer)
                {
                    dataTableViewModel.SelectedParameter1 = userModel.Custom1 == CustomConstants.DBTMTrainer ? (JsonConvert.DeserializeObject<DBTMCustomUserModel>(userModel.Custom3 ?? string.Empty)?.GeneralTrainerMasterId?.ToString() ?? "") : "";
                    list = _dBTMTraineeDetailsAgent.GetDBTMTraineeDetailsList(dataTableViewModel, "");
                }
                else
                {
                    list = _dBTMTraineeDetailsAgent.GetDBTMTraineeDetailsList(dataTableViewModel);
                }
            }

            list.SelectedCentreCode = dataTableViewModel.SelectedCentreCode;
            list.SelectedParameter1 = dataTableViewModel.SelectedParameter1;
            list.SelectedParameter2 = dataTableViewModel.SelectedParameter1;

            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMTraineeDetails/_List.cshtml", list);
            }

            return View("~/Views/DBTM/DBTMTraineeDetails/List.cshtml", list);
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
                    dataTableViewModel.SelectedParameter1 = userModel.Custom1 == CustomConstants.DBTMTrainer ? (JsonConvert.DeserializeObject<DBTMCustomUserModel>(userModel.Custom3 ?? string.Empty)?.GeneralTrainerMasterId?.ToString() ?? "") : "";
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
                    dataTableViewModel.SelectedParameter1 = userModel.Custom1 == CustomConstants.DBTMTrainer ? (JsonConvert.DeserializeObject<DBTMCustomUserModel>(userModel.Custom3 ?? string.Empty)?.GeneralTrainerMasterId?.ToString() ?? "") : "";
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
                    if (string.Equals(dBTMTraineeDetailsCreateEditViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("UpdateDBTMTraineePersonalDetails", new { dBTMTraineeDetailId = dBTMTraineeDetailsCreateEditViewModel.DBTMTraineeDetailId, personId = dBTMTraineeDetailsCreateEditViewModel.PersonId });
                    }
                    else if (string.Equals(dBTMTraineeDetailsCreateEditViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction(AdminConstants.ActionRedirectToList, new DataTableViewModel() { SelectedCentreCode = dBTMTraineeDetailsCreateEditViewModel.SelectedCentreCode });
                    }
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMTraineeDetailsCreateEditViewModel.ErrorMessage));
            return View(createEditTraineeDetails, dBTMTraineeDetailsCreateEditViewModel);
        }

        [HttpGet]
        public virtual ActionResult UpdateDBTMTraineePersonalDetails(long dBTMTraineeDetailId, long personId, string SelectedParameter2)
        {
            DBTMTraineeDetailsCreateEditViewModel dBTMTraineeDetailsCreateEditViewModel = _dBTMTraineeDetailsAgent.GetDBTMTraineePersonalDetails(dBTMTraineeDetailId, personId);
            dBTMTraineeDetailsCreateEditViewModel.UserType = UserTypeEnum.Trainee.ToString();
            dBTMTraineeDetailsCreateEditViewModel.SelectedParameter2 = SelectedParameter2;
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
                if (string.Equals(dBTMTraineeDetailsCreateEditViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("UpdateDBTMTraineePersonalDetails", new { dBTMTraineeDetailId = dBTMTraineeDetailsCreateEditViewModel.DBTMTraineeDetailId, personId = dBTMTraineeDetailsCreateEditViewModel.PersonId });
                }
                else if (string.Equals(dBTMTraineeDetailsCreateEditViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction(AdminConstants.ActionRedirectToList, new DataTableViewModel() { SelectedCentreCode = dBTMTraineeDetailsCreateEditViewModel.SelectedCentreCode });
                }
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
                if (string.Equals(dBTMTraineeDetailsViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("MemberOtherDetails", new { dBTMTraineeDetailId = dBTMTraineeDetailsViewModel.DBTMTraineeDetailId });
                }
                else if (string.Equals(dBTMTraineeDetailsViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction(AdminConstants.ActionRedirectToList, new DataTableViewModel() { SelectedCentreCode = dBTMTraineeDetailsViewModel.CentreCode });
                }
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
            list.SelectedParameter1 = dataTableViewModel.SelectedParameter1;
            list.SelectedParameter2 = dataTableViewModel.SelectedParameter2;
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/GeneralMaster/GeneralTrainerMaster/GeneralTraineeAssociatedToTrainer/_AssociatedTrainerList.cshtml", list);
            }

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
            if (string.IsNullOrEmpty(generalTraineeAssociatedToTrainerViewModel.SelectedDepartmentId))
            {
                SetNotificationMessage(GetErrorNotificationMessage("Please select department."));
            }
            else if (generalTraineeAssociatedToTrainerViewModel.GeneralTrainerMasterId == 0)
            {
                SetNotificationMessage(GetErrorNotificationMessage("Please select trainer."));
            }
            else
            {
                if (ModelState.IsValid)
                {
                    generalTraineeAssociatedToTrainerViewModel.IsCurrentTrainer = true;
                    generalTraineeAssociatedToTrainerViewModel = _dBTMTraineeDetailsAgent.InsertAssociatedTrainer(generalTraineeAssociatedToTrainerViewModel);
                    if (!generalTraineeAssociatedToTrainerViewModel.HasError)
                    {
                        SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                        if (string.Equals(generalTraineeAssociatedToTrainerViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                        {
                            return RedirectToAction("UpdateAssociatedTrainer", new { generalTraineeAssociatedToTrainerId = generalTraineeAssociatedToTrainerViewModel.GeneralTraineeAssociatedToTrainerId, dBTMTraineeDetailId = generalTraineeAssociatedToTrainerViewModel.DBTMTraineeDetailId, personId = generalTraineeAssociatedToTrainerViewModel.PersonId });
                        }
                        else if (string.Equals(generalTraineeAssociatedToTrainerViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                        {
                            return RedirectToAction("GetAssociatedTrainerList", new
                            {
                                SelectedParameter1 = generalTraineeAssociatedToTrainerViewModel.EntityId,
                                SelectedParameter2 = generalTraineeAssociatedToTrainerViewModel.PersonId
                            });
                        }
                    }
                }
                SetNotificationMessage(GetErrorNotificationMessage(generalTraineeAssociatedToTrainerViewModel.ErrorMessage));
            }
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
                generalTraineeAssociatedToTrainerViewModel.IsCurrentTrainer = true;
                SetNotificationMessage(_dBTMTraineeDetailsAgent.UpdateAssociatedTrainer(generalTraineeAssociatedToTrainerViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                if (string.Equals(generalTraineeAssociatedToTrainerViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("UpdateAssociatedTrainer", new { generalTraineeAssociatedToTrainerId = generalTraineeAssociatedToTrainerViewModel.GeneralTraineeAssociatedToTrainerId, dBTMTraineeDetailId = generalTraineeAssociatedToTrainerViewModel.DBTMTraineeDetailId, personId = generalTraineeAssociatedToTrainerViewModel.PersonId });
                }
                else if (string.Equals(generalTraineeAssociatedToTrainerViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("GetAssociatedTrainerList", new
                    {
                        SelectedParameter1 = generalTraineeAssociatedToTrainerViewModel.EntityId,
                        SelectedParameter2 = generalTraineeAssociatedToTrainerViewModel.PersonId
                    });
                }
            }
            return View(createEditAssociatedTrainer, generalTraineeAssociatedToTrainerViewModel);
        }

        [HttpGet]
        public ActionResult GetAssociateUnAssociateTrainer(GeneralTraineeAssociatedToTrainerViewModel model)
        {
            model.EntityId = model.DBTMTraineeDetailId > 0 ? model.DBTMTraineeDetailId : model.EntityId;
            return PartialView("~/Views/GeneralMaster/GeneralTrainerMaster/GeneralTraineeAssociatedToTrainer/_AssociateUnAssociateTrainer.cshtml", model);
        }

        [HttpPost]
        public ActionResult AssociateUnAssociateTrainer(GeneralTraineeAssociatedToTrainerViewModel model)
        {
            SetNotificationMessage(_dBTMTraineeDetailsAgent.AssociateUnAssociateTrainer(model).HasError
               ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
               : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
            return RedirectToAction("GetAssociatedTrainerList", new DataTableViewModel { SelectedParameter1 = model.DBTMTraineeDetailId.ToString(), SelectedParameter2 = model.PersonId.ToString() });
        }

        public virtual ActionResult DeleteAssociatedTrainer(string generalTraineeAssociatedToTrainerIds, string SelectedParameter1, string SelectedParameter2)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(generalTraineeAssociatedToTrainerIds))
            {
                status = _dBTMTraineeDetailsAgent.DeleteAssociatedTrainer(generalTraineeAssociatedToTrainerIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction("GetAssociatedTrainerList", new DataTableViewModel { SelectedParameter1 = SelectedParameter1, SelectedParameter2 = SelectedParameter2 });
            }
            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction("GetAssociatedTrainerList", new DataTableViewModel { SelectedParameter1 = SelectedParameter1, SelectedParameter2 = SelectedParameter2 });
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

        #endregion TraineeAssociatedToTrainer

        public virtual ActionResult DBTMTraineeDetailsCancel(string SelectedCentreCode)
        {
            DataTableViewModel dataTableViewModel = new DataTableViewModel() { SelectedCentreCode = SelectedCentreCode };
            return RedirectToAction("List", dataTableViewModel);
        }

        #region Trainee Activities List
        public virtual ActionResult TraineeActivitiesList(DataTableViewModel dataTableModel)
        {
            DBTMActivitiesListViewModel list = _dBTMTraineeDetailsAgent.GetTraineeActivitiesList(Convert.ToString(dataTableModel.SelectedParameter1), 0, dataTableModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMActivities/_List.cshtml", list);
            }
            list.SelectedParameter2 = dataTableModel.SelectedParameter2;

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
            list.SelectedParameter2 = dataTableModel.SelectedParameter2;

            return View($"~/Views/DBTM/DBTMActivities/DBTMActivitiesDetailsList.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult GetActivityDetailsPopup(long dBTMDeviceDataId, long trainerId)
        {
            DataTableViewModel dataTableModel = new DataTableViewModel();
            dataTableModel.SelectedParameter1 = dBTMDeviceDataId.ToString();
            dataTableModel.SelectedParameter2 = trainerId.ToString();
            DBTMActivitiesDetailsListViewModel model = _dBTMTraineeDetailsAgent.GetTraineeActivitiesDetailsList(dBTMDeviceDataId, dataTableModel);
            return PartialView("~/Views/DBTM/DBTMActivities/_ActivityDetailsPopup.cshtml", model);
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

        public virtual ActionResult Cancel(string SelectedCentreCode, string GeneralTrainerMasterId)
        {
            DataTableViewModel dataTableViewModel = new DataTableViewModel()
            {
                SelectedCentreCode = SelectedCentreCode,
                SelectedParameter1 = GeneralTrainerMasterId
            };
            return RedirectToAction("List", dataTableViewModel);
        }

        [HttpGet]
        public virtual ActionResult TraineeRegistration(string joiningCode, string custom1, long trainerId = 0, bool isChange = false)
        {
            DBTMNewRegistrationViewModel model = new DBTMNewRegistrationViewModel();
            UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
            if (trainerId == 0)
            {
                trainerId = JsonConvert.DeserializeObject<DBTMCustomUserModel>(userModel.Custom3 ?? "")?.GeneralTrainerMasterId ?? 0;
            }
            string joiningCodeTrainerId = string.Empty;
            if (string.IsNullOrEmpty(joiningCode) && userModel != null && !isChange)
            {
                OrganisationCentrewiseJoiningCodeViewModel joiningCodeDetails = _dBTMNewRegistrationAgent.GetJoiningCode(trainerId.ToString());
                if (joiningCodeDetails.HasError || string.IsNullOrEmpty(joiningCodeDetails.JoiningCode))
                {
                    SetNotificationMessage(GetErrorNotificationMessage("No Active Joining Code found for this trainer."));
                    return View("~/Views/DBTM/DBTMTraineeDetails/DBTMTraineeRegistration.cshtml", model);
                }
                joiningCode = joiningCodeDetails.JoiningCode;
                joiningCodeTrainerId = joiningCodeDetails.Custom1;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(joiningCode))
                {
                    DBTMNewRegistrationListViewModel list = _dBTMNewRegistrationAgent.GetGeneralTrainerByJoiningCode(joiningCode, 0);
                    if (list.HasError)
                    {
                        SetNotificationMessage(GetErrorNotificationMessage(list.ErrorMessage));
                        return View("~/Views/DBTM/DBTMTraineeDetails/DBTMTraineeRegistration.cshtml", model);
                    }
                    joiningCode = list.JoiningCode;
                    if (string.IsNullOrEmpty(joiningCodeTrainerId))
                    {
                        joiningCodeTrainerId = list.SelectedTrainerId;
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(joiningCode))
            {
                return View("~/Views/DBTM/DBTMTraineeDetails/DBTMTraineeRegistration.cshtml", model);
            }
            var allTrainerList = CoditechCustomDropdownHelper.GeneralDropdownList(new DropdownViewModel { DropdownType = DropdownCustomTypeEnum.JoiningCodewiseGeneralTrainer.ToString(), Parameter = joiningCode }).DropdownList?.Where(x => x.Value != "").ToList();
            List<string> selected = new List<string>();
            if (!string.IsNullOrEmpty(joiningCodeTrainerId))
            {
                selected.Add(joiningCodeTrainerId);
            }
            else if (trainerId > 0)
            {
                selected.Add(trainerId.ToString());
            }
            foreach (var item in allTrainerList)
            {
                item.Selected = selected.Contains(item.Value);
            }
            model = new DBTMNewRegistrationViewModel
            {
                JoiningCode = joiningCode,
                AllTrainerList = allTrainerList,
                SelectedTrainer = selected
            };
            return View("~/Views/DBTM/DBTMTraineeDetails/DBTMTraineeRegistration.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult TraineeRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel)
        {
            // Remove unnecessary model validations
            ModelState.Remove("CentreName");
            ModelState.Remove("CentreCode");
            ModelState.Remove("DeviceSerialCode");
            ModelState.Remove("GeneralCityMasterId");
            ModelState.Remove("GeneralCountryMasterId");
            ModelState.Remove("GeneralRegionMasterId");
            ModelState.Remove("AddressLine1");
            ModelState.Remove("Pincode");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("Password");

            // Main validation check
            if (!dBTMNewRegistrationViewModel.IsTermsAndCondition
                || !ModelState.IsValid
                || string.IsNullOrEmpty(dBTMNewRegistrationViewModel.JoiningCode))
            {
                if (!string.IsNullOrEmpty(dBTMNewRegistrationViewModel.JoiningCode))
                {
                    var isTermsAndCondition = dBTMNewRegistrationViewModel.IsTermsAndCondition;
                    var joiningCode = dBTMNewRegistrationViewModel.JoiningCode;
                    var generalTrainerMasterId = dBTMNewRegistrationViewModel.GeneralTrainerMasterId;

                    DBTMNewRegistrationListViewModel list = _dBTMNewRegistrationAgent.GetGeneralTrainerByJoiningCode(joiningCode, generalTrainerMasterId);

                    // ✅ Stop execution immediately if backend returned an error
                    if (list.HasError)
                    {
                        dBTMNewRegistrationViewModel.ErrorMessage = list.ErrorMessage
                            ?? "An unexpected error occurred while fetching trainer details.";

                        SetNotificationMessage(GetErrorNotificationMessage(dBTMNewRegistrationViewModel.ErrorMessage));
                        return View("~/Views/DBTM/DBTMTraineeDetails/DBTMTraineeRegistration.cshtml", dBTMNewRegistrationViewModel);
                    }

                    // ✅ Continue only if no error
                    dBTMNewRegistrationViewModel.AllTrainerList =
                        CoditechCustomDropdownHelper.GeneralDropdownList(new DropdownViewModel
                        {
                            DropdownType = DropdownCustomTypeEnum.JoiningCodewiseGeneralTrainer.ToString(),
                            Parameter = joiningCode
                        }).DropdownList?.Where(x => !string.IsNullOrEmpty(x.Value))?.ToList();

                    dBTMNewRegistrationViewModel.IsTermsAndCondition = isTermsAndCondition;
                    dBTMNewRegistrationViewModel.JoiningCode = joiningCode;
                }

                // Validation messages
                if (string.IsNullOrEmpty(dBTMNewRegistrationViewModel.JoiningCode))
                {
                    dBTMNewRegistrationViewModel.ErrorMessage = "Please enter a valid Joining Code.";
                }
                else if (!dBTMNewRegistrationViewModel.IsTermsAndCondition)
                {
                    dBTMNewRegistrationViewModel.ErrorMessage = "Please accept Terms and Conditions.";
                }

                SetNotificationMessage(GetErrorNotificationMessage(dBTMNewRegistrationViewModel.ErrorMessage));
                return View("~/Views/DBTM/DBTMTraineeDetails/DBTMTraineeRegistration.cshtml", dBTMNewRegistrationViewModel);
            }

            // Secondary validation
            ModelState.Remove("CentreName");
            ModelState.Remove("CentreCode");
            ModelState.Remove("DeviceSerialCode");
            if (ModelState.IsValid)
            {
                dBTMNewRegistrationViewModel = _dBTMNewRegistrationAgent.TraineeRegistration(dBTMNewRegistrationViewModel);

                if (!dBTMNewRegistrationViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage("You have registered successfully."));

                    if (string.Equals(dBTMNewRegistrationViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("UpdateDBTMTraineePersonalDetails",
                            new { dBTMTraineeDetailId = dBTMNewRegistrationViewModel.EntityId, personId = dBTMNewRegistrationViewModel.PersonId });
                    }
                    else if (string.Equals(dBTMNewRegistrationViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction(AdminConstants.ActionRedirectToList,
                            new DataTableViewModel() { SelectedCentreCode = dBTMNewRegistrationViewModel.CentreCode });
                    }
                }
            }

            SetNotificationMessage(GetErrorNotificationMessage(dBTMNewRegistrationViewModel.ErrorMessage));
            return View("~/Views/DBTM/DBTMTraineeDetails/DBTMTraineeRegistration.cshtml", dBTMNewRegistrationViewModel);
        }

        [HttpPost]
        public JsonResult ConvertCampUserToBatchUser(long dBTMTraineeDetailId)
        {
            bool status = _dBTMNewRegistrationAgent.ConvertCampUserToBatchUser(dBTMTraineeDetailId, out string message);
            return Json(new { success = status, message = message });
        }
        [HttpGet]
        public ActionResult GetConvertCampPopup(long dBTMTraineeDetailId)
        {
            ViewBag.TraineeId = dBTMTraineeDetailId;
            return PartialView("~/Views/DBTM/DBTMTraineeDetails/_ConvertCampPopup.cshtml");
        }
        #region Profilee
        [HttpGet]
        public virtual ActionResult Profile(long dBTMTraineeDetailId)
        {
            DBTMTraineeProfileViewModel dBTMTraineeProfileViewModel = _dBTMTraineeDetailsAgent.GetProfileDetails(dBTMTraineeDetailId);
            return View("~/Views/DBTM/DBTMTraineeDetails/Profile.cshtml", dBTMTraineeProfileViewModel);
        }

        [HttpGet]
        public JsonResult CheckAthleteReportAvailability(long dBTMTraineeDetailId, string remarks)
        {
            DBTMTraineeProfileViewModel profile = _dBTMTraineeDetailsAgent.GetProfileDetails(dBTMTraineeDetailId);
            if (profile == null)
            {
                return Json(new { success = false, message = "Profile not found." });
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public ActionResult GetRemarksPopup(long dBTMTraineeDetailId, string remarks)
        {
            ViewBag.TraineeId = dBTMTraineeDetailId;
            ViewBag.Remarks = remarks;
            return PartialView("~/Views/DBTM/DBTMTraineeDetails/_RemarksPopup.cshtml");
        }
        #endregion

        #region Upload Trainee
        //Get Upload Trainee Popup
        [HttpGet]
        public ActionResult GetUploadTraineePopup()
        {
            return PartialView("~/Views/DBTM/DBTMTraineeDetails/_UploadTraineePopup.cshtml");
        }

        [HttpPost]
        public JsonResult UploadTrainee(IFormFile file)
        {
            TempData.Remove("NotificationMessage");
            TempData.Remove("NotificationType");
            if (file == null || file.Length == 0)
                return Json(new { success = false, message = "File not selected." });
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (extension != ".xlsx")
            {
                return Json(new { success = false, message = "Only Excel (.xlsx) file is allowed." });
            }
            DBTMTraineeUploadResultViewModel result = _dBTMTraineeDetailsAgent.UploadTraineeFromFile(file);
            if (result.HasError)
            {
                return Json(new { success = false, message = result.ErrorMessage });
            }
            if (result.FailedRows != null && result.FailedRows.Count > 0)
            {
                return Json(new { success = false, message = "Data correction required.", failedRows = result.FailedRows, headers = result.Headers });
            }
            SetNotificationMessage(GetSuccessNotificationMessage("Trainee uploaded successfully."));
            return Json(new { success = true });
        }

        //Download Trainee Template
        [HttpGet]
        public IActionResult GetDownloadTemplatePopup()
        {
            return PartialView("~/Views/DBTM/DBTMTraineeDetails/_DownloadTraineeTemplatePopup.cshtml");
        }
        [HttpGet]
        public JsonResult CheckTraineeTemplateAvailability(string centreCode, long trainerId, string userType, int count)
        {
            DBTMTraineeUploadResultViewModel result = _dBTMTraineeDetailsAgent.DownloadTraineeUploadTemplate(centreCode, trainerId, userType, count);
            if (result.HasError)
                return Json(new { success = false, message = result.ErrorMessage });

            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult DownloadTraineeTemplate(string centreCode, long trainerId, string userType, int count)
        {
            DBTMTraineeUploadResultViewModel result = _dBTMTraineeDetailsAgent.DownloadTraineeUploadTemplate(centreCode, trainerId, userType, count);
            if (result == null || string.IsNullOrEmpty(result.FilePath) || !System.IO.File.Exists(result.FilePath))
                return Content("File not found.");
            if (result.HasError || string.IsNullOrEmpty(result.FilePath) || !System.IO.File.Exists(result.FilePath))
            {
                return Content(result.ErrorMessage ?? "File not found.");
            }
            var bytes = System.IO.File.ReadAllBytes(result.FilePath);
            var fileName = result.FileName;
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet]
        public ActionResult DownloadAthleteReportPdf(long dBTMTraineeDetailId, string remarks)
        {
            DBTMReportsListViewModel report = _dBTMTraineeDetailsAgent.GenerateAthletePdfRemark(dBTMTraineeDetailId, remarks);
            if (report == null)
                return Content("Athlete profile not found.");
            byte[] bytes;
            try
            {
                bytes = System.IO.File.ReadAllBytes(report.FilePath);
            }
            finally
            {
                if (System.IO.File.Exists(report.FilePath))
                {
                    System.IO.File.Delete(report.FilePath);
                }
            }
            return File(bytes, "application/pdf", report.FileName);
        }
        #endregion
    }
}