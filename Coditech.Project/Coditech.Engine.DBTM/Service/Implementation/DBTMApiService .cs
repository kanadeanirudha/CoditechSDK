using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using System.Collections.Generic;
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
                    dBTMBatchModel.DBTMTestApiModel.TestName = testDetails.TestName;
                    dBTMBatchModel.DBTMTestApiModel.TestCode = testDetails.TestCode;
                    dBTMBatchModel.DBTMTestApiModel.MinimunPairedDevice = testDetails.MinimunPairedDevice;
                    dBTMBatchModel.DBTMTestApiModel.LapDistance = testDetails.LapDistance;
                    dBTMBatchModel.DBTMTestApiModel.IsLapDistanceChange = testDetails.IsLapDistanceChange;
                    dBTMBatchModel.DBTMTestApiModel.IsMultiTest = testDetails.IsMultiTest;
                }
                List<DBTMGeneralBatchUserModel> generalBatchUserList = (from a in _generalBatchUserRepository.Table
                                                                        join b in _userMasterRepository.Table on a.EntityId equals b.EntityId
                                                                        where a.GeneralBatchMasterId == generalBatchMasterId && b.UserType == UserTypeEnum.Trainee.ToString()
                                                                        select new DBTMGeneralBatchUserModel
                                                                        {
                                                                            FirstName = b.FirstName,
                                                                            LastName = b.LastName,
                                                                            ActivityStatusEnumId = a.ActivityStatusEnumId
                                                                        }
                                                   )?.ToList();
                if (generalBatchUserList?.Count > 0)
                {
                    foreach (var user in generalBatchUserList)
                    {
                        user.DBTMActivityStatus = GetEnumDisplayTextByEnumId(user.ActivityStatusEnumId);
                    }
                }
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

                List<DBTMTraineeAssignmentToUserApiModel> generalTraineeAssignmentToUserList = (from a in _dBTMTraineeAssignmentToUserRepository.Table

                                                                                                where a.DBTMTraineeAssignmentId == dBTMTraineeAssignmentId
                                                                                                select new DBTMTraineeAssignmentToUserApiModel
                                                                                                {
                                                                                                    DBTMTraineeDetailId = a.DBTMTraineeDetailId
                                                                                                }
                                                                   )?.ToList();
                dBTMTestApiModel.DBTMTraineeAssignmentToUserApiModel = generalTraineeAssignmentToUserList ?? new List<DBTMTraineeAssignmentToUserApiModel>();
            }
            return dBTMTestApiModel;
        }
    }
}

