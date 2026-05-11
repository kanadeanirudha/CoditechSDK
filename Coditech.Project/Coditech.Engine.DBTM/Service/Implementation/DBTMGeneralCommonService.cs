using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace Coditech.API.Service
{
    public class DBTMGeneralCommonService : BaseService, IDBTMGeneralCommonService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMDeviceDataDetails> _dBTMDeviceDataDetailsRepository;


        public DBTMGeneralCommonService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMDeviceDataDetailsRepository = new CoditechRepository<DBTMDeviceDataDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }
        #region InsertDeviceData
        //Centrewise Test List
        public virtual DBTMDeviceDataDetailsModel GetDBTMDeviceDataDecrypted(string dBTMDeviceDataIds)
        {
            var model = new DBTMDeviceDataDetailsModel();

            if (string.IsNullOrEmpty(dBTMDeviceDataIds))
                return model;

            // Split IDs
            var ids = dBTMDeviceDataIds
                .Split(',')
                .Select(x => long.TryParse(x.Trim(), out var id) ? id : 0)
                .Where(x => x > 0)
                .ToList();

            if (!ids.Any())
                return model;

            // Fetch data for multiple IDs
            var list = _dBTMDeviceDataDetailsRepository.Table
                .Where(x => ids.Contains(x.DBTMDeviceDataId))
                .AsNoTracking()
                .Select(x => new DBTMDeviceDataDetailsModel
                {
                    DBTMDeviceDataDetailId = x.DBTMDeviceDataDetailId,
                    DBTMDeviceDataId = x.DBTMDeviceDataId,
                    ParameterCode = x.ParameterCode,

                    // Decrypt only if needed
                    ParameterValue = x.IsEncrypted ? EncryptionHelper.Decrypt(Convert.ToString(x.ParameterValue)) : x.ParameterValue,

                    FromTo = x.FromTo,
                    Row = x.Row,
                    Unit = x.Unit,
                    Comment1 = x.Comment1,
                    Comment2 = x.Comment2,
                    Comment3 = x.Comment3,
                    IsEncrypted = x.IsEncrypted,
                    CreatedBy = (long)x.CreatedBy
                })
                .ToList();

            model.DBTMDeviceDataDetailsList = list;

            return model;
        }
        #endregion

    }
}

