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
    public class DBTMTraineeAssignmentController : BaseController
    {
        private readonly IDBTMTraineeAssignmentService _dBTMTraineeAssignmentService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMTraineeAssignmentController(ICoditechLogging coditechLogging, IDBTMTraineeAssignmentService dBTMTraineeAssignmentService)
        {
            _dBTMTraineeAssignmentService = dBTMTraineeAssignmentService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/DBTMTraineeAssignment/GetDBTMTraineeAssignmentList")]
        [Produces(typeof(DBTMTraineeAssignmentListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMTraineeAssignmentList(long generalTrainerMasterId,FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMTraineeAssignmentListModel list = _dBTMTraineeAssignmentService.GetDBTMTraineeAssignmentList(generalTrainerMasterId,filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMTraineeAssignmentListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeAssignment", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTraineeAssignmentListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeAssignment", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTraineeAssignmentListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTraineeAssignment/CreateDBTMTraineeAssignment")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMTraineeAssignmentResponse))]
        public virtual IActionResult CreateDBTMTraineeAssignment([FromBody] DBTMTraineeAssignmentModel model)
        {
            try
            {
                DBTMTraineeAssignmentModel dBTMTraineeAssignment = _dBTMTraineeAssignmentService.CreateDBTMTraineeAssignment(model);
                return IsNotNull(dBTMTraineeAssignment) ? CreateCreatedResponse(new DBTMTraineeAssignmentResponse { DBTMTraineeAssignmentModel = dBTMTraineeAssignment }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeAssignment", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMTraineeAssignmentResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeAssignment", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTraineeAssignmentResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTraineeAssignment/GetDBTMTraineeAssignment")]
        [HttpGet]
        [Produces(typeof(DBTMTraineeAssignmentResponse))]
        public virtual IActionResult GetDBTMTraineeAssignment(long dBTMTraineeAssignmentId)
        {
            try
            {
                DBTMTraineeAssignmentModel dBTMTraineeAssignmentModel = _dBTMTraineeAssignmentService.GetDBTMTraineeAssignment(dBTMTraineeAssignmentId);
                return IsNotNull(dBTMTraineeAssignmentModel) ? CreateOKResponse(new DBTMTraineeAssignmentResponse { DBTMTraineeAssignmentModel = dBTMTraineeAssignmentModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeAssignment", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMTraineeAssignmentResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeAssignment", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTraineeAssignmentResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTraineeAssignment/UpdateDBTMTraineeAssignment")]
        [HttpPut, ValidateModel]
        [Produces(typeof(DBTMTraineeAssignmentResponse))]
        public virtual IActionResult UpdateDBTMTraineeAssignment([FromBody] DBTMTraineeAssignmentModel model)
        {
            try
            {
                bool isUpdated = _dBTMTraineeAssignmentService.UpdateDBTMTraineeAssignment(model);
                return isUpdated ? CreateOKResponse(new DBTMTraineeAssignmentResponse { DBTMTraineeAssignmentModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeAssignment", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMTraineeAssignmentResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeAssignment", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTraineeAssignmentResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTraineeAssignment/DeleteDBTMTraineeAssignment")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteDBTMTraineeAssignment([FromBody] ParameterModel dBTMTraineeAssignmentIds)
        {
            try
            {
                bool deleted = _dBTMTraineeAssignmentService.DeleteDBTMTraineeAssignment(dBTMTraineeAssignmentIds);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeAssignment", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeAssignment", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTraineeAssignment/GetTrainerByCentreCode")]
        [HttpGet]
        [Produces(typeof(GeneralTrainerListResponse))]
        public virtual IActionResult GetTrainerByCentreCode(string centreCode)
        {
            try
            {
                GeneralTrainerListModel list = _dBTMTraineeAssignmentService.GetTrainerByCentreCode(centreCode);
                return IsNotNull(list) ? CreateOKResponse(new GeneralTrainerListResponse { GeneralTrainerList = list.GeneralTrainerList }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.Trainer.ToString(), TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GeneralTrainerListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.Trainer.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GeneralTrainerListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMTraineeAssignment/GetTraineeDetailByCentreCodeAndgeneralTrainerId")]
        [Produces(typeof(DBTMTraineeDetailsListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetTraineeDetailByCentreCodeAndgeneralTrainerId(string centreCode, long generalTrainerId,int pagingStart, int pagingLength)
        {
            try
            {
                DBTMTraineeDetailsListModel list = _dBTMTraineeAssignmentService.GetTraineeDetailByCentreCodeAndgeneralTrainerId(centreCode,generalTrainerId,pagingStart, pagingLength);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMTraineeDetailsListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTraineeDetailsListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTraineeDetailsListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTraineeAssignment/SendAssignmentReminder")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult SendAssignmentReminder([FromBody] string dBTMTraineeAssignmentId)
        {
            try
            {
                bool isUpdated = _dBTMTraineeAssignmentService.SendAssignmentReminder(dBTMTraineeAssignmentId);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = isUpdated });

            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeAssignment", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMTraineeAssignmentResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeAssignment", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTraineeAssignmentResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}