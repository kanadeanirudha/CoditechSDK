using Coditech.Common.API.Model;

namespace Coditech.Admin.Agents
{
    public interface IBioradMedisyMediaManagerAgent
    {
        /// <summary>
        /// Get Media Details by mediaId.
        /// </summary>
        /// <param name="mediaId">mediaId</param>
        /// <returns>Returns MediaModel.</returns>
        BioradMedisyMediaModel GetMediaDetails(long mediaId);

        /// <summary>
        /// UpdateFileApprovalFlow.
        /// </summary>
        /// <param name="bioradMedisyMediaModel">bioradMedisyMediaModel.</param>
        /// <returns>Returns updated bioradMedisyMediaModel</returns>
        BioradMedisyMediaModel UpdateFileApprovalFlow(BioradMedisyMediaModel bioradMedisyMediaModel);
    }
}
