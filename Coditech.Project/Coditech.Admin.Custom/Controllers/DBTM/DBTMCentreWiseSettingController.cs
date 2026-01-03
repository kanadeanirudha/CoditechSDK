using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc;
namespace Coditech.Admin.Controllers
{
    public class DBTMCentreWiseSettingController : BaseController
    {
        private readonly IDBTMCentreWiseSettingAgent _dBTMCentreWiseSettingAgent;
        private const string createEdit = "~/Views/DBTM/DBTMCentreWiseSetting/CreateEdit.cshtml";
        public DBTMCentreWiseSettingController(IDBTMCentreWiseSettingAgent dBTMCentreWiseSettingAgent)
        {
            _dBTMCentreWiseSettingAgent = dBTMCentreWiseSettingAgent;
        }

        [HttpGet]
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
                    return RedirectToAction(AdminConstants.ActionRedirectToList);
                }
            }
            return View(createEdit, dBTMCentreWiseSettingViewModel);
        }
    }
}
