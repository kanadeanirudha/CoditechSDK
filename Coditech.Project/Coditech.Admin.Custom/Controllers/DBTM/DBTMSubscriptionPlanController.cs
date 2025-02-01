using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Resources;

using Microsoft.AspNetCore.Mvc;

namespace Coditech.Admin.Controllers
{
    public class DBTMSubscriptionPlanController : BaseController
    {
        private readonly IDBTMSubscriptionPlanAgent _dBTMSubscriptionPlanAgent;
        private const string createEditSubscriptionPlan = "~/Views/DBTM/DBTMSubscriptionPlan/CreateEditDBTMSubscriptionPlan.cshtml";

        public DBTMSubscriptionPlanController(IDBTMSubscriptionPlanAgent dBTMSubscriptionPlanAgent)
        {
            _dBTMSubscriptionPlanAgent = dBTMSubscriptionPlanAgent;
        }

        public virtual ActionResult List(DataTableViewModel dataTableModel)
        {
            DBTMSubscriptionPlanListViewModel list = new DBTMSubscriptionPlanListViewModel();
            
            list = _dBTMSubscriptionPlanAgent.GetDBTMSubscriptionPlanList(dataTableModel);
            
           
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMSubscriptionPlan/_List.cshtml", list);
            }
            return View($"~/Views/DBTM/DBTMSubscriptionPlan/List.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            return View(createEditSubscriptionPlan, new DBTMSubscriptionPlanViewModel());
        }

        [HttpPost]
        public virtual ActionResult Create(DBTMSubscriptionPlanViewModel dBTMSubscriptionPlanViewModel)
        {
            if (ModelState.IsValid)
            {
                dBTMSubscriptionPlanViewModel = _dBTMSubscriptionPlanAgent.CreateDBTMSubscriptionPlan(dBTMSubscriptionPlanViewModel);
                if (!dBTMSubscriptionPlanViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    return RedirectToAction("List", CreateActionDataTable());
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(dBTMSubscriptionPlanViewModel.ErrorMessage));
            return View(createEditSubscriptionPlan, dBTMSubscriptionPlanViewModel);
        }

        [HttpGet]
        public virtual ActionResult UpdateDBTMSubscriptionPlan(int dBTMSubscriptionPlanId)
        {
            DBTMSubscriptionPlanViewModel dBTMSubscriptionPlanViewModel = _dBTMSubscriptionPlanAgent.GetDBTMSubscriptionPlan(dBTMSubscriptionPlanId);
            return ActionView(createEditSubscriptionPlan, dBTMSubscriptionPlanViewModel);
        }

        [HttpPost]
        public virtual ActionResult UpdateDBTMSubscriptionPlan(DBTMSubscriptionPlanViewModel dBTMSubscriptionPlanViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_dBTMSubscriptionPlanAgent.UpdateDBTMSubscriptionPlan(dBTMSubscriptionPlanViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                return RedirectToAction("UpdateDBTMSubscriptionPlan", new { dBTMSubscriptionPlanId = dBTMSubscriptionPlanViewModel.DBTMSubscriptionPlanId });
            }
            return View(createEditSubscriptionPlan, dBTMSubscriptionPlanViewModel);
        }

        public virtual ActionResult Delete(string dBTMSubscriptionPlanIds)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(dBTMSubscriptionPlanIds))
            {
                status = _dBTMSubscriptionPlanAgent.DeleteDBTMSubscriptionPlan(dBTMSubscriptionPlanIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction<DBTMSubscriptionPlanController>(x => x.List(null));
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction<DBTMSubscriptionPlanController>(x => x.List(null));
        }

        #region PlanActivity
        public virtual ActionResult GetDBTMSubscriptionPlanActivityList(DataTableViewModel dataTableViewModel)
        {
            DBTMSubscriptionPlanActivityListViewModel list = _dBTMSubscriptionPlanAgent.GetDBTMSubscriptionPlanActivityList(Convert.ToInt32(dataTableViewModel.SelectedParameter1), dataTableViewModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMSubscriptionPlan/_PlanActivityList.cshtml", list);
            }
            list.SelectedParameter1 = dataTableViewModel.SelectedParameter1;
            return View($"~/Views/DBTM/DBTMSubscriptionPlan/PlanActivityList.cshtml", list);


        }

        [HttpGet]
        public virtual ActionResult GetAssociateUnAssociatePlanActivity(DBTMSubscriptionPlanActivityViewModel dBTMSubscriptionPlanActivityViewModel)
        {
            return PartialView("~/Views/DBTM/DBTMSubscriptionPlan/_AssociateUnAssociatePlanActivity.cshtml", dBTMSubscriptionPlanActivityViewModel);
        }

        [HttpPost]
        public virtual ActionResult AssociateUnAssociatePlanActivity(DBTMSubscriptionPlanActivityViewModel dBTMSubscriptionPlanActivityViewModel)
        {
            SetNotificationMessage(_dBTMSubscriptionPlanAgent.AssociateUnAssociatePlanActivity(dBTMSubscriptionPlanActivityViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
            return RedirectToAction("GetDBTMSubscriptionPlanActivityList", new DataTableViewModel { SelectedParameter1 = dBTMSubscriptionPlanActivityViewModel.DBTMSubscriptionPlanId.ToString() });
        }
        #endregion

        public virtual ActionResult Cancel()
        {
            DataTableViewModel dataTableViewModel = new DataTableViewModel();
            return RedirectToAction("List", dataTableViewModel);
        }
    }
}
