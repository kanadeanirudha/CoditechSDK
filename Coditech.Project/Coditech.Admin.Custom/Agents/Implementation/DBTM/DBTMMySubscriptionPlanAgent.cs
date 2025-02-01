using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;

namespace Coditech.Admin.Agents
{
    public class DBTMMySubscriptionPlanAgent : BaseAgent, IDBTMMySubscriptionPlanAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMMySubscriptionPlanClient _dBTMMySubscriptionPlanClient;
        #endregion

        #region Public Constructor
        public DBTMMySubscriptionPlanAgent(ICoditechLogging coditechLogging, IDBTMMySubscriptionPlanClient dBTMMySubscriptionPlanClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMMySubscriptionPlanClient = GetClient<IDBTMMySubscriptionPlanClient>(dBTMMySubscriptionPlanClient);
        }
        #endregion

        #region Public Methods    
        public virtual DBTMMySubscriptionPlanListViewModel GetDBTMMySubscriptionPlanList(DataTableViewModel dataTableModel)
        {
            long entityId = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession)?.EntityId ?? 0;
            if (entityId > 0)
            {

                FilterCollection filters = new FilterCollection();
                dataTableModel = dataTableModel ?? new DataTableViewModel();
                if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
                {
                    filters.Add("PlanName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                    filters.Add("DurationInDays", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                }
                SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);

                DBTMMySubscriptionPlanListResponse response = _dBTMMySubscriptionPlanClient.List(entityId,null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
                DBTMMySubscriptionPlanListModel mySubscriptionPlanList = new DBTMMySubscriptionPlanListModel { DBTMMySubscriptionPlanList = response?.DBTMMySubscriptionPlanList };
                DBTMMySubscriptionPlanListViewModel listViewModel = new DBTMMySubscriptionPlanListViewModel();
                listViewModel.DBTMMySubscriptionPlanList = mySubscriptionPlanList?.DBTMMySubscriptionPlanList?.ToViewModel<DBTMSubscriptionPlanViewModel>().ToList();

                SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMMySubscriptionPlanList.Count, BindColumns(),false);
                return listViewModel;
            }
            return new DBTMMySubscriptionPlanListViewModel();
        }
        #endregion

        #region protected
        protected virtual List<DatatableColumns> BindColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Plan Name",
                ColumnCode = "PlanName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Serial Code",
                ColumnCode = "DeviceSerialCode",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Duration In Days",
                ColumnCode = "DurationInDays",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Plan Cost",
                ColumnCode = "PlanCost",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Plan Discount",
                ColumnCode = "PlanDiscount",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Is Expired",
                ColumnCode = "IsExpired",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Plan Duration Expiration Date",
                ColumnCode = "PlanDurationExpirationDate",
                IsSortable = true,
            });
            return datatableColumnList;
        }

        #endregion
    }
}
