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
    public class DBTMBatchActivityService : BaseService, IDBTMBatchActivityService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMBatchActivity> _dBTMBatchActivityRepository;
        private readonly ICoditechRepository<GeneralBatchMaster> _generalBatchMasterRepository;

        public DBTMBatchActivityService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMBatchActivityRepository = new CoditechRepository<DBTMBatchActivity>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _generalBatchMasterRepository = new CoditechRepository<GeneralBatchMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        public virtual DBTMBatchActivityListModel GetDBTMBatchActivityList(int generalBatchMasterId, bool isAssociated, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMBatchActivityModel> objStoredProc = new CoditechViewRepository<DBTMBatchActivityModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@GeneralBatchMasterId", generalBatchMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@IsAssociated", isAssociated, ParameterDirection.Input, DbType.Boolean);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMBatchActivityModel> dBTMBatchActivityList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMBatchActivityList @GeneralBatchMasterId,@IsAssociated,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 6, out pageListModel.TotalRowCount)?.ToList();
            DBTMBatchActivityListModel listModel = new DBTMBatchActivityListModel();

            listModel.DBTMBatchActivityList = dBTMBatchActivityList?.Count > 0 ? dBTMBatchActivityList : new List<DBTMBatchActivityModel>();
            listModel.BindPageListModel(pageListModel);

            if (generalBatchMasterId > 0)
            {
                listModel.BatchName = _generalBatchMasterRepository.Table.Where(x=>x.GeneralBatchMasterId == generalBatchMasterId).FirstOrDefault().BatchName;
            }

            listModel.GeneralBatchMasterId = generalBatchMasterId;
            return listModel;
        }

        //Create DBTMBatchActivity.
        public virtual DBTMBatchActivityModel CreateDBTMBatchActivity(DBTMBatchActivityModel dBTMBatchActivityModel)
        {
            if (IsNull(dBTMBatchActivityModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);


            DBTMBatchActivity dBTMBatchActivity = dBTMBatchActivityModel.FromModelToEntity<DBTMBatchActivity>();

            //Insert new Associated Trainer and return it.
            DBTMBatchActivity dBTMBatchActivityData = _dBTMBatchActivityRepository.Insert(dBTMBatchActivity);
            if (dBTMBatchActivityData?.DBTMBatchActivityId > 0)
            {
                dBTMBatchActivityModel.DBTMBatchActivityId = dBTMBatchActivityData.DBTMBatchActivityId;
            }
            else
            {
                dBTMBatchActivityModel.HasError = true;
                dBTMBatchActivityModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return dBTMBatchActivityModel;
        }

        //Delete DBTMBatchActivity.
        public virtual bool DeleteDBTMBatchActivity(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMBatchActivityId"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("DBTMBatchActivityId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteDBTMBatchActivity @DBTMBatchActivityId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }
        #region Protected Method

        #endregion
    }
}
