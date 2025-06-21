using Coditech.Admin.Agents;
using Coditech.Admin.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Coditech.Resources;

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
            LiveTestResultLoginViewModel liveTestResultLoginViewModel = new LiveTestResultLoginViewModel();
            return View("~/Views/DBTM/DBTMLiveTestResult/LiveTestResultLogin.cshtml", liveTestResultLoginViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(LiveTestResultLoginViewModel liveTestResultLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                liveTestResultLoginViewModel = _liveTestResultDashboardAgent.GetLiveTestResultDashboard(liveTestResultLoginViewModel);
                if (!liveTestResultLoginViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    return RedirectToAction("Index");
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(liveTestResultLoginViewModel.ErrorMessage));
            return View("~/Views/DBTM/DBTMLiveTestResult/LiveTestResultLogin.cshtml", liveTestResultLoginViewModel);
        }
    }
}