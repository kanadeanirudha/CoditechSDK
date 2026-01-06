using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Coditech.Admin.Controllers
{
    public class DBTMOrganisationCentreMasterController : BaseController
    {
        private readonly IDBTMOrganisationCentreAgent _dBTMOrganisationCentreAgent;

        public DBTMOrganisationCentreMasterController(IDBTMOrganisationCentreAgent dBTMOrganisationCentreAgent)
        {
            _dBTMOrganisationCentreAgent = dBTMOrganisationCentreAgent;
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
        public ActionResult GetActivityListViewEditPopup( int dBTMTestParameterListViewSequenceId, string testName, string centreCode)
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
                    return Json(new { success = true, centreCode = dBTMTestViewModel.CentreCode});
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage));
            return Json(new { success = false, centreCode = dBTMTestViewModel.CentreCode});
        }

        #region Protected

        #endregion
    }
}
