using Coditech.API.Data;
using Coditech.API.Model.Custom.DBTM.DBTMApplicationVersion;
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
    public class DBTMApplicationVersionService :IDBTMApplicationVersionService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMApplicationVersion> _dBTMApplicationVersionRepository;
        public DBTMApplicationVersionService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMApplicationVersionRepository = new CoditechRepository<DBTMApplicationVersion>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        public virtual DBTMApplicationVersionListModel GetDBTMApplicationVersionList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMApplicationVersionModel> objStoredProc = new CoditechViewRepository<DBTMApplicationVersionModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMApplicationVersionModel> dBTMApplicationVersionList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMApplicationVersionList @WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 4, out pageListModel.TotalRowCount)?.ToList();
            DBTMApplicationVersionListModel listModel = new DBTMApplicationVersionListModel();

            listModel.DBTMApplicationVersionList = dBTMApplicationVersionList?.Count > 0 ? dBTMApplicationVersionList : new List<DBTMApplicationVersionModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }

        //Create DBTMApplicationVersion.
        public virtual DBTMApplicationVersionModel CreateDBTMApplicationVersion(DBTMApplicationVersionModel dBTMApplicationVersionModel)
        {
            if (IsNull(dBTMApplicationVersionModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);
            DBTMApplicationVersion dBTMApplicationVersion = dBTMApplicationVersionModel.FromModelToEntity<DBTMApplicationVersion>();
            //Create new DBTMApplicationVersion and return it.
            DBTMApplicationVersion dBTMApplicationVersionData = _dBTMApplicationVersionRepository.Insert(dBTMApplicationVersion);
            if (dBTMApplicationVersionData?.DBTMApplicationVersionId > 0)
            {
                dBTMApplicationVersionModel.DBTMApplicationVersionId = dBTMApplicationVersionData.DBTMApplicationVersionId;
            }
            else
            {
                dBTMApplicationVersionModel.HasError = true;
                dBTMApplicationVersionModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return dBTMApplicationVersionModel;
        }

        //Get DBTMApplicationVersion by DBTMApplicationVersion id.
        public virtual DBTMApplicationVersionModel GetDBTMApplicationVersion(short dBTMApplicationVersionId)
        {
            if (dBTMApplicationVersionId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMApplicationVersionId"));

            //Get the DBTMApplicationVersion Details based on id.
            DBTMApplicationVersion dBTMApplicationVersion = _dBTMApplicationVersionRepository.Table.Where(x => x.DBTMApplicationVersionId == dBTMApplicationVersionId)?.FirstOrDefault();
            DBTMApplicationVersionModel dBTMApplicationVersionModel = dBTMApplicationVersion?.FromEntityToModel<DBTMApplicationVersionModel>();
            return dBTMApplicationVersionModel;
        }

        //Update DBTMApplicationVersion.
        public virtual bool UpdateDBTMApplicationVersion(DBTMApplicationVersionModel dBTMApplicationVersionModel)
        {
            if (IsNull(dBTMApplicationVersionModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (dBTMApplicationVersionModel.DBTMApplicationVersionId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMApplicationVersionID"));

            DBTMApplicationVersion dBTMApplicationVersion = dBTMApplicationVersionModel.FromModelToEntity<DBTMApplicationVersion>();

            //Update DBTMApplicationVersion
            bool isDBTMApplicationVersionUpdated = _dBTMApplicationVersionRepository.Update(dBTMApplicationVersion);
            if (!isDBTMApplicationVersionUpdated)
            {
                dBTMApplicationVersionModel.HasError = true;
                dBTMApplicationVersionModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isDBTMApplicationVersionUpdated;
        }

        //Delete DBTMApplicationVersion.
        public virtual bool DeleteDBTMApplicationVersion(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMApplicationVersionID"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("DBTMApplicationVersionId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteDBTMApplicationVersion @DBTMApplicationVersionId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        #region Protected Method
        #endregion
    }
}