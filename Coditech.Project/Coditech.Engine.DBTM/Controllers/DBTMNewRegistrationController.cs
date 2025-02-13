using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
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

        [Route("/DBTMCentreRegistration/DBTMCentreRegistration")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMNewRegistrationResponse))]
        [AllowAnonymous]
        public virtual IActionResult DBTMCentreRegistration([FromBody] DBTMNewRegistrationModel model)
        {
            try
            {
                DBTMNewRegistrationModel newRegistration = _dBTMNewRegistrationService.DBTMCentreRegistration(model);
                return IsNotNull(newRegistration) ? CreateCreatedResponse(new DBTMNewRegistrationResponse { DBTMNewRegistrationModel = newRegistration }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.DBTMCentreRegistration.ToString(), TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMNewRegistrationResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.DBTMCentreRegistration.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMNewRegistrationResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/TrainerRegistration/TrainerRegistration")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMNewRegistrationResponse))]
        [AllowAnonymous]
        public virtual IActionResult TrainerRegistration([FromBody] DBTMNewRegistrationModel model)
        {
            try
            {
                DBTMNewRegistrationModel newRegistration = _dBTMNewRegistrationService.TrainerRegistration(model);
                return IsNotNull(newRegistration) ? CreateCreatedResponse(new DBTMNewRegistrationResponse { DBTMNewRegistrationModel = newRegistration }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.TrainerRegistration.ToString(), TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMNewRegistrationResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.TrainerRegistration.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMNewRegistrationResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/IndividualRegistration/IndividualRegistration")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMNewRegistrationResponse))]
        [AllowAnonymous]
        public virtual IActionResult IndividualRegistration([FromBody] DBTMNewRegistrationModel model)
        {
            try
            {
                DBTMNewRegistrationModel newRegistration = _dBTMNewRegistrationService.IndividualRegistration(model);
                return IsNotNull(newRegistration) ? CreateCreatedResponse(new DBTMNewRegistrationResponse { DBTMNewRegistrationModel = newRegistration }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.IndividualRegistration.ToString(), TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMNewRegistrationResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.IndividualRegistration.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMNewRegistrationResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}