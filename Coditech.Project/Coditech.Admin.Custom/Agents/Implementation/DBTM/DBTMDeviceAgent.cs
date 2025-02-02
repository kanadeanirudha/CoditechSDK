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
    public class DBTMDeviceAgent : BaseAgent, IDBTMDeviceAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMDeviceClient _dBTMDeviceClient;
        #endregion

        #region Public Constructor
        public DBTMDeviceAgent(ICoditechLogging coditechLogging, IDBTMDeviceClient dBTMDeviceClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMDeviceClient = GetClient<IDBTMDeviceClient>(dBTMDeviceClient);
        }
        #endregion

        #region Public Methods
        public virtual DBTMDeviceListViewModel GetDBTMDeviceList(DataTableViewModel dataTableModel)
        {
            FilterCollection filters = new FilterCollection();
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters.Add("DeviceName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("DeviceSerialCode", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("RegistrationDate", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }
            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "DeviceName" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            DBTMDeviceListResponse response = _dBTMDeviceClient.List(null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            DBTMDeviceListModel deviceList = new DBTMDeviceListModel { DBTMDeviceList = response?.DBTMDeviceList };
            DBTMDeviceListViewModel listViewModel = new DBTMDeviceListViewModel();
            listViewModel.DBTMDeviceList = deviceList?.DBTMDeviceList?.ToViewModel<DBTMDeviceViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.DBTMDeviceList.Count, BindColumns());
            return listViewModel;
        }

        //Create DBTMDevice.
        public virtual DBTMDeviceViewModel CreateDBTMDevice(DBTMDeviceViewModel dBTMDeviceViewModel)
        {
            try
            {
                DBTMDeviceResponse response = _dBTMDeviceClient.CreateDBTMDevice(dBTMDeviceViewModel.ToModel<DBTMDeviceModel>());
                DBTMDeviceModel dBTMDeviceModel = response?.DBTMDeviceModel;
                return IsNotNull(dBTMDeviceModel) ? dBTMDeviceModel.ToViewModel<DBTMDeviceViewModel>() : new DBTMDeviceViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDevice", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (DBTMDeviceViewModel)GetViewModelWithErrorMessage(dBTMDeviceViewModel, ex.ErrorMessage);
                    default:
                        return (DBTMDeviceViewModel)GetViewModelWithErrorMessage(dBTMDeviceViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDevice", TraceLevel.Error);
                return (DBTMDeviceViewModel)GetViewModelWithErrorMessage(dBTMDeviceViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Get DBTMDevice by dBTMDevice id.
        public virtual DBTMDeviceViewModel GetDBTMDevice(long dBTMDeviceId)
        {
            DBTMDeviceResponse response = _dBTMDeviceClient.GetDBTMDevice(dBTMDeviceId);
            return response?.DBTMDeviceModel.ToViewModel<DBTMDeviceViewModel>();
        }

        //Update DBTMDevice.
        public virtual DBTMDeviceViewModel UpdateDBTMDevice(DBTMDeviceViewModel dBTMDeviceViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMDevice", TraceLevel.Info);
                DBTMDeviceResponse response = _dBTMDeviceClient.UpdateDBTMDevice(dBTMDeviceViewModel.ToModel<DBTMDeviceModel>());
                DBTMDeviceModel dBTMDeviceModel = response?.DBTMDeviceModel;
                _coditechLogging.LogMessage("Agent method execution done.", "DBTMDevice", TraceLevel.Info);
                return IsNotNull(dBTMDeviceModel) ? dBTMDeviceModel.ToViewModel<DBTMDeviceViewModel>() : (DBTMDeviceViewModel)GetViewModelWithErrorMessage(new DBTMDeviceViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDevice", TraceLevel.Error);
                return (DBTMDeviceViewModel)GetViewModelWithErrorMessage(dBTMDeviceViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Delete DBTMDevice.
        public virtual bool DeleteDBTMDevice(string dBTMDeviceIds, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "DBTMDevice", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _dBTMDeviceClient.DeleteDBTMDevice(new ParameterModel { Ids = dBTMDeviceIds });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDevice", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteDBTMDeviceMaster;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDevice", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
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
                ColumnName = "Device Name",
                ColumnCode = "DeviceName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Serial Code",
                ColumnCode = "DeviceSerialCode",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = " DBTM Device Status",
                ColumnCode = "StatusEnumId",
                IsSortable = true,
            }); datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Registration Date",
                ColumnCode = "RegistrationDate",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Is Master Device",
                ColumnCode = "IsMasterDevice",
                IsSortable = true,
            }); datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "IsActive",
                ColumnCode = "IsActive",
                IsSortable = true,
            });
            return datatableColumnList;
        }
        #endregion
    }
}
