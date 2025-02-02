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
    public class DBTMActivityCategoryController : BaseController
    {
        private readonly IDBTMActivityCategoryService _dBTMActivityCategoryService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMActivityCategoryController(ICoditechLogging coditechLogging, IDBTMActivityCategoryService dBTMActivityCategoryService)
        {
            _dBTMActivityCategoryService = dBTMActivityCategoryService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/DBTMActivityCategory/GetDBTMActivityCategoryList")]
        [Produces(typeof(DBTMActivityCategoryListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMActivityCategoryList(FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMActivityCategoryListModel list = _dBTMActivityCategoryService.GetDBTMActivityCategoryList(filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMActivityCategoryListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivityCategory", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityCategoryListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivityCategory", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityCategoryListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMActivityCategory/CreateDBTMActivityCategory")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMActivityCategoryResponse))]
        public virtual IActionResult CreateInventoryCategory([FromBody] DBTMActivityCategoryModel model)
        {
            try
            {
                DBTMActivityCategoryModel dBTMActivityCategory = _dBTMActivityCategoryService.CreateDBTMActivityCategory(model);
                return IsNotNull(dBTMActivityCategory) ? CreateCreatedResponse(new DBTMActivityCategoryResponse { DBTMActivityCategoryModel = dBTMActivityCategory }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivityCategory", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMActivityCategoryResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivityCategory", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityCategoryResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMActivityCategory/GetDBTMActivityCategory")]
        [HttpGet]
        [Produces(typeof(DBTMActivityCategoryResponse))]
        public virtual IActionResult GetDBTMActivityCategory(short dBTMActivityCategoryId)
        {
            try
            {
                DBTMActivityCategoryModel dBTMActivityCategoryModel = _dBTMActivityCategoryService.GetDBTMActivityCategory(dBTMActivityCategoryId);
                return IsNotNull(dBTMActivityCategoryModel) ? CreateOKResponse(new DBTMActivityCategoryResponse { DBTMActivityCategoryModel = dBTMActivityCategoryModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivityCategory", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMActivityCategoryResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivityCategory", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityCategoryResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMActivityCategory/UpdateDBTMActivityCategory")]
        [HttpPut, ValidateModel]
        [Produces(typeof(DBTMActivityCategoryResponse))]
        public virtual IActionResult UpdateDBTMActivityCategory([FromBody] DBTMActivityCategoryModel model)
        {
            try
            {
                bool isUpdated = _dBTMActivityCategoryService.UpdateDBTMActivityCategory(model);
                return isUpdated ? CreateOKResponse(new DBTMActivityCategoryResponse { DBTMActivityCategoryModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivityCategory", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMActivityCategoryResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivityCategory", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityCategoryResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMActivityCategory/DeleteDBTMActivityCategory")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteDBTMActivityCategory([FromBody] ParameterModel dBTMActivityCategoryId)
        {
            try
            {
                bool deleted = _dBTMActivityCategoryService.DeleteDBTMActivityCategory(dBTMActivityCategoryId);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivityCategory", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivityCategory", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}