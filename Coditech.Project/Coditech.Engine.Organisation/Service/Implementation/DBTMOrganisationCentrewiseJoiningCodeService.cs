using Coditech.API.Data;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;
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
