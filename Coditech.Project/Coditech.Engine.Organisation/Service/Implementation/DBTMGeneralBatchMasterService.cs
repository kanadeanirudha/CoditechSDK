using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Specialized;
using System.Data;

using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMGeneralBatchMasterService : GeneralBatchMasterService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        public DBTMGeneralBatchMasterService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(coditechLogging, serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
        }

        #region GeneralBatchUser
        public override bool AssociateUnAssociateBatchwiseUser(GeneralBatchUserModel generalBatchUserModel)
        {
            if (generalBatchUserModel.GeneralBatchUserId == 0)
                generalBatchUserModel.ActivityStatusEnumId = GetEnumIdByEnumCode("Pending", "DBTMTestStatus");
          
            return base.AssociateUnAssociateBatchwiseUser(generalBatchUserModel);
        }

        public override GeneralBatchUserListModel GetGeneralBatchUserList(int generalBatchMasterId, string userType, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<GeneralBatchUserModel> objStoredProc = new CoditechViewRepository<GeneralBatchUserModel>(_serviceProvider.GetService<Coditech_Entities>());
            objStoredProc.SetParameter("@GeneralBatchMasterId", generalBatchMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@UserType", userType, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<GeneralBatchUserModel> batchList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMGeneralBatchUserAssociatedList @GeneralBatchMasterId,@UserType,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 6, out pageListModel.TotalRowCount)?.ToList();
            GeneralBatchUserListModel listModel = new GeneralBatchUserListModel();

            listModel.GeneralBatchUserList = batchList?.Count > 0 ? batchList : new List<GeneralBatchUserModel>();
            listModel.BindPageListModel(pageListModel);

            if (generalBatchMasterId > 0)
            {
                GeneralBatchModel model = GetGeneralBatch(generalBatchMasterId);
                if (IsNotNull(listModel))
                {
                    listModel.BatchName = model.BatchName;
                }
            }
            listModel.GeneralBatchMasterId = generalBatchMasterId;
            return listModel;
        }

        #endregion
    }
}
