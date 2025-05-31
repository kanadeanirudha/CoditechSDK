using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;
using System.Collections.Specialized;
using System.Data;
using static Coditech.Common.Helper.HelperUtility;

namespace Coditech.API.Service
{
    public class DBTMTraineeDetailsService : BaseService, IDBTMTraineeDetailsService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMTraineeDetails> _dBTMTraineeDetailsRepository;
        private readonly ICoditechRepository<DBTMDeviceData> _dBTMDeviceDataRepository;
        private readonly ICoditechRepository<DBTMTestMaster> _dBTMTestMasterRepository;
        private readonly ICoditechRepository<DBTMParametersAssociatedToTest> _dBTMParametersAssociatedToTestRepository;
        private readonly ICoditechRepository<DBTMTestParameter> _dBTMTestParameterRepository;
        private readonly ICoditechRepository<DBTMCalculationAssociatedToTest> _dBTMCalculationAssociatedToTestRepository;
        private readonly ICoditechRepository<DBTMTestCalculation> _dBTMTestCalculationRepository;

        public DBTMTraineeDetailsService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMTraineeDetailsRepository = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMDeviceDataRepository = new CoditechRepository<DBTMDeviceData>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestMasterRepository = new CoditechRepository<DBTMTestMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMParametersAssociatedToTestRepository = new CoditechRepository<DBTMParametersAssociatedToTest>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestParameterRepository = new CoditechRepository<DBTMTestParameter>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMCalculationAssociatedToTestRepository = new CoditechRepository<DBTMCalculationAssociatedToTest>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestCalculationRepository = new CoditechRepository<DBTMTestCalculation>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        public virtual DBTMTraineeDetailsListModel GetDBTMTraineeDetailsList(string SelectedCentreCode, long generalTrainerMasterId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            string listType = "";
            string isActive = filters?.Find(x => string.Equals(x.FilterName, FilterKeys.IsActive, StringComparison.CurrentCultureIgnoreCase))?.FilterValue;
            if (!string.IsNullOrEmpty(isActive))
            {
                filters.RemoveAll(x => x.FilterName == FilterKeys.IsActive);
                listType = $"and IsActive={isActive}";
            }
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMTraineeDetailsModel> objStoredProc = new CoditechViewRepository<DBTMTraineeDetailsModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@CentreCode", SelectedCentreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@GeneralTrainerMasterId", generalTrainerMasterId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@ListType", listType, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMTraineeDetailsModel> dBTMTraineeDetailsList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMTraineeDetailsList @CentreCode,@GeneralTrainerMasterId,@listType,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 7, out pageListModel.TotalRowCount)?.ToList();
            DBTMTraineeDetailsListModel listModel = new DBTMTraineeDetailsListModel();

            listModel.DBTMTraineeDetailsList = dBTMTraineeDetailsList?.Count > 0 ? dBTMTraineeDetailsList : new List<DBTMTraineeDetailsModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }

