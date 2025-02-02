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
    public class DBTMBatchActivityController : BaseController
    {
        private readonly IDBTMBatchActivityService _dBTMBatchActivityService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMBatchActivityController(ICoditechLogging coditechLogging, IDBTMBatchActivityService dBTMBatchActivityService)
        {
            _dBTMBatchActivityService = dBTMBatchActivityService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/DBTMBatchActivity/GetDBTMBatchActivityList")]
        [Produces(typeof(DBTMBatchActivityListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMBatchActivityList(int generalBatchMasterId, bool isAssociated, FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMBatchActivityListModel list = _dBTMBatchActivityService.GetDBTMBatchActivityList(generalBatchMasterId, isAssociated, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMBatchActivityListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMBatchActivity", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMBatchActivityListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMBatchActivity", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMBatchActivityListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMBatchActivity/CreateDBTMBatchActivity")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMBatchActivityResponse))]
        public virtual IActionResult CreateDBTMBatchActivity([FromBody] DBTMBatchActivityModel model)
        {
            try
            {
                DBTMBatchActivityModel dBTMBatchActivity = _dBTMBatchActivityService.CreateDBTMBatchActivity(model);
                return IsNotNull(dBTMBatchActivity) ? CreateCreatedResponse(new DBTMBatchActivityResponse { DBTMBatchActivityModel = dBTMBatchActivity }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMBatchActivity", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMBatchActivityResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMBatchActivity", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMBatchActivityResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMBatchActivity/DeleteDBTMBatchActivity")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteDBTMBatchActivity([FromBody] ParameterModel dBTMBatchActivityIds)
        {
            try
            {
                bool deleted = _dBTMBatchActivityService.DeleteDBTMBatchActivity(dBTMBatchActivityIds);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMBatchActivity", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMBatchActivity", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}