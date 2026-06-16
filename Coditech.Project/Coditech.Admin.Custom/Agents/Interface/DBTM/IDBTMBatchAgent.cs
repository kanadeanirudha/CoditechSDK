using Coditech.Admin.ViewModel;

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
        GeneralBatchListViewModel GetCalendarBatches(string centreCode, long userMasterId, DateTime startDate, DateTime endDate);
        bool TransferBatch(int generalBatchMasterId, long trainerId, out string message);
    }
}
