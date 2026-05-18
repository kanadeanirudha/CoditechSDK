using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Logger;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Controllers
{
    public class DBTMGeneralCommonController : BaseController
    {
        private readonly IDBTMGeneralCommonService _dBTMGeneralCommonService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMGeneralCommonController(ICoditechLogging coditechLogging, IDBTMGeneralCommonService dBTMGeneralCommonService)
        {
            _dBTMGeneralCommonService = dBTMGeneralCommonService;
            _coditechLogging = coditechLogging;
        }
        [Route("/DBTMGeneralCommon/GetDBTMDeviceDataDecrypted")]
        [HttpGet]
        [Produces(typeof(DBTMDeviceDataDetailsResponse))]
        public virtual IActionResult GetDBTMDeviceDataDecrypted(string dBTMDeviceDataIds)
        {
            try
            {
                DBTMDeviceDataDetailsModel dBTMDeviceDataDetailsModel = _dBTMGeneralCommonService.GetDBTMDeviceDataDecrypted(dBTMDeviceDataIds);
                return IsNotNull(dBTMDeviceDataDetailsModel) ? CreateOKResponse(new DBTMDeviceDataDetailsResponse { DBTMDeviceDataDetailsModel = dBTMDeviceDataDetailsModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceDataDecrypted", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMDeviceDataDetailsResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceDataDecrypted", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMDeviceDataDetailsResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}