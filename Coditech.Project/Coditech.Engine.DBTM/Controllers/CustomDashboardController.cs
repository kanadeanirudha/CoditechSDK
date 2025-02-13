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

    public class CustomDashboardController : BaseController
    {

        private readonly ICustomDashboardService _dashboardService;
        protected readonly ICoditechLogging _coditechLogging;
        public CustomDashboardController(ICoditechLogging coditechLogging, ICustomDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
            _coditechLogging = coditechLogging;
        }

        [Route("/CustomDashboardController/GetCustomDashboardDetails")]
        [HttpGet]
        [Produces(typeof(CustomDashboardResponse))]
        public virtual IActionResult GetCustomDashboardDetails(int selectedAdminRoleMasterId,long userMasterId)
        {
            try
            {
                CustomDashboardModel dashboardModel = _dashboardService.GetCustomDashboardDetails(selectedAdminRoleMasterId, userMasterId);
                return IsNotNull(dashboardModel) ? CreateOKResponse(new CustomDashboardResponse { CustomDashboardModel = dashboardModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.Dashboard.ToString(), TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new CustomDashboardResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.Dashboard.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new CustomDashboardResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}