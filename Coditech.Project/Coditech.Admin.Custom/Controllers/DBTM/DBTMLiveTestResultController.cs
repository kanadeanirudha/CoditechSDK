using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Coditech.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coditech.Admin.Controllers
{
    public class LiveTestResultController : BaseController
    {
        private readonly ILiveTestResultDashboardAgent _liveTestResultDashboardAgent;

        public LiveTestResultController(ILiveTestResultDashboardAgent liveTestResultDashboardAgent)
        {
            _liveTestResultDashboardAgent = liveTestResultDashboardAgent;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            LiveTestResultDashboardViewModel liveTestResultDashboardViewModel = SessionHelper.GetDataFromSession<LiveTestResultDashboardViewModel>(CustomConstants.LiveResultSession);

            if (liveTestResultDashboardViewModel != null && !liveTestResultDashboardViewModel.HasError)
            {
                return View("~/Views/DBTM/DBTMLiveTestResult/LiveTestResult.cshtml", liveTestResultDashboardViewModel);
            }
            else
            {
                LiveTestResultLoginViewModel liveTestResultLoginViewModel = new LiveTestResultLoginViewModel();
                return View("~/Views/DBTM/DBTMLiveTestResult/LiveTestResultLogin.cshtml", liveTestResultLoginViewModel);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(LiveTestResultLoginViewModel liveTestResultLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                LiveTestResultDashboardViewModel liveTestResultDashboardViewModel =
                    _liveTestResultDashboardAgent.GetLiveTestResultDashboard(liveTestResultLoginViewModel);

                if (!liveTestResultDashboardViewModel.HasError)
                {
                    SessionHelper.SaveDataInSession(CustomConstants.LiveResultSession, liveTestResultDashboardViewModel);
                    return View("~/Views/DBTM/DBTMLiveTestResult/LiveTestResult.cshtml", liveTestResultDashboardViewModel);
                }

                SetNotificationMessage(GetErrorNotificationMessage(liveTestResultDashboardViewModel.ErrorMessage));
            }

            return View("~/Views/DBTM/DBTMLiveTestResult/LiveTestResultLogin.cshtml", liveTestResultLoginViewModel);
        }

    }
}