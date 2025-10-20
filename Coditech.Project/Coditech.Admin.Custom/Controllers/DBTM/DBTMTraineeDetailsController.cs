using Coditech.Admin.Agents;
using Coditech.Admin.Helpers;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace Coditech.Admin.Controllers
{
    public class DBTMTraineeDetailsController : BaseController
    {
        private readonly IDBTMTraineeDetailsAgent _dBTMTraineeDetailsAgent;
        private readonly IDBTMNewRegistrationAgent _dBTMNewRegistrationAgent;
        private const string createEditTraineeDetails = "~/Views/DBTM/DBTMTraineeDetails/DBTMTraineeDetails.cshtml";
        private const string createEditAssociatedTrainer = "~/Views/GeneralMaster/GeneralTrainerMaster/GeneralTraineeAssociatedToTrainer/CreateEditAssociatedTrainer.cshtml";
        public DBTMTraineeDetailsController(IDBTMTraineeDetailsAgent dBTMTraineeDetailsAgent, IDBTMNewRegistrationAgent dBTMNewRegistrationAgent)
        {
            _dBTMTraineeDetailsAgent = dBTMTraineeDetailsAgent;
            _dBTMNewRegistrationAgent = dBTMNewRegistrationAgent;
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
        public virtual ActionResult TraineeRegistration(string joiningCode, string custom1)
        {
            DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel = new DBTMNewRegistrationViewModel();

            if (!string.IsNullOrEmpty(joiningCode))
            {
                var list = _dBTMNewRegistrationAgent.GetGeneralTrainerByJoiningCode(joiningCode);

                if (!list.HasError)
                {
                    var allTrainerList = CoditechCustomDropdownHelper.GeneralDropdownList(new DropdownViewModel
                    {
                        DropdownType = DropdownCustomTypeEnum.JoiningCodewiseGeneralTrainer.ToString(),
                        Parameter = joiningCode
                    }).DropdownList?.Where(x => x.Value != "").ToList();

                    // Multi-select: set Selected property based on custom1 (comma-separated)
                    if (!string.IsNullOrEmpty(custom1) && allTrainerList != null)
                    {
                        var selectedIds = custom1.Split(','); // e.g., "7,8,12"
                        foreach (var item in allTrainerList)
                        {
                            item.Selected = selectedIds.Contains(item.Value);
                        }
                    }

                    dBTMNewRegistrationViewModel = new DBTMNewRegistrationViewModel
                    {
                        JoiningCode = joiningCode,
                        AllTrainerList = allTrainerList,
                        SelectedTrainer = !string.IsNullOrEmpty(custom1) ? custom1.Split(',').ToList() : new List<string>()
                    };
                }
                if (list.HasError)
                {
                    SetNotificationMessage(GetErrorNotificationMessage(list.ErrorMessage));
                }
                return View("~/Views/DBTM/DBTMTraineeDetails/DBTMTraineeRegistration.cshtml", dBTMNewRegistrationViewModel);
            }
            return View("~/Views/DBTM/DBTMTraineeDetails/DBTMTraineeRegistration.cshtml", dBTMNewRegistrationViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult TraineeRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel)
        {
            ModelState.Remove("CentreName");
            ModelState.Remove("CentreCode");
            ModelState.Remove("DeviceSerialCode");
            ModelState.Remove("GeneralCityMasterId");
            ModelState.Remove("GeneralCountryMasterId");
            ModelState.Remove("GeneralRegionMasterId");
            ModelState.Remove("AddressLine1");
            ModelState.Remove("Pincode");
            if (!dBTMNewRegistrationViewModel.IsTermsAndCondition || !ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(dBTMNewRegistrationViewModel.JoiningCode))
                {
                    //var generalcountrymasterid = dBTMNewRegistrationViewModel.GeneralCountryMasterId;
                    //var generalcitymasterid = dBTMNewRegistrationViewModel.GeneralCityMasterId;
                    //var regionmasterid = dBTMNewRegistrationViewModel.GeneralRegionMasterId;
                    var isTermsAndCondition = dBTMNewRegistrationViewModel.IsTermsAndCondition;

                    DBTMNewRegistrationListViewModel list = _dBTMNewRegistrationAgent.GetGeneralTrainerByJoiningCode(dBTMNewRegistrationViewModel.JoiningCode);
                    if (!list.HasError)
                    {
                        dBTMNewRegistrationViewModel = new DBTMNewRegistrationViewModel
                        {
                            JoiningCode = dBTMNewRegistrationViewModel.JoiningCode,
                            AllTrainerList = CoditechCustomDropdownHelper.GeneralDropdownList(new DropdownViewModel
                            {
                                DropdownType = DropdownCustomTypeEnum.JoiningCodewiseGeneralTrainer.ToString(),
                                Parameter = dBTMNewRegistrationViewModel.JoiningCode
                            }).DropdownList?.Where(x => x.Value != "")?.ToList()

                        };
                    }
                    //dBTMNewRegistrationViewModel.GeneralRegionMasterId = regionmasterid;
                    //dBTMNewRegistrationViewModel.GeneralCountryMasterId = generalcountrymasterid;
                    //dBTMNewRegistrationViewModel.GeneralCityMasterId = generalcitymasterid;
                    dBTMNewRegistrationViewModel.IsTermsAndCondition = isTermsAndCondition;
                }

                if (!dBTMNewRegistrationViewModel.IsTermsAndCondition)
                {
                    dBTMNewRegistrationViewModel.ErrorMessage = "Please accept Terms And Conditions.";
                }
            }
            else
            {
                ModelState.Remove("CentreName");
                ModelState.Remove("CentreCode");
                ModelState.Remove("DeviceSerialCode");
                if (ModelState.IsValid)
                {
                    dBTMNewRegistrationViewModel = _dBTMNewRegistrationAgent.TraineeRegistration(dBTMNewRegistrationViewModel);
                    if (!dBTMNewRegistrationViewModel.HasError)
                    {
                        SetNotificationMessage(GetSuccessNotificationMessage("You have registered successfully."));
                        return RedirectToAction("List", "DBTMTraineeDetails");
                    }
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMNewRegistrationViewModel.ErrorMessage));
            return View("~/Views/DBTM/DBTMTraineeDetails/DBTMTraineeRegistration.cshtml", dBTMNewRegistrationViewModel);
        }
        #region Profilee
        [HttpGet]
        public virtual ActionResult Profile(long dBTMTraineeDetailId)
        {
            DBTMTraineeProfileViewModel dBTMTraineeProfileViewModel = _dBTMTraineeDetailsAgent.GetProfileDetails(dBTMTraineeDetailId);
            return View("~/Views/DBTM/DBTMTraineeDetails/Profile.cshtml", dBTMTraineeProfileViewModel);
        }
        #endregion
    }
}