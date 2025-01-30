using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;

namespace Coditech.API.Client
{
    public interface IBioradMedisyMediaManagerClient : IBaseClient
    {

        /// <summary>
        /// Get MediaSettingMaster by mediaTypeMasterId.
        /// </summary>
        /// <param name="mediaTypeMasterId">mediaTypeMasterId</param>
        /// <returns>Returns BioradMedisyMediaResponse.</returns>
        BioradMedisyMediaResponse GetMediaDetails(long mediaId, long entityId);

        /// <summary>
        /// Update File Approval Flow.
        /// </summary>
        /// <param name="MediaModel">MediaModel.</param>
        /// <returns>Returns updated BioradMedisyMediaResponse</returns>
        BioradMedisyMediaResponse UpdateFileApprovalFlow(BioradMedisyMediaModel body);
    }
}
