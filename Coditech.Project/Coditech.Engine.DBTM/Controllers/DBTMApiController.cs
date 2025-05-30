using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Logger;
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

        [Route("/DBTMDeviceData/InsertDeviceData")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult InsertDeviceData([FromBody] List<DBTMDeviceDataModel> model)
        {
            try
            {
                bool deleted = _dBTMApiService.InsertDeviceData(model);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
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
        [Route("/dbtmapi/getbatchlist")]
        [HttpGet]
        [Produces(typeof(DBTMBatchListResponse))]
        public virtual IActionResult GetBatchList(long entityId, string userType)
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
        public virtual IActionResult GetBatchDetails(int generalBatchMasterId)
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
        public virtual IActionResult GetAssignmentList(long entityId, string userType)
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
        public virtual IActionResult GetAssignmentDetails(long dBTMTraineeAssignmentId)
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
    }
}