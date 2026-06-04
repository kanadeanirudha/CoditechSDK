using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
using Microsoft.AspNetCore.Mvc;
namespace Coditech.Admin.Controllers
{
    public class DBTMGeneralTrainerMasterController : BaseController
    {
        private readonly IDBTMNewRegistrationAgent _dBTMNewRegistrationAgent;
        private readonly IDBTMOrganisationCentrewiseJoiningCodeAgent _dBTMOrganisationCentrewiseJoiningCodeAgent;

        public DBTMGeneralTrainerMasterController(IDBTMNewRegistrationAgent dBTMNewRegistrationAgent, IDBTMOrganisationCentrewiseJoiningCodeAgent dBTMOrganisationCentrewiseJoiningCodeAgent)
        {
            _dBTMNewRegistrationAgent = dBTMNewRegistrationAgent;
            _dBTMOrganisationCentrewiseJoiningCodeAgent = dBTMOrganisationCentrewiseJoiningCodeAgent;
        }

        [HttpGet]
        public ActionResult TrainerRegistration(string joiningCode)
        {
            DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel = new DBTMNewRegistrationViewModel();
            if (!string.IsNullOrEmpty(joiningCode))
            {
                dBTMNewRegistrationViewModel.JoiningCode = joiningCode;
            }
            else
            {
                UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
                if (userModel != null)
                {
                    DBTMOrganisationCentrewiseJoiningCodeViewModel result = _dBTMOrganisationCentrewiseJoiningCodeAgent.GetTrainerActiveJoiningCode(userModel.SelectedCentreCode);
                    if (result != null && !string.IsNullOrEmpty(result.JoiningCode))
                    {
                        dBTMNewRegistrationViewModel.JoiningCode = result.JoiningCode;
                    }
                    else
                    {
                        SetNotificationMessage(GetErrorNotificationMessage("No Joining Code found for this trainer."));
                        return RedirectToAction("List", "GeneralTrainerMaster");
                    }
                }
            }
            return View("~/Views/DBTM/DBTMGeneralTrainerMaster/DBTMTrainerRegistration.cshtml", dBTMNewRegistrationViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TrainerRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel)
        {
            if (!dBTMNewRegistrationViewModel.IsTermsAndCondition)
            {
                dBTMNewRegistrationViewModel.ErrorMessage = "Please accept Terms And Conditions.";
            }
            else
            {
                ModelState.Remove("DeviceSerialCode");
                ModelState.Remove("CentreName");
                ModelState.Remove("CentreCode");
                ModelState.Remove("DateOfBirth");
                ModelState.Remove("JoiningCode");
                ModelState.Remove("SpecializationEnumId");
                ModelState.Remove("SelectedTrainer");
                ModelState.Remove("GeneralCityMasterId");
                ModelState.Remove("GeneralCountryMasterId");
                ModelState.Remove("GeneralRegionMasterId");
                ModelState.Remove("AddressLine1");
                ModelState.Remove("Pincode");
                ModelState.Remove("ConfirmPassword");
                ModelState.Remove("Password");
                ModelState.Remove("RegistrationType");
                if (ModelState.IsValid)
                {
                    dBTMNewRegistrationViewModel.CentreCode = dBTMNewRegistrationViewModel.JoiningCode;
                    dBTMNewRegistrationViewModel = _dBTMNewRegistrationAgent.TrainerRegistration(dBTMNewRegistrationViewModel);
                    if (!dBTMNewRegistrationViewModel.HasError)
                    {
                        SetNotificationMessage(GetSuccessNotificationMessage("You have registered successfully."));
                        if (string.Equals(dBTMNewRegistrationViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                        {
                            return RedirectToAction("Edit", "GeneralTrainerMaster", new { generalTrainerId = dBTMNewRegistrationViewModel.GeneralTrainerMasterId});
                        }
                        else if (string.Equals(dBTMNewRegistrationViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                        {
                            return RedirectToAction("List", "GeneralTrainerMaster", new DataTableViewModel() { SelectedCentreCode = dBTMNewRegistrationViewModel.CentreCode, SelectedDepartmentId = Convert.ToInt16(dBTMNewRegistrationViewModel.Custom5) });
                        }
                    }
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMNewRegistrationViewModel.ErrorMessage));
            return View("~/Views/DBTM/DBTMGeneralTrainerMaster/DBTMTrainerRegistration.cshtml", dBTMNewRegistrationViewModel);
        }
    }
}


