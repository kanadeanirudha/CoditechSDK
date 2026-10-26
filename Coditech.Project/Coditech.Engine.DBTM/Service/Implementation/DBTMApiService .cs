using Coditech.API.Data;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Transactions;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMApiService : BaseService, IDBTMApiService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<GeneralBatchMaster> _generalBatchRepository;
        private readonly ICoditechRepository<GeneralBatchUser> _generalBatchUserRepository;
        private readonly ICoditechRepository<UserMaster> _userMasterRepository;
        private readonly ICoditechRepository<EmployeeMaster> _employeeMasterRepository;
        private readonly ICoditechRepository<GeneralPerson> _generalPersonRepository;
        private readonly ICoditechRepository<DBTMBatchActivity> _dBTMBatchActivityRepository;
        private readonly ICoditechRepository<DBTMTestMaster> _dBTMTestMasterRepository;
        private readonly ICoditechRepository<DBTMTraineeAssignment> _dBTMTraineeAssignmentRepository;
        private readonly ICoditechRepository<DBTMTraineeAssignmentToUser> _dBTMTraineeAssignmentToUserRepository;
        private readonly ICoditechRepository<DBTMDeviceData> _dBTMDeviceDataRepository;
        private readonly ICoditechRepository<DBTMDeviceDataDetails> _dBTMDeviceDataDetailsRepository;
        private readonly ICoditechRepository<DBTMTraineeDetails> _dBTMTraineeDetailsRepository;
        private readonly ICoditechRepository<DBTMActivityCategory> _dBTMActivityCategoryRepository;
        private readonly ICoditechRepository<OrganisationCentrewiseJoiningCode> _organisationCentrewiseJoiningCodeRepository;
        private readonly ICoditechRepository<DBTMCentreWiseTest> _dBTMCentreWiseTestRepository;
        private readonly ICoditechRepository<DBTMCampMaster> _dBTMCampMasterRepository;
        private readonly ICoditechRepository<DBTMCampActivity> _dBTMCampActivityRepository;
        private readonly ICoditechRepository<DBTMDeviceDataInsertionUniqueCheck> _dBTMDeviceDataInsertionUniqueCheckRepository;
        private readonly ICoditechRepository<GeneralTrainerMaster> _generalTrainerMasterRepository;


        public DBTMApiService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _generalBatchRepository = new CoditechRepository<GeneralBatchMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _generalBatchUserRepository = new CoditechRepository<GeneralBatchUser>(_serviceProvider.GetService<Coditech_Entities>());
            _userMasterRepository = new CoditechRepository<UserMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _employeeMasterRepository = new CoditechRepository<EmployeeMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _generalPersonRepository = new CoditechRepository<GeneralPerson>(_serviceProvider.GetService<Coditech_Entities>());
            _dBTMBatchActivityRepository = new CoditechRepository<DBTMBatchActivity>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestMasterRepository = new CoditechRepository<DBTMTestMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTraineeAssignmentRepository = new CoditechRepository<DBTMTraineeAssignment>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTraineeAssignmentToUserRepository = new CoditechRepository<DBTMTraineeAssignmentToUser>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMDeviceDataRepository = new CoditechRepository<DBTMDeviceData>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMDeviceDataDetailsRepository = new CoditechRepository<DBTMDeviceDataDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTraineeDetailsRepository = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMActivityCategoryRepository = new CoditechRepository<DBTMActivityCategory>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _organisationCentrewiseJoiningCodeRepository = new CoditechRepository<OrganisationCentrewiseJoiningCode>(_serviceProvider.GetService<Coditech_Entities>());
            _dBTMCentreWiseTestRepository = new CoditechRepository<DBTMCentreWiseTest>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMCampMasterRepository = new CoditechRepository<DBTMCampMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMCampActivityRepository = new CoditechRepository<DBTMCampActivity>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMDeviceDataInsertionUniqueCheckRepository = new CoditechRepository<DBTMDeviceDataInsertionUniqueCheck>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _generalTrainerMasterRepository = new CoditechRepository<GeneralTrainerMaster>(_serviceProvider.GetService<Coditech_Entities>());
        }
        #region InsertDeviceData
        public bool InsertDeviceDataViaFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            using var reader = new StreamReader(file.OpenReadStream());
            string fileContent = reader.ReadToEnd();
            List<DBTMDeviceDataModel> dBTMDeviceDataModelList = JsonConvert.DeserializeObject<List<DBTMDeviceDataModel>>(fileContent);
            return InsertDeviceData(dBTMDeviceDataModelList);

        }

        //Add DBTMDeviceData.
        public bool InsertDeviceData(List<DBTMDeviceDataModel> dBTMDeviceDataModelList)
        {
            if (IsNull(dBTMDeviceDataModelList))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (dBTMDeviceDataModelList.Count == 0)
                return false;

            try
            {
                _coditechLogging.LogMessage(JsonConvert.SerializeObject(dBTMDeviceDataModelList), "InsertDeviceData", TraceLevel.Warning);
                using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
                {
                    // Cache trainee details to avoid duplicate DB hits
                    var traineeCache = new Dictionary<string, DBTMTraineeDetails>(StringComparer.OrdinalIgnoreCase);

                    var DataUniqueIds = new List<DBTMDeviceDataInsertionUniqueCheck>();
                    List<long> traineeAssignmentUserIds = new List<long>();
                    foreach (var dBTMDeviceDataModel in dBTMDeviceDataModelList)
                    {
                        if (string.Equals(dBTMDeviceDataModel.PersonCode, "DryRun", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                        // Skip if a record with the same DataUniqueId already exists
                        if (_dBTMDeviceDataInsertionUniqueCheckRepository.Table.Any(x => x.DataUniqueId == dBTMDeviceDataModel.DataUniqueId))
                        {
                            continue;
                        }
                        // Get trainee details from cache or DB
                        if (!traineeCache.TryGetValue(dBTMDeviceDataModel.PersonCode, out DBTMTraineeDetails traineeDetails))
                        {
                            traineeDetails = GetDBTMTraineeDetailsByCode(dBTMDeviceDataModel.PersonCode);

                            if (IsNull(traineeDetails))
                                throw new CoditechException(ErrorCodes.InvalidData, $"Invalid Person Code : {dBTMDeviceDataModel.PersonCode}");
                            traineeCache[dBTMDeviceDataModel.PersonCode] = traineeDetails;
                        }
                        DateTime createdDate = DateTime.Now;
                        var dBTMDeviceData = new DBTMDeviceData
                        {
                            TypeOfRecord = dBTMDeviceDataModel.TypeOfRecord,
                            TablePrimaryColumnId = dBTMDeviceDataModel.TablePrimaryColumnId,
                            DeviceSerialCode = dBTMDeviceDataModel.DeviceSerialCode,
                            PersonCode = dBTMDeviceDataModel.PersonCode,
                            TestCode = dBTMDeviceDataModel.TestCode,
                            Comments = dBTMDeviceDataModel.Comments,
                            Height = dBTMDeviceDataModel.Height == 0 ? traineeDetails.Height : dBTMDeviceDataModel.Height,
                            Weight = dBTMDeviceDataModel.Weight == 0 ? traineeDetails.Weight : dBTMDeviceDataModel.Weight,
                            TestPerformedTime = dBTMDeviceDataModel.TestPerformedTime,
                            NumberOfTurn = dBTMDeviceDataModel.NumberOfTurn,
                            CreatedBy = dBTMDeviceDataModel.CreatedBy,
                            CreatedDate = createdDate,
                            IsValidRecord = true,
                            AgeGroupEnumId = traineeDetails.AgeGroupEnumId
                        };

                        // Insert master record
                        DBTMDeviceData insertedDeviceData = _dBTMDeviceDataRepository.Insert(dBTMDeviceData, dBTMDeviceDataModel.CreatedBy);

                        if (insertedDeviceData?.DBTMDeviceDataId <= 0)
                        {
                            throw new CoditechException(ErrorCodes.InvalidData, "Failed to insert device data.");
                        }

                        // Insert detail records
                        if (dBTMDeviceDataModel.DataList != null && dBTMDeviceDataModel.DataList.Count > 0)
                        {
                            var detailList = new List<DBTMDeviceDataDetails>(dBTMDeviceDataModel.DataList.Count);

                            foreach (var item in dBTMDeviceDataModel.DataList)
                            {
                                string paramCodeStr = Convert.ToString(item.ParameterCode);
                                string parameterCode = int.TryParse(paramCodeStr, out int parameterCodeInt) ? ((TestParameterCode)parameterCodeInt).ToString() : paramCodeStr;
                                string parameterValue = item.ParameterValue.ToString() ?? string.Empty;
                                string encryptedValue = DataEncryptionHelper.DataEncrypt(parameterValue);

                                detailList.Add(new DBTMDeviceDataDetails
                                {
                                    DBTMDeviceDataId = insertedDeviceData.DBTMDeviceDataId,
                                    ParameterCode = parameterCode,
                                    ParameterValue = encryptedValue,
                                    IsEncrypted = true,
                                    FromTo = item.FromTo,
                                    Row = item.Row,
                                    Unit = item.Unit,
                                    Comment1 = item.Comment1,
                                    Comment2 = item.Comment2,
                                    Comment3 = item.Comment3,
                                    CreatedBy = dBTMDeviceDataModel.CreatedBy,
                                    CreatedDate = createdDate
                                });
                            }

                            var insertedDetails = _dBTMDeviceDataDetailsRepository.Insert(detailList, dBTMDeviceDataModel.CreatedBy);
                            if (insertedDetails == null)
                            {
                                throw new CoditechException(ErrorCodes.InvalidData, "Failed to insert device data details.");
                            }
                            DataUniqueIds.Add(new DBTMDeviceDataInsertionUniqueCheck
                            {
                                DataUniqueId = dBTMDeviceDataModel.DataUniqueId,
                                ExpiryDate = DateTime.Now.AddHours(ApiCustomSettings.DBTMDeviceDataInsertionUniqueCheckExipiryTimeInHour)
                            });
                            if (dBTMDeviceDataModel.TypeOfRecord == "Assignment")
                            {
                                traineeAssignmentUserIds.Add(dBTMDeviceDataModel.TablePrimaryColumnId);
                            }
                        }
                    }
                    if (DataUniqueIds.Any())
                        _dBTMDeviceDataInsertionUniqueCheckRepository.Insert(DataUniqueIds);
                    if (traineeAssignmentUserIds.Any())
                    {
                        List<DBTMTraineeAssignmentToUser> dBTMTraineeAssignmentToUserList = _dBTMTraineeAssignmentToUserRepository.Table
                            .Where(x => traineeAssignmentUserIds.Contains(x.DBTMTraineeAssignmentUserId))
                            .ToList();
                        foreach (var item in dBTMTraineeAssignmentToUserList)
                        {
                            item.DBTMTestStatusEnumId = 95;
                        }
                        _dBTMTraineeAssignmentToUserRepository.BatchUpdate(dBTMTraineeAssignmentToUserList);
                    }
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "InsertDeviceData failed. Transaction rolled back.");
                return false;
            }
        }

        //Add DBTMDeviceData.
        public bool InsertDeviceDataV2(string rawJson)
        {
            List<DBTMDeviceDataModel> dBTMDeviceDataModelList = JsonConvert.DeserializeObject<List<DBTMDeviceDataModel>>(rawJson);

            if (IsNull(dBTMDeviceDataModelList))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            ValidateEnum(dBTMDeviceDataModelList);


            return InsertDeviceData(dBTMDeviceDataModelList);
        }
        #endregion
        #region DBTMBatch

        public List<DBTMBatchModel> GetBatchList(long entityId, string userType, bool isCheckTestPerformed)
        {
            List<DBTMBatchModel> batcheslist = null;
            var employeeData = _employeeMasterRepository.Table
                .Where(x => x.EmployeeId == entityId)
                .Select(x => new { x.PersonId, x.CentreCode })
                .FirstOrDefault();

            if (employeeData != null)
            {
                string custom1 = _generalPersonRepository.Table.Where(x => x.PersonId == employeeData.PersonId).Select(x => x.Custom1).FirstOrDefault();
                if (custom1 == CustomConstants.DBTMTrainer)
                {
                    var user = _userMasterRepository.Table.Where(x => x.EntityId == entityId && x.UserType == userType).Select(x => new { x.UserMasterId }).FirstOrDefault();
                    if (user != null)
                    {
                        batcheslist = _generalBatchRepository.Table.Where(b => b.CreatedBy == user.UserMasterId && b.IsActive)
                       .Select(b => new DBTMBatchModel
                       {
                           GeneralBatchMasterId = b.GeneralBatchMasterId,
                           BatchName = b.BatchName,
                           BatchStartTime = b.BatchStartTime
                       }).ToList();
                    }
                }
                else if (custom1 == CustomConstants.DBTMCentreOwner)
                {
                    batcheslist = (from b in _generalBatchRepository.Table
                                   join u in _userMasterRepository.Table
                                       on b.CreatedBy equals u.UserMasterId
                                   where b.CentreCode == employeeData.CentreCode && b.IsActive
                                   select new DBTMBatchModel
                                   {
                                       GeneralBatchMasterId = b.GeneralBatchMasterId,
                                       BatchName = u.EntityId == entityId
                                           ? $"{b.BatchName}(Self)"
                                           : u.IsDataEncrypted ? $"{b.BatchName}({DataEncryptionHelper.DataDecrypt(u.FirstName)} {DataEncryptionHelper.DataDecrypt(u.LastName)})" : $"{b.BatchName}({u.FirstName} {u.LastName})",
                                       BatchStartTime = b.BatchStartTime
                                   }).ToList();


                }
                if (batcheslist?.Count > 0)
                {
                    if (isCheckTestPerformed)
                    {
                        var validBatchIds = _dBTMDeviceDataRepository.Table.Where(d => d.TypeOfRecord == "Batch" && d.IsValidRecord).Select(d => d.TablePrimaryColumnId).Distinct().ToList();
                        batcheslist = batcheslist.Where(b => validBatchIds.Contains(b.GeneralBatchMasterId)).ToList();
                    }
                    batcheslist = batcheslist.OrderBy(b => b.BatchName, StringComparer.OrdinalIgnoreCase).ToList();
                }
            }
            else
            {
                batcheslist = new List<DBTMBatchModel>();
            }
            return batcheslist ?? new List<DBTMBatchModel>();
        }

        public DBTMBatchModel GetBatchDetails(int generalBatchMasterId)
        {
            List<int> dbtmTestMasterIds = _dBTMBatchActivityRepository.Table.Where(x => x.GeneralBatchMasterId == generalBatchMasterId).Select(x => x.DBTMTestMasterId).ToList();
            DBTMBatchModel dBTMBatchModel = new DBTMBatchModel()
            {
                GeneralBatchMasterId = generalBatchMasterId,
            };

            if (dbtmTestMasterIds?.Count > 0)
            {
                string centreCode = _generalBatchRepository.Table.Where(x => x.GeneralBatchMasterId == generalBatchMasterId).Select(y => y.CentreCode).FirstOrDefault();
                List<DBTMTestMaster> testDetailList = (from test in _dBTMTestMasterRepository.Table
                                                       join centreTest in _dBTMCentreWiseTestRepository.Table
                                                           on test.DBTMTestMasterId equals centreTest.DBTMTestMasterId
                                                       where dbtmTestMasterIds.Contains(test.DBTMTestMasterId)
                                                             && test.IsActive && centreTest.CentreCode == centreCode
                                                       select test)?.Distinct()?.ToList();

                if (testDetailList?.Count == 0)
                {
                    throw new Exception("The test is not active or does not exist.");
                }
                else
                {
                    dBTMBatchModel.DBTMBatchTestList = new List<DBTMTestApiModel>();
                    foreach (DBTMTestMaster item in testDetailList)
                    {
                        DBTMTestApiModel dbtmTestApiModel = item.FromEntityToModel<DBTMTestApiModel>();
                        dbtmTestApiModel.ActivityCode = item.DBTMTestMasterId;
                        dBTMBatchModel.DBTMBatchTestList.Add(dbtmTestApiModel);
                    }
                }
                PageListModel pageListModel = new PageListModel(null, null, 0, 0);
                CoditechViewRepository<DBTMGeneralBatchUserModel> objStoredProc = new CoditechViewRepository<DBTMGeneralBatchUserModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
                objStoredProc.SetParameter("@GeneralBatchMasterId", generalBatchMasterId, ParameterDirection.Input, DbType.Int32);
                objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
                List<DBTMGeneralBatchUserModel> generalBatchUserList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMGeneralBatchUserListForAPI @GeneralBatchMasterId,@RowsCount OUT", 1, out pageListModel.TotalRowCount)?.ToList();

                foreach (var item in generalBatchUserList?.Where(x => x.IsDataEncrypted))
                {
                    item.FirstName = DataEncryptionHelper.DataDecrypt(item.FirstName);
                    item.LastName = DataEncryptionHelper.DataDecrypt(item.LastName);
                    item.DisplayName = DataEncryptionHelper.DataDecrypt(item.DisplayName);
                    string fullName = $"{item.FirstName} {item.LastName}";
                    string displayName = item.DisplayName == fullName ? string.Empty : item.DisplayName ?? fullName;
                    item.DisplayName = displayName;
                }
                generalBatchUserList = generalBatchUserList.Count > 0 ? generalBatchUserList.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList() : new List<DBTMGeneralBatchUserModel>();

                dBTMBatchModel.DBTMGeneralBatchUserModel = generalBatchUserList;
            }
            return dBTMBatchModel;
        }


        public List<DBTMGeneralBatchUserModel> GetBatchAndActivityWiseUserDetails(int generalBatchMasterId, int dbtmTestMasterId)
        {
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<DBTMGeneralBatchUserModel> objStoredProc = new CoditechViewRepository<DBTMGeneralBatchUserModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@GeneralBatchMasterId", generalBatchMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@DBTMTestMasterId", dbtmTestMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMGeneralBatchUserModel> generalBatchUserList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMGeneralBatchUserListForAPI_V2 @GeneralBatchMasterId,@DBTMTestMasterId,@RowsCount OUT", 2, out pageListModel.TotalRowCount)?.ToList();

            foreach (var item in generalBatchUserList?.Where(x => x.IsDataEncrypted))
            {
                item.FirstName = DataEncryptionHelper.DataDecrypt(item.FirstName);
                item.LastName = DataEncryptionHelper.DataDecrypt(item.LastName);
                item.DisplayName = DataEncryptionHelper.DataDecrypt(item.DisplayName);
                string fullName = $"{item.FirstName} {item.LastName}";
                string displayName = item.DisplayName == fullName ? string.Empty : item.DisplayName ?? fullName;
                item.DisplayName = displayName;
            }
            generalBatchUserList = generalBatchUserList.Count > 0 ? generalBatchUserList.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList() : new List<DBTMGeneralBatchUserModel>();
            return generalBatchUserList;
        }
        #endregion
        #region TrainerDashboard

        //Get Trainer Dashboard Details
        public DBTMMobileDashboardModel GetTrainerDashboard(long userMasterId)
        {
            if (userMasterId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "UserMasterId"));

            DBTMMobileDashboardModel dBTMDashboardModel = new DBTMMobileDashboardModel();
            ExecuteSpHelper objStoredProc = new ExecuteSpHelper(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.GetParameter("@UserId", userMasterId, ParameterDirection.Input, SqlDbType.BigInt);
            DataSet dataset = objStoredProc.GetSPResultInDataSet("Coditech_GetDBTMMobileTrainerDashboard");

            dataset.Tables[0].TableName = "NumberOfTrainersDetails";
            ConvertDataTableToList dataTable = new ConvertDataTableToList();
            dBTMDashboardModel = dataTable.ConvertDataTable<DBTMMobileDashboardModel>(dataset.Tables["NumberOfTrainersDetails"])?.FirstOrDefault();

            dBTMDashboardModel.ActivityCategories = (from a in _dBTMActivityCategoryRepository.Table
                                                     join b in _dBTMTestMasterRepository.Table on a.DBTMActivityCategoryId equals b.DBTMActivityCategoryId
                                                     join c in _dBTMCentreWiseTestRepository.Table on b.DBTMTestMasterId equals c.DBTMTestMasterId
                                                     where a.IsActive && c.CentreCode == dBTMDashboardModel.CentreCode
                                                     select new
                                                     {
                                                         a.DBTMActivityCategoryId,
                                                         a.ActivityCategoryName,
                                                         a.AppIcon
                                                     })
                                                        .Distinct()
                                                        .Select(x => new DBTMMobileActivityCategoryModel
                                                        {
                                                            DBTMActivityCategoryId = x.DBTMActivityCategoryId,
                                                            CategoryName = x.ActivityCategoryName,
                                                            AppIcon = x.AppIcon
                                                        })
                                                        .ToList();
            return dBTMDashboardModel;
        }
        #endregion
        #region TraineeDashboard
        //Get Trainee Dashboard Details
        public DBTMMobileTraineeDashboardModel GetTraineeDashboard(long userMasterId)
        {
            if (userMasterId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "UserMasterId"));

            DBTMMobileTraineeDashboardModel dBTMDashboardModel = new DBTMMobileTraineeDashboardModel();

            ExecuteSpHelper objStoredProc = new ExecuteSpHelper(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.GetParameter("@UserId", userMasterId, ParameterDirection.Input, SqlDbType.BigInt);
            DataSet dataset = objStoredProc.GetSPResultInDataSet("Coditech_GetDBTMMobileTraineeDashboard");

            dataset.Tables[0].TableName = "TraineeDetails";
            ConvertDataTableToList dataTable = new ConvertDataTableToList();
            dBTMDashboardModel = dataTable.ConvertDataTable<DBTMMobileTraineeDashboardModel>(dataset.Tables["TraineeDetails"])?.FirstOrDefault();
            if (!string.IsNullOrEmpty(dBTMDashboardModel.TrainerNames))
            {
                string trainerNames = string.Empty;

                if (!string.IsNullOrWhiteSpace(dBTMDashboardModel.TrainerNames))
                {
                    var trainerList = dBTMDashboardModel.TrainerNames.Split(',', StringSplitOptions.RemoveEmptyEntries);

                    var decryptedNames = new List<string>();

                    foreach (string item in trainerList)
                    {
                        string[] name = item.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                        if (name.Length == 0)
                            continue;

                        string firstName = name[0];
                        string lastName = name.Length > 1 ? string.Join(" ", name.Skip(1)) : string.Empty;

                        // Decrypt only if the names are encrypted.
                        firstName = DataEncryptionHelper.DataDecrypt(firstName);

                        if (!string.IsNullOrWhiteSpace(lastName))
                        {
                            lastName = DataEncryptionHelper.DataDecrypt(lastName);
                        }

                        decryptedNames.Add($"{firstName} {lastName}".Trim());
                    }

                    trainerNames = string.Join(", ", decryptedNames);
                    dBTMDashboardModel.TrainerNames = trainerNames;
                }
            }
            return dBTMDashboardModel;
        }
        #endregion
        public DBTMTraineeDetailsListModel GetTraineesByPerformedActivity(string dBTMTestMasterIds, string centreCode, long generalTrainerMasterId)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<DBTMTraineeDetailsModel> objStoredProc = new CoditechViewRepository<DBTMTraineeDetailsModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@CentreCode", centreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@DBTMTestMasterIds", dBTMTestMasterIds, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@GeneralTrainerMasterId", generalTrainerMasterId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMTraineeDetailsModel> dBTMTraineeDetailsList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMTraineeListByActivityIds @CentreCode,@DBTMTestMasterIds,@GeneralTrainerMasterId,@RowsCount OUT", 3, out pageListModel.TotalRowCount)?.ToList();
            foreach (var item in dBTMTraineeDetailsList?.Where(x => x.IsDataEncrypted))
            {
                item.FirstName = DataEncryptionHelper.DataDecrypt(item.FirstName);
                item.LastName = DataEncryptionHelper.DataDecrypt(item.LastName);
            }

            DBTMTraineeDetailsListModel listModel = new DBTMTraineeDetailsListModel();
            listModel.DBTMTraineeDetailsList = dBTMTraineeDetailsList?.Count > 0 ? dBTMTraineeDetailsList : new List<DBTMTraineeDetailsModel>();
            return listModel;
        }
        public DBTMTestListModel GetactivitiesBytrainee(long selectedTraineeId)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<DBTMTestModel> objStoredProc = new CoditechViewRepository<DBTMTestModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@DBTMTraineeDetailId", selectedTraineeId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMTestModel> dBTMTestList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetActivityListByTraineeDetailId @DBTMTraineeDetailId,@RowsCount OUT", 1, out pageListModel.TotalRowCount)?.ToList();
            DBTMTestListModel listModel = new DBTMTestListModel();

            listModel.DBTMTestList = dBTMTestList?.Count > 0 ? dBTMTestList : new List<DBTMTestModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }
        public OrganisationCentrewiseJoiningCodeModel GetJoiningCode(string generalTrainerMasterId)
        {
            List<OrganisationCentrewiseJoiningCode> list = _organisationCentrewiseJoiningCodeRepository.Table.Where(x => x.Custom1 == generalTrainerMasterId && x.QueueValidTill != null && x.QueueValidTill <= DateTime.Now).ToList();
            if (list?.Count > 0)
            {
                list.ForEach(x => { x.QueueValidTill = null; });
                _organisationCentrewiseJoiningCodeRepository.BatchUpdate(list);
            }
            OrganisationCentrewiseJoiningCode organisationCentrewiseJoiningCode = _organisationCentrewiseJoiningCodeRepository.Table.Where(x => x.Custom1 == generalTrainerMasterId && !x.IsExpired && x.QueueValidTill == null).FirstOrDefault();
            OrganisationCentrewiseJoiningCodeModel organisationCentrewiseJoiningCodeModel = organisationCentrewiseJoiningCode.FromEntityToModel<OrganisationCentrewiseJoiningCodeModel>();
            if (organisationCentrewiseJoiningCodeModel != null)
            {
                organisationCentrewiseJoiningCode.QueueValidTill = DateTime.Now.AddMinutes(ApiCustomSettings.JoiningCodeQueueTimeInMinutes);
                _organisationCentrewiseJoiningCodeRepository.Update(organisationCentrewiseJoiningCode);
            }
            return organisationCentrewiseJoiningCodeModel ?? new OrganisationCentrewiseJoiningCodeModel { JoiningCode = string.Empty, Custom3 = string.Empty };
        }
        public string GetCentreWiseJoiningCode(string centreCode, int joiningCodeTypeEnumId)
        {
            string joiningCode = _organisationCentrewiseJoiningCodeRepository.Table.Where(x => x.CentreCode == centreCode && x.JoiningCodeTypeEnumId == joiningCodeTypeEnumId && !x.IsExpired).Select(x => x.JoiningCode).FirstOrDefault();
            return joiningCode ?? string.Empty;
        }

        private DBTMTraineeDetails GetDBTMTraineeDetailsByCode(string personCode) => _dBTMTraineeDetailsRepository.Table.Where(x => x.PersonCode == personCode).FirstOrDefault();

        #region DBTMCamp
        public List<DBTMBatchModel> GetCampList(long entityId, string userType)
        {
            List<DBTMBatchModel> batcheslist = null;
            var employeeData = _employeeMasterRepository.Table
                .Where(x => x.EmployeeId == entityId)
                .Select(x => new { x.PersonId, x.CentreCode })
                .FirstOrDefault();

            if (employeeData != null)
            {
                string custom1 = _generalPersonRepository.Table
                    .Where(x => x.PersonId == employeeData.PersonId)
                    .Select(x => x.Custom1)
                    .FirstOrDefault();

                if (custom1 == CustomConstants.DBTMTrainer)
                {
                    var user = _userMasterRepository.Table
                        .Where(x => x.EntityId == entityId && x.UserType == userType)
                        .Select(x => new { x.UserMasterId })
                        .FirstOrDefault();

                    if (user != null)
                    {
                        batcheslist = _dBTMCampMasterRepository.Table
                            .Where(x => x.CreatedBy == user.UserMasterId && x.IsActive)
                            .Select(b => new DBTMBatchModel
                            {
                                DBTMCampMasterId = b.DBTMCampMasterId,
                                CampName = b.CampName,
                            })
                             .ToList().OrderBy(b => b.BatchName, StringComparer.OrdinalIgnoreCase).ToList();
                    }
                    else
                    {
                        batcheslist = new List<DBTMBatchModel>();
                    }
                }
                else if (custom1 == CustomConstants.DBTMCentreOwner)
                {
                    var camps = _dBTMCampMasterRepository.Table.Where(b => b.CentreCode == employeeData.CentreCode && b.IsActive).ToList();
                    var users = _userMasterRepository.Table.ToList();
                    batcheslist = (from b in camps
                                   join u in users on b.CreatedBy equals u.UserMasterId
                                   select new DBTMBatchModel
                                   {
                                       DBTMCampMasterId = b.DBTMCampMasterId,
                                       CampName = u.EntityId == entityId
                                                  ? $"{b.CampName}(Self)"
                                                  : u.IsDataEncrypted ? $"{b.CampName}({DataEncryptionHelper.DataDecrypt(u.FirstName)} {DataEncryptionHelper.DataDecrypt(u.LastName)})" : $"{b.CampName}({u.FirstName} {u.LastName})",

                                   })
                                   .OrderBy(b => b.CampName, StringComparer.OrdinalIgnoreCase).ToList();
                }
                else
                {
                    batcheslist = new List<DBTMBatchModel>();
                }
            }
            else
            {
                batcheslist = new List<DBTMBatchModel>();
            }
            return batcheslist ?? new List<DBTMBatchModel>();
        }
        public DBTMBatchModel GetCampDetails(int dBTMCampMasterId)
        {
            List<int> dbtmTestMasterIds = _dBTMCampActivityRepository.Table.Where(x => x.DBTMCampMasterId == dBTMCampMasterId).Select(x => x.DBTMTestMasterId).ToList();
            DBTMBatchModel dBTMBatchModel = new DBTMBatchModel()
            {
                DBTMCampMasterId = dBTMCampMasterId,
            };

            if (dbtmTestMasterIds?.Count > 0)
            {
                string centreCode = _dBTMCampMasterRepository.Table.Where(x => x.DBTMCampMasterId == dBTMCampMasterId).Select(y => y.CentreCode).FirstOrDefault();
                List<DBTMTestMaster> testDetailList = (from test in _dBTMTestMasterRepository.Table
                                                       join centreTest in _dBTMCentreWiseTestRepository.Table
                                                           on test.DBTMTestMasterId equals centreTest.DBTMTestMasterId
                                                       where dbtmTestMasterIds.Contains(test.DBTMTestMasterId)
                                                             && test.IsActive && centreTest.CentreCode == centreCode
                                                       select test)?.Distinct()?.ToList();

                if (testDetailList?.Count == 0)
                {
                    throw new Exception("The test is not active or does not exist.");
                }
                else
                {
                    dBTMBatchModel.DBTMBatchTestList = new List<DBTMTestApiModel>();
                    foreach (DBTMTestMaster item in testDetailList)
                    {
                        DBTMTestApiModel dbtmTestApiModel = item.FromEntityToModel<DBTMTestApiModel>();
                        dbtmTestApiModel.ActivityCode = item.DBTMTestMasterId;
                        dBTMBatchModel.DBTMBatchTestList.Add(dbtmTestApiModel);
                    }
                }
            }
            return dBTMBatchModel;
        }

        public List<DBTMGeneralBatchUserModel> GetCampAndActivityWiseUserDetails(int dBTMcampMasterId, int dbtmTestMasterId, string userType)
        {
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<DBTMGeneralBatchUserModel> objStoredProc = new CoditechViewRepository<DBTMGeneralBatchUserModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@DBTMcampMasterId", dBTMcampMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@DBTMTestMasterId", dbtmTestMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@UserType", userType, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMGeneralBatchUserModel> campUserList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMCampUserListForAPI @DBTMcampMasterId,@DBTMTestMasterId,@UserType,@RowsCount OUT", 3, out pageListModel.TotalRowCount)?.ToList();
            foreach (var item in campUserList?.Where(x => x.IsDataEncrypted))
            {
                item.FirstName = DataEncryptionHelper.DataDecrypt(item.FirstName);
                item.LastName = DataEncryptionHelper.DataDecrypt(item.LastName);
                item.DisplayName = DataEncryptionHelper.DataDecrypt(item.DisplayName);
                string displayName = string.IsNullOrEmpty(item.DisplayName) ? $"{item.FirstName} {item.LastName}"
                                                                               : item.DisplayName == $"{item.FirstName} {item.LastName}"
                                                                            ? $"{item.FirstName} {item.LastName}"
                                                                               : item.DisplayName;
                item.DisplayName = displayName;
            }
            campUserList = campUserList.Count > 0 ? campUserList.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList() : new List<DBTMGeneralBatchUserModel>();

            return campUserList;
        }
        #endregion
        public bool UpdateValidRecord(long dBTMDeviceDataId, bool isValidRecord)
        {
            if (dBTMDeviceDataId < 1)
                throw new CoditechException(
                    ErrorCodes.IdLessThanOne,
                    string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMDeviceDataId")
                );

            DBTMDeviceData deviceData = _dBTMDeviceDataRepository.Table.Where(x => x.DBTMDeviceDataId == dBTMDeviceDataId).FirstOrDefault();
            if (deviceData == null)
                throw new CoditechException(ErrorCodes.NotFound, "Record not found");

            // Update field
            deviceData.IsValidRecord = isValidRecord;

            bool isUpdated = _dBTMDeviceDataRepository.Update(deviceData);

            return isUpdated;
        }
        public DBTMBatchListModel GetDBTMCentrAndTrainerewiseBatchList(string centreCode, int joiningCodeTypeEnumId, long generalTrainerMasterId)
        {
            var validBatchIds = _dBTMDeviceDataRepository.Table
                                .Where(x => x.TypeOfRecord == "Batch" && x.IsValidRecord)
                                .Select(x => x.TablePrimaryColumnId)
                                .Distinct()
                                .ToList();
            var batches = (from a in _generalBatchRepository.Table
                           join b in _userMasterRepository.Table
                               on a.CreatedBy equals b.UserMasterId
                           join c in _employeeMasterRepository.Table
                               on b.EntityId equals c.EmployeeId
                           join d in _generalTrainerMasterRepository.Table
                               on c.EmployeeId equals d.EmployeeId
                           where a.IsActive
                                 && b.IsActive
                                 && b.UserType == "Employee"
                                 && a.CentreCode == centreCode
                                 && d.GeneralTrainerMasterId == generalTrainerMasterId
                           select new DBTMBatchModel
                           {
                               GeneralBatchMasterId = a.GeneralBatchMasterId,
                               BatchName = a.BatchName
                           })
                           .OrderBy(x => x.BatchName)
                           .ToList();

            return new DBTMBatchListModel
            {
                DBTMBatchList = batches ?? new List<DBTMBatchModel>()
            };
        }

        public DBTMBatchListModel GetDBTMTrainerwiseBatchList(string centreCode, long generalTrainerMasterId)
        {
            var validBatchIds = _dBTMDeviceDataRepository.Table
                                .Where(x => x.TypeOfRecord == "Batch" && x.IsValidRecord)
                                .Select(x => x.TablePrimaryColumnId)
                                .Distinct()
                                .ToList();
            var batches = (from a in _generalBatchRepository.Table
                           join b in _userMasterRepository.Table
                               on a.CreatedBy equals b.UserMasterId
                           join c in _employeeMasterRepository.Table
                               on b.EntityId equals c.EmployeeId
                           join d in _generalTrainerMasterRepository.Table
                               on c.EmployeeId equals d.EmployeeId
                           where a.IsActive
                                 && b.IsActive
                                 && b.UserType == "Employee"
                                 && a.CentreCode == centreCode
                                 && d.GeneralTrainerMasterId == generalTrainerMasterId
                                 && validBatchIds.Contains(a.GeneralBatchMasterId)
                           select new DBTMBatchModel
                           {
                               GeneralBatchMasterId = a.GeneralBatchMasterId,
                               BatchName = a.BatchName
                           })
                           .OrderBy(x => x.BatchName)
                           .ToList();

            return new DBTMBatchListModel
            {
                DBTMBatchList = batches ?? new List<DBTMBatchModel>()
            };
        }

        private void ValidateEnum(List<DBTMDeviceDataModel> dBTMDeviceDataModelList)
        {
            foreach (var item in dBTMDeviceDataModelList)
            {
                List<string> parameterCodes = item.DataList
                                                  .Select(x => x.ParameterCode)
                                                  .Distinct()
                                                  .ToList();

                foreach (string code in parameterCodes)
                {
                    if (!int.TryParse(code, out int enumValue) ||
                        !Enum.IsDefined(typeof(TestParameterCode), enumValue))
                    {
                        throw new CoditechException(
                            0,
                            $"DBTM Insert Device Data Invalid Parameter Code: {code}"
                        );
                    }
                }
            }
        }

        public DBTMTraineeDetailsModel GetDBTMTraineeDetails(long dBTMTraineeDetailId)
        {
            if (dBTMTraineeDetailId <= 0) throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTraineeDetailId"));

            DBTMTraineeDetails dBTMTraineeDetails = _dBTMTraineeDetailsRepository.Table.FirstOrDefault(x => x.DBTMTraineeDetailId == dBTMTraineeDetailId);
            if (IsNull(dBTMTraineeDetails))
                return null;

            DBTMTraineeDetailsModel dBTMTraineeDetailsModel = dBTMTraineeDetails?.FromEntityToModel<DBTMTraineeDetailsModel>();

            GeneralPerson personData = _generalPersonRepository.Table.FirstOrDefault(x => x.PersonId == dBTMTraineeDetails.PersonId);

            if (personData != null)
            {
                dBTMTraineeDetailsModel.FirstName = personData.IsDataEncrypted ? DataEncryptionHelper.DataDecrypt(personData.FirstName) : personData.FirstName;
                dBTMTraineeDetailsModel.MiddleName = personData.IsDataEncrypted ? DataEncryptionHelper.DataDecrypt(personData.MiddleName) : personData.MiddleName;
                dBTMTraineeDetailsModel.LastName = personData.IsDataEncrypted ? DataEncryptionHelper.DataDecrypt(personData.LastName) : personData.LastName;
                dBTMTraineeDetailsModel.DisplayName = personData.IsDataEncrypted ? DataEncryptionHelper.DataDecrypt(personData.Custom2) : personData.Custom2;
                dBTMTraineeDetailsModel.EmailId = personData.IsDataEncrypted ? DataEncryptionHelper.DataDecrypt(personData.EmailId) : personData.EmailId;
                dBTMTraineeDetailsModel.MobileNumber = personData.IsDataEncrypted ? DataEncryptionHelper.DataDecrypt(personData.MobileNumber) : personData.MobileNumber;
                dBTMTraineeDetailsModel.DateOfBirth = string.IsNullOrEmpty(personData.DateOfBirth) ? null : Convert.ToDateTime(personData.IsDataEncrypted ? DataEncryptionHelper.DataDecrypt(personData.DateOfBirth) : personData.DateOfBirth);
                dBTMTraineeDetailsModel.PersonTitle = personData.PersonTitle;
            }
            return dBTMTraineeDetailsModel;
        }

        //Update DBTM Trainee Other Details
        public bool UpdateDBTMTraineeDetails(DBTMTraineeDetailsModel dBTMTraineeDetailsModel)
        {
            if (dBTMTraineeDetailsModel == null)
                throw new CoditechException(ErrorCodes.NullModel, "Model is null.");

            // Update DBTMTraineeDetails
            DBTMTraineeDetails traineeDetails = _dBTMTraineeDetailsRepository.Table
                .FirstOrDefault(x => x.DBTMTraineeDetailId == dBTMTraineeDetailsModel.DBTMTraineeDetailId);

            if (traineeDetails == null)
                throw new CoditechException(ErrorCodes.NotFound, "Trainee details not found.");

            traineeDetails.Height = dBTMTraineeDetailsModel.Height;
            traineeDetails.Weight = dBTMTraineeDetailsModel.Weight;
            traineeDetails.SpecializationEnumId = dBTMTraineeDetailsModel.SpecializationEnumId;
            traineeDetails.IsActive = dBTMTraineeDetailsModel.IsActive;

            _dBTMTraineeDetailsRepository.Update(traineeDetails);

            // Update GeneralPerson
            GeneralPerson person = _generalPersonRepository.Table.FirstOrDefault(x => x.PersonId == traineeDetails.PersonId);

            if (person != null)
            {
                person.FirstName = person.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt(dBTMTraineeDetailsModel.FirstName) : dBTMTraineeDetailsModel.FirstName;
                person.MiddleName = person.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt(dBTMTraineeDetailsModel.MiddleName) : dBTMTraineeDetailsModel.MiddleName;
                person.LastName = person.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt(dBTMTraineeDetailsModel.LastName) : dBTMTraineeDetailsModel.LastName;
                person.EmailId = person.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt(dBTMTraineeDetailsModel.EmailId) : dBTMTraineeDetailsModel.EmailId;
                person.MobileNumber = person.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt(dBTMTraineeDetailsModel.MobileNumber) : dBTMTraineeDetailsModel.MobileNumber;
                person.DateOfBirth = person.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt(dBTMTraineeDetailsModel.DateOfBirth.ToString()) : dBTMTraineeDetailsModel.DateOfBirth.ToString();
                person.PersonTitle = dBTMTraineeDetailsModel.PersonTitle;
                person.CustomSearchTokens2 = SearchHashHelper.CreateSearchTokens(dBTMTraineeDetailsModel.DisplayName);
                person.Custom2 = person.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt(dBTMTraineeDetailsModel.DisplayName) : dBTMTraineeDetailsModel.DisplayName;
                _generalPersonRepository.Update(person);
            }

            UserMaster userMaster = _userMasterRepository.Table
                .FirstOrDefault(x => x.EntityId == traineeDetails.DBTMTraineeDetailId
                                  && x.UserType == UserTypeEnum.Trainee.ToString());

            if (userMaster != null)
            {
                userMaster.IsActive = dBTMTraineeDetailsModel.IsActive;
                userMaster.FirstName = person.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt(dBTMTraineeDetailsModel.FirstName) : dBTMTraineeDetailsModel.FirstName;
                userMaster.MiddleName = person.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt(dBTMTraineeDetailsModel.MiddleName) : dBTMTraineeDetailsModel.MiddleName;
                userMaster.LastName = person.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt(dBTMTraineeDetailsModel.LastName) : dBTMTraineeDetailsModel.LastName;
                userMaster.EmailId = person.IsDataEncrypted ? DataEncryptionHelper.DataEncrypt(dBTMTraineeDetailsModel.EmailId) : dBTMTraineeDetailsModel.EmailId;
                _userMasterRepository.Update(userMaster);
            }
            return true;
        }
        #region Assignment
        public DBTMBatchModel GetTestListForAssignmentWiseTestingCreatedByTrainer(long generalTrainerMasterId, DateTime assignmentDate, string centreCode)
        {
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<DBTMTestApiModel> objStoredProc = new CoditechViewRepository<DBTMTestApiModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@GeneralTrainerMasterId", generalTrainerMasterId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@AssignmentDate", assignmentDate, ParameterDirection.Input, DbType.DateTime);
            objStoredProc.SetParameter("@CentreCode", centreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMTestApiModel> testList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetTestListForAssignmentWiseTestingCreatedByTrainer @GeneralTrainerMasterId,@AssignmentDate,@CentreCode,@RowsCount OUT", 3, out pageListModel.TotalRowCount)?.ToList();
            testList = testList ?? new List<DBTMTestApiModel>();
            return new DBTMBatchModel
            {
                DBTMBatchTestList = testList
            };
        }
        public List<DBTMGeneralBatchUserModel> GetUserDetailsForAssignmentWiseTesting(long generalTrainerMasterId, int dbtmTestMasterId, DateTime assignmentDate)
        {
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<DBTMGeneralBatchUserModel> objStoredProc = new CoditechViewRepository<DBTMGeneralBatchUserModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@GeneralTrainerMasterId", generalTrainerMasterId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@DBTMTestMasterId", dbtmTestMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@AssignmentDate", assignmentDate, ParameterDirection.Input, DbType.DateTime);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMGeneralBatchUserModel> assignmentUserList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMUserDetailsForAssignmentWiseTesting @GeneralTrainerMasterId,@DBTMTestMasterId,@AssignmentDate,@RowsCount OUT", 3, out pageListModel.TotalRowCount)?.ToList();
            foreach (var item in assignmentUserList?.Where(x => x.IsDataEncrypted))
            {
                item.FirstName = DataEncryptionHelper.DataDecrypt(item.FirstName);
                item.LastName = DataEncryptionHelper.DataDecrypt(item.LastName);
                item.DisplayName = DataEncryptionHelper.DataDecrypt(item.DisplayName);
                string displayName = string.IsNullOrEmpty(item.DisplayName) ? $"{item.FirstName} {item.LastName}"
                                                                               : item.DisplayName == $"{item.FirstName} {item.LastName}"
                                                                            ? $"{item.FirstName} {item.LastName}"
                                                                               : item.DisplayName;
                item.DisplayName = displayName;
            }
            assignmentUserList?.OrderBy(x => x.FirstName).ThenBy(x => x.LastName)?.ToList();
            assignmentUserList = assignmentUserList ?? new List<DBTMGeneralBatchUserModel>();
            return assignmentUserList;
        }
        public DBTMTraineeDetailsListModel GetTraineeListForAssignmentWiseTesting(long generalTrainerMasterId, string dBTMTestMasterId, string centreCode, DateTime assignmentDate)
        {
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<DBTMTraineeDetailsModel> objStoredProc = new CoditechViewRepository<DBTMTraineeDetailsModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@GeneralTrainerMasterId", generalTrainerMasterId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@DBTMTestMasterId", dBTMTestMasterId, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@CentreCode", centreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@AssignmentDate", assignmentDate, ParameterDirection.Input, DbType.DateTime);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMTraineeDetailsModel> traineeList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMTraineeListForAssignmentWiseTesting @GeneralTrainerMasterId,@DBTMTestMasterId,@CentreCode,@AssignmentDate,@RowsCount OUT", 4, out pageListModel.TotalRowCount)?.ToList();
            foreach (var item in traineeList?.Where(x => x.IsDataEncrypted))
            {
                item.FirstName = DataEncryptionHelper.DataDecrypt(item.FirstName);
                item.LastName = DataEncryptionHelper.DataDecrypt(item.LastName);
            }
            traineeList?.OrderBy(x => x.FirstName).ThenBy(x => x.LastName)?.ToList();
            traineeList = traineeList ?? new List<DBTMTraineeDetailsModel>();
            return new DBTMTraineeDetailsListModel
            {
                DBTMTraineeDetailsList = traineeList
            };
        }

        #endregion
        protected virtual int CalculateAge(DateTime dateOfBirth)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;

            // Go back to the year the person was born in case of a leap year
            if (dateOfBirth > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        public async Task<bool> BulkDataEncrypt()
        {
            List<GeneralPerson> genaralPersonList = _generalPersonRepository.Table.Where(x => !x.IsDataEncrypted)?.ToList();
            foreach (GeneralPerson generalPersonModel in genaralPersonList)
            {
                generalPersonModel.FirstNameSearchTokens = SearchHashHelper.CreateSearchTokens(generalPersonModel.FirstName);
                generalPersonModel.MiddleNameSearchTokens = SearchHashHelper.CreateSearchTokens(generalPersonModel.MiddleName);
                generalPersonModel.LastNameSearchTokens = SearchHashHelper.CreateSearchTokens(generalPersonModel.LastName);
                generalPersonModel.DateOfBirthSearchTokens = SearchHashHelper.CreateSearchTokens(generalPersonModel.DateOfBirth);
                generalPersonModel.EmailIdSearchTokens = SearchHashHelper.CreateSearchTokens(generalPersonModel.EmailId);
                generalPersonModel.PhoneNumberSearchTokens = SearchHashHelper.CreateSearchTokens(generalPersonModel.PhoneNumber);
                generalPersonModel.MobileNumberSearchTokens = SearchHashHelper.CreateSearchTokens(generalPersonModel.MobileNumber);
                generalPersonModel.EmergencyContactSearchTokens = SearchHashHelper.CreateSearchTokens(generalPersonModel.EmergencyContact);
                generalPersonModel.IndentificationNumberSearchTokens = SearchHashHelper.CreateSearchTokens(generalPersonModel.IndentificationNumber);
                generalPersonModel.AttendanceIntegrationIdSearchTokens = SearchHashHelper.CreateSearchTokens(generalPersonModel.AttendanceIntegrationId);
                generalPersonModel.CustomSearchTokens2 = SearchHashHelper.CreateSearchTokens(generalPersonModel.Custom2);

                // Personal Details
                generalPersonModel.FirstName = DataEncryptionHelper.DataEncrypt(generalPersonModel.FirstName);
                generalPersonModel.MiddleName = DataEncryptionHelper.DataEncrypt(generalPersonModel.MiddleName);
                generalPersonModel.LastName = DataEncryptionHelper.DataEncrypt(generalPersonModel.LastName);
                generalPersonModel.EmailId = DataEncryptionHelper.DataEncrypt(generalPersonModel.EmailId);
                generalPersonModel.PhoneNumber = DataEncryptionHelper.DataEncrypt(generalPersonModel.PhoneNumber);
                generalPersonModel.MobileNumber = DataEncryptionHelper.DataEncrypt(generalPersonModel.MobileNumber);
                generalPersonModel.EmergencyContact = DataEncryptionHelper.DataEncrypt(generalPersonModel.EmergencyContact);
                generalPersonModel.IndentificationNumber = DataEncryptionHelper.DataEncrypt(generalPersonModel.IndentificationNumber);
                generalPersonModel.AttendanceIntegrationId = DataEncryptionHelper.DataEncrypt(generalPersonModel.AttendanceIntegrationId);
                generalPersonModel.IsMinor = HelperUtility.IsMinor(generalPersonModel.DateOfBirth);
                generalPersonModel.DateOfBirth = DataEncryptionHelper.DataEncrypt(generalPersonModel.DateOfBirth);
                generalPersonModel.Custom2 = DataEncryptionHelper.DataEncrypt(generalPersonModel.Custom2);
                generalPersonModel.IsDataEncrypted = true;
            }
            if (genaralPersonList.Count > 0)
                _generalPersonRepository.BatchUpdate(genaralPersonList);

            List<UserMaster> userMasters = _userMasterRepository.Table.Where(x => !x.IsDataEncrypted)?.ToList();
            foreach (var userMaster in userMasters)
            {
                userMaster.UserNameHash = SearchHashHelper.CreateSearchHash(userMaster.UserName);

                userMaster.UserName = DataEncryptionHelper.DataEncrypt(userMaster.UserName);
                userMaster.EmailId = DataEncryptionHelper.DataEncrypt(userMaster.EmailId);
                userMaster.FirstName = DataEncryptionHelper.DataEncrypt(userMaster.FirstName);
                userMaster.MiddleName = DataEncryptionHelper.DataEncrypt(userMaster.MiddleName);
                userMaster.LastName = DataEncryptionHelper.DataEncrypt(userMaster.LastName);
                userMaster.IsDataEncrypted = true;
            }
            if (userMasters.Count > 0)
                _userMasterRepository.BatchUpdate(userMasters);

            List<EmployeeMaster> employeeMasters = _employeeMasterRepository.Table.Where(x => !x.IsDataEncrypted)?.ToList();
            foreach (var employee in employeeMasters)
            {
                employee.PANCardNumber = DataEncryptionHelper.DataEncrypt(employee.PANCardNumber);
                employee.UANNumber = DataEncryptionHelper.DataEncrypt(employee.UANNumber);
                employee.PassportNumber = DataEncryptionHelper.DataEncrypt(employee.PassportNumber);
                employee.AdharCardNumber = DataEncryptionHelper.DataEncrypt(employee.AdharCardNumber);
                employee.BankAccountNumber = DataEncryptionHelper.DataEncrypt(employee.BankAccountNumber);
                employee.BankName = DataEncryptionHelper.DataEncrypt(employee.BankName);
                employee.BankIFSCCode = DataEncryptionHelper.DataEncrypt(employee.BankIFSCCode);
                employee.IsDataEncrypted = true;
            }
            if (employeeMasters.Count > 0)
                _employeeMasterRepository.BatchUpdate(employeeMasters);

            List<OrganisationCentreMaster> organisationCentreMasters = new CoditechRepository<OrganisationCentreMaster>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => !x.IsDataEncrypted)?.ToList();
            foreach (var organisationCentreMaster in organisationCentreMasters)
            {
                organisationCentreMaster.CentreAddress = DataEncryptionHelper.DataEncrypt(organisationCentreMaster.CentreAddress);
                organisationCentreMaster.Pincode = DataEncryptionHelper.DataEncrypt(organisationCentreMaster.Pincode);
                organisationCentreMaster.EmailId = DataEncryptionHelper.DataEncrypt(organisationCentreMaster.EmailId);
                organisationCentreMaster.CellPhone = DataEncryptionHelper.DataEncrypt(organisationCentreMaster.CellPhone);
                organisationCentreMaster.FaxNumber = DataEncryptionHelper.DataEncrypt(organisationCentreMaster.FaxNumber);
                organisationCentreMaster.PhoneNumberOffice = DataEncryptionHelper.DataEncrypt(organisationCentreMaster.PhoneNumberOffice);
                organisationCentreMaster.CINNumber = DataEncryptionHelper.DataEncrypt(organisationCentreMaster.CINNumber);
                organisationCentreMaster.GSTINNumber = DataEncryptionHelper.DataEncrypt(organisationCentreMaster.GSTINNumber);
                organisationCentreMaster.PanNumber = DataEncryptionHelper.DataEncrypt(organisationCentreMaster.PanNumber);
                organisationCentreMaster.PFNumber = DataEncryptionHelper.DataEncrypt(organisationCentreMaster.PFNumber);
                organisationCentreMaster.ESICNumber = DataEncryptionHelper.DataEncrypt(organisationCentreMaster.ESICNumber);
                organisationCentreMaster.IsDataEncrypted = true;
            }

            int updatedCount = await EncryptDeviceDataDetailsAsync(
                CancellationToken.None);

            return true;
        }


        public async Task<int> EncryptDeviceDataDetailsAsync(
            CancellationToken cancellationToken = default)
        {
            const int batchSize = 1000;

            int totalProcessed = 0;
            long lastId = 0;

            await using var connection = new SqlConnection("connection string");
            await connection.OpenAsync(cancellationToken);

            // Create a temporary staging table once per execution.
            const string createTempTableSql = @"
        CREATE TABLE #EncryptedBatch
        (
            DBTMDeviceDataDetailId INT NOT NULL PRIMARY KEY,
            ParameterValue NVARCHAR(MAX) NULL
        );";

            await using (var createCommand =
                new SqlCommand(createTempTableSql, connection))
            {
                await createCommand.ExecuteNonQueryAsync(cancellationToken);
            }

            while (true)
            {
                // Fetch only the next batch; do not load all 75k rows.
                var batch = await _dBTMDeviceDataDetailsRepository.Table
                    .AsNoTracking()
                    .Where(x =>
                        x.DBTMDeviceDataDetailId > lastId &&
                        (x.Comment3 == null || x.Comment3 != "1"))
                    .OrderBy(x => x.DBTMDeviceDataDetailId)
                    .Take(batchSize)
                    .Select(x => new
                    {
                        x.DBTMDeviceDataDetailId,
                        x.ParameterValue,
                        x.IsEncrypted
                    })
                    .ToListAsync(cancellationToken);

                if (batch.Count == 0)
                    break;

                lastId = batch[batch.Count - 1].DBTMDeviceDataDetailId;

                var table = new DataTable();

                table.Columns.Add(
                    "DBTMDeviceDataDetailId", typeof(int));

                table.Columns.Add(
                    "ParameterValue", typeof(string));

                foreach (var item in batch)
                {
                    try
                    {
                        string? value = item.ParameterValue;

                        if (!string.IsNullOrEmpty(value))
                        {
                            // Decrypt first if the existing value is encrypted.
                            if (item.IsEncrypted)
                            {
                                value = EncryptionHelper.Decrypt(value);
                            }

                            // Encrypt the original plaintext exactly once.
                            value = DataEncryptionHelper.DataEncrypt(value);
                        }

                        table.Rows.Add(
                            item.DBTMDeviceDataDetailId,
                            (object?)value ?? DBNull.Value);
                    }
                    catch (Exception ex)
                    {
                        // Replace with your application's ILogger.
                        Console.Error.WriteLine(
                            $"Encryption failed for ID " +
                            $"{item.DBTMDeviceDataDetailId}: {ex.Message}");
                    }
                }

                if (table.Rows.Count == 0)
                    continue;

                // Clear staging data from the previous batch.
                await using (var truncateCommand =
                    new SqlCommand(
                        "TRUNCATE TABLE #EncryptedBatch;",
                        connection))
                {
                    await truncateCommand.ExecuteNonQueryAsync(
                        cancellationToken);
                }

                // Bulk insert encrypted values into SQL Server.
                using (var bulkCopy = new SqlBulkCopy(
                    connection,
                    SqlBulkCopyOptions.TableLock,
                    null))
                {
                    bulkCopy.DestinationTableName = "#EncryptedBatch";
                    bulkCopy.BatchSize = batchSize;
                    bulkCopy.BulkCopyTimeout = 120;

                    bulkCopy.ColumnMappings.Add(
                        "DBTMDeviceDataDetailId",
                        "DBTMDeviceDataDetailId");

                    bulkCopy.ColumnMappings.Add(
                        "ParameterValue",
                        "ParameterValue");

                    await bulkCopy.WriteToServerAsync(
                        table, cancellationToken);
                }

                // Update all records in this batch with one SQL statement.
                const string updateSql = @"
            UPDATE target
            SET target.ParameterValue = source.ParameterValue,
                target.IsEncrypted = 1,
                target.Comment3 = '1'
            FROM dbo.dBTMDeviceDataDetails AS target
            INNER JOIN #EncryptedBatch AS source
                ON source.DBTMDeviceDataDetailId =
                   target.DBTMDeviceDataDetailId
            WHERE target.Comment3 IS NULL
               OR target.Comment3 <> '1';";

                await using (var updateCommand =
                    new SqlCommand(updateSql, connection))
                {
                    updateCommand.CommandTimeout = 120;
                    try
                    {
                        totalProcessed += await updateCommand.ExecuteNonQueryAsync(
                            cancellationToken);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            return totalProcessed;
        }
    }

    public enum TestParameterCode
    {
        AirTime = 1,
        Count = 2,
        Direction = 3,
        Distance = 4,
        DistanceMultiplyByRow = 5,
        JumpHeight = 6,
        JumpLength = 7,
        ModeOfStart = 8,
        PersonDetectionRange = 9,
        Position = 10,
        Round = 11,
        ShuttleNo = 12,
        Speed = 13,
        SpeedLevel = 14,
        Time = 15,
        TimeC = 16,
        Velocity = 17,
        Vo2Max = 18
    }

}

