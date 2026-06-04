using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;
using Newtonsoft.Json;
using System.Data;
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
                using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
                {
                    // Cache trainee details to avoid duplicate DB hits
                    var traineeCache = new Dictionary<string, DBTMTraineeDetails>(StringComparer.OrdinalIgnoreCase);

                    foreach (var dBTMDeviceDataModel in dBTMDeviceDataModelList)
                    {
                        if (string.Equals(dBTMDeviceDataModel.PersonCode, "DryRun", StringComparison.OrdinalIgnoreCase))
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
                                string encryptedValue =EncryptionHelper.Encrypt(parameterValue);

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

                            var insertedDetails =_dBTMDeviceDataDetailsRepository.Insert(detailList,dBTMDeviceDataModel.CreatedBy);

                            if (insertedDetails == null)
                            {
                                throw new CoditechException(ErrorCodes.InvalidData,"Failed to insert device data details.");
                            }
                        }
                    }

                    scope.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex,"InsertDeviceData failed. Transaction rolled back.");
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
                                           : $"{b.BatchName}({u.FirstName} {u.LastName})",
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

                dBTMBatchModel.DBTMGeneralBatchUserModel = generalBatchUserList ?? new List<DBTMGeneralBatchUserModel>();
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
            generalBatchUserList = generalBatchUserList ?? new List<DBTMGeneralBatchUserModel>();
            return generalBatchUserList;
        }
        #endregion
        #region Assignment
        public List<DBTMTestApiModel> GetAssignmentList(long entityId, string userType)
        {
            long entityIds = _userMasterRepository.Table.Where(x => x.EntityId == entityId && x.UserType == userType).FirstOrDefault().UserMasterId;
            //GetGeneralAssignmentList
            List<DBTMTestApiModel> assignmentList = new List<DBTMTestApiModel>();
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<DBTMTestApiModel> objStoredProc = new CoditechViewRepository<DBTMTestApiModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@EntityId", entityIds, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@UserType", userType, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMTestApiModel> generalAssignmentList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetGeneralAssignmentList @EntityId,@UserType,@RowsCount OUT", 1, out pageListModel.TotalRowCount)?.ToList();
            return generalAssignmentList;
        }

        public DBTMTestApiModel GetAssignmentDetails(long dBTMTraineeAssignmentId)
        {
            DBTMTestApiModel dBTMTestApiModel = new DBTMTestApiModel();
            int dbtmTestMasterId = _dBTMTraineeAssignmentRepository.Table.Where(x => x.DBTMTraineeAssignmentId == dBTMTraineeAssignmentId).Select(x => x.DBTMTestMasterId).FirstOrDefault();
            if (dbtmTestMasterId > 0)
            {
                DBTMTestMaster testDetails = _dBTMTestMasterRepository.Table.Where(x => x.DBTMTestMasterId == dbtmTestMasterId).FirstOrDefault();
                dbtmTestMasterId = testDetails.DBTMTestMasterId;
                dBTMTestApiModel.TestName = testDetails.TestName;
                dBTMTestApiModel.TestCode = testDetails.TestCode;
                dBTMTestApiModel.MinimunPairedDevice = testDetails.MinimunPairedDevice;
                dBTMTestApiModel.LapDistance = testDetails.LapDistance;
                dBTMTestApiModel.IsLapDistanceChange = testDetails.IsLapDistanceChange;
                dBTMTestApiModel.IsMultiTest = testDetails.IsMultiTest;
                dBTMTestApiModel.IsActive = testDetails.IsActive;

                PageListModel pageListModel = new PageListModel(null, null, 0, 0);
                CoditechViewRepository<DBTMTraineeAssignmentToUserApiModel> objStoredProc = new CoditechViewRepository<DBTMTraineeAssignmentToUserApiModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
                objStoredProc.SetParameter("@DBTMTraineeAssignmentId", dBTMTraineeAssignmentId, ParameterDirection.Input, DbType.Int64);
                objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
                List<DBTMTraineeAssignmentToUserApiModel> generalTraineeAssignmentToUserList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetGeneralAssignmentToUserList @DBTMTraineeAssignmentId,@RowsCount OUT", 1, out pageListModel.TotalRowCount)?.ToList();

                dBTMTestApiModel.DBTMTraineeAssignmentToUserApiModel = generalTraineeAssignmentToUserList ?? new List<DBTMTraineeAssignmentToUserApiModel>();
            }
            return dBTMTestApiModel;
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
                                                         a.ActivityCategoryName
                                                     })
                                                        .Distinct()
                                                        .Select(x => new DBTMMobileActivityCategoryModel
                                                        {
                                                            DBTMActivityCategoryId = x.DBTMActivityCategoryId,
                                                            CategoryName = x.ActivityCategoryName
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
            List<DBTMTraineeDetailsModel> dBTMTraineeDetailsList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetTraineeListByActivityIds @CentreCode,@DBTMTestMasterIds,@GeneralTrainerMasterId,@RowsCount OUT", 3, out pageListModel.TotalRowCount)?.ToList();
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
            OrganisationCentrewiseJoiningCodeModel organisationCentrewiseJoiningCodeModel = _organisationCentrewiseJoiningCodeRepository.Table
                .Where(x => x.Custom1 == generalTrainerMasterId && !x.IsExpired).Select(x =>
                new OrganisationCentrewiseJoiningCodeModel { JoiningCode = x.JoiningCode, Custom3 = x.Custom3 }).FirstOrDefault();
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
                                           ? $"{b.CampName}"
                                           : $"{b.CampName}({u.FirstName} {u.LastName})",
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
            campUserList = campUserList ?? new List<DBTMGeneralBatchUserModel>();
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

