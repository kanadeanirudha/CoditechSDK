using Coditech.Admin.Agents;
using Coditech.Admin.Helpers;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
namespace Coditech.Admin.Controllers
{
    public class DBTMTraineeAssignmentController : BaseController
    {
        private readonly IDBTMTraineeAssignmentAgent _dBTMTraineeAssignmentAgent;
        private const string createEdit = "~/Views/DBTM/DBTMTraineeAssignment/Create.cshtml";

        public DBTMTraineeAssignmentController(IDBTMTraineeAssignmentAgent dBTMTraineeAssignmentAgent)
        {
            _dBTMTraineeAssignmentAgent = dBTMTraineeAssignmentAgent;
        }

        public ActionResult List(DataTableViewModel dataTableModel)
        {
            DBTMTraineeAssignmentListViewModel list = new DBTMTraineeAssignmentListViewModel();
            GetListOnlyIfSingleCentre(dataTableModel);
            if (!string.IsNullOrEmpty(dataTableModel.SelectedCentreCode) && !string.IsNullOrEmpty(dataTableModel.SelectedParameter1))
            {
                list = _dBTMTraineeAssignmentAgent.GetDBTMTraineeAssignmentList(dataTableModel);
            }
            list.SelectedCentreCode = dataTableModel.SelectedCentreCode;
            list.SelectedParameter1 = dataTableModel.SelectedParameter1;
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMTraineeAssignment/_List.cshtml", list);
            }
            return View($"~/Views/DBTM/DBTMTraineeAssignment/List.cshtml", list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
            DBTMTraineeAssignmentViewModel dBTMTraineeAssignmentViewModel = new DBTMTraineeAssignmentViewModel
            {
                SelectedCentreCode = userModel.SelectedCentreCode,
                SelectedTrainee = new List<string>(),
                GeneralTrainerMasterId = userModel.Custom1 == CustomConstants.DBTMTrainer || userModel.Custom1 == CustomConstants.DBTMCentreOwner ? (JsonConvert.DeserializeObject<DBTMCustomUserModel>(userModel.Custom3 ?? string.Empty)?.GeneralTrainerMasterId ?? 0) : 0,
            };

            dBTMTraineeAssignmentViewModel.AllTraineeList = CoditechCustomDropdownHelper.GeneralDropdownList(new DropdownViewModel
            {
                DropdownType = DropdownCustomTypeEnum.TraineeDetailsListByDBTMTrainer.ToString(),
                Parameter = $"{dBTMTraineeAssignmentViewModel.SelectedCentreCode}~{dBTMTraineeAssignmentViewModel.GeneralTrainerMasterId}"
            }).DropdownList?.Where(x => x.Value != "")?.ToList();
            BindDBTMTest(dBTMTraineeAssignmentViewModel);

            return View(createEdit, dBTMTraineeAssignmentViewModel);
        }


        [HttpPost]
        public ActionResult Create(DBTMTraineeAssignmentViewModel dBTMTraineeAssignmentViewModel)
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

            dBTMTraineeAssignmentViewModel.AllTraineeList = CoditechCustomDropdownHelper.GeneralDropdownList(new DropdownViewModel
            {
                DropdownType = DropdownCustomTypeEnum.TraineeDetailsListByDBTMTrainer.ToString(),
                Parameter = $"{dBTMTraineeAssignmentViewModel.SelectedCentreCode}~{dBTMTraineeAssignmentViewModel.GeneralTrainerMasterId}"
            }).DropdownList?.Where(x => x.Value != "")?.ToList();
            BindDBTMTest(dBTMTraineeAssignmentViewModel);

            SetNotificationMessage(GetErrorNotificationMessage(dBTMTraineeAssignmentViewModel.ErrorMessage));
            return View(createEdit, dBTMTraineeAssignmentViewModel);
        }

        [HttpGet]
        public ActionResult GetDBTMTraineeAssignment(long dBTMTraineeAssignmentUserId)
        {
            DBTMTraineeAssignmentViewModel dBTMTraineeAssignmentViewModel = _dBTMTraineeAssignmentAgent.GetDBTMTraineeAssignment(dBTMTraineeAssignmentUserId);
            return View("~/Views/DBTM/DBTMTraineeAssignment/Edit.cshtml", dBTMTraineeAssignmentViewModel);
        }

