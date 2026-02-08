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

        public DBTMOrganisationCentreMasterController(IDBTMOrganisationCentreAgent dBTMOrganisationCentreAgent, IDBTMNewRegistrationAgent dBTMNewRegistrationAgent)
        {
            _dBTMOrganisationCentreAgent = dBTMOrganisationCentreAgent;
            _dBTMNewRegistrationAgent = dBTMNewRegistrationAgent;
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
        }
        #region Protected

        #endregion
    }
}
