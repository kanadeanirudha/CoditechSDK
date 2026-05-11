using Coditech.Admin.Agents;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.Admin.Controllers
{
    public class DBTMCommonCustomController : BaseController
    {
        private readonly IGeneralCommonAgent _generalCommonAgent;
        private readonly IDBTMGeneralCommonAgent _dBTMGeneralCommonAgent;
        public DBTMCommonCustomController(IGeneralCommonAgent generalCommonAgent, IDBTMGeneralCommonAgent dBTMGeneralCommonAgent)
        {
            _generalCommonAgent = generalCommonAgent;
            _dBTMGeneralCommonAgent = dBTMGeneralCommonAgent;
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
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetDBTMDeviceDataDecrypted(string dBTMDeviceDataIds)
        {
            DBTMDeviceDataDetailsModel model = new DBTMDeviceDataDetailsModel();

            if (!string.IsNullOrEmpty(dBTMDeviceDataIds))
            {
                model = _dBTMGeneralCommonAgent.GetDBTMDeviceDataDecrypted(dBTMDeviceDataIds);
            }

            return View("~/Views/DBTM/DBTMGeneralCommon/DBTMDeviceDataDecryptedView.cshtml", model);
        }
    }
}
