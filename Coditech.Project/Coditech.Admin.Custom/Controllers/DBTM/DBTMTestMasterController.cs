using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Resources;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Coditech.Admin.Controllers
{
    public class DBTMTestMasterController : BaseController
    {
        private readonly IDBTMTestAgent _dBTMTestAgent;
        private const string createEdit = "~/Views/DBTM/DBTMTestMaster/CreateEdit.cshtml";

        public DBTMTestMasterController(IDBTMTestAgent dBTMTestAgent)
        {
            _dBTMTestAgent = dBTMTestAgent;
        }

        public virtual ActionResult List(DataTableViewModel dataTableModel)
        {
            DBTMTestListViewModel list = _dBTMTestAgent.GetDBTMTestList(dataTableModel);
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/DBTM/DBTMTestMaster/_List.cshtml", list);
            }
            return View($"~/Views/DBTM/DBTMTestMaster/List.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            DBTMTestViewModel dBTMTestViewModel = new DBTMTestViewModel();
            BindDBTMTestParameter(dBTMTestViewModel);
            return View(createEdit, dBTMTestViewModel);
        }

        [HttpPost]
        public virtual ActionResult Create(DBTMTestViewModel dBTMTestViewModel)
        {
            if (ModelState.IsValid)
            {
                dBTMTestViewModel = _dBTMTestAgent.CreateDBTMTest(dBTMTestViewModel);
                if (!dBTMTestViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    return RedirectToAction("List", CreateActionDataTable());
                }
            }
            BindDBTMTestParameter(dBTMTestViewModel);
            SetNotificationMessage(GetErrorNotificationMessage(dBTMTestViewModel.ErrorMessage));
            return View(createEdit, dBTMTestViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(int dBTMTestMasterId)
        {
            DBTMTestViewModel dBTMTestViewModel = _dBTMTestAgent.GetDBTMTest(dBTMTestMasterId);
            BindDBTMTestParameter(dBTMTestViewModel);
            return ActionView(createEdit, dBTMTestViewModel);
        }

        [HttpPost]
        public virtual ActionResult Edit(DBTMTestViewModel dBTMTestViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_dBTMTestAgent.UpdateDBTMTest(dBTMTestViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                return RedirectToAction("Edit", new { dBTMTestMasterId = dBTMTestViewModel.DBTMTestMasterId });
            }
            BindDBTMTestParameter(dBTMTestViewModel);
            return View(createEdit, dBTMTestViewModel);
        }

        public virtual ActionResult Delete(string dBTMTestMasterIds)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(dBTMTestMasterIds))
            {
                status = _dBTMTestAgent.DeleteDBTMTest(dBTMTestMasterIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction<DBTMTestMasterController>(x => x.List(null));
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction<DBTMTestMasterController>(x => x.List(null));
        }

        #region Protected
        protected virtual void BindDBTMTestParameter(DBTMTestViewModel dBTMTestViewModel)
        {
            dBTMTestViewModel.DBTMTestParameterList = dBTMTestViewModel.DBTMTestParameterList ?? new List<SelectListItem>();
            DBTMTestParameterListViewModel parameterList = _dBTMTestAgent.DBTMTestParameter();

            if (parameterList?.DBTMTestParameterList != null)
            {
                foreach (var item in parameterList.DBTMTestParameterList)
                {
                    dBTMTestViewModel.DBTMTestParameterList.Add(new SelectListItem
                    {
                        Text = item.ParameterName,
                        Value = item.DBTMTestParameterId.ToString()
                    });
                }
            }
        }
        #endregion
    }
}