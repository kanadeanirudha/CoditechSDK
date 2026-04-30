using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Engine.DBTM.Helpers;
using Coditech.Resources;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Collections.Specialized;
using System.Data;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMTraineeDetailsService : BaseService, IDBTMTraineeDetailsService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMReportsService _dBTMReportsService;
        private readonly ICoditechRepository<DBTMTraineeDetails> _dBTMTraineeDetailsRepository;
        private readonly ICoditechRepository<DBTMDeviceData> _dBTMDeviceDataRepository;
        private readonly ICoditechRepository<DBTMTestMaster> _dBTMTestMasterRepository;
        private readonly ICoditechRepository<GeneralEnumaratorMaster> _generalEnumaratorMasterRepository;
        private readonly ICoditechRepository<GeneralTraineeAssociatedToTrainer> _generalTraineeAssociatedToTrainerRepository;
        private readonly ICoditechRepository<UserMaster> _userMasterRepository;
        private readonly ICoditechRepository<GeneralTrainerMaster> _generalTrainerMasterRepository;
        private readonly ICoditechRepository<GeneralBatchMaster> _generalBatchMasterRepository;
        private readonly ICoditechRepository<GeneralBatchUser> _generalBatchUserRepository;
        private readonly ICoditechRepository<DBTMBatchActivity> _dbtmBatchActivityRepository;
        private readonly IConverter _converter;
        private readonly ICoditechRepository<DBTMCentreWiseSetting> _dBTMCentreWiseSettingRepository;

        #region public
        public DBTMTraineeDetailsService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider, IDBTMReportsService dBTMReportsService, IConverter converter) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _converter = converter;
            _dBTMReportsService = dBTMReportsService;
            _dBTMTraineeDetailsRepository = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMDeviceDataRepository = new CoditechRepository<DBTMDeviceData>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestMasterRepository = new CoditechRepository<DBTMTestMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _generalEnumaratorMasterRepository = new CoditechRepository<GeneralEnumaratorMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _generalTraineeAssociatedToTrainerRepository = new CoditechRepository<GeneralTraineeAssociatedToTrainer>(_serviceProvider.GetService<Coditech_Entities>()); ;
            _userMasterRepository = new CoditechRepository<UserMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _generalTrainerMasterRepository = new CoditechRepository<GeneralTrainerMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _dBTMCentreWiseSettingRepository = new CoditechRepository<DBTMCentreWiseSetting>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dbtmBatchActivityRepository = new CoditechRepository<DBTMBatchActivity>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _generalBatchMasterRepository = new CoditechRepository<GeneralBatchMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _generalBatchUserRepository = new CoditechRepository<GeneralBatchUser>(_serviceProvider.GetService<Coditech_Entities>());
        }

        public DBTMTraineeDetailsListModel GetDBTMTraineeDetailsList(string SelectedCentreCode, long generalTrainerMasterId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            string listType = "";
            string isActive = filters?.Find(x => string.Equals(x.FilterName, FilterKeys.IsActive, StringComparison.CurrentCultureIgnoreCase))?.FilterValue;
            if (!string.IsNullOrEmpty(isActive))
            {
                filters.RemoveAll(x => x.FilterName == FilterKeys.IsActive);
                listType = $"and IsActive={isActive}";
            }
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMTraineeDetailsModel> objStoredProc = new CoditechViewRepository<DBTMTraineeDetailsModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@CentreCode", SelectedCentreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@GeneralTrainerMasterId", generalTrainerMasterId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@ListType", listType, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMTraineeDetailsModel> dBTMTraineeDetailsList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMTraineeDetailsList @CentreCode,@GeneralTrainerMasterId,@listType,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 7, out pageListModel.TotalRowCount)?.ToList();
            DBTMTraineeDetailsListModel listModel = new DBTMTraineeDetailsListModel();

            listModel.DBTMTraineeDetailsList = dBTMTraineeDetailsList?.Count > 0 ? dBTMTraineeDetailsList : new List<DBTMTraineeDetailsModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }

        //Get DBTM Trainee Other Details
        public DBTMTraineeDetailsModel GetDBTMTraineeOtherDetails(long dBTMTraineeDetailId)
        {
            if (dBTMTraineeDetailId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTraineeDetailId"));

            DBTMTraineeDetails dBTMTraineeDetails = _dBTMTraineeDetailsRepository.Table.FirstOrDefault(x => x.DBTMTraineeDetailId == dBTMTraineeDetailId);
            DBTMTraineeDetailsModel dBTMTraineeDetailsModel = dBTMTraineeDetails?.FromEntityToModel<DBTMTraineeDetailsModel>();
            DBTMCentreWiseSetting dBTMCentreWiseSetting = _dBTMCentreWiseSettingRepository.Table.FirstOrDefault(x => x.CentreCode == dBTMTraineeDetails.CentreCode);
            if (IsNotNull(dBTMTraineeDetailsModel))
            {
                GeneralPersonModel generalPersonModel = GetGeneralPersonDetails(dBTMTraineeDetailsModel.PersonId);
                if (IsNotNull(dBTMTraineeDetailsModel))
                {
                    dBTMTraineeDetailsModel.FirstName = generalPersonModel.FirstName;
                    dBTMTraineeDetailsModel.LastName = generalPersonModel.LastName;
                    dBTMTraineeDetailsModel.IsActive = dBTMTraineeDetails.IsActive;
                    dBTMTraineeDetailsModel.TypeOfCentre = dBTMCentreWiseSetting?.TypeOfCentre;
                }
            }
            return dBTMTraineeDetailsModel;
        }

        //Update DBTM Trainee Other Details
        public bool UpdateDBTMTraineeOtherDetails(DBTMTraineeDetailsModel dBTMTraineeDetailsModel)
        {
            if (IsNull(dBTMTraineeDetailsModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (dBTMTraineeDetailsModel.DBTMTraineeDetailId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTraineeDetailId"));

            DBTMTraineeDetails dBTMTraineeDetails = _dBTMTraineeDetailsRepository.Table.FirstOrDefault(x => x.DBTMTraineeDetailId == dBTMTraineeDetailsModel.DBTMTraineeDetailId);

            bool isUpdated = false;
            if (IsNull(dBTMTraineeDetails))
            {
                return isUpdated;
            }
            dBTMTraineeDetails.PastInjuries = dBTMTraineeDetailsModel.PastInjuries;
            dBTMTraineeDetails.MedicalHistory = dBTMTraineeDetailsModel.MedicalHistory;
            dBTMTraineeDetails.GroupEnumId = dBTMTraineeDetailsModel.GroupEnumId;
            dBTMTraineeDetails.SourceEnumId = dBTMTraineeDetailsModel.SourceEnumId;
            dBTMTraineeDetails.OtherInformation = dBTMTraineeDetailsModel.OtherInformation;
            dBTMTraineeDetails.Weight = dBTMTraineeDetailsModel.Weight;
            dBTMTraineeDetails.Height = dBTMTraineeDetailsModel.Height;
            dBTMTraineeDetails.SpecializationEnumId = dBTMTraineeDetailsModel.SpecializationEnumId;
            dBTMTraineeDetails.SchoolName = dBTMTraineeDetailsModel.SchoolName;
            dBTMTraineeDetails.Section = dBTMTraineeDetailsModel.Section;
            dBTMTraineeDetails.Standard = dBTMTraineeDetailsModel.Standard;

            isUpdated = _dBTMTraineeDetailsRepository.Update(dBTMTraineeDetails);
            if (isUpdated)
            {
                ActiveInActiveUserLogin(dBTMTraineeDetails.IsActive, Convert.ToInt64(dBTMTraineeDetails.DBTMTraineeDetailId), UserTypeEnum.Trainee.ToString());
            }
            else
            {
                dBTMTraineeDetailsModel.HasError = true;
                dBTMTraineeDetailsModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isUpdated;
        }

        //Delete DBTM Trainee Details
        public bool DeleteDBTMTraineeDetails(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTraineeDetailId"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("DBTMTraineeDetailIds", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteDBTMTraineeDetails @DBTMTraineeDetailIds,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        //TraineeActivitiesList
        public DBTMActivitiesListModel GetTraineeActivitiesList(string personCode, int numberOfDaysRecord, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMActivitiesModel> objStoredProc = new CoditechViewRepository<DBTMActivitiesModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@PersonCode", personCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@NumberOfDaysRecord", numberOfDaysRecord, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMActivitiesModel> dBTMActivitiesList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetTraineeActivitiesList @PersonCode,@NumberOfDaysRecord,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 5, out pageListModel.TotalRowCount)?.ToList();
            DBTMActivitiesListModel listModel = new DBTMActivitiesListModel();

            listModel.ActivitiesList = dBTMActivitiesList?.Count > 0 ? dBTMActivitiesList : new List<DBTMActivitiesModel>();
            listModel.BindPageListModel(pageListModel);

            long? dBTMTraineeDetailId = _dBTMTraineeDetailsRepository.Table.Where(x => x.PersonCode == personCode)?.Select(y => y.DBTMTraineeDetailId)?.FirstOrDefault();
            if (dBTMTraineeDetailId > 0)
            {
                GeneralPersonModel generalPersonModel = GetDBTMGeneralPersonDetailsByEntityType((int)dBTMTraineeDetailId, UserTypeEnum.Trainee.ToString());
                if (IsNotNull(generalPersonModel))
                {
                    listModel.FirstName = generalPersonModel.FirstName;
                    listModel.LastName = generalPersonModel.LastName;
                    listModel.SelectedCentreCode = generalPersonModel.SelectedCentreCode;
                }
            }

            listModel.PersonCode = personCode;
            return listModel;
        }

        public DBTMActivitiesDetailsListModel GetTraineeActivitiesDetailsList(long dBTMDeviceDataId, long entityId, string userType, string centreCode, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            DBTMActivitiesDetailsListModel listModel = new DBTMActivitiesDetailsListModel();

            DBTMDeviceData dBTMDeviceData = _dBTMDeviceDataRepository.Table.Where(x => x.DBTMDeviceDataId == dBTMDeviceDataId)?.FirstOrDefault();

            if (IsNull(dBTMDeviceData))
                return listModel;

            DateTime activityDate = (dBTMDeviceData.CreatedDate ?? dBTMDeviceData.TestPerformedTime);

            long traineeDetailId = _dBTMTraineeDetailsRepository.Table.Where(x => x.PersonCode == dBTMDeviceData.PersonCode).Select(y => y.DBTMTraineeDetailId).FirstOrDefault();

            if (IsNull(traineeDetailId))
                return listModel;

            DBTMTestMaster dBTMTestMaster = _dBTMTestMasterRepository.Table.FirstOrDefault(x => x.TestCode == dBTMDeviceData.TestCode);

            if (IsNull(dBTMTestMaster))
                return listModel;

            GeneralPersonModel generalPersonModel = GetDBTMGeneralPersonDetailsByEntityType(traineeDetailId, UserTypeEnum.Trainee.ToString());

            if (IsNotNull(generalPersonModel))
            {
                listModel.FirstName = generalPersonModel.FirstName;
                listModel.LastName = generalPersonModel.LastName;
                listModel.PersonCode = generalPersonModel.PersonCode;
            }

            DBTMReportsListModel report = _dBTMReportsService.TestWiseMultipleReports(dBTMTestMaster.DBTMTestMasterId.ToString(), traineeDetailId, activityDate, activityDate, entityId, userType, centreCode, false, false);

            listModel.DataTable = report.DataTable;
            listModel.TestName = dBTMTestMaster.TestName;
            listModel.PersonCode = dBTMDeviceData.PersonCode;
            listModel.DataTable = report.DataTableList?.FirstOrDefault().Value;

            return listModel;
        }

        public DBTMTraineeProfileModel GetProfileDetails(long dBTMTraineeDetailId)
        {
            if (dBTMTraineeDetailId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTraineeDetailId"));

            long generalBatchMasterId = (
                                            from a in _generalBatchMasterRepository.Table
                                            join b in _generalBatchUserRepository.Table
                                                on a.GeneralBatchMasterId equals b.GeneralBatchMasterId
                                            where b.EntityId == dBTMTraineeDetailId
                                                  && b.UserType == UserTypeEnum.Trainee.ToString()
                                            select a.GeneralBatchMasterId
                                        ).FirstOrDefault();

            DBTMTraineeProfileModel dBTMTraineeProfileModel = GetProfileDetailsList(generalBatchMasterId, dBTMTraineeDetailId.ToString(), string.Empty, DateTime.Now.AddDays(-365), DateTime.Now)?.DBTMTraineeProfileList?.FirstOrDefault();
            dBTMTraineeProfileModel.GeneralBatchMasterId = generalBatchMasterId;
            GeneralBatchMaster batch = _generalBatchMasterRepository.Table.FirstOrDefault(x => x.GeneralBatchMasterId == generalBatchMasterId);
            if (batch != null)
            {
                dBTMTraineeProfileModel.BatchName = batch.BatchName;
            }
            return dBTMTraineeProfileModel;
        }

        public DBTMReportsListModel GenerateAthletePdfRemark(long dBTMTraineeDetailId, string remarks)
        {
            DBTMTraineeProfileModel profile = GetProfileDetails(dBTMTraineeDetailId);
            if (profile == null)
                throw new CoditechException(ErrorCodes.NullModel, "Trainee profile not found");

            string traineeName = $"{profile.FirstName}_{profile.LastName}".Trim('_');
            traineeName = string.Concat(traineeName.Split(Path.GetInvalidFileNameChars()));
            // GetTraineeProfileHtml
            string html = GetTraineeProfileHtml(dBTMTraineeDetailId, remarks);

            // Generate PDF
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "AthleteReportPdf");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName = $"Athlete_Profile_{traineeName}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            string filePath = Path.Combine(folderPath, fileName);

            var pdf = new HtmlToPdfDocument
            {
                GlobalSettings = { PaperSize = PaperKind.A4, Orientation = Orientation.Portrait, Out = filePath },
                Objects = { new ObjectSettings { HtmlContent = html, WebSettings = { DefaultEncoding = "utf-8" } } }
            };
            _converter.Convert(pdf);

            return new DBTMReportsListModel
            {
                FileName = fileName,
                FilePath = filePath
            };
        }

        //Get trainee profile html
        public string GetTraineeProfileHtml(long dBTMTraineeDetailId, string remarks)
        {
            if (dBTMTraineeDetailId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTraineeDetailId"));

            // Get trainee profile
            DBTMTraineeProfileModel profile = GetProfileDetails(dBTMTraineeDetailId);
            if (profile == null)
                throw new CoditechException(ErrorCodes.NullModel, "Trainee profile not found");

            string centreName = base.GetOrganisationCentreNameByCentreCode(profile.CentreCode);
            string templateCode = EmailTemplateCodeCustomEnum.TraineeReportTemplate.ToString();

            var emailTemplate = GetEmailTemplateByCode(profile.CentreCode, templateCode);

            if (string.IsNullOrWhiteSpace(emailTemplate?.EmailTemplate))
                throw new CoditechException(ErrorCodes.NullModel, $"Template '{templateCode}' not found for centre '{centreName}'");

            string html = emailTemplate.EmailTemplate;

            html = ReplaceTraineeTemplate(html, profile, remarks, centreName);
            return html;
        }

        public DBTMTraineeProfileListModel GetProfileDetailsList(long generalBatchMasterId, string dBTMTraineeDetailIds, string orderBy, DateTime FromDate, DateTime ToDate)
        {
            DBTMTraineeProfileListModel dBTMTraineeProfileListModel = new DBTMTraineeProfileListModel();
            CoditechViewRepository<DBTMTraineeProfileModel> objStoredProc = new CoditechViewRepository<DBTMTraineeProfileModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@DBTMTraineeDetailIds", dBTMTraineeDetailIds, ParameterDirection.Input, DbType.String);
            List<DBTMTraineeProfileModel> list = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMTraineeDetailsListByIds @DBTMTraineeDetailIds")?.ToList();

            if (list?.Count > 0)
            {
                var trainerList = from gtat in _generalTraineeAssociatedToTrainerRepository.Table
                                  join gtm in _generalTrainerMasterRepository.Table
                                      on gtat.GeneralTrainerMasterId equals gtm.GeneralTrainerMasterId
                                  join um in _userMasterRepository.Table
                                      on gtm.EmployeeId equals um.EntityId
                                  where dBTMTraineeDetailIds.Contains(gtat.EntityId.ToString())
                                        && gtat.UserType == UserTypeEnum.Trainee.ToString()
                                        && gtat.IsCurrentTrainer
                                        && um.UserType == UserTypeEnum.Employee.ToString()
                                  select new
                                  {
                                      DBTMTraineeDetailId = gtat.EntityId,
                                      TrainerName = um.FirstName + " " + um.LastName
                                  };
                List<DBTMTraineeProfilePerformanceModel> traineeProfilePerformanceList = null;
                DataTable dt = GetTraineePerformanceRankingDetails(generalBatchMasterId, FromDate, ToDate, out traineeProfilePerformanceList);
                string[] testName = null;
                if (dt?.Rows?.Count > 0)
                {
                    string[] testCode = dt.Columns.Cast<DataColumn>().Where(c => c.ColumnName != "FinalScore" && c.ColumnName.Contains("Score")).Select(c => c.ColumnName.Replace("Score", "")).ToArray();
                    testName = _dBTMTestMasterRepository.Table.Where(x => testCode.Contains(x.TestCode)).Select(x => x.TestName).ToArray();
                }
                foreach (var dBTMTraineeProfileModel in list)
                {
                    dBTMTraineeProfileModel.IsListView = true;
                    dBTMTraineeProfileModel.TotalDuration = dBTMTraineeProfileModel.DateOfJoining.HasValue
                      ? CalculateDuration(dBTMTraineeProfileModel.DateOfJoining.Value, DateTime.Now)
                      : "N/A";

                    dBTMTraineeProfileModel.TrainerName = trainerList.FirstOrDefault(x => x.DBTMTraineeDetailId == dBTMTraineeProfileModel.DBTMTraineeDetailId)?.TrainerName ?? "N/A";
                    dBTMTraineeProfileModel.TraineeProfilePerformanceList = traineeProfilePerformanceList.Where(x => x.DBTMTraineeDetailId == dBTMTraineeProfileModel.DBTMTraineeDetailId)?.ToList();
                    if (dt?.Rows?.Count > 0)
                    {
                        DataRow dataRow = dt.Rows.Find(dBTMTraineeProfileModel.DBTMTraineeDetailId);
                        if (dataRow != null)
                        {
                            dBTMTraineeProfileModel.Rank = dataRow["Rank"].ToString();
                            if (testName?.Count() > 2)
                                dBTMTraineeProfileModel.RadarChart = BindRadarChartDetails(dt, dataRow, testName);
                        }
                    }
                }
            }
            if (list?.Count > 0)
            {
                if (orderBy == "Rank")
                    list = list.OrderBy(x => Convert.ToInt32(x.Rank) == 0).ThenBy(x => Convert.ToInt32(x.Rank)).ToList();
                else if (orderBy == "FirstName")
                    list = list.OrderBy(x => x.FirstName).ToList();
                else if (orderBy == "LastName")
                    list = list.OrderBy(x => x.LastName).ToList();
            }
            dBTMTraineeProfileListModel.DBTMTraineeProfileList = list;
            return dBTMTraineeProfileListModel;
        }
        public List<DateTime> GetTraineeListActivityDates(string dBTMTraineeDetailIds, int generalBatchMasterId)
        {
            if (string.IsNullOrWhiteSpace(dBTMTraineeDetailIds))
                return new List<DateTime>();
            var traineeIds = dBTMTraineeDetailIds.Split(',').Select(id => Convert.ToInt64(id)).ToList();
            bool isAllTrainee = traineeIds.Contains(0);
            List<string> personCodes;
            if (isAllTrainee)
            {
                var allTraineeIds = _generalBatchUserRepository.Table.Where(x => x.GeneralBatchMasterId == generalBatchMasterId && x.UserType == "Trainee").Select(x => x.EntityId).ToList();
                personCodes = _dBTMTraineeDetailsRepository.Table.Where(x => allTraineeIds.Contains(x.DBTMTraineeDetailId)).Select(x => x.PersonCode).ToList();
            }
            else
            {
                personCodes = _dBTMTraineeDetailsRepository.Table.Where(x => traineeIds.Contains(x.DBTMTraineeDetailId)).Select(x => x.PersonCode).ToList();
            }
            if (!personCodes.Any())
                return new List<DateTime>();
            var dates = _dBTMDeviceDataRepository.Table.Where(x => personCodes.Contains(x.PersonCode)).Select(x => (x.CreatedDate ?? x.TestPerformedTime).Date).Distinct().OrderBy(x => x).ToList();
            return dates;
        }
        #endregion

        #region private
        private GeneralPersonModel GetDBTMGeneralPersonDetailsByEntityType(long entityId, string entityType)
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
                return base.BindGeneralPersonInformation(personId, centreCode, personCode, generalDepartmentMasterId, dbtmTraineeDetails.IsActive);
            }
            else
            {
                return base.GetGeneralPersonDetailsByEntityType(entityId, entityType);
            }
        }

        private string CalculateDuration(DateTime fromDate, DateTime toDate)
        {
            int years = toDate.Year - fromDate.Year;
            int months = toDate.Month - fromDate.Month;
            int days = toDate.Day - fromDate.Day;

            if (days < 0)
            {
                months--;
                days += DateTime.DaysInMonth(toDate.Year, toDate.Month == 1 ? 12 : toDate.Month - 1);
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }

            return $"{years} years {months} months {days} days";
        }

        public static string ImageUrlToBase64(string imageUrl)
        {
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            using var client = new HttpClient(handler);

            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
            client.DefaultRequestHeaders.Add("Accept", "*/*");

            HttpResponseMessage response = client.GetAsync(imageUrl).Result;

            response.EnsureSuccessStatusCode();

            byte[] bytes = response.Content.ReadAsByteArrayAsync().Result;

            string contentType =
                response.Content.Headers.ContentType?.MediaType ?? "image/png";

            string base64 = Convert.ToBase64String(bytes);

            return $"data:{contentType};base64,{base64}";
        }
        //Template Html Replacement
        private string ReplaceTraineeTemplate(string html, DBTMTraineeProfileModel profile, string remarks, string centreName)
        {
            string logoImage = ImageUrlToBase64("https://dbtm-prod-api-mediamanager.poweredsportstech.com/Data/ApplicationImages/ApplicationLogo.png");
            html = ReplaceTokenWithMessageText("#LogoImage#", logoImage, html);

            string personImage = ImageUrlToBase64("https://dbtm-prod-api-mediamanager.poweredsportstech.com/Data/Media/eeaf0d14-bff4-4f30-a159-e66cc24d750e_maleavatar.png");
            html = ReplaceTokenWithMessageText("#PersonImage#", personImage, html);

            //string logoImage = ConvertImageToBase64("https://dbtm-prod-api-mediamanager.poweredsportstech.com/Data/Media/a5a540e1-b5d0-4325-a3ec-754e9b2905ca_female.jpg");
            //html = ReplaceTokenWithMessageText("#LogoImage#", logoImage, html);

            html = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.FirstName, profile.FirstName, html);
            html = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.LastName, profile.LastName, html);
            html = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.DOB, profile.DateOfBirth?.ToString("dd-MMM-yyyy"), html);
            html = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.JoiningDate, profile.DateOfJoining?.ToString("dd-MMM-yyyy"), html);
            html = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.Weight, profile.Weight.ToString(), html);
            html = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.Height, profile.Height.ToString(), html);
            html = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.WeeklyHours, profile.WeekelyHours?.ToString("hh\\:mm"), html);
            html = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.TotalDuration, profile.TotalDuration, html);
            html = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.TrainerName, profile.TrainerName, html);
            html = html.Replace("#Batch#", profile.BatchName ?? "");
            html = html.Replace("#WeeklyHours#", profile.WeekelyHours?.ToString());
            html = html.Replace("#Sport#", profile.Sport ?? "");
            html = html.Replace("#Rank#", profile.Rank ?? "");
            html = html.Replace("#Session#", profile.Session ?? "");
            html = html.Replace("#Participants#", profile.Participants ?? "");
            html = ReplaceTokenWithMessageText("#ReportIssuedDate#", DateTime.Now.ToString("dd-MMM-yyyy"), html);

            html = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.CentreName, centreName, html);
            if (profile?.TraineeProfilePerformanceList?.Count > 0)
            {
                var traineeProfilePerformanceList = (profile.TraineeProfilePerformanceList ?? new List<DBTMTraineeProfilePerformanceModel>())
               .GroupBy(x => x.PerformanceMatrix).ToDictionary(g => g.Key, g => g.ToList());
                int maxRows = traineeProfilePerformanceList.Values.Max(g => g.Count);
                string performanceMatrixHtml = "<table width=\"100%\" cellpadding=\"10\" cellspacing=\"0\" style=\"border-collapse:collapse; margin-top:30px;\">";
                //Bind Table Header
                performanceMatrixHtml += " <thead><tr>";
                foreach (var matrix in traineeProfilePerformanceList.Keys)
                {
                    performanceMatrixHtml += "<th style=\"border:1px solid #333;padding:8px;background:#ee445d;color:#fff;\">" + matrix + "</th>";
                    performanceMatrixHtml += "<th style=\"border:1px solid #333;padding:8px;background:#182750;color:#fff;\">Score</th>";
                }
                performanceMatrixHtml += "</tr></thead>";
                //End Bind Table Header
                //Bind Table Rows
                performanceMatrixHtml += "<tbody>";
                for (int i = 0; i < maxRows; i++)
                {
                    performanceMatrixHtml += "<tr>";
                    foreach (var matrix in traineeProfilePerformanceList.Keys)
                    {
                        var tests = traineeProfilePerformanceList[matrix];
                        if (i < tests.Count)
                        {
                            performanceMatrixHtml += "<td style=\"border:1px solid #182650;padding: 8px;text-align:center;font-size:14px;\">" + tests[i].TestName + "</td>";
                            performanceMatrixHtml += "<td style=\"border:1px solid #182650;padding: 8px;text-align:center;font-size:14px;\">" + tests[i].Score + "</td>";
                        }
                        else
                        {
                            performanceMatrixHtml += "<td style=\"border:1px solid #182650;padding: 8px;text-align:center;font-size:14px;\">-</td>";
                            performanceMatrixHtml += "<td style=\"border:1px solid #182650;padding: 8px;text-align:center;font-size:14px;\">-</td>";
                        }
                    }
                    performanceMatrixHtml += "</tr>";
                }
                string remarksHtml = "<tr><td colspan=\"10\" style=\"border:1px solid #182650;padding: 8px;text-align:left;font-size:14px;\"><strong>Remark:</strong> #Remarks#</td></tr>";
                if (string.IsNullOrWhiteSpace(remarks))
                {
                    remarksHtml = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.Remarks, string.Empty, remarksHtml);
                }
                else
                {
                    remarksHtml = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.Remarks, remarks, remarksHtml);
                }
                performanceMatrixHtml += remarksHtml;
                performanceMatrixHtml += "</tbody>";
                //End Bind Table Rows
                performanceMatrixHtml += "</table>";
                html = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.DataTable, performanceMatrixHtml, html);
            }
            else
            {
                html = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.DataTable, "No Record Found", html);
            }
            return html;
        }

        private DataTable GetTraineePerformanceRankingDetails(long generalBatchMasterId, DateTime FromDate, DateTime ToDate, out List<DBTMTraineeProfilePerformanceModel> traineeProfilePerformanceList)
        {
            DataTable dt = new DataTable();
            traineeProfilePerformanceList = new List<DBTMTraineeProfilePerformanceModel>();
            if (generalBatchMasterId <= 0)
                return dt;

            CoditechViewRepository<DBTMTraineeProfilePerformanceRankingModel> objStoredProc = new CoditechViewRepository<DBTMTraineeProfilePerformanceRankingModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@GeneralBranchMasterId", generalBatchMasterId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@FromDate", FromDate, ParameterDirection.Input, DbType.Date);
            objStoredProc.SetParameter("@ToDate", ToDate, ParameterDirection.Input, DbType.Date);
            List<DBTMTraineeProfilePerformanceRankingModel> traineeProfilePerformanceRankDataList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMTraineeRanking @GeneralBranchMasterId,@FromDate, @ToDate")?.ToList();
            if (traineeProfilePerformanceRankDataList != null && traineeProfilePerformanceRankDataList.Count > 0)
            {
                traineeProfilePerformanceRankDataList.ForEach(x =>
                {
                    x.ParameterValue = x.IsEncrypted ? EncryptionHelper.Decrypt(x.ParameterValue) : x.ParameterValue;
                });

                var testList = traineeProfilePerformanceRankDataList
                               .Select(x => new { x.TestName, x.TestCode, x.PerformanceMatrix, x.TestOutputHigher, x.TestResultBasedon })
                               .Distinct()
                               .ToList();

                dt = new DataTable();
                List<string> excludeColumnNames = new List<string> { "DBTMTraineeDetailId", "Name", "FinalScore" };
                dt.Columns.Add("DBTMTraineeDetailId", typeof(long));
                dt.PrimaryKey = new DataColumn[] { dt.Columns["DBTMTraineeDetailId"] };
                dt.Columns.Add("Name", typeof(string));
                foreach (var test in testList)
                {
                    dt.Columns.Add(test.TestCode, typeof(double));
                    dt.Columns.Add($"{test.TestCode}Score", typeof(double));
                    excludeColumnNames.Add($"{test.TestCode}Score");
                }
                dt.Columns.Add("FinalScore", typeof(double));
                dt.Columns.Add("Rank", typeof(int));

                List<long> dbtmTraineeDetailIdList = traineeProfilePerformanceRankDataList
                    .Select(x => x.DBTMTraineeDetailId)
                    .Distinct()
                    .ToList();

                foreach (long traineeId in dbtmTraineeDetailIdList)
                {
                    var traineeProfile = traineeProfilePerformanceRankDataList.First(x => x.DBTMTraineeDetailId == traineeId);

                    DataRow dr = dt.NewRow();
                    dr["DBTMTraineeDetailId"] = traineeProfile.DBTMTraineeDetailId;
                    dr["Name"] = traineeProfile.Name;

                    var traineeProfileList = traineeProfilePerformanceRankDataList.Where(x => x.DBTMTraineeDetailId == traineeId);

                    foreach (var test in testList)
                    {
                        var testResultData = traineeProfileList
                            .Where(x => x.TestCode == test.TestCode)
                            .Select(x => new { x.TestResultBasedon, x.TestOutputHigher })
                            .FirstOrDefault();

                        if (testResultData == null)
                        {
                            dr[test.TestCode] = 0d;
                            continue;
                        }

                        var groupedData = traineeProfileList.Where(x => x.TestCode == test.TestCode && x.ParameterCode == testResultData.TestResultBasedon)
                                        .GroupBy(x => x.CreatedDate)
                                        .Select(g => new
                                        {
                                            CreatedDate = g.Key,
                                            ParameterValueSum = g.Sum(x =>
                                                string.IsNullOrEmpty(x.ParameterValue) ? 0 : Convert.ToDouble(x.ParameterValue))
                                        }).ToList();

                        if (!groupedData.Any())
                        {
                            dr[test.TestCode] = 0d;
                            continue;
                        }

                        double value = 0;

                        if (testResultData.TestOutputHigher == "LO")
                            value = groupedData.Min(x => x.ParameterValueSum);
                        else if (testResultData.TestOutputHigher == "HO")
                            value = groupedData.Max(x => x.ParameterValueSum);

                        dr[test.TestCode] = value;
                        DBTMTraineeProfilePerformanceModel dBTMTraineeProfilePerformanceModel = new DBTMTraineeProfilePerformanceModel
                        {
                            DBTMTraineeDetailId = traineeProfile.DBTMTraineeDetailId,
                            TestCode = test.TestCode,
                            TestName = testList.First(x => x.TestCode == test.TestCode).TestName,
                            PerformanceMatrix = testList.First(x => x.TestCode == test.TestCode).PerformanceMatrix,
                            Score = $"{value} {DBTMCustomHelper.Unit(testList.First(x => x.TestCode == test.TestCode).TestResultBasedon)} (Total {testList.First(x => x.TestCode == test.TestCode).TestResultBasedon}) "
                        };

                        traineeProfilePerformanceList.Add(dBTMTraineeProfilePerformanceModel);
                    }

                    dt.Rows.Add(dr);
                }

                /*
                    Define Weights for the Final Score				
                    All activities are equally weighted by default.				
                    Each activity weight = 1 ÷ total number of activities.				
                    Total of all weights equals 1.				
                    Weight can be changed as per required (sepcific requirement of sport) based on the final score and rank will be updated				
                 */
                foreach (DataColumn column in dt.Columns.Cast<DataColumn>().Where(c => !excludeColumnNames.Contains(c.ColumnName)))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (var test in testList.Where(x => x.TestCode == column.ColumnName))
                        {
                            if (dr[column] != DBNull.Value)
                            {
                                double testValue = Convert.ToDouble(dr[column]);
                                double score = 0;
                                if (testValue > 0)
                                {
                                    /*
                                        Score Calculation: 
                                        If the test output is higher the better (HO), then Score = (Test Value - Minimum Value) ÷ (Maximum Value - Minimum Value) × 100. 
                                        If the test output is lower the better (LO), then Score = ((Maximum Value - Test Value) ÷ (Maximum Value - Minimum Value)) × 100. 
                                     */
                                    double maxValue = Convert.ToDouble(dt.Compute($"MAX([{column.ColumnName}])", ""));
                                    double minValue = Convert.ToDouble(dt.Compute($"MIN([{column.ColumnName}])", ""));
                                    if (test.TestOutputHigher == "HO")
                                    {
                                        score = (testValue - minValue) / (maxValue - minValue) * 100;
                                    }
                                    else if (test.TestOutputHigher == "LO")
                                    {
                                        score = ((maxValue - testValue) / (maxValue - minValue)) * 100;
                                    }
                                }
                                dr[$"{test.TestCode}Score"] = score;
                            }
                        }
                    }
                }
                double weights = 100 / testList.Count;
                //Calculate Final Score
                foreach (DataRow dr in dt.Rows)
                {
                    double finalScore = 0;

                    foreach (var test in testList)
                    {
                        double value = dr.Field<double?>($"{test.TestCode}Score") ?? 0;
                        finalScore += (value * weights) / 100;
                    }

                    dr["FinalScore"] = Math.Round(finalScore, 3);
                }
                //Calculate Rank
                var rankedList = dt.AsEnumerable()
                                   .OrderByDescending(r => r.Field<double>("FinalScore"))
                                   .Select((r, index) => new
                                   {

                                       DBTMTraineeDetailId = r.Field<long>("DBTMTraineeDetailId"),
                                       Name = r.Field<string>("Name"),
                                       FinalScore = r.Field<double>("FinalScore"),
                                       Rank = index + 1
                                   }).ToList();
                foreach (var item in rankedList)
                {
                    DataRow dr = dt.Rows.Find(item.DBTMTraineeDetailId);
                    if (dr != null)
                    {
                        dr["Rank"] = item.Rank;
                    }
                }
            }
            return dt;
        }

        //private List<DBTMTraineeProfilePerformanceModel> GetTraineePerformanceDetails(string dBTMTraineeDetailIds, DateTime FromDate, DateTime ToDate)
        //{
        //    List<DBTMTraineeProfilePerformanceModel> listModel = new List<DBTMTraineeProfilePerformanceModel>();
        //    CoditechViewRepository<DBTMTraineeProfilePerformanceModel> objStoredProc = new CoditechViewRepository<DBTMTraineeProfilePerformanceModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
        //    objStoredProc.SetParameter("@DBTMTraineeDetailIds", dBTMTraineeDetailIds, ParameterDirection.Input, DbType.String);
        //    objStoredProc.SetParameter("@FromDate", FromDate, ParameterDirection.Input, DbType.Date);
        //    objStoredProc.SetParameter("@ToDate", ToDate, ParameterDirection.Input, DbType.Date);
        //    List<DBTMTraineeProfilePerformanceModel> traineeProfilePerformanceListData = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMTestAndPerformanceMatrixByTraineeDetailIds @DBTMTraineeDetailIds,@FromDate, @ToDate")?.ToList();
        //    if (traineeProfilePerformanceListData != null && traineeProfilePerformanceListData.Count > 0)
        //    {
        //        traineeProfilePerformanceListData.ForEach(x =>
        //        {
        //            x.ParameterValue = x.IsEncrypted ? EncryptionHelper.Decrypt(x.ParameterValue) : x.ParameterValue;
        //        });
        //        foreach (string dBTMTraineeDetailId in dBTMTraineeDetailIds.Split(","))
        //        {
        //            foreach (var item in traineeProfilePerformanceListData.Where(x => x.DBTMTraineeDetailId.ToString() == dBTMTraineeDetailId).GroupBy(x => x.TestCode))
        //            {
        //                List<DBTMTraineeProfilePerformanceModel> list = traineeProfilePerformanceListData.Where(x => x.TestCode == item.Key && x.RowNumber == 1 && x.DBTMTraineeDetailId == item.FirstOrDefault().DBTMTraineeDetailId).ToList();
        //                DBTMTraineeProfilePerformanceModel performanceModel = new DBTMTraineeProfilePerformanceModel();
        //                performanceModel.TestCode = item.Key;
        //                performanceModel.TestName = list.FirstOrDefault().TestName;
        //                performanceModel.PerformanceMatrix = list.FirstOrDefault().PerformanceMatrix;
        //                performanceModel.DBTMTraineeDetailId = Convert.ToInt64(dBTMTraineeDetailId);
        //                decimal lastRecordSum, previousResordSum;
        //                if (list.Any(x => x.ParameterCode == CustomConstants.Count && (x.TestCode != CustomConstants.PlateTapTest)))
        //                {
        //                    lastRecordSum = list.Where(y => y.ParameterCode == CustomConstants.Count).Sum(x => Convert.ToDecimal(x.ParameterValue));
        //                    performanceModel.Score = $"{Convert.ToUInt32(lastRecordSum)} {DBTMCustomHelper.Unit(CustomConstants.Count)} (Total Count)";
        //                    previousResordSum = traineeProfilePerformanceListData.Where(x => x.TestCode == item.Key && x.ParameterCode == CustomConstants.Count && x.RowNumber == 2).Sum(x => Convert.ToDecimal(x.ParameterValue));
        //                    UpdateArrowStatus(performanceModel, lastRecordSum, previousResordSum);
        //                }
        //                else if (list.Any(x => x.ParameterCode == CustomConstants.Time))
        //                {
        //                    lastRecordSum = list.Where(y => y.ParameterCode == CustomConstants.Time).Sum(x => Convert.ToDecimal(x.ParameterValue));
        //                    performanceModel.Score = $"{lastRecordSum} {DBTMCustomHelper.Unit(CustomConstants.Time)} (Total Time)";
        //                    previousResordSum = traineeProfilePerformanceListData.Where(x => x.TestCode == item.Key && x.ParameterCode == CustomConstants.Time && x.RowNumber == 2).Sum(x => Convert.ToDecimal(x.ParameterValue));
        //                    UpdateArrowStatus(performanceModel, lastRecordSum, previousResordSum);
        //                }
        //                if (list.Any(x => x.ParameterCode == CustomConstants.JumpHeight))
        //                {
        //                    lastRecordSum = list.Where(y => y.ParameterCode == CustomConstants.JumpHeight).Sum(x => Convert.ToDecimal(x.ParameterValue));
        //                    performanceModel.Score = $"{lastRecordSum} {DBTMCustomHelper.Unit(CustomConstants.JumpHeight)} (Jump Height)";
        //                    previousResordSum = traineeProfilePerformanceListData.Where(x => x.TestCode == item.Key && x.ParameterCode == CustomConstants.JumpHeight && x.RowNumber == 2).Sum(x => Convert.ToDecimal(x.ParameterValue));
        //                    UpdateArrowStatus(performanceModel, lastRecordSum, previousResordSum);
        //                }
        //                if (list.Any(x => x.ParameterCode == CustomConstants.JumpLength))
        //                {
        //                    lastRecordSum = list.Where(y => y.ParameterCode == CustomConstants.JumpLength).Sum(x => Convert.ToDecimal(x.ParameterValue));
        //                    performanceModel.Score = $"{lastRecordSum} {DBTMCustomHelper.Unit(CustomConstants.JumpLength)} (Jump Length)";
        //                    previousResordSum = traineeProfilePerformanceListData.Where(x => x.TestCode == item.Key && x.ParameterCode == CustomConstants.JumpLength && x.RowNumber == 2).Sum(x => Convert.ToDecimal(x.ParameterValue));
        //                    UpdateArrowStatus(performanceModel, lastRecordSum, previousResordSum);
        //                }
        //                listModel.Add(performanceModel);
        //            }
        //        }
        //    }
        //    return listModel;
        //}

        private static void UpdateArrowStatus(DBTMTraineeProfilePerformanceModel performanceModel, decimal lastRecordSum, decimal previousResordSum)
        {
            if (previousResordSum == 0 || lastRecordSum == previousResordSum)
            {
                performanceModel.IsUp = null;
            }
            else if (lastRecordSum > previousResordSum)
            {
                performanceModel.IsUp = false;
            }
            else
            {
                performanceModel.IsUp = true;
            }
        }

        private RadarChartModel BindRadarChartDetails(DataTable dt, DataRow dataRow, string[] testName)
        {
            return new RadarChartModel()
            {
                RadarChartId = dataRow["DBTMTraineeDetailId"].ToString(),
                Title = "Score",
                Labels = string.Join(",", testName),
                Datasets = new List<RadarGraphsDatasetModel>()
                            {
                                new RadarGraphsDatasetModel()
                                {
                                    Label = dataRow["Name"].ToString(),
                                    Data = string.Join(",", dt.Columns.Cast<DataColumn>().Where(c => c.ColumnName != "FinalScore" &&  c.ColumnName.Contains("Score")).Select(c => dataRow[c].ToString())),
                                    Color = "rgba(255, 99, 132, 0.2)"
                                }
                            }
            };
        }
        #endregion
    }
}
