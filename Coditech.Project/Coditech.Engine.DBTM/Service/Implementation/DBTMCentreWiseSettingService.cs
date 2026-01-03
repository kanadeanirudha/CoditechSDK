using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Resources;
namespace Coditech.API.Service
{
    public class DBTMCentreWiseSettingService : IDBTMCentreWiseSettingService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMCentreWiseSetting> _dBTMCentreWiseSettingRepository;
        private readonly ICoditechRepository<OrganisationCentreMaster> _organisationCentreMasterRepository;

        public DBTMCentreWiseSettingService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMCentreWiseSettingRepository = new CoditechRepository<DBTMCentreWiseSetting>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _organisationCentreMasterRepository = new CoditechRepository<OrganisationCentreMaster>(_serviceProvider.GetService<Coditech_Entities>());
        }
        // Get DBTMCentreWiseSetting by OrganisationCentreId
        public virtual DBTMCentreWiseSettingModel GetDBTMCentreWiseSetting(int organisationCentreId)
        {
            // 1️⃣ Get Organisation Centre
            OrganisationCentreMaster organisationData =
                _organisationCentreMasterRepository.Table
                    .FirstOrDefault(x => x.OrganisationCentreMasterId == organisationCentreId);

            if (organisationData == null) throw new CoditechException(ErrorCodes.InvalidData, "Invalid OrganisationCentreMasterId");

            string centreCode = organisationData.CentreCode;

            DBTMCentreWiseSetting dBTMCentreWiseSetting = _dBTMCentreWiseSettingRepository.Table.FirstOrDefault(x => x.CentreCode == centreCode);

            if (HelperUtility.IsNull(dBTMCentreWiseSetting))
            {
                return new DBTMCentreWiseSettingModel
                {
                    CentreCode = centreCode,
                    OrganisationCentreMasterId = organisationData.OrganisationCentreMasterId
                };
            }

            DBTMCentreWiseSettingModel model = dBTMCentreWiseSetting.FromEntityToModel<DBTMCentreWiseSettingModel>();

            model.OrganisationCentreMasterId = organisationData.OrganisationCentreMasterId;

            return model;
        }

        //Update OrganisationMaster.
        public virtual DBTMCentreWiseSettingModel UpdateDBTMCentreWiseSetting(DBTMCentreWiseSettingModel model)
        {
            if (HelperUtility.IsNull(model))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            bool isSuccess;
            DBTMCentreWiseSetting entity = model.FromModelToEntity<DBTMCentreWiseSetting>();

            // UPDATE
            if (model.DBTMCentreWiseSettingId > 0)
            {
                isSuccess = _dBTMCentreWiseSettingRepository.Update(entity);
            }
            // INSERT
            else
            {
                DBTMCentreWiseSetting insertedEntity =
                    _dBTMCentreWiseSettingRepository.Insert(entity);

                isSuccess = insertedEntity != null;

                if (isSuccess)
                    model.DBTMCentreWiseSettingId = insertedEntity.DBTMCentreWiseSettingId;
            }

            if (!isSuccess)
            {
                model.HasError = true;
                model.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }

            return model;
        }

    }
}
