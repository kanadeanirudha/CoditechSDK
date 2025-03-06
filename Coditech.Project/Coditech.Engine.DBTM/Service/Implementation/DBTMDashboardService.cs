
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

        public DBTMDashboardService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _adminRoleMasterRepository = new CoditechRepository<AdminRoleMaster>(_serviceProvider.GetService<Coditech_Entities>());
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
                    DataSet dataset = GetDBTMCenterOwenerDashboardDetailsByUserId(numberOfDaysRecord,userMasterId);
                    dataset.Tables[0].TableName = "NumberOfTrainersDetails";
                    ConvertDataTableToList dataTable = new ConvertDataTableToList();
                    dBTMDashboardModel = dataTable.ConvertDataTable<DBTMDashboardModel>(dataset.Tables["NumberOfTrainersDetails"])?.FirstOrDefault();
                }
                else if (dashboardFormEnumCode.Equals(DashboardFormCustomEnum.DBTMTrainerDashboard.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    DataSet dataset = GetDBTMTrainerDashboardDetailsByUserId(numberOfDaysRecord, userMasterId);
                    dataset.Tables[0].TableName = "TraineeDetails";
                    ConvertDataTableToList dataTable = new ConvertDataTableToList();
                    dBTMDashboardModel = dataTable.ConvertDataTable<DBTMDashboardModel>(dataset.Tables["TraineeDetails"])?.FirstOrDefault();
                   
                    dataset.Tables[1].TableName = "TopActivityPerformed";
                    dBTMDashboardModel.TopActivityPerformed = new List<DBTMTestModel>();
                    dBTMDashboardModel.TopActivityPerformed = dataTable.ConvertDataTable<DBTMTestModel>(dataset.Tables["TopActivityPerformed"])?.ToList();
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

        protected virtual DataSet GetDBTMTrainerDashboardDetailsByUserId(short numberOfDaysRecord,long userId)
        {
            ExecuteSpHelper objStoredProc = new ExecuteSpHelper(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.GetParameter("@UserId", userId, ParameterDirection.Input, SqlDbType.BigInt);
            objStoredProc.GetParameter("@NumberOfDaysRecord", numberOfDaysRecord, ParameterDirection.Input, SqlDbType.SmallInt);
            return objStoredProc.GetSPResultInDataSet("Coditech_GetDBTMTrainerDashboard");
        }
    }
}
