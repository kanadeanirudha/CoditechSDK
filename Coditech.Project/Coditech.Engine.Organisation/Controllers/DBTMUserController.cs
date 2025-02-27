using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
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
    public class DBTMUserController : BaseController
    {

        private readonly IDBTMUserService _dbtmUserService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMUserController(ICoditechLogging coditechLogging, IDBTMUserService dbtmUserService)
        {
            _dbtmUserService = dbtmUserService;
            _coditechLogging = coditechLogging;
        }

        [Route("/DBTMUser/DBTMRegisterTrainee")]
        [HttpPost, ValidateModel]
        [Produces(typeof(GeneralPersonResponse))]
        public virtual IActionResult DBTMRegisterTrainee([FromBody] GeneralPersonModel model)
        {
            try
            {
                GeneralPersonModel generalPerson = _dbtmUserService.DBTMRegisterTrainee(model);
                return HelperUtility.IsNotNull(generalPerson) ? CreateCreatedResponse(new GeneralPersonResponse { GeneralPersonModel = generalPerson }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.DBTMRegisterTrainee.ToString(),TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GeneralPersonResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.DBTMRegisterTrainee.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GeneralPersonResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}