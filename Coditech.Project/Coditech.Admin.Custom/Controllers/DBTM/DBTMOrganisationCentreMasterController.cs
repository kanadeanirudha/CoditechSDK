using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc;
namespace Coditech.Admin.Controllers
{
    public class DBTMOrganisationCentreMasterController : BaseController
    {
        private readonly IDBTMOrganisationCentreAgent _dBTMOrganisationCentreAgent;
        private readonly IDBTMNewRegistrationAgent _dBTMNewRegistrationAgent;
        private readonly IDBTMCentreWiseSettingAgent _dBTMCentreWiseSettingAgent;
        private const string createEdit = "~/Views/DBTM/DBTMCentreWiseSetting/CreateEdit.cshtml";

        public DBTMOrganisationCentreMasterController(IDBTMOrganisationCentreAgent dBTMOrganisationCentreAgent, IDBTMNewRegistrationAgent dBTMNewRegistrationAgent, IDBTMCentreWiseSettingAgent dBTMCentreWiseSettingAgent)
        {
            _dBTMOrganisationCentreAgent = dBTMOrganisationCentreAgent;
            _dBTMNewRegistrationAgent = dBTMNewRegistrationAgent;
            _dBTMCentreWiseSettingAgent = dBTMCentreWiseSettingAgent;
        }
        // Get Activity List View Sequence
        public virtual ActionResult ActivityListViewSequenceList(int organisationCentreId, DataTableViewModel dataTableViewModel)
        {
            DBTMActivityListViewSequenceListViewModel list = _dBTMOrganisationCentreAgent.GetActivityListViewSequenceList(organisationCentreId, dataTableViewModel);
            list.OrganisationCentreMasterId = organisationCentreId;
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMOrganisationCentreMaster/ActivityListViewCentrewise/_ActivityListViewCentrewiseList.cshtml", list);
            }
            return View($"~/Views/DBTM/DBTMOrganisationCentreMaster/ActivityListViewCentrewise/ActivityListViewCentrewiseList.cshtml", list);
        }

        [HttpGet]
        public ActionResult GetActivityListViewEditPopup(int dBTMTestParameterListViewSequenceId, string testName, string centreCode)
        {
            DBTMCentrewiseTestParameterListViewViewModel model = _dBTMOrganisationCentreAgent.GetDBTMCentrewiseTestParameterListView(dBTMTestParameterListViewSequenceId, centreCode);
            model.TestName = testName;
            model.CentreCode = centreCode;
            return PartialView("~/Views/DBTM/DBTMOrganisationCentreMaster/ActivityListViewCentrewise/_ActivityListViewEditPopup.cshtml", model);
        }

        [HttpPost]
        public virtual ActionResult GetActivityListViewEditPopup(DBTMCentrewiseTestParameterListViewViewModel dBTMTestViewModel)
        {
            if (ModelState.IsValid)
            {
                DBTMCentrewiseTestParameterListViewViewModel response = _dBTMOrganisationCentreAgent.UpdateDBTMCentrewiseTestParameterListView(dBTMTestViewModel);
                if (!response.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                    return Json(new { success = true, centreCode = dBTMTestViewModel.CentreCode });
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage));
            return Json(new { success = false, centreCode = dBTMTestViewModel.CentreCode });
        }

        // Centre Registration
        [HttpGet]
        public IActionResult DBTMCentreRegistration()
        {
            DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel = new DBTMNewRegistrationViewModel
            {
                IsAdminMode = true
            };
            return View("~/Views/DBTM/DBTMOrganisationCentreMaster/DBTMCentreRegistration.cshtml", dBTMNewRegistrationViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DBTMCentreRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("CentreCode");
            ModelState.Remove("TrainerSpecializationEnumId");
            ModelState.Remove("DateOfBirth");
            ModelState.Remove("SpecializationEnumId");
            ModelState.Remove("SelectedTrainer");
            ModelState.Remove("RegistrationType");
            dBTMNewRegistrationViewModel.IsAdminMode = true;
            if (ModelState.IsValid)
            {
                dBTMNewRegistrationViewModel = _dBTMNewRegistrationAgent.DBTMCentreRegistration(dBTMNewRegistrationViewModel);
                if (!dBTMNewRegistrationViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage("Centre added successfully."));
                    return RedirectToAction("List", "OrganisationCentreMaster");
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMNewRegistrationViewModel.ErrorMessage));
            return View("~/Views/DBTM/DBTMOrganisationCentreMaster/DBTMCentreRegistration.cshtml", dBTMNewRegistrationViewModel);
        } [HttpGet]
        public virtual ActionResult Update(int organisationCentreId)
        {
            DBTMCentreWiseSettingViewModel dBTMCentreWiseSettingViewModel = _dBTMCentreWiseSettingAgent.GetDBTMCentreWiseSetting(organisationCentreId);
            return ActionView(createEdit, dBTMCentreWiseSettingViewModel);
        }

        [HttpPost]
        public virtual ActionResult Update(DBTMCentreWiseSettingViewModel dBTMCentreWiseSettingViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_dBTMCentreWiseSettingAgent.UpdateDBTMCentreWiseSetting(dBTMCentreWiseSettingViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                if (string.Equals(dBTMCentreWiseSettingViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("Update", new { organisationCentreId = dBTMCentreWiseSettingViewModel.OrganisationCentreMasterId });
                }
                else if (string.Equals(dBTMCentreWiseSettingViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("List", "OrganisationCentreMaster");
                }
            }
            return View(createEdit, dBTMCentreWiseSettingViewModel);
        }

        [HttpGet]
        public ActionResult GetAssociateUnAssociateCentreTest(DBTMCentreWiseTestViewModel dBTMCentreWiseTestViewModel)
        {
            return PartialView("~/Views/DBTM/DBTMCentreWiseSetting/_AssociateUnAssociateCentrewiseTest.cshtml", dBTMCentreWiseTestViewModel);
        }

        [HttpPost]
        public ActionResult AssociateUnAssociateCentreTest(DBTMCentreWiseTestViewModel dBTMCentreWiseTestViewModel)
        {
            SetNotificationMessage(_dBTMCentreWiseSettingAgent.AssociateUnAssociateCentreTest(dBTMCentreWiseTestViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
            return RedirectToAction("Update", new { organisationCentreId = dBTMCentreWiseTestViewModel.OrganisationCentreMasterId });
        }

        #region Protected

        #endregion
    }
}
