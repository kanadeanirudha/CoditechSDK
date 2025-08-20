using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.Admin.Controllers
{
    public class DBTMDashboardController : BaseController
    {
        private readonly IDashboardAgent _dashboardAgent;
        private readonly IGeneralBatchAgent _generalBatchAgent;
        private readonly IUserAgent _userAgent;
        private readonly IDBTMDashboardAgent _dBTMDashboardAgent;
        private readonly IDBTMTraineeAssignmentAgent _dBTMTraineeAssignmentAgent;

        public DBTMDashboardController(IDashboardAgent dashboardAgent, IDBTMDashboardAgent dBTMDashboardAgent, IDBTMTraineeAssignmentAgent dBTMTraineeAssignmentAgent, IUserAgent userAgent, IGeneralBatchAgent generalBatchAgent)
        {
            _dashboardAgent = dashboardAgent;
            _userAgent = userAgent;
            _dBTMDashboardAgent = dBTMDashboardAgent;
            _dBTMTraineeAssignmentAgent = dBTMTraineeAssignmentAgent;
            _generalBatchAgent = generalBatchAgent;
        }

        [HttpGet]
        public IActionResult Index(short numberOfDaysRecord)
        {
            DashboardViewModel dashboardViewModel = _dashboardAgent.GetDashboardDetails();
            if (IsNotNull(dashboardViewModel) && !string.IsNullOrEmpty(dashboardViewModel.DashboardFormEnumCode))
            {
                if (dashboardViewModel.DashboardFormEnumCode.Equals(DashboardFormCustomEnum.DBTMCentreDashboard.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    DBTMDashboardViewModel dBTMDashboardViewModel = _dBTMDashboardAgent.GetDBTMDashboardDetails(numberOfDaysRecord);
                    return View("~/Views/DBTM/DBTMDashboard/DBTMCentreDashboard.cshtml", dBTMDashboardViewModel);
                }
                else if (dashboardViewModel.DashboardFormEnumCode.Equals(DashboardFormCustomEnum.DBTMTrainerDashboard.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    DataTableViewModel dataTableModel = new DataTableViewModel();
                    DBTMDashboardViewModel dBTMDashboardViewModel = _dBTMDashboardAgent.GetDBTMDashboardDetails(numberOfDaysRecord);
                    UserProfileViewModel userProfileViewModel = _userAgent.GetUserProfile();
                    DBTMTraineeAssignmentListViewModel assignmentList = GetAssignmentListData(dataTableModel);
                    GeneralBatchListViewModel list = GetBatchListData(dataTableModel);
                    dBTMDashboardViewModel.DBTMTraineeAssignmentList ??= new List<DBTMTraineeAssignmentListViewModel>();
                    dBTMDashboardViewModel.DBTMTraineeAssignmentList.Add(assignmentList);
                    dBTMDashboardViewModel.GeneralBatchList ??= new List<GeneralBatchListViewModel>();
                    dBTMDashboardViewModel.GeneralBatchList.Add(list);
                    dBTMDashboardViewModel.DBTMTraineeAssignmentList ??= new List<DBTMTraineeAssignmentListViewModel>();
                    dBTMDashboardViewModel.DBTMTraineeAssignmentList.Add(assignmentList);
                    if (IsNotNull(userProfileViewModel))
                    {
                        dBTMDashboardViewModel.UserProfileModel = new List<UserProfileViewModel>();
                    }
                    dBTMDashboardViewModel.UserProfileModel.Add(userProfileViewModel);
                    return View("~/Views/DBTM/DBTMDashboard/DBTMTrainerDashboard.cshtml", dBTMDashboardViewModel);
                }
            }
            return View("~/Views/Dashboard/GeneralDashboard.cshtml");
        }
        [HttpPost]
        public ActionResult LoadBatchesPartial(DataTableViewModel dataTableModel)
        {

            GeneralBatchListViewModel list = GetBatchListData(dataTableModel);
            DBTMDashboardViewModel dBTMDashboardViewModel = TempData["DBTMModel"] != null ? JsonConvert.DeserializeObject<DBTMDashboardViewModel>(TempData["DBTMModel"].ToString()) : new DBTMDashboardViewModel();
            TempData.Keep();
            TempData["DBTMModel"] = JsonConvert.SerializeObject(dBTMDashboardViewModel);

            dBTMDashboardViewModel.GeneralBatchList ??= new List<GeneralBatchListViewModel>();
            dBTMDashboardViewModel.GeneralBatchList.Add(list);

            TempData.Keep("DBTMModel");
            return PartialView("~/Views/DBTM/DBTMDashboard/_DBTMBatchListView.cshtml", list);
        }
        [HttpPost]
        public ActionResult LoadAssignmentPartial(DataTableViewModel dataTableModel)
        {
            DBTMTraineeAssignmentListViewModel assignmentList = GetAssignmentListData(dataTableModel);
            DBTMDashboardViewModel dBTMDashboardViewModel = TempData["DBTMModel"] != null ? JsonConvert.DeserializeObject<DBTMDashboardViewModel>(TempData["DBTMModel"].ToString()) : new DBTMDashboardViewModel();
            TempData.Keep();
            TempData["DBTMModel"] = JsonConvert.SerializeObject(dBTMDashboardViewModel);
            dBTMDashboardViewModel.DBTMTraineeAssignmentList ??= new List<DBTMTraineeAssignmentListViewModel>();
            dBTMDashboardViewModel.DBTMTraineeAssignmentList.Add(assignmentList);
            TempData.Keep("DBTMModel");
            return PartialView("~/Views/DBTM/DBTMDashboard/_DBTMAssignmentListView.cshtml", assignmentList);
        }
        [HttpGet]
        private GeneralBatchListViewModel GetBatchListData(DataTableViewModel dataTableModel)
        {
            GeneralBatchListViewModel list = new GeneralBatchListViewModel();
            GetListOnlyIfSingleCentre(dataTableModel);
            if (!string.IsNullOrEmpty(dataTableModel.SelectedCentreCode))
            {
                list = _generalBatchAgent.GetBatchList(dataTableModel);
            }
            list.SelectedCentreCode = dataTableModel.SelectedCentreCode;
            list.Custom5 = "Mobile View";
            return list;
        }

        public virtual DBTMTraineeAssignmentListViewModel GetAssignmentListData(DataTableViewModel dataTableModel)
        {
            UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
            GetListOnlyIfSingleCentre(dataTableModel);
            dataTableModel.SelectedParameter1 = JsonConvert.DeserializeObject<DBTMCustomUserModel>(userModel.Custom3 ?? string.Empty)?.GeneralTrainerMasterId?.ToString() ?? "";
            DBTMTraineeAssignmentListViewModel assignmentList = new DBTMTraineeAssignmentListViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SelectedCentreCode) && !string.IsNullOrEmpty(dataTableModel.SelectedParameter1))
            {
                assignmentList = _dBTMTraineeAssignmentAgent.GetDBTMTraineeAssignmentList(dataTableModel);
            }
            assignmentList.SelectedParameter1 = userModel.Custom1 == CustomConstants.DBTMTrainer ? (JsonConvert.DeserializeObject<DBTMCustomUserModel>(userModel.Custom3 ?? string.Empty)?.GeneralTrainerMasterId?.ToString() ?? string.Empty) : string.Empty;
            assignmentList.Custom5 = "Mobile View";
            assignmentList.SelectedCentreCode = dataTableModel.SelectedCentreCode;
            assignmentList.SelectedParameter1 = dataTableModel.SelectedParameter1;
            return assignmentList;
        }
        public ActionResult Index()
        {
            // Info.  
            return this.View();
        }

        /// <summary>  
        /// GET: /Home/GetCalendarData  
        /// </summary>  
        /// <returns>Return data</returns>  

        [HttpGet]
        public ActionResult GetCalendarData()
        {
            // Initialization.  
            JsonResult result = new JsonResult(null);

            try
            {
                // Loading.  
                List<CalendarViewModel> data = this.LoadData();

                // Processing.  
                result = this.Json(data, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            // Return info.  
            return result;
        }

        /// <summary>  
        /// Load data method.  
        /// </summary>  
        /// <returns>Returns - Data</returns>  
        private List<CalendarViewModel> LoadData()
        {
            // Initialization.  
            List<CalendarViewModel> lst = new List<CalendarViewModel>();

            try
            {
                // Initialization.  
                string line = string.Empty;
                //  string srcFilePath = "";
                var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                //var fullPath = Path.Combine(rootPath, srcFilePath);
                var fullPath = Path.Combine(rootPath);
                string filePath = new Uri(fullPath).LocalPath;
                StreamReader sr = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read));

                // Read file.  
                while ((line = sr.ReadLine()) != null)
                {
                    // Initialization.  
                    CalendarViewModel infoObj = new CalendarViewModel();
                    string[] info = line.Split(',');

                    // Setting.  
                    infoObj.CalendarId = Convert.ToInt32(info[0].ToString());
                    infoObj.Title = info[1].ToString();
                    infoObj.Desc = info[2].ToString();
                    infoObj.Start_Date = info[3].ToString();
                    infoObj.End_Date = info[4].ToString();

                    // Adding.  
                    lst.Add(infoObj);
                }

                // Closing.  
                sr.Dispose();
                sr.Close();
            }
            catch (Exception ex)
            {
                // info.  
                Console.Write(ex);
            }

            // info.  
            return lst;
        }

        #region Send Reminder
        [HttpPost]
        public virtual ActionResult SendAssignmentReminder(long dBTMTraineeAssignmentId, long dBTMTraineeAssignmentUserId)
        {

            DBTMTraineeAssignmentViewModel model = new DBTMTraineeAssignmentViewModel()
            {
                DBTMTraineeAssignmentId = dBTMTraineeAssignmentId,
                DBTMTraineeAssignmentUserId = dBTMTraineeAssignmentUserId
            };

            model = _dBTMTraineeAssignmentAgent.SendAssignmentReminder(dBTMTraineeAssignmentId, dBTMTraineeAssignmentUserId);

            if (!model.HasError)
            {
                SetNotificationMessage(GetSuccessNotificationMessage("Assignment Reminder Send Successfully."));
                return Json(new { success = true });
            }
            else
            {
                SetNotificationMessage(GetErrorNotificationMessage("Failed to Send Reminder."));
                return Json(new { success = false });
            }
        }
        #endregion
    }
}
