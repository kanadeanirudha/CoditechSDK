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
    public class DBTMTestAgent : BaseAgent, IDBTMTestAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMTestClient _dBTMTestClient;
        #endregion

        #region Public Constructor
        public DBTMTestAgent(ICoditechLogging coditechLogging, IDBTMTestClient dBTMTestClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMTestClient = GetClient<IDBTMTestClient>(dBTMTestClient);
        }
        #endregion

        #region Public Methods
        public virtual DBTMTestListViewModel GetDBTMTestList(DataTableViewModel dataTableModel)
        {
            FilterCollection filters = new FilterCollection();
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters.Add("TestName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("TestCode", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("DBTMTestMasterId", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("PerformanceMatrix", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }
            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            DBTMTestListResponse response = _dBTMTestClient.List(null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            DBTMTestListModel dBTMTestList = new DBTMTestListModel { DBTMTestList = response?.DBTMTestList };
            DBTMTestListViewModel listViewModel = new DBTMTestListViewModel();
            listViewModel.DBTMTestList = dBTMTestList?.DBTMTestList?.ToViewModel<DBTMTestViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMTestList.Count, BindColumns());
            return listViewModel;
        }

        //Create DBTMTest.
        public virtual DBTMTestViewModel CreateDBTMTest(DBTMTestViewModel dBTMTestViewModel)
        {
            try
            {
                DBTMTestResponse response = _dBTMTestClient.CreateDBTMTest(dBTMTestViewModel.ToModel<DBTMTestModel>());
                DBTMTestModel dBTMTestModel = response?.DBTMTestModel;
                return IsNotNull(dBTMTestModel) ? dBTMTestModel.ToViewModel<DBTMTestViewModel>() : new DBTMTestViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMTestViewModel)GetViewModelWithErrorMessage(dBTMTestViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMTestViewModel)GetViewModelWithErrorMessage(dBTMTestViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return (DBTMTestViewModel)GetViewModelWithErrorMessage(dBTMTestViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Get DBTMTest by dBTMTestMaster id.
        public virtual DBTMTestViewModel GetDBTMTest(int dBTMTestMasterId)
        {
            DBTMTestResponse response = _dBTMTestClient.GetDBTMTest(dBTMTestMasterId);
            return response?.DBTMTestModel.ToViewModel<DBTMTestViewModel>();
        }

        //Update DBTMTest.
        public virtual DBTMTestViewModel UpdateDBTMTest(DBTMTestViewModel dBTMTestViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMTest", TraceLevel.Info);
                DBTMTestResponse response = _dBTMTestClient.UpdateDBTMTest(dBTMTestViewModel.ToModel<DBTMTestModel>());
                DBTMTestModel dBTMTestModel = response?.DBTMTestModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMTest", TraceLevel.Info);
                return IsNotNull(dBTMTestModel) ? dBTMTestModel.ToViewModel<DBTMTestViewModel>() : (DBTMTestViewModel)GetViewModelWithErrorMessage(new DBTMTestViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return (DBTMTestViewModel)GetViewModelWithErrorMessage(dBTMTestViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Delete DBTMTest.
        public virtual bool DeleteDBTMTest(string dBTMTestMasterIds, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMTest", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _dBTMTestClient.DeleteDBTMTest(new ParameterModel { Ids = dBTMTestMasterIds });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteDBTMTestMaster;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }

        public virtual DBTMGraphMasterListViewModel DBTMGraph(int dBTMTestMasterId)
        {
            DBTMGraphMasterListResponse response = _dBTMTestClient.GetDBTMGraph(dBTMTestMasterId);
            DBTMGraphMasterListModel dBTMGraphMasterList = new DBTMGraphMasterListModel { DBTMGraphMasterList = response?.DBTMGraphMasterList };
            DBTMGraphMasterListViewModel listViewModel = new DBTMGraphMasterListViewModel();
            listViewModel.DBTMGraphMasterList = dBTMGraphMasterList?.DBTMGraphMasterList?.ToViewModel<DBTMGraphMasterViewModel>().ToList();
            return listViewModel;
        }

        //Get Activity List View Sequence
        public virtual DBTMActivityListViewSequenceListViewModel GetActivityListViewSequenceList(int dBTMTestMasterId, DataTableViewModel dataTableModel)
        {
            FilterCollection filters = new FilterCollection();
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            DBTMActivityListViewSequenceListResponse response = _dBTMTestClient.GetActivityListViewSequenceList(dBTMTestMasterId, null, null, null, null, int.MaxValue);
            DBTMActivityListViewSequenceListModel associatedTrainerList = new DBTMActivityListViewSequenceListModel { DBTMActivityListViewSequenceList = response?.DBTMActivityListViewSequenceList };
            DBTMActivityListViewSequenceListViewModel listViewModel = new DBTMActivityListViewSequenceListViewModel();
            listViewModel.DBTMActivityListViewSequenceList = associatedTrainerList?.DBTMActivityListViewSequenceList?.ToViewModel<DBTMActivityListViewSequenceViewModel>().ToList();
            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMActivityListViewSequenceList.Count, BindActivityListViewSequenceColumns());
            listViewModel.DBTMTestMasterId = dBTMTestMasterId;
            listViewModel.TestName = response.TestName;
            return listViewModel;
        }

        public virtual DBTMActivityListViewSequenceViewModel GetActivityListViewSequence(int dBTMTestParameterListViewSequenceId)
        {
            DBTMActivityListViewSequenceResponse response = _dBTMTestClient.GetActivityListViewSequence(dBTMTestParameterListViewSequenceId);
            return response?.DBTMActivityListViewSequenceModel.ToViewModel<DBTMActivityListViewSequenceViewModel>();
        }

        //Update Activity List View Sequence
        public virtual DBTMActivityListViewSequenceViewModel UpdateActivityListViewSequence(DBTMActivityListViewSequenceViewModel dBTMTestViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMTest", TraceLevel.Info);
                DBTMActivityListViewSequenceResponse response = _dBTMTestClient.UpdateActivityListViewSequence(dBTMTestViewModel.ToModel<DBTMActivityListViewSequenceModel>());
                DBTMActivityListViewSequenceModel dBTMTestModel = response?.DBTMActivityListViewSequenceModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMTest", TraceLevel.Info);
                return IsNotNull(dBTMTestModel) ? dBTMTestModel.ToViewModel<DBTMActivityListViewSequenceViewModel>() : (DBTMActivityListViewSequenceViewModel)GetViewModelWithErrorMessage(new DBTMActivityListViewSequenceViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return (DBTMActivityListViewSequenceViewModel)GetViewModelWithErrorMessage(dBTMTestViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Update Sequence Number
        public virtual DBTMActivityListViewSequenceViewModel UpdateSequenceNumber(DBTMActivityListViewSequenceViewModel dBTMActivityListViewSequenceViewModel)
        {
            try
            {
                if (!string.IsNullOrEmpty(dBTMActivityListViewSequenceViewModel.DBTMSequenceData))
                {
                    List<DBTMActivityListViewSequenceModel> dBTMActivityListViewSequenceList = JsonConvert.DeserializeObject<List<DBTMActivityListViewSequenceModel>>(dBTMActivityListViewSequenceViewModel.DBTMSequenceData);
                    dBTMActivityListViewSequenceViewModel.DBTMActivityListViewSequenceList = dBTMActivityListViewSequenceList;
                }

                DBTMActivityListViewSequenceResponse response = _dBTMTestClient.UpdateSequenceNumber(dBTMActivityListViewSequenceViewModel.ToModel<DBTMActivityListViewSequenceModel>());
                DBTMActivityListViewSequenceModel dBTMActivityListViewSequenceModel = response?.DBTMActivityListViewSequenceModel;
                return IsNotNull(dBTMActivityListViewSequenceModel) ? dBTMActivityListViewSequenceModel.ToViewModel<DBTMActivityListViewSequenceViewModel>() : new DBTMActivityListViewSequenceViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMActivityListViewSequenceViewModel)GetViewModelWithErrorMessage(dBTMActivityListViewSequenceViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMActivityListViewSequenceViewModel)GetViewModelWithErrorMessage(dBTMActivityListViewSequenceViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return (DBTMActivityListViewSequenceViewModel)GetViewModelWithErrorMessage(dBTMActivityListViewSequenceViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Create Activity List View Sequence
        public virtual DBTMActivityListViewSequenceViewModel CreateActivityListViewSequence(DBTMActivityListViewSequenceViewModel dBTMActivityListViewSequenceViewModel)
        {
            try
            {
                DBTMActivityListViewSequenceResponse response = _dBTMTestClient.CreateActivityListViewSequence(dBTMActivityListViewSequenceViewModel.ToModel<DBTMActivityListViewSequenceModel>());
                DBTMActivityListViewSequenceModel gymWorkoutPlanModel = response?.DBTMActivityListViewSequenceModel;
                return IsNotNull(gymWorkoutPlanModel) ? gymWorkoutPlanModel.ToViewModel<DBTMActivityListViewSequenceViewModel>() : new DBTMActivityListViewSequenceViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMActivityListViewSequenceViewModel)GetViewModelWithErrorMessage(dBTMActivityListViewSequenceViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMActivityListViewSequenceViewModel)GetViewModelWithErrorMessage(dBTMActivityListViewSequenceViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return (DBTMActivityListViewSequenceViewModel)GetViewModelWithErrorMessage(dBTMActivityListViewSequenceViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Delete ActivityListViewSequence.
        public virtual bool DeleteActivityListViewSequence(string dBTMTestParameterListViewSequenceIds, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMTest", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _dBTMTestClient.DeleteActivityListViewSequence(new ParameterModel { Ids = dBTMTestParameterListViewSequenceIds });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteDBTMTestMaster;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }

        #region Activity Vertical View Sequence 

        //Get Activity Vertical View Sequence
        public virtual DBTMActivityVerticalViewSequenceListViewModel GetActivityVerticalViewSequenceList(int dBTMTestMasterId, DataTableViewModel dataTableModel)
        {
            FilterCollection filters = new FilterCollection();
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            DBTMActivityVerticalViewSequenceListResponse response = _dBTMTestClient.GetActivityVerticalViewSequenceList(dBTMTestMasterId, null, null, null, null, int.MaxValue);
            DBTMActivityVerticalViewSequenceListModel associatedTrainerList = new DBTMActivityVerticalViewSequenceListModel { DBTMActivityVerticalViewSequenceList = response?.DBTMActivityVerticalViewSequenceList };
            DBTMActivityVerticalViewSequenceListViewModel listViewModel = new DBTMActivityVerticalViewSequenceListViewModel();
            listViewModel.DBTMActivityVerticalViewSequenceList = associatedTrainerList?.DBTMActivityVerticalViewSequenceList?.ToViewModel<DBTMActivityVerticalViewSequenceViewModel>().ToList();
            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMActivityVerticalViewSequenceList.Count, BindActivityVerticalViewSequenceColumns());
            listViewModel.DBTMTestMasterId = dBTMTestMasterId;
            listViewModel.TestName = response.TestName;
            return listViewModel;
        }

        public virtual DBTMActivityVerticalViewSequenceViewModel GetActivityVerticalViewSequence(int dBTMTestParameterListViewSequenceId)
        {
            DBTMActivityVerticalViewSequenceResponse response = _dBTMTestClient.GetActivityVerticalViewSequence(dBTMTestParameterListViewSequenceId);
            return response?.DBTMActivityVerticalViewSequenceModel.ToViewModel<DBTMActivityVerticalViewSequenceViewModel>();
        }

        //Update Activity Vertical View Sequence
        public virtual DBTMActivityVerticalViewSequenceViewModel UpdateActivityVerticalViewSequence(DBTMActivityVerticalViewSequenceViewModel dBTMTestViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMTest", TraceLevel.Info);
                DBTMActivityVerticalViewSequenceResponse response = _dBTMTestClient.UpdateActivityVerticalViewSequence(dBTMTestViewModel.ToModel<DBTMActivityVerticalViewSequenceModel>());
                DBTMActivityVerticalViewSequenceModel dBTMTestModel = response?.DBTMActivityVerticalViewSequenceModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMTest", TraceLevel.Info);
                return IsNotNull(dBTMTestModel) ? dBTMTestModel.ToViewModel<DBTMActivityVerticalViewSequenceViewModel>() : (DBTMActivityVerticalViewSequenceViewModel)GetViewModelWithErrorMessage(new DBTMActivityVerticalViewSequenceViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return (DBTMActivityVerticalViewSequenceViewModel)GetViewModelWithErrorMessage(dBTMTestViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Update Vertical Sequence Number
        public virtual DBTMActivityVerticalViewSequenceViewModel UpdateVerticalSequenceNumber(DBTMActivityVerticalViewSequenceViewModel dBTMActivityVerticalViewSequenceViewModel)
        {
            try
            {
                if (!string.IsNullOrEmpty(dBTMActivityVerticalViewSequenceViewModel.DBTMSequenceData))
                {
                    List<DBTMActivityVerticalViewSequenceModel> dBTMActivityVerticalViewSequenceList = JsonConvert.DeserializeObject<List<DBTMActivityVerticalViewSequenceModel>>(dBTMActivityVerticalViewSequenceViewModel.DBTMSequenceData);
                    dBTMActivityVerticalViewSequenceViewModel.DBTMActivityVerticalViewSequenceList = dBTMActivityVerticalViewSequenceList;
                }

                DBTMActivityVerticalViewSequenceResponse response = _dBTMTestClient.UpdateVerticalSequenceNumber(dBTMActivityVerticalViewSequenceViewModel.ToModel<DBTMActivityVerticalViewSequenceModel>());
                DBTMActivityVerticalViewSequenceModel dBTMActivityVerticalViewSequenceModel = response?.DBTMActivityVerticalViewSequenceModel;
                return IsNotNull(dBTMActivityVerticalViewSequenceModel) ? dBTMActivityVerticalViewSequenceModel.ToViewModel<DBTMActivityVerticalViewSequenceViewModel>() : new DBTMActivityVerticalViewSequenceViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMActivityVerticalViewSequenceViewModel)GetViewModelWithErrorMessage(dBTMActivityVerticalViewSequenceViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMActivityVerticalViewSequenceViewModel)GetViewModelWithErrorMessage(dBTMActivityVerticalViewSequenceViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return (DBTMActivityVerticalViewSequenceViewModel)GetViewModelWithErrorMessage(dBTMActivityVerticalViewSequenceViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Create Activity Vertical View Sequence
        public virtual DBTMActivityVerticalViewSequenceViewModel CreateActivityVerticalViewSequence(DBTMActivityVerticalViewSequenceViewModel dBTMActivityVerticalViewSequenceViewModel)
        {
            try
            {
                DBTMActivityVerticalViewSequenceResponse response = _dBTMTestClient.CreateActivityVerticalViewSequence(dBTMActivityVerticalViewSequenceViewModel.ToModel<DBTMActivityVerticalViewSequenceModel>());
                DBTMActivityVerticalViewSequenceModel gymWorkoutPlanModel = response?.DBTMActivityVerticalViewSequenceModel;
                return IsNotNull(gymWorkoutPlanModel) ? gymWorkoutPlanModel.ToViewModel<DBTMActivityVerticalViewSequenceViewModel>() : new DBTMActivityVerticalViewSequenceViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMActivityVerticalViewSequenceViewModel)GetViewModelWithErrorMessage(dBTMActivityVerticalViewSequenceViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMActivityVerticalViewSequenceViewModel)GetViewModelWithErrorMessage(dBTMActivityVerticalViewSequenceViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return (DBTMActivityVerticalViewSequenceViewModel)GetViewModelWithErrorMessage(dBTMActivityVerticalViewSequenceViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Delete ActivityVerticalViewSequence.
        public virtual bool DeleteActivityVerticalViewSequence(string dBTMTestParameterVerticalViewSequenceIds, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMTest", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _dBTMTestClient.DeleteActivityVerticalViewSequence(new ParameterModel { Ids = dBTMTestParameterVerticalViewSequenceIds });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteDBTMTestMaster;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }
        #endregion
        public virtual DBTMTestWisePerformanceStandardListViewModel DBTMTestWisePerformanceStandardList(int dBTMTestMasterId)
        {
            DBTMTestWisePerformanceStandardListResponse response = _dBTMTestClient.GetDBTMTestWisePerformanceStandardList(dBTMTestMasterId);
            DBTMTestWisePerformanceStandardListModel listModel = new DBTMTestWisePerformanceStandardListModel { DBTMTestWisePerformanceStandardList = response?.DBTMTestWisePerformanceStandardList };
            DBTMTestWisePerformanceStandardListViewModel listViewModel = new DBTMTestWisePerformanceStandardListViewModel();
            listViewModel.DBTMTestWisePerformanceStandardList = listModel?.DBTMTestWisePerformanceStandardList ?.ToViewModel<DBTMTestWisePerformanceStandardViewModel>().ToList();
            listViewModel.DBTMTestMasterId = dBTMTestMasterId;
            listViewModel.TestName = response.TestName;
            return listViewModel;
        }
        public virtual DBTMTestWisePerformanceStandardViewModel CreateDBTMTestWisePerformanceStandard(DBTMTestWisePerformanceStandardViewModel dBTMTestWisePerformanceStandardViewModel)
        {
            try
            {
                DBTMTestWisePerformanceStandardResponse response = _dBTMTestClient.CreateDBTMTestWisePerformanceStandard(dBTMTestWisePerformanceStandardViewModel.ToModel<DBTMTestWisePerformanceStandardModel>());
                DBTMTestWisePerformanceStandardModel model = response?.DBTMTestWisePerformanceStandardModel;
                return IsNotNull(model) ? model.ToViewModel<DBTMTestWisePerformanceStandardViewModel>() : new DBTMTestWisePerformanceStandardViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMTestWisePerformanceStandardViewModel) GetViewModelWithErrorMessage(dBTMTestWisePerformanceStandardViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMTestWisePerformanceStandardViewModel) GetViewModelWithErrorMessage(dBTMTestWisePerformanceStandardViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return (DBTMTestWisePerformanceStandardViewModel) GetViewModelWithErrorMessage(dBTMTestWisePerformanceStandardViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }
        public virtual DBTMTestWisePerformanceStandardViewModel UpdateDBTMTestWisePerformanceStandard(DBTMTestWisePerformanceStandardViewModel dBTMTestWisePerformanceStandardViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMTest", TraceLevel.Info);
                DBTMTestWisePerformanceStandardResponse response = _dBTMTestClient.UpdateDBTMTestWisePerformanceStandard(dBTMTestWisePerformanceStandardViewModel.ToModel<DBTMTestWisePerformanceStandardModel>());
                DBTMTestWisePerformanceStandardModel model = response?.DBTMTestWisePerformanceStandardModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMTest", TraceLevel.Info);
                return IsNotNull(model) ? model.ToViewModel<DBTMTestWisePerformanceStandardViewModel>() : (DBTMTestWisePerformanceStandardViewModel) GetViewModelWithErrorMessage( new DBTMTestWisePerformanceStandardViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return (DBTMTestWisePerformanceStandardViewModel) GetViewModelWithErrorMessage(dBTMTestWisePerformanceStandardViewModel, GeneralResources.UpdateErrorMessage);
            }
        }
        public virtual DBTMCentreWiseTestListViewModel GetTestsByCentreCode(string centreCode)
        {
            DBTMCentreWiseTestListResponse response = _dBTMTestClient.GetTestsByCentreCode(centreCode);
            DBTMCentreWiseTestListViewModel listViewModel = new DBTMCentreWiseTestListViewModel();
            listViewModel.DBTMCentreWiseTestList = response?.DBTMCentreWiseTestList?.ToViewModel<DBTMCentreWiseTestViewModel>().ToList();
            return listViewModel;
        }
        #endregion

        #region protected
        protected virtual List<DatatableColumns> BindColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Device Activity Code",
                ColumnCode = "DBTMTestMasterId",
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
                ColumnName = "Test Code",
                ColumnCode = "TestCode",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Performance Matrix",
                ColumnCode = "PerformanceMatrix",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Minimun Paired Device",
                ColumnCode = "MinimunPairedDevice",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Lap Distance",
                ColumnCode = "LapDistance",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Is Lap Distance Change",
                ColumnCode = "IsLapDistanceChange",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Is Multi Test",
                ColumnCode = "IsMultiTest",
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

        protected virtual List<DatatableColumns> BindActivityListViewSequenceColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Parameter Code",
                ColumnCode = "ParameterCode",
            });
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
                ColumnName = "Sequence Number",
                ColumnCode = "SequenceNumber",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Recursion",
                ColumnCode = "Recursion",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Consecutive Parameter Code",
                ColumnCode = "ConsecutiveParameterCode",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Is Calculated Parameter",
                ColumnCode = "IsCalculatedParameter",
            });
            return datatableColumnList;
        }

        protected virtual List<DatatableColumns> BindActivityVerticalViewSequenceColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Parameter Code",
                ColumnCode = "ParameterCode",
            });
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
                ColumnName = "Sequence Number",
                ColumnCode = "SequenceNumber",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Recursion",
                ColumnCode = "Recursion",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Is Calculated Parameter",
                ColumnCode = "IsCalculatedParameter",
            });
            return datatableColumnList;
        }
        #endregion
    }
}
