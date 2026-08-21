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
    public class DBTMGraphAgent : BaseAgent, IDBTMGraphAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMGraphClient _dBTMGraphClient;
        #endregion

        #region Public Constructor
        public DBTMGraphAgent(ICoditechLogging coditechLogging, IDBTMGraphClient dBTMGraphClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMGraphClient = GetClient<IDBTMGraphClient>(dBTMGraphClient);
        }
        #endregion

        #region Public Methods
        public virtual DBTMGraphMasterListViewModel GetDBTMGraphList(DataTableViewModel dataTableModel)
        {
            FilterCollection filters = null;
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters = new FilterCollection();
                filters.Add("GraphName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("GraphCode", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("TestCode", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("GraphType", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "GraphName" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            DBTMGraphMasterListResponse response = _dBTMGraphClient.List(null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            DBTMGraphMasterListModel dBTMGraphList = new DBTMGraphMasterListModel { DBTMGraphMasterList = response?.DBTMGraphMasterList };
            DBTMGraphMasterListViewModel listViewModel = new DBTMGraphMasterListViewModel();
            listViewModel.DBTMGraphMasterList = dBTMGraphList?.DBTMGraphMasterList?.ToViewModel<DBTMGraphMasterViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMGraphMasterList.Count, BindColumns());
            return listViewModel;
        }

        //Create DBTM Graph.
        public virtual DBTMGraphMasterViewModel CreateDBTMGraph(DBTMGraphMasterViewModel dBTMGraphMasterViewModel)
        {
            try
            {
                DBTMGraphMasterResponse response = _dBTMGraphClient.CreateDBTMGraph(dBTMGraphMasterViewModel.ToModel<DBTMGraphMasterModel>());
                DBTMGraphMasterModel dBTMGraphMasterModel = response?.DBTMGraphMasterModel;
                return IsNotNull(dBTMGraphMasterModel) ? dBTMGraphMasterModel.ToViewModel<DBTMGraphMasterViewModel>() : new DBTMGraphMasterViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraphMaster", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMGraphMasterViewModel)GetViewModelWithErrorMessage(dBTMGraphMasterViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMGraphMasterViewModel)GetViewModelWithErrorMessage(dBTMGraphMasterViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraphMaster", TraceLevel.Error);
                return (DBTMGraphMasterViewModel)GetViewModelWithErrorMessage(dBTMGraphMasterViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Get DBTM Graph by DBTM graph master id.
        public virtual DBTMGraphMasterViewModel GetDBTMGraph(string graphCode)
        {
            DBTMGraphMasterResponse response = _dBTMGraphClient.GetDBTMGraph(graphCode);
            return response?.DBTMGraphMasterModel.ToViewModel<DBTMGraphMasterViewModel>();
        }

        //Update DBTMGraphMaster.
        public virtual DBTMGraphMasterViewModel UpdateDBTMGraph(DBTMGraphMasterViewModel dBTMGraphMasterViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMGraphMaster", TraceLevel.Info);
                DBTMGraphMasterResponse response = _dBTMGraphClient.UpdateDBTMGraph(dBTMGraphMasterViewModel.ToModel<DBTMGraphMasterModel>());
                DBTMGraphMasterModel dBTMGraphMasterModel = response?.DBTMGraphMasterModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMGraphMaster", TraceLevel.Info);
                return IsNotNull(dBTMGraphMasterModel) ? dBTMGraphMasterModel.ToViewModel<DBTMGraphMasterViewModel>() : (DBTMGraphMasterViewModel)GetViewModelWithErrorMessage(new DBTMGraphMasterViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraphMaster", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMGraphMasterViewModel)GetViewModelWithErrorMessage(dBTMGraphMasterViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMGraphMasterViewModel)GetViewModelWithErrorMessage(dBTMGraphMasterViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraphMaster", TraceLevel.Error);
                return (DBTMGraphMasterViewModel)GetViewModelWithErrorMessage(dBTMGraphMasterViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Delete DBTMGraphMaster.
        public virtual bool DeleteDBTMGraph(string graphCode, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMGraphMaster", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _dBTMGraphClient.DeleteDBTMGraph(new ParameterModel { Ids = graphCode });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraphMaster", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteDBTMGraphMaster;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraphMaster", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }

        //TestCode
        public virtual DBTMTestListViewModel DBTMGraphTestCode()
        {
            DBTMTestListResponse response = _dBTMGraphClient.GetDBTMGraphTestCode();
            DBTMTestListModel dBTMGraphMasterList = new DBTMTestListModel { DBTMTestList = response?.DBTMTestList };
            DBTMTestListViewModel listViewModel = new DBTMTestListViewModel();
            listViewModel.DBTMTestList = dBTMGraphMasterList?.DBTMTestList?.ToViewModel<DBTMTestViewModel>().ToList();
            return listViewModel;
        }
        #endregion

        //Get Graph Vertical View Sequence
        public virtual DBTMGraphVerticalViewSequenceListViewModel GetGraphVerticalViewSequenceList(int dBTMGraphMasterId, DataTableViewModel dataTableModel)
        {
            FilterCollection filters = new FilterCollection();
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            DBTMGraphVerticalViewSequenceListResponse response = _dBTMGraphClient.GetGraphVerticalViewSequenceList(dBTMGraphMasterId, null, null, null, null, int.MaxValue);
            DBTMGraphVerticalViewSequenceListModel graphVerticalViewSequenceList = new DBTMGraphVerticalViewSequenceListModel { DBTMGraphVerticalViewSequenceList = response?.DBTMGraphVerticalViewSequenceList };
            DBTMGraphVerticalViewSequenceListViewModel listViewModel = new DBTMGraphVerticalViewSequenceListViewModel();
            listViewModel.DBTMGraphVerticalViewSequenceList = graphVerticalViewSequenceList?.DBTMGraphVerticalViewSequenceList?.ToViewModel<DBTMGraphVerticalViewSequenceViewModel>().ToList();
            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMGraphVerticalViewSequenceList.Count, BindGraphVerticalViewSequenceColumns());
            listViewModel.DBTMGraphMasterId = dBTMGraphMasterId;
            listViewModel.GraphName = response?.GraphName;
            listViewModel.GraphCode = response?.GraphCode;
            return listViewModel;
        }

        public virtual DBTMGraphVerticalViewSequenceViewModel GetGraphVerticalViewSequence(int dBTMGraphVerticalViewSequenceId)
        {
            DBTMGraphVerticalViewSequenceResponse response = _dBTMGraphClient.GetGraphVerticalViewSequence(dBTMGraphVerticalViewSequenceId);
            return response?.DBTMGraphVerticalViewSequenceModel?.ToViewModel<DBTMGraphVerticalViewSequenceViewModel>();
        }

        //Update Graph Vertical View Sequence
        public virtual DBTMGraphVerticalViewSequenceViewModel UpdateGraphVerticalViewSequence(DBTMGraphVerticalViewSequenceViewModel model)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMGraph", TraceLevel.Info);
                DBTMGraphVerticalViewSequenceResponse response = _dBTMGraphClient.UpdateGraphVerticalViewSequence(model.ToModel<DBTMGraphVerticalViewSequenceModel>());
                DBTMGraphVerticalViewSequenceModel graphVerticalViewSequenceModel = response?.DBTMGraphVerticalViewSequenceModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMGraph", TraceLevel.Info);
                return IsNotNull(graphVerticalViewSequenceModel) ? graphVerticalViewSequenceModel.ToViewModel<DBTMGraphVerticalViewSequenceViewModel>() : (DBTMGraphVerticalViewSequenceViewModel)GetViewModelWithErrorMessage(new DBTMGraphVerticalViewSequenceViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraph", TraceLevel.Error);
                return (DBTMGraphVerticalViewSequenceViewModel)GetViewModelWithErrorMessage(model, GeneralResources.UpdateErrorMessage);
            }
        }

        //Update Graph Vertical Sequence Number
        public virtual DBTMGraphVerticalViewSequenceViewModel UpdateGraphVerticalSequenceNumber(DBTMGraphVerticalViewSequenceViewModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.DBTMSequenceData))
                {
                    List<DBTMGraphVerticalViewSequenceModel> graphVerticalViewSequenceList = JsonConvert.DeserializeObject<List<DBTMGraphVerticalViewSequenceModel>>(model.DBTMSequenceData);
                    model.DBTMGraphVerticalViewSequenceList = graphVerticalViewSequenceList;
                }

                DBTMGraphVerticalViewSequenceResponse response = _dBTMGraphClient.UpdateGraphVerticalSequenceNumber(model.ToModel<DBTMGraphVerticalViewSequenceModel>());
                DBTMGraphVerticalViewSequenceModel graphVerticalViewSequenceModel = response?.DBTMGraphVerticalViewSequenceModel;
                return IsNotNull(graphVerticalViewSequenceModel) ? graphVerticalViewSequenceModel.ToViewModel<DBTMGraphVerticalViewSequenceViewModel>() : new DBTMGraphVerticalViewSequenceViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraph", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMGraphVerticalViewSequenceViewModel)GetViewModelWithErrorMessage(model, ex.ErrorMessage);
                    default:
                        return (DBTMGraphVerticalViewSequenceViewModel)GetViewModelWithErrorMessage(model, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraph", TraceLevel.Error);
                return (DBTMGraphVerticalViewSequenceViewModel)GetViewModelWithErrorMessage(model, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Create Graph Vertical View Sequence
        public virtual DBTMGraphVerticalViewSequenceViewModel CreateGraphVerticalViewSequence(DBTMGraphVerticalViewSequenceViewModel model)
        {
            try
            {
                DBTMGraphVerticalViewSequenceResponse response = _dBTMGraphClient.CreateGraphVerticalViewSequence(model.ToModel<DBTMGraphVerticalViewSequenceModel>());
                DBTMGraphVerticalViewSequenceModel graphVerticalViewSequenceModel = response?.DBTMGraphVerticalViewSequenceModel;
                return IsNotNull(graphVerticalViewSequenceModel) ? graphVerticalViewSequenceModel.ToViewModel<DBTMGraphVerticalViewSequenceViewModel>() : new DBTMGraphVerticalViewSequenceViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraph", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist: return (DBTMGraphVerticalViewSequenceViewModel)GetViewModelWithErrorMessage(model, ex.ErrorMessage);
                    default:
                        return (DBTMGraphVerticalViewSequenceViewModel)GetViewModelWithErrorMessage(model, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraph", TraceLevel.Error);
                return (DBTMGraphVerticalViewSequenceViewModel)GetViewModelWithErrorMessage(model, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Delete Graph Vertical View Sequence
        public virtual bool DeleteGraphVerticalViewSequence(string dBTMGraphVerticalViewSequenceIds, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMGraph", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _dBTMGraphClient.DeleteGraphVerticalViewSequence(new ParameterModel { Ids = dBTMGraphVerticalViewSequenceIds });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraph", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteDBTMGraphMaster;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraph", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }

        #region protected
        protected virtual List<DatatableColumns> BindColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Graph Name",
                ColumnCode = "GraphName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Graph Code",
                ColumnCode = "GraphCode",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Graph Mode",
                ColumnCode = "GraphMode",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "X Parameter",
                ColumnCode = "XParameter",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Y Parameter",
                ColumnCode = "YParameter",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Graph Type",
                ColumnCode = "GraphType",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Graph Size",
                ColumnCode = "GraphSize",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Active",
                ColumnCode = "IsActive",
                IsSortable = true,
            });
            return datatableColumnList;
        }

        protected virtual List<DatatableColumns> BindGraphVerticalViewSequenceColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns
            {
                ColumnName = "Parameter Code",
                ColumnCode = "ParameterCode"
            });
            datatableColumnList.Add(new DatatableColumns
            {
                ColumnName = "Column Name",
                ColumnCode = "ColumnName"
            });
            datatableColumnList.Add(new DatatableColumns
            {
                ColumnName = "Display On",
                ColumnCode = "DisplayOn"
            });
            datatableColumnList.Add(new DatatableColumns
            {
                ColumnName = "Sequence Number",
                ColumnCode = "SequenceNumber"
            });
            datatableColumnList.Add(new DatatableColumns
            {
                ColumnName = "Recursion",
                ColumnCode = "Recursion"
            });
            datatableColumnList.Add(new DatatableColumns
            {
                ColumnName = "Calculated Parameter",
                ColumnCode = "IsCalculatedParameter"
            });
            return datatableColumnList;
        }
        #endregion
    }
}
