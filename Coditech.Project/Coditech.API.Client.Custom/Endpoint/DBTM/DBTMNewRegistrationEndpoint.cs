using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;

namespace Coditech.API.Endpoint
{
    public class DBTMNewRegistrationEndpoint : BaseEndpoint
    {
        public string DBTMCentreRegistrationAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMCentreRegistration/DBTMCentreRegistration";

        public string TrainerRegistrationAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/TrainerRegistration/TrainerRegistration";

        public string ValidateTrainerJoiningCode(string joiningCode) =>
           $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/TrainerRegistration/ValidateTrainerJoiningCode?joiningCode={joiningCode}";

        public string GetGeneralTrainerByJoiningCode(string joiningCode, long generalTrainerMasterId)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMUser/GetGeneralTrainerByJoiningCode?joiningCode={joiningCode}&generalTrainerMasterId={generalTrainerMasterId}";
            return endpoint;
        }
        public string GetTrainerListByJoiningCode(string joiningCode)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMUser/GetTrainerListByJoiningCode?joiningCode={joiningCode}";
            return endpoint;
        }
        public string ValidateTraineeJoiningCode(string joiningCode) =>
        $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMUser/ValidateTraineeJoiningCode?joiningCode={joiningCode}";
        public string ConvertCampUserToBatchUserAsync()
        {
            return $"{CoditechCustomAdminSettings.CoditechOrganisationApiRootUri}/DBTMGeneralBatchMaster/ConvertCampUserToBatchUser";
        }
        public string GetJoiningCode(string generalTrainerMasterId)
        {
            return $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMApi/GetJoiningCode?generalTrainerMasterId={generalTrainerMasterId}";
        }
        public string ValidateDeviceSerialCode(string deviceSerialCode) =>
        $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMCentreRegistration/ValidateDeviceSerialCode?deviceSerialCode={deviceSerialCode}";
    }
}
