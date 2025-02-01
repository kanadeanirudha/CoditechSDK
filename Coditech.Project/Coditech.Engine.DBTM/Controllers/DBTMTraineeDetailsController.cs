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
    public class DBTMTraineeDetailsController : BaseController
    {
        private readonly IDBTMTraineeDetailsService _dBTMTraineeDetailsService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMTraineeDetailsController(ICoditechLogging coditechLogging, IDBTMTraineeDetailsService dBTMTraineeDetailsService)
        {
            _dBTMTraineeDetailsService = dBTMTraineeDetailsService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/DBTMTraineeDetails/GetDBTMTraineeDetailsList")]
        [Produces(typeof(DBTMTraineeDetailsListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMTraineeDetailsList(string selectedCentreCode, FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMTraineeDetailsListModel list = _dBTMTraineeDetailsService.GetDBTMTraineeDetailsList(selectedCentreCode, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
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

        [Route("/DBTMTraineeDetails/GetDBTMTraineeOtherDetails")]
        [HttpGet]
        [Produces(typeof(DBTMTraineeDetailsResponse))]
        public virtual IActionResult GetDBTMTraineeOtherDetails(long dBTMTraineeDetailId)
        {
            try
            {
                DBTMTraineeDetailsModel dBTMTraineeDetailsModel = _dBTMTraineeDetailsService.GetDBTMTraineeOtherDetails(dBTMTraineeDetailId);
                return IsNotNull(dBTMTraineeDetailsModel) ? CreateOKResponse(new DBTMTraineeDetailsResponse { DBTMTraineeDetailsModel = dBTMTraineeDetailsModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMTraineeDetailsResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTraineeDetailsResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTraineeDetails/UpdateDBTMTraineeOtherDetails")]
        [HttpPut, ValidateModel]
        [Produces(typeof(DBTMTraineeDetailsResponse))]
        public virtual IActionResult UpdateDBTMTraineeOtherDetails([FromBody] DBTMTraineeDetailsModel model)
        {
            try
            {
                bool isUpdated = _dBTMTraineeDetailsService.UpdateDBTMTraineeOtherDetails(model);
                return isUpdated ? CreateOKResponse(new DBTMTraineeDetailsResponse { DBTMTraineeDetailsModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMTraineeDetailsResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMTraineeDetailsResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMTraineeDetails/DeleteDBTMTraineeDetails")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteDBTMTraineeDetails([FromBody] ParameterModel dBTMTraineeDetailIds)
        {
            try
            {
                bool deleted = _dBTMTraineeDetailsService.DeleteDBTMTraineeDetails(dBTMTraineeDetailIds);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMTraineeDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMTraineeDetails/GetTraineeActivitiesList")]
        [Produces(typeof(DBTMActivitiesListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetTraineeActivitiesList(string personCode,int numberOfDaysRecord,FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMActivitiesListModel list = _dBTMTraineeDetailsService.GetTraineeActivitiesList(personCode,numberOfDaysRecord,filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMActivitiesListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivities", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivitiesListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivities", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivitiesListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMTraineeDetails/GetTraineeActivitiesDetailsList")]
        [Produces(typeof(DBTMActivitiesDetailsListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetTraineeActivitiesDetailsList(long dBTMDeviceDataId,FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMActivitiesDetailsListModel list = _dBTMTraineeDetailsService.GetTraineeActivitiesDetailsList(dBTMDeviceDataId,filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMActivitiesDetailsListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivitiesDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivitiesDetailsListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMActivitiesDetails", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMActivitiesDetailsListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}