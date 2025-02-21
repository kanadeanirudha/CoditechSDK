using Coditech.API.Data;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;

using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMGeneralTrainerMasterService : GeneralTrainerMasterService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<AdminSanctionPost> _adminSanctionPostRepository;
        private readonly ICoditechRepository<AdminRoleMaster> _adminRoleMasterRepository;
        private readonly ICoditechRepository<GeneralPerson> _generalPersonRepository;
        public DBTMGeneralTrainerMasterService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(coditechLogging, serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _adminSanctionPostRepository = new CoditechRepository<AdminSanctionPost>(_serviceProvider.GetService<Coditech_Entities>());
            _adminRoleMasterRepository = new CoditechRepository<AdminRoleMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _generalPersonRepository = new CoditechRepository<GeneralPerson>(_serviceProvider.GetService<Coditech_Entities>());
        }

        protected override GeneralPersonModel GetGeneralPersonDetailsByEntityType(long entityId, string entityType)
        {
            long personId = 0;
            string centreCode = string.Empty;
            string personCode = string.Empty;
            short generalDepartmentMasterId = 0;
            if (entityType == UserTypeEnum.Trainee.ToString())
            {
                DBTMTraineeDetails dbtmTraineeDetails = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>()).Table.FirstOrDefault(x => x.DBTMTraineeDetailId == entityId);
                if (IsNotNull(dbtmTraineeDetails))
                {
                    personId = dbtmTraineeDetails.PersonId;
                    centreCode = dbtmTraineeDetails.CentreCode;
                }
                return base.BindGeneralPersonInformation(personId, centreCode, personCode, generalDepartmentMasterId);
            }
            else
            {
                return base.GetGeneralPersonDetailsByEntityType(entityId, entityType);
            }
        }

        public override GeneralTrainerModel CreateTrainer(GeneralTrainerModel generalTrainerModel)
        {
            generalTrainerModel = base.CreateTrainer(generalTrainerModel);
            if (!generalTrainerModel.HasError)
            {
                DateTime currentDate = DateTime.Now;
                var employeeMaster = new CoditechRepository<EmployeeMaster>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => x.EmployeeId == generalTrainerModel.EmployeeId)?.Select(y => new { y.CentreCode, y.PersonId })?.FirstOrDefault();
                GeneralPerson generalPerson = _generalPersonRepository.Table.Where(x => x.PersonId == employeeMaster.PersonId)?.FirstOrDefault();
                if (string.IsNullOrEmpty(generalPerson.Custom1))
                {
                    int adminSanctionPostId = _adminSanctionPostRepository.Table.Where(x => x.CentreCode == employeeMaster.CentreCode && x.DepartmentId == ApiCustomSettings.TrainerDepartmentId && x.DesignationId == ApiCustomSettings.TrainerDesignationId).Select(y => y.AdminSanctionPostId).FirstOrDefault();
                    if (adminSanctionPostId > 0)
                    {
                        var adminRoleMaster = _adminRoleMasterRepository.Table.Where(x => x.AdminSanctionPostId == adminSanctionPostId).FirstOrDefault();

                        if (adminRoleMaster?.AdminRoleMasterId > 0)
                        {
                            // Create the AdminRoleApplicableDetails object
                            AdminRoleApplicableDetails adminRoleApplicableDetails = new AdminRoleApplicableDetails()
                            {
                                AdminRoleMasterId = adminRoleMaster.AdminRoleMasterId,
                                EmployeeId = generalTrainerModel.EmployeeId,
                                IsActive = true,
                                CreatedDate = currentDate,
                                ModifiedDate = currentDate,
                            };
                            new CoditechRepository<AdminRoleApplicableDetails>(_serviceProvider.GetService<Coditech_Entities>()).Insert(adminRoleApplicableDetails);
                        }
                    }
                    else
                    {
                        string sanctionPostCode = string.Empty;
                        //Insert Admin Role
                        InsertAdminRole(currentDate, ApiCustomSettings.TrainerDepartmentId, employeeMaster.CentreCode, generalTrainerModel.EmployeeId, ApiCustomSettings.TrainerDesignationId, DashboardFormCustomEnum.DBTMTrainerDashboard.ToString(), ApiCustomSettings.DBTMTrainerMenuCode.Split(",").ToList(), out sanctionPostCode);
                    }
                    generalPerson.Custom1 = "DBTMTrainer";
                    _generalPersonRepository.Update(generalPerson);
                }
                return generalTrainerModel;
            }
            return generalTrainerModel;
        }
        protected virtual void InsertAdminRole(DateTime currentDate, short departmentId, string centreCode, long employeeId, short designationId, string dashboardFormCustomEnum, List<string> associateMenus, out string sanctionPostCode)
        {
            sanctionPostCode = string.Empty;
            AdminSanctionPostModel adminSanctionPostModel = new AdminSanctionPostModel()
            {
                DesignationId = designationId,
                DepartmentId = departmentId,
                NoOfPost = 1,
                IsActive = true,
                CentreCode = centreCode,
                DesignationType = "Regular",
                PostType = "Permanent",
                CreatedDate = currentDate,
                ModifiedDate = currentDate
            };
            EmployeeDesignationMaster employeeDesignationMaster = GetDesignationDetails(adminSanctionPostModel.DesignationId);
            GeneralDepartmentMaster generalDepartmentMaster = GetDepartmentDetails(adminSanctionPostModel.DepartmentId);

            sanctionPostCode = adminSanctionPostModel.SanctionPostCode = $"{employeeDesignationMaster.ShortCode}-{generalDepartmentMaster.DepartmentShortCode}-{adminSanctionPostModel.CentreCode}";
            adminSanctionPostModel.SanctionedPostDescription = $"{employeeDesignationMaster.Description}-{generalDepartmentMaster.DepartmentName}-{adminSanctionPostModel.PostType}-{adminSanctionPostModel.DesignationType}";
            AdminSanctionPost adminSanctionPostEntity = adminSanctionPostModel.FromModelToEntity<AdminSanctionPost>();

            //Create new adminSanctionPost and return it.
            AdminSanctionPost adminSanctionPostData = new CoditechRepository<AdminSanctionPost>(_serviceProvider.GetService<Coditech_Entities>()).Insert(adminSanctionPostEntity);
            if (adminSanctionPostData?.AdminSanctionPostId > 0)
            {
                adminSanctionPostModel.AdminSanctionPostId = adminSanctionPostData.AdminSanctionPostId;

                AdminRoleMaster adminRoleMaster = new AdminRoleMaster()
                {
                    AdminSanctionPostId = adminSanctionPostModel.AdminSanctionPostId,
                    SanctionPostName = adminSanctionPostModel.SanctionedPostDescription,
                    MonitoringLevel = APIConstant.Self,
                    AdminRoleCode = adminSanctionPostModel.SanctionPostCode,
                    OthCentreLevel = string.Empty,
                    IsActive = true,
                    DashboardFormEnumId = GetEnumIdByEnumCode(dashboardFormCustomEnum, GeneralEnumaratorGroupCodeEnum.DashboardForm.ToString()),
                    CreatedDate = currentDate,
                    ModifiedDate = currentDate
                };
                //Create new adminRoleMaster
                adminRoleMaster = new CoditechRepository<AdminRoleMaster>(_serviceProvider.GetService<Coditech_Entities>()).Insert(adminRoleMaster);
                AdminRoleCentreRights adminRoleCentreRight = new AdminRoleCentreRights()
                {
                    AdminRoleMasterId = adminRoleMaster.AdminRoleMasterId,
                    CentreCode = adminSanctionPostModel.CentreCode,
                    IsActive = true,
                    CreatedDate = currentDate,
                    ModifiedDate = currentDate
                };

                //Create new adminRoleCentreRight
                adminRoleCentreRight = new CoditechRepository<AdminRoleCentreRights>(_serviceProvider.GetService<Coditech_Entities>()).Insert(adminRoleCentreRight);
                AdminRoleApplicableDetails adminRoleApplicableDetails = new AdminRoleApplicableDetails()
                {
                    AdminRoleMasterId = adminRoleMaster.AdminRoleMasterId,
                    EmployeeId = employeeId,
                    IsActive = true,
                    RoleType = "Regular",
                    CreatedDate = currentDate,
                    ModifiedDate = currentDate
                };
                new CoditechRepository<AdminRoleApplicableDetails>(_serviceProvider.GetService<Coditech_Entities>()).Insert(adminRoleApplicableDetails);

                //insert admin Role Menu Detail
                List<UserMainMenuMaster> menuList = new CoditechRepository<UserMainMenuMaster>(_serviceProvider.GetService<Coditech_Entities>()).Table.Where(x => associateMenus.Contains(x.MenuCode)).ToList();
                List<AdminRoleMenuDetails> adminRoleMenuDetailList = new List<AdminRoleMenuDetails>();
                foreach (var menu in menuList)
                {
                    adminRoleMenuDetailList.Add(new AdminRoleMenuDetails()
                    {
                        AdminRoleMasterId = adminRoleMaster.AdminRoleMasterId,
                        AdminRoleCode = adminRoleMaster.AdminRoleCode,
                        ModuleCode = menu.ModuleCode,
                        MenuCode = menu.MenuCode,
                        EnableDate = currentDate,
                        IsActive = true,
                        CreatedDate = currentDate,
                        ModifiedDate = currentDate
                    });
                }
                new CoditechRepository<AdminRoleMenuDetails>(_serviceProvider.GetService<Coditech_Entities>()).Insert(adminRoleMenuDetailList);
            }
        }

    }
}
