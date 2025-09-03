using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Engine.DBTM.Helpers;
using Newtonsoft.Json;
using System.Data;
namespace Coditech.API.Service
{
    public class DBTMReportsService : BaseService, IDBTMReportsService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMBatchActivity> _dBTMBatchActivityRepository;
        private readonly ICoditechRepository<DBTMDeviceData> _dBTMDeviceDataRepository;
        private readonly ICoditechRepository<DBTMTestMaster> _dBTMTestMasterRepository;
        private readonly ICoditechRepository<DBTMParametersAssociatedToTest> _dBTMParametersAssociatedToTestRepository;
        private readonly ICoditechRepository<DBTMTestParameter> _dBTMTestParameterRepository;
        private readonly ICoditechRepository<DBTMCalculationAssociatedToTest> _dBTMCalculationAssociatedToTestRepository;
        private readonly ICoditechRepository<DBTMTestCalculation> _dBTMTestCalculationRepository;
        private readonly ICoditechRepository<DBTMGraphMaster> _dBTMGraphMasterRepository;
        public DBTMReportsService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMBatchActivityRepository = new CoditechRepository<DBTMBatchActivity>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMDeviceDataRepository = new CoditechRepository<DBTMDeviceData>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestMasterRepository = new CoditechRepository<DBTMTestMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMParametersAssociatedToTestRepository = new CoditechRepository<DBTMParametersAssociatedToTest>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestParameterRepository = new CoditechRepository<DBTMTestParameter>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMCalculationAssociatedToTestRepository = new CoditechRepository<DBTMCalculationAssociatedToTest>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestCalculationRepository = new CoditechRepository<DBTMTestCalculation>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMGraphMasterRepository = new CoditechRepository<DBTMGraphMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }
        public DBTMReportsListModel BatchWiseReports(int generalBatchMasterId, int dBTMTestMasterId, DateTime FromDate, DateTime ToDate, bool isMobileRequest)
        {
            if (dBTMTestMasterId <= 0)
            {
                return new DBTMReportsListModel();
            }
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<DBTMReportsModel> objStoredProc = new CoditechViewRepository<DBTMReportsModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@GeneralBatchMasterId", generalBatchMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@DBTMTestMasterId", dBTMTestMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@FromDate", FromDate, ParameterDirection.Input, DbType.Date);
            objStoredProc.SetParameter("@ToDate", ToDate, ParameterDirection.Input, DbType.Date);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMReportsModel> dBTMReportsList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMBatchWiseReportsList @GeneralBatchMasterId,@DBTMTestMasterId,@FromDate,@ToDate,@RowsCount OUT", 3, out pageListModel.TotalRowCount)?.ToList();

            return BindDBTMDataDetails(dBTMTestMasterId, isMobileRequest, dBTMReportsList, FromDate, ToDate);
        }

        public DBTMReportsListModel TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest)
        {
            return GetTestWiseReports(dBTMTestMasterId, dBTMTraineeDetailId, fromDate, toDate, entityId, userType, centreCode, isMobileRequest);
        }

        public GraphModel TestWiseGraphReports(int dBTMTestMasterId, long dBTMTraineeDetailId, int dBTMGraphMasterId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest)
        {
            GraphModel graphModel = new GraphModel();
            List<DBTMReportsModel> dBTMReportsList = GetTestWiseGraphReportFromDB(dBTMTestMasterId, dBTMTraineeDetailId, dBTMGraphMasterId, fromDate, toDate, ref entityId, userType, centreCode);
            if (dBTMReportsList?.Count > 0)
            {
                DBTMTestMaster dbtmTestMaster = _dBTMTestMasterRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestMasterId).FirstOrDefault();
                DBTMGraphMaster graphMaster = _dBTMGraphMasterRepository.Table.Where(x => x.DBTMGraphMasterId == dBTMGraphMasterId).FirstOrDefault();
                string[] XValuesList = null;
                if (graphMaster.XParameter == "Date")
                {
                    int daysDifference = (toDate - fromDate).Days;
                    List<string> xValues = new List<string>();
                    for (int i = 0; i <= daysDifference; i++)
                    {
                        xValues.Add(fromDate.AddDays(i).ToString("yyyy-MM-dd"));
                    }
                    XValuesList = xValues.ToArray();
                }
                else if (graphMaster.XParameter == "Distance")
                {
                    if (dbtmTestMaster.TestCode == "ProAgilityTest")
                        XValuesList = new string[] { "5", "15", "20" };
                    else if (dbtmTestMaster.TestCode == "FiveZeroFiveAgilityTest")
                        XValuesList = new string[] { "10", "15", "20" };
                    else if (dbtmTestMaster.TestCode == "3GateSprintTenTwenty")
                        XValuesList = new string[] { "10", "30" };
                    else if (dbtmTestMaster.TestCode == "5GateSprintTenTwentyThirtyFourty")
                        XValuesList = new string[] { "10", "20", "30", "40" };
                    else if (dbtmTestMaster.TestCode == "ThreeHundredYardTest")
                    {
                        List<string> xValues = new List<string>();

                        for (int i = 25; i <= (25 * 12); i = i + 5)
                        {
                            xValues.Add(i.ToString());
                        }
                        XValuesList = xValues.ToArray();
                    }
                }
                else if (graphMaster.XParameter == "Attempt")
                {

                }

                if (XValuesList != null)
                {
                    int count = dBTMReportsList.Where(x => x.ParameterCode == graphMaster.YParameter).Select(x => x.ParameterValue.ToString()).Count();

                    decimal[] YValuesList = new decimal[count];
                    int count1 = 0;
                    foreach (var item in dBTMReportsList.Where(x => x.ParameterCode == graphMaster.YParameter).Select(x => x.ParameterValue).ToList())
                    {
                        YValuesList[count1] = item;
                        count1++;
                    }
                    graphModel.IsRecordFound = true;
                    graphModel.GraphType = "LineChart";
                    string[] backgroundColor = ["rgba(42, 118, 244, 0.18)", "rgba(42, 118, 244, 0.30)"];

                    graphModel.LineChartModel = new LineChartModel();
                    graphModel.LineChartModel.XAxisLabel = graphMaster?.XParameter;
                    graphModel.LineChartModel.XValues = JsonConvert.SerializeObject(XValuesList);
                    graphModel.LineChartModel.YAxisLabel = graphMaster?.YParameter;
                    graphModel.LineChartModel.YValues = JsonConvert.SerializeObject(YValuesList);
                    graphModel.LineChartModel.BackgroundColor = JsonConvert.SerializeObject(backgroundColor);
                    graphModel.LineChartModel.LineChartId = dBTMTestMasterId.ToString();
                }
            }
            return graphModel;
        }

        private DBTMReportsListModel GetTestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest)
        {
            List<DBTMReportsModel> dBTMReportsList = GetTestWiseReportFromDB(dBTMTestMasterId, dBTMTraineeDetailId, fromDate, toDate, ref entityId, userType, centreCode);

            return BindDBTMDataDetails(dBTMTestMasterId, isMobileRequest, dBTMReportsList, fromDate, toDate);
        }

        private List<DBTMReportsModel> GetTestWiseReportFromDB(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, ref long entityId, string userType, string centreCode)
        {
            if (dBTMTestMasterId <= 0)
            {
                return new List<DBTMReportsModel>();
            }
            List<DBTMReportsModel> dBTMReportsList = new List<DBTMReportsModel>();
            if (userType == UserTypeEnum.Employee.ToString())
            {
                entityId = 0;
            }
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<DBTMReportsModel> objStoredProc = new CoditechViewRepository<DBTMReportsModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@DBTMTestMasterId", dBTMTestMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@DBTMTraineeDetailId", dBTMTraineeDetailId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@FromDate", fromDate, ParameterDirection.Input, DbType.Date);
            objStoredProc.SetParameter("@ToDate", toDate, ParameterDirection.Input, DbType.Date);
            objStoredProc.SetParameter("@GeneralTrainerMasterId", entityId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@CentreCode", centreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            dBTMReportsList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMTestWiseReportsList @DBTMTestMasterId,@DBTMTraineeDetailId,@FromDate,@ToDate,@GeneralTrainerMasterId,@CentreCode,@RowsCount OUT", 6, out pageListModel.TotalRowCount)?.ToList();
            return dBTMReportsList;
        }

        private List<DBTMReportsModel> GetTestWiseGraphReportFromDB(int dBTMTestMasterId, long dBTMTraineeDetailId, int dBTMGraphMasterId, DateTime fromDate, DateTime toDate, ref long entityId, string userType, string centreCode)
        {
            if (dBTMTestMasterId <= 0)
            {
                return new List<DBTMReportsModel>();
            }
            List<DBTMReportsModel> dBTMReportsList = new List<DBTMReportsModel>();
            if (userType == UserTypeEnum.Employee.ToString())
            {
                entityId = 0;
            }
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<DBTMReportsModel> objStoredProc = new CoditechViewRepository<DBTMReportsModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@DBTMTestMasterId", dBTMTestMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@DBTMTraineeDetailId", dBTMTraineeDetailId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@DBTMGraphMasterId", dBTMGraphMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@FromDate", fromDate, ParameterDirection.Input, DbType.Date);
            objStoredProc.SetParameter("@ToDate", toDate, ParameterDirection.Input, DbType.Date);
            objStoredProc.SetParameter("@GeneralTrainerMasterId", entityId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@CentreCode", centreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            dBTMReportsList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMTestWiseGraphReportsList @DBTMTestMasterId,@DBTMTraineeDetailId,@DBTMGraphMasterId,@FromDate,@ToDate,@GeneralTrainerMasterId,@CentreCode,@RowsCount OUT", 7, out pageListModel.TotalRowCount)?.ToList();
            return dBTMReportsList;
        }

        private DBTMReportsListModel BindDBTMDataDetails(int dBTMTestMasterId, bool isMobileRequest, List<DBTMReportsModel> dBTMReportsList, DateTime fromDate, DateTime toDate)
        {
            DBTMReportsListModel listModel = new DBTMReportsListModel();

            if (dBTMReportsList?.Count > 0)
            {
                List<string> displayColumn = new List<string>();
                if (isMobileRequest)
                {
                    displayColumn.Add("Person Name");
                    //displayColumn.Add("Weight");
                    //displayColumn.Add("Height");
                    displayColumn.Add("Activity Time");
                }
                else
                {
                    displayColumn.Add("Person Name");
                    displayColumn.Add("Activity Status");
                    displayColumn.Add("Weight");
                    displayColumn.Add("Height");
                    displayColumn.Add("Activity Time");
                }
                foreach (string item in displayColumn)
                    listModel.DataTable.Columns.Add(item, typeof(String));

                var testColumnList = (from a in _dBTMParametersAssociatedToTestRepository.Table
                                      join b in _dBTMTestParameterRepository.Table
                                      on a.DBTMTestParameterId equals b.DBTMTestParameterId
                                      where a.DBTMTestMasterId == dBTMTestMasterId && a.IsActive
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
                foreach (var item in dBTMReportsList)
                {
                    if (dateTime != item.CreatedDate)
                    {
                        newRow = listModel.DataTable.NewRow();
                        foreach (string displayColumnName in displayColumn)
                        {
                            switch (displayColumnName)
                            {
                                case "Activity Name":
                                    newRow["Activity Name"] = item.TestName;
                                    break;
                                case "Person Name":
                                    newRow["Person Name"] = $"{item.FirstName} {item.LastName}";
                                    break;
                                case "Activity Status":
                                    newRow["Activity Status"] = item.ActivityStatus;//$"<span class=\"badge badge-soft-info\">{item.ActivityStatus}</span>";
                                    break;
                                case "Weight":
                                    newRow["Weight"] = $"{item.Weight} {DBTMCustomHelper.Unit("Weight")}";
                                    break;
                                case "Height":
                                    newRow["Height"] = $"{item.Height} {DBTMCustomHelper.Unit("Height")}";
                                    break;
                                case "Activity Time":
                                    newRow["Activity Time"] = isMobileRequest && fromDate.Date == toDate.Date/* && fromDate.Date.Date == DateTime.Now.Date*/
                                        ? item.TestPerformedTime.ToString("hh:mm tt")
                                        : item.TestPerformedTime;
                                    break;
                            }
                        }
                    }

                    if (dateTime != item.CreatedDate && !string.IsNullOrEmpty(item.ParameterCode))
                    {
                        foreach (var item1 in calculationColumns)
                        {
                            if (!listModel.DataTable.Columns.Contains(item1.CalculationName))
                            {
                                listModel.DataTable.Columns.Add(item1.CalculationName, typeof(String));
                            }
                            DBTMCustomHelper.Calculation(item1.CalculationCode, item1.CalculationName, newRow, dBTMReportsList, item.CreatedDate);
                        }
                    }
                    string parameterName = testColumnList.FirstOrDefault(x => x.ParameterCode == item.ParameterCode)?.ParameterName;
                    if (!string.IsNullOrEmpty(parameterName))
                    {
                        string columnName = string.IsNullOrEmpty(item.FromTo) ? parameterName : $"{item.FromTo}-{parameterName}";
                        if (!listModel.DataTable.Columns.Contains(columnName))
                        {
                            listModel.DataTable.Columns.Add(columnName, typeof(String));
                        }

                        newRow[columnName] = $"{item.ParameterValue} {DBTMCustomHelper.Unit(item.ParameterCode)}";
                    }
                    if (dateTime != item.CreatedDate)
                    {
                        listModel.DataTable.Rows.Add(newRow);
                    }
                    dateTime = item.CreatedDate;
                }
            }
            return listModel;
        }

    }
}
