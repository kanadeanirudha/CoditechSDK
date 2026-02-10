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
        [Route("/DBTMTestMaster/GetTestsByCentreCode")]
        [HttpGet]
        [Produces(typeof(DBTMCentreWiseTestResponse))]
        public virtual IActionResult GetTestsByCentreCode(string centreCode)
        {
            try
            {
                DBTMCentreWiseTestListModel list = _dBTMTestMasterService.GetTestsByCentreCode(centreCode);
                return IsNotNull(list) ? CreateOKResponse(new DBTMCentreWiseTestListResponse { DBTMCentreWiseTestList = list.DBTMCentreWiseTestList }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCentreWiseTest", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMCentreWiseTestListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCentreWiseTest", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCentreWiseTestListResponse { HasError = true, ErrorMessage = ex.Message });
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

        [HttpGet]
        [Route("/DBTMTestMaster/GetDBTMTestCalculation")]
        [Produces(typeof(DBTMTestCalculationListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMTestCalculation()
        {
            try
            {
                DBTMTestCalculationListModel list = _dBTMTestMasterService.GetDBTMTestCalculation();
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMTestCalculationListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestCalculation", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestCalculationListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTestCalculation", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTestCalculationListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMTestMaster/GetDBTMGraph")]
        [Produces(typeof(DBTMGraphMasterListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMGraph(int dBTMTestMasterId)
        {
            try
            {
                DBTMGraphMasterListModel list = _dBTMTestMasterService.GetDBTMGraph(dBTMTestMasterId);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMGraphMasterListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraph", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMGraphMasterListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraph", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMGraphMasterListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [Route("/DBTMTestMaster/DBTMGraphByDBTMTestMasterId")]
        [HttpGet]
        [Produces(typeof(DBTMGraphMasterListResponse))]
        public virtual IActionResult GetDBTMGraphByDBTMTestMasterId(int dBTMTestMasterId, string graphMode)
        {
            try
            {
                DBTMGraphMasterListModel list = _dBTMTestMasterService.GetDBTMGraphByDBTMTestMasterId(dBTMTestMasterId, graphMode);
                return IsNotNull(list) ? CreateOKResponse(new DBTMGraphMasterListResponse { DBTMGraphMasterList = list.DBTMGraphMasterList }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraph", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMGraphMasterListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMGraph", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMGraphMasterListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [HttpGet]
        [Route("/DBTMTestMaster/GetDBTMPerformanceMatrixList")]
        [Produces(typeof(DBTMPerformanceMatrixListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMPerformanceMatrixList(FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMPerformanceMatrixListModel list = _dBTMTestMasterService.GetDBTMPerformanceMatrixList(filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMPerformanceMatrixListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPerformanceMatrix", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMPerformanceMatrixListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPerformanceMatrix", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMPerformanceMatrixListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMTestMaster/GetActivityListViewSequenceList")]
        [Produces(typeof(DBTMActivityListViewSequenceListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetActivityListViewSequenceList(int dBTMTestMasterId, FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMActivityListViewSequenceListModel list = _dBTMTestMasterService.GetActivityListViewSequenceList(dBTMTestMasterId, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMActivityListViewSequenceListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityListViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityListViewSequenceListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityListViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityListViewSequenceListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTestMaster/GetActivityListViewSequence")]
        [HttpGet]
        [Produces(typeof(DBTMActivityListViewSequenceResponse))]
        public virtual IActionResult GetActivityListViewSequence(int dBTMTestParameterListViewSequenceId)
        {
            try
            {
                DBTMActivityListViewSequenceModel dBTMTestModel = _dBTMTestMasterService.GetActivityListViewSequence(dBTMTestParameterListViewSequenceId);
                return IsNotNull(dBTMTestModel) ? CreateOKResponse(new DBTMActivityListViewSequenceResponse { DBTMActivityListViewSequenceModel = dBTMTestModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityListViewSequence", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMActivityListViewSequenceResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityListViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityListViewSequenceResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTestMaster/UpdateActivityListViewSequence")]
        [HttpPut, ValidateModel]
        [Produces(typeof(DBTMActivityListViewSequenceResponse))]
        public virtual IActionResult UpdateActivityListViewSequence([FromBody] DBTMActivityListViewSequenceModel model)
        {
            try
            {
                bool isUpdated = _dBTMTestMasterService.UpdateActivityListViewSequence(model);
                return isUpdated ? CreateOKResponse(new DBTMActivityListViewSequenceResponse { DBTMActivityListViewSequenceModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityListViewSequence", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMActivityListViewSequenceResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityListViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityListViewSequenceResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTestMaster/DeleteActivityListViewSequence")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteActivityListViewSequence([FromBody] ParameterModel dBTMTestMasterIds)
        {
            try
            {
                bool deleted = _dBTMTestMasterService.DeleteActivityListViewSequence(dBTMTestMasterIds);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityListViewSequence", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityListViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTestMaster/UpdateSequenceNumber")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMActivityListViewSequenceResponse))]
        public virtual IActionResult UpdateSequenceNumber([FromBody] DBTMActivityListViewSequenceModel model)
        {
            try
            {
                DBTMActivityListViewSequenceModel activityListViewSequence = _dBTMTestMasterService.UpdateSequenceNumber(model);
                return IsNotNull(activityListViewSequence) ? CreateCreatedResponse(new DBTMActivityListViewSequenceResponse { DBTMActivityListViewSequenceModel = activityListViewSequence }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityListViewSequence", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMActivityListViewSequenceResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityListViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityListViewSequenceResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTestMaster/CreateActivityListViewSequence")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMActivityListViewSequenceResponse))]
        public virtual IActionResult CreateActivityListViewSequence([FromBody] DBTMActivityListViewSequenceModel model)
        {
            try
            {
                DBTMActivityListViewSequenceModel dBTMActivityListViewSequence = _dBTMTestMasterService.CreateActivityListViewSequence(model);
                return IsNotNull(dBTMActivityListViewSequence) ? CreateCreatedResponse(new DBTMActivityListViewSequenceResponse { DBTMActivityListViewSequenceModel = dBTMActivityListViewSequence }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityListViewSequence", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMActivityListViewSequenceResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityListViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityListViewSequenceResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMTestMaster/GetActivityVerticalViewSequenceList")]
        [Produces(typeof(DBTMActivityVerticalViewSequenceListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetActivityVerticalViewSequenceList(int dBTMTestMasterId, FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMActivityVerticalViewSequenceListModel list = _dBTMTestMasterService.GetActivityVerticalViewSequenceList(dBTMTestMasterId, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMActivityVerticalViewSequenceListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityVerticalViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityVerticalViewSequenceListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityVerticalViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityVerticalViewSequenceListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTestMaster/GetActivityVerticalViewSequence")]
        [HttpGet]
        [Produces(typeof(DBTMActivityVerticalViewSequenceResponse))]
        public virtual IActionResult GetActivityVerticalViewSequence(int dBTMTestParameterVerticalViewSequenceId)
        {
            try
            {
                DBTMActivityVerticalViewSequenceModel dBTMTestModel = _dBTMTestMasterService.GetActivityVerticalViewSequence(dBTMTestParameterVerticalViewSequenceId);
                return IsNotNull(dBTMTestModel) ? CreateOKResponse(new DBTMActivityVerticalViewSequenceResponse { DBTMActivityVerticalViewSequenceModel = dBTMTestModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityVerticalViewSequence", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMActivityVerticalViewSequenceResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityVerticalViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityVerticalViewSequenceResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTestMaster/UpdateActivityVerticalViewSequence")]
        [HttpPut, ValidateModel]
        [Produces(typeof(DBTMActivityVerticalViewSequenceResponse))]
        public virtual IActionResult UpdateActivityVerticalViewSequence([FromBody] DBTMActivityVerticalViewSequenceModel model)
        {
            try
            {
                bool isUpdated = _dBTMTestMasterService.UpdateActivityVerticalViewSequence(model);
                return isUpdated ? CreateOKResponse(new DBTMActivityVerticalViewSequenceResponse { DBTMActivityVerticalViewSequenceModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityVerticalViewSequence", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMActivityVerticalViewSequenceResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityVerticalViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityVerticalViewSequenceResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTestMaster/DeleteActivityVerticalViewSequence")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteActivityVerticalViewSequence([FromBody] ParameterModel dBTMTestMasterIds)
        {
            try
            {
                bool deleted = _dBTMTestMasterService.DeleteActivityVerticalViewSequence(dBTMTestMasterIds);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityVerticalViewSequence", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityVerticalViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTestMaster/UpdateVerticalSequenceNumber")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMActivityVerticalViewSequenceResponse))]
        public virtual IActionResult UpdateVerticalSequenceNumber([FromBody] DBTMActivityVerticalViewSequenceModel model)
        {
            try
            {
                DBTMActivityVerticalViewSequenceModel ActivityVerticalViewSequence = _dBTMTestMasterService.UpdateVerticalSequenceNumber(model);
                return IsNotNull(ActivityVerticalViewSequence) ? CreateCreatedResponse(new DBTMActivityVerticalViewSequenceResponse { DBTMActivityVerticalViewSequenceModel = ActivityVerticalViewSequence }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityVerticalViewSequence", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMActivityVerticalViewSequenceResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityVerticalViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityVerticalViewSequenceResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTestMaster/CreateActivityVerticalViewSequence")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMActivityVerticalViewSequenceResponse))]
        public virtual IActionResult CreateActivityVerticalViewSequence([FromBody] DBTMActivityVerticalViewSequenceModel model)
        {
            try
            {
                DBTMActivityVerticalViewSequenceModel dBTMActivityVerticalViewSequence = _dBTMTestMasterService.CreateActivityVerticalViewSequence(model);
                return IsNotNull(dBTMActivityVerticalViewSequence) ? CreateCreatedResponse(new DBTMActivityVerticalViewSequenceResponse { DBTMActivityVerticalViewSequenceModel = dBTMActivityVerticalViewSequence }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityVerticalViewSequence", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMActivityVerticalViewSequenceResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "ActivityVerticalViewSequence", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivityVerticalViewSequenceResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}