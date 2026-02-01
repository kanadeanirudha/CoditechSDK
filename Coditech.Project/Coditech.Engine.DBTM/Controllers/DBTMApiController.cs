using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static Coditech.Common.Helper.HelperUtility;

namespace Coditech.Engine.DBTM.Controllers
{
    public class DBTMApiController : BaseController
    {
        private readonly IDBTMApiService _dBTMApiService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMApiController(ICoditechLogging coditechLogging, IDBTMApiService dBTMApiService)
        {
            _dBTMApiService = dBTMApiService;
            _coditechLogging = coditechLogging;
        }

        [Route("/dbtmapi/fileupload")]
        [HttpPost]
        public IActionResult InsertDeviceDataViaFile(IFormFile file)
        {
            try
            {
                bool result = _dBTMApiService.InsertDeviceDataViaFile(file);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = result });
            }
            catch (Exception ex)
            {
                _coditechLogging?.LogMessage(ex, "FileUpload", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [AllowAnonymous]
        [Route("/dbtmapi/healthcheck")]
        [HttpGet]
        public IActionResult HealthCheck()
        {
            return Ok();
        }

        [AllowAnonymous]
        [Route("/dbtmapi/servertime")]
        [HttpGet]
        public IActionResult ServerTime()
        {
            return Ok(new
            {
                serverTime = DateTime.Now
            });
        }

        [Route("/DBTMApi/InsertDeviceData")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public IActionResult InsertDeviceData([FromBody] List<DBTMDeviceDataModel> model)
        {
            try
            {
                bool status = _dBTMApiService.InsertDeviceData(model);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = status });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceData", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceData", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [Route("/DBTMApi/Getbatchlist")]
        [HttpGet]
        [Produces(typeof(DBTMBatchListResponse))]
        public IActionResult GetBatchList(long entityId, string userType)
        {
            try
            {
                List<DBTMBatchModel> list = _dBTMApiService.GetBatchList(entityId, userType);
                return IsNotNull(list) ? CreateOKResponse(new DBTMBatchListResponse { DBTMBatchList = list }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMBatch", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMBatchListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMBatch", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMBatchListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMApi/GetBatchDetails")]
        [HttpGet]
        [Produces(typeof(DBTMBatchResponse))]
        public IActionResult GetBatchDetails(int generalBatchMasterId)
        {
            try
            {
                DBTMBatchModel model = _dBTMApiService.GetBatchDetails(generalBatchMasterId);
                return IsNotNull(model) ? CreateOKResponse(new DBTMBatchResponse { BatchModel = model }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMBatchActivity", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMBatchResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMBatchActivity", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMBatchResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMApi/GetAssignmentList")]
        [HttpGet]
        [Produces(typeof(DBTMTestApiListResponse))]
        public IActionResult GetAssignmentList(long entityId, string userType)
        {
            try
            {
                List<DBTMTestApiModel> list = _dBTMApiService.GetAssignmentList(entityId, userType);
                return IsNotNull(list) ? CreateOKResponse(new DBTMTestApiListResponse { DBTMTestList = list }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMTestApiListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestApiListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMApi/GetAssignmentDetails")]
        [HttpGet]
        [Produces(typeof(DBTMTestApiResponse))]
        public IActionResult GetAssignmentDetails(long dBTMTraineeAssignmentId)
        {
            try
            {
                DBTMTestApiModel model = _dBTMApiService.GetAssignmentDetails(dBTMTraineeAssignmentId);
                return IsNotNull(model) ? CreateOKResponse(new DBTMTestApiResponse { DBTMTestApiModel = model }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestDetails", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMTestApiResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestApiResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/dbtmapi/gettrainerdashboard")]
        [HttpGet]
        [Produces(typeof(DBTMMobileDashboardResponse))]
        public IActionResult GetTrainerDashboard(long userMasterId)
        {
            try
            {
                DBTMMobileDashboardModel model = _dBTMApiService.GetTrainerDashboard(userMasterId);
                return IsNotNull(model) ? CreateOKResponse(new DBTMMobileDashboardResponse { DBTMMobileDashboardModel = model }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMMobileDashboard", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMMobileDashboardResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMMobileDashboard", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMMobileDashboardResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/dbtmapi/gettraineedashboard")]
        [HttpGet]
        [Produces(typeof(DBTMMobileTraineeDashboardResponse))]
        public IActionResult GetTraineeDashboard(long userMasterId)
        {
            try
            {
                DBTMMobileTraineeDashboardModel model = _dBTMApiService.GetTraineeDashboard(userMasterId);
                return IsNotNull(model) ? CreateOKResponse(new DBTMMobileTraineeDashboardResponse { DBTMMobileTraineeDashboardModel = model }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMMobileTraineeDashboard", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMMobileTraineeDashboardResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMMobileTraineeDashboard", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMMobileTraineeDashboardResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/dbtmapi/getjoiningcode")]
        [Produces(typeof(StringResponse))]
        public virtual IActionResult GetJoiningCode(string generalTrainerMasterId)
        {
            try
            {
                string apiDomainkey = _dBTMApiService.GetJoiningCode(generalTrainerMasterId);
                StringResponse response = new StringResponse() { Response = apiDomainkey };
                string data = ApiHelper.ToJson(response);
                return !string.IsNullOrEmpty(apiDomainkey) ? CreateOKResponse<StringResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GetJoiningCode", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new StringResponse { Response = "", ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GetJoiningCode", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new StringResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/dbtmapi/getcentrewisejoiningcode")]
        [Produces(typeof(StringResponse))]
        public virtual IActionResult GetCentreWiseJoiningCode(string centreCode, int joiningCodeTypeEnumId)
        {
            try
            {
                string apiDomainkey = _dBTMApiService.GetCentreWiseJoiningCode(centreCode, joiningCodeTypeEnumId);
                StringResponse response = new StringResponse() { Response = apiDomainkey };
                string data = ApiHelper.ToJson(response);
                return !string.IsNullOrEmpty(apiDomainkey) ? CreateOKResponse<StringResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "getcentrewisejoiningcode", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new StringResponse { Response = "", ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "getcentrewisejoiningcode", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new StringResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [HttpGet]
        [Route("/dbtmapi/Gettraineesbyperformedactivity")]
        [Produces(typeof(DBTMTraineeDetailsListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetTraineesByPerformedActivity(string dBTMTestMasterIds)
        {
            try
            {
                DBTMTraineeDetailsListModel list = _dBTMApiService.GetTraineesByPerformedActivity(dBTMTestMasterIds);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMTraineeDetailsListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTraineeDetailsListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTraineeDetailsListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}