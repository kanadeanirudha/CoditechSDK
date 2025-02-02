using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;

using Microsoft.AspNetCore.Mvc;

namespace Coditech.Admin.Controllers
{
    public class DBTMMySubscriptionPlanController : BaseController
    {
        private readonly IDBTMMySubscriptionPlanAgent _dBTMMySubscriptionPlanAgent;

        public DBTMMySubscriptionPlanController(IDBTMMySubscriptionPlanAgent dBTMMySubscriptionPlanAgent)
        {
            _dBTMMySubscriptionPlanAgent = dBTMMySubscriptionPlanAgent;
        }

        public virtual ActionResult List(DataTableViewModel dataTableModel)
        {
            DBTMMySubscriptionPlanListViewModel list = _dBTMMySubscriptionPlanAgent.GetDBTMMySubscriptionPlanList(dataTableModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMMySubscriptionPlan/_List.cshtml", list);
            }
            return View($"~/Views/DBTM/DBTMMySubscriptionPlan/List.cshtml", list);
        }
    }
}
