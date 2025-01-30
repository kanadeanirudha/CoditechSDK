using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;
using Coditech.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Coditech.Admin.Controllers
{
    public class BioradMedisyMediaManagerController : BaseController
    {
        private readonly IBioradMedisyMediaManagerAgent _bioradMedisysAgent;
        private readonly IMediaManagerFolderAgent _mediaManagerFolderAgent;
        public BioradMedisyMediaManagerController(IBioradMedisyMediaManagerAgent bioradMedisysAgent, IMediaManagerFolderAgent mediaManagerFolderAgent)
        {
            _bioradMedisysAgent = bioradMedisysAgent;
            _mediaManagerFolderAgent = mediaManagerFolderAgent;
        }

        [HttpGet]
        public ActionResult GetMediaDetails(long mediaId)
        {
           BioradMedisyMediaModel  bioradMedisysViewModel = _bioradMedisysAgent.GetMediaDetails(mediaId);
            DropdownViewModel dropdownViewModel = new DropdownViewModel()
            {
                DropdownSelectedValue = bioradMedisysViewModel.TaskApprovalStatusEnumId.ToString(),
                DropdownName = "TaskApprovalStatusEnumId",
                GroupCode = "BioradMedisyApprovalStatus",
                AddSelectItem = false
            };

            List<GeneralEnumaratorModel> generalEnumaratorList = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession)?.GeneralEnumaratorList?.Where(x => x.EnumGroupCode == dropdownViewModel.GroupCode)?.OrderBy(y => y.SequenceNumber)?.ToList();
            List<SelectListItem> bioradMedisyApprovalStatusList = new List<SelectListItem>();
            if (generalEnumaratorList != null)
            {
                bioradMedisysViewModel.TaskApprovalStatusEnumCode = generalEnumaratorList?.FirstOrDefault(x => x.GeneralEnumaratorId == bioradMedisysViewModel.TaskApprovalStatusEnumId)?.EnumName;
                foreach (var item in generalEnumaratorList)
                {
                    bool disabled = true;
                    if (bioradMedisysViewModel.ApprovalSequenceNumber == 1 && (item.EnumName == "InProgress" || item.EnumName == "SubmittedForApproval"))
                    {
                        disabled = false;
                    }
                    else if (bioradMedisysViewModel.ApprovalSequenceNumber > 1 && !bioradMedisysViewModel.IsFinalApproval && (item.EnumName == "InReview" || item.EnumName == "SubmittedForApproval" || item.EnumName == "NeedChanges"))
                    {
                        disabled = false;
                    }
                    else if (bioradMedisysViewModel.IsFinalApproval && bioradMedisysViewModel.TaskApprovalStatusEnumCode == "Completed")
                    {
                        if (item.EnumName == "Completed" || item.EnumName == "NeedChanges")
                        {
                            disabled = false;
                        }
                    }
                    else if (bioradMedisysViewModel.IsFinalApproval && (item.EnumName == "InReview" || item.EnumName == "Completed" || item.EnumName == "NeedChanges"))
                    {
                        disabled = false;
                    }
                    bioradMedisyApprovalStatusList.Add(new SelectListItem()
                    {
                        Text = item.EnumDisplayText,
                        Value = Convert.ToString(item.GeneralEnumaratorId),
                        Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.GeneralEnumaratorId),
                        Disabled = disabled
                    });
                }
            }
            ViewData["BioradMedisyApprovalStatus"] = bioradMedisyApprovalStatusList;
            return ActionView("~/Views/BioradMedisys/ViewMediaDetails.cshtml", bioradMedisysViewModel);
        }

        [HttpPost]
        public ActionResult Edit(BioradMedisyMediaModel bioradMedisyMediaModel)
        {
            if (ModelState.IsValid)
            {
                SetNotificationMessage(_bioradMedisysAgent.UpdateFileApprovalFlow(bioradMedisyMediaModel).HasError
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));
                return RedirectToAction("GetMediaDetails", new { mediaId = bioradMedisyMediaModel.MediaId });
            }
            return ActionView("~/Views/BioradMedisys/ViewMediaDetails.cshtml", bioradMedisyMediaModel);
        }

        public virtual ActionResult ReplaceFile(int folderId, long mediaId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (mediaId == 0)
                {
                    return Json(new { success = false, message = "Failed to replace media." });
                }
                IFormFileCollection filess = Request.Form.Files;
                if (filess.Count == 0)
                {
                    return Json(new { success = false, message = "No file uploaded." });
                }

                IFormFile file = filess[0];

                if (file.Length == 0)
                {
                    return Json(new { success = false, message = "Empty file uploaded." });
                }

                MediaModel uploadMediaModel = _mediaManagerFolderAgent.UploadFile(folderId, mediaId, file);

                SetNotificationMessage(!uploadMediaModel.HasError
                       ? GetSuccessNotificationMessage("File successfully replaced.")
                       : GetErrorNotificationMessage(uploadMediaModel.ErrorMessage));
                return RedirectToAction("GetMediaDetails", new { mediaId = mediaId });
            }
            else
            {
                return RedirectToAction<UserController>(x => x.Login(string.Empty));
            }
        }
    }
}