using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using static Coditech.Common.Helper.HelperUtility;

namespace Coditech.Engine.DBTM.Controllers
{
    public class DBTMDeviceMasterController : BaseController
    {
        private readonly IDBTMDeviceMasterService _dBTMDeviceMasterService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMDeviceMasterController(ICoditechLogging coditechLogging, IDBTMDeviceMasterService dBTMDeviceMasterService)
        {
            _dBTMDeviceMasterService = dBTMDeviceMasterService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/DBTMDeviceMaster/GetDBTMDeviceList")]
        [Produces(typeof(DBTMDeviceListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMDeviceList(FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMDeviceListModel list = _dBTMDeviceMasterService.GetDBTMDeviceList(filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMDeviceListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDevice", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMDeviceListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDevice", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMDeviceListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMDeviceMaster/CreateDBTMDevice")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMDeviceResponse))]
        public virtual IActionResult CreateDBTMDevice([FromBody] DBTMDeviceModel model)
        {
            try
            {
                DBTMDeviceModel dBTMDevice = _dBTMDeviceMasterService.CreateDBTMDevice(model);
                return IsNotNull(dBTMDevice) ? CreateCreatedResponse(new DBTMDeviceResponse { DBTMDeviceModel = dBTMDevice }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDevice", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMDeviceResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDevice", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMDeviceResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMDeviceMaster/GetDBTMDevice")]
        [HttpGet]
        [Produces(typeof(DBTMDeviceResponse))]
        public virtual IActionResult GetDBTMDevice(long dBTMDeviceId)
        {
            try
            {
                DBTMDeviceModel dBTMDeviceModel = _dBTMDeviceMasterService.GetDBTMDevice(dBTMDeviceId);
                return IsNotNull(dBTMDeviceModel) ? CreateOKResponse(new DBTMDeviceResponse { DBTMDeviceModel = dBTMDeviceModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDevice", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMDeviceResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDevice", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMDeviceResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMDeviceMaster/UpdateDBTMDevice")]
        [HttpPut, ValidateModel]
        [Produces(typeof(DBTMDeviceResponse))]
        public virtual IActionResult UpdateDBTMDevice([FromBody] DBTMDeviceModel model)
        {
            try
            {
                bool isUpdated = _dBTMDeviceMasterService.UpdateDBTMDevice(model);
                return isUpdated ? CreateOKResponse(new DBTMDeviceResponse { DBTMDeviceModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDevice", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMDeviceResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDevice", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMDeviceResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMDeviceMaster/DeleteDBTMDevice")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteDBTMDevice([FromBody] ParameterModel dBTMDeviceIds)
        {
            try
            {
                bool deleted = _dBTMDeviceMasterService.DeleteDBTMDevice(dBTMDeviceIds);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDevice", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDevice", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMDeviceMaster/IsValidDeviceSerialCode")]
        [HttpPut, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult IsValidDeviceSerialCode([FromBody] string deviceSerialCode)
        {
            try
            {
                bool isUpdated = _dBTMDeviceMasterService.IsValidDeviceSerialCode(deviceSerialCode);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = isUpdated });
                
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDevice", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMDeviceResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDevice", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMDeviceResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}