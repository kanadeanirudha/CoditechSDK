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
    public class DBTMTraineeDetailsAgent : BaseAgent, IDBTMTraineeDetailsAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMTraineeDetailsClient _dBTMTraineeDetailsClient;
        private readonly IUserClient _userClient;
        private readonly IGeneralTrainerClient _generalTrainerClient;
        #endregion

        #region Public Constructor
        public DBTMTraineeDetailsAgent(ICoditechLogging coditechLogging, IDBTMTraineeDetailsClient dBTMTraineeDetailsClient, IUserClient userClient, IGeneralTrainerClient generalTrainerClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMTraineeDetailsClient = GetClient<IDBTMTraineeDetailsClient>(dBTMTraineeDetailsClient);
            _userClient = GetClient<IUserClient>(userClient);
            _generalTrainerClient = GetClient<IGeneralTrainerClient>(generalTrainerClient);
        }
        #endregion

        #region Public Methods
        public virtual DBTMTraineeDetailsListViewModel GetDBTMTraineeDetailsList(DataTableViewModel dataTableModel, string listType = null)
        {
            FilterCollection filters = new FilterCollection();
            if (listType == "Active")
            {
                filters.Add("IsActive", ProcedureFilterOperators.Equals, "1");
            }
            else if (listType == "InActive")
            {
                filters.Add("IsActive", ProcedureFilterOperators.Equals, "0");
            }
            dataTableModel = dataTableModel ?? new DataTableViewModel();

            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters.Add("FirstName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("LastName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("EmailId", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("MobileNumber", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("PersonCode", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            DBTMTraineeDetailsListResponse response = _dBTMTraineeDetailsClient.List(dataTableModel.SelectedCentreCode, Convert.ToInt64(string.IsNullOrEmpty(dataTableModel.SelectedParameter1) ? "0" : dataTableModel.SelectedParameter1), null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            DBTMTraineeDetailsListModel dBTMTraineeDetailsList = new DBTMTraineeDetailsListModel { DBTMTraineeDetailsList = response?.DBTMTraineeDetailsList };
            DBTMTraineeDetailsListViewModel listViewModel = new DBTMTraineeDetailsListViewModel();
            listViewModel.DBTMTraineeDetailsList = dBTMTraineeDetailsList?.DBTMTraineeDetailsList?.ToViewModel<DBTMTraineeDetailsViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMTraineeDetailsList.Count, BindColumns());
            return listViewModel;
        }

        #region DBTMTraineeDetails
        //Create DBTMTraineeDetails
        public virtual DBTMTraineeDetailsCreateEditViewModel CreateDBTMTraineeDetails(DBTMTraineeDetailsCreateEditViewModel dBTMTraineeDetailsCreateEditViewModel)
        {
            try
            {
                dBTMTraineeDetailsCreateEditViewModel.UserType = UserTypeEnum.Trainee.ToString();
                GeneralPersonResponse response = _userClient.InsertPersonInformation(dBTMTraineeDetailsCreateEditViewModel.ToModel<GeneralPersonModel>());
                GeneralPersonModel generalPersonModel = response?.GeneralPersonModel;
                return IsNotNull(generalPersonModel) ? generalPersonModel.ToViewModel<DBTMTraineeDetailsCreateEditViewModel>() : new DBTMTraineeDetailsCreateEditViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMTraineeDetailsCreateEditViewModel)GetViewModelWithErrorMessage(dBTMTraineeDetailsCreateEditViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMTraineeDetailsCreateEditViewModel)GetViewModelWithErrorMessage(dBTMTraineeDetailsCreateEditViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Error);
                return (DBTMTraineeDetailsCreateEditViewModel)GetViewModelWithErrorMessage(dBTMTraineeDetailsCreateEditViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Get DBTM Trainee Details by personId.
        public virtual DBTMTraineeDetailsCreateEditViewModel GetDBTMTraineePersonalDetails(long dBTMTraineeDetailId, long personId)
        {
            GeneralPersonResponse response = _userClient.GetPersonInformation(personId);
            DBTMTraineeDetailsCreateEditViewModel dBTMTraineeDetailsCreateEditViewModel = response?.GeneralPersonModel.ToViewModel<DBTMTraineeDetailsCreateEditViewModel>();
            if (IsNotNull(dBTMTraineeDetailsCreateEditViewModel))
            {
                DBTMTraineeDetailsResponse dBTMTraineeDetailsResponse = _dBTMTraineeDetailsClient.GetDBTMTraineeOtherDetails(dBTMTraineeDetailId);
                if (IsNotNull(dBTMTraineeDetailsResponse))
                {
                    dBTMTraineeDetailsCreateEditViewModel.SelectedCentreCode = dBTMTraineeDetailsResponse.DBTMTraineeDetailsModel.CentreCode;
                }
                dBTMTraineeDetailsCreateEditViewModel.DBTMTraineeDetailId = dBTMTraineeDetailId;
                dBTMTraineeDetailsCreateEditViewModel.PersonId = personId;
            }
            return dBTMTraineeDetailsCreateEditViewModel;
        }

        //Update DBTM Trainee Details 
        public virtual DBTMTraineeDetailsCreateEditViewModel UpdateDBTMTraineePersonalDetails(DBTMTraineeDetailsCreateEditViewModel dBTMTraineeDetailsCreateEditViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMTraineeDetails", TraceLevel.Info);
                GeneralPersonModel generalPersonModel = dBTMTraineeDetailsCreateEditViewModel.ToModel<GeneralPersonModel>();
                generalPersonModel.EntityId = dBTMTraineeDetailsCreateEditViewModel.DBTMTraineeDetailId;
                generalPersonModel.UserType = UserTypeEnum.Trainee.ToString();
                GeneralPersonResponse response = _userClient.UpdatePersonInformation(generalPersonModel);
                generalPersonModel = response?.GeneralPersonModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMTraineeDetails", TraceLevel.Info);
                return IsNotNull(generalPersonModel) ? generalPersonModel.ToViewModel<DBTMTraineeDetailsCreateEditViewModel>() : (DBTMTraineeDetailsCreateEditViewModel)GetViewModelWithErrorMessage(new DBTMTraineeDetailsCreateEditViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Error);
                return (DBTMTraineeDetailsCreateEditViewModel)GetViewModelWithErrorMessage(dBTMTraineeDetailsCreateEditViewModel, GeneralResources.UpdateErrorMessage);
            }
        }
        #endregion

        #region Member Other Details
        //Get Member Other Details
        public virtual DBTMTraineeDetailsViewModel GetDBTMTraineeOtherDetails(long dBTMTraineeDetailId)
        {
            DBTMTraineeDetailsResponse response = _dBTMTraineeDetailsClient.GetDBTMTraineeOtherDetails(dBTMTraineeDetailId);
            DBTMTraineeDetailsViewModel dBTMTraineeDetailsViewModel = response?.DBTMTraineeDetailsModel.ToViewModel<DBTMTraineeDetailsViewModel>();
            return dBTMTraineeDetailsViewModel;
        }

        //Update DBTM Trainee Details.
        public virtual DBTMTraineeDetailsViewModel UpdateDBTMTraineeOtherDetails(DBTMTraineeDetailsViewModel dBTMTraineeDetailsViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMTraineeDetails", TraceLevel.Info);
                DBTMTraineeDetailsResponse response = _dBTMTraineeDetailsClient.UpdateDBTMTraineeOtherDetails(dBTMTraineeDetailsViewModel.ToModel<DBTMTraineeDetailsModel>());
                DBTMTraineeDetailsModel dBTMTraineeDetailsModel = response?.DBTMTraineeDetailsModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMTraineeDetails", TraceLevel.Info);
                return IsNotNull(dBTMTraineeDetailsModel) ? dBTMTraineeDetailsModel.ToViewModel<DBTMTraineeDetailsViewModel>() : (DBTMTraineeDetailsViewModel)GetViewModelWithErrorMessage(new DBTMTraineeDetailsViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Error);
                return (DBTMTraineeDetailsViewModel)GetViewModelWithErrorMessage(dBTMTraineeDetailsViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Delete DBTM Trainee Details .
        public virtual bool DeleteDBTMTraineeDetails(string dBTMTraineeDetailIds, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMTraineeDetails", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _dBTMTraineeDetailsClient.DeleteDBTMTraineeDetails(new ParameterModel { Ids = dBTMTraineeDetailIds });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteDBTMTraineeDetails;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }
        #endregion

        #region TraineeAssociatedToTrainer
        public virtual GeneralTraineeAssociatedToTrainerListViewModel GetAssociatedTrainerList(long dBTMTraineeDetailId, long personId, DataTableViewModel dataTableModel)
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

            GeneralTraineeAssociatedToTrainerListResponse response = _generalTrainerClient.GetAssociatedTrainerList(null, 0, true, dBTMTraineeDetailId, UserTypeEnum.Trainee.ToString(), personId, null, filters, sortlist, dataTableModel.PageIndex, int.MaxValue);
            GeneralTraineeAssociatedToTrainerListModel associatedTrainerList = new GeneralTraineeAssociatedToTrainerListModel { AssociatedTrainerList = response?.AssociatedTrainerList };
            GeneralTraineeAssociatedToTrainerListViewModel listViewModel = new GeneralTraineeAssociatedToTrainerListViewModel();
            listViewModel.AssociatedTrainerList = associatedTrainerList?.AssociatedTrainerList?.ToViewModel<GeneralTraineeAssociatedToTrainerViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.AssociatedTrainerList.Count, BindAssociatedTraineeColumns());
            listViewModel.DBTMTraineeDetailId = dBTMTraineeDetailId;
            listViewModel.PersonId = personId;
            listViewModel.FirstName = response.FirstName;
            listViewModel.LastName = response.LastName;
            listViewModel.IsEntityActive = response.IsEntityActive;
            return listViewModel;
        }

        //Get AssociatedTrainer by general Trainer id.
        public virtual GeneralTraineeAssociatedToTrainerViewModel AssociatedTrainer(long dBTMTraineeDetailId, long personId)
        {
            GeneralPersonResponse response = _userClient.GetPersonInformation(personId);
            GeneralTraineeAssociatedToTrainerViewModel generalTraineeAssociatedToTrainerViewModel = new GeneralTraineeAssociatedToTrainerViewModel()
            {
                FirstName = response.GeneralPersonModel.FirstName,
                LastName = response.GeneralPersonModel.LastName,
                EntityId = dBTMTraineeDetailId,
                PersonId = personId,
                UserType = UserTypeEnum.Trainee.ToString()
            };
            return generalTraineeAssociatedToTrainerViewModel;
        }

        //Insert AssociatedTrainer
        public virtual GeneralTraineeAssociatedToTrainerViewModel InsertAssociatedTrainer(GeneralTraineeAssociatedToTrainerViewModel generalTraineeAssociatedToTrainerViewModel)
        {
            try
            {
                long personId = generalTraineeAssociatedToTrainerViewModel.PersonId;
                long dBTMTraineeDetailId = generalTraineeAssociatedToTrainerViewModel.EntityId;
                generalTraineeAssociatedToTrainerViewModel.UserType = UserTypeEnum.Trainee.ToString();
                GeneralTraineeAssociatedToTrainerResponse response = _generalTrainerClient.InsertAssociatedTrainer(generalTraineeAssociatedToTrainerViewModel.ToModel<GeneralTraineeAssociatedToTrainerModel>());
                GeneralTraineeAssociatedToTrainerModel generalTraineeAssociatedToTrainerModel = response?.GeneralTraineeAssociatedToTrainerModel;
                generalTraineeAssociatedToTrainerViewModel = IsNotNull(generalTraineeAssociatedToTrainerModel) ? generalTraineeAssociatedToTrainerModel.ToViewModel<GeneralTraineeAssociatedToTrainerViewModel>() : new GeneralTraineeAssociatedToTrainerViewModel();
                generalTraineeAssociatedToTrainerViewModel.PersonId = personId;
                generalTraineeAssociatedToTrainerViewModel.EntityId = dBTMTraineeDetailId;
                return generalTraineeAssociatedToTrainerViewModel;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.AssociatedTrainer.ToString(), TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (GeneralTraineeAssociatedToTrainerViewModel)GetViewModelWithErrorMessage(generalTraineeAssociatedToTrainerViewModel, ex.ErrorMessage);
                    default:
                        return (GeneralTraineeAssociatedToTrainerViewModel)GetViewModelWithErrorMessage(generalTraineeAssociatedToTrainerViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.AssociatedTrainer.ToString(), TraceLevel.Error);
                return (GeneralTraineeAssociatedToTrainerViewModel)GetViewModelWithErrorMessage(generalTraineeAssociatedToTrainerViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Get AssociatedTrainer by general Trainer id.
        public virtual GeneralTraineeAssociatedToTrainerViewModel GetAssociatedTrainer(long generalTraineeAssociatedToTrainerId)
        {
            GeneralTraineeAssociatedToTrainerResponse response = _generalTrainerClient.GetAssociatedTrainer(generalTraineeAssociatedToTrainerId);
            return response?.GeneralTraineeAssociatedToTrainerModel.ToViewModel<GeneralTraineeAssociatedToTrainerViewModel>();
        }

        //Update  AssociatedTrainer.
        public virtual GeneralTraineeAssociatedToTrainerViewModel UpdateAssociatedTrainer(GeneralTraineeAssociatedToTrainerViewModel generalTraineeAssociatedToTrainerViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", CoditechLoggingEnum.Components.AssociatedTrainer.ToString(), TraceLevel.Info);
                GeneralTraineeAssociatedToTrainerResponse response = _generalTrainerClient.UpdateAssociatedTrainer(generalTraineeAssociatedToTrainerViewModel.ToModel<GeneralTraineeAssociatedToTrainerModel>());
                GeneralTraineeAssociatedToTrainerModel generalTraineeAssociatedToTrainerModel = response?.GeneralTraineeAssociatedToTrainerModel;
                _coditechLogging.LogMessage("Agent method execution done.", CoditechLoggingEnum.Components.AssociatedTrainer.ToString(), TraceLevel.Info);
                return HelperUtility.IsNotNull(generalTraineeAssociatedToTrainerModel) ? generalTraineeAssociatedToTrainerModel.ToViewModel<GeneralTraineeAssociatedToTrainerViewModel>() : (GeneralTraineeAssociatedToTrainerViewModel)GetViewModelWithErrorMessage(new GeneralTraineeAssociatedToTrainerViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.AssociatedTrainer.ToString(), TraceLevel.Error);
                return (GeneralTraineeAssociatedToTrainerViewModel)GetViewModelWithErrorMessage(generalTraineeAssociatedToTrainerViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Delete Associated Trainer Details .
        public virtual bool DeleteAssociatedTrainer(string generalTraineeAssociatedToTrainerIds, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", CoditechLoggingEnum.Components.AssociatedTrainer.ToString(), TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _generalTrainerClient.DeleteAssociatedTrainer(new ParameterModel { Ids = generalTraineeAssociatedToTrainerIds });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.AssociatedTrainer.ToString(), TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteAssociatedTrainer;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.AssociatedTrainer.ToString(), TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }
        #endregion

        #region DBTMTraineeActivities

        public virtual DBTMActivitiesListViewModel GetTraineeActivitiesList(string personCode, int numberOfDaysRecord, DataTableViewModel dataTableModel)
        {
            FilterCollection filters = new FilterCollection();
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters.Add("TestName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("DeviceSerialCode", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            DBTMActivitiesListResponse response = _dBTMTraineeDetailsClient.GetTraineeActivitiesList(personCode, numberOfDaysRecord, null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            DBTMActivitiesListModel dBTMActivitiesList = new DBTMActivitiesListModel { ActivitiesList = response?.ActivitiesList };
            DBTMActivitiesListViewModel listViewModel = new DBTMActivitiesListViewModel();
            listViewModel.ActivitiesList = dBTMActivitiesList?.ActivitiesList?.ToViewModel<DBTMActivitiesViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.ActivitiesList.Count, BindTraineeActivitiesColumns());
            listViewModel.FirstName = response.FirstName;
            listViewModel.LastName = response.LastName;
            listViewModel.SelectedCentreCode = response.SelectedCentreCode;
            return listViewModel;
        }

        public virtual DBTMActivitiesDetailsListViewModel GetTraineeActivitiesDetailsList(long dBTMDeviceDataId, DataTableViewModel dataTableModel)
        {
            FilterCollection filters = new FilterCollection();
            dataTableModel = dataTableModel ?? new DataTableViewModel();

            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters.Add("Time", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("Distance", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("Angle", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("Force", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("Acceleration", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortList = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            DBTMActivitiesDetailsListResponse response = _dBTMTraineeDetailsClient.GetTraineeActivitiesDetailsList(dBTMDeviceDataId, null, filters, sortList, dataTableModel.PageIndex, int.MaxValue);
           
            DBTMActivitiesDetailsListViewModel listViewModel = new DBTMActivitiesDetailsListViewModel
            {
                DataTable = response.DataTable
            };

            listViewModel.DBTMDeviceDataId = dBTMDeviceDataId;
            listViewModel.FirstName = response.FirstName;
            listViewModel.LastName = response.LastName;
            listViewModel.PersonCode = response.PersonCode;
            listViewModel.TestName = response.TestName;
            return listViewModel;
        }
        #endregion

        #region protected
        protected virtual List<DatatableColumns> BindColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Image",
                ColumnCode = "Image",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Person Code",
                ColumnCode = "PersonCode",
                IsSortable = true,
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
                ColumnName = "Email Id",
                ColumnCode = "EmailId",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Number of Activity Performed",
                ColumnCode = "NumberOfActivityPerformed",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "IsActive",
                ColumnCode = "IsActive",
                IsSortable = true,
            });
            return datatableColumnList;
        }

        protected virtual List<DatatableColumns> BindAssociatedTraineeColumns()
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
                ColumnName = "Email Id",
                ColumnCode = "EmailId",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Current Trainer",
                ColumnCode = "IsCurrentTrainer",
                IsSortable = true,
            });
            return datatableColumnList;
        }

        protected virtual List<DatatableColumns> BindTraineeActivitiesColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();

            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Test Name",
                ColumnCode = "TestName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Date",
                ColumnCode = "CreatedDate",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Serial Code",
                ColumnCode = "DeviceSerialCode",
                IsSortable = true,
            });
            return datatableColumnList;
        }
        #endregion
    }
}
#endregion