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
    public class DBTMActivityCategoryService : IDBTMActivityCategoryService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMActivityCategory> _dBTMActivityCategoryRepository;
        public DBTMActivityCategoryService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMActivityCategoryRepository = new CoditechRepository<DBTMActivityCategory>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        public virtual DBTMActivityCategoryListModel GetDBTMActivityCategoryList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMActivityCategoryModel> objStoredProc = new CoditechViewRepository<DBTMActivityCategoryModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMActivityCategoryModel> dBTMActivityCategoryList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMActivityCategoryList @WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 4, out pageListModel.TotalRowCount)?.ToList();
            DBTMActivityCategoryListModel listModel = new DBTMActivityCategoryListModel();

            listModel.DBTMActivityCategoryList = dBTMActivityCategoryList?.Count > 0 ? dBTMActivityCategoryList : new List<DBTMActivityCategoryModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }

        //Create DBTMActivityCategory.
        public virtual DBTMActivityCategoryModel CreateDBTMActivityCategory(DBTMActivityCategoryModel dBTMActivityCategoryModel)
        {
            if (IsNull(dBTMActivityCategoryModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (IsDBTMActivityCategoryCodeAlreadyExist(dBTMActivityCategoryModel.ActivityCategoryCode))
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "ActivityCategoryCode"));

            DBTMActivityCategory dBTMActivityCategory = dBTMActivityCategoryModel.FromModelToEntity<DBTMActivityCategory>();

            //Create new DBTMActivityCategory and return it.
            DBTMActivityCategory dBTMActivityCategoryData = _dBTMActivityCategoryRepository.Insert(dBTMActivityCategory);
            if (dBTMActivityCategoryData?.DBTMActivityCategoryId > 0)
            {
                dBTMActivityCategoryModel.DBTMActivityCategoryId = dBTMActivityCategoryData.DBTMActivityCategoryId;
            }
            else
            {
                dBTMActivityCategoryModel.HasError = true;
                dBTMActivityCategoryModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return dBTMActivityCategoryModel;
        }

        //Get DBTMActivityCategory by DBTMActivityCategory id.
        public virtual DBTMActivityCategoryModel GetDBTMActivityCategory(short dBTMActivityCategoryId)
        {
            if (dBTMActivityCategoryId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMActivityCategoryId"));

            //Get the DBTMActivityCategory Details based on id.
            DBTMActivityCategory dBTMActivityCategory = _dBTMActivityCategoryRepository.Table.Where(x => x.DBTMActivityCategoryId == dBTMActivityCategoryId)?.FirstOrDefault();
            DBTMActivityCategoryModel dBTMActivityCategoryModel = dBTMActivityCategory?.FromEntityToModel<DBTMActivityCategoryModel>();
            return dBTMActivityCategoryModel;
        }

        //Update DBTMActivityCategory.
        public virtual bool UpdateDBTMActivityCategory(DBTMActivityCategoryModel dBTMActivityCategoryModel)
        {
            if (IsNull(dBTMActivityCategoryModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (dBTMActivityCategoryModel.DBTMActivityCategoryId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMActivityCategoryID"));

            if (IsDBTMActivityCategoryCodeAlreadyExist(dBTMActivityCategoryModel.ActivityCategoryCode, dBTMActivityCategoryModel.DBTMActivityCategoryId))
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "Activity Category Code"));

            DBTMActivityCategory dBTMActivityCategory = dBTMActivityCategoryModel.FromModelToEntity<DBTMActivityCategory>();

            //Update DBTMActivityCategory
            bool isDBTMActivityCategoryUpdated = _dBTMActivityCategoryRepository.Update(dBTMActivityCategory);
            if (!isDBTMActivityCategoryUpdated)
            {
                dBTMActivityCategoryModel.HasError = true;
                dBTMActivityCategoryModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isDBTMActivityCategoryUpdated;
        }

        //Delete DBTMActivityCategory.
        public virtual bool DeleteDBTMActivityCategory(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMActivityCategoryID"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("DBTMActivityCategoryId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteDBTMActivityCategory @DBTMActivityCategoryId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        #region Protected Method
        //Check if Activity Category code is already present or not.
        protected virtual bool IsDBTMActivityCategoryCodeAlreadyExist(string activityCategoryCode, short dBTMActivityCategoryId = 0)
         => _dBTMActivityCategoryRepository.Table.Any(x => x.ActivityCategoryCode == activityCategoryCode && (x.DBTMActivityCategoryId != dBTMActivityCategoryId || dBTMActivityCategoryId == 0));
        #endregion
    }
}
