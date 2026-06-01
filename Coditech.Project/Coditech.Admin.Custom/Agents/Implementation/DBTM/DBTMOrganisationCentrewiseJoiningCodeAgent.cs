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
using Newtonsoft.Json;
using System.Diagnostics;
namespace Coditech.Admin.Agents
{
    public class DBTMOrganisationCentrewiseJoiningCodeAgent : OrganisationCentrewiseJoiningCodeAgent, IDBTMOrganisationCentrewiseJoiningCodeAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IOrganisationCentrewiseJoiningCodeClient _organisationCentrewiseJoiningCodeClient;
        private readonly IDBTMOrganisationCentrewiseJoiningCodeClient _dBTMOrganisationCentrewiseJoiningCodeClient;
        #endregion

        #region Public Constructor
        public DBTMOrganisationCentrewiseJoiningCodeAgent(ICoditechLogging coditechLogging, IOrganisationCentrewiseJoiningCodeClient organisationCentrewiseJoiningCodeClient, IDBTMOrganisationCentrewiseJoiningCodeClient dBTMOrganisationCentrewiseJoiningCodeClient, IUserClient userClient)
            : base(coditechLogging, organisationCentrewiseJoiningCodeClient, userClient)
        {
            _coditechLogging = coditechLogging;
            _organisationCentrewiseJoiningCodeClient = GetClient<IOrganisationCentrewiseJoiningCodeClient>(organisationCentrewiseJoiningCodeClient);
            _dBTMOrganisationCentrewiseJoiningCodeClient = GetClient<IDBTMOrganisationCentrewiseJoiningCodeClient>(dBTMOrganisationCentrewiseJoiningCodeClient);
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
            if (!string.IsNullOrEmpty(dataTableModel.SelectedParameter2) && dataTableModel.SelectedParameter2 != "0")
            {
                if (dataTableModel.SelectedParameter2 == "1")
                {
                    filters.Add("IsExpired", ProcedureFilterOperators.Equals, "0");
                }
                else if (dataTableModel.SelectedParameter2 == "2")
                {
                    filters.Add("IsExpired", ProcedureFilterOperators.Equals, "1");
                }
            }
            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "IsExpired" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            OrganisationCentrewiseJoiningCodeListResponse response = _organisationCentrewiseJoiningCodeClient.List(null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            OrganisationCentrewiseJoiningCodeListModel organisationCentrewiseJoiningCodeList = new OrganisationCentrewiseJoiningCodeListModel { OrganisationCentrewiseJoiningCodeList = response?.OrganisationCentrewiseJoiningCodeList };
            OrganisationCentrewiseJoiningCodeListViewModel listViewModel = new OrganisationCentrewiseJoiningCodeListViewModel();
            listViewModel.OrganisationCentrewiseJoiningCodeList = organisationCentrewiseJoiningCodeList?.OrganisationCentrewiseJoiningCodeList?.ToViewModel<OrganisationCentrewiseJoiningCodeViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.OrganisationCentrewiseJoiningCodeList.Count, BindColumns());
            return listViewModel;
        }

        //Test Wise Reports File
        public virtual DBTMOrganisationCentrewiseJoiningCodeViewModel GetTraineeActiveJoiningCode(string centreCode)
        {
            string trainerId = "";
            UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
            if (userModel?.Custom1?.ToLower() == "dbtmtrainer")
            {
                trainerId = JsonConvert.DeserializeObject<DBTMCustomUserModel>(userModel.Custom3 ?? string.Empty)?.GeneralTrainerMasterId?.ToString() ?? "";
            }
            DBTMOrganisationCentrewiseJoiningCodeViewModel viewModel = new DBTMOrganisationCentrewiseJoiningCodeViewModel();
            DBTMOrganisationCentrewiseJoiningCodeResponse response = _dBTMOrganisationCentrewiseJoiningCodeClient.GetTraineeActiveJoiningCode(centreCode, trainerId);
            viewModel.FilePath = response.FilePath;
            viewModel.FileName = response.FileName;
            return viewModel;
        }

