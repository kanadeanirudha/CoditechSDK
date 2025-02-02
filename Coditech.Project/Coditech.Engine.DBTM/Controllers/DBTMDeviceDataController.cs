using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Logger;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using static Coditech.Common.Helper.HelperUtility;

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
        [Produces(typeof(DBTMDeviceDataResponse))]
        public virtual IActionResult InsertDeviceData([FromBody] DBTMDeviceDataModel model)
        {
            try
            {
                DBTMDeviceDataModel dBTMDeviceData = _dBTMDeviceDataService.InsertDeviceData(model);
                return IsNotNull(dBTMDeviceData) ? CreateCreatedResponse(new DBTMDeviceDataResponse { DBTMDeviceDataModel = dBTMDeviceData }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceData", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMDeviceDataResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "DBTMDeviceData", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMDeviceDataResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}