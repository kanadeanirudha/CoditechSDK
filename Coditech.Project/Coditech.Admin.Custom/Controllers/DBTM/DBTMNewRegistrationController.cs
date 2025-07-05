using Coditech.Admin.Agents;
using Coditech.Admin.Helpers;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

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
            if (!dBTMNewRegistrationViewModel.IsTermsAndCondition)
            {
                dBTMNewRegistrationViewModel.ErrorMessage = "Please accept Terms And Conditions.";
            }
            else
            {
                ModelState.Remove("CentreCode");
                ModelState.Remove("TrainerSpecializationEnumId");
                if (ModelState.IsValid)
                {
                    dBTMNewRegistrationViewModel = _dBTMNewRegistrationAgent.DBTMCentreRegistration(dBTMNewRegistrationViewModel);
                    if (!dBTMNewRegistrationViewModel.HasError)
                    {
                        TempData["FormSizeClass"] = "col-lg-4";
                        SetNotificationMessage(GetSuccessNotificationMessage("Your Registration successfully."));
                        return RedirectToAction("Login", "user");
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

            if (!dBTMNewRegistrationViewModel.IsTermsAndCondition)
            {
                dBTMNewRegistrationViewModel.ErrorMessage = "Please accept Terms And Conditions.";
            }
            else
            {
                ModelState.Remove("DeviceSerialCode");
                ModelState.Remove("CentreName");
                if (ModelState.IsValid)
                {
                    dBTMNewRegistrationViewModel = _dBTMNewRegistrationAgent.TrainerRegistration(dBTMNewRegistrationViewModel);
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

            if (!dBTMNewRegistrationViewModel.IsTermsAndCondition)
            {
                dBTMNewRegistrationViewModel.ErrorMessage = "Please accept Terms And Conditions.";
            }
            else
            {
                ModelState.Remove("DeviceSerialCode");
                ModelState.Remove("CentreName");
                ModelState.Remove("CentreCode");
                if (ModelState.IsValid)
                {
                    dBTMNewRegistrationViewModel = _dBTMNewRegistrationAgent.IndividualRegistration(dBTMNewRegistrationViewModel);
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

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult TraineeRegistration(string JoiningCode)
        {
            TempData["FormSizeClass"] = "col-lg-8";

            DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel = new DBTMNewRegistrationViewModel
            {
                JoiningCode = JoiningCode,
                SelectedTrainer = new List<string>(), // if needed
                AllTrainerList = CoditechCustomDropdownHelper.GeneralDropdownList(new DropdownViewModel
                {
                    DropdownType = DropdownCustomTypeEnum.JoiningCodewiseGeneralTrainer.ToString(),
                    Parameter = JoiningCode
                }).DropdownList?.Where(x => !string.IsNullOrEmpty(x.Value))?.ToList()
            };
            dBTMNewRegistrationViewModel.AllTrainerList = CoditechCustomDropdownHelper.GeneralDropdownList(new DropdownViewModel
            {
                DropdownType = DropdownCustomTypeEnum.JoiningCodewiseGeneralTrainer.ToString(),
                Parameter = JoiningCode,
            }).DropdownList?.Where(x => x.Value != "")?.ToList();

            return View("~/Views/DBTM/DBTMNewRegistration/DBTMTraineeRegistration.cshtml", dBTMNewRegistrationViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public virtual ActionResult TraineeRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel)
        {
            TempData["FormSizeClass"] = "col-lg-8";

            if (!dBTMNewRegistrationViewModel.IsTermsAndCondition)
            {
                dBTMNewRegistrationViewModel.ErrorMessage = "Please accept Terms And Conditions.";
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
                        TempData["FormSizeClass"] = "col-lg-4";
                        SetNotificationMessage(GetSuccessNotificationMessage("Your Registration successfully."));
                        return RedirectToAction("Login", "user");
                    }
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMNewRegistrationViewModel.ErrorMessage));
            return View("~/Views/DBTM/DBTMNewRegistration/DBTMTraineeRegistration.cshtml", dBTMNewRegistrationViewModel);
        }
    }
}