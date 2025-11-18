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
using System.Text.RegularExpressions;
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
        private readonly ICoditechRepository<DBTMTestParameterListViewSequence> _dBTMTestParameterListviewSequenceRepository;
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
            _dBTMTestParameterListviewSequenceRepository = new CoditechRepository<DBTMTestParameterListViewSequence>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        public List<GraphModel> TestWiseGraphReportsV2(int dBTMTestMasterId, long dBTMTraineeDetailId, string dBTMGraphMasterIds, string graphMode, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest)
        {
            List<GraphModel> graphModelList = new List<GraphModel>();
            foreach (string dBTMGraphMasterId in dBTMGraphMasterIds.Split(','))
            {
                graphModelList.Add(TestWiseGraphReports(dBTMTestMasterId, dBTMTraineeDetailId, Convert.ToInt32(dBTMGraphMasterId), graphMode, fromDate, toDate, entityId, userType, centreCode, isMobileRequest));
            }
            return graphModelList;
        }

        public GraphModel TestWiseGraphReports(int dBTMTestMasterId, long dBTMTraineeDetailId, int dBTMGraphMasterId, string graphMode, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest)
        {
            GraphModel graphModel = new GraphModel();
            DBTMGraphMaster graphMaster = _dBTMGraphMasterRepository.Table.Where(x => x.DBTMGraphMasterId == dBTMGraphMasterId).FirstOrDefault();
            string xParameter = string.IsNullOrEmpty(graphMaster.XParameterBasedOn) ? graphMaster.XParameter : graphMaster.XParameterBasedOn;
            string yParameter = string.IsNullOrEmpty(graphMaster.YParameterBasedOn) ? graphMaster.YParameter : graphMaster.YParameterBasedOn;

            List<DBTMReportsModel> dBTMReportsList = GetTestWiseGraphReportFromDB(dBTMTestMasterId, dBTMTraineeDetailId, xParameter, yParameter, fromDate, toDate, ref entityId, userType, centreCode);
            if (dBTMReportsList?.Count > 0)
            {
                DBTMTestMaster dbtmTestMaster = _dBTMTestMasterRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestMasterId).FirstOrDefault();
                string[] XValuesList = null;

                if (graphMaster.XParameter == "Split")
                {
                    if (dbtmTestMaster.TestCode == CustomConstants.ThreeHundredYardTest)
                    {
                        XValuesList = new string[] { "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9", "S10", "S11", "S12" };
                    }
                    else if (dbtmTestMaster.TestCode == "FiveZeroFiveAgilityTest")
                    {
                        XValuesList = new string[] { "A-B", "B-C", "C-B" };
                    }
                }
                else if (graphMaster.XParameter == "Date")
                {
                    List<string> xValues = new List<string>();
                    foreach (DateTime item in dBTMReportsList.Select(x => x.TestPerformedTime.Date).Distinct())
                    {
                        xValues.Add(item.ToString(CustomConstants.GraphDateFormat));
                    }
                    XValuesList = xValues.ToArray();
                }
                else if (graphMaster.XParameter == CustomConstants.Distance)
                {
                    if (dbtmTestMaster.TestCode == CustomConstants.ThreeHundredYardTest)
                    {
                        List<string> xValues = new List<string>();
                        double distance = 22.86;
                        for (double i = distance; i <= (distance * 12); i = i + distance)
                        {
                            xValues.Add(Math.Round(i, 2).ToString());
                        }
                        XValuesList = xValues.ToArray();
                    }
                }
                if (XValuesList != null)
                {
                    graphModel.IsRecordFound = true;
                    graphModel.GraphType = graphMaster.GraphType;
                    graphModel.GraphName = graphMaster.GraphName;
                    int colorIndex = 0;
                    var groupedReports = dBTMReportsList.GroupBy(x => x.CreatedDate);
                    var groupedReportsByDateFormat = dBTMReportsList.GroupBy(x => x.CreatedDate.ToString(CustomConstants.GraphDateFormat));
                    int groupedReportCount = groupedReports.Count();
                    if (graphMaster.IsCalculateAvarage && groupedReportCount > 1)
                    {
                        groupedReportCount++;
                    }
                    string[] colorPalette = Enumerable.Range(0, groupedReportCount).Select(i => $"hsl({i * 360 / groupedReportCount}, 70%, 50%)").ToArray();

                    if (graphModel.GraphType == "LineChart")
                    {
                        graphModel.LineChartModel = new LineChartModel()
                        {
                            LineChartId = dBTMTestMasterId.ToString(),
                            XAxisLabel = string.IsNullOrEmpty(DBTMCustomHelper.Unit(graphMaster.XParameter)) ? graphMaster.XAxixLabel : $"{graphMaster.XAxixLabel} ({DBTMCustomHelper.Unit(graphMaster.XParameter)})",
                            XValues = JsonConvert.SerializeObject(XValuesList),
                            YAxisLabel = $"{graphMaster.YAxixLabel} ({DBTMCustomHelper.Unit(graphMaster.YParameter)})",
                            Datasets = new List<LineGraphsDatasetModel>()
                        };

                        if (graphMaster.GraphMode == "InstantaneousChart")
                        {
                            BindInstantaneousChart(graphModel, graphMaster, yParameter, dBTMReportsList, colorIndex, groupedReports, colorPalette);
                        }
                        else if (graphMaster.GraphMode == "ProgressChart")
                        {
                            BindProgressChart(graphModel, graphMaster, yParameter, dBTMReportsList, colorIndex, groupedReportsByDateFormat, colorPalette);
                        }
                    }
                    //else if (graphModel.GraphType == "BarChart") 
                    //{
                    //    graphModel.BarChartModel = new BarChartModel()
                    //    {
                    //        BarChartId = dBTMTestMasterId.ToString(),
                    //        XAxisLabel = string.IsNullOrEmpty(DBTMCustomHelper.Unit(graphMaster.XParameter)) ? graphMaster.XAxixLabel : $"{graphMaster.XAxixLabel} ({DBTMCustomHelper.Unit(graphMaster.XParameter)})",
                    //        XValues = JsonConvert.SerializeObject(XValuesList),
                    //        YAxisLabel = $"{graphMaster.YAxixLabel} ({DBTMCustomHelper.Unit(graphMaster.YParameter)})",
                    //        Datasets = new List<BarGraphsDatasetModel>()
                    //    };

                    //    short i = 1;
                    //    foreach (var group in dBTMReportsList.GroupBy(x => x.CreatedDate))
                    //    {
                    //        short j = 1;
                    //        List<decimal> yValuesList = new List<decimal>();
                    //        if (graphMaster.IsYParameterCalculated)
                    //        {
                    //            foreach (var item in group.Where(x => x.ParameterCode == yParameter))
                    //            {
                    //                yValuesList.Add(Convert.ToDecimal(DBTMCustomHelper.Calculation(graphMaster.YParameter, string.Empty, group.ToLookup(x => x.CreatedDate.ToString()).FirstOrDefault(), j, false)));
                    //                j++;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            foreach (var item in group.Where(x => x.ParameterCode == yParameter))
                    //            {
                    //                yValuesList.Add(Convert.ToDecimal(dBTMReportsList.Where(x => x.ParameterCode == yParameter && x.CreatedDate == group.Key && x.Row == j).Select(x => x.ParameterValue).FirstOrDefault()));
                    //                j++;
                    //            }
                    //        }
                    //        graphModel.LineChartModel.Datasets.Add(new LineGraphsDatasetModel()
                    //        {
                    //            Color = colorPalette[colorIndex % colorPalette.Length],
                    //            Label = $"Turn {i} {graphMaster.YAxixLabel}",
                    //            Data = JsonConvert.SerializeObject(yValuesList.ToArray()),
                    //        });
                    //        colorIndex++;
                    //        i++;
                    //    }
                    //}
                }
            }
            return graphModel;
        }

        private void BindInstantaneousChart(GraphModel graphModel, DBTMGraphMaster graphMaster, string yParameter, List<DBTMReportsModel> dBTMReportsList, int colorIndex, IEnumerable<IGrouping<DateTime, DBTMReportsModel>> groupedReports, string[] colorPalette)
        {
            short i = 1;
            foreach (var group in groupedReports)
            {
                short j = 1;
                List<decimal> yValuesList = new List<decimal>();
                if (graphMaster.IsYParameterCalculated)
                {
                    foreach (var item in group.Where(x => x.ParameterCode == yParameter))
                    {
                        yValuesList.Add(Convert.ToDecimal(DBTMCustomHelper.Calculation(graphMaster.YParameter, string.Empty, group.ToLookup(x => x.CreatedDate.ToString()).FirstOrDefault(), j, false, true)));
                        j++;
                    }
                }
                else
                {
                    foreach (var item in group.Where(x => x.ParameterCode == yParameter))
                    {
                        yValuesList.Add(Convert.ToDecimal(dBTMReportsList.Where(x => x.ParameterCode == yParameter && x.CreatedDate == group.Key && x.Row == j).Select(x => x.ParameterValue).FirstOrDefault()));
                        j++;
                    }
                }
                graphModel.LineChartModel.Datasets.Add(new LineGraphsDatasetModel()
                {
                    Color = colorPalette[colorIndex % colorPalette.Length],
                    Label = $"Turn {i} {graphMaster.YAxixLabel}",
                    Data = JsonConvert.SerializeObject(yValuesList.ToArray()),
                });
                colorIndex++;
                i++;
            }

            if (graphMaster.IsCalculateAvarage && graphModel.LineChartModel.Datasets.Count() > 1)
            {
                List<decimal> yValuesList = new List<decimal>();
                int datasetsCount = graphModel.LineChartModel.Datasets.Count();
                var dataArray1 = JsonConvert.DeserializeObject<decimal[]>(graphModel.LineChartModel.Datasets[0].Data);
                int dataCount = dataArray1.Count();
                for (int index = 0; index < dataCount; index++)
                {
                    decimal sum = 0;
                    foreach (var dataset in graphModel.LineChartModel.Datasets)
                    {
                        var dataArray = JsonConvert.DeserializeObject<decimal[]>(dataset.Data);
                        sum += dataArray[index];
                    }
                    yValuesList.Add(sum / dataCount);

                }
                graphModel.LineChartModel.Datasets.Add(new LineGraphsDatasetModel()
                {
                    Color = colorPalette[colorIndex % colorPalette.Length],
                    Label = $"Avarage {graphMaster.YAxixLabel}",
                    Data = JsonConvert.SerializeObject(yValuesList.ToArray()),
                });
            }
        }

        private void BindProgressChart(GraphModel graphModel, DBTMGraphMaster graphMaster, string yParameter, List<DBTMReportsModel> dBTMReportsList, int colorIndex, IEnumerable<IGrouping<string, DBTMReportsModel>> groupedReports, string[] colorPalette)
        {
            List<decimal> yValuesList = new List<decimal>();
            foreach (var group in groupedReports)
            {
                if (graphMaster.IsYParameterCalculated)
                {
                    short count = Convert.ToInt16(group.Select(x => x.CreatedDate).Distinct().Count());
                    yValuesList.Add(Convert.ToDecimal(DBTMCustomHelper.Calculation(graphMaster.YParameter, string.Empty, group.ToLookup(x => x.CreatedDate.ToString()).FirstOrDefault(), count, false, true)));
                }
            }
            graphModel.LineChartModel.Datasets.Add(new LineGraphsDatasetModel()
            {
                Color = colorPalette[colorIndex % colorPalette.Length],
                Label = $"{graphMaster.YAxixLabel}",
                Data = JsonConvert.SerializeObject(yValuesList.ToArray()),
            });
            colorIndex++;
        }

        public DBTMReportsListModel BatchWiseReports(int generalBatchMasterId, int dBTMTestMasterId, DateTime FromDate, DateTime ToDate, bool isMobileRequest, bool isDownloadReport)
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
            dBTMReportsListModel.DataTable = BindDBTMDataDetails(dBTMTestMasterId, isMobileRequest, dBTMReportsList, FromDate, ToDate, isDownloadReport);
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
                        DBTMReportsListModel list = BatchWiseReports(generalBatchMasterId, Convert.ToInt32(testId), FromDate, ToDate, isMobileRequest, false);
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
                        DBTMReportsListModel list = BatchWiseReports(generalBatchMasterId, Convert.ToInt32(testId), fromDate, toDate, isMobileRequest, true);

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
                    string sheetName = table.Key.Trim();
                    var replacements = new Dictionary<string, string>
                    {
                        { "5-0-5", "FiveZeroFive" },
                        { "5-10-5", "FiveTenFive" }
                    };
                    foreach (var kv in replacements)
                    {
                        if (sheetName.Contains(kv.Key))
                        {
                            sheetName = sheetName.Replace(kv.Key, kv.Value);
                        }
                    }
                    if (!sheetName.Contains("FiveZeroFive") && !sheetName.Contains("FiveTenFive"))
                    {
                        Match match = Regex.Match(sheetName, @"^(\d+)");
                        if (match.Success)
                        {
                            int number = int.Parse(match.Value);
                            string numberInWords = NumberToWords(number);
                            sheetName = Regex.Replace(sheetName, @"^(\d+)", numberInWords);
                        }
                    }
                    char[] invalidChars = { ':', '\\', '/', '?', '*', '[', ']' };
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
            return GetTestWiseReports(dBTMTestMasterId, dBTMTraineeDetailId, fromDate, toDate, entityId, userType, centreCode, isMobileRequest, false);
        }

        public DBTMReportsListModel TestWiseMultipleReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest, bool isDownloadReport)
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
                        DBTMReportsListModel list = GetTestWiseReports(Convert.ToInt32(testId), dBTMTraineeDetailId, fromDate, toDate, entityId, userType, centreCode, isMobileRequest, isDownloadReport);
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
            var reportData = TestWiseMultipleReports(dBTMTestMasterIds, dBTMTraineeDetailId, fromDate, toDate, entityId, userType, centreCode, isMobileRequest, true);
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
                    string sheetName = table.Key.Trim();
                    var replacements = new Dictionary<string, string>
                    {
                        { "5-0-5", "FiveZeroFive" },
                        { "5-10-5", "FiveTenFive" }
                    };
                    foreach (var kv in replacements)
                    {
                        if (sheetName.Contains(kv.Key))
                        {
                            sheetName = sheetName.Replace(kv.Key, kv.Value);
                        }
                    }
                    if (!sheetName.Contains("FiveZeroFive") && !sheetName.Contains("FiveTenFive"))
                    {
                        Match match = Regex.Match(sheetName, @"^(\d+)");
                        if (match.Success)
                        {
                            int number = int.Parse(match.Value);
                            string numberInWords = NumberToWords(number);
                            sheetName = Regex.Replace(sheetName, @"^(\d+)", numberInWords);
                        }
                    }
                    char[] invalidChars = { ':', '\\', '/', '?', '*', '[', ']' };
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
            reportData.FilePath = filePath;
            reportData.FileName = fileName;
            return reportData;
        }

        // Delete Report File from Data folder
        public bool DeleteReportsFile(string fileName)
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
                        DBTMReportsListModel list = GetTestWiseReports(Convert.ToInt32(testId), dBTMTraineeDetailId, fromDate, toDate, entityId, userType, centreCode, isMobileRequest, false);
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

        #region Private Methods
        private DBTMReportsListModel GetTestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest, bool isDownloadReport)
        {
            List<DBTMReportsModel> dBTMReportsList = GetTestWiseReportFromDB(dBTMTestMasterId, dBTMTraineeDetailId, fromDate, toDate, ref entityId, userType, centreCode);
            DBTMReportsListModel dBTMReportsListModel = new DBTMReportsListModel();
            dBTMReportsListModel.DataTable = BindDBTMDataDetails(dBTMTestMasterId, isMobileRequest, dBTMReportsList, fromDate, toDate, isDownloadReport);
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

        private DataTable BindDBTMDataDetails(int dBTMTestMasterId, bool isMobileRequest, List<DBTMReportsModel> dBTMReportsList, DateTime fromDate, DateTime toDate, bool isDownloadReport)
        {
            DataTable dataTable = new DataTable();
            if (dBTMReportsList?.Count > 0)
            {
                string displayOn = isMobileRequest ? "OnlyMobileApp" : "OnlyWeb";
                List<DBTMTestParameterListViewSequence> listviewSequenceColumns = _dBTMTestParameterListviewSequenceRepository.Table
                                          .Where(x => x.DBTMTestMasterId == dBTMTestMasterId && x.IsActive && (x.DisplayOn.Contains("both") || x.DisplayOn == displayOn))
                                          .OrderBy(y => y.SequenceNumber)
                                          .ToList();
                if (listviewSequenceColumns != null && listviewSequenceColumns.Any())
                {
                    return BindDBTMDataDetailsV2(dBTMTestMasterId, isMobileRequest, dBTMReportsList, fromDate, toDate, listviewSequenceColumns, isDownloadReport);
                }

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

        private DataTable BindDBTMDataDetailsV2(int dBTMTestMasterId, bool isMobileRequest, List<DBTMReportsModel> dBTMReportsList, DateTime fromDate, DateTime toDate, List<DBTMTestParameterListViewSequence> listviewSequenceColumns, bool isDownloadReport)
        {
            //DBTMReportsListModel listModel = new DBTMReportsListModel();
            DataTable dataTable = new DataTable();
            if (dBTMReportsList?.Count > 0)
            {
                List<string> displayColumnList = isMobileRequest
                 ? new List<string> { "Activity Time", "Person Name" }
                 : new List<string> { "Activity Time", "Person Name", "Activity Status", "Weight(kg)", "Height(cm)" };
                foreach (var paramColumn in displayColumnList)
                {
                    dataTable.Columns.Add(paramColumn, typeof(String));
                }

                //List<DBTMTestParameterListviewSequence> listviewSequenceColumns = _dBTMTestParameterListviewSequenceRepository.Table
                //                           .Where(x => x.DBTMTestMasterId == dBTMTestMasterId)
                //                           .OrderBy(y => y.SequenceNumber)
                //                           .ToList();
                List<DBTMTestParameterListViewSequence> listviewSequenceColumnsOriginal = new List<DBTMTestParameterListViewSequence>(listviewSequenceColumns);
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
                            case "Weight(kg)":
                                newRow["Weight(kg)"] = $"{group.FirstOrDefault().Weight}";
                                break;
                            case "Height(cm)":
                                newRow["Height(cm)"] = $"{group.FirstOrDefault().Height}";
                                break;
                            case "Activity Time":
                                newRow["Activity Time"] = isMobileRequest && fromDate.Date == toDate.Date
                                    ? group.FirstOrDefault().TestPerformedTime.ToString("hh:mm:ss tt")
                                    : group.FirstOrDefault().TestPerformedTime;
                                break;
                        }
                    }
                    BindParameterValue(listviewSequenceColumnList, group.ToLookup(x => x.CreatedDate.ToString()).FirstOrDefault(), listviewSequenceColumnsOriginal, newRow, isMobileRequest, isDownloadReport);
                    dataTable.Rows.Add(newRow);
                }

                //Updated Column Name
                UpdateDatatableColumnName(dBTMReportsList, dataTable, listviewSequenceColumnsOriginal, isMobileRequest);
            }
            return dataTable;
        }

        private static void UpdateDatatableColumnName(List<DBTMReportsModel> dBTMReportsList, DataTable dataTable, List<DBTMTestParameterListViewSequence> listviewSequenceColumnsOriginal, bool isMobileRequest)
        {
            string updatedColumnName = string.Empty;
            foreach (DataColumn col in dataTable.Columns)
            {
                string[] spilt = col.ColumnName.Split('-');
                DBTMTestParameterListViewSequence dBTMTestParameterListviewSequence = spilt.Length > 1 ? listviewSequenceColumnsOriginal.FirstOrDefault(x => x.ParameterCode == spilt[0]) :
                                                                                                         listviewSequenceColumnsOriginal.FirstOrDefault(x => x.ParameterCode == col.ColumnName);

                if (dBTMTestParameterListviewSequence != null)
                {
                    updatedColumnName = dBTMTestParameterListviewSequence.ColumnName;
                    if (spilt.Length > 1)
                    {
                        string fromTo = string.Empty;
                        var dBTMReportsListGroupByData = dBTMReportsList.GroupBy(x => x.CreatedDate).LastOrDefault();
                        if (!string.IsNullOrEmpty(dBTMTestParameterListviewSequence.ConsecutiveParameterCode) && dBTMTestParameterListviewSequence.IsCalculatedParameter)
                        {
                            if (dBTMTestParameterListviewSequence.IsCalculatedParameter)
                            {
                                if (dBTMTestParameterListviewSequence.ParameterCode == CustomConstants.CumulativeTime ||
                                    dBTMTestParameterListviewSequence.ParameterCode == CustomConstants.CumulativeVelocity ||
                                    dBTMTestParameterListviewSequence.ParameterCode == CustomConstants.Velocity ||
                                    dBTMTestParameterListviewSequence.ParameterCode == CustomConstants.VelocityByRow ||
                                    dBTMTestParameterListviewSequence.ParameterCode == CustomConstants.CumulativeVelocityByRow ||
                                    dBTMTestParameterListviewSequence.ParameterCode == CustomConstants.AccelerationByRow ||
                                    dBTMTestParameterListviewSequence.ParameterCode == CustomConstants.ForceByRow ||
                                    dBTMTestParameterListviewSequence.ParameterCode == CustomConstants.PowerByRow
                                    )
                                {
                                    fromTo = dBTMReportsListGroupByData.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.Row == Convert.ToInt16(spilt[1]))?.FromTo;
                                }
                            }
                            else
                                fromTo = dBTMReportsListGroupByData.FirstOrDefault(x => x.ParameterCode == dBTMTestParameterListviewSequence.ConsecutiveParameterCode && x.Row == Convert.ToInt16(spilt[1]))?.FromTo;
                        }
                        else
                        {
                            if (dBTMTestParameterListviewSequence.ParameterCode == CustomConstants.Velocity)
                            {
                                fromTo = dBTMReportsListGroupByData.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.Row == Convert.ToInt16(spilt[1]))?.FromTo;
                            }
                            else
                            {
                                fromTo = dBTMReportsListGroupByData.FirstOrDefault(x => x.ParameterCode == spilt[0] && x.Row == Convert.ToInt16(spilt[1]))?.FromTo;
                            }
                        }
                        updatedColumnName = updatedColumnName.Replace("{FromTo}", fromTo);
                        updatedColumnName = updatedColumnName.Replace("{Row}", spilt[1]);
                        if (updatedColumnName.Contains("{Distance*Row}"))
                        {
                            decimal distance = dBTMReportsListGroupByData.FirstOrDefault(x => x.ParameterCode == CustomConstants.Distance).ParameterValue * Convert.ToInt32(spilt[1]);
                            updatedColumnName = updatedColumnName.Replace("{Distance*Row}", distance.ToString());
                        }
                        updatedColumnName = updatedColumnName.Replace("{Row}", spilt[1]);
                    }
                    updatedColumnName = updatedColumnName.Replace("{DistanceUnit}", DBTMCustomHelper.Unit(CustomConstants.Distance));
                    updatedColumnName = updatedColumnName.Replace("{Unit}", DBTMCustomHelper.Unit(dBTMTestParameterListviewSequence.ParameterCode));
                    if (!dataTable.Columns.Contains(updatedColumnName))
                    {
                        if (!isMobileRequest)
                        {
                            if (string.IsNullOrEmpty(dBTMTestParameterListviewSequence.HelpText))
                            {
                                col.ColumnName = updatedColumnName;
                            }
                            else
                            {
                                col.ColumnName = $"{updatedColumnName}~{dBTMTestParameterListviewSequence.HelpText}";
                            }
                        }
                        else
                        {
                            col.ColumnName = updatedColumnName;
                        }
                    }
                }
            }
        }

        private void BindParameterValue(List<string> listviewSequenceColumnList, IGrouping<string, DBTMReportsModel> group, List<DBTMTestParameterListViewSequence> listviewSequenceColumns, DataRow newRow, bool isMobileRequest, bool isDownloadReport)
        {
            foreach (var displayColumn in listviewSequenceColumnList)
            {
                string[] spilt = displayColumn.Split('-');
                DBTMTestParameterListViewSequence dBTMTestParameterListviewSequence = spilt.Length > 1 ? listviewSequenceColumns.FirstOrDefault(x => x.ParameterCode == spilt[0]) :
                                                                                                         listviewSequenceColumns.FirstOrDefault(x => x.ParameterCode == displayColumn);
                if (dBTMTestParameterListviewSequence == null)
                {
                    newRow[displayColumn] = "NA";
                    return;
                }
                string rowValue = string.Empty;
                if (displayColumn == "ModeOfStart" || displayColumn == "Direction")
                {
                    rowValue = !string.IsNullOrEmpty(group.FirstOrDefault(x => x.ParameterCode == displayColumn)?.Comment1) ? group.FirstOrDefault(x => x.ParameterCode == displayColumn)?.Comment1.ToString() : "NA";
                }
                else
                {
                    if (dBTMTestParameterListviewSequence.IsCalculatedParameter)
                    {
                        if (spilt.Length == 1)
                            rowValue = DBTMCustomHelper.Calculation(dBTMTestParameterListviewSequence.ParameterCode, dBTMTestParameterListviewSequence.ParameterCode, group, 1);
                        else
                            rowValue = DBTMCustomHelper.Calculation(dBTMTestParameterListviewSequence.ParameterCode, dBTMTestParameterListviewSequence.ParameterCode, group, Convert.ToInt16(spilt[1]));
                    }
                    else
                    {
                        if (spilt.Length == 1)
                            rowValue = group.FirstOrDefault(x => x.ParameterCode == spilt[0] && x.Row == 1)?.ParameterValue.ToString() ?? "NA";
                        else
                        {
                            rowValue = group.FirstOrDefault(x => x.ParameterCode == spilt[0] && x.Row == Convert.ToInt16(spilt[1]))?.ParameterValue.ToString();
                        }
                    }
                }
                newRow[displayColumn] = isMobileRequest || isDownloadReport ? rowValue : $"{rowValue}~{dBTMTestParameterListviewSequence.IsColumnCellBold}~{dBTMTestParameterListviewSequence.ColumnCellColor}";
            }
        }

        private List<string> BindReportColumns(int dBTMTestMasterId, bool isMobileRequest, DataTable dataTable, List<DBTMTestParameterListViewSequence> listviewSequenceColumns)
        {

            List<string> listviewSequenceColumnList = new List<string>();
            // Create a copy to safely iterate and remove items
            for (int idx = 0; idx < listviewSequenceColumns.Count; idx++)
            {
                var item = listviewSequenceColumns[idx];
                var consecutiveParameterDataList = listviewSequenceColumns.Where(x => x.ConsecutiveParameterCode == item.ParameterCode && x.Recursion == item.Recursion)?.ToList();

                if (consecutiveParameterDataList != null && consecutiveParameterDataList.Any())
                {
                    for (Int16 i = 1; i <= item.Recursion; i++)
                    {
                        int count = 1;
                        foreach (var consecutiveParameterData in consecutiveParameterDataList.OrderBy(x => x.SequenceNumber))
                        {
                            if (!string.IsNullOrEmpty(consecutiveParameterData.ParameterCode))
                            {
                                if (count == 1)
                                    listviewSequenceColumnList.Add($"{item.ParameterCode}-{i}");
                                listviewSequenceColumnList.Add($"{consecutiveParameterData.ParameterCode}-{i}");
                            }
                            listviewSequenceColumns.Remove(consecutiveParameterData);
                            // If the removed item is ahead of the current index, adjust the index
                            if (idx > listviewSequenceColumns.IndexOf(item))
                            {
                                idx--;
                            }
                            count++;
                        }
                    }
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

        private static string NumberToWords(int number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                string[] unitsMap = { "Zero", "One", "Two", "Three", "Four", "Five", "Six",
                              "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve",
                              "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen",
                              "Eighteen", "Nineteen" };

                string[] tensMap = { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty",
                             "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }
            return words.Trim();
        }
        #endregion
    }
}
