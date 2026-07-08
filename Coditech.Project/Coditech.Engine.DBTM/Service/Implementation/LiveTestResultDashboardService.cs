using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;
using System.Data;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class LiveTestResultDashboardService : BaseService, ILiveTestResultDashboardService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMReportsService _dBTMReportsService;
        private readonly ICoditechRepository<UserMaster> _userMasterRepository;
        private readonly ICoditechRepository<EmployeeMaster> _employeeMasterRepository;

        public LiveTestResultDashboardService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider, IDBTMReportsService dBTMReportsService) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMReportsService = dBTMReportsService;
            _userMasterRepository = new CoditechRepository<UserMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _employeeMasterRepository = new CoditechRepository<EmployeeMaster>(_serviceProvider.GetService<Coditech_Entities>());
        }

        //Get Dashboard Details by selected Admin Role Master id.
        public virtual LiveTestResultDashboardModel GetLiveTestResultLogin(LiveTestResultLoginModel liveTestResultLoginModel)
        {
            if (IsNull(liveTestResultLoginModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            liveTestResultLoginModel.Password = MD5Hash(liveTestResultLoginModel.Password);
            UserMaster userMasterData = _userMasterRepository.Table.FirstOrDefault(x => x.UserName == liveTestResultLoginModel.UserName && x.Password == liveTestResultLoginModel.Password && (x.UserType == UserTypeEnum.Employee.ToString()));
            if (IsNull(userMasterData))
                throw new CoditechException(ErrorCodes.NotFound, null);
            else if (!userMasterData.IsActive)
                throw new CoditechException(ErrorCodes.ContactAdministrator, null);

            string selectedCentreCode = _employeeMasterRepository.Table.Where(x => x.EmployeeId == userMasterData.EntityId).FirstOrDefault().CentreCode;
            var fromDate = liveTestResultLoginModel.FromDate?.Date ?? DateTime.Today;
            var toDate = liveTestResultLoginModel.ToDate?.Date ?? DateTime.Today;
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<DBTMReportsModel> objStoredProc = new CoditechViewRepository<DBTMReportsModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@CentreCode", selectedCentreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@DBTMTestMasterId", liveTestResultLoginModel.DBTMTestMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@FromDate", fromDate, ParameterDirection.Input, DbType.Date);
            objStoredProc.SetParameter("@ToDate", toDate, ParameterDirection.Input, DbType.Date);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMReportsModel> dBTMReportsList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMLiveTestResult @CentreCode,@DBTMTestMasterId,@FromDate,@ToDate,@RowsCount OUT", 4, out pageListModel.TotalRowCount)?.ToList();
            LiveTestResultDashboardModel listModel = new LiveTestResultDashboardModel();
            if (dBTMReportsList?.Any() == true)
            {
                var activityGroups = dBTMReportsList.GroupBy(x => x.DBTMTestMasterId);
                foreach (var activity in activityGroups)
                {
                    DataTable dataTable = _dBTMReportsService.GetLiveResultDataTable(activity.Key, selectedCentreCode, activity.ToList(), fromDate, toDate);
                    listModel.DataTableList.Add(dataTable);
                }
            }
            return listModel;
        }
    }
}

