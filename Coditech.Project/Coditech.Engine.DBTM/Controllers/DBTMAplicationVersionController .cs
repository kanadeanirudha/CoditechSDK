using Coditech.API.Data;
using Coditech.API.Model.Custom.DBTM.DBTMApplicationVersion;
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
    public class DBTMApplicationVersionController : BaseController
    {
        private readonly IDBTMApplicationVersionService _dBTMApplicationVersionService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMApplicationVersionController(ICoditechLogging coditechLogging, IDBTMApplicationVersionService dBTMApplicationVersionService)
        {
            _dBTMApplicationVersionService = dBTMApplicationVersionService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/DBTMApplicationVersion/GetDBTMApplicationVersionList")]
        [Produces(typeof(DBTMApplicationVersionListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMApplicationVersionList(FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMApplicationVersionListModel list = _dBTMApplicationVersionService.GetDBTMApplicationVersionList(filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMApplicationVersionListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMApplicationVersion", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMApplicationVersionListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMApplicationVersion", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMApplicationVersionListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMApplicationVersion/CreateDBTMApplicationVersion")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMApplicationVersionResponse))]
        public virtual IActionResult CreateInventoryCategory([FromBody] DBTMApplicationVersionModel model)
        {
            try
            {
                DBTMApplicationVersionModel dBTMApplicationVersion = _dBTMApplicationVersionService.CreateDBTMApplicationVersion(model);
                return IsNotNull(dBTMApplicationVersion) ? CreateCreatedResponse(new DBTMApplicationVersionResponse { DBTMApplicationVersionModel = dBTMApplicationVersion }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMApplicationVersion", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMApplicationVersionResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMApplicationVersion", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMApplicationVersionResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMApplicationVersion/GetDBTMApplicationVersion")]
        [HttpGet]
        [Produces(typeof(DBTMApplicationVersionResponse))]
        public virtual IActionResult GetDBTMApplicationVersion(short dBTMApplicationVersionId)
        {
            try
            {
                DBTMApplicationVersionModel dBTMApplicationVersionModel = _dBTMApplicationVersionService.GetDBTMApplicationVersion(dBTMApplicationVersionId);
                return IsNotNull(dBTMApplicationVersionModel) ? CreateOKResponse(new DBTMApplicationVersionResponse { DBTMApplicationVersionModel = dBTMApplicationVersionModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMApplicationVersion", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMApplicationVersionResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMApplicationVersion", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMApplicationVersionResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMApplicationVersion/UpdateDBTMApplicationVersion")]
        [HttpPut, ValidateModel]
        [Produces(typeof(DBTMApplicationVersionResponse))]
        public virtual IActionResult UpdateDBTMApplicationVersion([FromBody] DBTMApplicationVersionModel model)
        {
            try
            {
                bool isUpdated = _dBTMApplicationVersionService.UpdateDBTMApplicationVersion(model);
                return isUpdated ? CreateOKResponse(new DBTMApplicationVersionResponse { DBTMApplicationVersionModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMApplicationVersion", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMApplicationVersionResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMApplicationVersion", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMApplicationVersionResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMApplicationVersion/DeleteDBTMApplicationVersion")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteDBTMApplicationVersion([FromBody] ParameterModel dBTMApplicationVersionId)
        {
            try
            {
                bool deleted = _dBTMApplicationVersionService.DeleteDBTMApplicationVersion(dBTMApplicationVersionId);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMApplicationVersion", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMApplicationVersion", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}