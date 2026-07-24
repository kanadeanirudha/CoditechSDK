using Coditech.Admin.Utilities;
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
    public class DBTMPrintQRAgent : BaseAgent, IDBTMPrintQRAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMPrintQRClient _dBTMPrintQRClient;
        #endregion

        #region Public Constructor
        public DBTMPrintQRAgent(ICoditechLogging coditechLogging, IDBTMPrintQRClient dBTMPrintQRClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMPrintQRClient = GetClient<IDBTMPrintQRClient>(dBTMPrintQRClient);
        }
        #endregion

        #region Public Methods

        //DBTM PrintQR.
        public virtual DBTMPrintQRViewModel DBTMPrintQR(DBTMPrintQRViewModel dBTMPrintQRViewModel)
        {
            try
            {
                DBTMPrintQRResponse response = _dBTMPrintQRClient.DBTMPrintQR(dBTMPrintQRViewModel.ToModel<DBTMPrintQRModel>());
                DBTMPrintQRModel dBTMPrintQRModel = response?.DBTMPrintQRModel;
                return IsNotNull(dBTMPrintQRModel) ? dBTMPrintQRModel.ToViewModel<DBTMPrintQRViewModel>() : new DBTMPrintQRViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrintQR", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMPrintQRViewModel)GetViewModelWithErrorMessage(dBTMPrintQRViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMPrintQRViewModel)GetViewModelWithErrorMessage(dBTMPrintQRViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrintQR", TraceLevel.Error);
                return (DBTMPrintQRViewModel)GetViewModelWithErrorMessage(dBTMPrintQRViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Get DBTM PrintQR by DBTM PrintQR master id.
        public virtual DBTMPrintQRListViewModel GetDBTMPrintQR(string personIds)
        {
            DBTMPrintQRListResponse response = _dBTMPrintQRClient.GetDBTMPrintQR(personIds);
            DBTMPrintQRListViewModel listViewModel = new DBTMPrintQRListViewModel();
            listViewModel.DBTMPrintQRList = response?.DBTMPrintQRList?.ToViewModel<DBTMPrintQRViewModel>().ToList() ?? new List<DBTMPrintQRViewModel>();
            return listViewModel;
        }


        #region DBTMPrintQRTrainee
        public virtual DBTMPrintQRListViewModel GetDBTMPrintQRTraineeList(int generalBatchMasterId, DataTableViewModel dataTableModel)
        {
            FilterCollection filters = new FilterCollection();
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters.Add("FirstName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("MiddleName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("LastName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);
            DBTMPrintQRListResponse response = _dBTMPrintQRClient.GetDBTMPrintQRTraineeList(generalBatchMasterId, null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            DBTMPrintQRListModel DBTMPrintQRList = new DBTMPrintQRListModel { DBTMPrintQRList = response?.DBTMPrintQRList };
            DBTMPrintQRListViewModel listViewModel = new DBTMPrintQRListViewModel();
            listViewModel.DBTMPrintQRList = DBTMPrintQRList?.DBTMPrintQRList?.ToViewModel<DBTMPrintQRViewModel>().ToList();
            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMPrintQRList.Count, BindAssociatedBatchTraineeColumns());
            return listViewModel;
        }
        #endregion  
        #region protected      
        protected virtual List<DatatableColumns> BindAssociatedBatchTraineeColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "First Name",
                ColumnCode = "FirstName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Middle Name",
                ColumnCode = "MiddleName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Last Name",
                ColumnCode = "LastName",
                IsSortable = true,
            });
            //datatableColumnList.Add(new DatatableColumns()
            //{
            //    ColumnName = "Contact",
            //    ColumnCode = "MobileNumber",
            //    IsSortable = true,
            //});
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "QR",
                ColumnCode = " ",
            });
            return datatableColumnList;
        }
        #endregion
        #endregion
    }
}