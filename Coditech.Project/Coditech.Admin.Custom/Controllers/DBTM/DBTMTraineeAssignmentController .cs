using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Coditech.Resources;

using Microsoft.AspNetCore.Mvc;

namespace Coditech.Admin.Controllers
{
    public class DBTMTraineeAssignmentController : BaseController
    {
        private readonly IDBTMTraineeAssignmentAgent _dBTMTraineeAssignmentAgent;
        private const string createEdit = "~/Views/DBTM/DBTMTraineeAssignment/CreateEdit.cshtml";

        public DBTMTraineeAssignmentController(IDBTMTraineeAssignmentAgent dBTMTraineeAssignmentAgent)
        {
            _dBTMTraineeAssignmentAgent = dBTMTraineeAssignmentAgent;
        }

        public virtual ActionResult List(DataTableViewModel dataTableModel)
        {
            DBTMTraineeAssignmentListViewModel list = new DBTMTraineeAssignmentListViewModel();
            GetListOnlyIfSingleCentre(dataTableModel);
            if (!string.IsNullOrEmpty(dataTableModel.SelectedCentreCode) && !string.IsNullOrEmpty(dataTableModel.SelectedParameter1))
            {
                list = _dBTMTraineeAssignmentAgent.GetDBTMTraineeAssignmentList(dataTableModel);
            }
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMTraineeAssignment/_List.cshtml", list);
            }
            list.SelectedCentreCode = dataTableModel.SelectedCentreCode;

            return View($"~/Views/DBTM/DBTMTraineeAssignment/List.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            DBTMTraineeAssignmentViewModel dBTMTraineeAssignmentViewModel = new DBTMTraineeAssignmentViewModel()
            {
                SelectedCentreCode = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession)?.SelectedCentreCode
            };
            return View(createEdit, dBTMTraineeAssignmentViewModel);
        }

