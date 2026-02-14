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
    public class DBTMCampMasterController : BaseController
    {
        private readonly IDBTMCampMasterService _dBTMCampMasterService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMCampMasterController(ICoditechLogging coditechLogging, IDBTMCampMasterService dBTMCampMasterService)
        {
            _dBTMCampMasterService = dBTMCampMasterService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/DBTMCampMaster/GetDBTMCampList")]
        [Produces(typeof(DBTMCampListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMCampList(string selectedCentreCode,FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMCampMasterListModel list = _dBTMCampMasterService.GetDBTMCampList(selectedCentreCode, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMCampListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampMaster", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCampListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampMaster", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCampListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMCampMaster/CreateDBTMCamp")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMCampResponse))]
        public virtual IActionResult CreateDBTMCamp([FromBody] DBTMCampMasterModel model)
        {
            try
            {
                DBTMCampMasterModel campMaster = _dBTMCampMasterService.CreateDBTMCamp(model);
                return IsNotNull(campMaster) ? CreateCreatedResponse(new DBTMCampResponse { DBTMCampModel = campMaster }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampMaster", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMCampResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampMaster", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCampResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMCampMaster/GetDBTMCamp")]
        [HttpGet]
        [Produces(typeof(DBTMCampResponse))]
        public virtual IActionResult GetDBTMCamp(long dBTMCampMasterId)
        {
            try
            {
                DBTMCampMasterModel dBTMCampMasterModel = _dBTMCampMasterService.GetDBTMCamp(dBTMCampMasterId);
                return IsNotNull(dBTMCampMasterModel) ? CreateOKResponse(new DBTMCampResponse { DBTMCampModel = dBTMCampMasterModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampMaster", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMCampResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampMaster", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCampResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMCampMaster/UpdateDBTMCamp")]
        [HttpPut, ValidateModel]
        [Produces(typeof(DBTMCampResponse))]
        public virtual IActionResult UpdateDBTMCamp([FromBody] DBTMCampMasterModel model)
        {
            try
            {
                bool isUpdated = _dBTMCampMasterService.UpdateDBTMCamp(model);
                return isUpdated ? CreateOKResponse(new DBTMCampResponse { DBTMCampModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampMaster", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMCampResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampMaster", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCampResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMCampMaster/DeleteDBTMCamp")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteDBTMCamp([FromBody] ParameterModel CampIds)
        {
            try
            {
                bool deleted = _dBTMCampMasterService.DeleteDBTMCamp(CampIds);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampMaster", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampMaster", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [HttpGet]
        [Route("/DBTMCampMaster/GetDBTMCampUserList")]
        [Produces(typeof(DBTMCampUserListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMCampUserList(long dBTMCampMasterId, string userType, FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                DBTMCampUserListModel list = _dBTMCampMasterService.GetDBTMCampUserList(dBTMCampMasterId, userType, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMCampUserListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampUser" , TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCampUserListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCampUser", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCampUserListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMCampMaster/AssociateUnAssociateCampwiseUser")]
        [HttpPut, ValidateModel]
        [Produces(typeof(DBTMCampUserResponse))]
        public virtual IActionResult AssociateUnAssociateCampwiseUser([FromBody] DBTMCampUserModel model)
        {
            try
            {
                bool isUpdated = _dBTMCampMasterService.AssociateUnAssociateCampwiseUser(model);
                return isUpdated ? CreateOKResponse(new DBTMCampUserResponse { DBTMCampUserModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "AssociateUnAssociateCampwiseUser", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMCampUserResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "AssociateUnAssociateCampwiseUser", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCampUserResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [HttpGet]
        [Route("/DBTMCampMaster/GetCamUserListByCentreCodeAndGeneralTrainerMasterId")]
        [Produces(typeof(DBTMCampUserListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetCamUserListByCentreCodeAndGeneralTrainerMasterId(string selectedCentreCode, long generalTrainerMasterId, long dBTMCampMasterId)
        {
            try
            {
                DBTMCampUserListModel list = _dBTMCampMasterService.GetCampUserListByCentreCodeAndGeneralTrainerMasterId(selectedCentreCode, generalTrainerMasterId, dBTMCampMasterId);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMCampUserListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex,"DBTMCampUser", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCampUserListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex,"DBTMCampUser", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCampUserListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}