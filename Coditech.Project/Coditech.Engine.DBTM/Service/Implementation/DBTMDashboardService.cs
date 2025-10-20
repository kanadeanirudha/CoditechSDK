using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;
using System.Data;
namespace Coditech.API.Service
{
    public class DBTMDashboardService : BaseService, IDBTMDashboardService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<AdminRoleMaster> _adminRoleMasterRepository;
        private readonly ICoditechRepository<UserMaster> _userMasterRepository;
        private readonly ICoditechRepository<EmployeeMaster> _employeeMasterRepository;
        private readonly ICoditechRepository<GeneralTrainerMaster> _generalTrainerMasterRepository;

        public DBTMDashboardService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _adminRoleMasterRepository = new CoditechRepository<AdminRoleMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _userMasterRepository = new CoditechRepository<UserMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _employeeMasterRepository = new CoditechRepository<EmployeeMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _generalTrainerMasterRepository = new CoditechRepository<GeneralTrainerMaster>(_serviceProvider.GetService<Coditech_Entities>());
        }

        //Get Dashboard Details by selected Admin Role Master id.
        public virtual DBTMDashboardModel GetDBTMDashboardDetails(short numberOfDaysRecord, int selectedAdminRoleMasterId, long userMasterId)
        {
            if (selectedAdminRoleMasterId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "SelectedAdminRoleMasterId"));

            if (userMasterId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "UserMasterId"));

            int? dashboardFormEnumId = _adminRoleMasterRepository.Table.Where(x => x.AdminRoleMasterId == selectedAdminRoleMasterId)?.Select(y => y.DashboardFormEnumId)?.FirstOrDefault();
            DBTMDashboardModel dBTMDashboardModel = new DBTMDashboardModel();
            if (dashboardFormEnumId > 0)
            {
                string dashboardFormEnumCode = GetEnumCodeByEnumId((int)dashboardFormEnumId);
                dBTMDashboardModel.DBTMDashboardFormEnumCode = dashboardFormEnumCode;
                if (dashboardFormEnumCode.Equals(DashboardFormCustomEnum.DBTMCentreDashboard.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    DataSet dataset = GetDBTMCenterOwenerDashboardDetailsByUserId(numberOfDaysRecord, userMasterId);
                    dataset.Tables[0].TableName = "NumberOfTrainersDetails";
                    ConvertDataTableToList dataTable = new ConvertDataTableToList();
                    dBTMDashboardModel = dataTable.ConvertDataTable<DBTMDashboardModel>(dataset.Tables["NumberOfTrainersDetails"])?.FirstOrDefault();

                    dataset.Tables[1].TableName = "YearlyTraineeOverview";
                    dBTMDashboardModel.YearlyTraineeOverviewList = new List<DBTMYearlyTraineeOverviewModel>();
                    dBTMDashboardModel.YearlyTraineeOverviewList = dataTable.ConvertDataTable<DBTMYearlyTraineeOverviewModel>(dataset.Tables["YearlyTraineeOverview"])?.ToList();

                    dataset.Tables[2].TableName = "TrainerDetails";
                    dBTMDashboardModel.TrainersList = new List<DBTMTrainerDetailsModel>();
                    dBTMDashboardModel.TrainersList = dataTable.ConvertDataTable<DBTMTrainerDetailsModel>(dataset.Tables["TrainerDetails"])?.ToList();
                    foreach (var trainer in dBTMDashboardModel.TrainersList)
                    {
                        if (!string.IsNullOrWhiteSpace(trainer.PhotoMediaPath))
                        {
                            trainer.PhotoMediaPath = trainer.PhotoMediaPath;
                        }
                        else
                        {
                            trainer.PhotoMediaPath = GetImagePath(trainer.PhotoMediaId);
                        }
                    }
                }
                else if (dashboardFormEnumCode.Equals(DashboardFormCustomEnum.DBTMTrainerDashboard.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    DataSet dataset = GetDBTMTrainerDashboardDetailsByUserId(numberOfDaysRecord, userMasterId);
                    dataset.Tables[0].TableName = "TrainersDetails";
                    ConvertDataTableToList dataTable = new ConvertDataTableToList();
                    dBTMDashboardModel = dataTable.ConvertDataTable<DBTMDashboardModel>(dataset.Tables["TrainersDetails"])?.FirstOrDefault();

                    dataset.Tables[1].TableName = "YearlyTraineeOverview";
                    dBTMDashboardModel.YearlyTraineeOverviewList = new List<DBTMYearlyTraineeOverviewModel>();
                    dBTMDashboardModel.YearlyTraineeOverviewList = dataTable.ConvertDataTable<DBTMYearlyTraineeOverviewModel>(dataset.Tables["YearlyTraineeOverview"])?.ToList();                                       
                    UserMaster userMasterData = _userMasterRepository.Table.Where(x => x.UserMasterId == userMasterId)?.FirstOrDefault();
                    if (userMasterData == null)
                        return null;
                    long personId = 0;
                    if (userMasterData.UserType == UserTypeEnum.Employee.ToString())
                    {
                        var data = _employeeMasterRepository.Table.Where(x => x.EmployeeId == userMasterData.EntityId).Select(x => new { x.PersonId })?.FirstOrDefault();
                        if (data != null)
                        {
                            personId = data.PersonId;
                        }
                        var trainerData = _generalTrainerMasterRepository.Table.Where(x => x.EmployeeId == userMasterData.EntityId).Select(x => new { x.GeneralTrainerMasterId, x.TrainerSpecializationEnumId, x.CreatedDate }).FirstOrDefault();
                        if (trainerData == null)
                            return null;
                        DBTMTrainerDetailsModel trainerProfileModel = new DBTMTrainerDetailsModel();
                        if (trainerProfileModel == null)
                            return null;
                        // Get person details 
                        GeneralPersonModel generalPersonModel = GetGeneralPersonDetails(personId);
                        if (generalPersonModel != null)
                        {
                            trainerProfileModel.FirstName = generalPersonModel.FirstName;
                            trainerProfileModel.LastName = generalPersonModel.LastName;
                            trainerProfileModel.DateOfBirth = generalPersonModel.DateOfBirth;
                            trainerProfileModel.MobileNumber = generalPersonModel.MobileNumber;
                            trainerProfileModel.EmailId = generalPersonModel.EmailId;
                            trainerProfileModel.PhotoMediaPath = GetImagePath(generalPersonModel.PhotoMediaId);
                        }
                        trainerProfileModel.TrainerSpecialization = GetEnumDisplayTextByEnumId(Convert.ToInt32(trainerData.TrainerSpecializationEnumId));
                        trainerProfileModel.DateOfJoining = trainerData.CreatedDate.HasValue ? trainerData.CreatedDate.Value : default(DateTime);
                        dBTMDashboardModel.TrainersDetails = trainerProfileModel;
                        trainerProfileModel.DurationWithUs = CalculateDuration(trainerProfileModel.DateOfJoining, DateTime.Now);
                    }
                }
            }
            return dBTMDashboardModel;
        }
        protected virtual DataSet GetDBTMCenterOwenerDashboardDetailsByUserId(short numberOfDaysRecord, long userId)
        {
            ExecuteSpHelper objStoredProc = new ExecuteSpHelper(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.GetParameter("@UserId", userId, ParameterDirection.Input, SqlDbType.BigInt);
            objStoredProc.GetParameter("@NumberOfDaysRecord", numberOfDaysRecord, ParameterDirection.Input, SqlDbType.SmallInt);
            return objStoredProc.GetSPResultInDataSet("Coditech_GetDBTMCenterOwenerDashboard");
        }

        protected virtual DataSet GetDBTMTrainerDashboardDetailsByUserId(short numberOfDaysRecord, long userId)
        {
            ExecuteSpHelper objStoredProc = new ExecuteSpHelper(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.GetParameter("@UserId", userId, ParameterDirection.Input, SqlDbType.BigInt);
            return objStoredProc.GetSPResultInDataSet("Coditech_GetDBTMTrainerDashboard");
        }

        #region 
        private string CalculateDuration(DateTime fromDate, DateTime toDate)
        {
            int years = toDate.Year - fromDate.Year;
            int months = toDate.Month - fromDate.Month;
            int days = toDate.Day - fromDate.Day;
            if (days < 0)
            {
                months--;
                days += DateTime.DaysInMonth(toDate.Year, toDate.Month == 1 ? 12 : toDate.Month - 1);
            }
            if (months < 0)
            {
                years--;
                months += 12;
            }
            return $"{years} Y {months} M {days} D";
        }
        #endregion
    }
}
