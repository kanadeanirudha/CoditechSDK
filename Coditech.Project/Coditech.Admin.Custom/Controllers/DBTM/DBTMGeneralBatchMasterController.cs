using Coditech.Admin.Agents;
using Coditech.Admin.ViewModel;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc;
using Coditech.Common.Helper;
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
        public virtual ActionResult Create(GeneralBatchViewModel generalBatchViewModel)
        {
            if (ModelState.IsValid)
            {
                generalBatchViewModel = _generalBatchAgent.CreateGeneralBatch(generalBatchViewModel);
                if (!generalBatchViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordAddedSuccessMessage));
                    return RedirectToAction("List", new { selectedCentreCode = generalBatchViewModel.CentreCode });
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(generalBatchViewModel.ErrorMessage));
            return View(createEditBatch, generalBatchViewModel);
        }

        [HttpGet]
        public virtual ActionResult UpdateGeneralBatch(int generalBatchMasterId)
        {
            GeneralBatchViewModel generalBatchViewModel = _generalBatchAgent.GetGeneralBatch(generalBatchMasterId);
            return ActionView(createEditBatch, generalBatchViewModel);
        }

        [HttpPost]
        public virtual ActionResult UpdateGeneralBatch(GeneralBatchViewModel generalBatchViewModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_generalBatchAgent.UpdateGeneralBatch(generalBatchViewModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                return RedirectToAction("UpdateGeneralBatch", new { generalBatchMasterId = generalBatchViewModel.GeneralBatchMasterId });
            }
            return View(createEditBatch, generalBatchViewModel);
        }

        protected virtual void BindDBTMBatchActivity(GeneralBatchViewModel generalBatchViewModel)
        {
            generalBatchViewModel.CustomDropdownList1 = generalBatchViewModel.CustomDropdownList1 ?? new List<SelectListItem>();
            DataTableViewModel dataTableModel = new DataTableViewModel();
            DBTMBatchActivityListViewModel dBTMBatchActivityList = _dBTMBatchActivityAgent.GetDBTMBatchActivityList(generalBatchViewModel.GeneralBatchMasterId, dataTableModel);

            if (dBTMBatchActivityList?.CustomDropdownList1 != null)
            {
                foreach (var item in dBTMBatchActivityList.CustomDropdownList1)
                {
                    generalBatchViewModel.CustomDropdownList1.Add(new SelectListItem
                    {
                        Text = item.Text,
                        Value = item.Value
                    });
                }
            }
        }


    }
}


