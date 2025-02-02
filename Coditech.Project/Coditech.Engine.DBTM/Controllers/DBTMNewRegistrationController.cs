using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using static Coditech.Common.Helper.HelperUtility;

namespace Coditech.Engine.DBTM.Controllers
{
    public class DBTMNewRegistrationController : BaseController
    {
        private readonly IDBTMNewRegistrationService _dBTMNewRegistrationService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMNewRegistrationController(ICoditechLogging coditechLogging, IDBTMNewRegistrationService dBTMNewRegistrationService)
        {
            _dBTMNewRegistrationService = dBTMNewRegistrationService;
            _coditechLogging = coditechLogging;
        }

        [Route("/DBTMCentreRegistration/CentreRegistration")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMNewRegistrationResponse))]
        [AllowAnonymous]
        public virtual IActionResult CentreRegistration([FromBody] DBTMNewRegistrationModel model)
        {
            try
            {
                DBTMNewRegistrationModel newRegistration = _dBTMNewRegistrationService.DBTMNewRegistration(model);
                return IsNotNull(newRegistration) ? CreateCreatedResponse(new DBTMNewRegistrationResponse { DBTMNewRegistrationModel = newRegistration }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMNewRegistration", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMNewRegistrationResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMNewRegistration", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMNewRegistrationResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}