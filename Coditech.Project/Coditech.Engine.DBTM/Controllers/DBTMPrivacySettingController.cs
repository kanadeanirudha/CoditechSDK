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
    public class DBTMPrivacySettingController : BaseController
    {
        private readonly IDBTMPrivacySettingService _dBTMPrivacySettingService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMPrivacySettingController(ICoditechLogging coditechLogging, IDBTMPrivacySettingService dBTMPrivacySettingService)
        {
            _dBTMPrivacySettingService = dBTMPrivacySettingService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/DBTMPrivacySetting/GetDBTMPrivacySettingList")]
        [Produces(typeof(DBTMPrivacySettingListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMPrivacySettingList(string selectedCentreCode, FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMPrivacySettingListModel list = _dBTMPrivacySettingService.GetDBTMPrivacySettingList(selectedCentreCode, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMPrivacySettingListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrivacySetting", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMPrivacySettingListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrivacySetting", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMPrivacySettingListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMPrivacySetting/CreateDBTMPrivacySetting")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMPrivacySettingResponse))]
        public virtual IActionResult CreateDBTMPrivacySetting([FromBody] DBTMPrivacySettingModel model)
        {
            try
            {
                DBTMPrivacySettingModel dBTMPrivacySetting = _dBTMPrivacySettingService.CreateDBTMPrivacySetting(model);
                return IsNotNull(dBTMPrivacySetting) ? CreateCreatedResponse(new DBTMPrivacySettingResponse { DBTMPrivacySettingModel = dBTMPrivacySetting }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrivacySetting", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMPrivacySettingResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrivacySetting", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMPrivacySettingResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMPrivacySetting/GetDBTMPrivacySetting")]
        [HttpGet]
        [Produces(typeof(DBTMPrivacySettingResponse))]
        public virtual IActionResult GetDBTMPrivacySetting(int dBTMPrivacySettingId)
        {
            try
            {
                DBTMPrivacySettingModel dBTMPrivacySettingModel = _dBTMPrivacySettingService.GetDBTMPrivacySetting(dBTMPrivacySettingId);
                return IsNotNull(dBTMPrivacySettingModel) ? CreateOKResponse(new DBTMPrivacySettingResponse { DBTMPrivacySettingModel = dBTMPrivacySettingModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrivacySetting", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMPrivacySettingResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrivacySetting", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMPrivacySettingResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [Route("/DBTMPrivacySetting/UpdateDBTMPrivacySetting")]
        [HttpPut, ValidateModel]
        [Produces(typeof(DBTMPrivacySettingResponse))]
        public virtual IActionResult UpdateDBTMPrivacySetting([FromBody] DBTMPrivacySettingModel model)
        {
            try
            {
                bool isUpdated = _dBTMPrivacySettingService.UpdateDBTMPrivacySetting(model);
                return isUpdated ? CreateOKResponse(new DBTMPrivacySettingResponse { DBTMPrivacySettingModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrivacySetting", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMPrivacySettingResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrivacySetting", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMPrivacySettingResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [Route("/DBTMPrivacySetting/DeleteDBTMPrivacySetting")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteDBTMPrivacySetting([FromBody] ParameterModel dBTMPrivacySettingId)
        {
            try
            {
                bool deleted = _dBTMPrivacySettingService.DeleteDBTMPrivacySetting(dBTMPrivacySettingId);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrivacySetting", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMPrivacySetting", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

    }
}
