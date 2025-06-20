using Coditech.Admin.Agents;
using Coditech.Admin.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
            return View("~/Views/DBTM/DBTMLiveTestResult/LiveTestResultLogin.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(string username,string password,string deviceCode, string testcode)
        {
            LiveTestResultDashboardViewModel liveTestResultDashboardViewModel = _liveTestResultDashboardAgent.GetLiveTestResultDashboard();
            return View("~/Views/DBTM/DBTMLiveTestResult/LiveTestResult.cshtml", liveTestResultDashboardViewModel);
        }
    }
}