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
    public class DBTMBatchActivityAgent : BaseAgent, IDBTMBatchActivityAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMBatchActivityClient _dBTMBatchActivityClient;
        private readonly IGeneralBatchClient _generalBatchClient;
        #endregion

        #region Public Constructor
        public DBTMBatchActivityAgent(ICoditechLogging coditechLogging, IDBTMBatchActivityClient dBTMBatchActivityClient, IGeneralBatchClient generalBatchClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMBatchActivityClient = GetClient<IDBTMBatchActivityClient>(dBTMBatchActivityClient);
            _generalBatchClient = GetClient<IGeneralBatchClient>(generalBatchClient);
        }
        #endregion

        #region Public Methods
        public virtual GeneralBatchListViewModel GetBatchList(DataTableViewModel dataTableModel)
        {
            FilterCollection filters = null;
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters = new FilterCollection();
                filters.Add("BatchName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("BatchTime", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            GeneralBatchListResponse response = _generalBatchClient.List(dataTableModel.SelectedCentreCode, null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            GeneralBatchListModel generalBatchList = new GeneralBatchListModel { GeneralBatchList = response?.GeneralBatchList };
            GeneralBatchListViewModel listViewModel = new GeneralBatchListViewModel();
            listViewModel.GeneralBatchList = generalBatchList?.GeneralBatchList?.ToViewModel<GeneralBatchViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.GeneralBatchList.Count, BindColumns());
            return listViewModel;
        }

        public virtual DBTMBatchActivityListViewModel GetDBTMBatchActivityList(int generalBatchMasterId, DataTableViewModel dataTableModel)
        {
            FilterCollection filters = new FilterCollection();
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters.Add("TestName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            DBTMBatchActivityListResponse response = _dBTMBatchActivityClient.GetDBTMBatchActivityList(generalBatchMasterId, true, null, filters, sortlist, dataTableModel.PageIndex, int.MaxValue);
            DBTMBatchActivityListModel associatedTrainerList = new DBTMBatchActivityListModel { DBTMBatchActivityList = response?.DBTMBatchActivityList };
            DBTMBatchActivityListViewModel listViewModel = new DBTMBatchActivityListViewModel();
            listViewModel.DBTMBatchActivityList = associatedTrainerList?.DBTMBatchActivityList?.ToViewModel<DBTMBatchActivityViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMBatchActivityList.Count, BindDBTMBatchActivityColumns());
            listViewModel.GeneralBatchMasterId = generalBatchMasterId;
            listViewModel.BatchName = response.BatchName;
            return listViewModel;
        }

        //Get DBTMBatchActivity by general Batch Master Id.
        public virtual DBTMBatchActivityViewModel DBTMBatchActivity(int generalBatchMasterId)
        {
            GeneralBatchResponse response = _generalBatchClient.GetGeneralBatch(generalBatchMasterId);
            DBTMBatchActivityViewModel dBTMBatchActivityViewModel = new DBTMBatchActivityViewModel()
            {
                BatchName = response.GeneralBatchModel.BatchName,
                GeneralBatchMasterId = generalBatchMasterId,
            };
            return dBTMBatchActivityViewModel;
        }

        //Create DBTMBatchActivity
        public virtual DBTMBatchActivityViewModel CreateDBTMBatchActivity(DBTMBatchActivityViewModel dBTMBatchActivityViewModel)
        {
            try
            {
                int generalBatchMasterId = dBTMBatchActivityViewModel.GeneralBatchMasterId;
                DBTMBatchActivityResponse response = _dBTMBatchActivityClient.CreateDBTMBatchActivity(dBTMBatchActivityViewModel.ToModel<DBTMBatchActivityModel>());
                DBTMBatchActivityModel dBTMBatchActivityModel = response?.DBTMBatchActivityModel;
                dBTMBatchActivityViewModel = IsNotNull(dBTMBatchActivityModel) ? dBTMBatchActivityModel.ToViewModel<DBTMBatchActivityViewModel>() : new DBTMBatchActivityViewModel();
                dBTMBatchActivityViewModel.GeneralBatchMasterId = generalBatchMasterId;
                return dBTMBatchActivityViewModel;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMBatchActivity", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMBatchActivityViewModel)GetViewModelWithErrorMessage(dBTMBatchActivityViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMBatchActivityViewModel)GetViewModelWithErrorMessage(dBTMBatchActivityViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMBatchActivity", TraceLevel.Error);
                return (DBTMBatchActivityViewModel)GetViewModelWithErrorMessage(dBTMBatchActivityViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Delete DBTMBatchActivity.
        public virtual bool DeleteDBTMBatchActivity(string dBTMBatchActivityId, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMBatchActivity", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _dBTMBatchActivityClient.DeleteDBTMBatchActivity(new ParameterModel { Ids = dBTMBatchActivityId});
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMBatchActivity", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteBatchActivityDetails;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMBatchActivity", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }
        #endregion

        #region protected
        protected virtual List<DatatableColumns> BindDBTMBatchActivityColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Test Name",
                ColumnCode = "TestName",
                IsSortable = true,
            });
            return datatableColumnList;
        }

        protected virtual List<DatatableColumns> BindColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Batch Name",
                ColumnCode = "BatchName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Batch Time",
                ColumnCode = "BatchTime",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Batch Start Time",
                ColumnCode = "BatchStartTime",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Is Active",
                ColumnCode = "IsActive",
                IsSortable = true,
            });
            return datatableColumnList;
        }
        #endregion
        #region
        #endregion
    }
}
