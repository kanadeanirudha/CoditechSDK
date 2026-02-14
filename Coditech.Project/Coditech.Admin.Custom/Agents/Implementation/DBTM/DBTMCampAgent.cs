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
    public class DBTMCampAgent : BaseAgent, IDBTMCampAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMCampClient _dBTMCampClient;
        #endregion

        #region Public Constructor
        public DBTMCampAgent(ICoditechLogging coditechLogging, IDBTMCampClient dBTMCampClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMCampClient = GetClient<IDBTMCampClient>(dBTMCampClient);
        }
        #endregion

        #region Public Methods
        public virtual DBTMCampListViewModel GetDBTMCampList(DataTableViewModel dataTableModel)
        {
            FilterCollection filters = null;
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters = new FilterCollection();
                filters.Add("CampName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("CampTime", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("CampStartDate", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("CampEndDate", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }
            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "CampName" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            DBTMCampListResponse response = _dBTMCampClient.List(dataTableModel.SelectedCentreCode,null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            DBTMCampMasterListModel dBTMCampMasterList = new DBTMCampMasterListModel { DBTMCampMasterList = response?.DBTMCampMasterList };
            DBTMCampListViewModel listViewModel = new DBTMCampListViewModel();
            listViewModel.DBTMCampMasterList = dBTMCampMasterList?.DBTMCampMasterList?.ToViewModel<DBTMCampMasterViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMCampMasterList.Count, BindColumns());
            return listViewModel;
        }

        //Create DBTM Camp.
        public virtual DBTMCampMasterViewModel CreateDBTMCamp(DBTMCampMasterViewModel dBTMCampMasterViewModel)
        {
            try
            {
                DBTMCampResponse response = _dBTMCampClient.CreateDBTMCamp(dBTMCampMasterViewModel.ToModel<DBTMCampMasterModel>());
                DBTMCampMasterModel dBTMCampMasterModel = response?.DBTMCampModel;
                return IsNotNull(dBTMCampMasterModel) ? dBTMCampMasterModel.ToViewModel<DBTMCampMasterViewModel>() : new DBTMCampMasterViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampMaster", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMCampMasterViewModel)GetViewModelWithErrorMessage(dBTMCampMasterViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMCampMasterViewModel)GetViewModelWithErrorMessage(dBTMCampMasterViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampMaster", TraceLevel.Error);
                return (DBTMCampMasterViewModel)GetViewModelWithErrorMessage(dBTMCampMasterViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Get DBTM Camp by DBTM Camp master id.
        public virtual DBTMCampMasterViewModel GetDBTMCamp(long dBTMCampMasterId)
        {
            DBTMCampResponse response = _dBTMCampClient.GetDBTMCamp(dBTMCampMasterId);
            return response?.DBTMCampModel.ToViewModel<DBTMCampMasterViewModel>();
        }

        //Update DBTMCampMaster.
        public virtual DBTMCampMasterViewModel UpdateDBTMCamp(DBTMCampMasterViewModel dBTMCampMasterViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMCampMaster", TraceLevel.Info);
                DBTMCampResponse response = _dBTMCampClient.UpdateDBTMCamp(dBTMCampMasterViewModel.ToModel<DBTMCampMasterModel>());
                DBTMCampMasterModel dBTMCampMasterModel = response?.DBTMCampModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMCampMaster", TraceLevel.Info);
                return IsNotNull(dBTMCampMasterModel) ? dBTMCampMasterModel.ToViewModel<DBTMCampMasterViewModel>() : (DBTMCampMasterViewModel)GetViewModelWithErrorMessage(new DBTMCampMasterViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampMaster", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMCampMasterViewModel)GetViewModelWithErrorMessage(dBTMCampMasterViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMCampMasterViewModel)GetViewModelWithErrorMessage(dBTMCampMasterViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampMaster", TraceLevel.Error);
                return (DBTMCampMasterViewModel)GetViewModelWithErrorMessage(dBTMCampMasterViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Delete DBTMCampMaster.
        public virtual bool DeleteDBTMCamp(string CampCode, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMCampMaster", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _dBTMCampClient.DeleteDBTMCamp(new ParameterModel { Ids = CampCode });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampMaster", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = "ErrorDeleteDBTMCampMaster";
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampMaster", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }

        #region DBTMCampUser
        public virtual DBTMCampUserListViewModel GetDBTMCampUserList(long dBTMCampMasterId, string userType, DataTableViewModel dataTableModel)
        {
            FilterCollection filters = new FilterCollection();
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters.Add("FirstName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("LastName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("EmailId", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("MobileNumber", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);
            DBTMCampUserListResponse response = _dBTMCampClient.GetDBTMCampUserList(dBTMCampMasterId, UserTypeEnum.Trainee.ToString(), null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            DBTMCampUserListModel DBTMCampUserList = new DBTMCampUserListModel { DBTMCampUserList = response?.DBTMCampUserList };
            DBTMCampUserListViewModel listViewModel = new DBTMCampUserListViewModel();
            listViewModel.DBTMCampUserList = DBTMCampUserList?.DBTMCampUserList?.ToViewModel<DBTMCampUserViewModel>().ToList();
            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMCampUserList.Count, BindAssociatedBatchColumns());
            listViewModel.DBTMCampMasterId = dBTMCampMasterId;
            listViewModel.CampName = response.CampName;
            return listViewModel;
        }

        //Update Associate UnAssociate Campwise User.
        public virtual DBTMCampUserViewModel AssociateUnAssociateCampwiseUser(DBTMCampUserViewModel dBTMCampUserViewModel)
        {
            try
            {
                long dBTMCampMasterId = dBTMCampUserViewModel.DBTMCampMasterId;
                long dBTMCampUserId = dBTMCampUserViewModel.DBTMCampUserId;
                dBTMCampUserViewModel.UserType = UserTypeEnum.Trainee.ToString();
                DBTMCampUserResponse response = _dBTMCampClient.AssociateUnAssociateCampwiseUser(dBTMCampUserViewModel.ToModel<DBTMCampUserModel>());
                DBTMCampUserModel dBTMCampUserModel = response?.DBTMCampUserModel;
                dBTMCampUserViewModel = IsNotNull(dBTMCampUserModel) ? dBTMCampUserModel.ToViewModel<DBTMCampUserViewModel>() : new DBTMCampUserViewModel();
                dBTMCampUserViewModel.DBTMCampMasterId = dBTMCampMasterId;
                dBTMCampUserViewModel.DBTMCampUserId = dBTMCampUserId;
                return dBTMCampUserViewModel;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampUser", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMCampUserViewModel)GetViewModelWithErrorMessage(dBTMCampUserViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMCampUserViewModel)GetViewModelWithErrorMessage(dBTMCampUserViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampUser", TraceLevel.Error);
                return (DBTMCampUserViewModel)GetViewModelWithErrorMessage(dBTMCampUserViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }
        #endregion  
        #region DBTMCampUserList
        public virtual DBTMCampUserListViewModel GetCampUserListByCentreCodeAndGeneralTrainerMasterId(string selectedCentreCode, long generalTrainerMasterId, long DBTMCampMasterId)
        {
            DBTMCampUserListResponse response = _dBTMCampClient.GetCampUserListByCentreCodeAndGeneralTrainerMasterId(selectedCentreCode, generalTrainerMasterId, DBTMCampMasterId);
            DBTMCampUserListModel DBTMCampUserList = new DBTMCampUserListModel { DBTMCampUserList = response?.DBTMCampUserList };
            DBTMCampUserListViewModel listViewModel = new DBTMCampUserListViewModel();
            listViewModel.DBTMCampUserList = DBTMCampUserList?.DBTMCampUserList?.ToViewModel<DBTMCampUserViewModel>().ToList();
            return listViewModel;
        }
        #endregion

        #region protected
        protected virtual List<DatatableColumns> BindColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Camp Name",
                ColumnCode = "CampName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Camp Time",
                ColumnCode = "CampTime",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Camp Start Date",
                ColumnCode = "CampStartDate",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Camp End Date",
                ColumnCode = "CampEndDate",
                IsSortable = true,
            });
            return datatableColumnList;
        }
        protected virtual List<DatatableColumns> BindAssociatedBatchColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Image",
                ColumnCode = "Image",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "First Name",
                ColumnCode = "FirstName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Last Name",
                ColumnCode = "LastName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Contact",
                ColumnCode = "MobileNumber",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Is Associated",
                ColumnCode = "DBTMCampUserId",
                IsSortable = true,
            });
            return datatableColumnList;
        }
        #endregion
        #endregion
    }
}
