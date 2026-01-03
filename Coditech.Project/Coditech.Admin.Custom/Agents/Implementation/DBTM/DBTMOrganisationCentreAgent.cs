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
using Newtonsoft.Json;
using System.Diagnostics;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.Admin.Agents
{
    public class DBTMOrganisationCentreAgent : BaseAgent, IDBTMOrganisationCentreAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMOrganisationCentreClient _dBTMOrganisationCentreClient;
        #endregion

        #region Public Constructor
        public DBTMOrganisationCentreAgent(ICoditechLogging coditechLogging, IDBTMOrganisationCentreClient dBTMOrganisationCentreClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMOrganisationCentreClient = GetClient<IDBTMOrganisationCentreClient>(dBTMOrganisationCentreClient);
        }
        #endregion

        #region Public Methods
        //Get Activity List View Sequence
        public virtual DBTMActivityListViewSequenceListViewModel GetActivityListViewSequenceList(int dBTMOrganisationCentreMasterId, DataTableViewModel dataTableModel)
        {
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            DBTMActivityListViewSequenceListResponse response = _dBTMOrganisationCentreClient.GetActivityListViewSequenceList(dBTMOrganisationCentreMasterId, null, null, null, 0, int.MaxValue);
            DBTMActivityListViewSequenceListViewModel listViewModel = new DBTMActivityListViewSequenceListViewModel();
            listViewModel.DBTMActivityListViewSequenceList = response?.DBTMActivityListViewSequenceList?.ToViewModel<DBTMActivityListViewSequenceViewModel>().ToList()?? new List<DBTMActivityListViewSequenceViewModel>();
            SetListPagingData( listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMActivityListViewSequenceList.Count, BindActivityListViewSequenceColumns());
            listViewModel.CentreCode = response?.CentreCode;
            listViewModel.TestName = response?.TestName;
            return listViewModel;
        }

        public virtual DBTMCentrewiseTestParameterListViewViewModel GetDBTMCentrewiseTestParameterListView(int dBTMOrganisationCentreParameterListViewSequenceId)
        {
            DBTMCentrewiseTestParameterListViewResponse response = _dBTMOrganisationCentreClient.GetDBTMCentrewiseTestParameterListView(dBTMOrganisationCentreParameterListViewSequenceId);
            return response?.DBTMCentrewiseTestParameterListViewModel.ToViewModel<DBTMCentrewiseTestParameterListViewViewModel>();
        }

        //Update Activity List View Sequence
        public virtual DBTMCentrewiseTestParameterListViewViewModel UpdateDBTMCentrewiseTestParameterListView(DBTMCentrewiseTestParameterListViewViewModel dBTMCentrewiseTestParameterListViewViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMTest", TraceLevel.Info);
                DBTMCentrewiseTestParameterListViewResponse response = _dBTMOrganisationCentreClient.UpdateDBTMCentrewiseTestParameterListView(dBTMCentrewiseTestParameterListViewViewModel.ToModel<DBTMCentrewiseTestParameterListViewModel>());
                DBTMCentrewiseTestParameterListViewModel dBTMCentrewiseTestParameterListViewModel = response?.DBTMCentrewiseTestParameterListViewModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMTest", TraceLevel.Info);
                return IsNotNull(dBTMCentrewiseTestParameterListViewModel) ? dBTMCentrewiseTestParameterListViewModel.ToViewModel<DBTMCentrewiseTestParameterListViewViewModel>() : (DBTMCentrewiseTestParameterListViewViewModel)GetViewModelWithErrorMessage(new DBTMCentrewiseTestParameterListViewViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return (DBTMCentrewiseTestParameterListViewViewModel)GetViewModelWithErrorMessage(dBTMCentrewiseTestParameterListViewViewModel, GeneralResources.UpdateErrorMessage);
            }
        }
        #endregion

        #region protected
        protected virtual List<DatatableColumns> BindActivityListViewSequenceColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Column Name",
                ColumnCode = "ColumnName",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Display On",
                ColumnCode = "DisplayOn",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Is Column Cell Bold",
                ColumnCode = "IsColumnCellBold",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Is Active",
                ColumnCode = "IsActive",
            });
            return datatableColumnList;
        }
        #endregion
    }
}
