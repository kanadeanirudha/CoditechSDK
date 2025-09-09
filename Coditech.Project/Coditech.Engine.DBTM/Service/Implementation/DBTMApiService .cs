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
        private readonly ICoditechRepository<DBTMTestParameter> _dBTMTestParameterRepository;
        private readonly ICoditechRepository<DBTMActivityCategory> _dBTMActivityCategoryRepository;


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
            _dBTMTestParameterRepository = new CoditechRepository<DBTMTestParameter>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMActivityCategoryRepository = new CoditechRepository<DBTMActivityCategory>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

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

            if (dBTMDeviceDataModelList.Any(x => x.PersonCode == "DryRun"))
            {
                return true;
            }

            if (dBTMDeviceDataModelList.Count > 0)
            {
                DateTime? createdDate = null;
                foreach (DBTMDeviceDataModel dBTMDeviceDataModel in dBTMDeviceDataModelList)
                {
                    createdDate = DateTime.Now;
                    DBTMTraineeDetails dBTMTraineeDetails = GetDBTMTraineeDetailsByCode(dBTMDeviceDataModel.PersonCode);
                    if (IsNull(dBTMTraineeDetails))
                        throw new CoditechException(ErrorCodes.InvalidData, "Invalid Person Code");

                    DBTMDeviceData dBTMDeviceData = new DBTMDeviceData()
                    {
                        TypeOfRecord = dBTMDeviceDataModel.TypeOfRecord,
                        TablePrimaryColumnId = dBTMDeviceDataModel.TablePrimaryColumnId,
                        DeviceSerialCode = dBTMDeviceDataModel.DeviceSerialCode,
                        PersonCode = dBTMDeviceDataModel.PersonCode,
                        TestCode = dBTMDeviceDataModel.TestCode,
                        Comments = dBTMDeviceDataModel.Comments,
                        Height = dBTMTraineeDetails.Height,
                        Weight = dBTMTraineeDetails.Weight,
                        TestPerformedTime = dBTMDeviceDataModel.TestPerformedTime,
                        CreatedBy = dBTMDeviceDataModel.CreatedBy,
                        CreatedDate = createdDate
                    };

                    DBTMDeviceData DBTMDeviceDataDetails = _dBTMDeviceDataRepository.Insert(dBTMDeviceData, dBTMDeviceDataModel.CreatedBy);

                    if (DBTMDeviceDataDetails?.DBTMDeviceDataId > 0)
                    {
                        dBTMDeviceDataModel.DBTMDeviceDataId = DBTMDeviceDataDetails.DBTMDeviceDataId;
                        List<DBTMDeviceDataDetails> dBTMDeviceDataDetailsList = new List<DBTMDeviceDataDetails>();
                        foreach (var item in dBTMDeviceDataModel?.DataList)
                        {
                            DBTMDeviceDataDetails dBTMDeviceDataDetails = new DBTMDeviceDataDetails()
                            {
                                DBTMDeviceDataId = DBTMDeviceDataDetails.DBTMDeviceDataId,
                                ParameterCode = item.ParameterCode,
                                ParameterValue = item.ParameterValue,
                                FromTo = item.FromTo,
                                Row = item.Row,
                                CreatedBy = dBTMDeviceDataModel.CreatedBy,
                                CreatedDate = createdDate
                            };
                            dBTMDeviceDataDetailsList.Add(dBTMDeviceDataDetails);
                        }
                        _dBTMDeviceDataDetailsRepository.Insert(dBTMDeviceDataDetailsList);
                    }
                }

                string typeOfRecord = dBTMDeviceDataModelList.FirstOrDefault().TypeOfRecord;
                long tablePrimaryColumnId = dBTMDeviceDataModelList.FirstOrDefault().TablePrimaryColumnId;
                if (typeOfRecord == "Batch")
                {
                    List<long> entityIds = dBTMDeviceDataModelList.Where(x => x.EntityId > 0).Select(x => x.EntityId).ToList();
                    if (entityIds?.Count > 0)
                    {
                        List<GeneralBatchUser> generalBatchUsers = _generalBatchUserRepository.Table.Where(x => x.GeneralBatchMasterId == tablePrimaryColumnId && entityIds.Contains(x.EntityId)).ToList();
                        int activityStatusEnumId = GetEnumIdByEnumCode("Completed", "DBTMTestStatus");
                        generalBatchUsers.ForEach(x => { x.ActivityStatusEnumId = activityStatusEnumId; });
                        _generalBatchUserRepository.BatchUpdate(generalBatchUsers);
                    }
                }
            }
            return true;
        }

        public List<DBTMBatchModel> GetBatchList(long entityId, string userType)
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
                        batcheslist = _generalBatchRepository.Table
                            .Where(x => x.CreatedBy == user.UserMasterId && x.IsActive)
                            .Select(b => new DBTMBatchModel
                            {
                                GeneralBatchMasterId = b.GeneralBatchMasterId,
                                BatchName = b.BatchName,
                                BatchStartTime = b.BatchStartTime,
                            })
                            .ToList();
                    }
                    else
                    {
                        batcheslist = new List<DBTMBatchModel>();
                    }
                }
                else if (custom1 == CustomConstants.DBTMCentreOwner)
                {
                    batcheslist = (from b in _generalBatchRepository.Table
                                   join u in _userMasterRepository.Table on b.CreatedBy equals u.UserMasterId
                                   where b.CentreCode == employeeData.CentreCode && b.IsActive
                                   select new DBTMBatchModel
                                   {
                                       GeneralBatchMasterId = b.GeneralBatchMasterId,
                                       BatchName = b.BatchName + "(" + u.FirstName + " " + u.LastName + ")",
                                       BatchStartTime = b.BatchStartTime,
                                   })
                                   .ToList();
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

        public DBTMBatchModel GetBatchDetails(int generalBatchMasterId)
        {
            List<int> dbtmTestMasterIds = _dBTMBatchActivityRepository.Table.Where(x => x.GeneralBatchMasterId == generalBatchMasterId).Select(x => x.DBTMTestMasterId).ToList();
            DBTMBatchModel dBTMBatchModel = new DBTMBatchModel()
            {
                GeneralBatchMasterId = generalBatchMasterId,
            };

            if (dbtmTestMasterIds?.Count > 0)
            {
                List<DBTMTestMaster> testDetailList = _dBTMTestMasterRepository.Table.Where(x => dbtmTestMasterIds.Contains(x.DBTMTestMasterId) && x.IsActive)?.ToList();

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

        //Get Dashboard Details
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
                                                     where a.IsActive
                                                     select new DBTMMobileActivityCategoryModel()
                                                     {
                                                         CategoryName = a.ActivityCategoryName,
                                                         DBTMActivityCategoryId = a.DBTMActivityCategoryId
                                                     }).ToList();
            return dBTMDashboardModel;
        }
        private DBTMTraineeDetails GetDBTMTraineeDetailsByCode(string personCode)
            => _dBTMTraineeDetailsRepository.Table.Where(x => x.PersonCode == personCode).FirstOrDefault();
    }
}