        [HttpPost]
        public ActionResult GetDBTMTraineeAssignment(DBTMTraineeAssignmentViewModel dBTMTraineeAssignmentViewModel)
        {
            ModelState.Remove("DBTMTestStatusEnumId");
            ModelState.Remove("SelectedTrainee");
            ModelState.Remove("SelectedTest");
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_dBTMTraineeAssignmentAgent.UpdateDBTMTraineeAssignment(dBTMTraineeAssignmentViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                return RedirectToAction("GetDBTMTraineeAssignment", new { dBTMTraineeAssignmentUserId = dBTMTraineeAssignmentViewModel.DBTMTraineeAssignmentUserId });
            }
            return View("~/Views/DBTM/DBTMTraineeAssignment/Edit.cshtml", dBTMTraineeAssignmentViewModel);

        }
        public ActionResult Delete(string dBTMTraineeAssignmentUserId, string selectedCentreCode, string selectedParameter1)
        {
            string message = string.Empty;
            bool status = false;

            if (!string.IsNullOrEmpty(dBTMTraineeAssignmentUserId))
            {
                status = _dBTMTraineeAssignmentAgent.DeleteDBTMTraineeAssignment(dBTMTraineeAssignmentUserId, out message);

                SetNotificationMessage(!status
                    ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                    : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction("List", new DataTableViewModel { SelectedCentreCode = selectedCentreCode, SelectedParameter1 = selectedParameter1 });
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction("List", new DataTableViewModel { SelectedCentreCode = selectedCentreCode });
        }

        public ActionResult SendAssignmentReminder(long dBTMTraineeAssignmentId, long dBTMTraineeAssignmentUserId)
        {

            DBTMTraineeAssignmentViewModel model = new DBTMTraineeAssignmentViewModel();

            model = _dBTMTraineeAssignmentAgent.SendAssignmentReminder(dBTMTraineeAssignmentId, dBTMTraineeAssignmentUserId);

            if (!model.HasError)
            {
                SetNotificationMessage(GetSuccessNotificationMessage("Assignment reminder send successfully."));
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

        public ActionResult GetTraineeDetailByCentreCodeAndgeneralTrainerId(string centreCode, long generalTrainerId)
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

        public ActionResult GetAssignmentResult(DataTableViewModel dataTableModel)
        {
            DBTMActivitiesDetailsListViewModel list = _dBTMTraineeAssignmentAgent.GetAssignmentResult(Convert.ToInt64(dataTableModel.SelectedParameter1), dataTableModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMTraineeAssignment/DBTMTraineeAssignmentUser/_AssignmentResult.cshtml", list);
            }
            list.SelectedParameter1 = dataTableModel.SelectedParameter1;
            list.SelectedParameter2 = dataTableModel.SelectedParameter2;
            list.SelectedCentreCode = dataTableModel.SelectedCentreCode;

            return View($"~/Views/DBTM/DBTMTraineeAssignment/DBTMTraineeAssignmentUser/AssignmentResult.cshtml", list);
        }

        #region Assignmnet User
        public ActionResult GetDBTMTraineeAssignmentToUserList(DataTableViewModel dataTableViewModel)
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
        public ActionResult GetAssociateUnAssociateAssignmentwiseUser(DBTMTraineeAssignmentToUserViewModel DBTMTraineeAssignmentToUserViewModel)
        {
            return PartialView("~/Views/DBTM/DBTMTraineeAssignment/DBTMTraineeAssignmentUser/_AssociateUnAssociateAssignmentwiseUser.cshtml", DBTMTraineeAssignmentToUserViewModel);
        }

        [HttpPost]
        public ActionResult AssociateUnAssociateAssignmentwiseUser(DBTMTraineeAssignmentToUserViewModel DBTMTraineeAssignmentToUserViewModel)
        {
            SetNotificationMessage(_dBTMTraineeAssignmentAgent.AssociateUnAssociateAssignmentwiseUser(DBTMTraineeAssignmentToUserViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
            return RedirectToAction("GetDBTMTraineeAssignmentToUserList", new DataTableViewModel { SelectedParameter1 = DBTMTraineeAssignmentToUserViewModel.DBTMTraineeAssignmentId.ToString() });
        }
        #endregion
        #region Protected
        protected virtual void BindDBTMTest(DBTMTraineeAssignmentViewModel dBTMTraineeAssignmentViewModel)
        {
            dBTMTraineeAssignmentViewModel.DBTMTestList = dBTMTraineeAssignmentViewModel.DBTMTestList ?? new List<SelectListItem>();
            DBTMTestListViewModel dBTMTestList = _dBTMTraineeAssignmentAgent.GetDBTMTestList();

            if (dBTMTestList?.DBTMTestList != null)
            {
                foreach (var item in dBTMTestList.DBTMTestList)
                {
                    dBTMTraineeAssignmentViewModel.DBTMTestList.Add(new SelectListItem
                    {
                        Text = item.TestName,
                        Value = item.DBTMTestMasterId.ToString()
                    });
                }
            }
        }
        #endregion
    }
}


