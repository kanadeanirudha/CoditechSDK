using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;
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
        private readonly ICoditechRepository<DBTMBatchActivity> _dBTMBatchActivityRepository;
        private readonly ICoditechRepository<DBTMTestMaster> _dBTMTestMasterRepository;
        private readonly ICoditechRepository<DBTMTraineeAssignment> _dBTMTraineeAssignmentRepository;
        private readonly ICoditechRepository<DBTMTraineeAssignmentToUser> _dBTMTraineeAssignmentToUserRepository;
        private readonly ICoditechRepository<DBTMDeviceData> _dBTMDeviceDataRepository;
        private readonly ICoditechRepository<DBTMDeviceDataDetails> _dBTMDeviceDataDetailsRepository;
        private readonly ICoditechRepository<DBTMTraineeDetails> _dBTMTraineeDetailsRepository;
        private readonly ICoditechRepository<DBTMTestParameter> _dBTMTestParameterRepository;


        public DBTMApiService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _generalBatchRepository = new CoditechRepository<GeneralBatchMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _generalBatchUserRepository = new CoditechRepository<GeneralBatchUser>(_serviceProvider.GetService<Coditech_Entities>());
            _userMasterRepository = new CoditechRepository<UserMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _dBTMBatchActivityRepository = new CoditechRepository<DBTMBatchActivity>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestMasterRepository = new CoditechRepository<DBTMTestMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTraineeAssignmentRepository = new CoditechRepository<DBTMTraineeAssignment>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTraineeAssignmentToUserRepository = new CoditechRepository<DBTMTraineeAssignmentToUser>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMDeviceDataRepository = new CoditechRepository<DBTMDeviceData>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMDeviceDataDetailsRepository = new CoditechRepository<DBTMDeviceDataDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTraineeDetailsRepository = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestParameterRepository = new CoditechRepository<DBTMTestParameter>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        //Add DBTMDeviceData.
        public bool InsertDeviceData(List<DBTMDeviceDataModel> dBTMDeviceDataModelList)
        {
            if (IsNull(dBTMDeviceDataModelList))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (dBTMDeviceDataModelList.Count > 0)
            {
                DateTime createdDate = DateTime.Now;
                foreach (DBTMDeviceDataModel dBTMDeviceDataModel in dBTMDeviceDataModelList)
                {
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

                    DBTMDeviceData DBTMDeviceDataDetails = _dBTMDeviceDataRepository.Insert(dBTMDeviceData);

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
            long userId = _userMasterRepository.Table.Where(x => x.EntityId == entityId && x.UserType == userType).FirstOrDefault().UserMasterId;
            List<DBTMBatchModel> batcheslist = _generalBatchRepository.Table.Where(x => x.CreatedBy == userId && x.IsActive)?
                .Select(b => new DBTMBatchModel
                {
                    GeneralBatchMasterId = b.GeneralBatchMasterId,
                    BatchName = b.BatchName,
                    BatchStartTime = b.BatchStartTime,
                })?.ToList();

            return batcheslist ?? new List<DBTMBatchModel>();
        }

        public DBTMBatchModel GetBatchDetails(int generalBatchMasterId)
        {
            int dbtmTestMasterId = _dBTMBatchActivityRepository.Table.Where(x => x.GeneralBatchMasterId == generalBatchMasterId).Select(x => x.DBTMTestMasterId).FirstOrDefault();
            DBTMBatchModel dBTMBatchModel = new DBTMBatchModel()
            {
                GeneralBatchMasterId = generalBatchMasterId,
            };

            if (dbtmTestMasterId > 0)
            {
                DBTMTestMaster testDetails = _dBTMTestMasterRepository.Table.Where(x => x.DBTMTestMasterId == dbtmTestMasterId).FirstOrDefault();

                if (testDetails == null || !testDetails.IsActive)
                {
                    throw new Exception("The test is not active or does not exist.");
                }
                else
                {
                    dBTMBatchModel.DBTMTestApiModel = new DBTMTestApiModel();
                    dBTMBatchModel.DBTMTestApiModel.ActivityCode = testDetails.DBTMTestMasterId.ToString();
                    dBTMBatchModel.DBTMTestApiModel.TestName = testDetails.TestName;
                    dBTMBatchModel.DBTMTestApiModel.TestCode = testDetails.TestCode;
                    dBTMBatchModel.DBTMTestApiModel.MinimunPairedDevice = testDetails.MinimunPairedDevice;
                    dBTMBatchModel.DBTMTestApiModel.LapDistance = testDetails.LapDistance;
                    dBTMBatchModel.DBTMTestApiModel.IsLapDistanceChange = testDetails.IsLapDistanceChange;
                    dBTMBatchModel.DBTMTestApiModel.IsMultiTest = testDetails.IsMultiTest;
                    dBTMBatchModel.DBTMTestApiModel.TestInstructions = testDetails.TestInstructions;
                }
                PageListModel pageListModel = new PageListModel(null, null, 0, 0);
                CoditechViewRepository<DBTMGeneralBatchUserModel> objStoredProc = new CoditechViewRepository<DBTMGeneralBatchUserModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
                objStoredProc.SetParameter("@GeneralBatchMasterId", generalBatchMasterId, ParameterDirection.Input, DbType.Int32);
                objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
                List<DBTMGeneralBatchUserModel> generalBatchUserList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetGeneralBatchUserList @GeneralBatchMasterId,@RowsCount OUT", 1, out pageListModel.TotalRowCount)?.ToList();

                dBTMBatchModel.DBTMGeneralBatchUserModel = generalBatchUserList ?? new List<DBTMGeneralBatchUserModel>();
            }
            return dBTMBatchModel;
        }

        public List<DBTMTestApiModel> GetAssignmentList(long entityId, string userType)
        {
            long entityIds = _userMasterRepository.Table.Where(x => x.EntityId == entityId && x.UserType == userType).FirstOrDefault().UserMasterId;

            List<DBTMTestApiModel> assignmentList = (from a in _dBTMTestMasterRepository.Table
                                                     join b in _dBTMTraineeAssignmentRepository.Table
                                                         on a.DBTMTestMasterId equals b.DBTMTestMasterId
                                                     where b.GeneralTrainerMasterId == entityId
                                                           && b.AssignmentDate <= DateTime.Today
                                                     select new DBTMTestApiModel
                                                     {
                                                         DBTMTraineeAssignmentId = b.DBTMTraineeAssignmentId,
                                                         DBTMTestMasterId = a.DBTMTestMasterId,
                                                         TestName = a.TestName,
                                                         AssignmentDate = b.AssignmentDate,
                                                         AssignmentTime = b.AssignmentTime,
                                                     })?.ToList();
            return assignmentList;
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

        private DBTMTraineeDetails GetDBTMTraineeDetailsByCode(string personCode)
            => _dBTMTraineeDetailsRepository.Table.Where(x => x.PersonCode == personCode).FirstOrDefault();
    }
}

