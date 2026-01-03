using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Logger;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.Engine.DBTM.Controllers
{
    public class DBTMCentreWiseSettingController : BaseController
    {
        private readonly IDBTMCentreWiseSettingService _dBTMCentreWiseSettingService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMCentreWiseSettingController(ICoditechLogging coditechLogging, IDBTMCentreWiseSettingService dBTMCentreWiseSettingService)
        {
            _dBTMCentreWiseSettingService = dBTMCentreWiseSettingService;
            _coditechLogging = coditechLogging;
        }

        [Route("/DBTMCentreWiseSetting/GetDBTMCentreWiseSetting")]
        [HttpGet]
        [Produces(typeof(DBTMCentreWiseSettingResponse))]
        public virtual IActionResult GetDBTMCentreWiseSetting(int organisationCentreId)
        {
            try
            {
                DBTMCentreWiseSettingModel dBTMCentreWiseSettingModel = _dBTMCentreWiseSettingService.GetDBTMCentreWiseSetting(organisationCentreId);
                return IsNotNull(dBTMCentreWiseSettingModel) ? CreateOKResponse(new DBTMCentreWiseSettingResponse { DBTMCentreWiseSettingModel = dBTMCentreWiseSettingModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCentreWiseSetting", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMCentreWiseSettingResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCentreWiseSetting", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCentreWiseSettingResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [Route("/DBTMCentreWiseSetting/UpdateDBTMCentreWiseSetting")]
        [HttpPut, ValidateModel]
        [Produces(typeof(DBTMCentreWiseSettingResponse))]
        public virtual IActionResult UpdateOrganisation([FromBody] DBTMCentreWiseSettingModel model)
        {
            try
            {
                DBTMCentreWiseSettingModel dBTMCentreWiseSettingModel = _dBTMCentreWiseSettingService.UpdateDBTMCentreWiseSetting(model);
                return HelperUtility.IsNotNull(dBTMCentreWiseSettingModel) ? CreateOKResponse(new DBTMCentreWiseSettingResponse() { DBTMCentreWiseSettingModel = dBTMCentreWiseSettingModel }) : null;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCentreWiseSetting", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCentreWiseSettingResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMCentreWiseSetting", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCentreWiseSettingResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}
