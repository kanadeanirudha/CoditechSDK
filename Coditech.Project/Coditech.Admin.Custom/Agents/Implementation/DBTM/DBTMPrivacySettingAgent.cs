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
using static Coditech.Common.Helper.HelperUtility;
using System.Diagnostics;
namespace Coditech.Admin.Agents
{
    public class DBTMPrivacySettingAgent : BaseAgent, IDBTMPrivacySettingAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMPrivacySettingClient _dBTMPrivacySettingClient;
        #endregion

        #region Public Constructor
        public DBTMPrivacySettingAgent(ICoditechLogging coditechLogging, IDBTMPrivacySettingClient dBTMPrivacySettingClient, IUserClient userClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMPrivacySettingClient = GetClient<IDBTMPrivacySettingClient>(dBTMPrivacySettingClient);
        }
        #endregion

        #region Public Methods
        #region DBTMPrivacySetting
        public virtual DBTMPrivacySettingListViewModel GetDBTMPrivacySettingList(DataTableViewModel dataTableModel)
        {
            FilterCollection filters = new FilterCollection();
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            
            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            DBTMPrivacySettingListResponse response = _dBTMPrivacySettingClient.List(dataTableModel.SelectedCentreCode, null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            DBTMPrivacySettingListModel dBTMPrivacySettingList = new DBTMPrivacySettingListModel { DBTMPrivacySettingList = response?.DBTMPrivacySettingList };
            DBTMPrivacySettingListViewModel listViewModel = new DBTMPrivacySettingListViewModel();
            listViewModel.DBTMPrivacySettingList = dBTMPrivacySettingList?.DBTMPrivacySettingList?.ToViewModel<DBTMPrivacySettingViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMPrivacySettingList.Count, BindColumns());
            return listViewModel;
        }

        //Create DBTMPrivacySetting.
        public virtual DBTMPrivacySettingViewModel CreateDBTMPrivacySetting(DBTMPrivacySettingViewModel dBTMPrivacySettingViewModel)
        {
            try
            {
                DBTMPrivacySettingResponse response = _dBTMPrivacySettingClient.CreateDBTMPrivacySetting(dBTMPrivacySettingViewModel.ToModel<DBTMPrivacySettingModel>());
                DBTMPrivacySettingModel dBTMPrivacySettingModel = response?.DBTMPrivacySettingModel;
                return IsNotNull(dBTMPrivacySettingModel) ? dBTMPrivacySettingModel.ToViewModel<DBTMPrivacySettingViewModel>() : new DBTMPrivacySettingViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrivacySetting", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMPrivacySettingViewModel)GetViewModelWithErrorMessage(dBTMPrivacySettingViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMPrivacySettingViewModel)GetViewModelWithErrorMessage(dBTMPrivacySettingViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrivacySetting", TraceLevel.Error);
                return (DBTMPrivacySettingViewModel)GetViewModelWithErrorMessage(dBTMPrivacySettingViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Get DBTMPrivacySetting by  DBTMPrivacySetting id.
        public virtual DBTMPrivacySettingViewModel GetDBTMPrivacySetting(int dBTMPrivacySettingId)
        {
            DBTMPrivacySettingResponse response = _dBTMPrivacySettingClient.GetDBTMPrivacySetting(dBTMPrivacySettingId);
            return response?.DBTMPrivacySettingModel.ToViewModel<DBTMPrivacySettingViewModel>();
        }

        //Update DBTMPrivacySetting.
        public virtual DBTMPrivacySettingViewModel UpdateDBTMPrivacySetting(DBTMPrivacySettingViewModel dBTMPrivacySettingViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMPrivacySetting", TraceLevel.Info);
                DBTMPrivacySettingResponse response = _dBTMPrivacySettingClient.UpdateDBTMPrivacySetting(dBTMPrivacySettingViewModel.ToModel<DBTMPrivacySettingModel>());
                DBTMPrivacySettingModel dBTMPrivacySettingModel = response?.DBTMPrivacySettingModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMPrivacySetting", TraceLevel.Info);
                return HelperUtility.IsNotNull(dBTMPrivacySettingModel) ? dBTMPrivacySettingModel.ToViewModel<DBTMPrivacySettingViewModel>() : (DBTMPrivacySettingViewModel)GetViewModelWithErrorMessage(new DBTMPrivacySettingViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrivacySetting", TraceLevel.Error);
                return (DBTMPrivacySettingViewModel)GetViewModelWithErrorMessage(dBTMPrivacySettingViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Delete DBTMPrivacySetting.
        public virtual bool DeleteDBTMPrivacySetting(string dBTMPrivacySettingIds, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMPrivacySetting", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _dBTMPrivacySettingClient.DeleteDBTMPrivacySetting(new ParameterModel { Ids = dBTMPrivacySettingIds });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrivacySetting", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteDBTMPrivacySetting;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrivacySetting", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
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
                ColumnName = "Is Notification On",
                ColumnCode = "IsNotificationOn",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Is Location On",
                ColumnCode = "IsLocationOn",
                IsSortable = true,
            });
           
            return datatableColumnList;
        }
        #endregion
    }
}
