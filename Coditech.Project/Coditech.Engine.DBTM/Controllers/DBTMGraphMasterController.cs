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
    public class DBTMGraphMasterController : BaseController
    {
        private readonly IDBTMGraphMasterService _dBTMGraphMasterService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMGraphMasterController(ICoditechLogging coditechLogging, IDBTMGraphMasterService dBTMGraphMasterService)
        {
            _dBTMGraphMasterService = dBTMGraphMasterService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/DBTMGraphMaster/GetDBTMGraphList")]
        [Produces(typeof(DBTMGraphMasterListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMGraphList(FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMGraphMasterListModel list = _dBTMGraphMasterService.GetDBTMGraphList(filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMGraphMasterListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraphMaster", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMGraphMasterListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraphMaster", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMGraphMasterListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMGraphMaster/CreateDBTMGraph")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMGraphMasterResponse))]
        public virtual IActionResult CreateDBTMGraph([FromBody] DBTMGraphMasterModel model)
        {
            try
            {
                DBTMGraphMasterModel graphMaster = _dBTMGraphMasterService.CreateDBTMGraph(model);
                return IsNotNull(graphMaster) ? CreateCreatedResponse(new DBTMGraphMasterResponse { DBTMGraphMasterModel = graphMaster }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraphMaster", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMGraphMasterResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraphMaster", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMGraphMasterResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMGraphMaster/GetDBTMGraph")]
        [HttpGet]
        [Produces(typeof(DBTMGraphMasterResponse))]
        public virtual IActionResult GetDBTMGraph(string graphCode)
        {
            try
            {
                DBTMGraphMasterModel dBTMGraphMasterModel = _dBTMGraphMasterService.GetDBTMGraph(graphCode);
                return IsNotNull(dBTMGraphMasterModel) ? CreateOKResponse(new DBTMGraphMasterResponse { DBTMGraphMasterModel = dBTMGraphMasterModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraphMaster", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMGraphMasterResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraphMaster", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMGraphMasterResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMGraphMaster/UpdateDBTMGraph")]
        [HttpPut, ValidateModel]
        [Produces(typeof(DBTMGraphMasterResponse))]
        public virtual IActionResult UpdateDBTMGraph([FromBody] DBTMGraphMasterModel model)
        {
            try
            {
                bool isUpdated = _dBTMGraphMasterService.UpdateDBTMGraph(model);
                return isUpdated ? CreateOKResponse(new DBTMGraphMasterResponse { DBTMGraphMasterModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraphMaster", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMGraphMasterResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraphMaster", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMGraphMasterResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMGraphMaster/DeleteDBTMGraph")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteDBTMGraph([FromBody] ParameterModel graphIds)
        {
            try
            {
                bool deleted = _dBTMGraphMasterService.DeleteDBTMGraph(graphIds);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraphMaster", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraphMaster", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMGraphMaster/GetDBTMGraphTestCode")]
        [Produces(typeof(DBTMTestListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMGraphTestCode()
        {
            try
            {
                DBTMTestListModel list = _dBTMGraphMasterService.GetDBTMGraphTestCode();
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMTestListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraph", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraph", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMGraphMaster/GetGraphVerticalViewSequenceList")]
        [Produces(typeof(DBTMGraphVerticalViewSequenceListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetGraphVerticalViewSequenceList(int dBTMGraphMasterId, FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMGraphVerticalViewSequenceListModel list = _dBTMGraphMasterService.GetGraphVerticalViewSequenceList(dBTMGraphMasterId, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMGraphVerticalViewSequenceListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GraphVerticalViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMGraphVerticalViewSequenceListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GraphVerticalViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMGraphVerticalViewSequenceListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMGraphMaster/GetGraphVerticalViewSequence")]
        [HttpGet]
        [Produces(typeof(DBTMGraphVerticalViewSequenceResponse))]
        public virtual IActionResult GetGraphVerticalViewSequence(int dBTMGraphVerticalViewSequenceId)
        {
            try
            {
                DBTMGraphVerticalViewSequenceModel model = _dBTMGraphMasterService.GetGraphVerticalViewSequence(dBTMGraphVerticalViewSequenceId);
                return IsNotNull(model) ? CreateOKResponse(new DBTMGraphVerticalViewSequenceResponse { DBTMGraphVerticalViewSequenceModel = model }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GraphVerticalViewSequence", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMGraphVerticalViewSequenceResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GraphVerticalViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMGraphVerticalViewSequenceResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMGraphMaster/UpdateGraphVerticalViewSequence")]
        [HttpPut, ValidateModel]
        [Produces(typeof(DBTMGraphVerticalViewSequenceResponse))]
        public virtual IActionResult UpdateGraphVerticalViewSequence( [FromBody] DBTMGraphVerticalViewSequenceModel model)
        {
            try
            {
                bool isUpdated =  _dBTMGraphMasterService.UpdateGraphVerticalViewSequence(model);
                return isUpdated ? CreateOKResponse( new DBTMGraphVerticalViewSequenceResponse { DBTMGraphVerticalViewSequenceModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage( ex, "GraphVerticalViewSequence", TraceLevel.Warning);
                return CreateInternalServerErrorResponse( new DBTMGraphVerticalViewSequenceResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GraphVerticalViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse( new DBTMGraphVerticalViewSequenceResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMGraphMaster/DeleteGraphVerticalViewSequence")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteGraphVerticalViewSequence([FromBody] ParameterModel graphVerticalViewSequenceIds)
        {
            try
            {
                bool deleted = _dBTMGraphMasterService.DeleteGraphVerticalViewSequence(graphVerticalViewSequenceIds);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GraphVerticalViewSequence", TraceLevel.Warning);
                return CreateInternalServerErrorResponse( new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage( ex, "GraphVerticalViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse( new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMGraphMaster/UpdateGraphVerticalSequenceNumber")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMGraphVerticalViewSequenceResponse))]
        public virtual IActionResult UpdateGraphVerticalSequenceNumber([FromBody] DBTMGraphVerticalViewSequenceModel model)
        {
            try
            {
                DBTMGraphVerticalViewSequenceModel result = _dBTMGraphMasterService.UpdateGraphVerticalSequenceNumber(model);
                return IsNotNull(result) ? CreateCreatedResponse( new DBTMGraphVerticalViewSequenceResponse { DBTMGraphVerticalViewSequenceModel = result }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GraphVerticalViewSequence", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GraphVerticalViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMGraphMaster/CreateGraphVerticalViewSequence")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMGraphVerticalViewSequenceResponse))]
        public virtual IActionResult CreateGraphVerticalViewSequence([FromBody] DBTMGraphVerticalViewSequenceModel model)
        {
            try
            {
                DBTMGraphVerticalViewSequenceModel result = _dBTMGraphMasterService.CreateGraphVerticalViewSequence(model);
                return IsNotNull(result) ? CreateCreatedResponse( new DBTMGraphVerticalViewSequenceResponse { DBTMGraphVerticalViewSequenceModel = result }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GraphVerticalViewSequence", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GraphVerticalViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}