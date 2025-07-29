using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace Coditech.API.Controllers
{
    [ApiController]
    public class DBTMGeneralBatchMasterController : BaseController
    {
        private readonly IDBTMBatchMasterService _dbtmGeneralBatchMasterService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMGeneralBatchMasterController(ICoditechLogging coditechLogging, IDBTMBatchMasterService dbtmGeneralBatchMasterService)
        {
            _dbtmGeneralBatchMasterService = dbtmGeneralBatchMasterService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/DBTMGeneralBatchMaster/GetDBTMBatchUserList")]
        [Produces(typeof(GeneralBatchUserListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetDBTMBatchUserList(string selectedCentreCode, long  generalTrainerMasterId, int generalBatchMasterId)
        {
            try
            {
                GeneralBatchUserListModel list = _dbtmGeneralBatchMasterService.GetDBTMBatchUserList(selectedCentreCode, generalTrainerMasterId,generalBatchMasterId);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<GeneralBatchUserListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.GeneralBatchUser.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GeneralBatchUserListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.GeneralBatchUser.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GeneralBatchUserListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}