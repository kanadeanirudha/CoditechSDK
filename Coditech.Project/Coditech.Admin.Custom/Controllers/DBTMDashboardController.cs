using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
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
        [HttpGet, HttpPost]
        public ActionResult LoadBatchesPartial(DataTableViewModel dataTableModel)
        {
            // If no model came from GET, create a default one
            if (dataTableModel == null)
            {
                dataTableModel = new DataTableViewModel();
            }

            GeneralBatchListViewModel list = GetBatchListData(dataTableModel);

            DBTMDashboardViewModel dBTMDashboardViewModel = TempData["DBTMModel"] != null
                ? JsonConvert.DeserializeObject<DBTMDashboardViewModel>(TempData["DBTMModel"].ToString())
                : new DBTMDashboardViewModel();

            TempData["DBTMModel"] = JsonConvert.SerializeObject(dBTMDashboardViewModel);

            dBTMDashboardViewModel.GeneralBatchList ??= new List<GeneralBatchListViewModel>();
            dBTMDashboardViewModel.GeneralBatchList.Add(list);

            TempData.Keep("DBTMModel");

            return PartialView("~/Views/DBTM/DBTMDashboard/_DBTMBatchListView.cshtml", list);
        }

        [HttpGet, HttpPost]
        public ActionResult LoadAssignmentPartial(DataTableViewModel dataTableModel)
        {
            if (dataTableModel == null)
            {
                dataTableModel = new DataTableViewModel();
            }

            DBTMTraineeAssignmentListViewModel assignmentList = GetAssignmentListData(dataTableModel);

            DBTMDashboardViewModel dBTMDashboardViewModel = TempData["DBTMModel"] != null
                ? JsonConvert.DeserializeObject<DBTMDashboardViewModel>(TempData["DBTMModel"].ToString())
                : new DBTMDashboardViewModel();

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
                list = _dBTMDashboardAgent.GetBatchList(dataTableModel);
            }
            list.SelectedCentreCode = dataTableModel.SelectedCentreCode;
            list.Custom5 = "Mobile View";
            return list;
        }

        [HttpGet]
        public virtual DBTMTraineeAssignmentListViewModel GetAssignmentListData(DataTableViewModel dataTableModel)
        {
            UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);

            if (string.IsNullOrEmpty(dataTableModel.SelectedParameter1))
            {
                dataTableModel.SelectedParameter1 = JsonConvert.DeserializeObject<DBTMCustomUserModel>(userModel.Custom3 ?? string.Empty)?.GeneralTrainerMasterId?.ToString() ?? "";
            }

            GetListOnlyIfSingleCentre(dataTableModel);

            DBTMTraineeAssignmentListViewModel assignmentList = new DBTMTraineeAssignmentListViewModel();

            if (!string.IsNullOrEmpty(dataTableModel.SelectedCentreCode) && !string.IsNullOrEmpty(dataTableModel.SelectedParameter1))
            {
                assignmentList = _dBTMTraineeAssignmentAgent.GetDBTMTraineeAssignmentList(dataTableModel);
            }

            // Additional assignments
            assignmentList.SelectedParameter1 = userModel.Custom1 == CustomConstants.DBTMTrainer? (JsonConvert.DeserializeObject<DBTMCustomUserModel>(userModel.Custom3 ?? string.Empty)?.GeneralTrainerMasterId?.ToString() ?? string.Empty) : dataTableModel.SelectedParameter1;   // keep client value if not trainer
            assignmentList.Custom5 = "Mobile View";
            assignmentList.SelectedCentreCode = dataTableModel.SelectedCentreCode;

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
                List<DBTMCalendarViewModel> data = this.LoadData();

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
        private List<DBTMCalendarViewModel> LoadData()
        {
            // Initialization.  
            List<DBTMCalendarViewModel> lst = new List<DBTMCalendarViewModel>();

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
                    DBTMCalendarViewModel infoObj = new DBTMCalendarViewModel();
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
        #region Trainer DashBoard
        [HttpGet]
        public virtual ActionResult GetTrainerDashBoard(short numberOfDaysRecord, long generalTrainerMasterId, int adminRoleMasterId, long userMasterId)
        {
            TempData["FormSizeClass"] = "col-lg-8";
            DBTMDashboardViewModel dBTMDashboardViewModel = _dBTMDashboardAgent.GetTrainerDashBoard(numberOfDaysRecord, generalTrainerMasterId, adminRoleMasterId, userMasterId);
            UserProfileViewModel userProfileViewModel = _dBTMDashboardAgent.GetUserProfile(userMasterId);
            dBTMDashboardViewModel.SelectedParameter1 = generalTrainerMasterId.ToString();
            dBTMDashboardViewModel.SelectedParameter2 = userMasterId.ToString();
            if (IsNotNull(userProfileViewModel))
            {
                dBTMDashboardViewModel.UserProfileModel = new List<UserProfileViewModel>();
            }
            dBTMDashboardViewModel.UserProfileModel.Add(userProfileViewModel);
            return View("~/Views/DBTM/DBTMDashboard/_dBTMTrainerDashboardPopUp.cshtml", dBTMDashboardViewModel);
        }
        #endregion

        [HttpGet]
        public ActionResult DBTMCalendar()
        {
            var model = new DBTMDashboardViewModel();

            model.CalendarEvent = new List<DBTMCalendarViewModel>
            {
                new DBTMCalendarViewModel
                {
                    CalendarId = 1,
                    Title = "New Event",
                    Desc = "Static test event",
                    Start_Date = "2025-08-05",
                    End_Date = "2025-08-08",
                    BackgroundColor = "#f39c12"
                },
                new DBTMCalendarViewModel
                {
                    CalendarId = 2,
                    Title = "Holiday",
                    Desc = "Static holiday",
                    Start_Date = "2025-08-15",
                    End_Date = "2025-08-16",
                    BackgroundColor = "#00a65a"
                },
                new DBTMCalendarViewModel
                {
                    CalendarId = 3,
                    Title = "Conference",
                    Desc = "Static conference",
                    Start_Date = "2025-08-27",
                    End_Date = "2025-08-28",
                    BackgroundColor = "#0073b7"
                },
                new DBTMCalendarViewModel
                {
                    CalendarId = 3,
                    Title = "My Birthday",
                    Desc = "Birthday",
                    Start_Date = "2025-09-05",
                    End_Date = "2025-09-06",
                    BackgroundColor = "#0073b7"
                }
            };
            return View("~/Views/DBTM/DBTMDashboard/DBTMCalendar.cshtml", model);
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
