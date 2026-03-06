using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Resources;
using System.Collections.Specialized;
using System.Data;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMGeneralBatchMasterService : GeneralBatchMasterService, IDBTMBatchMasterService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMDeviceData> _dBTMDeviceDataRepository;
        private readonly ICoditechRepository<DBTMBatchActivity> _dBTMBatchActivityRepository;
        private readonly ICoditechRepository<GeneralBatchMaster> _generalBatchMasterRepository;
        private readonly ICoditechRepository<GeneralBatchUser> _generalBatchUserRepository;
        private readonly ICoditechRepository<DBTMCampUser> _dBTMCampUserRepository;
        private readonly ICoditechRepository<DBTMTraineeDetails> _dBTMTraineeDetailsRepository;
        public DBTMGeneralBatchMasterService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(coditechLogging, serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMDeviceDataRepository = new CoditechRepository<DBTMDeviceData>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMDeviceDataRepository = new CoditechRepository<DBTMDeviceData>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMBatchActivityRepository = new CoditechRepository<DBTMBatchActivity>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _generalBatchMasterRepository = new CoditechRepository<GeneralBatchMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _generalBatchUserRepository = new CoditechRepository<GeneralBatchUser>(_serviceProvider.GetService<Coditech_Entities>());
            _dBTMCampUserRepository = new CoditechRepository<DBTMCampUser>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTraineeDetailsRepository = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        public GeneralBatchListModel GetCalendarBatches(string centreCode, long userId, DateTime startDate, DateTime endDate)
        {       
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<GeneralBatchModel> objStoredProc = new CoditechViewRepository<GeneralBatchModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@CentreCode", centreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@UserMasterId", userId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@StartDate", startDate, ParameterDirection.Input, DbType.DateTime);
            objStoredProc.SetParameter("@EndDate", endDate, ParameterDirection.Input, DbType.DateTime);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<GeneralBatchModel> batchList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMCalendarBatches @CentreCode, @UserMasterId, @StartDate, @EndDate, @WhereClause, @Rows, @PageNo, @Order_BY, @RowsCount OUT", 8, out pageListModel.TotalRowCount)?.ToList();
            GeneralBatchListModel listModel = new GeneralBatchListModel();
            listModel.GeneralBatchList = batchList?.Count > 0 ? batchList : new List<GeneralBatchModel>();
            return listModel;
        }

        //Create GeneralBatch.
        public override GeneralBatchModel CreateGeneralBatch(GeneralBatchModel generalBatchModel)
        {
            //ToDo Anirudha sir
            if (generalBatchModel.BatchExpireDate == null)
            {
                generalBatchModel.BatchExpireDate = generalBatchModel.BatchStartDate.AddYears(1);
            }
            generalBatchModel = base.CreateGeneralBatch(generalBatchModel);
            if (generalBatchModel.GeneralBatchMasterId > 0)
            {
                if (generalBatchModel.CustomDropdownSelectedValue1?.Count > 0)
                {
                    List<DBTMBatchActivity> activityList = new List<DBTMBatchActivity>();
                    foreach (int dBTMTestMasterId in generalBatchModel.CustomDropdownSelectedValue1.Select(int.Parse))
                    {
                        activityList.Add(new DBTMBatchActivity
                        {
                            GeneralBatchMasterId = generalBatchModel.GeneralBatchMasterId,
                            DBTMTestMasterId = dBTMTestMasterId,
                        });
                    }
                    _dBTMBatchActivityRepository.Insert(activityList);
                }
                int activityStatusEnumId = GetEnumIdByEnumCode("Pending", "DBTMTestStatus");
                if (generalBatchModel.CustomDropdownSelectedValue2?.Count > 0)
                {
                    List<GeneralBatchUser> userList = new List<GeneralBatchUser>();
                    foreach (long traineeEntityId in generalBatchModel.CustomDropdownSelectedValue2.Select(long.Parse))
                    {
                        userList.Add(new GeneralBatchUser
                        {
                            GeneralBatchMasterId = generalBatchModel.GeneralBatchMasterId,
                            ActivityStatusEnumId = activityStatusEnumId,
                            EntityId = traineeEntityId,
                            UserType = UserTypeEnum.Trainee.ToString(),
                        });
                    }
                    _generalBatchUserRepository.Insert(userList);
                }
            }
            return generalBatchModel;
        }

        //Get GeneralBatchMaster by generalBatchMaster id.
        public override GeneralBatchModel GetGeneralBatch(int generalBatchMasterId)
        {
            GeneralBatchModel generalBatchModel = base.GetGeneralBatch(generalBatchMasterId);
            if (IsNull(generalBatchModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);
            generalBatchModel.CustomDropdownSelectedValue1 = _dBTMBatchActivityRepository.Table.Where(x => x.GeneralBatchMasterId == generalBatchMasterId).Select(x => x.DBTMTestMasterId.ToString()).ToList();
            generalBatchModel.CustomDropdownSelectedValue2 = _generalBatchUserRepository.Table.Where(x => x.GeneralBatchMasterId == generalBatchMasterId).Select(x => x.EntityId.ToString()).ToList();
            generalBatchModel.Duration = _generalBatchMasterRepository.Table.Where(x => x.GeneralBatchMasterId == generalBatchMasterId).Select(x => x.Duration).FirstOrDefault();
            return generalBatchModel;
        }

        //Update GeneralBatchMaster.
        public override bool UpdateGeneralBatch(GeneralBatchModel generalBatchModel)
        {
            //ToDo Anirudha sir
            if (generalBatchModel.BatchExpireDate == null)
            {
                generalBatchModel.BatchExpireDate = generalBatchModel.BatchStartDate.AddYears(1);
            }
            bool isGeneralBatchUpdated = base.UpdateGeneralBatch(generalBatchModel);
            if (isGeneralBatchUpdated)
            {
                if (generalBatchModel.CustomDropdownSelectedValue1?.Count > 0)
                {
                    // Get current and new test master IDs
                    var currentIds = _dBTMBatchActivityRepository.Table
                        .Where(x => x.GeneralBatchMasterId == generalBatchModel.GeneralBatchMasterId)
                        .Select(x => x.DBTMTestMasterId)
                        .ToList();

                    var newIds = generalBatchModel.CustomDropdownSelectedValue1.Select(int.Parse).ToList();

                    // Delete activities not in newIds
                    var idsToDelete = currentIds.Except(newIds).ToList();

                    var activityToDelete = _dBTMBatchActivityRepository.Table
                            .Where(x => x.GeneralBatchMasterId == generalBatchModel.GeneralBatchMasterId && idsToDelete.Contains(x.DBTMTestMasterId));

                    if (activityToDelete != null)
                        _dBTMBatchActivityRepository.Delete(activityToDelete);

                    // Insert activities that are new
                    var idsToInsert = newIds.Except(currentIds).ToList();

                    if (idsToInsert?.Count > 0)
                    {
                        List<DBTMBatchActivity> activityList = new List<DBTMBatchActivity>();
                        foreach (int dBTMTestMasterId in idsToInsert)
                        {
                            activityList.Add(new DBTMBatchActivity
                            {
                                GeneralBatchMasterId = generalBatchModel.GeneralBatchMasterId,
                                DBTMTestMasterId = dBTMTestMasterId,
                            });
                        }
                        _dBTMBatchActivityRepository.Insert(activityList);
                    }
                }
                if (generalBatchModel.CustomDropdownSelectedValue2 != null && generalBatchModel.CustomDropdownSelectedValue2.Any())
                {
                    // Get current and new trainee entity IDs
                    var currentUserIds = _generalBatchUserRepository.Table
                        .Where(x => x.GeneralBatchMasterId == generalBatchModel.GeneralBatchMasterId)
                        .Select(x => x.EntityId)
                        .ToList();

                    var newUserIds = generalBatchModel.CustomDropdownSelectedValue2.Select(long.Parse).ToList();

                    // Delete users not in new selection
                    var idsToDelete = currentUserIds.Except(newUserIds).ToList();
                    var userToDelete = _generalBatchUserRepository.Table
                          .Where(x => x.GeneralBatchMasterId == generalBatchModel.GeneralBatchMasterId && idsToDelete.Contains(x.EntityId));
                    if (userToDelete != null)
                        _generalBatchUserRepository.Delete(userToDelete);

                    // Insert users that are new
                    var idsToInsert = newUserIds.Except(currentUserIds).ToList();
                    List<GeneralBatchUser> userList = new List<GeneralBatchUser>();
                    int activityStatusEnumId = GetEnumIdByEnumCode("Pending", "DBTMTestStatus");

                    foreach (var id in idsToInsert)
                    {
                        userList.Add(new GeneralBatchUser
                        {
                            GeneralBatchMasterId = generalBatchModel.GeneralBatchMasterId,
                            EntityId = id,
                            ActivityStatusEnumId = activityStatusEnumId,
                            UserType = UserTypeEnum.Trainee.ToString(),
                        });
                    }
                    if (userList?.Count > 0)
                        _generalBatchUserRepository.Insert(userList);
                }
            }
            return isGeneralBatchUpdated;
        }

        #region GeneralBatchUser
        public override bool AssociateUnAssociateBatchwiseUser(GeneralBatchUserModel generalBatchUserModel)
        {
            if (generalBatchUserModel.GeneralBatchUserId == 0)
                generalBatchUserModel.ActivityStatusEnumId = GetEnumIdByEnumCode("Pending", "DBTMTestStatus");

            return base.AssociateUnAssociateBatchwiseUser(generalBatchUserModel);
        }

        public override GeneralBatchUserListModel GetGeneralBatchUserList(int generalBatchMasterId, string userType, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<GeneralBatchUserModel> objStoredProc = new CoditechViewRepository<GeneralBatchUserModel>(_serviceProvider.GetService<Coditech_Entities>());
            objStoredProc.SetParameter("@GeneralBatchMasterId", generalBatchMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@UserType", userType, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<GeneralBatchUserModel> batchList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMGeneralBatchUserAssociatedTrailList @GeneralBatchMasterId,@UserType,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 6, out pageListModel.TotalRowCount)?.ToList();
            GeneralBatchUserListModel listModel = new GeneralBatchUserListModel();

            listModel.GeneralBatchUserList = batchList?.Count > 0 ? batchList : new List<GeneralBatchUserModel>();
            listModel.BindPageListModel(pageListModel);

            if (generalBatchMasterId > 0)
            {
                listModel.BatchName = _generalBatchMasterRepository.Table.Where(x => x.GeneralBatchMasterId == generalBatchMasterId).Select(x => x.BatchName).FirstOrDefault();
            }
            listModel.GeneralBatchMasterId = generalBatchMasterId;
            return listModel;
        }
        //Delete GeneralBatchMaster.
        public override bool DeleteGeneralBatch(ParameterModel parameterModel)
        {
            const string batch = "Batch";
            int generalBatchMasterId = Convert.ToInt32(parameterModel.Ids);
            bool isReferenced = _dBTMDeviceDataRepository.Table.Any(d => d.TablePrimaryColumnId == generalBatchMasterId && d.TypeOfRecord == batch);
            if (isReferenced)
            {
                throw new CoditechException(ErrorCodes.AssociationDeleteError, "The batch is in use deleteion not allowed.");
            }
            return base.DeleteGeneralBatch(parameterModel);
        }

        public virtual GeneralBatchUserListModel GetDBTMBatchUserList(string selectedCentreCode, long generalTrainerMasterId, int generalBatchMasterId)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<GeneralBatchUserModel> objStoredProc = new CoditechViewRepository<GeneralBatchUserModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@CentreCode", selectedCentreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@GeneralTrainerMasterId", generalTrainerMasterId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<GeneralBatchUserModel> batchList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMBatchUserTrailList @CentreCode,@GeneralTrainerMasterId,@RowsCount OUT", 2, out pageListModel.TotalRowCount)?.ToList();
            GeneralBatchUserListModel listModel = new GeneralBatchUserListModel();
            listModel.GeneralBatchUserList = batchList?.Count > 0 ? batchList : new List<GeneralBatchUserModel>();
            return listModel;
        }
        public bool ConvertCampUserToBatchUser(long dBTMTraineeDetailId)
        {
            if (dBTMTraineeDetailId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTraineeDetailId"));
            DBTMTraineeDetails trainee = _dBTMTraineeDetailsRepository.Table.FirstOrDefault(x => x.DBTMTraineeDetailId == dBTMTraineeDetailId);
            if (IsNull(trainee))
                throw new CoditechException(ErrorCodes.NullModel, "Trainee not found.");
            DBTMCampUser campUser = _dBTMCampUserRepository.Table.FirstOrDefault(x => x.EntityId == dBTMTraineeDetailId);
            if (IsNull(campUser))
                throw new CoditechException(ErrorCodes.NullModel, "Camp user not found.");
            GeneralBatchUser existingBatchUser = _generalBatchUserRepository.Table.FirstOrDefault(x => x.EntityId == dBTMTraineeDetailId);
            if (existingBatchUser != null)
                throw new CoditechException(ErrorCodes.AlreadyExist, "User already converted to batch.");
            campUser.ActivityStatusEnumId = GetEnumIdByEnumCode("Converted", "DBTMTestStatus");
            campUser.ModifiedDate = DateTime.Now;
            _dBTMCampUserRepository.Update(campUser);
            GeneralBatchUser batchUser = new GeneralBatchUser
            {
                EntityId = dBTMTraineeDetailId,
                UserType = UserTypeEnum.Trainee.ToString(),
                ActivityStatusEnumId = GetEnumIdByEnumCode("Pending", "DBTMTestStatus"),
                CreatedDate = DateTime.Now
            };
            _generalBatchUserRepository.Insert(batchUser);
            trainee.IsBatchUser = true;
            _dBTMTraineeDetailsRepository.Update(trainee);
            return true;
        }
        #endregion
    }
}
