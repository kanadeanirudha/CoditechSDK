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
        [Route("/DBTMCentreWiseSetting/AssociateUnAssociateCentreTest")]
        [HttpPut, ValidateModel]
        [Produces(typeof(DBTMCentreWiseTestResponse))]
        public virtual IActionResult AssociateUnAssociateCentreTest([FromBody] DBTMCentreWiseTestModel model)
        {
            try
            {
                bool isUpdated = _dBTMCentreWiseSettingService.AssociateUnAssociateCentreTest(model);
                return isUpdated ? CreateOKResponse(new DBTMCentreWiseTestResponse { DBTMCentreWiseTestModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "AssociateUnAssociateCentreTest", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMCentreWiseTestResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "AssociateUnAssociateCentreTest", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCentreWiseTestResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        [Route("/DBTMCentreWiseSetting/AssociateCentreTests")]
        [HttpPut]
        [Produces(typeof(DBTMCentreWiseTestResponse))]
        public virtual IActionResult AssociateCentreTests([FromBody] DBTMCentreWiseTestModel dBTMCentreWiseTestModel)
        {
            try
            {
                DBTMCentreWiseTestModel result = _dBTMCentreWiseSettingService.AssociateCentreTests(dBTMCentreWiseTestModel.OrganisationCentreMasterId, dBTMCentreWiseTestModel.CentreCode, dBTMCentreWiseTestModel.TestIds);
                return CreateOKResponse(new DBTMCentreWiseTestResponse { DBTMCentreWiseTestModel = result });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "AssociateCentreTests", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMCentreWiseTestResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "AssociateCentreTests", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCentreWiseTestResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/DBTMCentreWiseSetting/UnAssociateCentreTests")]
        [HttpPut]
        [Produces(typeof(DBTMCentreWiseTestResponse))]
        public virtual IActionResult UnAssociateCentreTests([FromBody] DBTMCentreWiseTestModel dBTMCentreWiseTestModel)
        {
            try
            {
                DBTMCentreWiseTestModel result = _dBTMCentreWiseSettingService.UnAssociateCentreTests(dBTMCentreWiseTestModel.OrganisationCentreMasterId, dBTMCentreWiseTestModel.CentreCode, dBTMCentreWiseTestModel.TestIds);
                return CreateOKResponse(new DBTMCentreWiseTestResponse { DBTMCentreWiseTestModel = result });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "UnAssociateCentreTests", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMCentreWiseTestResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "UnAssociateCentreTests", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMCentreWiseTestResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}
