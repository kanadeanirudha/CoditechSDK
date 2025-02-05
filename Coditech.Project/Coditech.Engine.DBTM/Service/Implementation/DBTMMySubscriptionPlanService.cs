using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;

using System.Collections.Specialized;
using System.Data;

using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMMySubscriptionPlanService : IDBTMMySubscriptionPlanService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
       
        public DBTMMySubscriptionPlanService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
        }

        public virtual DBTMMySubscriptionPlanListModel GetDBTMMySubscriptionPlanList(long entityId,FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMSubscriptionPlanModel> objStoredProc = new CoditechViewRepository<DBTMSubscriptionPlanModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@EntityId", entityId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMSubscriptionPlanModel> dBTMMySubscriptionPlanList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMMySubscriptionPlanList @EntityId,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT",5, out pageListModel.TotalRowCount)?.ToList();
            DBTMMySubscriptionPlanListModel listModel = new DBTMMySubscriptionPlanListModel();

            listModel.DBTMMySubscriptionPlanList = dBTMMySubscriptionPlanList?.Count > 0 ? dBTMMySubscriptionPlanList : new List<DBTMSubscriptionPlanModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }
    }
}
