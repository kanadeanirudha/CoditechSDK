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
    public class DBTMGeneralTrainerMasterController : BaseController
    {
        private readonly IDBTMGeneralTrainerMasterService _dbtmGeneralTrainerMasterService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMGeneralTrainerMasterController(ICoditechLogging coditechLogging, IDBTMGeneralTrainerMasterService dbtmGeneralTrainerMasterService)
        {
            _dbtmGeneralTrainerMasterService = dbtmGeneralTrainerMasterService;
            _coditechLogging = coditechLogging;
        }

        [Route("/DBTMGeneralTrainerMaster/AssociateUnAssociateTrainer")]
        [HttpPut, ValidateModel]
        [Produces(typeof(GeneralTraineeAssociatedToTrainerResponse))]
        public virtual IActionResult AssociateUnAssociateTrainer([FromBody] GeneralTraineeAssociatedToTrainerModel model)
        {
            try
            {
                bool isUpdated = _dbtmGeneralTrainerMasterService.AssociateUnAssociateTrainer(model);
                return isUpdated  ? CreateOKResponse( new GeneralTraineeAssociatedToTrainerResponse { GeneralTraineeAssociatedToTrainerModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "AssociateUnAssociateTrainer", TraceLevel.Warning);
                return CreateInternalServerErrorResponse( new GeneralTraineeAssociatedToTrainerResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "AssociateUnAssociateTrainer", TraceLevel.Error);
                return CreateInternalServerErrorResponse( new GeneralTraineeAssociatedToTrainerResponse{ HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}