        public DBTMOrganisationCentrewiseJoiningCodeViewModel GetTrainerActiveJoiningCode(string centreCode)
        {
            DBTMOrganisationCentrewiseJoiningCodeResponse data = _dBTMOrganisationCentrewiseJoiningCodeClient.GetTrainerActiveJoiningCode(centreCode);
            if (data == null)
                return null;
            DBTMOrganisationCentrewiseJoiningCodeViewModel dBTMOrganisationCentrewiseJoiningCodeViewModel = new DBTMOrganisationCentrewiseJoiningCodeViewModel
            {
                JoiningCode = data.JoiningCode,
                IsInQueue = data.IsInQueue,
                QueueValidTill = data.QueueValidTill
            };
            return dBTMOrganisationCentrewiseJoiningCodeViewModel;
        }

        public List<OrganisationCentrewiseJoiningCodeViewModel> GetTraineeActiveJoiningCodeList(string centreCode, string trainerId, int rows)
        {
            try
            {
                _coditechLogging.LogMessage("GetTraineeActiveJoiningCodeList method execution started.", "DBTMOrganisationCentrewiseJoiningCode", TraceLevel.Info);
                OrganisationCentrewiseJoiningCodeListResponse response = _dBTMOrganisationCentrewiseJoiningCodeClient.GetTraineeActiveJoiningCodeList(centreCode, trainerId, rows);
                return response?.OrganisationCentrewiseJoiningCodeList?.ToViewModel<OrganisationCentrewiseJoiningCodeViewModel>() ?.ToList() ?? new List<OrganisationCentrewiseJoiningCodeViewModel>();             
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMOrganisationCentrewiseJoiningCode", TraceLevel.Error);
                return new List<OrganisationCentrewiseJoiningCodeViewModel>();
            }
        }

        //Delete Report .
        public virtual bool DeleteJoiningCodeFile(string fileName)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMOrganisationCentrewiseJoiningCode", TraceLevel.Info);
                TrueFalseResponse response = _dBTMOrganisationCentrewiseJoiningCodeClient.DeleteJoiningCodeFile(new ParameterModel { Ids = fileName });
                return response?.IsSuccess ?? false;
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMOrganisationCentrewiseJoiningCode", TraceLevel.Error);
                return false;
            }
        }
        public override OrganisationCentrewiseJoiningCodeViewModel CreateOrganisationCentrewiseJoiningCode(OrganisationCentrewiseJoiningCodeViewModel organisationCentrewiseJoiningCodeViewModel)
        {
            try
            {
                UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
                organisationCentrewiseJoiningCodeViewModel.CreatedBy = userModel.UserMasterId;

                OrganisationCentrewiseJoiningCodeResponse response = _organisationCentrewiseJoiningCodeClient.CreateOrganisationCentrewiseJoiningCode(organisationCentrewiseJoiningCodeViewModel.ToModel<OrganisationCentrewiseJoiningCodeModel>());
                OrganisationCentrewiseJoiningCodeModel organisationCentrewiseJoiningCodeModel = response?.OrganisationCentrewiseJoiningCodeModel;
                return HelperUtility.IsNotNull(organisationCentrewiseJoiningCodeModel) ? organisationCentrewiseJoiningCodeModel.ToViewModel<OrganisationCentrewiseJoiningCodeViewModel>() : new OrganisationCentrewiseJoiningCodeViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.OrganisationCentrewiseJoiningCode.ToString(), TraceLevel.Warning);

                return (OrganisationCentrewiseJoiningCodeViewModel)GetViewModelWithErrorMessage(organisationCentrewiseJoiningCodeViewModel, ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.OrganisationCentrewiseJoiningCode.ToString(), TraceLevel.Error);
                return (OrganisationCentrewiseJoiningCodeViewModel)GetViewModelWithErrorMessage(organisationCentrewiseJoiningCodeViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }
        public bool IsTrainerJoiningCodeLocked(string joiningCode)
        {
            return _dBTMOrganisationCentrewiseJoiningCodeClient.IsTrainerJoiningCodeLocked(joiningCode);
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
                ColumnName = "Created By",
                ColumnCode = "Custom2",
                IsSortable = false,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Batch",
                ColumnCode = "Custom3",
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