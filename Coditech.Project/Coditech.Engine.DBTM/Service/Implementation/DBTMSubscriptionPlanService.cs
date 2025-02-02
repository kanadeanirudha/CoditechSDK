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
    public class DBTMSubscriptionPlanService : BaseService, IDBTMSubscriptionPlanService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMSubscriptionPlan> _dBTMSubscriptionPlanRepository;
        private readonly ICoditechRepository<DBTMSubscriptionPlanActivity> _dBTMSubscriptionPlanActivityRepository;
        public DBTMSubscriptionPlanService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMSubscriptionPlanRepository = new CoditechRepository<DBTMSubscriptionPlan>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMSubscriptionPlanActivityRepository = new CoditechRepository<DBTMSubscriptionPlanActivity>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        public virtual DBTMSubscriptionPlanListModel GetDBTMSubscriptionPlanList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMSubscriptionPlanModel> objStoredProc = new CoditechViewRepository<DBTMSubscriptionPlanModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMSubscriptionPlanModel> dBTMSubscriptionPlanList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMSubscriptionPlanList @WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 4, out pageListModel.TotalRowCount)?.ToList();
            DBTMSubscriptionPlanListModel listModel = new DBTMSubscriptionPlanListModel();

            listModel.DBTMSubscriptionPlanList = dBTMSubscriptionPlanList?.Count > 0 ? dBTMSubscriptionPlanList : new List<DBTMSubscriptionPlanModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }

        //Create DBTMSubscriptionPlan.
        public virtual DBTMSubscriptionPlanModel CreateDBTMSubscriptionPlan(DBTMSubscriptionPlanModel dBTMSubscriptionPlanModel)
        {
            if (IsNull(dBTMSubscriptionPlanModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            DBTMSubscriptionPlan dBTMSubscriptionPlan = dBTMSubscriptionPlanModel.FromModelToEntity<DBTMSubscriptionPlan>();

            //Create new DBTMSubscriptionPlan and return it.
            DBTMSubscriptionPlan dBTMSubscriptionPlanData = _dBTMSubscriptionPlanRepository.Insert(dBTMSubscriptionPlan);
            if (dBTMSubscriptionPlanData?.DBTMSubscriptionPlanId > 0)
            {
                dBTMSubscriptionPlanModel.DBTMSubscriptionPlanId = dBTMSubscriptionPlan.DBTMSubscriptionPlanId;
            }
            else
            {
                dBTMSubscriptionPlanModel.HasError = true;
                dBTMSubscriptionPlanModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return dBTMSubscriptionPlanModel;
        }

        //Get DBTMSubscriptionPlan by dBTMSubscriptionPlan id.
        public virtual DBTMSubscriptionPlanModel GetDBTMSubscriptionPlan(int dBTMSubscriptionPlanId)
        {
            if (dBTMSubscriptionPlanId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMSubscriptionPlanId"));

            //Get the DBTMSubscriptionPlan Details based on id.
            DBTMSubscriptionPlan dBTMSubscriptionPlan = _dBTMSubscriptionPlanRepository.Table.Where(x => x.DBTMSubscriptionPlanId == dBTMSubscriptionPlanId)?.FirstOrDefault();
            DBTMSubscriptionPlanModel dBTMSubscriptionPlanModel = dBTMSubscriptionPlan?.FromEntityToModel<DBTMSubscriptionPlanModel>();
            dBTMSubscriptionPlanModel.SubscriptionPlanType = GetEnumCodeByEnumId(dBTMSubscriptionPlanModel.SubscriptionPlanTypeEnumId);
            return dBTMSubscriptionPlanModel;
        }

        //Update DBTMSubscriptionPlan.
        public virtual bool UpdateDBTMSubscriptionPlan(DBTMSubscriptionPlanModel dBTMSubscriptionPlanModel)
        {
            if (IsNull(dBTMSubscriptionPlanModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (dBTMSubscriptionPlanModel.DBTMSubscriptionPlanId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMSubscriptionPlanId"));

            DBTMSubscriptionPlan dBTMSubscriptionPlan = dBTMSubscriptionPlanModel.FromModelToEntity<DBTMSubscriptionPlan>();

            //Update DBTMSubscriptionPlan
            bool isDBTMSubscriptionPlanUpdated = _dBTMSubscriptionPlanRepository.Update(dBTMSubscriptionPlan);
            if (!isDBTMSubscriptionPlanUpdated)
            {
                dBTMSubscriptionPlanModel.HasError = true;
                dBTMSubscriptionPlanModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isDBTMSubscriptionPlanUpdated;
        }

        //Delete DBTMSubscriptionPlan.
        public virtual bool DeleteDBTMSubscriptionPlan(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMSubscriptionPlanId"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("DBTMSubscriptionPlanId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteDBTMSubscriptionPlan @DBTMSubscriptionPlanId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        public virtual DBTMSubscriptionPlanActivityListModel GetDBTMSubscriptionPlanActivityList(int dBTMSubscriptionPlanId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {

            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMSubscriptionPlanActivityModel> objStoredProc = new CoditechViewRepository<DBTMSubscriptionPlanActivityModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@DBTMSubscriptionPlanId", dBTMSubscriptionPlanId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMSubscriptionPlanActivityModel> dBTMSubscriptionPlanActivityList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMTestMasterAssociatedList @DBTMSubscriptionPlanId, @WhereClause, @Rows, @PageNo, @Order_BY, @RowsCount OUT", 5, out pageListModel.TotalRowCount)?.ToList();
            DBTMSubscriptionPlanActivityListModel listModel = new DBTMSubscriptionPlanActivityListModel();

            listModel.DBTMSubscriptionPlanActivityList = dBTMSubscriptionPlanActivityList?.Count > 0 ? dBTMSubscriptionPlanActivityList : new List<DBTMSubscriptionPlanActivityModel>();
            listModel.BindPageListModel(pageListModel);

            if (dBTMSubscriptionPlanId > 0)
            {
                listModel.PlanName = _dBTMSubscriptionPlanRepository.Table.Where(x => x.DBTMSubscriptionPlanId == dBTMSubscriptionPlanId).FirstOrDefault().PlanName;
            }
            listModel.DBTMSubscriptionPlanId = dBTMSubscriptionPlanId;
            return listModel;

        }

        //Update  Associate UnAssociate 
        public virtual bool AssociateUnAssociatePlanActivity(DBTMSubscriptionPlanActivityModel dBTMSubscriptionPlanActivityModel)
        {

            bool isAssociateUnAssociateDBTMSubscriptionPlanActivity = false;
            DBTMSubscriptionPlanActivity dBTMSubscriptionPlanActivity = new DBTMSubscriptionPlanActivity();
            if (dBTMSubscriptionPlanActivityModel.DBTMSubscriptionPlanActivityId > 0)
            {
                dBTMSubscriptionPlanActivity = _dBTMSubscriptionPlanActivityRepository.Table.Where(x => x.DBTMSubscriptionPlanActivityId == dBTMSubscriptionPlanActivityModel.DBTMSubscriptionPlanActivityId)?.FirstOrDefault();
                isAssociateUnAssociateDBTMSubscriptionPlanActivity = _dBTMSubscriptionPlanActivityRepository.Delete(dBTMSubscriptionPlanActivity);
            }
            else
            {
                dBTMSubscriptionPlanActivity = dBTMSubscriptionPlanActivityModel.FromModelToEntity<DBTMSubscriptionPlanActivity>();
                dBTMSubscriptionPlanActivity = _dBTMSubscriptionPlanActivityRepository.Insert(dBTMSubscriptionPlanActivity);
                isAssociateUnAssociateDBTMSubscriptionPlanActivity = dBTMSubscriptionPlanActivity.DBTMSubscriptionPlanActivityId > 0;
            }

            if (!isAssociateUnAssociateDBTMSubscriptionPlanActivity)
            {
                dBTMSubscriptionPlanActivityModel.HasError = true;
                dBTMSubscriptionPlanActivityModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isAssociateUnAssociateDBTMSubscriptionPlanActivity;
        }        
    }
}
