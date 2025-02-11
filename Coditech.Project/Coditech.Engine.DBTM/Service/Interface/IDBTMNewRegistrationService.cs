using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface IDBTMNewRegistrationService
    {
        DBTMNewRegistrationModel DBTMCentreRegistration(DBTMNewRegistrationModel model);
        DBTMNewRegistrationModel TrainerRegistration(DBTMNewRegistrationModel model);
        DBTMNewRegistrationModel IndividualRegistration(DBTMNewRegistrationModel model);
    }
}
