using ClosedXML.Excel;
using Coditech.API.Data;
using Coditech.Common.API;
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
    public class DBTMOrganisationCentrewiseJoiningCodeService : OrganisationCentrewiseJoiningCodeService, IDBTMOrganisationCentrewiseJoiningCodeService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<OrganisationCentrewiseJoiningCode> _organisationCentrewiseJoiningCodeRepository;
        private readonly ICoditechRepository<GeneralEnumaratorMaster> _generalEnumaratorMasterRepository;
        private readonly ICoditechRepository<DBTMCentreWiseSetting> _dBTMCentreWiseSettingRepository;
        public DBTMOrganisationCentrewiseJoiningCodeService(ICoditechLogging coditechLogging, ICoditechEmail coditechEmail, ICoditechSMS coditechSMS, ICoditechWhatsApp coditechWhatsApp, IServiceProvider serviceProvider) : base(coditechLogging, coditechEmail, coditechSMS, coditechWhatsApp, serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _organisationCentrewiseJoiningCodeRepository = new CoditechRepository<OrganisationCentrewiseJoiningCode>(_serviceProvider.GetService<Coditech_Entities>());
            _generalEnumaratorMasterRepository = new CoditechRepository<GeneralEnumaratorMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _dBTMCentreWiseSettingRepository = new CoditechRepository<DBTMCentreWiseSetting>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        public override OrganisationCentrewiseJoiningCodeListModel GetOrganisationCentrewiseJoiningCodeList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            string selectedCentreCode = filters?.Find(x => string.Equals(x.FilterName, FilterKeys.SelectedCentreCode, StringComparison.CurrentCultureIgnoreCase))?.FilterValue;
            filters.RemoveAll(x => x.FilterName == FilterKeys.SelectedCentreCode);

            int JoiningCodeTypeEnumId = Convert.ToInt32(filters?.Find(x => string.Equals(x.FilterName, FilterKeys.JoiningCodeTypeEnumId, StringComparison.CurrentCultureIgnoreCase))?.FilterValue);
            filters.RemoveAll(x => x.FilterName == FilterKeys.JoiningCodeTypeEnumId);
            string trainerId = filters?.Find(x => string.Equals(x.FilterName, "Custom1", StringComparison.CurrentCultureIgnoreCase))?.FilterValue;
            filters.RemoveAll(x => string.Equals(x.FilterName, "Custom1", StringComparison.CurrentCultureIgnoreCase));
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<OrganisationCentrewiseJoiningCodeModel> objStoredProc = new CoditechViewRepository<OrganisationCentrewiseJoiningCodeModel>(_serviceProvider.GetService<Coditech_Entities>());
            objStoredProc.SetParameter("@CentreCode", selectedCentreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@JoiningCodeTypeEnumId", JoiningCodeTypeEnumId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@TrainerId", trainerId, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<OrganisationCentrewiseJoiningCodeModel> OrganisationCentrewiseJoiningCodeList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMOrganisationCentrewiseJoiningCodeList @CentreCode,@JoiningCodeTypeEnumId,@TrainerId,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 7, out pageListModel.TotalRowCount)?.ToList();
            OrganisationCentrewiseJoiningCodeListModel listModel = new OrganisationCentrewiseJoiningCodeListModel();

            listModel.OrganisationCentrewiseJoiningCodeList = OrganisationCentrewiseJoiningCodeList?.Count > 0 ? OrganisationCentrewiseJoiningCodeList : new List<OrganisationCentrewiseJoiningCodeModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }
        public override OrganisationCentrewiseJoiningCodeModel CreateOrganisationCentrewiseJoiningCode(OrganisationCentrewiseJoiningCodeModel organisationCentrewiseJoiningCodeModel)
        {
            if (IsNull(organisationCentrewiseJoiningCodeModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);
            DBTMCentreWiseSetting centreSetting = _dBTMCentreWiseSettingRepository.Table.FirstOrDefault(x => x.CentreCode == organisationCentrewiseJoiningCodeModel.CentreCode);
            if (IsNull(centreSetting) || centreSetting.AllowBatchUser <= 0)
                throw new CoditechException(ErrorCodes.InvalidData, "Centre settings Or Batch user limit not configured for this centre.");

            int? allowJoiningCodeCount = centreSetting.AllowBatchUser + centreSetting.AllowCampUser;
            string traineeEnumCode = _generalEnumaratorMasterRepository.Table.Where(x => x.GeneralEnumaratorId == organisationCentrewiseJoiningCodeModel.JoiningCodeTypeEnumId).Select(x => x.EnumName).FirstOrDefault();
            if (!allowJoiningCodeCount.HasValue)
                throw new CoditechException(ErrorCodes.NotFound, "Joining code limit not configured for this centre.");
            if (traineeEnumCode == CustomConstants.Trainee)
            {
                int existingJoiningCodeCount = _organisationCentrewiseJoiningCodeRepository.Table.Count(x => x.CentreCode == organisationCentrewiseJoiningCodeModel.CentreCode && x.JoiningCodeTypeEnumId == organisationCentrewiseJoiningCodeModel.JoiningCodeTypeEnumId);
                if ((existingJoiningCodeCount + organisationCentrewiseJoiningCodeModel.Quantity) > allowJoiningCodeCount.Value)
                {
                    throw new CoditechException(ErrorCodes.InvalidData, $"Joining code limit exceeded. Allowed: {allowJoiningCodeCount.Value}, Existing: {existingJoiningCodeCount}. Kindly contact Powered Sports Tech or raise a support ticket for assistance.");
                }
            }
            List<OrganisationCentrewiseJoiningCode> insertList = new List<OrganisationCentrewiseJoiningCode>();
            for (int i = 1; i <= organisationCentrewiseJoiningCodeModel.Quantity; i++)
            {
                insertList.Add(new OrganisationCentrewiseJoiningCode
                {
                    JoiningCode = GenerateAlphaNumericCode(ApiSettings.JoiningCodeLength),
                    Quantity = 1,
                    CentreCode = organisationCentrewiseJoiningCodeModel.CentreCode,
                    JoiningCodeTypeEnumId = organisationCentrewiseJoiningCodeModel.JoiningCodeTypeEnumId,
                    Custom1 = organisationCentrewiseJoiningCodeModel.Custom1,
                    Custom3 = organisationCentrewiseJoiningCodeModel.Custom3
                });
            }
            _organisationCentrewiseJoiningCodeRepository.Insert(insertList, organisationCentrewiseJoiningCodeModel.CreatedBy);
            return organisationCentrewiseJoiningCodeModel;
        }

        //GetTraineeActiveJoiningCodeList
        public List<OrganisationCentrewiseJoiningCodeModel> GetTraineeActiveJoiningCodeList(string centreCode, string trainerId, int rows)
        {
            FilterCollection filters = new FilterCollection();
            NameValueCollection sorts = new NameValueCollection { { "a.JoiningCode", "ASC" } };
            PageListModel pageListModel = new PageListModel(filters, sorts, 1, rows);
            CoditechViewRepository<OrganisationCentrewiseJoiningCodeModel> repo = new CoditechViewRepository<OrganisationCentrewiseJoiningCodeModel>(_serviceProvider.GetService<Coditech_Entities>());
            repo.SetParameter("@CentreCode", centreCode, ParameterDirection.Input, DbType.String);
            repo.SetParameter("@JoiningCodeTypeEnumId", 324, ParameterDirection.Input, DbType.Int32);
            repo.SetParameter("@TrainerId", trainerId, ParameterDirection.Input, DbType.String);
            repo.SetParameter("@WhereClause", "IsExpired = 0", ParameterDirection.Input, DbType.String);
            repo.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            repo.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            repo.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            repo.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<OrganisationCentrewiseJoiningCodeModel> aa = repo.ExecuteStoredProcedureList("Coditech_GetDBTMOrganisationCentrewiseJoiningCodeList @CentreCode,@JoiningCodeTypeEnumId,@TrainerId,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 7, out int totalRows)?.ToList();
            if (aa.Count > 0 && rows == 1)
            {
                OrganisationCentrewiseJoiningCode pp = _organisationCentrewiseJoiningCodeRepository.Table.Where(x => x.OrganisationCentrewiseJoiningCodeId == aa.FirstOrDefault().OrganisationCentrewiseJoiningCodeId);
                pp.IsInQueue = true;
                pp.QueueValidTill = DateTime.Now;
                _organisationCentrewiseJoiningCodeRepository.Update(pp);
            }
            return aa;
        }

        public DBTMOrganisationCentrewiseJoiningCodeModel GetTraineeActiveJoiningCode(string centreCode, string trainerId, int rows)
        {
            List<OrganisationCentrewiseJoiningCodeModel> list = GetTraineeActiveJoiningCodeList(centreCode, trainerId, rows).OrderBy(x => x.Custom2).ToList();
            if (list == null || !list.Any())
                return new DBTMOrganisationCentrewiseJoiningCodeModel();
            string currentDir = Directory.GetCurrentDirectory();
            string dataFolder = Path.Combine(currentDir, "data", "JoiningCodeForTrainee");
            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);
            string fileName = $"Trainee_JoiningCode_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            string filePath = Path.Combine(dataFolder, fileName);
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("TraineeJoiningCode");
                worksheet.Cell(1, 1).Value = "Joining Code";
                worksheet.Cell(1, 2).Value = "Trainer";
                int row = 2;
                foreach (var item in list)
                {
                    worksheet.Cell(row, 1).Value = item.JoiningCode;
                    worksheet.Cell(row, 2).Value = item.Custom2;
                    row++;
                }
                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(filePath);
            }

            DBTMOrganisationCentrewiseJoiningCodeModel model = new DBTMOrganisationCentrewiseJoiningCodeModel
            {
                FilePath = filePath,
                FileName = fileName
            };
            return model;
        }

        public DBTMOrganisationCentrewiseJoiningCodeModel GetTrainerActiveJoiningCode(string centreCode)
        {
            CoditechViewRepository<OrganisationCentrewiseJoiningCodeModel> repo = new CoditechViewRepository<OrganisationCentrewiseJoiningCodeModel>(_serviceProvider.GetService<Coditech_Entities>());
            repo.SetParameter("@CentreCode", centreCode, ParameterDirection.Input, DbType.String);
            repo.SetParameter("@JoiningCodeTypeEnumId", 323, ParameterDirection.Input, DbType.Int32);
            repo.SetParameter("@TrainerId", "", ParameterDirection.Input, DbType.String);
            repo.SetParameter("@WhereClause", "IsExpired = 0", ParameterDirection.Input, DbType.String);
            repo.SetParameter("@PageNo", 1, ParameterDirection.Input, DbType.Int32);
            repo.SetParameter("@Rows", 1, ParameterDirection.Input, DbType.Int32);
            repo.SetParameter("@Order_BY", "a.CreatedDate DESC", ParameterDirection.Input, DbType.String);
            repo.SetParameter("@RowsCount", 0, ParameterDirection.Output, DbType.Int32);
            List<OrganisationCentrewiseJoiningCodeModel> list = repo.ExecuteStoredProcedureList("Coditech_GetDBTMOrganisationCentrewiseJoiningCodeList @CentreCode,@JoiningCodeTypeEnumId,@TrainerId,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 7, out int totalRows)?.ToList();
            OrganisationCentrewiseJoiningCodeModel item = list?.FirstOrDefault();
            if (item == null)
                return new DBTMOrganisationCentrewiseJoiningCodeModel();
            DBTMOrganisationCentrewiseJoiningCodeModel listModel = new DBTMOrganisationCentrewiseJoiningCodeModel()
            {
                JoiningCode = item.JoiningCode,
                Custom1 = item.Custom1,
                Custom2 = item.Custom2,
                Custom3 = item.Custom3
            };
            return listModel;
        }

        // Delete Report File from Data folder
        public bool DeleteOrganisationCentrewiseJoiningCodeFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;
            try
            {
                string currentDir = Directory.GetCurrentDirectory();
                string activityPath = Path.Combine(currentDir, "data", "JoiningCodeForTrainee", fileName);
                if (File.Exists(activityPath))
                {
                    File.Delete(activityPath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
