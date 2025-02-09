using Coditech.Admin.Agents;
using Coditech.Admin.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coditech.Admin.Controllers
{
    public class DBTMNewRegistrationController : BaseController
    {
        private readonly IDBTMNewRegistrationAgent _dBTMNewRegistrationAgent;

        public DBTMNewRegistrationController(IDBTMNewRegistrationAgent dBTMNewRegistrationAgent)
        {
            _dBTMNewRegistrationAgent = dBTMNewRegistrationAgent;
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult CentreRegistration()
        {
            TempData["FormSizeClass"] = "col-lg-8";
            return View("~/Views/DBTM/DBTMNewRegistration/DBTMCentreRegistration.cshtml", new DBTMNewRegistrationViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public virtual ActionResult CentreRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel)
        {
            TempData["FormSizeClass"] = "col-lg-8";
            if (!dBTMNewRegistrationViewModel.IsTermsAndCondition) {
                dBTMNewRegistrationViewModel.ErrorMessage = "Please accept Terms And Conditions.";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    dBTMNewRegistrationViewModel = _dBTMNewRegistrationAgent.DBTMCentreRegistration(dBTMNewRegistrationViewModel);
                    if (!dBTMNewRegistrationViewModel.HasError)
                    {
                        TempData["FormSizeClass"] = "col-lg-4";
                        SetNotificationMessage(GetSuccessNotificationMessage("Your Registration successfully."));
                        return RedirectToAction("Login","user");
                    }
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMNewRegistrationViewModel.ErrorMessage));
            return View("~/Views/DBTM/DBTMNewRegistration/DBTMCentreRegistration.cshtml", dBTMNewRegistrationViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult TrainerRegistration()
        {
            TempData["FormSizeClass"] = "col-lg-8";
            return View("~/Views/DBTM/DBTMNewRegistration/DBTMTrainerRegistration.cshtml", new DBTMNewRegistrationViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public virtual ActionResult TrainerRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel)
        {
            TempData["FormSizeClass"] = "col-lg-8";
           
                dBTMNewRegistrationViewModel.CentreName = "DBTMCentre";
            if (!dBTMNewRegistrationViewModel.IsTermsAndCondition)
            {
                dBTMNewRegistrationViewModel.ErrorMessage = "Please accept Terms And Conditions.";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    dBTMNewRegistrationViewModel = new DBTMNewRegistrationViewModel();// _dBTMNewRegistrationAgent.TrainerRegistration(dBTMNewRegistrationViewModel);
                    if (!dBTMNewRegistrationViewModel.HasError)
                    {
                        TempData["FormSizeClass"] = "col-lg-4";
                        SetNotificationMessage(GetSuccessNotificationMessage("Your Registration successfully."));
                        return RedirectToAction("Login", "user");
                    }
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMNewRegistrationViewModel.ErrorMessage));
            return View("~/Views/DBTM/DBTMNewRegistration/DBTMTrainerRegistration.cshtml", dBTMNewRegistrationViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult IndividualRegistration()
        {
            TempData["FormSizeClass"] = "col-lg-8";
            return View("~/Views/DBTM/DBTMNewRegistration/DBTMIndividualRegistration.cshtml", new DBTMNewRegistrationViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public virtual ActionResult IndividualRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel)
        {
            TempData["FormSizeClass"] = "col-lg-8";

            dBTMNewRegistrationViewModel.CentreName = "DBTMCentre";
            if (!dBTMNewRegistrationViewModel.IsTermsAndCondition)
            {
                dBTMNewRegistrationViewModel.ErrorMessage = "Please accept Terms And Conditions.";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    dBTMNewRegistrationViewModel = new DBTMNewRegistrationViewModel();// _dBTMNewRegistrationAgent.IndividualRegistration(dBTMNewRegistrationViewModel);
                    if (!dBTMNewRegistrationViewModel.HasError)
                    {
                        TempData["FormSizeClass"] = "col-lg-4";
                        SetNotificationMessage(GetSuccessNotificationMessage("Your Registration successfully."));
                        return RedirectToAction("Login", "user");
                    }
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMNewRegistrationViewModel.ErrorMessage));
            return View("~/Views/DBTM/DBTMNewRegistration/DBTMIndividualRegistration.cshtml", dBTMNewRegistrationViewModel);
        }
    }
}