using Coditech.Admin.Agents;
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
        public  ActionResult TrainerRegistration()
        {          
            return View("~/Views/DBTM/DBTMGeneralTrainerMaster/DBTMTrainerRegistration.cshtml", new DBTMNewRegistrationViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult TrainerRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel)
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
                if (ModelState.IsValid)
                {
                    dBTMNewRegistrationViewModel = _dBTMNewRegistrationAgent.TrainerRegistration(dBTMNewRegistrationViewModel);
                    if (!dBTMNewRegistrationViewModel.HasError)
                    {
                        SetNotificationMessage(GetSuccessNotificationMessage("Your Registration successfully."));
                        return RedirectToAction("List", "GeneralTrainerMaster");
                    }
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMNewRegistrationViewModel.ErrorMessage));
            return View("~/Views/DBTM/DBTMGeneralTrainerMaster/DBTMTrainerRegistration.cshtml", dBTMNewRegistrationViewModel);
        }
    }
}


