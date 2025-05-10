using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Resources;
using System.Collections.Specialized;
using System.Data;

using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMDeviceMasterService : IDBTMDeviceMasterService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMDeviceMaster> _dBTMDeviceMasterRepository;
        public DBTMDeviceMasterService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMDeviceMasterRepository = new CoditechRepository<DBTMDeviceMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        public virtual DBTMDeviceListModel GetDBTMDeviceList(long dBTMParentDeviceMasterId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMDeviceModel> objStoredProc = new CoditechViewRepository<DBTMDeviceModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@DBTMParentDeviceMasterId", dBTMParentDeviceMasterId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMDeviceModel> dBTMDeviceList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMDeviceList @DBTMParentDeviceMasterId,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 5, out pageListModel.TotalRowCount)?.ToList();
            DBTMDeviceListModel listModel = new DBTMDeviceListModel();

            listModel.DBTMDeviceList = dBTMDeviceList?.Count > 0 ? dBTMDeviceList : new List<DBTMDeviceModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }

        //Create DBTMDevice.
        public virtual DBTMDeviceModel CreateDBTMDevice(DBTMDeviceModel dBTMDeviceModel)
        {
           

            if (IsNull(dBTMDeviceModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);
            dBTMDeviceModel.DBTMParentDeviceMasterId = dBTMDeviceModel.IsMasterDevice ? 0 : dBTMDeviceModel.DBTMParentDeviceMasterId;
            DBTMDeviceMaster dBTMDeviceMaster = dBTMDeviceModel.FromModelToEntity<DBTMDeviceMaster>();

            //Create new DBTMDevice and return it.
            DBTMDeviceMaster dBTMDeviceData = _dBTMDeviceMasterRepository.Insert(dBTMDeviceMaster);
            if (dBTMDeviceData?.DBTMDeviceMasterId > 0)
            {
                dBTMDeviceModel.DBTMDeviceMasterId = dBTMDeviceData.DBTMDeviceMasterId;
            }
            else
            {
                dBTMDeviceModel.HasError = true;
                dBTMDeviceModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return dBTMDeviceModel;
        }

        //Get DBTMDevice by dBTMDevice id.
        public virtual DBTMDeviceModel GetDBTMDevice(long dBTMDeviceId)
        {
            if (dBTMDeviceId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMDeviceId"));

            //Get the DBTMDevice Details based on id.
            DBTMDeviceMaster dBTMDeviceMaster = _dBTMDeviceMasterRepository.Table.FirstOrDefault(x => x.DBTMDeviceMasterId == dBTMDeviceId);
            DBTMDeviceModel dBTMDeviceModel = dBTMDeviceMaster?.FromEntityToModel<DBTMDeviceModel>();
            return dBTMDeviceModel;
        }

        //Update DBTMDevice.
        public virtual bool UpdateDBTMDevice(DBTMDeviceModel dBTMDeviceModel)
        {
            if (IsNull(dBTMDeviceModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (dBTMDeviceModel.DBTMDeviceMasterId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMDeviceMasterID"));

            dBTMDeviceModel.DBTMParentDeviceMasterId = dBTMDeviceModel.IsMasterDevice ? 0 : dBTMDeviceModel.DBTMParentDeviceMasterId;
            DBTMDeviceMaster dBTMDeviceMaster = dBTMDeviceModel.FromModelToEntity<DBTMDeviceMaster>();

            //Update DBTMDevice
            bool isdBTMDeviceUpdated = _dBTMDeviceMasterRepository.Update(dBTMDeviceMaster);
            if (!isdBTMDeviceUpdated)
            {
                dBTMDeviceModel.HasError = true;
                dBTMDeviceModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isdBTMDeviceUpdated;
        }

        //Delete DBTMDevice.
        public virtual bool DeleteDBTMDevice(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMDeviceID"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("DBTMDeviceId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteDBTMDevice @DBTMDeviceId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        public virtual bool IsValidDeviceSerialCode(string deviceSerialCode)
        {
            if (string.IsNullOrWhiteSpace(deviceSerialCode))
            {
                throw new ArgumentException("Device Serial Code cannot be null or empty");
            }

            // Return true if the device code exists in the repository, false otherwise
            return _dBTMDeviceMasterRepository.Table
                .Any(x => x.DeviceSerialCode == deviceSerialCode);
        }

        public DBTMDeviceMaster GetDBTMDeviceMasterDetailsByCode(string deviceSerialCode)
       => _dBTMDeviceMasterRepository.Table.Where(x => x.DeviceSerialCode == deviceSerialCode && x.IsActive).FirstOrDefault();
        
        #region Protected Method

        #endregion
    }
}
