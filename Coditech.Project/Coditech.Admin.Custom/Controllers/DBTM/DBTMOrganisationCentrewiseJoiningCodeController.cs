using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc;
namespace Coditech.Admin.Controllers
{
    public class DBTMOrganisationCentrewiseJoiningCodeController : BaseController
    {
        private readonly IDBTMOrganisationCentrewiseJoiningCodeAgent _dBTMOrganisationCentrewiseJoiningCodeAgent;
        private readonly IOrganisationCentrewiseJoiningCodeAgent _organisationCentrewiseJoiningCodeAgent;
        private const string createEdit = "~/Views/Organisation/OrganisationCentrewiseJoiningCode/CreateEdit.cshtml";
        public DBTMOrganisationCentrewiseJoiningCodeController(IDBTMOrganisationCentrewiseJoiningCodeAgent dBTMOrganisationCentrewiseJoiningCodeAgent, IOrganisationCentrewiseJoiningCodeAgent organisationCentrewiseJoiningCodeAgent)
        {
            _dBTMOrganisationCentrewiseJoiningCodeAgent = dBTMOrganisationCentrewiseJoiningCodeAgent;
            _organisationCentrewiseJoiningCodeAgent = organisationCentrewiseJoiningCodeAgent;
        }

        [HttpGet]
        public IActionResult DownloadTraineeJoiningCode(string centreCode)
        {
            if (string.IsNullOrEmpty(centreCode))
                return Content("Centre not selected.");
            DBTMOrganisationCentrewiseJoiningCodeViewModel result = _dBTMOrganisationCentrewiseJoiningCodeAgent.GetTraineeActiveJoiningCode(centreCode);
            if (result == null || string.IsNullOrEmpty(result.FilePath) || !System.IO.File.Exists(result.FilePath))
                return Content("File not found.");
            var bytes = System.IO.File.ReadAllBytes(result.FilePath);
            var fileName = result.FileName;
            _dBTMOrganisationCentrewiseJoiningCodeAgent.DeleteJoiningCodeFile(fileName);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        public IActionResult TrainerList(DataTableViewModel dataTableModel)
        {
            OrganisationCentrewiseJoiningCodeListViewModel list = new OrganisationCentrewiseJoiningCodeListViewModel();
            GetListOnlyIfSingleCentre(dataTableModel);
            if (!string.IsNullOrEmpty(dataTableModel.SelectedCentreCode))
            {
                dataTableModel.SelectedParameter1 = "323";
                list = _organisationCentrewiseJoiningCodeAgent.GetOrganisationCentrewiseJoiningCodeList(dataTableModel);
            }

            list.SelectedCentreCode = dataTableModel.SelectedCentreCode;
            list.SelectedParameter1 = dataTableModel.SelectedParameter1;
            list.SelectedParameter2 = dataTableModel.SelectedParameter2;
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/Organisation/OrganisationCentrewiseJoiningCode/_List.cshtml", list);
            }
            return View($"~/Views/Organisation/OrganisationCentrewiseJoiningCode/List.cshtml", list);
        }

        public IActionResult TraineeList(DataTableViewModel dataTableModel)
        {
            OrganisationCentrewiseJoiningCodeListViewModel list = new OrganisationCentrewiseJoiningCodeListViewModel();
            GetListOnlyIfSingleCentre(dataTableModel);
            if (!string.IsNullOrEmpty(dataTableModel.SelectedCentreCode))
            {
                dataTableModel.SelectedParameter1 = "324";
                list = _organisationCentrewiseJoiningCodeAgent.GetOrganisationCentrewiseJoiningCodeList(dataTableModel);
            }

            list.SelectedCentreCode = dataTableModel.SelectedCentreCode;
            list.SelectedParameter1 = dataTableModel.SelectedParameter1;
            list.SelectedParameter2 = dataTableModel.SelectedParameter2;

            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/Organisation/OrganisationCentrewiseJoiningCode/_List.cshtml", list);
            }
            return View($"~/Views/Organisation/OrganisationCentrewiseJoiningCode/List.cshtml", list);
        }

        [HttpGet]
        public ActionResult Create(int JoiningCodeTypeEnumId)
        {
            OrganisationCentrewiseJoiningCodeViewModel organisationCentrewiseJoiningCodeViewModel = new OrganisationCentrewiseJoiningCodeViewModel();
            organisationCentrewiseJoiningCodeViewModel.JoiningCodeTypeEnumId = JoiningCodeTypeEnumId;
            return View(createEdit, organisationCentrewiseJoiningCodeViewModel);
        }

        [HttpPost]
        public ActionResult Create(OrganisationCentrewiseJoiningCodeViewModel organisationCentrewiseJoiningCodeViewModel)
        {
            if (organisationCentrewiseJoiningCodeViewModel.IsReserved && string.IsNullOrEmpty(organisationCentrewiseJoiningCodeViewModel.ValidTillHours))
            {
                ModelState.AddModelError(nameof(organisationCentrewiseJoiningCodeViewModel.ValidTillHours), "Please select Expiry Time.");
            }
            if (ModelState.IsValid)
            {
                organisationCentrewiseJoiningCodeViewModel = _organisationCentrewiseJoiningCodeAgent.CreateOrganisationCentrewiseJoiningCode(organisationCentrewiseJoiningCodeViewModel);

                if (!organisationCentrewiseJoiningCodeViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    if (string.Equals(organisationCentrewiseJoiningCodeViewModel.ActionMode, AdminConstants.ActionModeSave, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("Create", new { CentreCode = organisationCentrewiseJoiningCodeViewModel.CentreCode, JoiningCodeTypeEnumId = organisationCentrewiseJoiningCodeViewModel.JoiningCodeTypeEnumId });
                    }
                    else if (string.Equals(organisationCentrewiseJoiningCodeViewModel.ActionMode, AdminConstants.ActionModeSaveAndClose, StringComparison.OrdinalIgnoreCase))
                    {
                        string returnList = organisationCentrewiseJoiningCodeViewModel.JoiningCodeTypeEnumId == 323 ?
                                            "trainerlist" : "traineelist";
                        return RedirectToAction(returnList, new { SelectedCentreCode = organisationCentrewiseJoiningCodeViewModel.CentreCode });
                    }
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(organisationCentrewiseJoiningCodeViewModel.ErrorMessage));
            return View(createEdit, organisationCentrewiseJoiningCodeViewModel);
        }
        #region Protected
        #endregion
    }
}
