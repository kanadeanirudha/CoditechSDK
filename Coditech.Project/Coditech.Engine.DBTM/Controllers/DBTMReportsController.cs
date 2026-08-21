using Coditech.API.Data;
using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Exceptions;
using Coditech.Common.Logger;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.Engine.DBTM.Controllers
{
    public class DBTMReportsController : BaseController
    {
        private readonly IDBTMReportsService _dBTMReportsService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMReportsController(ICoditechLogging coditechLogging, IDBTMReportsService dBTMReportsService)
        {
            _dBTMReportsService = dBTMReportsService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/DBTMReports/BatchWiseReports")]
        [Produces(typeof(DBTMBatchWiseReportsListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult BatchWiseReports(int generalBatchMasterId, int dBTMTestMasterId, string FromDate, string ToDate, bool isMobileRequest)
        {
            try
            {
                DBTMReportsListModel list = _dBTMReportsService.BatchWiseReports(generalBatchMasterId, dBTMTestMasterId, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), isMobileRequest, false);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMBatchWiseReportsListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMBatchWiseReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMBatchWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMBatchWiseReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMBatchWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMReports/TestWiseReports")]
        [Produces(typeof(DBTMTestWiseReportsListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, string fromDate, string toDate, long entityId, string userType, string centreCode, bool isMobileRequest)
        {
            try
            {
                DBTMReportsListModel list = _dBTMReportsService.TestWiseReports(dBTMTestMasterId, dBTMTraineeDetailId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), entityId, userType, centreCode, isMobileRequest);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMTestWiseReportsListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestWiseReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestWiseReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMReports/TestWiseGraphReports")]
        [Produces(typeof(GraphResponse))]
        public virtual IActionResult TestWiseGraphReports(int dBTMTestMasterId, long dBTMTraineeDetailId, int dBTMGraphMasterId, string graphMode, string fromDate, string toDate, long entityId, string userType, string centreCode, bool isMobileRequest, string typeOfRecord = "Batch")
        {
            try
            {
                GraphModel model = _dBTMReportsService.TestWiseGraphReports(dBTMTestMasterId, dBTMTraineeDetailId, dBTMGraphMasterId, graphMode, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), entityId, userType, centreCode, isMobileRequest, typeOfRecord);
                return IsNotNull(model) ? CreateOKResponse(new GraphResponse { GraphModel = model }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestWiseGraphReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GraphResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestWiseGraphReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GraphResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMReports/TestWiseGraphReportsV2")]
        [Produces(typeof(GraphListResponse))]
        public virtual IActionResult TestWiseGraphReportsV2(int dBTMTestMasterId, long dBTMTraineeDetailId, string dBTMGraphMasterIds, string graphMode, string fromDate, string toDate, long entityId, string userType, string centreCode, bool isMobileRequest, string typeOfRecord = "Batch")
        {
            try
            {
                List<GraphModel> model = _dBTMReportsService.TestWiseGraphReportsV2(dBTMTestMasterId, dBTMTraineeDetailId, dBTMGraphMasterIds, graphMode, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), entityId, userType, centreCode, isMobileRequest, typeOfRecord);
                return IsNotNull(model) ? CreateOKResponse(new GraphListResponse { GraphList = model }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestWiseGraphReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GraphListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestWiseGraphReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GraphListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMReports/NameWiseReports")]
        [Produces(typeof(DBTMTestWiseReportsListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult NameWiseReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, string fromDate, string toDate, long entityId, string userType, string centreCode, bool isMobileRequest)
        {
            try
            {
                DBTMReportsListModel list = _dBTMReportsService.NameWiseMultipleReports(dBTMTestMasterIds, dBTMTraineeDetailId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), entityId, userType, centreCode, isMobileRequest);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMTestWiseReportsListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestWiseReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestWiseReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMReports/TestWiseMultipleReports")]
        [Produces(typeof(DBTMTestWiseReportsListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult TestWiseMultipleReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, string fromDate, string toDate, long entityId, string userType, string centreCode, bool isMobileRequest)
        {
            try
            {
                DBTMReportsListModel list = _dBTMReportsService.TestWiseMultipleReports(dBTMTestMasterIds, dBTMTraineeDetailId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), entityId, userType, centreCode, isMobileRequest, false);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMTestWiseReportsListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestWiseReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestWiseReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMReports/TestWiseMultipleReportsFile")]
        [Produces(typeof(DBTMTestWiseReportsListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult TestWiseMultipleReportsFile(string dBTMTestMasterIds, long dBTMTraineeDetailId, string fromDate, string toDate, long entityId, string userType, string centreCode, bool isMobileRequest, string reportType)
        {
            try
            {
                DBTMReportsListModel list = _dBTMReportsService.TestWiseMultipleReportsFile(dBTMTestMasterIds, dBTMTraineeDetailId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), entityId, userType, centreCode, isMobileRequest, reportType);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMTestWiseReportsListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestWiseReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestWiseReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMReports/BatchWiseMultipleReports")]
        [Produces(typeof(DBTMTestWiseReportsListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult BatchWiseMultipleReports(string dBTMTestMasterIds, int generalBatchMasterId, string fromDate, string toDate, bool isMobileRequest)
        {
            try
            {
                DBTMReportsListModel list = _dBTMReportsService.BatchWiseMultipleReports(dBTMTestMasterIds, generalBatchMasterId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), isMobileRequest);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMTestWiseReportsListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestWiseReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestWiseReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMReports/BatchWiseMultipleReportsFile")]
        [Produces(typeof(DBTMTestWiseReportsListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult BatchWiseMultipleReportsFile(string dBTMTestMasterIds, int generalBatchMasterId, string fromDate, string toDate, long entityId, string userType, string centreCode, bool isMobileRequest, string reportType)
        {
            try
            {
                DBTMReportsListModel list = _dBTMReportsService.BatchWiseMultipleReportsFile(dBTMTestMasterIds, generalBatchMasterId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), entityId, userType, centreCode, isMobileRequest, reportType);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMTestWiseReportsListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestWiseReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestWiseReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMReports/CampWiseMultipleReports")]
        [Produces(typeof(DBTMTestWiseReportsListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult CampWiseMultipleReports(string dBTMTestMasterIds, int dBTMCampMasterId, string fromDate, string toDate, bool isMobileRequest)
        {
            try
            {
                DBTMReportsListModel list = _dBTMReportsService.CampWiseMultipleReports(dBTMTestMasterIds, dBTMCampMasterId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), isMobileRequest);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMTestWiseReportsListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "CampWiseMultipleReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "CampWiseMultipleReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMReports/CampWiseMultipleReportsFile")]
        [Produces(typeof(DBTMTestWiseReportsListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult CampWiseMultipleReportsFile(string dBTMTestMasterIds, int dBTMCampMasterId, string fromDate, string toDate, long entityId, string userType, string centreCode, bool isMobileRequest, string reportType)
        {
            try
            {
                DBTMReportsListModel list = _dBTMReportsService.CampWiseMultipleReportsFile(dBTMTestMasterIds, dBTMCampMasterId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), entityId, userType, centreCode, isMobileRequest, reportType);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMTestWiseReportsListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "CampWiseMultipleReportsFile", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "CampWiseMultipleReportsFile", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMReports/DeleteReportsFile")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteReportsFile([FromBody] ParameterModel parameterModel)
        {
            try
            {
                string fileName = parameterModel?.Ids;
                bool deleted = _dBTMReportsService.DeleteReportsFile(fileName);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMReports", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMReports/GetActivityPerformedDates")]
        [Produces("application/json")]
        public virtual IActionResult GetActivityPerformedDates(string dBTMTestMasterIds, long dBTMTraineeDetailId, string centreCode)
        {
            try
            {
                List<DateTime> dates = _dBTMReportsService.GetActivityPerformedDates(dBTMTestMasterIds, dBTMTraineeDetailId, centreCode);
                if (dates == null || !dates.Any())
                    return Ok(new List<string>());

                var result = dates.Select(d => d.ToString("yyyy-MM-dd")).ToList();
                return Ok(result);
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GetActivityPerformedDates", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GetActivityPerformedDates", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [HttpGet]
        [Route("/DBTMReports/GetBatchActivityPerformedDates")]
        [Produces("application/json")]
        public virtual IActionResult GetBatchActivityPerformedDates(string dBTMTestMasterIds, int generalBatchMasterId)
        {
            try
            {
                List<DateTime> dates = _dBTMReportsService.GetBatchActivityPerformedDates(dBTMTestMasterIds, generalBatchMasterId);
                if (dates == null || !dates.Any())
                    return Ok(new List<string>());

                var result = dates.Select(d => d.ToString("yyyy-MM-dd")).ToList();
                return Ok(result);
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GetBatchActivityPerformedDates", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GetBatchActivityPerformedDates", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMReports/GetActivityVerticalDetails")]
        [Produces(typeof(DBTMReportVerticalDataResponse))]
        public virtual IActionResult GetActivityVerticalDetails(long dBTMDeviceDataId, string typeOfRecord = "Batch")
        {
            try
            {
                DBTMReportVerticalDataModel model = _dBTMReportsService.GetActivityVerticalDetails(dBTMDeviceDataId, typeOfRecord);
                if (model == null || model.TurnList == null || model.TurnList.Count == 0)
                    return CreateNoContentResponse();
                return CreateOKResponse(new DBTMReportVerticalDataResponse { DBTMReportVerticalDataModel = model });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GetActivityVerticalDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMReportVerticalDataResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GetActivityVerticalDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMReportVerticalDataResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [HttpGet]
        [Route("/DBTMReports/GetBatchWiseUser")]
        [Produces(typeof(GeneralBatchUserListResponse))]
        public virtual IActionResult GetBatchWiseUser(long generalBatchMasterId)
        {
            try
            {
                GeneralBatchUserListModel list = _dBTMReportsService.GetBatchWiseUser(generalBatchMasterId);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<GeneralBatchUserListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "BatchWiseUser", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GeneralBatchUserListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "BatchWiseUser", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GeneralBatchUserListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [HttpGet]
        [Route("/DBTMReports/GetCampActivityPerformedDates")]
        [Produces("application/json")]
        public virtual IActionResult GetCampActivityPerformedDates(string dBTMTestMasterIds, int dBTMCampMasterId)
        {
            try
            {
                List<DateTime> dates = _dBTMReportsService.GetCampActivityPerformedDates(dBTMTestMasterIds, dBTMCampMasterId);
                if (dates == null || !dates.Any())
                    return Ok(new List<string>());
                var result = dates.Select(d => d.ToString("yyyy-MM-dd")).ToList();
                return Ok(result);
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GetCampActivityPerformedDates", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GetCampActivityPerformedDates", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMReports/AssignmentWiseMultipleReports")]
        [Produces(typeof(DBTMTestWiseReportsListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult AssignmentWiseMultipleReports(string dBTMTestMasterIds, long generalTrainerMasterId, string fromDate, string toDate, bool isMobileRequest)
        {
            try
            {
                DBTMReportsListModel list = _dBTMReportsService.AssignmentWiseMultipleReports(dBTMTestMasterIds, generalTrainerMasterId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), isMobileRequest);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMTestWiseReportsListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "AssignmentWiseMultipleReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "AssignmentWiseMultipleReports", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestWiseReportsListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}