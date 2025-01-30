using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Resources;
using System.Collections.Specialized;
namespace Coditech.API.Service
{
    public class BioradMedisyMediaManagerService : MediaManagerService, IBioradMedisyMediaManagerService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<TaskMaster> _taskMasterMasterRepository;
        private readonly ICoditechRepository<TaskApprovalSetting> _taskApprovalSettingRepository;
        private readonly ICoditechRepository<TaskApprovalTransaction> _taskApprovalTransactionRepository;
        private readonly ICoditechRepository<MediaDetail> _mediaDetailRepository;
        private readonly ICoditechRepository<UserMaster> _userMasterRepository;
        public BioradMedisyMediaManagerService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider, IWebHostEnvironment environment) : base(coditechLogging, serviceProvider, environment)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _taskMasterMasterRepository = new CoditechRepository<TaskMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _taskApprovalSettingRepository = new CoditechRepository<TaskApprovalSetting>(_serviceProvider.GetService<Coditech_Entities>());
            _taskApprovalTransactionRepository = new CoditechRepository<TaskApprovalTransaction>(_serviceProvider.GetService<Coditech_Entities>());
            _mediaDetailRepository = new CoditechRepository<MediaDetail>(_serviceProvider.GetService<Coditech_Entities>());
            _userMasterRepository = new CoditechRepository<UserMaster>(_serviceProvider.GetService<Coditech_Entities>());
        }

        public override MediaManagerResponse UploadMedia(int folderId, string folderName, long mediaId, IEnumerable<IFormFile> formFile, HttpRequest request)
        {
            MediaManagerResponse mediaManagerResponse = base.UploadMedia(folderId, folderName, mediaId, formFile, request);
            if (!mediaManagerResponse.HasError && mediaId == 0)
            {
                InsertFileApprovalFlow(HelperMethods.GetLoginUserId(), mediaManagerResponse.MediaModel.MediaId);
            }
            return mediaManagerResponse;
        }
        public override MediaManagerFolderResponse GetMediaList(int rootFolderId, int adminRoleId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            MediaManagerFolderResponse mediaManagerFolderResponse = base.GetMediaList(rootFolderId, adminRoleId, filters, sorts, expands, pagingStart, pagingLength);
            if (mediaManagerFolderResponse?.MediaManagerFolderModel?.MediaFiles?.Count > 0)
            {
                List<long> mediaIds = mediaManagerFolderResponse.MediaManagerFolderModel.MediaFiles.Select(x => x.MediaId).ToList();
                _taskApprovalTransactionRepository.Table.Where(x => mediaIds.Contains(x.TablePrimaryColumnId) && x.IsCurrentStatus)?.ToList()?.ForEach(x =>
                {
                    MediaModel mediaModel = mediaManagerFolderResponse.MediaManagerFolderModel.MediaFiles.FirstOrDefault(y => y.MediaId == x.TablePrimaryColumnId);
                    if (mediaModel != null)
                    {
                        mediaModel.Custom1 = GetEnumCodeByEnumId(x.TaskApprovalStatusEnumId);
                        mediaModel.Custom2 = GetEnumDisplayTextByEnumId(x.TaskApprovalStatusEnumId);
                    }
                });
            }
            return mediaManagerFolderResponse;
        }
        public bool UpdateFileApprovalFlow(BioradMedisyMediaModel model)
        {
            long userId = HelperMethods.GetLoginUserId();
            long entityId = _userMasterRepository.Table.Where(x => x.UserMasterId == userId).Select(y => y.EntityId).FirstOrDefault();
            string taskApprovalStatusEnumCode = GetEnumCodeByEnumId(model.TaskApprovalStatusEnumId);
            TaskApprovalTransaction taskApprovalCurrentTransaction = _taskApprovalTransactionRepository.Table.Where(x => x.TablePrimaryColumnId == model.MediaId && x.IsCurrentStatus)?.FirstOrDefault();
            if (taskApprovalCurrentTransaction == null || taskApprovalCurrentTransaction?.TaskApprovalTransactionId <= 0)
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (taskApprovalStatusEnumCode == "SubmittedForApproval")
            {
                taskApprovalCurrentTransaction.IsCurrentStatus = false;
                _taskApprovalTransactionRepository.Update(taskApprovalCurrentTransaction);

                TaskMaster taskMaster = GetTaskMasterDetails();
                string centreCode = new CoditechRepository<EmployeeMaster>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.EmployeeId == entityId).Select(y => y.CentreCode).FirstOrDefault();
                TaskApprovalSetting taskApprovalSetting = _taskApprovalSettingRepository.Table.Where(x => x.CentreCode == centreCode && x.EmployeeId == entityId && x.TaskMasterId == taskMaster.TaskMasterId)?.FirstOrDefault();

                TaskApprovalSetting taskApprovalNextSetting = _taskApprovalSettingRepository.Table.Where(x => x.CentreCode == centreCode && x.TaskMasterId == taskMaster.TaskMasterId && x.ApprovalSequenceNumber == taskApprovalSetting.ApprovalSequenceNumber + 1)?.FirstOrDefault();
                taskApprovalCurrentTransaction.TaskApprovalTransactionId = 0;
                taskApprovalCurrentTransaction.TaskApprovalSettingId = taskApprovalNextSetting.TaskApprovalSettingId;
                taskApprovalCurrentTransaction.TaskApprovalStatusEnumId = model.TaskApprovalStatusEnumId;
                taskApprovalCurrentTransaction.IsCurrentStatus = true;
                taskApprovalCurrentTransaction.Comments = model.Comments;
                _taskApprovalTransactionRepository.Insert(taskApprovalCurrentTransaction);
                //MoveMediaFile(taskApprovalNextSetting.EmployeeId, model.MediaId, string.Empty);
            }
            else if (taskApprovalStatusEnumCode == "InReview" || taskApprovalStatusEnumCode == "InProgress")
            {
                taskApprovalCurrentTransaction.IsCurrentStatus = false;
                _taskApprovalTransactionRepository.Update(taskApprovalCurrentTransaction);

                taskApprovalCurrentTransaction.TaskApprovalTransactionId = 0;
                taskApprovalCurrentTransaction.IsCurrentStatus = true;
                taskApprovalCurrentTransaction.TaskApprovalStatusEnumId = model.TaskApprovalStatusEnumId;
                taskApprovalCurrentTransaction.Comments = model.Comments;
                _taskApprovalTransactionRepository.Insert(taskApprovalCurrentTransaction);

            }
            else if (taskApprovalStatusEnumCode == "NeedChanges")
            {
                taskApprovalCurrentTransaction.IsCurrentStatus = false;
                _taskApprovalTransactionRepository.Update(taskApprovalCurrentTransaction);

                TaskMaster taskMaster = GetTaskMasterDetails();
                string centreCode = new CoditechRepository<EmployeeMaster>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.EmployeeId == entityId).Select(y => y.CentreCode).FirstOrDefault();
                TaskApprovalSetting taskApprovalSetting = _taskApprovalSettingRepository.Table.Where(x => x.CentreCode == centreCode && x.EmployeeId == entityId && x.TaskMasterId == taskMaster.TaskMasterId)?.FirstOrDefault();
                TaskApprovalSetting taskApprovalNextSetting = _taskApprovalSettingRepository.Table.Where(x => x.CentreCode == centreCode && x.TaskMasterId == taskMaster.TaskMasterId && x.ApprovalSequenceNumber == taskApprovalSetting.ApprovalSequenceNumber - 1)?.FirstOrDefault();
                taskApprovalCurrentTransaction.TaskApprovalTransactionId = 0;
                taskApprovalCurrentTransaction.TaskApprovalSettingId = taskApprovalNextSetting.TaskApprovalSettingId;
                taskApprovalCurrentTransaction.TaskApprovalStatusEnumId = model.TaskApprovalStatusEnumId;
                taskApprovalCurrentTransaction.IsCurrentStatus = true;
                taskApprovalCurrentTransaction.Comments = model.Comments;
                _taskApprovalTransactionRepository.Insert(taskApprovalCurrentTransaction);
                //MoveMediaFile(taskApprovalNextSetting.EmployeeId, model.MediaId, string.Empty);
            }
            else if (taskApprovalStatusEnumCode == "Completed")
            {
                taskApprovalCurrentTransaction.IsCurrentStatus = false;
                _taskApprovalTransactionRepository.Update(taskApprovalCurrentTransaction);

                taskApprovalCurrentTransaction.TaskApprovalTransactionId = 0;
                taskApprovalCurrentTransaction.TaskApprovalStatusEnumId = model.TaskApprovalStatusEnumId;
                taskApprovalCurrentTransaction.IsCurrentStatus = true;
                taskApprovalCurrentTransaction.Comments = model.Comments;
                _taskApprovalTransactionRepository.Insert(taskApprovalCurrentTransaction);
                //MoveMediaFile(0, model.MediaId, "Final Documents");
            }
            return true;
        }

        public BioradMedisyMediaModel GetMediaDetails(long mediaId, long entityId)
        {
            MediaDetail mediaDetail = _mediaDetailRepository.Table.Where(x => x.MediaId == mediaId)?.FirstOrDefault();

            if (mediaDetail == null)
            {
                return new BioradMedisyMediaModel();
            }

            BioradMedisyMediaModel bioradMedisyMediaModel = mediaDetail.FromEntityToModel<BioradMedisyMediaModel>();
            bioradMedisyMediaModel.Path = $"{GetMediaUrl()}{bioradMedisyMediaModel.Path}";
            bioradMedisyMediaModel.Size = !string.IsNullOrEmpty(bioradMedisyMediaModel.Size) ? bioradMedisyMediaModel.Size : "0";
            bioradMedisyMediaModel.Type = bioradMedisyMediaModel.Type.Replace("application/", "");

            bioradMedisyMediaModel.FileHistoryList = (from a in _taskApprovalTransactionRepository.Table
                                                      join b in _userMasterRepository.Table on a.CreatedBy equals b.UserMasterId
                                                      join c in new CoditechRepository<GeneralEnumaratorMaster>(_serviceProvider.GetService<Coditech_Entities>()).Table
                                                       on a.TaskApprovalStatusEnumId equals c.GeneralEnumaratorId
                                                      where a.TablePrimaryColumnId == mediaId
                                                      orderby a.IsCurrentStatus ascending
                                                      orderby a.CreatedDate descending
                                                      select new BioradMedisyFileHistoryModel()
                                                      {
                                                          TaskApprovalSettingId = a.TaskApprovalSettingId,
                                                          UserName = $"{b.FirstName} {b.LastName}",
                                                          TaskApprovalStatusEnumId = a.TaskApprovalStatusEnumId,
                                                          IsCurrentStatus = a.IsCurrentStatus,
                                                          TaskApprovalStatusDisplayName = c.EnumDisplayText,
                                                          TaskApprovalStatusEnumCode = c.EnumName,
                                                          ChangeDate = Convert.ToDateTime(a.CreatedDate),
                                                          Comments = a.Comments,
                                                      })?.ToList();

            bioradMedisyMediaModel.FileHistoryList = bioradMedisyMediaModel.FileHistoryList ?? new List<BioradMedisyFileHistoryModel>();
            BioradMedisyFileHistoryModel bioradMedisyFileHistoryCurrentStatusModel = bioradMedisyMediaModel.FileHistoryList.FirstOrDefault(x => x.IsCurrentStatus);
            TaskApprovalSetting taskApprovalSettingCurrent = _taskApprovalSettingRepository.Table.Where(x => x.TaskApprovalSettingId == bioradMedisyFileHistoryCurrentStatusModel.TaskApprovalSettingId)?.FirstOrDefault();
            bioradMedisyMediaModel.IsEditable = HelperUtility.IsNotNull(taskApprovalSettingCurrent) && taskApprovalSettingCurrent.EmployeeId == entityId;
            bioradMedisyMediaModel.TaskApprovalStatusEnumId = Convert.ToInt32(bioradMedisyFileHistoryCurrentStatusModel.TaskApprovalStatusEnumId);
            if (bioradMedisyMediaModel.IsEditable)
            {
                TaskMaster taskMaster = GetTaskMasterDetails();
                if (taskMaster?.TaskMasterId > 0)
                {
                    string centreCode = new CoditechRepository<EmployeeMaster>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.EmployeeId == entityId).Select(y => y.CentreCode).FirstOrDefault();
                    TaskApprovalSetting taskApprovalSetting = _taskApprovalSettingRepository.Table.Where(x => x.CentreCode == centreCode && x.EmployeeId == entityId && x.TaskMasterId == taskMaster.TaskMasterId)?.FirstOrDefault();
                    if (HelperUtility.IsNotNull(taskApprovalSetting))
                    {
                        bioradMedisyMediaModel.ApprovalSequenceNumber = taskApprovalSetting.ApprovalSequenceNumber;
                        bioradMedisyMediaModel.IsFinalApproval = taskApprovalSetting.IsFinalApproval;
                    }
                }
            }
            return bioradMedisyMediaModel;
        }

        #region private Method
        private void InsertFileApprovalFlow(long userId, long mediaId)
        {
            TaskMaster taskMaster = GetTaskMasterDetails();
            if (taskMaster?.TaskMasterId > 0)
            {
                if (!_taskApprovalTransactionRepository.Table.Any(x => x.TablePrimaryColumnId == mediaId))
                {
                    long entityId = _userMasterRepository.Table.Where(x => x.UserMasterId == userId).Select(y => y.EntityId).FirstOrDefault();
                    string centreCode = new CoditechRepository<EmployeeMaster>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.EmployeeId == entityId).Select(y => y.CentreCode).FirstOrDefault();
                    TaskApprovalSetting taskApprovalSetting = _taskApprovalSettingRepository.Table.Where(x => x.CentreCode == centreCode && x.EmployeeId == entityId && x.TaskMasterId == taskMaster.TaskMasterId)?.FirstOrDefault();
                    if (taskApprovalSetting?.TaskApprovalSettingId > 0)
                    {
                        TaskApprovalTransaction taskApprovalTransaction = new TaskApprovalTransaction()
                        {
                            TaskApprovalSettingId = taskApprovalSetting.TaskApprovalSettingId,
                            TaskApprovalStatusEnumId = GetEnumIdByEnumCode("InProgress", "BioradMedisyApprovalStatus"),
                            IsCurrentStatus = true,
                            TablePrimaryColumnId = mediaId,
                            Comments = "Work In Progress"
                        };
                        _taskApprovalTransactionRepository.Insert(taskApprovalTransaction);
                    }
                }
            }
        }

        private TaskMaster GetTaskMasterDetails()
        {
            return _taskMasterMasterRepository.Table.Where(x => x.TaskCode == "MediaUploadApprovalTask" && x.IsActive)?.FirstOrDefault();
        }

        private int MoveMediaFile(long employeeId, long mediaId, string folderName)
        {
            int folderId = 0;
            if (employeeId > 0)
            {
                int adminRoleMasterId = new CoditechRepository<AdminRoleApplicableDetails>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.EmployeeId == employeeId).Select(y => y.AdminRoleMasterId).FirstOrDefault();
                folderId = new CoditechRepository<AdminRoleMediaFolders>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.AdminRoleMasterId == adminRoleMasterId).Select(y => y.MediaFolderMasterId).FirstOrDefault();
            }
            else if (!string.IsNullOrEmpty(folderName))
            {
                folderId = new CoditechRepository<MediaFolderMaster>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.FolderName == folderName).Select(y => y.MediaFolderMasterId).FirstOrDefault();

            }
            if (folderId > 0)
            {
                MediaDetail mediaDetail = _mediaDetailRepository.Table.Where(x => x.MediaId == mediaId)?.FirstOrDefault();
                mediaDetail.MediaFolderMasterId = folderId;
                _mediaDetailRepository.Update(mediaDetail);
            }
            return folderId;
        }
        #endregion
    }
}
