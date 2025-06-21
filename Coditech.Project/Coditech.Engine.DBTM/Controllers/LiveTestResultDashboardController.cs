using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Controllers
{

    public class LiveTestResultDashboardController : BaseController
    {
        private readonly ILiveTestResultDashboardService _liveTestResultDashboardService;
        protected readonly ICoditechLogging _coditechLogging;
        public LiveTestResultDashboardController(ICoditechLogging coditechLogging, ILiveTestResultDashboardService liveTestResultDashboardService)
        {
            _liveTestResultDashboardService = liveTestResultDashboardService;
            _coditechLogging = coditechLogging;
        }

        [Route("/LiveTestResultDashboard/GetLiveTestResultDashboard")]     
        [HttpPost, ValidateModel]
        [Produces(typeof(LiveTestResultDashboardResponse))]
        public virtual IActionResult GetLiveTestResultDashboard([FromBody] LiveTestResultLoginModel model)
        {
            try
            {
                LiveTestResultDashboardModel dashboardModel = _liveTestResultDashboardService.GetLiveTestResultLogin(model);
                return IsNotNull(dashboardModel) ? CreateOKResponse(new LiveTestResultDashboardResponse { LiveTestResultDashboardModel = dashboardModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.LiveTestResultLogin.ToString(), TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new LiveTestResultDashboardResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, LogComponentCustomEnum.LiveTestResultLogin.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new LiveTestResultDashboardResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}