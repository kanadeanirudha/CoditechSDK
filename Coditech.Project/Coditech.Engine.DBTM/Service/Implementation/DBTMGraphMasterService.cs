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
    public class DBTMGraphMasterService : IDBTMGraphMasterService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMGraphMaster> _dBTMGraphMasterRepository;
        private readonly ICoditechRepository<DBTMTestMaster> _dBTMTestMasterRepository;
        public DBTMGraphMasterService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMGraphMasterRepository = new CoditechRepository<DBTMGraphMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestMasterRepository = new CoditechRepository<DBTMTestMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }
        public virtual DBTMGraphMasterListModel GetDBTMGraphList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMGraphMasterModel> objStoredProc = new CoditechViewRepository<DBTMGraphMasterModel>(_serviceProvider.GetService<Coditech_Entities>());
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMGraphMasterModel> dBTMGraphList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMGraphList @WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 4, out pageListModel.TotalRowCount)?.ToList();
            DBTMGraphMasterListModel listModel = new DBTMGraphMasterListModel();

            listModel.DBTMGraphMasterList = dBTMGraphList?.Count > 0 ? dBTMGraphList : new List<DBTMGraphMasterModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }
        //Create DBTMGraphMaster.
        public virtual DBTMGraphMasterModel CreateDBTMGraph(DBTMGraphMasterModel dBTMGraphMasterModel)
        {
            if (IsNull(dBTMGraphMasterModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);
            dBTMGraphMasterModel.TestCode = string.Join(",", dBTMGraphMasterModel.DBTMSelectedTestCode ?? new List<string>());
            DBTMGraphMaster dBTMGraphMaster = dBTMGraphMasterModel.FromModelToEntity<DBTMGraphMaster>();
            //Create new Graph and return it.
            DBTMGraphMaster graphData = _dBTMGraphMasterRepository.Insert(dBTMGraphMaster);
            if (graphData?.DBTMGraphMasterId > 0)
            {
                dBTMGraphMasterModel.DBTMGraphMasterId = graphData.DBTMGraphMasterId;
                dBTMGraphMasterModel.TestCode = graphData.TestCode ?? "";
                dBTMGraphMasterModel.DBTMSelectedTestCode = graphData.TestCode?.Split(',').ToList() ?? new List<string>();
            }
            else
            {
                dBTMGraphMasterModel.HasError = true;
                dBTMGraphMasterModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return dBTMGraphMasterModel;
        }

        //Get Graph by Graph id.
        public virtual DBTMGraphMasterModel GetDBTMGraph(string graphCode)
        {
            //Get the Graph Details based on id.
            DBTMGraphMaster dBTMGraphMaster = _dBTMGraphMasterRepository.Table.Where(x => x.GraphCode == graphCode).FirstOrDefault();
            DBTMGraphMasterModel dBTMGraphMasterModel = dBTMGraphMaster?.FromEntityToModel<DBTMGraphMasterModel>();
            if (dBTMGraphMasterModel != null && !string.IsNullOrEmpty(dBTMGraphMasterModel.TestCode))
            {
                dBTMGraphMasterModel.DBTMSelectedTestCode = dBTMGraphMasterModel.TestCode.Split(',').ToList();
            }
            return dBTMGraphMasterModel;
        }

        //Update DBTMGraph.
        public virtual bool UpdateDBTMGraph(DBTMGraphMasterModel dBTMGraphMasterModel)
        {
            if (IsNull(dBTMGraphMasterModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (dBTMGraphMasterModel.DBTMGraphMasterId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "GraphID"));
            dBTMGraphMasterModel.TestCode = string.Join(",", dBTMGraphMasterModel.DBTMSelectedTestCode ?? new List<string>());
            DBTMGraphMaster dBTMGraphMaster = dBTMGraphMasterModel.FromModelToEntity<DBTMGraphMaster>();
            //Update Graph
            bool isGraphUpdated = _dBTMGraphMasterRepository.Update(dBTMGraphMaster);
            if (isGraphUpdated)
            {
                dBTMGraphMasterModel.TestCode = dBTMGraphMaster.TestCode ?? "";
                dBTMGraphMasterModel.DBTMSelectedTestCode = dBTMGraphMaster.TestCode?.Split(',').ToList() ?? new List<string>();
            }
            else
            {
                dBTMGraphMasterModel.HasError = true;
                dBTMGraphMasterModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isGraphUpdated;
        }

        //Delete DBTMGraph.
        public virtual bool DeleteDBTMGraph(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "GraphCode"));
            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<Coditech_Entities>());
            objStoredProc.SetParameter("GraphCode", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteDBTMGraph @GraphCode,  @Status OUT", 1, out status);
            return status == 1 ? true : false;
        }

        public virtual DBTMTestListModel GetDBTMGraphTestCode()
        {
            DBTMTestListModel list = new DBTMTestListModel
            {
                DBTMTestList = (from a in _dBTMTestMasterRepository.Table
                                       select new DBTMTestModel
                                       {
                                           DBTMTestMasterId = a.DBTMTestMasterId,
                                           TestName = a.TestName,
                                           TestCode = a.TestCode,
                                       }).ToList()
            };
            return list;
        }
        #region Protected Method
        #endregion
    }
}
