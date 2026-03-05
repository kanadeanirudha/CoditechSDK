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
    public class DBTMCampMasterService: BaseService, IDBTMCampMasterService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMCampMaster> _dBTMCampMasterRepository;
        private readonly ICoditechRepository<DBTMTestMaster> _dBTMTestMasterRepository;
        private readonly ICoditechRepository<DBTMCampUser> _dBTMCampUserRepository; private readonly ICoditechRepository<DBTMCampActivity> _dBTMCampActivityRepository;
        public DBTMCampMasterService(ICoditechLogging coditechLogging,IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMCampMasterRepository = new CoditechRepository<DBTMCampMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestMasterRepository = new CoditechRepository<DBTMTestMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMCampUserRepository = new CoditechRepository<DBTMCampUser>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMCampActivityRepository = new CoditechRepository<DBTMCampActivity>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }
        public virtual DBTMCampMasterListModel GetDBTMCampList(string selectedCentreCode, long userId,FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMCampMasterModel> objStoredProc = new CoditechViewRepository<DBTMCampMasterModel>(_serviceProvider.GetService<Coditech_Entities>());
            objStoredProc.SetParameter("@CentreCode", selectedCentreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@UserMasterId", userId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMCampMasterModel> dBTMCampList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMCampMasterList @CentreCode, @UserMasterId, @WhereClause, @Rows, @PageNo, @Order_BY, @RowsCount OUT", 6, out pageListModel.TotalRowCount)?.ToList();       
            DBTMCampMasterListModel listModel = new DBTMCampMasterListModel();
            listModel.DBTMCampMasterList = dBTMCampList?.Count > 0 ? dBTMCampList : new List<DBTMCampMasterModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }

        //Create DBTMCampMaster.
        public virtual DBTMCampMasterModel CreateDBTMCamp(DBTMCampMasterModel dBTMCampMasterModel)
        {
            if (IsNull(dBTMCampMasterModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);
            DBTMCampMaster dBTMCampMaster = dBTMCampMasterModel.FromModelToEntity<DBTMCampMaster>();
            //Create new Camp and return it.
            DBTMCampMaster CampData = _dBTMCampMasterRepository.Insert(dBTMCampMaster);
            if (CampData?.DBTMCampMasterId > 0)
            {
                dBTMCampMasterModel.DBTMCampMasterId = CampData.DBTMCampMasterId;
                if (dBTMCampMasterModel.CustomDropdownSelectedValue1?.Count > 0)
                {
                    List<DBTMCampActivity> activityList = new();
                    foreach (int testId in dBTMCampMasterModel.CustomDropdownSelectedValue1.Select(int.Parse))
                    {
                        activityList.Add(new DBTMCampActivity
                        {
                            DBTMCampMasterId = CampData.DBTMCampMasterId,
                            DBTMTestMasterId = testId
                        });
                    }
                    _dBTMCampActivityRepository.Insert(activityList);
                }
                if (dBTMCampMasterModel.CustomDropdownSelectedValue2?.Count > 0)
                {
                    List<DBTMCampUser> userList = new List<DBTMCampUser>();
                    int activityStatusEnumId = GetEnumIdByEnumCode("Pending", "DBTMTestStatus");
                    foreach (long traineeEntityId in dBTMCampMasterModel.CustomDropdownSelectedValue2.Select(long.Parse))
                    {
                        userList.Add(new DBTMCampUser
                        {
                            DBTMCampMasterId = dBTMCampMasterModel.DBTMCampMasterId,
                            EntityId = traineeEntityId,
                            ActivityStatusEnumId = activityStatusEnumId,
                            UserType = UserTypeEnum.Trainee.ToString(),
                        });
                    }
                    _dBTMCampUserRepository.Insert(userList);
                }
            }
            return dBTMCampMasterModel;
        }

        //Get Camp by Camp id.
        public virtual DBTMCampMasterModel GetDBTMCamp(int dBTMCampMasterId)
        {
            DBTMCampMaster dBTMCampMaster = _dBTMCampMasterRepository.Table.Where(x => x.DBTMCampMasterId == dBTMCampMasterId).FirstOrDefault();
            DBTMCampMasterModel dBTMCampMasterModel = dBTMCampMaster?.FromEntityToModel<DBTMCampMasterModel>();
            if (IsNotNull(dBTMCampMasterModel))
            {
                dBTMCampMasterModel.CustomDropdownSelectedValue1 =_dBTMCampActivityRepository.Table.Where(x => x.DBTMCampMasterId == dBTMCampMasterId).Select(x => x.DBTMTestMasterId.ToString()).ToList();
                dBTMCampMasterModel.CustomDropdownSelectedValue2 = _dBTMCampUserRepository.Table.Where(x => x.DBTMCampMasterId == dBTMCampMasterId).Select(x => x.EntityId.ToString()).ToList();
                dBTMCampMasterModel.Duration = _dBTMCampMasterRepository.Table.Where(x => x.DBTMCampMasterId == dBTMCampMasterId).Select(x => x.Duration).FirstOrDefault();
            }
            return dBTMCampMasterModel;
        }

        //Update DBTMCamp.
        public virtual bool UpdateDBTMCamp(DBTMCampMasterModel dBTMCampMasterModel)
        {
            if (IsNull(dBTMCampMasterModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);
            if (dBTMCampMasterModel.DBTMCampMasterId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMCampMasterId"));      
            DBTMCampMaster dBTMCampMaster = dBTMCampMasterModel.FromModelToEntity<DBTMCampMaster>();
            //Update Camp
            bool isCampUpdated = _dBTMCampMasterRepository.Update(dBTMCampMaster);
            if (isCampUpdated)
            {
                var currentIds = _dBTMCampActivityRepository.Table.Where(x => x.DBTMCampMasterId == dBTMCampMasterModel.DBTMCampMasterId).Select(x => x.DBTMTestMasterId).ToList();
                var newIds = dBTMCampMasterModel.CustomDropdownSelectedValue1?.Select(int.Parse).ToList() ?? new List<int>();
                var idsToDelete = currentIds.Except(newIds);
                var activitiesToDelete = _dBTMCampActivityRepository.Table.Where(x => x.DBTMCampMasterId == dBTMCampMasterModel.DBTMCampMasterId && idsToDelete.Contains(x.DBTMTestMasterId));
                if (activitiesToDelete.Any())
                    _dBTMCampActivityRepository.Delete(activitiesToDelete);
                var idsToInsert = newIds.Except(currentIds);
                if (idsToInsert.Any())
                {
                    List<DBTMCampActivity> activityList = new();
                    foreach (var id in idsToInsert)
                    {
                        activityList.Add(new DBTMCampActivity
                        {
                            DBTMCampMasterId = dBTMCampMasterModel.DBTMCampMasterId,
                            DBTMTestMasterId = id
                        });
                    }
                    _dBTMCampActivityRepository.Insert(activityList);
                }
            }
            return isCampUpdated;
        }

        //Delete DBTMCamp.
        public virtual bool DeleteDBTMCamp(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMCampMasterId"));
            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<Coditech_Entities>());
            objStoredProc.SetParameter("DBTMCampMasterId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteDBTMCamp @DBTMCampMasterId,  @Status OUT", 1, out status);
            return status == 1 ? true : false;
        }

        #region DBTMCampUser
        public virtual DBTMCampUserListModel GetDBTMCampUserList(int dBTMCampMasterId, string userType, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMCampUserModel> objStoredProc = new CoditechViewRepository<DBTMCampUserModel>(_serviceProvider.GetService<Coditech_Entities>());
            objStoredProc.SetParameter("@DBTMCampMasterId", dBTMCampMasterId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@UserType", userType, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMCampUserModel> CampList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMCampUserAssociatedList @DBTMCampMasterId,@UserType,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 6, out pageListModel.TotalRowCount)?.ToList();
            DBTMCampUserListModel listModel = new DBTMCampUserListModel();

            listModel.DBTMCampUserList = CampList?.Count > 0 ? CampList : new List<DBTMCampUserModel>();
            listModel.BindPageListModel(pageListModel);


            if (dBTMCampMasterId > 0)
            {
                DBTMCampMasterModel model = GetDBTMCamp(dBTMCampMasterId);
                if (IsNotNull(listModel))
                {
                    listModel.CampName = model.CampName;
                }
            }
            listModel.DBTMCampMasterId = dBTMCampMasterId;
            return listModel;
        }

        public virtual bool AssociateUnAssociateCampwiseUser(DBTMCampUserModel dBTMCampUserModel)
        {
            bool isAssociateUnAssociateCampwiseUser = false;

            DBTMCampUser dBTMCampUser = new DBTMCampUser();
            if (dBTMCampUserModel.DBTMCampUserId > 0)
            {
                dBTMCampUser = _dBTMCampUserRepository.Table.Where(x => x.DBTMCampUserId == dBTMCampUserModel.DBTMCampUserId)?.FirstOrDefault();
                isAssociateUnAssociateCampwiseUser = _dBTMCampUserRepository.Delete(dBTMCampUser);
            }
            else
            {
                dBTMCampUser = dBTMCampUserModel.FromModelToEntity<DBTMCampUser>();
                //dBTMCampUser.DBTMCampUserId = 0;
                dBTMCampUser = _dBTMCampUserRepository.Insert(dBTMCampUser);
                isAssociateUnAssociateCampwiseUser = dBTMCampUser.DBTMCampUserId > 0;
            }

            if (!isAssociateUnAssociateCampwiseUser)
            {
                dBTMCampUserModel.HasError = true;
                dBTMCampUserModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isAssociateUnAssociateCampwiseUser;
        }

        public virtual DBTMCampUserListModel GetCampUserListByCentreCodeAndGeneralTrainerMasterId(string selectedCentreCode, long generalTrainerMasterId, long DBTMCampMasterId)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<DBTMCampUserModel> objStoredProc = new CoditechViewRepository<DBTMCampUserModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@CentreCode", selectedCentreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@GeneralTrainerMasterId", generalTrainerMasterId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMCampUserModel> CampList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMCampUserList @CentreCode,@GeneralTrainerMasterId,@RowsCount OUT", 2, out pageListModel.TotalRowCount)?.ToList();
            DBTMCampUserListModel listModel = new DBTMCampUserListModel();
            listModel.DBTMCampUserList = CampList?.Count > 0 ? CampList : new List<DBTMCampUserModel>();
            return listModel;
        }
        #endregion
        #region Protected Method
        #endregion
    }
}
