using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Logger;
namespace Coditech.Admin.Agents
{
    public class DBTMGeneralCommonAgent : BaseAgent, IDBTMGeneralCommonAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IDBTMGeneralCommonClient _dBTMGeneralCommonClient;
        #endregion

        #region Public Constructor
        public DBTMGeneralCommonAgent(ICoditechLogging coditechLogging, IDBTMGeneralCommonClient dBTMGeneralCommonClient)
        {
            _coditechLogging = coditechLogging;
            _dBTMGeneralCommonClient = GetClient<IDBTMGeneralCommonClient>(dBTMGeneralCommonClient);
        }
        #endregion

        #region Public Methods
        public virtual DBTMDeviceDataDetailsModel GetDBTMDeviceDataDecrypted(string dBTMDeviceDataIds)
        {

            DBTMDeviceDataDetailsResponse response = _dBTMGeneralCommonClient.GetDBTMDeviceDataDecrypted(dBTMDeviceDataIds);
            return response?.DBTMDeviceDataDetailsModel;
        }
        #endregion
    }
}
