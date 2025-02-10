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
    public class DBTMTraineeAssignmentAgent : BaseAgent, IDBTMTraineeAssignmentAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMTraineeAssignmentClient _dBTMTraineeAssignmentClient;
        #endregion

        #region Public Constructor
        public DBTMTraineeAssignmentAgent(ICoditechLogging coditechLogging, IDBTMTraineeAssignmentClient dBTMTraineeAssignmentClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMTraineeAssignmentClient = GetClient<IDBTMTraineeAssignmentClient>(dBTMTraineeAssignmentClient);
        }
        #endregion

        #region Public Methods
        public virtual DBTMTraineeAssignmentListViewModel GetDBTMTraineeAssignmentList(DataTableViewModel dataTableModel)
        {
            FilterCollection filters = new FilterCollection();
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters.Add("FirstName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("LastName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("TestName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("AssignmentDate", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("AssignmentTime", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }
            
            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            DBTMTraineeAssignmentListResponse response = _dBTMTraineeAssignmentClient.List(Convert.ToInt64(dataTableModel.SelectedParameter1), null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            DBTMTraineeAssignmentListModel deviceList = new DBTMTraineeAssignmentListModel { DBTMTraineeAssignmentList = response?.DBTMTraineeAssignmentList };
            DBTMTraineeAssignmentListViewModel listViewModel = new DBTMTraineeAssignmentListViewModel();
            listViewModel.DBTMTraineeAssignmentList = deviceList?.DBTMTraineeAssignmentList?.ToViewModel<DBTMTraineeAssignmentViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMTraineeAssignmentList.Count, BindColumns());
            return listViewModel;
        }

        //Create DBTMTraineeAssignment.
        public virtual DBTMTraineeAssignmentViewModel CreateDBTMTraineeAssignment(DBTMTraineeAssignmentViewModel dBTMTraineeAssignmentViewModel)
        {
            try
            {
                DBTMTraineeAssignmentResponse response = _dBTMTraineeAssignmentClient.CreateDBTMTraineeAssignment(dBTMTraineeAssignmentViewModel.ToModel<DBTMTraineeAssignmentModel>());
                DBTMTraineeAssignmentModel dBTMTraineeAssignmentModel = response?.DBTMTraineeAssignmentModel;
                return IsNotNull(dBTMTraineeAssignmentModel) ? dBTMTraineeAssignmentModel.ToViewModel<DBTMTraineeAssignmentViewModel>() : new DBTMTraineeAssignmentViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeAssignment", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMTraineeAssignmentViewModel)GetViewModelWithErrorMessage(dBTMTraineeAssignmentViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMTraineeAssignmentViewModel)GetViewModelWithErrorMessage(dBTMTraineeAssignmentViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeAssignment", TraceLevel.Error);
                return (DBTMTraineeAssignmentViewModel)GetViewModelWithErrorMessage(dBTMTraineeAssignmentViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Get DBTMTraineeAssignment by dBTMTraineeAssignment id.
        public virtual DBTMTraineeAssignmentViewModel GetDBTMTraineeAssignment(long dBTMTraineeAssignmentId)
        {
            DBTMTraineeAssignmentResponse response = _dBTMTraineeAssignmentClient.GetDBTMTraineeAssignment(dBTMTraineeAssignmentId);
            return response?.DBTMTraineeAssignmentModel.ToViewModel<DBTMTraineeAssignmentViewModel>();
        }

        //Update DBTMTraineeAssignment.
        public virtual DBTMTraineeAssignmentViewModel UpdateDBTMTraineeAssignment(DBTMTraineeAssignmentViewModel dBTMTraineeAssignmentViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMTraineeAssignment", TraceLevel.Info);
                DBTMTraineeAssignmentResponse response = _dBTMTraineeAssignmentClient.UpdateDBTMTraineeAssignment(dBTMTraineeAssignmentViewModel.ToModel<DBTMTraineeAssignmentModel>());
                DBTMTraineeAssignmentModel dBTMTraineeAssignmentModel = response?.DBTMTraineeAssignmentModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMTraineeAssignment", TraceLevel.Info);
                return IsNotNull(dBTMTraineeAssignmentModel) ? dBTMTraineeAssignmentModel.ToViewModel<DBTMTraineeAssignmentViewModel>() : (DBTMTraineeAssignmentViewModel)GetViewModelWithErrorMessage(new DBTMTraineeAssignmentViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeAssignment", TraceLevel.Error);
                return (DBTMTraineeAssignmentViewModel)GetViewModelWithErrorMessage(dBTMTraineeAssignmentViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Delete DBTMTraineeAssignment.
        public virtual bool DeleteDBTMTraineeAssignment(string dBTMTraineeAssignmentIds, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMTraineeAssignment", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _dBTMTraineeAssignmentClient.DeleteDBTMTraineeAssignment(new ParameterModel { Ids = dBTMTraineeAssignmentIds });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeAssignment", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteDeleteDBTMTraineeAssignment;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeAssignment", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }

        //Send Reminder Assignment.
        public virtual bool SendAssignmentReminder(string dBTMTraineeAssignmentId, out string errorMessage)
        {
            errorMessage = "ErrorFailedToSendReminder";

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMTraineeAssignment", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _dBTMTraineeAssignmentClient.SendAssignmentReminder(dBTMTraineeAssignmentId);
                return trueFalseResponse.IsSuccess;
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeAssignment", TraceLevel.Error);
                errorMessage = "ErrorFailedToSendReminder";
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
                ColumnName = "Test Name",
                ColumnCode = "TestName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Date",
                ColumnCode = "AssignmentDate",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Time",
                ColumnCode = "AssignmentTime",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Test Status",
                ColumnCode = "DBTMTestStatusEnumId",
                IsSortable = true,
            });
            return datatableColumnList;
        }
        #endregion

    }
}
