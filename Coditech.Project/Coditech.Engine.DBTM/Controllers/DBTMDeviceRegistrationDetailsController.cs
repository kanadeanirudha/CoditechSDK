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
    public class DBTMDeviceRegistrationDetailsController : BaseController
    {
        private readonly IDBTMDeviceRegistrationDetailsService _dBTMDeviceRegistrationDetailsService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMDeviceRegistrationDetailsController(ICoditechLogging coditechLogging, IDBTMDeviceRegistrationDetailsService dBTMDeviceRegistrationDetailsService)
        {
            _dBTMDeviceRegistrationDetailsService = dBTMDeviceRegistrationDetailsService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/DBTMDeviceRegistrationDetails/GetDBTMDeviceRegistrationDetailsList")]
        [Produces(typeof(DBTMDeviceRegistrationDetailsListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMDeviceRegistrationDetailsList(long UserMasterId, FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMDeviceRegistrationDetailsListModel list = _dBTMDeviceRegistrationDetailsService.GetDBTMDeviceRegistrationDetailsList(UserMasterId, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMDeviceRegistrationDetailsListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceRegistrationDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMDeviceRegistrationDetailsListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceRegistrationDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMDeviceRegistrationDetailsListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMDeviceRegistrationDetails/CreateRegistrationDetails")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMDeviceRegistrationDetailsResponse))]
        public virtual IActionResult CreateRegistrationDetails([FromBody] DBTMDeviceRegistrationDetailsModel model)
        {
            try
            {
                DBTMDeviceRegistrationDetailsModel dBTMDeviceRegistrationDetails = _dBTMDeviceRegistrationDetailsService.CreateRegistrationDetails(model);
                return IsNotNull(dBTMDeviceRegistrationDetails) ? CreateCreatedResponse(new DBTMDeviceRegistrationDetailsResponse { DBTMDeviceRegistrationDetailsModel = dBTMDeviceRegistrationDetails }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceRegistrationDetails", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMDeviceRegistrationDetailsResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceRegistrationDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMDeviceRegistrationDetailsResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMDeviceRegistrationDetails/GetRegistrationDetails")]
        [HttpGet]
        [Produces(typeof(DBTMDeviceRegistrationDetailsResponse))]
        public virtual IActionResult GetDBTMTest(long dBTMDeviceRegistrationDetailId)
        {
            try
            {
                DBTMDeviceRegistrationDetailsModel dBTMDeviceRegistrationDetailsModel = _dBTMDeviceRegistrationDetailsService.GetRegistrationDetails(dBTMDeviceRegistrationDetailId);
                return IsNotNull(dBTMDeviceRegistrationDetailsModel) ? CreateOKResponse(new DBTMDeviceRegistrationDetailsResponse { DBTMDeviceRegistrationDetailsModel = dBTMDeviceRegistrationDetailsModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceRegistrationDetails", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMDeviceRegistrationDetailsResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceRegistrationDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMDeviceRegistrationDetailsResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMDeviceRegistrationDetails/UpdateRegistrationDetails")]
        [HttpPut, ValidateModel]
        [Produces(typeof(DBTMDeviceRegistrationDetailsResponse))]
        public virtual IActionResult UpdateRegistrationDetails([FromBody] DBTMDeviceRegistrationDetailsModel model)
        {
            try
            {
                bool isUpdated = _dBTMDeviceRegistrationDetailsService.UpdateRegistrationDetails(model);
                return isUpdated ? CreateOKResponse(new DBTMDeviceRegistrationDetailsResponse { DBTMDeviceRegistrationDetailsModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceRegistrationDetails", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMDeviceRegistrationDetailsResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceRegistrationDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMDeviceRegistrationDetailsResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMDeviceRegistrationDetails/DeleteRegistrationDetails")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteRegistrationDetails([FromBody] ParameterModel dBTMDeviceRegistrationDetailIds)
        {
            try
            {
                bool deleted = _dBTMDeviceRegistrationDetailsService.DeleteRegistrationDetails(dBTMDeviceRegistrationDetailIds);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceRegistrationDetails", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceRegistrationDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMDeviceRegistrationDetails/GetDeviceSerialCodeByCentreCode")]
        [HttpGet]
        [Produces(typeof(DBTMDeviceRegistrationDetailsListResponse))]
        public virtual IActionResult GetDeviceSerialCodeByCentreCode(string centreCode)
        {
            try
            {
                DBTMDeviceRegistrationDetailsListModel list = _dBTMDeviceRegistrationDetailsService.GetDeviceSerialCodeByCentreCode(centreCode);
                return IsNotNull(list) ? CreateOKResponse(new DBTMDeviceRegistrationDetailsListResponse { RegistrationDetailsList = list.RegistrationDetailsList }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceRegistrationDetails", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMDeviceRegistrationDetailsListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceRegistrationDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMDeviceRegistrationDetailsListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}