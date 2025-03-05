using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;

namespace Coditech.API.Endpoint
{
    public class DBTMUserEndpoint : BaseEndpoint
    {
        public string IndividualRegistrationAsync() =>
            $"{CoditechCustomAdminSettings.CoditechOrganisationApiRootUri}/DBTMUser/DBTMRegisterTrainee";
    }
}
       
        
      