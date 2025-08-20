using Coditech.Admin.Agents;
using Coditech.Admin.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.Admin.Controllers
{
    public class DBTMCommonCustomController : BaseController
    {
        private readonly IGeneralCommonAgent _generalCommonAgent;
        public DBTMCommonCustomController(IGeneralCommonAgent generalCommonAgent)
        {
            _generalCommonAgent = generalCommonAgent;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetTermsAndCondition()
        {
            string termsAndCondition = string.Empty;

            CoditechApplicationSettingListViewModel coditechApplicationSettingListViewModel =
                _generalCommonAgent.GetCoditechApplicationSettingList("TermsAndCondition");

            if (IsNotNull(coditechApplicationSettingListViewModel) &&
                coditechApplicationSettingListViewModel.CoditechApplicationSettingList?.Count > 0)
            {
                termsAndCondition = coditechApplicationSettingListViewModel.CoditechApplicationSettingList.FirstOrDefault().ApplicationValue3;
            }
            return PartialView("~/Views/Shared/PageTemplates/_dBTMTermsAndCondition.cshtml", termsAndCondition);
        }
    }
}
