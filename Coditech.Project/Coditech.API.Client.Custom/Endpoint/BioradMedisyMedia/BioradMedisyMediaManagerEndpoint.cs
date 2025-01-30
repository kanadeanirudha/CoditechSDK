using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;

namespace Coditech.API.Endpoint
{
    public class BioradMedisyMediaManagerEndpoint : BaseEndpoint
    {
        
        public string GetMediaDetailsAsync(long mediaId, long entityId) =>
            $"{CoditechAdminSettings.CoditechMediaManagerApiRootUri}/BioradMedisyMediaManager/GetMediaDetails?mediaId={mediaId}&entityId={entityId}";
       
        public string UpdateFileApprovalFlowAsync() =>
               $"{CoditechAdminSettings.CoditechMediaManagerApiRootUri}/BioradMedisyMediaManager/UpdateFileApprovalFlow";
    }
}
