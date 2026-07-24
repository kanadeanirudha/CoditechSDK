using Coditech.API.Data;
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

namespace Coditech.API.Controllers
{
    public class DBTMPrintQRController : BaseController
    {
        private readonly IDBTMPrintQRService _dBTMPrintQRService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMPrintQRController(ICoditechLogging coditechLogging, IDBTMPrintQRService dBTMPrintQRService)
        {
            _dBTMPrintQRService = dBTMPrintQRService;
            _coditechLogging = coditechLogging;
        }

        [Route("/DBTMPrintQR/GetDBTMPrintQR")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMPrintQRListResponse))]
        public IActionResult GetDBTMPrintQR([FromBody] ParameterModel model)
        {
            try
            {
                DBTMPrintQRListModel list = _dBTMPrintQRService.GetDBTMPrintQR(model);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMPrintQRListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrintQR", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMPrintQRListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrintQR", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMPrintQRListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
   
        [HttpGet]
        [Route("/DBTMPrintQR/GetDBTMPrintQRTraineeList")]
        [Produces(typeof(DBTMPrintQRListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMPrintQRTraineeList(int generalBatchMasterId, string userType, FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMPrintQRListModel list = _dBTMPrintQRService.GetDBTMPrintQRTraineeList(generalBatchMasterId, userType, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMPrintQRListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrintQR", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMPrintQRListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrintQR", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMPrintQRListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }    
    }
}