        [HttpPost]
        public virtual ActionResult Create(DBTMTraineeAssignmentViewModel dBTMTraineeAssignmentViewModel)
        {
            if (ModelState.IsValid)
            {
                dBTMTraineeAssignmentViewModel = _dBTMTraineeAssignmentAgent.CreateDBTMTraineeAssignment(dBTMTraineeAssignmentViewModel);
                if (!dBTMTraineeAssignmentViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    return RedirectToAction("List", new DataTableViewModel { SelectedCentreCode = dBTMTraineeAssignmentViewModel.SelectedCentreCode, SelectedParameter1 = Convert.ToString(dBTMTraineeAssignmentViewModel.GeneralTrainerMasterId) });
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMTraineeAssignmentViewModel.ErrorMessage));
            return View(createEdit, dBTMTraineeAssignmentViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(long dBTMTraineeAssignmentId)
        {
            DBTMTraineeAssignmentViewModel dBTMTraineeAssignmentViewModel = _dBTMTraineeAssignmentAgent.GetDBTMTraineeAssignment(dBTMTraineeAssignmentId);
            return ActionView(createEdit, dBTMTraineeAssignmentViewModel);
        }

        [HttpPost]
        public virtual ActionResult Edit(DBTMTraineeAssignmentViewModel dBTMTraineeAssignmentViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_dBTMTraineeAssignmentAgent.UpdateDBTMTraineeAssignment(dBTMTraineeAssignmentViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                return RedirectToAction("Edit", new { dBTMTraineeAssignmentId = dBTMTraineeAssignmentViewModel.DBTMTraineeAssignmentId });
            }
            return View(createEdit, dBTMTraineeAssignmentViewModel);
        }

        public virtual ActionResult Delete(string dBTMTraineeAssignmentIds, string selectedCentreCode)
        {
            string message = string.Empty;
            bool status = false;

            if (!string.IsNullOrEmpty(dBTMTraineeAssignmentIds))
            {
                status = _dBTMTraineeAssignmentAgent.DeleteDBTMTraineeAssignment(dBTMTraineeAssignmentIds, out message);

                SetNotificationMessage(!status
                    ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                    : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction("List", new DataTableViewModel { SelectedCentreCode = selectedCentreCode });
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction("List", new DataTableViewModel { SelectedCentreCode = selectedCentreCode });
        }

        public virtual ActionResult SendAssignmentReminder(long dBTMTraineeAssignmentId)
        {

            DBTMTraineeAssignmentViewModel model = new DBTMTraineeAssignmentViewModel();

            model = _dBTMTraineeAssignmentAgent.SendAssignmentReminder(dBTMTraineeAssignmentId);

            if (!model.HasError)
            {
                SetNotificationMessage(GetSuccessNotificationMessage("Send Reminder Assignment Successfully."));
            }
            else
            {
                SetNotificationMessage(GetErrorNotificationMessage(model.ErrorMessage));
            }
            return RedirectToAction("List", new DataTableViewModel { SelectedCentreCode = model.SelectedCentreCode, SelectedParameter1 = Convert.ToString(model.GeneralTrainerMasterId) });
        }

        public ActionResult GetTrainerByCentreCode(string centreCode)
        {
            DropdownViewModel trainerDropdown = new DropdownViewModel()
            {
                DropdownType = DropdownCustomTypeEnum.CentrewiseDBTMTrainer.ToString(),
                DropdownName = "GeneralTrainerMasterId",
                Parameter = centreCode,
                IsCustomDropdown = true
            };
            return PartialView("~/Views/Shared/Control/_DropdownList.cshtml", trainerDropdown);
        }

        public virtual ActionResult GetTraineeDetailByCentreCodeAndgeneralTrainerId(string centreCode, long generalTrainerId)
        {
            DropdownViewModel traineeDetailsDropdown = new DropdownViewModel()
            {
                DropdownType = DropdownCustomTypeEnum.TraineeDetailsListByDBTMTrainer.ToString(),
                DropdownName = "DBTMTraineeDetailId",
                Parameter = $"{centreCode}~{generalTrainerId}",
                IsCustomDropdown = true
            };
            return PartialView("~/Views/Shared/Control/_DropdownList.cshtml", traineeDetailsDropdown);
        }

        public virtual ActionResult Cancel(string SelectedCentreCode, string GeneralTrainerMasterId)
        {
            DataTableViewModel dataTableViewModel = new DataTableViewModel() { SelectedCentreCode = SelectedCentreCode, SelectedParameter1 = GeneralTrainerMasterId };
            return RedirectToAction("List", dataTableViewModel);
        }

        #region Assignmnet User
        public virtual ActionResult GetDBTMTraineeAssignmentToUserList(DataTableViewModel dataTableViewModel)
        {
            DBTMTraineeAssignmentToUserListViewModel list = _dBTMTraineeAssignmentAgent.GetDBTMTraineeAssignmentToUserList(Convert.ToInt64(dataTableViewModel.SelectedParameter1), dataTableViewModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMTraineeAssignment/DBTMTraineeAssignmentUser/_AssociatedAssignmentList.cshtml", list);
            }
            list.SelectedParameter1 = dataTableViewModel.SelectedParameter1;

            return View($"~/Views/DBTM/DBTMTraineeAssignment/DBTMTraineeAssignmentUser/AssociatedAssignmentList.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult GetAssociateUnAssociateAssignmentwiseUser(DBTMTraineeAssignmentToUserViewModel DBTMTraineeAssignmentToUserViewModel)
        {
            return PartialView("~/Views/DBTM/DBTMTraineeAssignment/DBTMTraineeAssignmentUser/_AssociateUnAssociateAssignmentwiseUser.cshtml", DBTMTraineeAssignmentToUserViewModel);
        }

        [HttpPost]
        public virtual ActionResult AssociateUnAssociateAssignmentwiseUser(DBTMTraineeAssignmentToUserViewModel DBTMTraineeAssignmentToUserViewModel)
        {
            SetNotificationMessage(_dBTMTraineeAssignmentAgent.AssociateUnAssociateAssignmentwiseUser(DBTMTraineeAssignmentToUserViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
            return RedirectToAction("GetDBTMTraineeAssignmentToUserList", new DataTableViewModel { SelectedParameter1 = DBTMTraineeAssignmentToUserViewModel.DBTMTraineeAssignmentId.ToString() });
        }
        #endregion
        #region Protected
        #endregion
    }
}


