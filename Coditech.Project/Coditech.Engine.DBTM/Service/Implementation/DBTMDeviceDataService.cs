using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Logger;
using Coditech.Resources;

using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMDeviceDataService : IDBTMDeviceDataService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMDeviceData> _dBTMDeviceDataRepository;
        private readonly ICoditechRepository<DBTMDeviceDataDetails> _dBTMDeviceDataDetailsRepository;
        private readonly ICoditechRepository<DBTMTraineeDetails> _dBTMTraineeDetailsRepository;
        private readonly ICoditechRepository<DBTMTestParameter> _dBTMTestParameterRepository;

        public DBTMDeviceDataService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMDeviceDataRepository = new CoditechRepository<DBTMDeviceData>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMDeviceDataDetailsRepository = new CoditechRepository<DBTMDeviceDataDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTraineeDetailsRepository = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestParameterRepository = new CoditechRepository<DBTMTestParameter>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        //Add DBTMDeviceData.
        public bool InsertDeviceData(List<DBTMDeviceDataModel> dBTMDeviceDataModelList)
        {
            if (IsNull(dBTMDeviceDataModelList))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (dBTMDeviceDataModelList.Count > 0)
            {
                DateTime createdDate = DateTime.Now;
                foreach (DBTMDeviceDataModel dBTMDeviceDataModel in dBTMDeviceDataModelList)
                {
                    DBTMTraineeDetails dBTMTraineeDetails = GetDBTMTraineeDetailsByCode(dBTMDeviceDataModel.PersonCode);
                    if (IsNull(dBTMTraineeDetails))
                        throw new CoditechException(ErrorCodes.InvalidData, "Invalid Person Code");

                    DBTMDeviceData dBTMDeviceData = new DBTMDeviceData()
                    {
                        DeviceSerialCode = dBTMDeviceDataModel.DeviceSerialCode,
                        PersonCode = dBTMDeviceDataModel.PersonCode,
                        TestCode = dBTMDeviceDataModel.TestCode,
                        Comments = dBTMDeviceDataModel.Comments,
                        Height = dBTMTraineeDetails.Height,
                        Weight = dBTMTraineeDetails.Weight,
                        CreatedBy = dBTMDeviceDataModel.CreatedBy,
                        CreatedDate = createdDate
                    };

                    DBTMDeviceData DBTMDeviceDataDetails = _dBTMDeviceDataRepository.Insert(dBTMDeviceData);

                    if (DBTMDeviceDataDetails?.DBTMDeviceDataId > 0)
                    {
                        dBTMDeviceDataModel.DBTMDeviceDataId = DBTMDeviceDataDetails.DBTMDeviceDataId;
                        List<DBTMDeviceDataDetails> dBTMDeviceDataDetailsList = new List<DBTMDeviceDataDetails>();
                        foreach (var item in dBTMDeviceDataModel?.DataList)
                        {
                            DBTMDeviceDataDetails dBTMDeviceDataDetails = new DBTMDeviceDataDetails()
                            {
                                DBTMDeviceDataId = DBTMDeviceDataDetails.DBTMDeviceDataId,
                                ParameterCode = item.ParameterCode,
                                ParameterValue = item.ParameterValue,
                                FromTo = item.FromTo,
                                Row = item.Row,
                                CreatedBy = dBTMDeviceDataModel.CreatedBy,
                                CreatedDate = createdDate
                            };
                            dBTMDeviceDataDetailsList.Add(dBTMDeviceDataDetails);
                        }
                        _dBTMDeviceDataDetailsRepository.Insert(dBTMDeviceDataDetailsList);
                    }
                }
            }
            return true;
        }

        public DBTMTraineeDetails GetDBTMTraineeDetailsByCode(string personCode)
    => _dBTMTraineeDetailsRepository.Table.Where(x => x.PersonCode == personCode).FirstOrDefault();
    }
}
