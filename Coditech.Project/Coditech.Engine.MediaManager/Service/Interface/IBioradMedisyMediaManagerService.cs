using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface IBioradMedisyMediaManagerService
    {
        BioradMedisyMediaModel GetMediaDetails(long mediaId, long entityId);
        bool UpdateFileApprovalFlow(BioradMedisyMediaModel model);
    }
}
