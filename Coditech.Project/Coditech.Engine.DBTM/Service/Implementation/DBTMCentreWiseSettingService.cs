using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Resources;
using System.Data;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMCentreWiseSettingService : IDBTMCentreWiseSettingService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMCentreWiseSetting> _dBTMCentreWiseSettingRepository;
        private readonly ICoditechRepository<OrganisationCentreMaster> _organisationCentreMasterRepository;
        private readonly ICoditechRepository<DBTMCentreWiseTest> _dBTMCentreWiseTestRepository;

        public DBTMCentreWiseSettingService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMCentreWiseSettingRepository = new CoditechRepository<DBTMCentreWiseSetting>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _organisationCentreMasterRepository = new CoditechRepository<OrganisationCentreMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _dBTMCentreWiseTestRepository = new CoditechRepository<DBTMCentreWiseTest>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }
        // Get DBTMCentreWiseSetting by OrganisationCentreId
        public virtual DBTMCentreWiseSettingModel GetDBTMCentreWiseSetting(int organisationCentreId)
        {
            OrganisationCentreMaster organisationData = _organisationCentreMasterRepository.Table.FirstOrDefault(x => x.OrganisationCentreMasterId == organisationCentreId);
            if (organisationData == null)
                throw new CoditechException(ErrorCodes.InvalidData, "Invalid OrganisationCentreMasterId");
            string centreCode = organisationData.CentreCode;
            DBTMCentreWiseSetting dBTMCentreWiseSetting = _dBTMCentreWiseSettingRepository.Table.FirstOrDefault(x => x.CentreCode == centreCode);
            DBTMCentreWiseSettingModel model = dBTMCentreWiseSetting?.FromEntityToModel<DBTMCentreWiseSettingModel>() ?? new DBTMCentreWiseSettingModel();
            model.CentreCode = centreCode;
            model.CentreName = organisationData.CentreName;
            model.TestListModel = GetCentreTests(centreCode);
            model.OrganisationCentreMasterId = organisationData.OrganisationCentreMasterId;
            if (model.TestListModel?.DBTMCentreWiseTestList != null)
            {
                foreach (var test in model.TestListModel.DBTMCentreWiseTestList)
                {
                    test.OrganisationCentreMasterId = organisationData.OrganisationCentreMasterId;
                }
            }
            return model;
        }

        public virtual DBTMCentreWiseTestListModel GetCentreTests(string centreCode)
        {
            CoditechViewRepository<DBTMCentreWiseTestModel> objStoredProc = new CoditechViewRepository<DBTMCentreWiseTestModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@CentreCode", centreCode, ParameterDirection.Input, DbType.String);
            List<DBTMCentreWiseTestModel> testList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMCentreWiseTestList @CentreCode")?.ToList();
            DBTMCentreWiseTestListModel model = new DBTMCentreWiseTestListModel
            {
                DBTMCentreWiseTestList = testList ?? new List<DBTMCentreWiseTestModel>()
            };
            return model;
        }

        public virtual bool AssociateUnAssociateCentreTest(DBTMCentreWiseTestModel dBTMCentreWiseTestModel)
        {
            bool isAssociateUnAssociateCentreWiseTest = false;
            DBTMCentreWiseTest dBTMCentreWiseTest = new DBTMCentreWiseTest();
            if (dBTMCentreWiseTestModel.DBTMCentreWiseTestId > 0)
            {
                dBTMCentreWiseTest = _dBTMCentreWiseTestRepository.Table.Where(x => x.DBTMCentreWiseTestId == dBTMCentreWiseTestModel.DBTMCentreWiseTestId)?.FirstOrDefault();
                isAssociateUnAssociateCentreWiseTest = _dBTMCentreWiseTestRepository.Delete(dBTMCentreWiseTest);
            }
            else
            {
                dBTMCentreWiseTest = dBTMCentreWiseTestModel.FromModelToEntity<DBTMCentreWiseTest>();
                dBTMCentreWiseTest = _dBTMCentreWiseTestRepository.Insert(dBTMCentreWiseTest);
                isAssociateUnAssociateCentreWiseTest = dBTMCentreWiseTest.DBTMCentreWiseTestId > 0;
            }
            if (!isAssociateUnAssociateCentreWiseTest)
            {
                dBTMCentreWiseTestModel.HasError = true;
                dBTMCentreWiseTestModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isAssociateUnAssociateCentreWiseTest;
        }

        //Update OrganisationMaster.
        public virtual DBTMCentreWiseSettingModel UpdateDBTMCentreWiseSetting(DBTMCentreWiseSettingModel model)
        {
            if (IsNull(model))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);
            bool isSuccess;
            DBTMCentreWiseSetting entity = model.FromModelToEntity<DBTMCentreWiseSetting>();
            if (model.DBTMCentreWiseSettingId > 0)
            {
                isSuccess = _dBTMCentreWiseSettingRepository.Update(entity);
            }
            else
            {
                DBTMCentreWiseSetting insertedEntity =  _dBTMCentreWiseSettingRepository.Insert(entity);
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
        public virtual DBTMCentreWiseTestModel AssociateCentreTests(int organisationCentreId, string centreCode, List<int> testIds)
        {
            DBTMCentreWiseTestModel result = new DBTMCentreWiseTestModel
            {
                OrganisationCentreMasterId = organisationCentreId,
                CentreCode = centreCode,
                HasError = false
            };
            if (testIds?.Count > 0)
            {
                foreach (var testId in testIds)
                {
                    var exists = _dBTMCentreWiseTestRepository.Table.Any(x => x.CentreCode == centreCode && x.DBTMTestMasterId == testId);
                    if (!exists)
                    {
                        DBTMCentreWiseTest entity = new DBTMCentreWiseTest
                        {
                            DBTMTestMasterId = testId,
                            CentreCode = centreCode,
                            CreatedDate = DateTime.Now
                        };
                        _dBTMCentreWiseTestRepository.Insert(entity);
                    }
                }
            }
            else
            {
                result.HasError = true;
                result.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return result;
        }

        public virtual DBTMCentreWiseTestModel UnAssociateCentreTests(int organisationCentreId, string centreCode, List<int> testIds)
        {
            DBTMCentreWiseTestModel result = new DBTMCentreWiseTestModel
            {
                OrganisationCentreMasterId = organisationCentreId,
                CentreCode = centreCode,
                HasError = false
            };
            if (testIds?.Count > 0)
            {
                foreach (var testId in testIds)
                {
                    DBTMCentreWiseTest entity = _dBTMCentreWiseTestRepository.Table.FirstOrDefault(x => x.CentreCode == centreCode && x.DBTMTestMasterId == testId);
                    if (entity != null)
                    {
                        _dBTMCentreWiseTestRepository.Delete(entity);
                    }
                }
            }
            return result;
        }
    }
}
