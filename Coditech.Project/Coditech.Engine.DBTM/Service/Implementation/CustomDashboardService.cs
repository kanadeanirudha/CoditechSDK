
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
    public class CustomDashboardService : BaseService, ICustomDashboardService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<AdminRoleMaster> _adminRoleMasterRepository;

        public CustomDashboardService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _adminRoleMasterRepository = new CoditechRepository<AdminRoleMaster>(_serviceProvider.GetService<Coditech_Entities>());
        }

        //Get Dashboard Details by selected Admin Role Master id.
        public virtual CustomDashboardModel GetCustomDashboardDetails(int selectedAdminRoleMasterId, long userMasterId)
        {
            if (selectedAdminRoleMasterId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "SelectedAdminRoleMasterId"));

            if (userMasterId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "UserMasterId"));

            int? dashboardFormEnumId = _adminRoleMasterRepository.Table.Where(x => x.AdminRoleMasterId == selectedAdminRoleMasterId)?.Select(y => y.DashboardFormEnumId)?.FirstOrDefault();
            CustomDashboardModel dBTMDashboardModel = new CustomDashboardModel();
            //if (dashboardFormEnumId > 0)
            //{
            //    string dashboardFormEnumCode = GetEnumCodeByEnumId((int)dashboardFormEnumId);
            //    dBTMDashboardModel.DBTMDashboardFormEnumCode = dashboardFormEnumCode;
            //    if (dashboardFormEnumCode.Equals(DashboardFormCustomEnum.DBTMCentreDashboard.ToString(), StringComparison.InvariantCultureIgnoreCase))
            //    {
            //        DataSet dataset = GetDBTMCenterOwenerDashboardDetailsByUserId(userMasterId);
            //        dataset.Tables[0].TableName = "NumberOfTrainersDetails";
            //        ConvertDataTableToList dataTable = new ConvertDataTableToList();
            //        dBTMDashboardModel = dataTable.ConvertDataTable<CustomDashboardModel>(dataset.Tables["NumberOfTrainersDetails"])?.FirstOrDefault();
            //    }
            //    else if (dashboardFormEnumCode.Equals(DashboardFormCustomEnum.DBTMTrainerDashboard.ToString(), StringComparison.InvariantCultureIgnoreCase))
            //    {
            //        DataSet dataset = GetDBTMCenterOwenerDashboardDetailsByUserId(userMasterId);
            //        dataset.Tables[0].TableName = "TraineeDetails";
            //        ConvertDataTableToList dataTable = new ConvertDataTableToList();
            //        dBTMDashboardModel = dataTable.ConvertDataTable<CustomDashboardModel>(dataset.Tables["TraineeDetails"])?.FirstOrDefault();
            //    }
            //}
            return dBTMDashboardModel;
        }
        //protected virtual DataSet GetCustomCenterOwenerDashboardDetailsByUserId(long userId)
        //{
        //    ExecuteSpHelper objStoredProc = new ExecuteSpHelper(_serviceProvider.GetService<CoditechCustom_Entities>());
        //    objStoredProc.GetParameter("@UserId", userId, ParameterDirection.Input, SqlDbType.BigInt);
        //    objStoredProc.GetParameter("@NumberOfDaysRecord", 30, ParameterDirection.Input, SqlDbType.SmallInt);
        //    return objStoredProc.GetSPResultInDataSet("Coditech_GetDBTMCenterOwenerDashboard");
        //}
    }
}