        //Get DBTM Trainee Other Details
        public virtual DBTMTraineeDetailsModel GetDBTMTraineeOtherDetails(long dBTMTraineeDetailId)
        {
            if (dBTMTraineeDetailId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTraineeDetailId"));

            DBTMTraineeDetails dBTMTraineeDetails = _dBTMTraineeDetailsRepository.Table.FirstOrDefault(x => x.DBTMTraineeDetailId == dBTMTraineeDetailId);
            DBTMTraineeDetailsModel dBTMTraineeDetailsModel = dBTMTraineeDetails?.FromEntityToModel<DBTMTraineeDetailsModel>();
            if (IsNotNull(dBTMTraineeDetailsModel))
            {
                GeneralPersonModel generalPersonModel = GetGeneralPersonDetails(dBTMTraineeDetailsModel.PersonId);
                if (IsNotNull(dBTMTraineeDetailsModel))
                {
                    dBTMTraineeDetailsModel.FirstName = generalPersonModel.FirstName;
                    dBTMTraineeDetailsModel.LastName = generalPersonModel.LastName;
                }
            }
            return dBTMTraineeDetailsModel;
        }

        //Update DBTM Trainee Other Details
        public virtual bool UpdateDBTMTraineeOtherDetails(DBTMTraineeDetailsModel dBTMTraineeDetailsModel)
        {
            if (IsNull(dBTMTraineeDetailsModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (dBTMTraineeDetailsModel.DBTMTraineeDetailId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTraineeDetailId"));

            DBTMTraineeDetails dBTMTraineeDetails = _dBTMTraineeDetailsRepository.Table.FirstOrDefault(x => x.DBTMTraineeDetailId == dBTMTraineeDetailsModel.DBTMTraineeDetailId);

            bool isUpdated = false;
            if (IsNull(dBTMTraineeDetails))
            {
                return isUpdated;
            }
            dBTMTraineeDetails.PastInjuries = dBTMTraineeDetailsModel.PastInjuries;
            dBTMTraineeDetails.MedicalHistory = dBTMTraineeDetailsModel.MedicalHistory;
            dBTMTraineeDetails.GroupEnumId = dBTMTraineeDetailsModel.GroupEnumId;
            dBTMTraineeDetails.SourceEnumId = dBTMTraineeDetailsModel.SourceEnumId;
            dBTMTraineeDetails.OtherInformation = dBTMTraineeDetailsModel.OtherInformation;
            dBTMTraineeDetails.IsActive = dBTMTraineeDetailsModel.IsActive;
            dBTMTraineeDetails.Weight = dBTMTraineeDetailsModel.Weight;
            dBTMTraineeDetails.Height = dBTMTraineeDetailsModel.Height;

            isUpdated = _dBTMTraineeDetailsRepository.Update(dBTMTraineeDetails);
            if (isUpdated)
            {
                ActiveInActiveUserLogin(dBTMTraineeDetails.IsActive, Convert.ToInt64(dBTMTraineeDetails.DBTMTraineeDetailId), UserTypeEnum.Trainee.ToString());
            }
            else
            {
                dBTMTraineeDetailsModel.HasError = true;
                dBTMTraineeDetailsModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isUpdated;
        }

        //Delete DBTM Trainee Details
        public virtual bool DeleteDBTMTraineeDetails(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTraineeDetailId"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("DBTMTraineeDetailIds", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteDBTMTraineeDetails @DBTMTraineeDetailIds,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        //TraineeActivitiesList
        public virtual DBTMActivitiesListModel GetTraineeActivitiesList(string personCode, int numberOfDaysRecord, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMActivitiesModel> objStoredProc = new CoditechViewRepository<DBTMActivitiesModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@PersonCode", personCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@NumberOfDaysRecord", numberOfDaysRecord, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMActivitiesModel> dBTMActivitiesList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetTraineeActivitiesList @PersonCode,@NumberOfDaysRecord,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 5, out pageListModel.TotalRowCount)?.ToList();
            DBTMActivitiesListModel listModel = new DBTMActivitiesListModel();

            listModel.ActivitiesList = dBTMActivitiesList?.Count > 0 ? dBTMActivitiesList : new List<DBTMActivitiesModel>();
            listModel.BindPageListModel(pageListModel);

            long? dBTMTraineeDetailId = _dBTMTraineeDetailsRepository.Table.Where(x => x.PersonCode == personCode)?.Select(y => y.DBTMTraineeDetailId)?.FirstOrDefault();
            if (dBTMTraineeDetailId > 0)
            {
                GeneralPersonModel generalPersonModel = GetDBTMGeneralPersonDetailsByEntityType((int)dBTMTraineeDetailId, UserTypeEnum.Trainee.ToString());
                if (IsNotNull(generalPersonModel))
                {
                    listModel.FirstName = generalPersonModel.FirstName;
                    listModel.LastName = generalPersonModel.LastName;
                    listModel.SelectedCentreCode = generalPersonModel.SelectedCentreCode;
                }
            }

            listModel.PersonCode = personCode;
            return listModel;
        }

        private GeneralPersonModel GetDBTMGeneralPersonDetailsByEntityType(long entityId, string entityType)
        {
            long personId = 0;
            string centreCode = string.Empty;
            string personCode = string.Empty;
            short generalDepartmentMasterId = 0;
            if (entityType == UserTypeEnum.Trainee.ToString())
            {
                DBTMTraineeDetails dbtmTraineeDetails = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>()).Table.FirstOrDefault(x => x.DBTMTraineeDetailId == entityId);
                if (IsNotNull(dbtmTraineeDetails))
                {
                    personId = dbtmTraineeDetails.PersonId;
                    centreCode = dbtmTraineeDetails.CentreCode;
                }
                return base.BindGeneralPersonInformation(personId, centreCode, personCode, generalDepartmentMasterId, dbtmTraineeDetails.IsActive);
            }
            else
            {
                return base.GetGeneralPersonDetailsByEntityType(entityId, entityType);
            }
        }
        public virtual DBTMActivitiesDetailsListModel GetTraineeActivitiesDetailsList(long dBTMDeviceDataId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMActivitiesDetailsModel> objStoredProc = new CoditechViewRepository<DBTMActivitiesDetailsModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@DBTMDeviceDataId", dBTMDeviceDataId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMActivitiesDetailsModel> dBTMActivitiesDetailsList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMDeviceDataDetailsList @DBTMDeviceDataId,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 5, out pageListModel.TotalRowCount)?.ToList();
            DBTMActivitiesDetailsListModel listModel = new DBTMActivitiesDetailsListModel();

            listModel.BindPageListModel(pageListModel);
            if (dBTMActivitiesDetailsList?.Count > 0)
            {
                DBTMDeviceData dBTMDeviceData = _dBTMDeviceDataRepository.Table.Where(x => x.DBTMDeviceDataId == dBTMDeviceDataId)?.FirstOrDefault();
                long? dBTMTraineeDetailId = _dBTMTraineeDetailsRepository.Table.Where(x => x.PersonCode == dBTMDeviceData.PersonCode)?.Select(y => y.DBTMTraineeDetailId)?.FirstOrDefault();

                if (dBTMTraineeDetailId > 0)
                {
                    GeneralPersonModel generalPersonModel = GetDBTMGeneralPersonDetailsByEntityType((int)dBTMTraineeDetailId, UserTypeEnum.Trainee.ToString());
                    if (IsNotNull(generalPersonModel))
                    {
                        listModel.FirstName = generalPersonModel.FirstName;
                        listModel.LastName = generalPersonModel.LastName;
                        listModel.PersonCode = dBTMDeviceData.PersonCode;
                    }
                    DBTMTestMaster dBTMTestMaster = _dBTMTestMasterRepository.Table.Where(x => x.TestCode == dBTMDeviceData.TestCode).FirstOrDefault();

                    if (dBTMTestMaster != null)
                    {
                        listModel.TestName = dBTMTestMaster.TestName;
                        var testColumnList = (from a in _dBTMParametersAssociatedToTestRepository.Table
                                              join b in _dBTMTestParameterRepository.Table
                                              on a.DBTMTestParameterId equals b.DBTMTestParameterId
                                              where a.DBTMTestMasterId == dBTMTestMaster.DBTMTestMasterId && a.IsActive
                                              select new
                                              {
                                                  b.ParameterName,
                                                  b.ParameterCode
                                              })?.Distinct()?.ToList();

                        DataRow newRow = listModel.DataTable.NewRow();
                        foreach (var item in dBTMActivitiesDetailsList)
                        {
                            string parameterName = testColumnList.FirstOrDefault(x => x.ParameterCode == item.ParameterCode)?.ParameterName;
                            if (!string.IsNullOrEmpty(parameterName))
                            {
                                string columnName = string.IsNullOrEmpty(item.FromTo) ? parameterName : $"{item.FromTo}-{parameterName}";
                                listModel.DataTable.Columns.Add(columnName, typeof(String));
                                newRow[columnName] = $"{ item.ParameterValue} {Unit(item.ParameterCode)}";
                            }
                        }

                        var calculationColumns = (from a in _dBTMCalculationAssociatedToTestRepository.Table
                                                  join b in _dBTMTestCalculationRepository.Table
                                                  on a.DBTMTestCalculationId equals b.DBTMTestCalculationId
                                                  where a.DBTMTestMasterId == dBTMTestMaster.DBTMTestMasterId
                                                  orderby b.OrderBy ascending
                                                  select new { b.CalculationName, b.CalculationCode })?.Distinct()?.ToList();
                        foreach (var item in calculationColumns)
                        {
                            listModel.DataTable.Columns.Add(item.CalculationName, typeof(String));
                            Calculation(item.CalculationCode, item.CalculationName, newRow, dBTMActivitiesDetailsList);
                        }
                        listModel.DataTable.Rows.Add(newRow);
                    }
                }
            }
            return listModel;
        }

        private void Calculation(string calculationCode, string calculationName, DataRow newRow, List<DBTMActivitiesDetailsModel> dBTMActivitiesDetailsList)
        {
            switch (calculationCode)
            {
                case "CompletionTime":
                    decimal completionTime = dBTMActivitiesDetailsList.Where(x => x.ParameterCode == "Time").Sum(x => x.ParameterValue);
                    newRow[calculationName] = $"{completionTime} {Unit(calculationCode)}";
                    break;
                case "AverageVelocity":
                    decimal totalDistance = dBTMActivitiesDetailsList.Where(x => x.ParameterCode == "Distance").Sum(x => x.ParameterValue);
                    decimal totalTime = dBTMActivitiesDetailsList.Where(x => x.ParameterCode == "Time").Sum(x => x.ParameterValue);
                    newRow[calculationName] = $" {Math.Round(totalDistance / totalTime, 3)} {Unit(calculationCode)}";
                    break;
                case "MaxLap":
                    newRow[calculationName] = $"{dBTMActivitiesDetailsList.Where(x => x.ParameterCode == "Time").Max(x => x.ParameterValue)} {Unit(calculationCode)}";
                    break;
                case "MinLap":
                    newRow[calculationName] = $"{dBTMActivitiesDetailsList.Where(x => x.ParameterCode == "Time").Min(x => x.ParameterValue)} {Unit(calculationCode)}";
                    break;
                case "Power":
                    newRow[calculationName] = $"{dBTMActivitiesDetailsList.FirstOrDefault(x => x.ParameterCode == "Power").ParameterValue} {Unit(calculationCode)}";
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
                    data = "sec";
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
