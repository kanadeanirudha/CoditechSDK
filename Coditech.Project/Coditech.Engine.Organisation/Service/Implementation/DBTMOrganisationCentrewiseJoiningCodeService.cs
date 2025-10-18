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
    public class DBTMOrganisationCentrewiseJoiningCodeService : OrganisationCentrewiseJoiningCodeService
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

            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<OrganisationCentrewiseJoiningCodeModel> objStoredProc = new CoditechViewRepository<OrganisationCentrewiseJoiningCodeModel>(_serviceProvider.GetService<Coditech_Entities>());
            objStoredProc.SetParameter("@CentreCode", selectedCentreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@JoiningCodeTypeEnumId", JoiningCodeTypeEnumId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<OrganisationCentrewiseJoiningCodeModel> OrganisationCentrewiseJoiningCodeList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMOrganisationCentrewiseJoiningCodeList @CentreCode,@JoiningCodeTypeEnumId,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 6, out pageListModel.TotalRowCount)?.ToList();
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

            _organisationCentrewiseJoiningCodeRepository.Insert(insertList);
            return organisationCentrewiseJoiningCodeModel;
        }
    }
}
