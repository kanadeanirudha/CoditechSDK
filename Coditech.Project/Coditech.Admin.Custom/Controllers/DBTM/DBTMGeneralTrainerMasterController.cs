using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Microsoft.AspNetCore.Mvc;
namespace Coditech.Admin.Controllers
{
    public class DBTMGeneralTrainerMasterController : BaseController
    {
        private readonly IDBTMNewRegistrationAgent _dBTMNewRegistrationAgent;

        public DBTMGeneralTrainerMasterController(IDBTMNewRegistrationAgent dBTMNewRegistrationAgent)
        {
            _dBTMNewRegistrationAgent = dBTMNewRegistrationAgent;
        }

        [HttpGet]
        public ActionResult TrainerRegistration(string joiningCode)
        {
            DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel = new DBTMNewRegistrationViewModel();
            dBTMNewRegistrationViewModel.CentreCode = joiningCode;
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
                if (ModelState.IsValid)
                {
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


