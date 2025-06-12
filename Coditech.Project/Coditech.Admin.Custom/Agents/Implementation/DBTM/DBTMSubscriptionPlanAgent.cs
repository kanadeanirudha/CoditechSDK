using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Resources;
using System.Diagnostics;
using static Coditech.Common.Helper.HelperUtility;

namespace Coditech.Admin.Agents
{
    public class DBTMSubscriptionPlanAgent : BaseAgent, IDBTMSubscriptionPlanAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMSubscriptionPlanClient _dBTMSubscriptionPlanClient;
        #endregion

        #region Public Constructor
        public DBTMSubscriptionPlanAgent(ICoditechLogging coditechLogging, IDBTMSubscriptionPlanClient dBTMSubscriptionPlanClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMSubscriptionPlanClient = GetClient<IDBTMSubscriptionPlanClient> (dBTMSubscriptionPlanClient);
        }
        #endregion

        #region Public Methods    
        public virtual DBTMSubscriptionPlanListViewModel GetDBTMSubscriptionPlanList(DataTableViewModel dataTableModel)
        {            
                FilterCollection filters = new FilterCollection();
                dataTableModel = dataTableModel ?? new DataTableViewModel();
                if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
                {
                    filters.Add("PlanName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                    filters.Add("DurationInDays", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                }
                SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);

                DBTMSubscriptionPlanListResponse response = _dBTMSubscriptionPlanClient.List(null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
                DBTMSubscriptionPlanListModel ticketMasterList = new DBTMSubscriptionPlanListModel { DBTMSubscriptionPlanList = response?.DBTMSubscriptionPlanList };
                DBTMSubscriptionPlanListViewModel listViewModel = new DBTMSubscriptionPlanListViewModel();
                listViewModel.DBTMSubscriptionPlanList = ticketMasterList?.DBTMSubscriptionPlanList?.ToViewModel<DBTMSubscriptionPlanViewModel>().ToList();

                SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMSubscriptionPlanList.Count, BindColumns());
                return listViewModel;
        }

        //Create DBTMSubscriptionPlan.
        public virtual DBTMSubscriptionPlanViewModel CreateDBTMSubscriptionPlan(DBTMSubscriptionPlanViewModel dBTMSubscriptionPlanViewModel)
        {
            try
            {
                DBTMSubscriptionPlanResponse response = _dBTMSubscriptionPlanClient.CreateDBTMSubscriptionPlan(dBTMSubscriptionPlanViewModel.ToModel<DBTMSubscriptionPlanModel>());
                DBTMSubscriptionPlanModel dBTMSubscriptionPlanModel = response?.DBTMSubscriptionPlanModel;
                return IsNotNull(dBTMSubscriptionPlanModel) ? dBTMSubscriptionPlanModel.ToViewModel<DBTMSubscriptionPlanViewModel>() : new DBTMSubscriptionPlanViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMSubscriptionPlan", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMSubscriptionPlanViewModel)GetViewModelWithErrorMessage(dBTMSubscriptionPlanViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMSubscriptionPlanViewModel)GetViewModelWithErrorMessage(dBTMSubscriptionPlanViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMSubscriptionPlan", TraceLevel.Error);
                return (DBTMSubscriptionPlanViewModel)GetViewModelWithErrorMessage(dBTMSubscriptionPlanViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }


        //Get DBTMSubScriptionPlan by dBTMSubScriptionPlanId.
        public virtual DBTMSubscriptionPlanViewModel GetDBTMSubscriptionPlan(int dBTMSubscriptionPlanId)
        {
            DBTMSubscriptionPlanResponse response = _dBTMSubscriptionPlanClient.GetDBTMSubscriptionPlan(dBTMSubscriptionPlanId);
            return response?.DBTMSubscriptionPlanModel.ToViewModel<DBTMSubscriptionPlanViewModel>();
        }

        //Update  DBTMSubScriptionPlan.
        public virtual DBTMSubscriptionPlanViewModel UpdateDBTMSubscriptionPlan(DBTMSubscriptionPlanViewModel dBTMSubscriptionPlanViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMSubscriptionPlan", TraceLevel.Info);
                DBTMSubscriptionPlanResponse response = _dBTMSubscriptionPlanClient.UpdateDBTMSubscriptionPlan(dBTMSubscriptionPlanViewModel.ToModel<DBTMSubscriptionPlanModel>());
                DBTMSubscriptionPlanModel dBTMSubscriptionPlanModel = response?.DBTMSubscriptionPlanModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMSubscriptionPlan", TraceLevel.Info);
                return IsNotNull(dBTMSubscriptionPlanModel) ? dBTMSubscriptionPlanModel.ToViewModel<DBTMSubscriptionPlanViewModel>() : (DBTMSubscriptionPlanViewModel)GetViewModelWithErrorMessage(new DBTMSubscriptionPlanViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMSubscriptionPlan", TraceLevel.Error);
                return (DBTMSubscriptionPlanViewModel)GetViewModelWithErrorMessage(dBTMSubscriptionPlanViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Delete DBTMSubscriptionPlan.
        public virtual bool DeleteDBTMSubscriptionPlan(string dBTMSubscriptionPlanIds, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMSubscriptionPlan", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _dBTMSubscriptionPlanClient.DeleteDBTMSubscriptionPlan(new ParameterModel { Ids = dBTMSubscriptionPlanIds });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMSubscriptionPlan", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteDBTMSubscriptionPlan;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMSubscriptionPlan", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }

        #region Plan Activity
        public virtual DBTMSubscriptionPlanActivityListViewModel GetDBTMSubscriptionPlanActivityList(int dBTMSubscriptionPlanId, DataTableViewModel dataTableModel)
        {
            FilterCollection filters = new FilterCollection();
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters = new FilterCollection();
                filters.Add("TestName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            DBTMSubscriptionPlanActivityListResponse response = _dBTMSubscriptionPlanClient.GetDBTMSubscriptionPlanActivityList(dBTMSubscriptionPlanId, null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            DBTMSubscriptionPlanActivityListModel dBTMSubscriptionPlanActivityList = new DBTMSubscriptionPlanActivityListModel { DBTMSubscriptionPlanActivityList = response?.DBTMSubscriptionPlanActivityList };
            DBTMSubscriptionPlanActivityListViewModel listViewModel = new DBTMSubscriptionPlanActivityListViewModel();
            listViewModel.DBTMSubscriptionPlanActivityList = dBTMSubscriptionPlanActivityList?.DBTMSubscriptionPlanActivityList?.ToViewModel<DBTMSubscriptionPlanActivityViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMSubscriptionPlanActivityList.Count, BindDBTMPlanActivityColumns());
            listViewModel.DBTMSubscriptionPlanId = dBTMSubscriptionPlanId;
            listViewModel.PlanName = response.PlanName;
            return listViewModel;
        }

        //Update Associate UnAssociate Plan Activity.
        public virtual DBTMSubscriptionPlanActivityViewModel AssociateUnAssociatePlanActivity(DBTMSubscriptionPlanActivityViewModel dBTMSubscriptionPlanActivityViewModel)
        {
            try
            {
                int dBTMSubscriptionPlanId = dBTMSubscriptionPlanActivityViewModel.DBTMSubscriptionPlanId;
                int dBTMSubscriptionPlanActivityId = dBTMSubscriptionPlanActivityViewModel.DBTMSubscriptionPlanActivityId;
                DBTMSubscriptionPlanActivityResponse response = _dBTMSubscriptionPlanClient.AssociateUnAssociatePlanActivity(dBTMSubscriptionPlanActivityViewModel.ToModel<DBTMSubscriptionPlanActivityModel>());
                DBTMSubscriptionPlanActivityModel dBTMSubscriptionPlanActivityModel = response?.DBTMSubscriptionPlanActivityModel;
                dBTMSubscriptionPlanActivityViewModel = IsNotNull(dBTMSubscriptionPlanActivityModel) ? dBTMSubscriptionPlanActivityModel.ToViewModel<DBTMSubscriptionPlanActivityViewModel>() : new DBTMSubscriptionPlanActivityViewModel();
                dBTMSubscriptionPlanActivityViewModel.DBTMSubscriptionPlanId = dBTMSubscriptionPlanId;
                dBTMSubscriptionPlanActivityViewModel.DBTMSubscriptionPlanActivityId = dBTMSubscriptionPlanActivityId;
                return dBTMSubscriptionPlanActivityViewModel;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMSubscriptionPlan", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMSubscriptionPlanActivityViewModel)GetViewModelWithErrorMessage(dBTMSubscriptionPlanActivityViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMSubscriptionPlanActivityViewModel)GetViewModelWithErrorMessage(dBTMSubscriptionPlanActivityViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMSubscriptionPlan", TraceLevel.Error);
                return (DBTMSubscriptionPlanActivityViewModel)GetViewModelWithErrorMessage(dBTMSubscriptionPlanActivityViewModel, GeneralResources.UpdateErrorMessage);
            }
        }
        #endregion


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
                ColumnName = "Subscription Plan Type",
                ColumnCode = "SubscriptionPlanTypeEnumId",
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
                ColumnName = "Is Active",
                ColumnCode = "IsActive",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Is Tax Exclusive",
                ColumnCode = "IsTaxExclusive",
                IsSortable = true,
            });
            return datatableColumnList;
        }

        protected virtual List<DatatableColumns> BindDBTMPlanActivityColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Test Name",
                ColumnCode = "TestName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Test Code",
                ColumnCode = "TestCode",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Is Associated",
                ColumnCode = "IsAssociated",
                IsSortable = true,
            });
            return datatableColumnList;
        }
        #endregion
    }
}
