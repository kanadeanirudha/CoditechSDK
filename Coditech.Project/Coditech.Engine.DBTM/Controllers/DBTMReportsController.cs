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
                DBTMReportsListModel list = _dBTMReportsService.BatchWiseReports(generalBatchMasterId, dBTMTestMasterId, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), isMobileRequest);
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
        public virtual IActionResult TestWiseGraphReports(int dBTMTestMasterId, long dBTMTraineeDetailId, int dBTMGraphMasterId,string fromDate, string toDate, long entityId, string userType, string centreCode, bool isMobileRequest)
        {
            try
            {
                GraphModel model = _dBTMReportsService.TestWiseGraphReports(dBTMTestMasterId, dBTMTraineeDetailId, dBTMGraphMasterId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), entityId, userType, centreCode, isMobileRequest);
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
                DBTMReportsListModel list = _dBTMReportsService.TestWiseMultipleReports(dBTMTestMasterIds, dBTMTraineeDetailId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), entityId, userType, centreCode, isMobileRequest);
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
    }
}