using Coditech.Admin.Agents;
using Coditech.Admin.Helpers;
using Coditech.Admin.ViewModel;
using Coditech.Common.Helper.Utilities;
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
            if (!dBTMNewRegistrationViewModel.IsTermsAndCondition)
            {
                dBTMNewRegistrationViewModel.ErrorMessage = "Please accept Terms And Conditions.";
            }
            else
            {
                ModelState.Remove("CentreCode");
                ModelState.Remove("TrainerSpecializationEnumId");
                ModelState.Remove("JoiningCode");
                ModelState.Remove("Weight");
                ModelState.Remove("Height");
                ModelState.Remove("DateOfBirth");
                ModelState.Remove("SpecializationEnumId");
                ModelState.Remove("SelectedTrainer");
                if (ModelState.IsValid)
                {
                    dBTMNewRegistrationViewModel = _dBTMNewRegistrationAgent.DBTMCentreRegistration(dBTMNewRegistrationViewModel);
                    if (!dBTMNewRegistrationViewModel.HasError)
                    {
                        TempData["FormSizeClass"] = "col-lg-4";
                        SetNotificationMessage(GetSuccessNotificationMessage("You have registered successfully."));
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
            else if (string.IsNullOrWhiteSpace(dBTMNewRegistrationViewModel.Custom4))
            {
                dBTMNewRegistrationViewModel.ErrorMessage = "Total Number Of Students is requirred";
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
                if (ModelState.IsValid)
                {
                    dBTMNewRegistrationViewModel = _dBTMNewRegistrationAgent.TrainerRegistration(dBTMNewRegistrationViewModel);
                    if (!dBTMNewRegistrationViewModel.HasError)
                    {
                        TempData["FormSizeClass"] = "col-lg-4";
                        SetNotificationMessage(GetSuccessNotificationMessage("You have registered successfully."));
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
            if (string.IsNullOrEmpty(dBTMNewRegistrationViewModel.Custom1))
            {
                dBTMNewRegistrationViewModel.ErrorMessage = "Device serial code is required.";
            }
            if (!dBTMNewRegistrationViewModel.IsTermsAndCondition)
            {
                dBTMNewRegistrationViewModel.ErrorMessage = "Please accept Terms And Conditions.";
            }
            else
            {
                ModelState.Remove("DeviceSerialCode");
                ModelState.Remove("CentreName");
                ModelState.Remove("CentreCode");
                ModelState.Remove("JoiningCode");
                ModelState.Remove("SelectedTrainer");
                if (ModelState.IsValid)
                {
                    dBTMNewRegistrationViewModel = _dBTMNewRegistrationAgent.IndividualRegistration(dBTMNewRegistrationViewModel);
                    if (!dBTMNewRegistrationViewModel.HasError)
                    {
                        TempData["FormSizeClass"] = "col-lg-4";
                        SetNotificationMessage(GetSuccessNotificationMessage("You have registered successfully."));
                        return RedirectToAction("Login", "user");
                    }
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMNewRegistrationViewModel.ErrorMessage));
            return View("~/Views/DBTM/DBTMNewRegistration/DBTMIndividualRegistration.cshtml", dBTMNewRegistrationViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult TraineeRegistration(string joiningCode, long generalTrainerMasterId)
        {
            TempData["FormSizeClass"] = "col-lg-8";
            DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel = new DBTMNewRegistrationViewModel();
            if (!string.IsNullOrEmpty(joiningCode))
            {
                DBTMNewRegistrationListViewModel list = _dBTMNewRegistrationAgent.GetGeneralTrainerByJoiningCode(joiningCode, generalTrainerMasterId);
                if (!list.HasError)
                {
                    dBTMNewRegistrationViewModel = new DBTMNewRegistrationViewModel
                    {
                        JoiningCode = joiningCode,
                        AllTrainerList = CoditechCustomDropdownHelper.GeneralDropdownList(new DropdownViewModel
                        {
                            DropdownType = DropdownCustomTypeEnum.JoiningCodewiseGeneralTrainer.ToString(),
                            Parameter = joiningCode
                        }).DropdownList?.Where(x => x.Value != "")?.ToList()

                    };
                }
                if (list.HasError)
                {
                    SetNotificationMessage(GetErrorNotificationMessage(list.ErrorMessage));
                }
                return View("~/Views/DBTM/DBTMNewRegistration/DBTMTraineeRegistration.cshtml", dBTMNewRegistrationViewModel);
            }
            return View("~/Views/DBTM/DBTMNewRegistration/DBTMTraineeRegistration.cshtml", dBTMNewRegistrationViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public virtual ActionResult TraineeRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel)
        {
            TempData["FormSizeClass"] = "col-lg-8";
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
                    //var isTermsAndCondition = dBTMNewRegistrationViewModel.IsTermsAndCondition;

                    DBTMNewRegistrationListViewModel list = _dBTMNewRegistrationAgent.GetGeneralTrainerByJoiningCode(dBTMNewRegistrationViewModel.JoiningCode,dBTMNewRegistrationViewModel.GeneralTrainerMasterId);
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
                    //dBTMNewRegistrationViewModel.IsTermsAndCondition = isTermsAndCondition;
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
                ModelState.Remove("GeneralCityMasterId");
                ModelState.Remove("GeneralCountryMasterId");
                ModelState.Remove("GeneralRegionMasterId");
                ModelState.Remove("AddressLine1");
                ModelState.Remove("Pincode");
                if (ModelState.IsValid)
                {
                    dBTMNewRegistrationViewModel = _dBTMNewRegistrationAgent.TraineeRegistration(dBTMNewRegistrationViewModel);
                    if (!dBTMNewRegistrationViewModel.HasError)
                    {
                        TempData["FormSizeClass"] = "col-lg-4";
                        SetNotificationMessage(GetSuccessNotificationMessage("You have registered successfully."));
                        return RedirectToAction("Login", "user");
                    }
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMNewRegistrationViewModel.ErrorMessage));

            return View("~/Views/DBTM/DBTMNewRegistration/DBTMTraineeRegistration.cshtml", dBTMNewRegistrationViewModel);
        }
    }
}