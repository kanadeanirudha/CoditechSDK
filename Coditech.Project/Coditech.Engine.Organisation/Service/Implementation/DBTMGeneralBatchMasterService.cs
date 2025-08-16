using Coditech.API.Data;
using Coditech.Common.API;
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
        public DBTMGeneralBatchMasterService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(coditechLogging, serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMDeviceDataRepository = new CoditechRepository<DBTMDeviceData>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMDeviceDataRepository = new CoditechRepository<DBTMDeviceData>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMBatchActivityRepository = new CoditechRepository<DBTMBatchActivity>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _generalBatchMasterRepository = new CoditechRepository<GeneralBatchMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _generalBatchUserRepository = new CoditechRepository<GeneralBatchUser>(_serviceProvider.GetService<Coditech_Entities>());
        }

        //Create GeneralBatch.
        public override GeneralBatchModel CreateGeneralBatch(GeneralBatchModel generalBatchModel)
        {
            if (IsNull(generalBatchModel.CustomDropdownSelectedValue2))
                throw new CoditechException(ErrorCodes.InvalidData, "Selected User cannot be null.");
            generalBatchModel = base.CreateGeneralBatch(generalBatchModel);
            if (generalBatchModel.GeneralBatchMasterId > 0 && generalBatchModel.CustomDropdownSelectedValue1?.Count > 0 && generalBatchModel.CustomDropdownSelectedValue2?.Count > 0)
            {
                foreach (int dBTMTestMasterId in generalBatchModel.CustomDropdownSelectedValue1.Select(int.Parse))
                {
                    DBTMBatchActivity dBTMBatchActivity = new DBTMBatchActivity
                    {
                        GeneralBatchMasterId = generalBatchModel.GeneralBatchMasterId,
                        DBTMTestMasterId = dBTMTestMasterId,
                    };
                    _dBTMBatchActivityRepository.Insert(dBTMBatchActivity);
                }
                foreach (long traineeEntityId in generalBatchModel.CustomDropdownSelectedValue2.Select(long.Parse))
                {
                    GeneralBatchUser generalBatchUser = new GeneralBatchUser
                    {
                        GeneralBatchMasterId = generalBatchModel.GeneralBatchMasterId,
                        EntityId = traineeEntityId,
                        UserType = UserTypeEnum.Trainee.ToString(),
                    };
                    _generalBatchUserRepository.Insert(generalBatchUser);
                }
            }
            else
            {
                generalBatchModel.HasError = true;
                generalBatchModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
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
            bool isGeneralBatchUpdated = base.UpdateGeneralBatch(generalBatchModel);
            if (isGeneralBatchUpdated)
            {
                if (generalBatchModel.CustomDropdownSelectedValue1?.Count > 0)
                {
                    //List<DBTMBatchActivity> existingActivities = _dBTMBatchActivityRepository.Table.Where(x => x.GeneralBatchMasterId == generalBatchModel.GeneralBatchMasterId).ToList();
                    //foreach (DBTMBatchActivity dBTMBatchActivity in existingActivities)
                    //{
                    //    _dBTMBatchActivityRepository.Delete(dBTMBatchActivity);
                    //}

                    //foreach (int dBTMTestMasterId in generalBatchModel.CustomDropdownSelectedValue1.Select(int.Parse))
                    //{
                    //    DBTMBatchActivity newDBTMBatchActivity = new DBTMBatchActivity
                    //    {
                    //        GeneralBatchMasterId = generalBatchModel.GeneralBatchMasterId,
                    //        DBTMTestMasterId = dBTMTestMasterId,
                    //    };
                    //    _dBTMBatchActivityRepository.Insert(newDBTMBatchActivity);
                    //}
                    // Get current and new test master IDs
                    var currentIds = _dBTMBatchActivityRepository.Table
                        .Where(x => x.GeneralBatchMasterId == generalBatchModel.GeneralBatchMasterId)
                        .Select(x => x.DBTMTestMasterId)
                        .ToList();

                    var newIds = generalBatchModel.CustomDropdownSelectedValue1.Select(int.Parse).ToList();

                    // Delete activities not in newIds
                    var idsToDelete = currentIds.Except(newIds).ToList();
                    foreach (var id in idsToDelete)
                    {
                        var activityToDelete = _dBTMBatchActivityRepository.Table
                            .FirstOrDefault(x => x.GeneralBatchMasterId == generalBatchModel.GeneralBatchMasterId && x.DBTMTestMasterId == id);
                        if (activityToDelete != null)
                            _dBTMBatchActivityRepository.Delete(activityToDelete);
                    }

                    // Insert activities that are new
                    var idsToInsert = newIds.Except(currentIds).ToList();
                    foreach (var id in idsToInsert)
                    {
                        var newActivity = new DBTMBatchActivity
                        {
                            GeneralBatchMasterId = generalBatchModel.GeneralBatchMasterId,
                            DBTMTestMasterId = id,
                        };
                        _dBTMBatchActivityRepository.Insert(newActivity);
                    }
                }
                if (generalBatchModel.CustomDropdownSelectedValue2 != null && generalBatchModel.CustomDropdownSelectedValue2.Any())
                {
                    List<GeneralBatchUser> existingUsers = _generalBatchUserRepository.Table.Where(x => x.GeneralBatchMasterId == generalBatchModel.GeneralBatchMasterId).ToList();
                    foreach (GeneralBatchUser generalBatchUser in existingUsers)
                    {
                        _generalBatchUserRepository.Delete(generalBatchUser);
                    }
                    foreach (long traineeEntityId in generalBatchModel.CustomDropdownSelectedValue2.Select(long.Parse))
                    {
                        GeneralBatchUser newGeneralBatchUser = new GeneralBatchUser
                        {
                            GeneralBatchMasterId = generalBatchModel.GeneralBatchMasterId,
                            EntityId = traineeEntityId,
                            UserType = UserTypeEnum.Trainee.ToString(),
                        };
                        _generalBatchUserRepository.Insert(newGeneralBatchUser);
                    }
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
            List<GeneralBatchUserModel> batchList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMGeneralBatchUserAssociatedList @GeneralBatchMasterId,@UserType,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 6, out pageListModel.TotalRowCount)?.ToList();
            GeneralBatchUserListModel listModel = new GeneralBatchUserListModel();

            listModel.GeneralBatchUserList = batchList?.Count > 0 ? batchList : new List<GeneralBatchUserModel>();
            listModel.BindPageListModel(pageListModel);

            if (generalBatchMasterId > 0)
            {
                listModel.BatchName= _generalBatchMasterRepository.Table.Where(x => x.GeneralBatchMasterId == generalBatchMasterId).Select(x => x.BatchName).FirstOrDefault();
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
            List<GeneralBatchUserModel> batchList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMBatchUserList @CentreCode,@GeneralTrainerMasterId,@RowsCount OUT", 2, out pageListModel.TotalRowCount)?.ToList();
            GeneralBatchUserListModel listModel = new GeneralBatchUserListModel();
            listModel.GeneralBatchUserList = batchList?.Count > 0 ? batchList : new List<GeneralBatchUserModel>();
            return listModel;
        }
        #endregion
    }
}
