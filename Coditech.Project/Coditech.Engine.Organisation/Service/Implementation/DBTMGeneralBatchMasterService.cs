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
    public class DBTMGeneralBatchMasterService : GeneralBatchMasterService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMDeviceData> _dBTMDeviceDataRepository;
        private readonly ICoditechRepository<DBTMBatchActivity> _dBTMBatchActivityRepository;
        private readonly ICoditechRepository<GeneralBatchMaster> _generalBatchMasterRepository;
        public DBTMGeneralBatchMasterService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(coditechLogging, serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMDeviceDataRepository = new CoditechRepository<DBTMDeviceData>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMDeviceDataRepository = new CoditechRepository<DBTMDeviceData>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMBatchActivityRepository = new CoditechRepository<DBTMBatchActivity>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _generalBatchMasterRepository = new CoditechRepository<GeneralBatchMaster>(_serviceProvider.GetService<Coditech_Entities>());
        }

        //Create GeneralBatch.
        public override GeneralBatchModel CreateGeneralBatch(GeneralBatchModel generalBatchModel)
        {
            generalBatchModel = base.CreateGeneralBatch(generalBatchModel);
            if (generalBatchModel.GeneralBatchMasterId > 0 && generalBatchModel.CustomDropdownSelectedValue1?.Count > 0)
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
            generalBatchModel.Duration = _generalBatchMasterRepository.Table.Where(x => x.GeneralBatchMasterId == generalBatchMasterId).Select(x => x.Duration).FirstOrDefault();
            return generalBatchModel;
        }

        //Update GeneralBatchMaster.
        public override bool UpdateGeneralBatch(GeneralBatchModel generalBatchModel)
        {
            bool isGeneralBatchUpdated = base.UpdateGeneralBatch(generalBatchModel);
            if (isGeneralBatchUpdated)
            {
                List<DBTMBatchActivity> existingActivities = _dBTMBatchActivityRepository.Table.Where(x => x.GeneralBatchMasterId == generalBatchModel.GeneralBatchMasterId).ToList();
                foreach (DBTMBatchActivity dBTMBatchActivity in existingActivities)
                {
                    _dBTMBatchActivityRepository.Delete(dBTMBatchActivity);
                }
                if (generalBatchModel.CustomDropdownSelectedValue1?.Count > 0)
                {
                    foreach (int dBTMTestMasterId in generalBatchModel.CustomDropdownSelectedValue1.Select(int.Parse))
                    {
                        DBTMBatchActivity newDBTMBatchActivity = new DBTMBatchActivity
                        {
                            GeneralBatchMasterId = generalBatchModel.GeneralBatchMasterId,
                            DBTMTestMasterId = dBTMTestMasterId,
                        };
                        _dBTMBatchActivityRepository.Insert(newDBTMBatchActivity);
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
                GeneralBatchModel model = GetGeneralBatch(generalBatchMasterId);
                if (IsNotNull(listModel))
                {
                    listModel.BatchName = model.BatchName;
                }
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

        #endregion
    }
}
