using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Helper;
using Coditech.Common.Logger;
using Coditech.Common.Service;
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
        }

        public virtual DBTMBatchWiseReportsListModel BatchWiseReports(int generalBatchMasterId)
        {
            int dBTMTestMasterId = _dBTMBatchActivityRepository.Table.Where(x => x.GeneralBatchMasterId == generalBatchMasterId).FirstOrDefault().DBTMTestMasterId;
            if (dBTMTestMasterId <= 0)
            {
                return new DBTMBatchWiseReportsListModel();
            }
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<DBTMBatchWiseReportsModel> objStoredProc = new CoditechViewRepository<DBTMBatchWiseReportsModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@GeneralBatchMasterId", generalBatchMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@DBTMTestMasterId", dBTMTestMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMBatchWiseReportsModel> dBTMBatchWiseReportsList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMBatchWiseReportsList @GeneralBatchMasterId,@DBTMTestMasterId,@RowsCount OUT", 1, out pageListModel.TotalRowCount)?.ToList();
            DBTMBatchWiseReportsListModel listModel = new DBTMBatchWiseReportsListModel();

            listModel.BindPageListModel(pageListModel);
            if (dBTMBatchWiseReportsList?.Count > 0)
            {
                listModel.DataTable.Columns.Add("Test Name", typeof(String));
                listModel.DataTable.Columns.Add("Person Name", typeof(String));
                listModel.DataTable.Columns.Add("Date", typeof(String));
                listModel.DataTable.Columns.Add("Weight", typeof(String));
                listModel.DataTable.Columns.Add("Height", typeof(String));
                var testColumnList = (from a in _dBTMParametersAssociatedToTestRepository.Table
                                      join b in _dBTMTestParameterRepository.Table
                                      on a.DBTMTestParameterId equals b.DBTMTestParameterId
                                      where a.DBTMTestMasterId == dBTMTestMasterId && a.IsActive
                                      select new
                                      {
                                          b.ParameterName,
                                          b.ParameterCode
                                      })?.Distinct()?.ToList();

                DataRow newRow = listModel.DataTable.NewRow();
                DateTime dateTime = DateTime.Now;
                foreach (var item in dBTMBatchWiseReportsList)
                {
                    if (dateTime != item.CreatedDate)
                    {
                        newRow["Test Name"] = item.TestName;
                        newRow["Person Name"] = $"{item.FirstName} {item.LastName}";
                        newRow["Date"] = item.CreatedDate;
                        newRow["Weight"] = item.Weight;
                        newRow["Height"] = item.Height;
                    }
                    dateTime = item.CreatedDate;
                    string parameterName = testColumnList.FirstOrDefault(x => x.ParameterCode == item.ParameterCode)?.ParameterName;
                    if (!string.IsNullOrEmpty(parameterName))
                    {
                        string columnName = string.IsNullOrEmpty(item.FromTo) ? parameterName : $"{item.FromTo}-{parameterName}";
                        if (!listModel.DataTable.Columns.Contains(columnName))
                        {
                            listModel.DataTable.Columns.Add(columnName, typeof(String));
                        }

                        newRow[columnName] = $"{item.ParameterValue} {Unit(item.ParameterCode)}";
                    }
                }
                var calculationColumns = (from a in _dBTMCalculationAssociatedToTestRepository.Table
                                          join b in _dBTMTestCalculationRepository.Table
                                          on a.DBTMTestCalculationId equals b.DBTMTestCalculationId
                                          where a.DBTMTestMasterId == dBTMTestMasterId
                                          orderby b.OrderBy ascending
                                          select new { b.CalculationName, b.CalculationCode })?.Distinct()?.ToList();
                foreach (var item in calculationColumns)
                {
                    listModel.DataTable.Columns.Add(item.CalculationName, typeof(String));
                    Calculation(item.CalculationCode, item.CalculationName, newRow, dBTMBatchWiseReportsList);
                }
                listModel.DataTable.Rows.Add(newRow);
            }
            return listModel;
        }
        private void Calculation(string calculationCode, string calculationName, DataRow newRow, List<DBTMBatchWiseReportsModel> dBTMBatchWiseReportsList)
        {
            switch (calculationCode)
            {
                case "CompletionTime":
                    decimal completionTime = dBTMBatchWiseReportsList.Where(x => x.ParameterCode == "Time").Sum(x => x.ParameterValue);
                    newRow[calculationName] = $"{completionTime} {Unit(calculationCode)}";
                    break;
                case "AverageVelocity":
                    decimal totalDistance = dBTMBatchWiseReportsList.Where(x => x.ParameterCode == "Distance").Sum(x => x.ParameterValue);
                    decimal totalTime = dBTMBatchWiseReportsList.Where(x => x.ParameterCode == "Time").Sum(x => x.ParameterValue);
                    newRow[calculationName] = $" {Math.Round(totalDistance / totalTime, 3)} {Unit(calculationCode)}";
                    break;
                case "MaxLap":
                    newRow[calculationName] = $"{dBTMBatchWiseReportsList.Where(x => x.ParameterCode == "Time").Max(x => x.ParameterValue)} {Unit(calculationCode)}";
                    break;
                case "MinLap":
                    newRow[calculationName] = $"{dBTMBatchWiseReportsList.Where(x => x.ParameterCode == "Time").Min(x => x.ParameterValue)} {Unit(calculationCode)}";
                    break;
                case "Power":
                    newRow[calculationName] = $"{dBTMBatchWiseReportsList.FirstOrDefault(x => x.ParameterCode == "Power").ParameterValue} {Unit(calculationCode)}";
                    break;
                default:
                    newRow[calculationName] = "N/A";
                    break;
            }
        }

        private string Unit(string parameterCode)
        {
            string data = string.Empty;
            switch (parameterCode)
            {
                case "CompletionTime":
                case "Time":
                    data = "sec";
                    break;
                case "Distance":
                    data = "m";
                    break;
                case "AverageVelocity":
                    data = "m/s";
                    break;
                case "Power":
                    data = "watt";
                    break;
                default:
                    data = "";
                    break;
            }
            return data;
        }
    }
}
