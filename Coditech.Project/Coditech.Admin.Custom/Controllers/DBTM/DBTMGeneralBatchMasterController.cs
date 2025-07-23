using Coditech.Admin.Agents;
using Coditech.Admin.ViewModel;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Coditech.Admin.Controllers
{
    public class DBTMGeneralBatchMasterController : BaseController
    {
        private readonly IGeneralBatchAgent _generalBatchAgent;
        private const string createEditBatch = "~/Views/GeneralMaster/GeneralBatchMaster/CreateEditGeneralBatch.cshtml";
        private readonly IDBTMBatchActivityAgent _dBTMBatchActivityAgent;
        private readonly IDBTMTestAgent _dBTMTestAgent;

        public DBTMGeneralBatchMasterController(IGeneralBatchAgent generalBatchAgent, IDBTMBatchActivityAgent dBTMBatchActivityAgent, IDBTMTestAgent dBTMTestAgent)
        {
            _generalBatchAgent = generalBatchAgent;
            _dBTMBatchActivityAgent = dBTMBatchActivityAgent;
            _dBTMTestAgent = dBTMTestAgent;
        }

        [HttpGet]
        public ActionResult Create()
        {
            GeneralBatchViewModel generalBatchViewModel = new GeneralBatchViewModel();
            BindDBTMBatchActivity(generalBatchViewModel);
            return View(createEditBatch, generalBatchViewModel);
        }

        [HttpPost]
        public ActionResult Create(GeneralBatchViewModel generalBatchViewModel)
        {
            if (generalBatchViewModel?.CustomDropdownSelectedValue1?.Count > 0)
            {
                if (ModelState.IsValid)
                {
                    generalBatchViewModel = _generalBatchAgent.CreateGeneralBatch(generalBatchViewModel);
                    if (!generalBatchViewModel.HasError)
                    {
                        SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                        return RedirectToAction<GeneralBatchMasterController>(x => x.List(new DataTableViewModel { SelectedCentreCode = generalBatchViewModel.CentreCode }));
                    }
                }
                SetNotificationMessage(GetErrorNotificationMessage(generalBatchViewModel.ErrorMessage));
            }
            else
            {
                SetNotificationMessage(GetErrorNotificationMessage("Please Select Activity."));
            }
            BindDBTMBatchActivity(generalBatchViewModel);
            return View(createEditBatch, generalBatchViewModel);
        }

        [HttpGet]
        public ActionResult UpdateGeneralBatch(int generalBatchMasterId)
        {
            GeneralBatchViewModel generalBatchViewModel = _generalBatchAgent.GetGeneralBatch(generalBatchMasterId);
            BindDBTMBatchActivity(generalBatchViewModel);
            return ActionView(createEditBatch, generalBatchViewModel);
        }

        [HttpPost]
        public ActionResult UpdateGeneralBatch(GeneralBatchViewModel generalBatchViewModel)
        {
            if (generalBatchViewModel?.CustomDropdownSelectedValue1?.Count > 0)
            {
                if (ModelState.IsValid)
                {
                    SetNotificationMessage(_generalBatchAgent.UpdateGeneralBatch(generalBatchViewModel).HasError
                    ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                    : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                    return RedirectToAction("UpdateGeneralBatch", new { generalBatchMasterId = generalBatchViewModel.GeneralBatchMasterId });
                }
            }
            else
            {
                SetNotificationMessage(GetErrorNotificationMessage("Please Select Activity."));
            }
            BindDBTMBatchActivity(generalBatchViewModel);
            return View(createEditBatch, generalBatchViewModel);
        }

        protected void BindDBTMBatchActivity(GeneralBatchViewModel generalBatchViewModel)
        {
            generalBatchViewModel.CustomDropdownList1 = generalBatchViewModel.CustomDropdownList1 ?? new List<SelectListItem>();
            DataTableViewModel dataTableModel = new DataTableViewModel() { };
            DBTMTestListViewModel dBTMBatchActivityList = _dBTMTestAgent.GetDBTMTestList(dataTableModel);

            if (dBTMBatchActivityList?.DBTMTestList != null)
            {
                foreach (var item in dBTMBatchActivityList.DBTMTestList)
                {
                    generalBatchViewModel.CustomDropdownList1.Add(new SelectListItem
                    {
                        Text = item.TestName,
                        Value = item.DBTMTestMasterId.ToString(),
                    });
                }
            }
        }
    }
}


