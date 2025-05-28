using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Exceptions;
using Coditech.Common.Logger;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace Coditech.Engine.DBTM.Controllers
{
    public class DBTMDeviceDataController : BaseController
    {
        private readonly IDBTMDeviceDataService _dBTMDeviceDataService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMDeviceDataController(ICoditechLogging coditechLogging, IDBTMDeviceDataService dBTMDeviceDataService)
        {
            _dBTMDeviceDataService = dBTMDeviceDataService;
            _coditechLogging = coditechLogging;
        }

        [Route("/DBTMDeviceData/InsertDeviceData")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult InsertDeviceData([FromBody] List<DBTMDeviceDataModel> model)
        {
            try
            {
                bool deleted = _dBTMDeviceDataService.InsertDeviceData(model);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceData", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceData", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}