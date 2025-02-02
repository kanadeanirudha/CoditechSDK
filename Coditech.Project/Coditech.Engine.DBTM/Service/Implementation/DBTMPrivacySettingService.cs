using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Resources;
using System.Collections.Specialized;
using System.Data;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMPrivacySettingService : IDBTMPrivacySettingService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMPrivacySetting> _dBTMPrivacySettingRepository;

        public DBTMPrivacySettingService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMPrivacySettingRepository = new CoditechRepository<DBTMPrivacySetting>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }
        public virtual DBTMPrivacySettingListModel GetDBTMPrivacySettingList(string SelectedCentreCode, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMPrivacySettingModel> objStoredProc = new CoditechViewRepository<DBTMPrivacySettingModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@CentreCode", SelectedCentreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMPrivacySettingModel> dBTMPrivacySettingList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMPrivacySettingList @CentreCode,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 5, out pageListModel.TotalRowCount)?.ToList();
            DBTMPrivacySettingListModel listModel = new DBTMPrivacySettingListModel();

            listModel.DBTMPrivacySettingList = dBTMPrivacySettingList?.Count > 0 ? dBTMPrivacySettingList : new List<DBTMPrivacySettingModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }
        //Create DBTMPrivacySetting.
        public virtual DBTMPrivacySettingModel CreateDBTMPrivacySetting(DBTMPrivacySettingModel dBTMPrivacySettingModel)
        {
            if (IsNull(dBTMPrivacySettingModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            //if (IsDBTMActivityCategoryCodeAlreadyExist(dBTMPrivacySettingModel.ActivityCategoryCode))
            //    throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "ActivityCategoryCode"));

            DBTMPrivacySetting dBTMPrivacySetting = dBTMPrivacySettingModel.FromModelToEntity<DBTMPrivacySetting>();

            //Create new DBTMPrivacySetting and return it.
            DBTMPrivacySetting dBTMPrivacySettingData = _dBTMPrivacySettingRepository.Insert(dBTMPrivacySetting);
            if (dBTMPrivacySettingData?.DBTMPrivacySettingId > 0)
            {
                dBTMPrivacySettingModel.DBTMPrivacySettingId = dBTMPrivacySettingData.DBTMPrivacySettingId;
            }
            else
            {
                dBTMPrivacySettingModel.HasError = true;
                dBTMPrivacySettingModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return dBTMPrivacySettingModel;
        }
        //Get DBTMPrivacySetting by DBTMPrivacySetting id.
        public virtual DBTMPrivacySettingModel GetDBTMPrivacySetting(int dBTMPrivacySettingId)
        {
            if (dBTMPrivacySettingId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMPrivacySettingId"));

            //Get the DBTMPrivacySetting Details based on id.
            DBTMPrivacySetting dBTMPrivacySetting = _dBTMPrivacySettingRepository.Table.Where(x => x.DBTMPrivacySettingId == dBTMPrivacySettingId)?.FirstOrDefault();
            DBTMPrivacySettingModel dBTMPrivacySettingModel = dBTMPrivacySetting?.FromEntityToModel<DBTMPrivacySettingModel>();
            return dBTMPrivacySettingModel;
        }
        //Update DBTMPrivacySetting.
        public virtual bool UpdateDBTMPrivacySetting(DBTMPrivacySettingModel dBTMPrivacySettingModel)
        {
            if (IsNull(dBTMPrivacySettingModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (dBTMPrivacySettingModel.DBTMPrivacySettingId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMPrivacySettingId"));

            //if (IsDBTMActivityCategoryCodeAlreadyExist(dBTMActivityCategoryModel.ActivityCategoryCode, dBTMActivityCategoryModel.DBTMActivityCategoryId))
            //    throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "Activity Category Code"));

            DBTMPrivacySetting dBTMPrivacySetting = dBTMPrivacySettingModel.FromModelToEntity<DBTMPrivacySetting>();

            //Update DBTMPrivacySetting
            bool isDBTMPrivacySettingUpdated = _dBTMPrivacySettingRepository.Update(dBTMPrivacySetting);
            if (!isDBTMPrivacySettingUpdated)
            {
                dBTMPrivacySettingModel.HasError = true;
                dBTMPrivacySettingModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isDBTMPrivacySettingUpdated;
        }
        //Delete DBTMActivityCategory.
        public virtual bool DeleteDBTMPrivacySetting(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMPrivacySettingID"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("DBTMPrivacySettingId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteDBTMPrivacySetting @DBTMPrivacySettingId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

    }
}
