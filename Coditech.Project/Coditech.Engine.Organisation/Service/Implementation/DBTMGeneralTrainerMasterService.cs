using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;

using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMGeneralTrainerMasterService : GeneralTrainerMasterService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMGeneralTrainerMasterService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(coditechLogging, serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
        }

        protected override GeneralPersonModel GetGeneralPersonDetailsByEntityType(long entityId, string entityType)
        {
            long personId = 0;
            string centreCode = string.Empty;
            string personCode = string.Empty;
            short generalDepartmentMasterId = 0;
            if (entityType == UserTypeEnum.Trainee.ToString())
            {
                DBTMTraineeDetails dbtmTraineeDetails = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>()).Table.FirstOrDefault(x => x.DBTMTraineeDetailId == entityId);
                if (IsNotNull(dbtmTraineeDetails))
                {
                    personId = dbtmTraineeDetails.PersonId;
                    centreCode = dbtmTraineeDetails.CentreCode;
                }
                return base.BindGeneralPersonInformation(personId, centreCode, personCode, generalDepartmentMasterId);
            }
            else
            {
                return base.GetGeneralPersonDetailsByEntityType(entityId, entityType);
            }
        }

        public override GeneralTrainerModel CreateTrainer(GeneralTrainerModel generalTrainerModel)
        {
            generalTrainerModel = base.CreateTrainer(generalTrainerModel);
            if (!generalTrainerModel.HasError)
            {

                return generalTrainerModel;
            }
            return generalTrainerModel;
        }

    }
}
