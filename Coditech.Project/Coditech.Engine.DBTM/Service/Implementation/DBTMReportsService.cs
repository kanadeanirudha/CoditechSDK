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

        public DBTMReportsListModel BatchWiseMultipleReports(int generalBatchMasterId, string dBTMTestMasterIds, DateTime FromDate, DateTime ToDate, bool isMobileRequest)
        {
            if (generalBatchMasterId <= 0)
            {
                return new DBTMReportsListModel();
            }
            DBTMReportsListModel dBTMReportsListModel = new DBTMReportsListModel();
            foreach (string testId in dBTMTestMasterIds.Split(',').ToList())
            {
                DBTMReportsListModel list = BatchWiseReports(generalBatchMasterId, Convert.ToInt32(testId), FromDate, ToDate, isMobileRequest);
                dBTMReportsListModel.DataTableList.Add(list.DataTable);
            }
            return dBTMReportsListModel;
        }

        public DBTMReportsListModel TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest)
        {
            return GetTestWiseReports(dBTMTestMasterId, dBTMTraineeDetailId, fromDate, toDate, entityId, userType, centreCode, isMobileRequest);
        }

        public GraphModel TestWiseGraphReports(int dBTMTestMasterId, long dBTMTraineeDetailId, int dBTMGraphMasterId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest)
        {
            GraphModel graphModel = new GraphModel();
            DBTMGraphMaster graphMaster = _dBTMGraphMasterRepository.Table.Where(x => x.DBTMGraphMasterId == dBTMGraphMasterId).FirstOrDefault();
            List<DBTMReportsModel> dBTMReportsList = GetTestWiseGraphReportFromDB(dBTMTestMasterId, dBTMTraineeDetailId, graphMaster.XParameter, graphMaster.YParameter, fromDate, toDate, ref entityId, userType, centreCode);
            if (dBTMReportsList?.Count > 0)
            {
                string dateTimeFormat = "yyyy-MM-dd";
                int rowCountPerDate = dBTMReportsList.Max(x => x.RowCountPerDate);
                DBTMTestMaster dbtmTestMaster = _dBTMTestMasterRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestMasterId).FirstOrDefault();
                string[] XValuesList = null;
                List<DateTime> testPerformedDateList = new List<DateTime>();
                foreach (DateTime item in dBTMReportsList.Select(x => x.TestPerformedTime.Date).Distinct())
                {
                    testPerformedDateList.Add(item.Date);
                }
                if (graphMaster.XParameter == "Date")
                {
                    List<string> xValues = new List<string>();
                    foreach (var item in testPerformedDateList)
                    {
                        xValues.Add(item.ToString(dateTimeFormat));
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
                        short distance = 25;
                        for (int i = distance; i <= (distance * 12); i = i + distance)
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
                    graphModel.IsRecordFound = true;
                    graphModel.GraphType = graphMaster.GraphType;
                    int colorIndex = 0;
                    var groupedReports = dBTMReportsList.Where(x => x.ParameterCode == graphMaster.YParameter).GroupBy(x => x.TestPerformedTime.Date);
                    string[] colorPalette = Enumerable.Range(0, groupedReports.Count()).Select(i => $"hsl({i * 360 / groupedReports.Count()}, 70%, 50%)").ToArray();
                    if (graphModel.GraphType == "LineChart")
                    {
                        graphModel.LineChartModel = new LineChartModel()
                        {
                            LineChartId = dBTMTestMasterId.ToString(),
                            XAxisLabel = graphMaster?.XParameter,
                            XValues = JsonConvert.SerializeObject(XValuesList),
                            YAxisLabel = $"{graphMaster?.YParameter} {DBTMCustomHelper.Unit(graphMaster?.YParameter)}",
                            Datasets = new List<LineGraphsDatasetModel>()
                        };

                        for (int i = 1; i <= rowCountPerDate; i++)
                        {
                            List<decimal> yValuesList = new List<decimal>();
                            foreach (DateTime date in testPerformedDateList)
                            {
                                yValuesList.Add(Convert.ToDecimal(dBTMReportsList.Where(x => x.ParameterCode == graphMaster.YParameter && x.TestPerformedTime.Date == date && x.RowOrder == i).Select(x => x.ParameterValue).FirstOrDefault()));
                            }

                            graphModel.LineChartModel.Datasets.Add(new LineGraphsDatasetModel()
                            {
                                Color = colorPalette[colorIndex % colorPalette.Length],
                                Label = $"Number Of Turns: {i}",
                                Data = JsonConvert.SerializeObject(yValuesList.ToArray()),
                            });
                            colorIndex++;
                        }
                    }
                    else if (graphModel.GraphType == "BarChart")
                    {
                        graphModel.BarChartModel = new BarChartModel()
                        {
                            BarChartId = dBTMTestMasterId.ToString(),
                            XAxisLabel = graphMaster?.XParameter,
                            XValues = JsonConvert.SerializeObject(XValuesList),
                            YAxisLabel = $"{graphMaster?.YParameter} {DBTMCustomHelper.Unit(graphMaster?.YParameter)}",
                            Datasets = new List<BarGraphsDatasetModel>()
                        };
                        // dataset will create based on rowCountPerDate count and dataset value will will bind based on testPerformedDateList
                        for (int i = 1; i <= rowCountPerDate; i++)
                        {
                            List<decimal> YValuesList = new List<decimal>();
                            foreach (DateTime date in testPerformedDateList)
                            {
                                YValuesList.Add(Convert.ToDecimal(dBTMReportsList.Where(x => x.ParameterCode == graphMaster.YParameter && x.TestPerformedTime.Date == date && x.RowOrder == i).Select(x => x.ParameterValue).FirstOrDefault()));
                            }

                            graphModel.BarChartModel.Datasets.Add(new BarGraphsDatasetModel()
                            {
                                Color = colorPalette[colorIndex % colorPalette.Length],
                                Label = $"Number Of Turns: {i}",
                                Data = JsonConvert.SerializeObject(YValuesList.ToArray()),
                            });
                            colorIndex++;
                        }
                    }
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

        private List<DBTMReportsModel> GetTestWiseGraphReportFromDB(int dBTMTestMasterId, long dBTMTraineeDetailId, string xParameter, string yParameter, DateTime fromDate, DateTime toDate, ref long entityId, string userType, string centreCode)
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
            objStoredProc.SetParameter("@XParameter", xParameter, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@YParameter", yParameter, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@FromDate", fromDate, ParameterDirection.Input, DbType.Date);
            objStoredProc.SetParameter("@ToDate", toDate, ParameterDirection.Input, DbType.Date);
            objStoredProc.SetParameter("@GeneralTrainerMasterId", entityId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@CentreCode", centreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            dBTMReportsList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMTestWiseGraphReportsList @DBTMTestMasterId,@DBTMTraineeDetailId,@XParameter,@YParameter,@FromDate,@ToDate,@GeneralTrainerMasterId,@CentreCode,@RowsCount OUT", 8, out pageListModel.TotalRowCount)?.ToList();
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
                    displayColumn.Add("Activity Time");
                    displayColumn.Add("Person Name");
                    //displayColumn.Add("Weight");
                    //displayColumn.Add("Height");
                }
                else
                {
                    displayColumn.Add("Activity Time");
                    displayColumn.Add("Person Name");
                    displayColumn.Add("Activity Status");
                    displayColumn.Add("Weight");
                    displayColumn.Add("Height");
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
                                    newRow["Activity Time"] = isMobileRequest && fromDate.Date == toDate.Date
                                        ? item.TestPerformedTime.ToString("hh:mm:ss tt")
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
