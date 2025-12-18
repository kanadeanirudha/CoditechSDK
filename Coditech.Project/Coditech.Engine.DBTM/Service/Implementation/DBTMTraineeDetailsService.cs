using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Engine.DBTM.Helpers;
using Coditech.Resources;
using System;
using System.Collections.Specialized;
using System.Data;
using DinkToPdf;
using DinkToPdf.Contracts;
using static Coditech.Common.Helper.HelperUtility;
using Twilio.Converters;
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
        private readonly ICoditechRepository<DBTMParametersAssociatedToTest> _dBTMParametersAssociatedToTestRepository;
        private readonly ICoditechRepository<DBTMTestParameter> _dBTMTestParameterRepository;
        private readonly ICoditechRepository<DBTMCalculationAssociatedToTest> _dBTMCalculationAssociatedToTestRepository;
        private readonly ICoditechRepository<DBTMTestCalculation> _dBTMTestCalculationRepository;
        private readonly ICoditechRepository<GeneralEnumaratorMaster> _generalEnumaratorMasterRepository;
        private readonly IConverter _converter;

        public DBTMTraineeDetailsService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider, IDBTMReportsService dBTMReportsService, IConverter converter) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMReportsService = dBTMReportsService;
            _dBTMTraineeDetailsRepository = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMDeviceDataRepository = new CoditechRepository<DBTMDeviceData>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestMasterRepository = new CoditechRepository<DBTMTestMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMParametersAssociatedToTestRepository = new CoditechRepository<DBTMParametersAssociatedToTest>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestParameterRepository = new CoditechRepository<DBTMTestParameter>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMCalculationAssociatedToTestRepository = new CoditechRepository<DBTMCalculationAssociatedToTest>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestCalculationRepository = new CoditechRepository<DBTMTestCalculation>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _generalEnumaratorMasterRepository = new CoditechRepository<GeneralEnumaratorMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _converter = converter;
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
            if (IsNotNull(dBTMTraineeDetailsModel))
            {
                GeneralPersonModel generalPersonModel = GetGeneralPersonDetails(dBTMTraineeDetailsModel.PersonId);
                if (IsNotNull(dBTMTraineeDetailsModel))
                {
                    dBTMTraineeDetailsModel.FirstName = generalPersonModel.FirstName;
                    dBTMTraineeDetailsModel.LastName = generalPersonModel.LastName;
                    dBTMTraineeDetailsModel.IsActive = dBTMTraineeDetails.IsActive;
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

        //Get ProfileDetails
        public DBTMTraineeProfileModel GetProfileDetails(long dBTMTraineeDetailId)
        {
            if (dBTMTraineeDetailId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTraineeDetailId"));

            var dBTMTraineeDetailsData = _dBTMTraineeDetailsRepository.Table.Where(x => x.DBTMTraineeDetailId == dBTMTraineeDetailId).Select(x => new { x.PersonId, x.SpecializationEnumId, x.CreatedDate, x.Weight, x.CentreCode }).FirstOrDefault();
            if (dBTMTraineeDetailsData == null)
                return null;

            DBTMTraineeProfileModel dBTMTraineeProfileModel = new DBTMTraineeProfileModel
            {
                DBTMTraineeDetailId = dBTMTraineeDetailId
            };
            if (dBTMTraineeProfileModel == null)
                return null;

            // Inline variable assignment for clarity and slight performance
            GeneralPersonModel generalPersonModel = GetGeneralPersonDetails(Convert.ToInt64(dBTMTraineeDetailsData.PersonId));

            if (generalPersonModel != null)
            {
                dBTMTraineeProfileModel.FirstName = generalPersonModel.FirstName;
                dBTMTraineeProfileModel.LastName = generalPersonModel.LastName;
                dBTMTraineeProfileModel.DateOfBirth = generalPersonModel.DateOfBirth;
                dBTMTraineeProfileModel.PhotoMediaPath = GetImagePath(generalPersonModel.PhotoMediaId);
                dBTMTraineeProfileModel.Weight = dBTMTraineeDetailsData.Weight;
                dBTMTraineeProfileModel.CentreCode = dBTMTraineeDetailsData.CentreCode;
            }

            dBTMTraineeProfileModel.Specialization = GetEnumDisplayTextByEnumId(Convert.ToInt32(dBTMTraineeDetailsData.SpecializationEnumId));
            dBTMTraineeProfileModel.DateOfJoining = dBTMTraineeDetailsData.CreatedDate;

            // Use ternary for brevity
            dBTMTraineeProfileModel.TotalDuration = dBTMTraineeProfileModel.DateOfJoining.HasValue
                ? CalculateDuration(dBTMTraineeProfileModel.DateOfJoining.Value, DateTime.Now)
                : "N/A";

            CoditechViewRepository<DBTMTraineeProfilePerformanceModel> objStoredProc = new CoditechViewRepository<DBTMTraineeProfilePerformanceModel>(_serviceProvider.GetService<Coditech_Entities>());
            objStoredProc.SetParameter("@DBTMTraineeDetailId", dBTMTraineeDetailId, ParameterDirection.Input, DbType.Int64);
            List<DBTMTraineeProfilePerformanceModel> traineeProfilePerformanceList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMTestAndPerformanceMatrix @DBTMTraineeDetailId")?.ToList();
            if (traineeProfilePerformanceList != null && traineeProfilePerformanceList.Count > 0)
            {
                dBTMTraineeProfileModel.TraineeProfilePerformanceList = null;
                foreach (var item in traineeProfilePerformanceList.GroupBy(x => x.TestCode))
                {
                    if (dBTMTraineeProfileModel.TraineeProfilePerformanceList == null)
                        dBTMTraineeProfileModel.TraineeProfilePerformanceList = new List<DBTMTraineeProfilePerformanceModel>();
                    List<DBTMTraineeProfilePerformanceModel> list = traineeProfilePerformanceList.Where(x => x.TestCode == item.Key && x.RowNumber == 1).ToList();
                    DBTMTraineeProfilePerformanceModel performanceModel = new DBTMTraineeProfilePerformanceModel();
                    performanceModel.TestCode = item.Key;
                    performanceModel.TestName = list.FirstOrDefault().TestName;
                    performanceModel.PerformanceMatrix = list.FirstOrDefault().PerformanceMatrix;
                    decimal lastRecordSum, previousResordSum;
                    if (list.Any(x => x.ParameterCode == CustomConstants.Time))
                    {
                        lastRecordSum = list.Where(y => y.ParameterCode == CustomConstants.Time).Sum(x => x.ParameterValue);
                        performanceModel.Score = $"{lastRecordSum} {DBTMCustomHelper.Unit(CustomConstants.Time)} (Total Time)";
                        previousResordSum = traineeProfilePerformanceList.Where(x => x.TestCode == item.Key && x.ParameterCode == CustomConstants.Time && x.RowNumber == 2).Sum(x => x.ParameterValue);
                        if (lastRecordSum < previousResordSum)
                        {
                            performanceModel.IsUp = false;
                        }
                    }
                    if (list.Any(x => x.ParameterCode == CustomConstants.JumpHeight))
                    {
                        lastRecordSum = list.Where(y => y.ParameterCode == CustomConstants.JumpHeight).Sum(x => x.ParameterValue);
                        performanceModel.Score = $"{lastRecordSum} {DBTMCustomHelper.Unit(CustomConstants.JumpHeight)} (Jump Height)";
                        previousResordSum = traineeProfilePerformanceList.Where(x => x.TestCode == item.Key && x.ParameterCode == CustomConstants.JumpHeight && x.RowNumber == 2).Sum(x => x.ParameterValue);
                        if (lastRecordSum < previousResordSum)
                        {
                            performanceModel.IsUp = false;
                        }
                    }
                    dBTMTraineeProfileModel.TraineeProfilePerformanceList.Add(performanceModel);
                }
            }
            return dBTMTraineeProfileModel;
        }

        //Download Trainee Report Pdf
        public DBTMReportsListModel GenerateAthletePdfRemark(long dBTMTraineeDetailId, string remarks)
        {
            // GetTraineeProfileHtml
            string html = GetTraineeProfileHtml(dBTMTraineeDetailId, remarks);

            // Generate PDF
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "AthleteReportPdf");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName = $"Athlete_Profile_{dBTMTraineeDetailId}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
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
            string centreName = base.GetOrganisationCentreNameByCentreCode(profile.CentreCode);
            if (profile == null)
                throw new CoditechException(ErrorCodes.NullModel, "Trainee profile not found");

            string templateCode = EmailTemplateCodeCustomEnum.TraineeReportTemplate.ToString();

            var emailTemplate = GetEmailTemplateByCode(profile.CentreCode, templateCode);

            if (string.IsNullOrWhiteSpace(emailTemplate?.EmailTemplate))
                throw new CoditechException(ErrorCodes.NullModel, $"Template '{templateCode}' not found for centre '{centreName}'");

            string html = emailTemplate.EmailTemplate;

            html = ReplaceTraineeTemplate(html, profile, remarks, centreName);
            return html;
        }

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

        //Template Html Replacement
        private string ReplaceTraineeTemplate(string html, DBTMTraineeProfileModel profile, string remarks, string centreName)
        {
            html = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.FirstName, profile.FirstName, html);
            html = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.LastName, profile.LastName, html);
            html = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.DOB, profile.DateOfBirth?.ToString("dd-MMM-yyyy"), html);
            html = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.JoiningDate, profile.DateOfJoining?.ToString("dd-MMM-yyyy"), html);
            html = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.Weight,profile.Weight.ToString(), html);           
            html = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.WeeklyHours, profile.WeekelyHours?.ToString("hh\\:mm"), html);
            html = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.TotalDuration, profile.TotalDuration, html);
            html = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.Remarks, remarks, html);
            html = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.CentreName, centreName, html);
            return html;
        }
    }
}
