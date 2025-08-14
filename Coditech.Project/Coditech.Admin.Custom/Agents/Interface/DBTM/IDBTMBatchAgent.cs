using Coditech.Admin.ViewModel;
using Coditech.Common.Helper.Utilities;

namespace Coditech.Admin.Agents
{
    public interface IDBTMBatchAgent
    {
        /// <summary>
        /// Get list of Associated Batch.
        /// </summary>
        /// <param name="selectedCentreCode">selectedCentreCode.</param>
        /// <param name="generalTrainerMasterId">GeneralTrainerMasterId.</param>
        /// <param name="generalBatchMasterId">GeneralBatchMasterId.</param>
        /// <returns>GeneralBatchUserListViewModel</returns>
        GeneralBatchUserListViewModel GetBatchUserListByCentreCodeAndGeneralTrainerMasterId(string selectedCentreCode, long generalTrainerMasterId, int generalBatchMasterId);
    }
}
