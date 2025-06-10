using Coditech.Admin.Agents;
using Coditech.Admin.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Coditech.Admin.Controllers
{
    public class LiveTestResultDashboardController : BaseController
    {
        private readonly ILiveTestResultDashboardAgent _liveTestResultDashboardAgent;
        private const string create = "~/Views/DBTM/DBTMLiveTestResult/LiveTestResult.cshtml";

        public LiveTestResultDashboardController(ILiveTestResultDashboardAgent liveTestResultDashboardAgent)
        {
            _liveTestResultDashboardAgent = liveTestResultDashboardAgent;
        }
        

        [HttpGet]
        public virtual ActionResult LiveTestResultDashboard()
        {
            LiveTestResultDashboardViewModel liveTestResultDashboardViewModel = _liveTestResultDashboardAgent.GetLiveTestResultDashboard();
            return View(create);
        }
    }
}