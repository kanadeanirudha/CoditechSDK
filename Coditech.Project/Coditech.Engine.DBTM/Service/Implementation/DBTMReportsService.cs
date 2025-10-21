using ClosedXML.Excel;
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
        private readonly ICoditechRepository<GeneralBatchMaster> _generalBatchMasterRepository;
        private readonly ICoditechRepository<DBTMTestParameterListviewSequence> _dBTMTestParameterListviewSequenceRepository;
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
            _generalBatchMasterRepository = new CoditechRepository<GeneralBatchMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _dBTMTestParameterListviewSequenceRepository = new CoditechRepository<DBTMTestParameterListviewSequence>(_serviceProvider.GetService<CoditechCustom_Entities>());
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
            DBTMReportsListModel dBTMReportsListModel = new DBTMReportsListModel();
            dBTMReportsListModel.DataTable = BindDBTMDataDetails(dBTMTestMasterId, isMobileRequest, dBTMReportsList, FromDate, ToDate);
            return dBTMReportsListModel;
        }

        public DBTMReportsListModel BatchWiseMultipleReports(string dBTMTestMasterIds, int generalBatchMasterId, DateTime FromDate, DateTime ToDate, bool isMobileRequest)
        {
            if (generalBatchMasterId <= 0)
            {
                return new DBTMReportsListModel();
            }
            DBTMReportsListModel dBTMReportsListModel = new DBTMReportsListModel();
            List<string> dBTMTestMasterIdList = dBTMTestMasterIds.Split(",").ToList();
            var testList = _dBTMTestMasterRepository.Table.Where(x => dBTMTestMasterIdList.Contains(x.DBTMTestMasterId.ToString()) && x.IsActive).Select(x => new { x.DBTMTestMasterId, x.TestName });
            if (!string.IsNullOrWhiteSpace(dBTMTestMasterIds))
            {
                if (dBTMReportsListModel.DataTableList == null)
                    dBTMReportsListModel.DataTableList = new List<KeyValuePair<string, DataTable>>();

                foreach (string testId in dBTMTestMasterIds.Split(',').ToList())
                {
                    if (!string.IsNullOrWhiteSpace(testId))
                    {
                        DBTMReportsListModel list = BatchWiseReports(generalBatchMasterId, Convert.ToInt32(testId), FromDate, ToDate, isMobileRequest);
                        if (list?.DataTable?.Rows?.Count > 0)
                        {
                            var test = testList.FirstOrDefault(x => x.DBTMTestMasterId == Convert.ToInt32(testId));
                            dBTMReportsListModel.DataTableList.Add(new KeyValuePair<string, DataTable>(test.TestName, list.DataTable));
                        }
                    }
                }
            }
            return dBTMReportsListModel;
        }

        public DBTMReportsListModel BatchWiseMultipleReportsFile(string dBTMTestMasterIds, int generalBatchMasterId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest, string reportType)
        {
            if (generalBatchMasterId <= 0)
            {
                return new DBTMReportsListModel();
            }
            DBTMReportsListModel dBTMReportsListModel = new DBTMReportsListModel();
            List<string> dBTMTestMasterIdList = dBTMTestMasterIds.Split(",").ToList();
            var testList = _dBTMTestMasterRepository.Table.Where(x => dBTMTestMasterIdList.Contains(x.DBTMTestMasterId.ToString()) && x.IsActive).Select(x => new { x.DBTMTestMasterId, x.TestName }).ToList();
            if (!string.IsNullOrWhiteSpace(dBTMTestMasterIds))
            {
                if (dBTMReportsListModel.DataTableList == null)
                    dBTMReportsListModel.DataTableList = new List<KeyValuePair<string, DataTable>>();

                foreach (string testId in dBTMTestMasterIds.Split(',').ToList())
                {
                    if (!string.IsNullOrWhiteSpace(testId))
                    {
                        DBTMReportsListModel list = BatchWiseReports(
                            generalBatchMasterId,
                            Convert.ToInt32(testId),
                            fromDate,
                            toDate,
                            isMobileRequest);

                        if (list?.DataTable?.Rows?.Count > 0)
                        {
                            var test = testList.FirstOrDefault(x => x.DBTMTestMasterId == Convert.ToInt32(testId));
                            dBTMReportsListModel.DataTableList.Add(
                                new KeyValuePair<string, DataTable>(test.TestName, list.DataTable));
                        }
                    }
                }
            }

            if (dBTMReportsListModel?.DataTableList == null || dBTMReportsListModel.DataTableList.Count == 0)
            {
                return dBTMReportsListModel;
            }

            GeneralBatchMaster batch = _generalBatchMasterRepository.Table.FirstOrDefault(b => b.GeneralBatchMasterId == generalBatchMasterId);
            string batchName = batch != null ? batch.BatchName : "";
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                batchName = batchName.Replace(c.ToString(), "");
            }
            string currentDir = Directory.GetCurrentDirectory();
            string dataFolder = Path.Combine(currentDir, "data", "BatchReport");
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            string fileName = $"Batch_Report_{batchName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            string filePath = Path.Combine(dataFolder, fileName);

            // Create Excel workbook
            using (var workbook = new XLWorkbook())
            {
                foreach (var table in dBTMReportsListModel.DataTableList)
                {
                    var replacements = new Dictionary<string, string>
                    {
                        { "300", "Threehundres" },
                        { "5-0-5 ", "FiveZeroFiveAgilityTest" },
                        { "5-10-5", "ProAgilityTest" },
                        { "3", "ThreeGateSprint" }
                    };
                    string sheetName = table.Key;
                    foreach (var kv in replacements)
                    {
                        sheetName = sheetName.Replace(kv.Key, kv.Value);
                    }
                    char[] invalidChars = new char[] { ':', '\\', '/', '?', '*', '[', ']' };
                    foreach (var c in invalidChars)
                    {
                        sheetName = sheetName.Replace(c.ToString(), "");
                    }
                    sheetName = sheetName.Trim();
                    if (sheetName.Length > 31)
                        sheetName = sheetName.Substring(0, 31);
                    var worksheet = workbook.Worksheets.Add(sheetName);
                    worksheet.Cell(1, 1).InsertTable(table.Value, sheetName, true);
                }
                workbook.SaveAs(filePath);
            }
            dBTMReportsListModel.FilePath = filePath;
            dBTMReportsListModel.FileName = fileName;
            return dBTMReportsListModel;
        }

        public DBTMReportsListModel TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest)
        {
            return GetTestWiseReports(dBTMTestMasterId, dBTMTraineeDetailId, fromDate, toDate, entityId, userType, centreCode, isMobileRequest);
        }

        public DBTMReportsListModel TestWiseMultipleReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest)
        {
            DBTMReportsListModel dBTMReportsListModel = new DBTMReportsListModel();
            List<string> dBTMTestMasterIdList = dBTMTestMasterIds.Split(",").ToList();
            var testList = _dBTMTestMasterRepository.Table.Where(x => dBTMTestMasterIdList.Contains(x.DBTMTestMasterId.ToString()) && x.IsActive).Select(x => new { x.DBTMTestMasterId, x.TestName });
            if (!string.IsNullOrWhiteSpace(dBTMTestMasterIds))
            {
                if (dBTMReportsListModel.DataTableList == null)
                    dBTMReportsListModel.DataTableList = new List<KeyValuePair<string, DataTable>>();

                foreach (string testId in dBTMTestMasterIds.Split(',').ToList())
                {
                    if (!string.IsNullOrWhiteSpace(testId))
                    {
                        DBTMReportsListModel list = GetTestWiseReports(Convert.ToInt32(testId), dBTMTraineeDetailId, fromDate, toDate, entityId, userType, centreCode, isMobileRequest);
                        if (list?.DataTable?.Rows?.Count > 0)
                        {
                            var test = testList.FirstOrDefault(x => x.DBTMTestMasterId == Convert.ToInt32(testId));
                            dBTMReportsListModel.DataTableList.Add(new KeyValuePair<string, DataTable>(test.TestName, list.DataTable));
                        }
                    }
                }
            }
            return dBTMReportsListModel;
        }

        public DBTMReportsListModel TestWiseMultipleReportsFile(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest, string reportType)
        {
            var reportData = TestWiseMultipleReports(dBTMTestMasterIds, dBTMTraineeDetailId, fromDate, toDate, entityId, userType, centreCode, isMobileRequest);
            if (reportData?.DataTableList == null || reportData.DataTableList.Count == 0)
                return reportData;
            string currentDir = Directory.GetCurrentDirectory();
            string dataFolder = Path.Combine(currentDir, "data", "ActivityReport");
            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);
            string fileName = $"Activity_Report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            string filePath = Path.Combine(dataFolder, fileName);
            // Create workbook using ClosedXML
            using (var workbook = new XLWorkbook())
            {
                foreach (var table in reportData.DataTableList)
                {
                    var replacements = new Dictionary<string, string>
                    {
                        { "300", "Threehundres" },
                        { "5-0-5 ", "FiveZeroFiveAgilityTest" },
                        { "5-10-5", "ProAgilityTest" },
                        { "3", "ThreeGateSprint" }
                    };
                    string sheetName = table.Key;
                    foreach (var kv in replacements)
                    {
                        sheetName = sheetName.Replace(kv.Key, kv.Value);
                    }
                    char[] invalidChars = new char[] { ':', '\\', '/', '?', '*', '[', ']' };
                    foreach (var c in invalidChars)
                    {
                        sheetName = sheetName.Replace(c.ToString(), "");
                    }
                    sheetName = sheetName.Trim();
                    if (sheetName.Length > 31)
                        sheetName = sheetName.Substring(0, 31);
                    var worksheet = workbook.Worksheets.Add(sheetName);
                    worksheet.Cell(1, 1).InsertTable(table.Value, sheetName, true);
                }
                // Save the workbook
                workbook.SaveAs(filePath);
            }
            reportData.FilePath = filePath;
            reportData.FileName = fileName;
            return reportData;
        }

        // Delete Report File from Data folder
        public virtual bool DeleteReportsFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;

            try
            {
                string currentDir = Directory.GetCurrentDirectory();
                string activityPath = Path.Combine(currentDir, "data", "ActivityReport", fileName);
                string batchPath = Path.Combine(currentDir, "data", "BatchReport", fileName);
                if (System.IO.File.Exists(activityPath))
                {
                    System.IO.File.Delete(activityPath);
                    return true;
                }
                if (System.IO.File.Exists(batchPath))
                {
                    System.IO.File.Delete(batchPath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public DBTMReportsListModel NameWiseMultipleReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest)
        {
            DBTMReportsListModel dBTMReportsListModel = new DBTMReportsListModel();
            List<string> dBTMTestMasterIdList = dBTMTestMasterIds.Split(",").ToList();
            var testList = _dBTMTestMasterRepository.Table.Where(x => dBTMTestMasterIdList.Contains(x.DBTMTestMasterId.ToString()) && x.IsActive).Select(x => new { x.DBTMTestMasterId, x.TestName });
            if (!string.IsNullOrWhiteSpace(dBTMTestMasterIds))
            {
                if (dBTMReportsListModel.DataTableList == null)
                    dBTMReportsListModel.DataTableList = new List<KeyValuePair<string, DataTable>>();

                foreach (string testId in dBTMTestMasterIds.Split(',').ToList())
                {
                    if (!string.IsNullOrWhiteSpace(testId))
                    {
                        DBTMReportsListModel list = GetTestWiseReports(Convert.ToInt32(testId), dBTMTraineeDetailId, fromDate, toDate, entityId, userType, centreCode, isMobileRequest);
                        if (list?.DataTable?.Rows?.Count > 0)
                        {
                            var test = testList.FirstOrDefault(x => x.DBTMTestMasterId == Convert.ToInt32(testId));
                            dBTMReportsListModel.DataTableList.Add(new KeyValuePair<string, DataTable>(test.TestName, list.DataTable));
                        }
                    }
                }
            }
            return dBTMReportsListModel;
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
            DBTMReportsListModel dBTMReportsListModel = new DBTMReportsListModel();
            dBTMReportsListModel.DataTable = BindDBTMDataDetails(dBTMTestMasterId, isMobileRequest, dBTMReportsList, fromDate, toDate);
            return dBTMReportsListModel;
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

        private DataTable BindDBTMDataDetails(int dBTMTestMasterId, bool isMobileRequest, List<DBTMReportsModel> dBTMReportsList, DateTime fromDate, DateTime toDate)
        {
            //DBTMReportsListModel listModel = new DBTMReportsListModel();
            DataTable dataTable = new DataTable();
            if (dBTMReportsList?.Count > 0)
            {
                List<string> displayColumn = isMobileRequest
                       ? new List<string> { "Activity Time", "Person Name" }
                       : new List<string> { "Activity Time", "Person Name", "Activity Status", "Weight", "Height" };

                foreach (var item in displayColumn)
                    dataTable.Columns.Add(item, typeof(string));

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
                        newRow = dataTable.NewRow();
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
                            if (!dataTable.Columns.Contains(item1.CalculationName))
                            {
                                dataTable.Columns.Add(item1.CalculationName, typeof(String));
                            }
                            DBTMCustomHelper.Calculation(item1.CalculationCode, item1.CalculationName, newRow, dBTMReportsList, item.CreatedDate);
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

                foreach (DataColumn col in dataTable.Columns)
                {
                    col.ColumnName = $"{col.ColumnName} {DBTMCustomHelper.Unit(col.ColumnName)}";
                }
            }
            return dataTable;
        }
        private DataTable BindDBTMDataDetailsV2(int dBTMTestMasterId, bool isMobileRequest, List<DBTMReportsModel> dBTMReportsList, DateTime fromDate, DateTime toDate)
        {
            //DBTMReportsListModel listModel = new DBTMReportsListModel();
            DataTable dataTable = new DataTable();
            if (dBTMReportsList?.Count > 0)
            {
                List<string> displayColumnList = isMobileRequest
                 ? new List<string> { "Activity Time", "Person Name" }
                 : new List<string> { "Activity Time", "Person Name", "Activity Status", "Weight", "Height" };
                foreach (var paramColumn in displayColumnList)
                {
                    dataTable.Columns.Add(paramColumn, typeof(String));
                }

                List<DBTMTestParameterListviewSequence> listviewSequenceColumns = _dBTMTestParameterListviewSequenceRepository.Table
                                           .Where(x => x.DBTMTestMasterId == dBTMTestMasterId)
                                           .OrderBy(y => y.SequenceNumber)
                                           .ToList();
                List<DBTMTestParameterListviewSequence> listviewSequenceColumnsOriginal = new List<DBTMTestParameterListviewSequence>(listviewSequenceColumns);
                List<string> listviewSequenceColumnList = BindReportColumns(dBTMTestMasterId, isMobileRequest, dataTable, listviewSequenceColumns);
                DataRow newRow = null;
                foreach (var group in dBTMReportsList.GroupBy(x => x.CreatedDate))
                {
                    newRow = dataTable.NewRow();

                    //Bind Activity Person Details
                    foreach (string displayColumnName in displayColumnList)
                    {
                        switch (displayColumnName)
                        {
                            case "Activity Name":
                                newRow["Activity Name"] = group.FirstOrDefault().TestName;
                                break;
                            case "Person Name":
                                newRow["Person Name"] = $"{group.FirstOrDefault().FirstName} {group.FirstOrDefault().LastName}";
                                break;
                            case "Activity Status":
                                newRow["Activity Status"] = group.FirstOrDefault().ActivityStatus;//$"<span class=\"badge badge-soft-info\">{item.ActivityStatus}</span>";
                                break;
                            case "Weight":
                                newRow["Weight"] = $"{group.FirstOrDefault().Weight} {DBTMCustomHelper.Unit("Weight")}";
                                break;
                            case "Height":
                                newRow["Height"] = $"{group.FirstOrDefault().Height} {DBTMCustomHelper.Unit("Height")}";
                                break;
                            case "Activity Time":
                                newRow["Activity Time"] = isMobileRequest && fromDate.Date == toDate.Date
                                    ? group.FirstOrDefault().TestPerformedTime.ToString("hh:mm:ss tt")
                                    : group.FirstOrDefault().TestPerformedTime;
                                break;
                        }
                    }
                    BindParameterValue(listviewSequenceColumnList, group.ToLookup(x => x.CreatedDate.ToString()).FirstOrDefault(), listviewSequenceColumnsOriginal, newRow);
                    dataTable.Rows.Add(newRow);
                }

                //foreach (DataColumn col in dataTable.Columns)
                //{
                //    col.ColumnName = $"{col.ColumnName} {DBTMCustomHelper.Unit(col.ColumnName)}";
                //}
            }
            return dataTable;
        }
        private void BindParameterValue(List<string> listviewSequenceColumnList, IGrouping<string, DBTMReportsModel> group, List<DBTMTestParameterListviewSequence> listviewSequenceColumns, DataRow newRow)
        {
            foreach (var displayColumn in listviewSequenceColumnList)
            {
                string[] spilt = displayColumn.Split('-');
                DBTMTestParameterListviewSequence dBTMTestParameterListviewSequence = spilt.Length > 1 ? listviewSequenceColumns.FirstOrDefault(x => x.ParameterCode == spilt[0]) :
                                                                                                         listviewSequenceColumns.FirstOrDefault(x => x.ParameterCode == displayColumn);
                if (dBTMTestParameterListviewSequence == null)
                    newRow[displayColumn] = "NA";
                else
                {
                    if (dBTMTestParameterListviewSequence.IsCalculatedParameter)
                    {
                        if (spilt.Length == 1)
                            newRow[displayColumn] = DBTMCustomHelper.Calculation(dBTMTestParameterListviewSequence.ParameterCode, dBTMTestParameterListviewSequence.ParameterCode, newRow, group,1);
                        else
                            newRow[displayColumn] = DBTMCustomHelper.Calculation(dBTMTestParameterListviewSequence.ParameterCode, dBTMTestParameterListviewSequence.ParameterCode, newRow, group, Convert.ToInt32(spilt[1]));
                    }
                    else
                    {
                        if (spilt.Length == 1)
                            newRow[displayColumn] = group.FirstOrDefault(x => x.ParameterCode == spilt[0] && x.Row == "1")?.ParameterValue.ToString() ?? "NA";
                        else
                        {
                            newRow[displayColumn] = group.FirstOrDefault(x => x.ParameterCode == spilt[0] && x.Row == spilt[1])?.ParameterValue.ToString();
                        }
                    }
                }
            }
        }

        private List<string> BindReportColumns(int dBTMTestMasterId, bool isMobileRequest, DataTable dataTable, List<DBTMTestParameterListviewSequence> listviewSequenceColumns)
        {

            List<string> listviewSequenceColumnList = new List<string>();
            // Create a copy to safely iterate and remove items
            for (int idx = 0; idx < listviewSequenceColumns.Count; idx++)
            {
                var item = listviewSequenceColumns[idx];
                var consecutiveParameterData = listviewSequenceColumns.FirstOrDefault(x => x.ConsecutiveParameterCode == item.ParameterCode);

                if (consecutiveParameterData != null && !string.IsNullOrEmpty(consecutiveParameterData.ParameterCode))
                {
                    for (Int16 i = 1; i <= item.Recursion; i++)
                    {
                        listviewSequenceColumnList.Add($"{item.ParameterCode}-{i}");
                        listviewSequenceColumnList.Add($"{consecutiveParameterData.ParameterCode}-{i}");
                    }
                    listviewSequenceColumns.Remove(consecutiveParameterData);
                    // If the removed item is ahead of the current index, adjust the index
                    if (idx > listviewSequenceColumns.IndexOf(item))
                        idx--;
                }
                else
                {
                    if (item.Recursion == 1)
                    {
                        listviewSequenceColumnList.Add(item.ParameterCode);
                        continue;
                    }
                    else
                    {
                        for (Int16 i = 1; i <= item.Recursion; i++)
                        {
                            listviewSequenceColumnList.Add($"{item.ParameterCode}-{i}");
                        }
                    }
                }
            }
            foreach (var paramColumn in listviewSequenceColumnList)
            {
                dataTable.Columns.Add(paramColumn, typeof(String));
            }
            return listviewSequenceColumnList;
        }
    }
}
