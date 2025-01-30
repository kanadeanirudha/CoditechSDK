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
    public class BioradMedisyMediaManagerController : BaseController
    {
        private readonly IBioradMedisyMediaManagerService _bioradMedisyMediaManagerServiceService;
        protected readonly ICoditechLogging _coditechLogging;
        public BioradMedisyMediaManagerController(ICoditechLogging coditechLogging, IBioradMedisyMediaManagerService bioradMedisyMediaManagerServiceService)
        {
            _bioradMedisyMediaManagerServiceService = bioradMedisyMediaManagerServiceService;
            _coditechLogging = coditechLogging;
        }

        [Route("/BioradMedisyMediaManager/GetMediaDetails")]
        [HttpGet]
        [Produces(typeof(BioradMedisyMediaResponse))]
        public  IActionResult GetMediaDetails(long mediaId, long entityId)
        {
            try
            {
                BioradMedisyMediaModel bioradMedisyMediaModel = _bioradMedisyMediaManagerServiceService.GetMediaDetails(mediaId,entityId);
                return IsNotNull(bioradMedisyMediaModel) ? CreateOKResponse(new BioradMedisyMediaResponse { MediaModel = bioradMedisyMediaModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "BioradMedisyMediaManager", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new BioradMedisyMediaResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "BioradMedisyMediaManager", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new BioradMedisyMediaResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/BioradMedisyMediaManager/UpdateFileApprovalFlow")]
        [HttpPut, ValidateModel]
        [Produces(typeof(BioradMedisyMediaResponse))]
        public IActionResult UpdateFileApprovalFlow([FromBody] BioradMedisyMediaModel model)
        {
            try
            {
                bool isUpdated = _bioradMedisyMediaManagerServiceService.UpdateFileApprovalFlow(model);
                return isUpdated ? CreateOKResponse(new BioradMedisyMediaResponse { MediaModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "BioradMedisyMediaManager", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new BioradMedisyMediaResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "BioradMedisyMediaManager", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new BioradMedisyMediaResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

    }
}