using Coditech.Admin.ViewModel;

namespace Coditech.Admin.Agents
{
    public interface IDBTMTraineeDetailsAgent
    {
        /// <summary>
        /// Get list of DBTMTraineeDetails.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>DBTMTraineeDetailsListViewModel</returns>
        DBTMTraineeDetailsListViewModel GetDBTMTraineeDetailsList(DataTableViewModel dataTableModel, string listType = null);

        /// <summary>
        /// Create DBTMTraineeDetails.
        /// </summary>
        /// <param name="dBTMTraineeDetailsCreateEditViewModel">DBTM Trainee Details View Model.</param>
        /// <returns>Returns created model.</returns>
        DBTMTraineeDetailsCreateEditViewModel CreateDBTMTraineeDetails(DBTMTraineeDetailsCreateEditViewModel dBTMTraineeDetailsCreateEditViewModel);

        /// <summary>
        /// Get DBTMTrainee Details by personId.
        /// </summary>
        /// <param name="personId">personId</param>
        /// <returns>Returns DBTMTraineeDetailsCreateEditViewModel.</returns>
        DBTMTraineeDetailsCreateEditViewModel GetDBTMTraineePersonalDetails(long dBTMTraineeDetailId, long personId);

        /// <summary>
        /// Update DBTM Trainee Details.
        /// </summary>
        /// <param name="dBTMTraineeDetailsCreateEditViewModel">dBTMTraineeDetailsCreateEditViewModel.</param>
        /// <returns>Returns updated DBTMTraineeDetailsCreateEditViewModel</returns>
        DBTMTraineeDetailsCreateEditViewModel UpdateDBTMTraineePersonalDetails(DBTMTraineeDetailsCreateEditViewModel dBTMTraineeDetailsCreateEditViewModel);

        /// <summary>
        /// Get DBTMTraineeDetails by dBTMTraineeDetailId.
        /// </summary>
        /// <param name="dBTMTraineeDetailId">dBTMTraineeDetailId</param>
        /// <returns>Returns DBTMTraineeDetailsResponse.</returns>
        DBTMTraineeDetailsViewModel GetDBTMTraineeOtherDetails(long dBTMTraineeDetailId);

        /// <summary>
        /// Update DBTM Trainee Other Details
        /// </summary>
        /// <param name="DBTMTraineeDetailsModel">DBTMTraineeDetailsModel.</param>
        /// <returns>Returns updated DBTMTraineeDetailsViewModel</returns>
        DBTMTraineeDetailsViewModel UpdateDBTMTraineeOtherDetails(DBTMTraineeDetailsViewModel dBTMTraineeDetailsModel);


        /// <summary>
        /// Delete DBTM Trainee Details.
        /// </summary>
        /// <param name="dBTMTraineeDetailIds">dBTMTraineeDetailIds.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteDBTMTraineeDetails(string dBTMTraineeDetailIds, out string errorMessage);

        /// <summary>
        /// Get list of Associated Trainer.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>GeneralTraineeAssociatedToTrainerListViewModel</returns>
        GeneralTraineeAssociatedToTrainerListViewModel GetAssociatedTrainerList(long dBTMTraineeDetailId, long personId, DataTableViewModel dataTableModel);


        GeneralTraineeAssociatedToTrainerViewModel AssociatedTrainer(long dBTMTraineeDetailId, long personId);

        /// <summary>
        /// Insert AssociatedTrainer.
        /// </summary>
        /// <param name="generalTraineeAssociatedToTrainerViewModel">General Trainee Associated To Trainer View Model.</param>
        /// <returns>Returns created model.</returns>
        GeneralTraineeAssociatedToTrainerViewModel InsertAssociatedTrainer(GeneralTraineeAssociatedToTrainerViewModel generalTraineeAssociatedToTrainerViewModel);

        /// <summary>
        /// Get AssociatedTrainer by generalTraineeAssociatedToTrainerId.
        /// </summary>
        /// <param name="generalTraineeAssociatedToTrainerId">generalTraineeAssociatedToTrainerId</param>
        /// <returns>Returns GeneralTraineeAssociatedToTrainerViewModel.</returns>
        GeneralTraineeAssociatedToTrainerViewModel GetAssociatedTrainer(long generalTraineeAssociatedToTrainerId);

        /// <summary>
        /// Update AssociatedTrainer.
        /// </summary>
        /// <param name="generalTraineeAssociatedToTrainerViewModel">generalTraineeAssociatedToTrainerViewModel.</param>
        /// <returns>Returns updated GeneralTraineeAssociatedToTrainerViewModel</returns>
        GeneralTraineeAssociatedToTrainerViewModel UpdateAssociatedTrainer(GeneralTraineeAssociatedToTrainerViewModel generalTraineeAssociatedToTrainerViewModel);

        /// <summary>
        /// Delete Associated Trainer Details.
        /// </summary>
        /// <param name="generalTraineeAssociatedToTrainerIds">generalTraineeAssociatedToTrainerIds.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteAssociatedTrainer(string generalTraineeAssociatedToTrainerIds, out string errorMessage);

        DBTMActivitiesListViewModel GetTraineeActivitiesList(string personCode,int numberOfDaysRecord,DataTableViewModel dataTableModel);
        DBTMActivitiesDetailsListViewModel GetTraineeActivitiesDetailsList(long dBTMDeviceDataId,DataTableViewModel dataTableModel);
    }
}
