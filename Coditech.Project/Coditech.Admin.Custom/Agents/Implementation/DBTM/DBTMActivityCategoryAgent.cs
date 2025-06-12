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
    public class DBTMActivityCategoryAgent : BaseAgent, IDBTMActivityCategoryAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMActivityCategoryClient _dBTMActivityCategoryClient;
        #endregion

        #region Public Constructor
        public DBTMActivityCategoryAgent(ICoditechLogging coditechLogging, IDBTMActivityCategoryClient dBTMActivityCategoryClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMActivityCategoryClient = GetClient<IDBTMActivityCategoryClient>(dBTMActivityCategoryClient);
        }
        #endregion

        #region Public Methods
        public virtual DBTMActivityCategoryListViewModel GetDBTMActivityCategoryList(DataTableViewModel dataTableModel)
        {
            FilterCollection filters = null;
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters = new FilterCollection();
                filters.Add("ActivityCategoryCode", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("ActivityCategoryName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "ActivityCategoryName " : dataTableModel.SortByColumn, dataTableModel.SortBy);

            DBTMActivityCategoryListResponse response = _dBTMActivityCategoryClient.List(null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            DBTMActivityCategoryListModel dBTMActivityCategoryList = new DBTMActivityCategoryListModel { DBTMActivityCategoryList = response?.DBTMActivityCategoryList };
            DBTMActivityCategoryListViewModel listViewModel = new DBTMActivityCategoryListViewModel();
            listViewModel.DBTMActivityCategoryList = dBTMActivityCategoryList?.DBTMActivityCategoryList?.ToViewModel<DBTMActivityCategoryViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMActivityCategoryList.Count, BindColumns());
            return listViewModel;
        }

        //Create DBTMActivityCategory.
        public virtual DBTMActivityCategoryViewModel CreateDBTMActivityCategory(DBTMActivityCategoryViewModel dBTMActivityCategoryViewModel)
        {
            try
            {
                DBTMActivityCategoryResponse response = _dBTMActivityCategoryClient.CreateDBTMActivityCategory(dBTMActivityCategoryViewModel.ToModel<DBTMActivityCategoryModel>());
                DBTMActivityCategoryModel dBTMActivityCategoryModel = response?.DBTMActivityCategoryModel;
                return IsNotNull(dBTMActivityCategoryModel) ? dBTMActivityCategoryModel.ToViewModel<DBTMActivityCategoryViewModel>() : new DBTMActivityCategoryViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivityCategory", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMActivityCategoryViewModel)GetViewModelWithErrorMessage(dBTMActivityCategoryViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMActivityCategoryViewModel)GetViewModelWithErrorMessage(dBTMActivityCategoryViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivityCategory", TraceLevel.Error);
                return (DBTMActivityCategoryViewModel)GetViewModelWithErrorMessage(dBTMActivityCategoryViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Get DBTMActivityCategory by dBTMActivityCategoryId.
        public virtual DBTMActivityCategoryViewModel GetDBTMActivityCategory(short dBTMActivityCategoryId)
        {
            DBTMActivityCategoryResponse response = _dBTMActivityCategoryClient.GetDBTMActivityCategory(dBTMActivityCategoryId);
            return response?.DBTMActivityCategoryModel.ToViewModel<DBTMActivityCategoryViewModel>();
        }

        //Update  DBTMActivityCategory.
        public virtual DBTMActivityCategoryViewModel UpdateDBTMActivityCategory(DBTMActivityCategoryViewModel dBTMActivityCategoryViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMActivityCategory", TraceLevel.Info);
                DBTMActivityCategoryResponse response = _dBTMActivityCategoryClient.UpdateDBTMActivityCategory(dBTMActivityCategoryViewModel.ToModel<DBTMActivityCategoryModel>());
                DBTMActivityCategoryModel dBTMActivityCategoryModel = response?.DBTMActivityCategoryModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMActivityCategory", TraceLevel.Info);
                return IsNotNull(dBTMActivityCategoryModel) ? dBTMActivityCategoryModel.ToViewModel<DBTMActivityCategoryViewModel>() : (DBTMActivityCategoryViewModel)GetViewModelWithErrorMessage(new DBTMActivityCategoryViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivityCategory", TraceLevel.Error);
                return (DBTMActivityCategoryViewModel)GetViewModelWithErrorMessage(dBTMActivityCategoryViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Delete DBTMActivityCategory.
        public virtual bool DeleteDBTMActivityCategory(string dBTMActivityCategoryId, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMActivityCategory", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _dBTMActivityCategoryClient.DeleteDBTMActivityCategory(new ParameterModel { Ids = dBTMActivityCategoryId });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivityCategory", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteDBTMActivityCategory;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivityCategory", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }
        #endregion

        #region protected
        protected virtual List<DatatableColumns> BindColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Activity Category Name",
                ColumnCode = "ActivityCategoryName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Activity Category Code",
                ColumnCode = "ActivityCategoryCode",
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
        // it will return get all DBTMActivityCategory list from database 
        public virtual DBTMActivityCategoryListResponse GetDBTMActivityCategoryList()
        {
            DBTMActivityCategoryListResponse dBTMActivityCategoryList = _dBTMActivityCategoryClient.List(null, null, null, 1, int.MaxValue);
            return dBTMActivityCategoryList?.DBTMActivityCategoryList?.Count > 0 ? dBTMActivityCategoryList : new DBTMActivityCategoryListResponse();
        }
        #endregion
    }
}
