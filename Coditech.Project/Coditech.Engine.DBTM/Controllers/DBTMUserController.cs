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
namespace Coditech.API.Controllers
{
    [ApiController]
    public class DBTMUserController : BaseController
    {

        private readonly IDBTMUserService _dbtmUserService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMUserController(ICoditechLogging coditechLogging, IDBTMUserService dbtmUserService)
        {
            _dbtmUserService = dbtmUserService;
            _coditechLogging = coditechLogging;
        }
        /// <summary>
        /// Login to application.
        /// </summary>
        /// <param name="model">UserLoginModel.</param>
        /// <returns>UserModel</returns>
        [Route("/DBTMUser/Login")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMUserModel))]
        public virtual IActionResult Login([FromBody] UserLoginModel model)
        {
            try
            {
                DBTMUserModel user = _dbtmUserService.Login(model);
                return HelperUtility.IsNotNull(user) ? CreateOKResponse(user) : null;

            }
            catch (CoditechUnauthorizedException ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.UserLogin.ToString(), TraceLevel.Warning);
                return CreateUnauthorizedResponse(new DBTMUserModel { HasError = true, ErrorCode = ex.ErrorCode });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.UserLogin.ToString(), TraceLevel.Warning);
                return CreateUnauthorizedResponse(new DBTMUserModel { HasError = true, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.UserLogin.ToString(), TraceLevel.Error);
                return CreateUnauthorizedResponse(new DBTMUserModel { HasError = true, ErrorMessage = ex.Message });
            }

        }

        [Route("/DBTMUser/UpdateAdditionalInformation")]
        [HttpPost, ValidateModel]
        [Produces(typeof(DBTMUserResponse))]
        public virtual IActionResult UpdateAdditionalInformation([FromBody] DBTMUserModel dbtmUserModel)
        {
            try
            {
                DBTMUserModel dbtmUserResponse = _dbtmUserService.UpdateAdditionalInformation(dbtmUserModel);
                return IsNotNull(dbtmUserResponse) ? CreateOKResponse(new DBTMUserResponse { DBTMUserModel = dbtmUserResponse }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMUser", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMUserResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMUser", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMUserResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/DBTMUser/GetDBTMTraineeDetails")]
        [Produces(typeof(DBTMUserResponse))]
        public virtual IActionResult GetDBTMTraineeDetails(long entityId)
        {
            try
            {
                DBTMUserModel dbtmUserResponse = _dbtmUserService.GetDBTMTraineeDetails(entityId);
                return IsNotNull(dbtmUserResponse) ? CreateOKResponse(new DBTMUserResponse { DBTMUserModel = dbtmUserResponse }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMUser", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMUserResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMUser", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMUserResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}