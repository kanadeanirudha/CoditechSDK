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
    public class DBTMTestMasterController : BaseController
    {
        private readonly IDBTMTestMasterService _dBTMTestMasterService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMTestMasterController(ICoditechLogging coditechLogging, IDBTMTestMasterService dBTMTestMasterService)
        {
            _dBTMTestMasterService = dBTMTestMasterService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/DBTMTestMaster/GetDBTMTestList")]
        [Produces(typeof(DBTMTestListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMTestList(FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMTestListModel list = _dBTMTestMasterService.GetDBTMTestList(filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
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

        [Route("/DBTMTestMaster/CreateDBTMTest")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMTestResponse))]
        public virtual IActionResult CreateDBTMTest([FromBody] DBTMTestModel model)
        {
            try
            {
                DBTMTestModel dBTMTest = _dBTMTestMasterService.CreateDBTMTest(model);
                return IsNotNull(dBTMTest) ? CreateCreatedResponse(new DBTMTestResponse { DBTMTestModel = dBTMTest }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMTestResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTestMaster/GetDBTMTest")]
        [HttpGet]
        [Produces(typeof(DBTMTestResponse))]
        public virtual IActionResult GetDBTMTest(int dBTMTestMasterId)
        {
            try
            {
                DBTMTestModel dBTMTestModel = _dBTMTestMasterService.GetDBTMTest(dBTMTestMasterId);
                return IsNotNull(dBTMTestModel) ? CreateOKResponse(new DBTMTestResponse { DBTMTestModel = dBTMTestModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMTestResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTestMaster/UpdateDBTMTest")]
        [HttpPut, ValidateModel]
        [Produces(typeof(DBTMTestResponse))]
        public virtual IActionResult UpdateDBTMTest([FromBody] DBTMTestModel model)
        {
            try
            {
                bool isUpdated = _dBTMTestMasterService.UpdateDBTMTest(model);
                return isUpdated ? CreateOKResponse(new DBTMTestResponse { DBTMTestModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMTestResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTestMaster/DeleteDBTMTest")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteDBTMTest([FromBody] ParameterModel dBTMTestMasterIds)
        {
            try
            {
                bool deleted = _dBTMTestMasterService.DeleteDBTMTest(dBTMTestMasterIds);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTest", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        
        [HttpGet]
        [Route("/DBTMTestMaster/GetDBTMTestParameter")]
        [Produces(typeof(DBTMTestParameterListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMTestParameter()
        {
            try
            {
                DBTMTestParameterListModel list = _dBTMTestMasterService.GetDBTMTestParameter();
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMTestParameterListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestParameter", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestParameterListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestParameter", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestParameterListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

    }
}