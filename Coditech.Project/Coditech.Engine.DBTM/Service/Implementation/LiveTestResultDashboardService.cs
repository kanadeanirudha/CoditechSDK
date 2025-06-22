using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Engine.DBTM.Helpers;
using Coditech.Resources;
using System.Data;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class LiveTestResultDashboardService : BaseService, ILiveTestResultDashboardService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMParametersAssociatedToTest> _dBTMParametersAssociatedToTestRepository;
        private readonly ICoditechRepository<DBTMTestParameter> _dBTMTestParameterRepository;
        private readonly ICoditechRepository<DBTMCalculationAssociatedToTest> _dBTMCalculationAssociatedToTestRepository;
        private readonly ICoditechRepository<DBTMTestCalculation> _dBTMTestCalculationRepository;
        private readonly ICoditechRepository<UserMaster> _userMasterRepository;
        private readonly ICoditechRepository<EmployeeMaster> _employeeMasterRepository;
        public LiveTestResultDashboardService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMParametersAssociatedToTestRepository = new CoditechRepository<DBTMParametersAssociatedToTest>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestParameterRepository = new CoditechRepository<DBTMTestParameter>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMCalculationAssociatedToTestRepository = new CoditechRepository<DBTMCalculationAssociatedToTest>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestCalculationRepository = new CoditechRepository<DBTMTestCalculation>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _userMasterRepository = new CoditechRepository<UserMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _employeeMasterRepository = new CoditechRepository<EmployeeMaster>(_serviceProvider.GetService<Coditech_Entities>());
        }

        //Get Dashboard Details by selected Admin Role Master id.
        public virtual LiveTestResultDashboardModel GetLiveTestResultLogin(LiveTestResultLoginModel liveTestResultLoginModel)
        {
            if (IsNull(liveTestResultLoginModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (!new CoditechRepository<DBTMDeviceMaster>(_serviceProvider.GetService<CoditechCustom_Entities>()).Table.Any(x => x.DeviceSerialCode == liveTestResultLoginModel.DeviceSerialCode && x.IsActive && x.IsMasterDevice))
                throw new CoditechException(ErrorCodes.InvalidData, "Invalide device serial code");

            liveTestResultLoginModel.Password = MD5Hash(liveTestResultLoginModel.Password);
            UserMaster userMasterData = _userMasterRepository.Table.FirstOrDefault(x => x.UserName == liveTestResultLoginModel.UserName && x.Password == liveTestResultLoginModel.Password
                                                                                        && (x.UserType == UserTypeEnum.Employee.ToString()));
            if (IsNull(userMasterData))
                throw new CoditechException(ErrorCodes.NotFound, null);
            else if (!userMasterData.IsActive)
                throw new CoditechException(ErrorCodes.ContactAdministrator, null);

            string selectedCentreCode = _employeeMasterRepository.Table.Where(x => x.EmployeeId == userMasterData.EntityId).FirstOrDefault().CentreCode;
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<DBTMReportsModel> objStoredProc = new CoditechViewRepository<DBTMReportsModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@CentreCode", selectedCentreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@DeviceSerialCode", liveTestResultLoginModel.DeviceSerialCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@DBTMTestMasterId", liveTestResultLoginModel.DBTMTestMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@FromDate", DateTime.Now.Date, ParameterDirection.Input, DbType.Date);
            objStoredProc.SetParameter("@ToDate", DateTime.Now.Date, ParameterDirection.Input, DbType.Date);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMReportsModel> dBTMReportsList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMLiveTestResult @CentreCode,@DeviceSerialCode,@DBTMTestMasterId,@FromDate,@ToDate,@RowsCount OUT", 5, out pageListModel.TotalRowCount)?.ToList();
            LiveTestResultDashboardModel listModel = new LiveTestResultDashboardModel();

            if (dBTMReportsList?.Count > 0)
            {
                List<int> dBTMTestMasterIds = dBTMReportsList.Select(g => g.DBTMTestMasterId).Distinct().ToList();
                foreach (int dBTMTestMasterId in dBTMTestMasterIds)
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Activity Name", typeof(String));
                    dataTable.Columns.Add("Person Name", typeof(String));
                    dataTable.Columns.Add("Activity Performed Time", typeof(String));
                    var testColumnList = (from a in _dBTMParametersAssociatedToTestRepository.Table
                                          join b in _dBTMTestParameterRepository.Table
                                          on a.DBTMTestParameterId equals b.DBTMTestParameterId
                                          where a.IsActive && a.DBTMTestMasterId == dBTMTestMasterId
                                          select new
                                          {
                                              b.ParameterName,
                                              b.ParameterCode
                                          })?.Distinct()?.ToList();

                    var calculationColumns = (from a in _dBTMCalculationAssociatedToTestRepository.Table
                                              join b in _dBTMTestCalculationRepository.Table
                                              on a.DBTMTestCalculationId equals b.DBTMTestCalculationId
                                              where a.DBTMTestMasterId == dBTMTestMasterId
                                              orderby b.OrderBy ascending
                                              select new { b.CalculationName, b.CalculationCode })?.Distinct()?.ToList();

                    DataRow newRow = null;
                    DateTime? dateTime = null;
                    foreach (var item in dBTMReportsList.Where(x => x.DBTMTestMasterId == dBTMTestMasterId))
                    {
                        if (dateTime != item.CreatedDate)
                        {
                            newRow = dataTable.NewRow();
                            newRow["Activity Name"] = item.TestName;
                            newRow["Person Name"] = item.PersonName;
                            newRow["Activity Performed Time"] = item.TestPerformedTime;
                        }

                        if (dateTime != item.CreatedDate && !string.IsNullOrEmpty(item.ParameterCode))
                        {
                            foreach (var item1 in calculationColumns)
                            {
                                if (!dataTable.Columns.Contains(item1.CalculationName))
                                {
                                    dataTable.Columns.Add(item1.CalculationName, typeof(String));
                                }
                                DBTMCustomHelper.Calculation(item1.CalculationCode, item1.CalculationName, newRow, dBTMReportsList.Where(x => x.DBTMTestMasterId == dBTMTestMasterId).ToList(), item.CreatedDate);
                            }
                        }
                        string parameterName = testColumnList.FirstOrDefault(x => x.ParameterCode == item.ParameterCode)?.ParameterName;
                        if (!string.IsNullOrEmpty(parameterName))
                        {
                            string columnName = string.IsNullOrEmpty(item.FromTo) ? parameterName : $"{item.FromTo}-{parameterName}";
                            if (!dataTable.Columns.Contains(columnName))
                            {
                                dataTable.Columns.Add(columnName, typeof(String));
                            }

                            newRow[columnName] = $"{item.ParameterValue} {DBTMCustomHelper.Unit(item.ParameterCode)}";
                        }
                        if (dateTime != item.CreatedDate)
                        {
                            dataTable.Rows.Add(newRow);
                        }
                        dateTime = item.CreatedDate;
                    }
                    listModel.DataTableList.Add(dataTable);
                }
            }
            return listModel;
        }
    }
}

