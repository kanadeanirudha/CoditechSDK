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
    public class DBTMOrganisationCentrewiseJoiningCodeController : BaseController
    {
        private readonly IDBTMOrganisationCentrewiseJoiningCodeService _dBTMOrganisationCentrewiseJoiningCodeService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMOrganisationCentrewiseJoiningCodeController(ICoditechLogging coditechLogging, IDBTMOrganisationCentrewiseJoiningCodeService dBTMOrganisationCentrewiseJoiningCodeService)
        {
            _dBTMOrganisationCentrewiseJoiningCodeService = dBTMOrganisationCentrewiseJoiningCodeService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/DBTMOrganisationCentrewiseJoiningCode/GetTraineeActiveJoiningCode")]
        [Produces(typeof(DBTMOrganisationCentrewiseJoiningCodeResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetTraineeActiveJoiningCode(string centreCode, string trainerId)
        {
            try
            {
                DBTMOrganisationCentrewiseJoiningCodeModel list = _dBTMOrganisationCentrewiseJoiningCodeService.GetTraineeActiveJoiningCode(centreCode, trainerId);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<DBTMOrganisationCentrewiseJoiningCodeResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMOrganisationCentrewiseJoiningCode", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMOrganisationCentrewiseJoiningCodeResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMOrganisationCentrewiseJoiningCode", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMOrganisationCentrewiseJoiningCodeResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMOrganisationCentrewiseJoiningCode/DeleteOrganisationCentrewiseJoiningCodeFile")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteOrganisationCentrewiseJoiningCodeFile([FromBody] ParameterModel parameterModel)
        {
            try
            {
                string fileName = parameterModel?.Ids;
                bool deleted = _dBTMOrganisationCentrewiseJoiningCodeService.DeleteOrganisationCentrewiseJoiningCodeFile(fileName);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMOrganisationCentrewiseJoiningCode", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMOrganisationCentrewiseJoiningCode", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}