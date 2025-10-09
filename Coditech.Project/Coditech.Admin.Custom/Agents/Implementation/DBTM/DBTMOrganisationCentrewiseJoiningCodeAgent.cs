using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Newtonsoft.Json;
namespace Coditech.Admin.Agents
{
    public class DBTMOrganisationCentrewiseJoiningCodeAgent : OrganisationCentrewiseJoiningCodeAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IOrganisationCentrewiseJoiningCodeClient _organisationCentrewiseJoiningCodeClient;
        #endregion

        #region Public Constructor
        public DBTMOrganisationCentrewiseJoiningCodeAgent(ICoditechLogging coditechLogging, IOrganisationCentrewiseJoiningCodeClient organisationCentrewiseJoiningCodeClient, IUserClient userClient)
            : base(coditechLogging, organisationCentrewiseJoiningCodeClient, userClient)
        {
            _coditechLogging = coditechLogging;
            _organisationCentrewiseJoiningCodeClient = GetClient<IOrganisationCentrewiseJoiningCodeClient>(organisationCentrewiseJoiningCodeClient);
        }
        #endregion

        #region Public Methods
        public override OrganisationCentrewiseJoiningCodeListViewModel GetOrganisationCentrewiseJoiningCodeList(DataTableViewModel dataTableModel)
        {
            UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
            FilterCollection filters = new FilterCollection();
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            filters.Add(FilterKeys.SelectedCentreCode, ProcedureFilterOperators.Equals, dataTableModel.SelectedCentreCode);
            filters.Add(FilterKeys.JoiningCodeTypeEnumId, ProcedureFilterOperators.Equals, dataTableModel.SelectedParameter1);
            if (userModel.Custom1.ToLower() == "dbtmtrainer")
            {
                string trainerId = JsonConvert.DeserializeObject<DBTMCustomUserModel>(userModel.Custom3 ?? string.Empty)?.GeneralTrainerMasterId?.ToString() ?? "";
                filters.Add("Custom1", ProcedureFilterOperators.Equals, trainerId);
            }
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters.Add("JoiningCode", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "IsExpired" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            OrganisationCentrewiseJoiningCodeListResponse response = _organisationCentrewiseJoiningCodeClient.List(null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            OrganisationCentrewiseJoiningCodeListModel organisationCentrewiseJoiningCodeList = new OrganisationCentrewiseJoiningCodeListModel { OrganisationCentrewiseJoiningCodeList = response?.OrganisationCentrewiseJoiningCodeList };
            OrganisationCentrewiseJoiningCodeListViewModel listViewModel = new OrganisationCentrewiseJoiningCodeListViewModel();
            listViewModel.OrganisationCentrewiseJoiningCodeList = organisationCentrewiseJoiningCodeList?.OrganisationCentrewiseJoiningCodeList?.ToViewModel<OrganisationCentrewiseJoiningCodeViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.OrganisationCentrewiseJoiningCodeList.Count, BindColumns());
            return listViewModel;
        }
        #endregion

        #region Protected Methods
        protected virtual List<DatatableColumns> BindColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Joining Code",
                ColumnCode = "JoiningCode",
                IsSortable = false,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Joining Code Type ",
                ColumnCode = "JoiningCodeType",
                IsSortable = false,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Trainer",
                ColumnCode = "Custom2",
                IsSortable = false,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Is Active Joining Code",
                ColumnCode = "IsExpired",
                IsSortable = false,
            });
            return datatableColumnList;
        }
        #endregion
    }
}