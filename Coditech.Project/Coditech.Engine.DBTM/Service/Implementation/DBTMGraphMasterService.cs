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
        private readonly ICoditechRepository<DBTMGraphVerticalViewSequence> _dBTMGraphVerticalViewSequenceRepository;

        public DBTMGraphMasterService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMGraphMasterRepository = new CoditechRepository<DBTMGraphMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestMasterRepository = new CoditechRepository<DBTMTestMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMGraphVerticalViewSequenceRepository = new CoditechRepository<DBTMGraphVerticalViewSequence>(_serviceProvider.GetService<CoditechCustom_Entities>());
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

        //Get Graph Vertical View Sequence by DBTMGraphMasterId.
        public virtual DBTMGraphVerticalViewSequenceListModel GetGraphVerticalViewSequenceList(int dBTMGraphMasterId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            List<DBTMGraphVerticalViewSequence> graphVerticalViewSequenceList = _dBTMGraphVerticalViewSequenceRepository.Table.Where(x => x.DBTMGraphMasterId == dBTMGraphMasterId).OrderBy(x => x.SequenceNumber).ToList();
            List<DBTMGraphVerticalViewSequenceModel> graphVerticalViewSequenceModelList = graphVerticalViewSequenceList.Select(x => new DBTMGraphVerticalViewSequenceModel
            {
                DBTMGraphVerticalViewSequenceId = x.DBTMGraphVerticalViewSequenceId,
                DBTMGraphMasterId = x.DBTMGraphMasterId,
                ParameterCode = x.ParameterCode ?? string.Empty,
                IsCalculatedParameter = x.IsCalculatedParameter,
                Recursion = x.Recursion,
                SequenceNumber = x.SequenceNumber,
                ConsecutiveParameterCode = x.ConsecutiveParameterCode ?? string.Empty,
                IsCalculatedConsecutiveParameterCode = x.IsCalculatedConsecutiveParameterCode ?? false,
                ColumnName = x.ColumnName ?? string.Empty,
                ColumnDisplayName = x.ColumnDisplayName ?? string.Empty,
                HelpText = x.HelpText ?? string.Empty,
                DisplayOn = x.DisplayOn ?? string.Empty,
                ColumnCellColor = x.ColumnCellColor ?? string.Empty,
                IsColumnCellBold = x.IsColumnCellBold ?? false
            }).ToList();
            DBTMGraphVerticalViewSequenceListModel listModel = new DBTMGraphVerticalViewSequenceListModel
            {
                DBTMGraphVerticalViewSequenceList = graphVerticalViewSequenceModelList,
                DBTMGraphMasterId = dBTMGraphMasterId
            };
            if (dBTMGraphMasterId > 0)
            {
                var graphMaster = _dBTMGraphMasterRepository.Table.Where(x => x.DBTMGraphMasterId == dBTMGraphMasterId).Select(x => new { x.GraphName, x.GraphCode }).FirstOrDefault();
                if (graphMaster != null)
                {
                    listModel.GraphName = graphMaster.GraphName;
                    listModel.GraphCode = graphMaster.GraphCode;
                }
            }
            return listModel;
        }

        public virtual DBTMGraphVerticalViewSequenceModel GetGraphVerticalViewSequence(int dBTMGraphVerticalViewSequenceId)
        {
            if (dBTMGraphVerticalViewSequenceId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMGraphVerticalViewSequenceId"));

            DBTMGraphVerticalViewSequence graphVerticalViewSequence = _dBTMGraphVerticalViewSequenceRepository.Table.Where(x => x.DBTMGraphVerticalViewSequenceId == dBTMGraphVerticalViewSequenceId).FirstOrDefault();
            DBTMGraphVerticalViewSequenceModel model = graphVerticalViewSequence?.FromEntityToModel<DBTMGraphVerticalViewSequenceModel>();
            return model;
        }

        //Update GraphVerticalViewSequence

        public virtual bool UpdateGraphVerticalViewSequence(DBTMGraphVerticalViewSequenceModel model)
        {
            if (IsNull(model))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (model.DBTMGraphVerticalViewSequenceId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMGraphVerticalViewSequenceId"));

            DBTMGraphVerticalViewSequence entity = model.FromModelToEntity<DBTMGraphVerticalViewSequence>();
            bool isUpdated = _dBTMGraphVerticalViewSequenceRepository.Update(entity);
            if (!isUpdated)
            {
                model.HasError = true;
                model.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isUpdated;
        }

        public virtual DBTMGraphVerticalViewSequenceModel UpdateGraphVerticalSequenceNumber(DBTMGraphVerticalViewSequenceModel model)
        {
            if (IsNull(model))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (model.DBTMGraphVerticalViewSequenceList == null || !model.DBTMGraphVerticalViewSequenceList.Any())
                return model;
            foreach (var updatedItem in model.DBTMGraphVerticalViewSequenceList)
            {
                var existing = _dBTMGraphVerticalViewSequenceRepository.Table.FirstOrDefault(x => x.DBTMGraphVerticalViewSequenceId == updatedItem.DBTMGraphVerticalViewSequenceId);
                if (existing != null)
                {
                    existing.SequenceNumber = updatedItem.SequenceNumber;
                    existing.ModifiedDate = DateTime.Now;
                    _dBTMGraphVerticalViewSequenceRepository.Update(existing);
                }
            }
            return model;
        }

        //Create Graph Vertical View Sequence.
        public virtual DBTMGraphVerticalViewSequenceModel CreateGraphVerticalViewSequence(DBTMGraphVerticalViewSequenceModel model)
        {
            if (IsNull(model))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);
            DBTMGraphVerticalViewSequence entity = model.FromModelToEntity<DBTMGraphVerticalViewSequence>();
            DBTMGraphVerticalViewSequence data = _dBTMGraphVerticalViewSequenceRepository.Insert(entity);
            if (data?.DBTMGraphVerticalViewSequenceId > 0)
            {
                model.DBTMGraphVerticalViewSequenceId = data.DBTMGraphVerticalViewSequenceId;
            }
            else
            {
                model.HasError = true;
                model.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return model;
        }

        //Delete GraphVerticalViewSequence.
        public virtual bool DeleteGraphVerticalViewSequence(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
            {
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMGraphVerticalViewSequenceId"));
            }
            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<Coditech_Entities>());
            objStoredProc.SetParameter("DBTMGraphVerticalViewSequenceId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteDBTMGraphVerticalViewSequence @DBTMGraphVerticalViewSequenceId, @Status OUT", 1, out status);
            return status == 1;
        }

        #region Protected Method
        #endregion
    }
}
