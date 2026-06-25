using Coditech.API.Data;
using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Logger;
using DocumentFormat.OpenXml.Drawing;
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
        [Produces(typeof(CustomTrueFalseResponse))]
        public IActionResult InsertDeviceData([FromBody] List<DBTMDeviceDataModel> model)
        {
            try
            {
                bool status = _dBTMApiService.InsertDeviceData(model);
                string DataUniqueIds = null;
                if (status)
                {
                    DataUniqueIds = string.Join(",",model.Where(x => !string.IsNullOrEmpty(x.DataUniqueId)).Select(x => x.DataUniqueId));
                }
                return CreateOKResponse(new CustomTrueFalseResponse { IsSuccess = status, DUI = DataUniqueIds });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceData", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new CustomTrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceData", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new CustomTrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMApi/InsertDeviceDataV2")]
        [HttpPost, ValidateModel]
        [Produces(typeof(CustomTrueFalseResponse))]
        public IActionResult InsertDeviceDataV2()
        {
            using var reader = new StreamReader(Request.Body);

            string rawJson = reader.ReadToEnd();

            if (string.IsNullOrWhiteSpace(rawJson))
            {
                return BadRequest(new CustomTrueFalseResponse
                {
                    HasError = true,
                    ErrorMessage = "Request body is empty."
                });
            }

            try
            {
                bool status = _dBTMApiService.InsertDeviceDataV2(rawJson);
                return CreateOKResponse(new CustomTrueFalseResponse { IsSuccess = status });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex.Message, "DBTMDeviceDataV2", TraceLevel.Warning, rawJson, "Application");
                return CreateInternalServerErrorResponse(new CustomTrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex.Message, "DBTMDeviceDataV2", TraceLevel.Error, rawJson, "Application");
                return CreateInternalServerErrorResponse(new CustomTrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [Route("/DBTMApi/Getbatchlist")]
        [HttpGet]
        [Produces(typeof(DBTMBatchListResponse))]
        public IActionResult GetBatchList(long entityId, string userType, bool isCheckTestPerformed = true)
        {
            try
            {
                List<DBTMBatchModel> list = _dBTMApiService.GetBatchList(entityId, userType, isCheckTestPerformed);
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

        [Route("/DBTMApi/GetBatchAndActivityWiseUserDetails")]
        [HttpGet]
        [Produces(typeof(DBTMBatchUserResponse))]
        public IActionResult GetBatchAndActivityWiseUserDetails(int generalBatchMasterId, int dbtmTestMasterId)
        {
            try
            {
                List<DBTMGeneralBatchUserModel> model = _dBTMApiService.GetBatchAndActivityWiseUserDetails(generalBatchMasterId, dbtmTestMasterId);
                return IsNotNull(model) ? CreateOKResponse(new DBTMBatchUserResponse { DBTMBatchUserList = model }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMBatchActivity", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMBatchUserResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMBatchActivity", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMBatchUserResponse { HasError = true, ErrorMessage = ex.Message });
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
        [Produces(typeof(OrganisationCentrewiseJoiningCodeResponse))]
        public virtual IActionResult GetJoiningCode(string generalTrainerMasterId)
        {
            try
            {
                OrganisationCentrewiseJoiningCodeModel model = _dBTMApiService.GetJoiningCode(generalTrainerMasterId);
                return IsNotNull(model) ? CreateOKResponse(new OrganisationCentrewiseJoiningCodeResponse { OrganisationCentrewiseJoiningCodeModel = model }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GetJoiningCode", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new OrganisationCentrewiseJoiningCodeResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GetJoiningCode", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new OrganisationCentrewiseJoiningCodeResponse { HasError = true, ErrorMessage = ex.Message });
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
        public virtual IActionResult GetTraineesByPerformedActivity(string dBTMTestMasterIds, string centreCode, long generalTrainerMasterId)
        {
            try
            {
                DBTMTraineeDetailsListModel list = _dBTMApiService.GetTraineesByPerformedActivity(dBTMTestMasterIds, centreCode, generalTrainerMasterId);
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

        [HttpGet]
        [Route("/dbtmapi/Getactivitiesbytrainee")]
        [Produces(typeof(DBTMTestListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetactivitiesBytrainee(long selectedTraineeId)
        {
            try
            {
                DBTMTestListModel list = _dBTMApiService.GetactivitiesBytrainee(selectedTraineeId);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMTestListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMApi/GetCampList")]
        [HttpGet]
        [Produces(typeof(DBTMBatchListResponse))]
        public IActionResult GetCampList(long entityId, string userType)
        {
            try
            {
                List<DBTMBatchModel> list = _dBTMApiService.GetCampList(entityId, userType);
                return IsNotNull(list) ? CreateOKResponse(new DBTMBatchListResponse { DBTMBatchList = list }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCamp", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMBatchListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCamp", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMBatchListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [Route("/DBTMApi/GetCampDetails")]
        [HttpGet]
        [Produces(typeof(DBTMBatchResponse))]
        public IActionResult GetCampDetails(int dBTMCampMasterId)
        {
            try
            {
                DBTMBatchModel model = _dBTMApiService.GetCampDetails(dBTMCampMasterId);
                return IsNotNull(model) ? CreateOKResponse(new DBTMBatchResponse { BatchModel = model }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampActivity", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMBatchResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampActivity", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMBatchResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [Route("/DBTMApi/GetCampAndActivityWiseUserDetails")]
        [HttpGet]
        [Produces(typeof(DBTMBatchUserResponse))]
        public IActionResult GetCampAndActivityWiseUserDetails(int dBTMcampMasterId, int dbtmTestMasterId, string userType)
        {
            try
            {
                List<DBTMGeneralBatchUserModel> model = _dBTMApiService.GetCampAndActivityWiseUserDetails(dBTMcampMasterId, dbtmTestMasterId, userType);
                return IsNotNull(model) ? CreateOKResponse(new DBTMBatchUserResponse { DBTMBatchUserList = model }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampActivity", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMBatchUserResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampActivity", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMBatchUserResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [Route("/DBTMApi/UpdateValidRecord")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public IActionResult UpdateValidRecord(long dBTMDeviceDataId, bool isValidRecord)
        {
            try
            {
                bool status = _dBTMApiService.UpdateValidRecord(dBTMDeviceDataId, isValidRecord);

                return CreateOKResponse(new TrueFalseResponse
                {
                    IsSuccess = status
                });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "ValidRecord", TraceLevel.Warning);

                return CreateInternalServerErrorResponse(new TrueFalseResponse
                {
                    HasError = true,
                    ErrorMessage = ex.Message,
                    ErrorCode = ex.ErrorCode
                });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "ValidRecord", TraceLevel.Error);

                return CreateInternalServerErrorResponse(new TrueFalseResponse
                {
                    HasError = true,
                    ErrorMessage = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("/DBTMApi/GetDBTMCentrAndTrainerewiseBatchList")]
        [Produces(typeof(DBTMBatchListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMCentrAndTrainerewiseBatchList(string centreCode, int joiningCodeTypeEnumId, long generalTrainerMasterId)
        {
            try
            {
                DBTMBatchListModel list = _dBTMApiService.GetDBTMCentrAndTrainerewiseBatchList(centreCode, joiningCodeTypeEnumId, generalTrainerMasterId);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMBatchListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCentrAndTrainerewiseBatchList", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMBatchListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCentrAndTrainerewiseBatchList", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMBatchListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [HttpGet]
        [Route("/DBTMApi/GetDBTMTrainerwiseBatchList")]
        [Produces(typeof(DBTMBatchListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMTrainerwiseBatchList(string centreCode, long generalTrainerMasterId)
        {
            try
            {
                DBTMBatchListModel list = _dBTMApiService.GetDBTMTrainerwiseBatchList(centreCode, generalTrainerMasterId);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMBatchListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTrainerewiseBatchList", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMBatchListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTrainerewiseBatchList", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMBatchListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [Route("/DBTMApi/GetDBTMTraineeDetails")]
        [HttpGet]
        [Produces(typeof(DBTMTraineeDetailsResponse))]
        public virtual IActionResult GetDBTMTraineeDetails(long dBTMTraineeDetailId, long personId)
        {
            try
            {
                DBTMTraineeDetailsModel dBTMTraineeDetailsModel = _dBTMApiService.GetDBTMTraineeDetails(dBTMTraineeDetailId, personId);
                return IsNotNull(dBTMTraineeDetailsModel) ? CreateOKResponse(new DBTMTraineeDetailsResponse { DBTMTraineeDetailsModel = dBTMTraineeDetailsModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMTraineeDetailsResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTraineeDetailsResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTraineeDetails/UpdateDBTMTraineeDetails")]
        [HttpPut, ValidateModel]
        [Produces(typeof(DBTMTraineeDetailsResponse))]
        public virtual IActionResult UpdateDBTMTraineeDetails([FromBody] DBTMTraineeDetailsModel model)
        {
            try
            {
                bool isUpdated = _dBTMApiService.UpdateDBTMTraineeDetails(model);
                return isUpdated ? CreateOKResponse(new DBTMTraineeDetailsResponse { DBTMTraineeDetailsModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMTraineeDetailsResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTraineeDetailsResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

    }
}