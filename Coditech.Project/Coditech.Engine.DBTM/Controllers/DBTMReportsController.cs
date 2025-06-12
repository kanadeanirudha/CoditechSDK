using Coditech.API.Data;
using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Exceptions;
using Coditech.Common.Logger;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
        public virtual IActionResult BatchWiseReports(int generalBatchMasterId)
        {
            try
            {
                DBTMReportsListModel list = _dBTMReportsService.BatchWiseReports(generalBatchMasterId);
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
        public virtual IActionResult TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate, long entityId)
        {
            try
            {
                DBTMReportsListModel list = _dBTMReportsService.TestWiseReports(dBTMTestMasterId,dBTMTraineeDetailId,FromDate,ToDate,entityId);
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