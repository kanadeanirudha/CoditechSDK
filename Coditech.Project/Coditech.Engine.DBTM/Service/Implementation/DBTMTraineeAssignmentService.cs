using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;
using System.Collections.Specialized;
using System.Data;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMTraineeAssignmentService : BaseService, IDBTMTraineeAssignmentService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMTraineeAssignment> _dBTMTraineeAssignmentRepository;
        private readonly ICoditechRepository<GeneralTrainerMaster> _generalTrainerRepository;
        private readonly ICoditechRepository<EmployeeMaster> _employeeMasterRepository;
        private readonly ICoditechRepository<GeneralPerson> _generalPersonRepository;
        private readonly ICoditechRepository<DBTMTraineeDetails> _dBTMTraineeDetailsRepository;
        private readonly ICoditechRepository<GeneralTraineeAssociatedToTrainer> _generalTraineeAssociatedToTrainerRepository;
        private readonly ICoditechRepository<DBTMTestMaster> _dBTMTestRepository;
        private readonly ICoditechRepository<DBTMTraineeAssignmentToUser> _dBTMTraineeAssignmentToUserRepository;

        public DBTMTraineeAssignmentService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMTraineeAssignmentRepository = new CoditechRepository<DBTMTraineeAssignment>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _generalTrainerRepository = new CoditechRepository<GeneralTrainerMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _employeeMasterRepository = new CoditechRepository<EmployeeMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _generalPersonRepository = new CoditechRepository<GeneralPerson>(_serviceProvider.GetService<Coditech_Entities>());
            _dBTMTraineeDetailsRepository = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _generalTraineeAssociatedToTrainerRepository = new CoditechRepository<GeneralTraineeAssociatedToTrainer>(_serviceProvider.GetService<Coditech_Entities>());
            _dBTMTestRepository = new CoditechRepository<DBTMTestMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTraineeAssignmentToUserRepository = new CoditechRepository<DBTMTraineeAssignmentToUser>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        public DBTMTraineeAssignmentListModel GetDBTMTraineeAssignmentList(long generalTrainerMasterId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMTraineeAssignmentModel> objStoredProc = new CoditechViewRepository<DBTMTraineeAssignmentModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@GeneralTrainerMasterId", generalTrainerMasterId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMTraineeAssignmentModel> dBTMTraineeAssignmentList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMTraineeAssignmentList @GeneralTrainerMasterId,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 5, out pageListModel.TotalRowCount)?.ToList();
            DBTMTraineeAssignmentListModel listModel = new DBTMTraineeAssignmentListModel();

            listModel.DBTMTraineeAssignmentList = dBTMTraineeAssignmentList?.Count > 0 ? dBTMTraineeAssignmentList : new List<DBTMTraineeAssignmentModel>();
            listModel.BindPageListModel(pageListModel);
            listModel.GeneralTrainerMasterId = generalTrainerMasterId;
            return listModel;
        }

        //Create DBTMTraineeAssignment
        public DBTMTraineeAssignmentModel CreateDBTMTraineeAssignment(DBTMTraineeAssignmentModel dBTMTraineeAssignmentModel)
        {
            if (IsNull(dBTMTraineeAssignmentModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);
            if (IsNull(dBTMTraineeAssignmentModel.SelectedTest))
                throw new CoditechException(ErrorCodes.InvalidData, "Selected Activity cannot be null.");
            if (IsNull(dBTMTraineeAssignmentModel.SelectedTrainee))
                throw new CoditechException(ErrorCodes.InvalidData, "Selected Trainee cannot be null.");

            DBTMTraineeAssignment dBTMTraineeAssignment = dBTMTraineeAssignmentModel.FromModelToEntity<DBTMTraineeAssignment>();

            int dBTMTestStatusEnumId = GetEnumIdByEnumCode("Pending", DropdownCustomTypeEnum.DBTMTestStatus.ToString());
            List<DBTMTraineeAssignmentToUser> dBTMTraineeAssignmentToUserData = new List<DBTMTraineeAssignmentToUser>();
            foreach (string dBTMTestMasterId in dBTMTraineeAssignmentModel.SelectedTest)
            {
                dBTMTraineeAssignment.DBTMTestMasterId = Convert.ToInt32(dBTMTestMasterId);
                dBTMTraineeAssignment.DBTMTraineeAssignmentId = 0;
                DBTMTraineeAssignment dBTMTraineeAssignmentData = _dBTMTraineeAssignmentRepository.Insert(dBTMTraineeAssignment);
                if (dBTMTraineeAssignmentData.DBTMTraineeAssignmentId > 0)
                    foreach (var dBTMTraineeDetailId in dBTMTraineeAssignmentModel?.SelectedTrainee)
                    {
                        dBTMTraineeAssignmentToUserData.Add(new DBTMTraineeAssignmentToUser
                        {
                            DBTMTraineeAssignmentId = dBTMTraineeAssignmentData.DBTMTraineeAssignmentId,
                            DBTMTraineeDetailId = Convert.ToInt64(dBTMTraineeDetailId),
                            DBTMTestStatusEnumId = dBTMTestStatusEnumId
                        });
                    }
            }

            if (dBTMTraineeAssignmentToUserData.Any())
                _dBTMTraineeAssignmentToUserRepository.Insert(dBTMTraineeAssignmentToUserData);

            return dBTMTraineeAssignmentModel;
        }
        // Get DBTMTraineeAssignment by dBTMTraineeAssignmentId.
        public DBTMTraineeAssignmentModel GetDBTMTraineeAssignment(long dBTMTraineeAssignmentId)
        {
            if (dBTMTraineeAssignmentId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTraineeAssignmentId"));

            // Get the DBTMTraineeAssignment entity.
            DBTMTraineeAssignment dBTMTraineeAssignment = _dBTMTraineeAssignmentRepository.Table
                .FirstOrDefault(x => x.DBTMTraineeAssignmentId == dBTMTraineeAssignmentId);

            DBTMTraineeAssignmentModel dBTMTraineeAssignmentModel = dBTMTraineeAssignment?.FromEntityToModel<DBTMTraineeAssignmentModel>();

            // Get trainer details if assigned
            if (dBTMTraineeAssignmentModel?.GeneralTrainerMasterId > 0)
            {
                long employeeId = _generalTrainerRepository.Table
                    .Where(x => x.GeneralTrainerMasterId == dBTMTraineeAssignmentModel.GeneralTrainerMasterId)
                    .Select(y => y.EmployeeId)
                    .FirstOrDefault();

                GeneralPersonModel generalTrainerDetails = GetGeneralPersonDetailsByEntityType(employeeId, UserTypeEnum.Employee.ToString());
                if (IsNotNull(generalTrainerDetails))
                {
                    dBTMTraineeAssignmentModel.SelectedCentreCode = generalTrainerDetails.SelectedCentreCode;
                }
            }
            var data = (from a in _dBTMTraineeAssignmentRepository.Table
                        join b in _dBTMTraineeAssignmentToUserRepository.Table
                            on a.DBTMTraineeAssignmentId equals b.DBTMTraineeAssignmentId
                        join c in _dBTMTraineeDetailsRepository.Table
                            on b.DBTMTraineeDetailId equals c.DBTMTraineeDetailId
                        join e in _dBTMTestRepository.Table
                            on a.DBTMTestMasterId equals e.DBTMTestMasterId
                        where a.DBTMTraineeAssignmentId == dBTMTraineeAssignmentId
                        select new
                        {
                            c.PersonId,
                            e.TestName
                        }).FirstOrDefault();

            if (data != null && data.PersonId > 0)
            {
                var person = _generalPersonRepository.Table
                                .Where(p => p.PersonId == data.PersonId)
                                .Select(p => new { p.FirstName, p.LastName })
                                .FirstOrDefault();


                dBTMTraineeAssignmentModel.FirstName = person.FirstName;
                dBTMTraineeAssignmentModel.LastName = person.LastName;
                dBTMTraineeAssignmentModel.TestName = data?.TestName ?? string.Empty;

            }

            return dBTMTraineeAssignmentModel;
        }

        //Update DBTMTraineeAssignment.
        public bool UpdateDBTMTraineeAssignment(DBTMTraineeAssignmentModel dBTMTraineeAssignmentModel)
        {
            if (IsNull(dBTMTraineeAssignmentModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (dBTMTraineeAssignmentModel.DBTMTraineeAssignmentId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTraineeAssignmentID"));

            DBTMTraineeAssignment existingAssignment = _dBTMTraineeAssignmentRepository.Table.FirstOrDefault(x => x.DBTMTraineeAssignmentId == dBTMTraineeAssignmentModel.DBTMTraineeAssignmentId);

            if (existingAssignment == null)
                throw new CoditechException(ErrorCodes.NotFound, "Trainee assignment not found.");

            existingAssignment.AssignmentDate = dBTMTraineeAssignmentModel.AssignmentDate;
            existingAssignment.AssignmentTime = dBTMTraineeAssignmentModel.AssignmentTime;

            bool isUpdated = _dBTMTraineeAssignmentRepository.Update(existingAssignment);

            if (!isUpdated)
            {
                dBTMTraineeAssignmentModel.HasError = true;
                dBTMTraineeAssignmentModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }

            return isUpdated;
        }

        //Delete DBTMTraineeAssignment.
        public bool DeleteDBTMTraineeAssignment(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTraineeAssignmentUserId"));

            long traineeAssignmentUserId = Convert.ToInt64(parameterModel.Ids);

            long traineeAssignmentId = _dBTMTraineeAssignmentToUserRepository.Table.Where(x => x.DBTMTraineeAssignmentUserId == traineeAssignmentUserId).Select(x => x.DBTMTraineeAssignmentId).FirstOrDefault();

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<CoditechCustom_Entities>());

            objStoredProc.SetParameter("DBTMTraineeAssignmentUserId", traineeAssignmentUserId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("DBTMTraineeAssignmentId", traineeAssignmentId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);

            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteDBTMTraineeAssignment @DBTMTraineeAssignmentUserId, @DBTMTraineeAssignmentId, @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        public DBTMTraineeAssignmentModel SendAssignmentReminder(long dBTMTraineeAssignmentId, long dBTMTraineeAssignmentUserId)
        {
            if (dBTMTraineeAssignmentId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, $"Invalid assignment ID: {dBTMTraineeAssignmentId}");
            var assignmentData = (from a in _dBTMTraineeAssignmentRepository.Table
                                  join b in _dBTMTraineeAssignmentToUserRepository.Table
                                      on a.DBTMTraineeAssignmentId equals b.DBTMTraineeAssignmentId
                                  join c in _dBTMTraineeDetailsRepository.Table
                                      on b.DBTMTraineeDetailId equals c.DBTMTraineeDetailId
                                  join e in _dBTMTestRepository.Table
                                      on a.DBTMTestMasterId equals e.DBTMTestMasterId
                                  where a.DBTMTraineeAssignmentId == dBTMTraineeAssignmentId
                                        && b.DBTMTraineeAssignmentUserId == dBTMTraineeAssignmentUserId
                                  select new
                                  {
                                      a.DBTMTraineeAssignmentId,
                                      a.GeneralTrainerMasterId,
                                      a.AssignmentDate,
                                      c.PersonId,
                                      c.CentreCode,
                                      e.TestName
                                  }).FirstOrDefault();

            if (assignmentData == null)
                return null;

            GeneralPerson person = _generalPersonRepository.Table.Where(p => p.PersonId == assignmentData.PersonId)
                                 .Select(p => new GeneralPerson
                                 {
                                     FirstName = p.FirstName,
                                     LastName = p.LastName,
                                     EmailId = p.EmailId
                                 }).FirstOrDefault();

            DBTMTraineeAssignmentModel dBTMTraineeAssignmentReminderModel = new DBTMTraineeAssignmentModel
            {
                DBTMTraineeAssignmentId = assignmentData.DBTMTraineeAssignmentId,
                GeneralTrainerMasterId = assignmentData.GeneralTrainerMasterId,
                AssignmentDate = assignmentData.AssignmentDate,
                FirstName = person?.FirstName,
                LastName = person?.LastName,
                EmailId = person?.EmailId,
                TestName = assignmentData.TestName,
                SelectedCentreCode = assignmentData.CentreCode
            };

            if (dBTMTraineeAssignmentReminderModel == null)
                throw new CoditechException(ErrorCodes.NullModel, "Reminder data not found.");

            if (string.IsNullOrWhiteSpace(dBTMTraineeAssignmentReminderModel.EmailId))
                throw new CoditechException(ErrorCodes.NullModel, "Email ID not found for trainee.");

            dBTMTraineeAssignmentReminderModel.CentreName =
                base.GetOrganisationCentreNameByCentreCode(dBTMTraineeAssignmentReminderModel.SelectedCentreCode);

            string templateCode = EmailTemplateCodeCustomEnum.DBTMSendPendingAssignmentReminder.ToString();
            var emailTemplate = base.GetEmailTemplateByCode(dBTMTraineeAssignmentReminderModel.SelectedCentreCode, templateCode);

            if (emailTemplate == null || string.IsNullOrWhiteSpace(emailTemplate.EmailTemplate))
                throw new CoditechException(ErrorCodes.NullModel, $"Email template '{templateCode}' not found for centre '{dBTMTraineeAssignmentReminderModel.CentreName}'.");

            string subject = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.CentreName, dBTMTraineeAssignmentReminderModel.SelectedCentreCode, emailTemplate.Subject);

            string messageText = emailTemplate.EmailTemplate;
            messageText = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.FirstName, dBTMTraineeAssignmentReminderModel.FirstName, messageText);
            messageText = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.LastName, dBTMTraineeAssignmentReminderModel.LastName, messageText);
            messageText = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.TestName, dBTMTraineeAssignmentReminderModel.TestName, messageText);
            messageText = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.AssignmentDate, dBTMTraineeAssignmentReminderModel.AssignmentDate.ToString("dd MMM yyyy"), messageText);
            messageText = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.CentreName, dBTMTraineeAssignmentReminderModel.CentreName, messageText);

            _coditechEmail.SendEmail(dBTMTraineeAssignmentReminderModel.SelectedCentreCode, dBTMTraineeAssignmentReminderModel.EmailId, "", subject, messageText, true);

            return dBTMTraineeAssignmentReminderModel;
        }
        public GeneralTrainerListModel GetTrainerByCentreCode(string centreCode)
        {
            var list = new GeneralTrainerListModel();

            list.GeneralTrainerList = (from a in _generalTrainerRepository.Table
                                       join b in _employeeMasterRepository.Table
                                        on a.EmployeeId equals b.EmployeeId
                                       join c in _generalPersonRepository.Table
                                       on b.PersonId equals c.PersonId

                                       where (b.CentreCode == centreCode || centreCode == null)

                                       select new GeneralTrainerModel()
                                       {
                                           GeneralTrainerMasterId = a.GeneralTrainerMasterId,
                                           FirstName = c.FirstName,
                                           LastName = c.LastName,
                                       }).ToList();

            return list;
        }
        public DBTMTraineeDetailsListModel GetTraineeDetailByCentreCodeAndgeneralTrainerId(string centreCode, long generalTrainerId)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(null, null, 0, 0);
            CoditechViewRepository<DBTMTraineeDetailsModel> objStoredProc = new CoditechViewRepository<DBTMTraineeDetailsModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@CentreCode", centreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@GeneralTrainerMasterId", generalTrainerId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMTraineeDetailsModel> dBTMTraineeDetailsList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetGetTraineeDetailListByCentreCodeAndgeneralTrainerId @CentreCode,@GeneralTrainerMasterId,@RowsCount OUT", 2, out pageListModel.TotalRowCount)?.ToList();
            DBTMTraineeDetailsListModel listModel = new DBTMTraineeDetailsListModel();

            listModel.DBTMTraineeDetailsList = dBTMTraineeDetailsList?.Count > 0 ? dBTMTraineeDetailsList : new List<DBTMTraineeDetailsModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }
        #region DBTMTraineeAssignmentToUser
        public DBTMTraineeAssignmentToUserListModel GetDBTMTraineeAssignmentToUserList(long dBTMTraineeAssignmentId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMTraineeAssignmentToUserModel> objStoredProc = new CoditechViewRepository<DBTMTraineeAssignmentToUserModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@DBTMTraineeAssignmentId", dBTMTraineeAssignmentId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMTraineeAssignmentToUserModel> AssignmentList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMTraineeAssignmentUserAssociatedList @DBTMTraineeAssignmentId,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 5, out pageListModel.TotalRowCount)?.ToList();
            DBTMTraineeAssignmentToUserListModel listModel = new DBTMTraineeAssignmentToUserListModel();

            listModel.DBTMTraineeAssignmentToUserList = AssignmentList?.Count > 0 ? AssignmentList : new List<DBTMTraineeAssignmentToUserModel>();
            listModel.BindPageListModel(pageListModel);


            if (dBTMTraineeAssignmentId > 0)
            {
                DBTMTraineeAssignmentModel model = GetDBTMTraineeAssignment(dBTMTraineeAssignmentId);
                if (IsNotNull(listModel))
                {
                    listModel.TestName = model.TestName;
                }
            }
            listModel.DBTMTraineeAssignmentId = dBTMTraineeAssignmentId;
            return listModel;
        }
        public bool AssociateUnAssociateAssignmentwiseUser(DBTMTraineeAssignmentToUserModel dBTMTraineeAssignmentToUserModel)
        {
            bool isAssociateUnAssociateAssignmentwiseUser = false;

            DBTMTraineeAssignmentToUser dBTMTraineeAssignmentToUser = new DBTMTraineeAssignmentToUser();
            if (dBTMTraineeAssignmentToUserModel.DBTMTraineeAssignmentUserId > 0)
            {
                dBTMTraineeAssignmentToUser = _dBTMTraineeAssignmentToUserRepository.Table.Where(x => x.DBTMTraineeAssignmentUserId == dBTMTraineeAssignmentToUserModel.DBTMTraineeAssignmentUserId)?.FirstOrDefault();
                isAssociateUnAssociateAssignmentwiseUser = _dBTMTraineeAssignmentToUserRepository.Delete(dBTMTraineeAssignmentToUser);
            }
            else
            {
                dBTMTraineeAssignmentToUser = dBTMTraineeAssignmentToUserModel.FromModelToEntity<DBTMTraineeAssignmentToUser>();
                dBTMTraineeAssignmentToUser = _dBTMTraineeAssignmentToUserRepository.Insert(dBTMTraineeAssignmentToUser);
                isAssociateUnAssociateAssignmentwiseUser = dBTMTraineeAssignmentToUser.DBTMTraineeAssignmentUserId > 0;
            }

            if (!isAssociateUnAssociateAssignmentwiseUser)
            {
                dBTMTraineeAssignmentToUserModel.HasError = true;
                dBTMTraineeAssignmentToUserModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isAssociateUnAssociateAssignmentwiseUser;
        }
        #endregion
        #region Protected Method

        #endregion
    }
}
