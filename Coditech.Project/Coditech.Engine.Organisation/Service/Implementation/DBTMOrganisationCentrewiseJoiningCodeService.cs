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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMOrganisationCentrewiseJoiningCodeService : OrganisationCentrewiseJoiningCodeService, IDBTMOrganisationCentrewiseJoiningCodeService
    {
        private readonly ICoditechRepository<OrganisationCentrewiseJoiningCode> _organisationCentrewiseJoiningCodeRepository;
        public DBTMOrganisationCentrewiseJoiningCodeService(ICoditechLogging coditechLogging, ICoditechEmail coditechEmail, ICoditechSMS coditechSMS, ICoditechWhatsApp coditechWhatsApp, IServiceProvider serviceProvider) : base(coditechLogging, coditechEmail, coditechSMS, coditechWhatsApp, serviceProvider)
        {
            _organisationCentrewiseJoiningCodeRepository = new CoditechRepository<OrganisationCentrewiseJoiningCode>(_serviceProvider.GetService<Coditech_Entities>());
        }
        public override OrganisationCentrewiseJoiningCodeListModel GetOrganisationCentrewiseJoiningCodeList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            string selectedCentreCode = filters?.Find(x => string.Equals(x.FilterName, FilterKeys.SelectedCentreCode, StringComparison.CurrentCultureIgnoreCase))?.FilterValue;
            filters.RemoveAll(x => x.FilterName == FilterKeys.SelectedCentreCode);

            int JoiningCodeTypeEnumId = Convert.ToInt32(filters?.Find(x => string.Equals(x.FilterName, FilterKeys.JoiningCodeTypeEnumId, StringComparison.CurrentCultureIgnoreCase))?.FilterValue);
            filters.RemoveAll(x => x.FilterName == FilterKeys.JoiningCodeTypeEnumId);
            string trainerId = filters?.Find(x => string.Equals(x.FilterName, "Custom1", StringComparison.CurrentCultureIgnoreCase))?.FilterValue;
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

            List<OrganisationCentrewiseJoiningCode> insertList = new List<OrganisationCentrewiseJoiningCode>();
            for (int i = 1; i <= organisationCentrewiseJoiningCodeModel.Quantity; i++)
            {

                insertList.Add(new OrganisationCentrewiseJoiningCode
                {
                    JoiningCode = GenerateAlphaNumericCode(ApiSettings.JoiningCodeLength),
                    Quantity = 1,
                    CentreCode = organisationCentrewiseJoiningCodeModel.CentreCode,
                    JoiningCodeTypeEnumId = organisationCentrewiseJoiningCodeModel.JoiningCodeTypeEnumId,
                    Custom1 = organisationCentrewiseJoiningCodeModel.Custom1
                });
            }

            _organisationCentrewiseJoiningCodeRepository.Insert(insertList, organisationCentrewiseJoiningCodeModel.CreatedBy);
            return organisationCentrewiseJoiningCodeModel;
        }

        //GetTraineeActiveJoiningCodeList
        private List<OrganisationCentrewiseJoiningCodeModel> GetTraineeActiveJoiningCodeList(string centreCode)
        {
            CoditechViewRepository<OrganisationCentrewiseJoiningCodeModel> repo = new CoditechViewRepository<OrganisationCentrewiseJoiningCodeModel>(_serviceProvider.GetService<Coditech_Entities>());
            repo.SetParameter("@CentreCode", centreCode, ParameterDirection.Input, DbType.String);
            repo.SetParameter("@JoiningCodeTypeEnumId", 324, ParameterDirection.Input, DbType.Int32);
            repo.SetParameter("@TrainerId", "", ParameterDirection.Input, DbType.String);
            repo.SetParameter("@WhereClause", "IsExpired = 0", ParameterDirection.Input, DbType.String);
            repo.SetParameter("@Rows", 100000, ParameterDirection.Input, DbType.Int32);
            repo.SetParameter("@PageNo", 1, ParameterDirection.Input, DbType.Int32);
            repo.SetParameter("@Order_BY", "a.JoiningCode ASC", ParameterDirection.Input, DbType.String);
            repo.SetParameter("@RowsCount", 0, ParameterDirection.Output, DbType.Int32);
            List <OrganisationCentrewiseJoiningCodeModel> OrganisationCentrewiseJoiningCodeList = repo.ExecuteStoredProcedureList("Coditech_GetDBTMOrganisationCentrewiseJoiningCodeList @CentreCode,@JoiningCodeTypeEnumId,@TrainerId,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 7, out int total)?.ToList();
            return OrganisationCentrewiseJoiningCodeList;
        }
        public DBTMOrganisationCentrewiseJoiningCodeModel GetTraineeActiveJoiningCode(string centreCode)
        {
            List<OrganisationCentrewiseJoiningCodeModel> list = GetTraineeActiveJoiningCodeList(centreCode);
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
                worksheet.Cell(1, 2).Value = "Joining Code Type";
                worksheet.Cell(1, 3).Value = "Trainer";
                worksheet.Cell(1, 4).Value = "Is Active";
                int row = 2;
                foreach (var item in list)
                {
                    worksheet.Cell(row, 1).Value = item.JoiningCode;
                    worksheet.Cell(row, 2).Value = item.JoiningCodeType;
                    worksheet.Cell(row, 3).Value = item.Custom2;
                    worksheet.Cell(row, 4).Value = item.IsExpired ? "No" : "Yes";
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
