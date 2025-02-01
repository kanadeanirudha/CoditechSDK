using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Logger;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Controllers
{

    public class DBTMDashboardController : BaseController
    {

        private readonly IDBTMDashboardService _dashboardService;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMDashboardController(ICoditechLogging coditechLogging, IDBTMDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
            _coditechLogging = coditechLogging;
        }

        [Route("/DBTMDashboardController/GetDBTMDashboardDetails")]
        [HttpGet]
        [Produces(typeof(DBTMDashboardResponse))]
        public virtual IActionResult GetDBTMDashboardDetails(int selectedAdminRoleMasterId,long userMasterId)
        {
            try
            {
                DBTMDashboardModel dashboardModel = _dashboardService.GetDBTMDashboardDetails(selectedAdminRoleMasterId, userMasterId);
                return IsNotNull(dashboardModel) ? CreateOKResponse(new DBTMDashboardResponse { DBTMDashboardModel = dashboardModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.Dashboard.ToString(), TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new DBTMDashboardResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.Dashboard.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new DBTMDashboardResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}