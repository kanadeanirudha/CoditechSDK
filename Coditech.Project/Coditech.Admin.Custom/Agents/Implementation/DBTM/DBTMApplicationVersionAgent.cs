using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.API.Model.Custom.DBTM.DBTMApplicationVersion;
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
    public class DBTMApplicationVersionAgent : BaseAgent, IDBTMApplicationVersionAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMApplicationVersionClient _dBTMApplicationVersionClient;
        #endregion

        #region Public Constructor
        public DBTMApplicationVersionAgent(ICoditechLogging coditechLogging, IDBTMApplicationVersionClient dBTMApplicationVersionClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMApplicationVersionClient = GetClient<IDBTMApplicationVersionClient>(dBTMApplicationVersionClient);
        }
        #endregion

        #region Public Methods
        public virtual DBTMApplicationVersionListViewModel GetDBTMApplicationVersionList(DataTableViewModel dataTableModel)
        {
            FilterCollection filters = null;
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters = new FilterCollection();
                filters.Add("ApplicationType", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("Version", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "ApplicationVersionName" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            DBTMApplicationVersionListResponse response = _dBTMApplicationVersionClient.List(null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            DBTMApplicationVersionListModel dBTMApplicationVersionList = new DBTMApplicationVersionListModel { DBTMApplicationVersionList = response?.DBTMApplicationVersionList };
            DBTMApplicationVersionListViewModel listViewModel = new DBTMApplicationVersionListViewModel();
            listViewModel.DBTMApplicationVersionList = dBTMApplicationVersionList?.DBTMApplicationVersionList?.ToViewModel<DBTMApplicationVersionViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMApplicationVersionList.Count, BindColumns());
            return listViewModel;
        }

        //Create DBTMApplicationVersion.
        public virtual DBTMApplicationVersionViewModel CreateDBTMApplicationVersion(DBTMApplicationVersionViewModel dBTMApplicationVersionViewModel)
        {
            try
            {
                DBTMApplicationVersionResponse response = _dBTMApplicationVersionClient.CreateDBTMApplicationVersion(dBTMApplicationVersionViewModel.ToModel<DBTMApplicationVersionModel>());
                DBTMApplicationVersionModel dBTMApplicationVersionModel = response?.DBTMApplicationVersionModel;
                return IsNotNull(dBTMApplicationVersionModel) ? dBTMApplicationVersionModel.ToViewModel<DBTMApplicationVersionViewModel>() : new DBTMApplicationVersionViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMApplicationVersion", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMApplicationVersionViewModel)GetViewModelWithErrorMessage(dBTMApplicationVersionViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMApplicationVersionViewModel)GetViewModelWithErrorMessage(dBTMApplicationVersionViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMApplicationVersion", TraceLevel.Error);
                return (DBTMApplicationVersionViewModel)GetViewModelWithErrorMessage(dBTMApplicationVersionViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Get DBTMApplicationVersion by dBTMApplicationVersionId.
        public virtual DBTMApplicationVersionViewModel GetDBTMApplicationVersion(long dBTMApplicationVersionId)
        {
            DBTMApplicationVersionResponse response = _dBTMApplicationVersionClient.GetDBTMApplicationVersion(dBTMApplicationVersionId);
            return response?.DBTMApplicationVersionModel.ToViewModel<DBTMApplicationVersionViewModel>();
        }

        //Update  DBTMApplicationVersion.
        public virtual DBTMApplicationVersionViewModel UpdateDBTMApplicationVersion(DBTMApplicationVersionViewModel dBTMApplicationVersionViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMApplicationVersion", TraceLevel.Info);
                DBTMApplicationVersionResponse response = _dBTMApplicationVersionClient.UpdateDBTMApplicationVersion(dBTMApplicationVersionViewModel.ToModel<DBTMApplicationVersionModel>());
                DBTMApplicationVersionModel dBTMApplicationVersionModel = response?.DBTMApplicationVersionModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMApplicationVersion", TraceLevel.Info);
                return IsNotNull(dBTMApplicationVersionModel) ? dBTMApplicationVersionModel.ToViewModel<DBTMApplicationVersionViewModel>() : (DBTMApplicationVersionViewModel)GetViewModelWithErrorMessage(new DBTMApplicationVersionViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMApplicationVersion", TraceLevel.Error);
                return (DBTMApplicationVersionViewModel)GetViewModelWithErrorMessage(dBTMApplicationVersionViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Delete DBTMApplicationVersion.
        public virtual bool DeleteDBTMApplicationVersion(string dBTMApplicationVersionId, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMApplicationVersion", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _dBTMApplicationVersionClient.DeleteDBTMApplicationVersion(new ParameterModel { Ids = dBTMApplicationVersionId });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMApplicationVersion", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = "ErrorDeleteDBTMApplicationVersion";
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMApplicationVersion", TraceLevel.Error);
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
                ColumnName = "Appication Type",
                ColumnCode = "AppicationType",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Version",
                ColumnCode = "Version",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Version Details",
                ColumnCode = "VersionDetails",
                IsSortable = true,
            });
            return datatableColumnList;
        }
        #endregion
        #region
        #endregion
    }
}
