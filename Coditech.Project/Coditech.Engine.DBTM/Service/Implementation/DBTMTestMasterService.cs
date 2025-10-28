using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;
using System.Collections.Specialized;
using System.Data;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMTestMasterService : BaseService, IDBTMTestMasterService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMTestMaster> _dBTMTestMasterRepository;
        private readonly ICoditechRepository<DBTMTestParameterListViewSequence> _dBTMActivityListViewSequenceMasterRepository;
        private readonly ICoditechRepository<DBTMTestParameter> _dBTMTestParameterRepository;
        private readonly ICoditechRepository<DBTMParametersAssociatedToTest> _dBTMParametersAssociatedToTestRepository;
        private readonly ICoditechRepository<DBTMTestCalculation> _dBTMTestCalculationRepository;
        private readonly ICoditechRepository<DBTMCalculationAssociatedToTest> _dBTMCalculationAssociatedToTestRepository;
        private readonly ICoditechRepository<MediaDetail> _mediaDetailRepository;
        private readonly ICoditechRepository<DBTMGraphMaster> _dBTMGraphMasterRepository;
        private readonly ICoditechRepository<DBTMTestGraph> _dBTMTestGraphRepository;
        private readonly ICoditechRepository<DBTMPerformanceMatrix> _dBTMPerformanceMatrixRepository;
        public DBTMTestMasterService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMTestMasterRepository = new CoditechRepository<DBTMTestMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMActivityListViewSequenceMasterRepository = new CoditechRepository<DBTMTestParameterListViewSequence>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestParameterRepository = new CoditechRepository<DBTMTestParameter>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMParametersAssociatedToTestRepository = new CoditechRepository<DBTMParametersAssociatedToTest>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestCalculationRepository = new CoditechRepository<DBTMTestCalculation>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMCalculationAssociatedToTestRepository = new CoditechRepository<DBTMCalculationAssociatedToTest>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _mediaDetailRepository = new CoditechRepository<MediaDetail>(_serviceProvider.GetService<Coditech_Entities>());
            _dBTMGraphMasterRepository = new CoditechRepository<DBTMGraphMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestGraphRepository = new CoditechRepository<DBTMTestGraph>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMPerformanceMatrixRepository = new CoditechRepository<DBTMPerformanceMatrix>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        public virtual DBTMTestListModel GetDBTMTestList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMTestModel> objStoredProc = new CoditechViewRepository<DBTMTestModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMTestModel> dBTMTestList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMTestList @WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 4, out pageListModel.TotalRowCount)?.ToList();
            DBTMTestListModel listModel = new DBTMTestListModel();

            listModel.DBTMTestList = dBTMTestList?.Count > 0 ? dBTMTestList : new List<DBTMTestModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }

        //Create DBTMTest.
        public virtual DBTMTestModel CreateDBTMTest(DBTMTestModel dBTMTestModel)
        {
            if (IsNull(dBTMTestModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (IsDBTMTestNameAlreadyExist(dBTMTestModel.TestCode, dBTMTestModel.DBTMTestMasterId))
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "Test Code"));


            DBTMTestMaster dBTMTestMaster = dBTMTestModel.FromModelToEntity<DBTMTestMaster>();

            //Create new DBTMTest and return it.
            DBTMTestMaster dBTMTestData = _dBTMTestMasterRepository.Insert(dBTMTestMaster);
            if (dBTMTestData?.DBTMTestMasterId > 0)
            {
                dBTMTestModel.DBTMTestMasterId = dBTMTestData.DBTMTestMasterId;
                List<DBTMParametersAssociatedToTest> parametersAssociatedToTestlist = new List<DBTMParametersAssociatedToTest>();
                foreach (string item in dBTMTestModel.DBTMSelectedTestParameter)
                {
                    parametersAssociatedToTestlist.Add(new DBTMParametersAssociatedToTest()
                    {
                        DBTMTestMasterId = dBTMTestModel.DBTMTestMasterId,
                        DBTMTestParameterId = Convert.ToByte(item),
                        IsActive = true
                    });
                }

                _dBTMParametersAssociatedToTestRepository.Insert(parametersAssociatedToTestlist);

                List<DBTMCalculationAssociatedToTest> calculationAssociatedToTestlist = new List<DBTMCalculationAssociatedToTest>();
                foreach (string item in dBTMTestModel.DBTMSelectedTestCalculation)
                {
                    calculationAssociatedToTestlist.Add(new DBTMCalculationAssociatedToTest()
                    {
                        DBTMTestMasterId = dBTMTestModel.DBTMTestMasterId,
                        DBTMTestCalculationId = Convert.ToByte(item)
                    });
                }

                _dBTMCalculationAssociatedToTestRepository.Insert(calculationAssociatedToTestlist);


                List<DBTMTestGraph> dBTMTestGraphlist = new List<DBTMTestGraph>();
                foreach (string dBTMGraphMasterId in dBTMTestModel.DBTMSelectedGraph)
                {
                    dBTMTestGraphlist.Add(new DBTMTestGraph()
                    {
                        DBTMGraphMasterId = Convert.ToInt32(dBTMGraphMasterId),
                        DBTMTestMasterId = dBTMTestModel.DBTMTestMasterId,
                    });
                }

                _dBTMTestGraphRepository.Insert(dBTMTestGraphlist);
            }

            else
            {
                dBTMTestModel.HasError = true;
                dBTMTestModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return dBTMTestModel;
        }

        //Get DBTMTest by dBTMTestMasterId.
        public virtual DBTMTestModel GetDBTMTest(int dBTMTestMasterId)
        {
            if (dBTMTestMasterId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTestMasterId"));

            //Get the DBTMTest Details based on id.
            DBTMTestMaster dBTMTestMaster = _dBTMTestMasterRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestMasterId)?.FirstOrDefault();
            DBTMTestModel dBTMTestModel = dBTMTestMaster?.FromEntityToModel<DBTMTestModel>();
            if (IsNotNull(dBTMTestMaster))
            {
                dBTMTestModel.DBTMSelectedTestParameter = _dBTMParametersAssociatedToTestRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestMasterId)?.Select(y => y.DBTMTestParameterId.ToString())?.ToList();
                dBTMTestModel.DBTMSelectedTestCalculation = _dBTMCalculationAssociatedToTestRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestMasterId)?.Select(y => y.DBTMTestCalculationId.ToString())?.ToList();
                dBTMTestModel.DBTMSelectedGraph = _dBTMTestGraphRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestMasterId)?.Select(y => y.DBTMGraphMasterId.ToString())?.ToList();
            }
            if (dBTMTestModel.TestMediaId > 0)
            {
                var mediaDetail = _mediaDetailRepository.Table.Where(x => x.MediaId == dBTMTestModel.TestMediaId).FirstOrDefault();
                if (mediaDetail != null)
                {
                    dBTMTestModel.TestMediaPath = $"{GetMediaUrl()}{mediaDetail.Path}";
                    dBTMTestModel.TestMediaFileName = mediaDetail.FileName;
                }
            }
            return dBTMTestModel;
        }

        //Update DBTM Test 

        public virtual bool UpdateDBTMTest(DBTMTestModel dBTMTestModel)
        {
            if (IsNull(dBTMTestModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (dBTMTestModel.DBTMTestMasterId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTestMasterID"));

            DBTMTestMaster dBTMTestMaster = dBTMTestModel.FromModelToEntity<DBTMTestMaster>();

            //Update DBTMTest
            bool isdBTMTestUpdated = _dBTMTestMasterRepository.Update(dBTMTestMaster);
            if (isdBTMTestUpdated)
            {
                List<DBTMParametersAssociatedToTest> deleteDBTMParametersAssociatedToTest = null;
                List<DBTMParametersAssociatedToTest> insertDBTMParametersAssociatedToTest = null;
                List<DBTMParametersAssociatedToTest> parametersAssociatedToTestList = _dBTMParametersAssociatedToTestRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestModel.DBTMTestMasterId)?.ToList();

                foreach (string item in dBTMTestModel.DBTMSelectedTestParameter)
                {
                    if (!parametersAssociatedToTestList.Any(x => x.DBTMTestParameterId.ToString() == item))
                    {
                        if (IsNull(insertDBTMParametersAssociatedToTest))
                        {
                            insertDBTMParametersAssociatedToTest = new List<DBTMParametersAssociatedToTest>();
                        }
                        insertDBTMParametersAssociatedToTest.Add(new DBTMParametersAssociatedToTest()
                        {
                            DBTMTestMasterId = dBTMTestModel.DBTMTestMasterId,
                            DBTMTestParameterId = Convert.ToByte(item),
                            IsActive = true
                        });
                    }
                }
                foreach (DBTMParametersAssociatedToTest item in parametersAssociatedToTestList)
                {
                    if (!dBTMTestModel.DBTMSelectedTestParameter.Any(x => x == item.DBTMTestParameterId.ToString()))
                    {
                        if (IsNull(deleteDBTMParametersAssociatedToTest))
                        {
                            deleteDBTMParametersAssociatedToTest = new List<DBTMParametersAssociatedToTest>();
                        }
                        item.IsActive = false;
                        deleteDBTMParametersAssociatedToTest.Add(item);
                    }
                }
                if (insertDBTMParametersAssociatedToTest?.Count > 0)
                {
                    _dBTMParametersAssociatedToTestRepository.Insert(insertDBTMParametersAssociatedToTest);
                }
                if (deleteDBTMParametersAssociatedToTest?.Count > 0)
                {
                    _dBTMParametersAssociatedToTestRepository.Delete(deleteDBTMParametersAssociatedToTest);
                }

                List<DBTMCalculationAssociatedToTest> deleteDBTMCalculationAssociatedToTest = null;
                List<DBTMCalculationAssociatedToTest> insertDBTMCalculationAssociatedToTest = null;
                List<DBTMCalculationAssociatedToTest> calculationAssociatedToTestList = _dBTMCalculationAssociatedToTestRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestModel.DBTMTestMasterId)?.ToList();

                foreach (string item in dBTMTestModel.DBTMSelectedTestCalculation)
                {
                    if (!calculationAssociatedToTestList.Any(x => x.DBTMTestCalculationId.ToString() == item))
                    {
                        if (IsNull(insertDBTMCalculationAssociatedToTest))
                        {
                            insertDBTMCalculationAssociatedToTest = new List<DBTMCalculationAssociatedToTest>();
                        }
                        insertDBTMCalculationAssociatedToTest.Add(new DBTMCalculationAssociatedToTest()
                        {
                            DBTMTestMasterId = dBTMTestModel.DBTMTestMasterId,
                            DBTMTestCalculationId = Convert.ToByte(item)
                        });
                    }
                }
                foreach (DBTMCalculationAssociatedToTest item in calculationAssociatedToTestList)
                {
                    if (!dBTMTestModel.DBTMSelectedTestCalculation.Any(x => x == item.DBTMTestCalculationId.ToString()))
                    {
                        if (IsNull(deleteDBTMCalculationAssociatedToTest))
                        {
                            deleteDBTMCalculationAssociatedToTest = new List<DBTMCalculationAssociatedToTest>();
                        }
                        deleteDBTMCalculationAssociatedToTest.Add(item);
                    }
                }
                if (insertDBTMCalculationAssociatedToTest?.Count > 0)
                {
                    _dBTMCalculationAssociatedToTestRepository.Insert(insertDBTMCalculationAssociatedToTest);
                }
                if (deleteDBTMCalculationAssociatedToTest?.Count > 0)
                {
                    _dBTMCalculationAssociatedToTestRepository.Delete(deleteDBTMCalculationAssociatedToTest);
                }

                List<DBTMTestGraph> deleteDBTMTestGraphList = null;
                List<DBTMTestGraph> insertDBTMTestGraphList = null;

                List<DBTMTestGraph> existingTestGraphList = _dBTMTestGraphRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestModel.DBTMTestMasterId).ToList();

                foreach (string graphId in dBTMTestModel.DBTMSelectedGraph)
                {
                    if (!existingTestGraphList.Any(x => x.DBTMGraphMasterId.ToString() == graphId))
                    {
                        if (IsNull(insertDBTMTestGraphList))
                        {
                            insertDBTMTestGraphList = new List<DBTMTestGraph>();
                        }

                        insertDBTMTestGraphList.Add(new DBTMTestGraph()
                        {
                            DBTMTestMasterId = dBTMTestModel.DBTMTestMasterId,
                            DBTMGraphMasterId = Convert.ToInt32(graphId),
                        });
                    }
                }

                foreach (DBTMTestGraph item in existingTestGraphList)
                {
                    if (!dBTMTestModel.DBTMSelectedGraph.Any(x => x == item.DBTMGraphMasterId.ToString()))
                    {
                        if (IsNull(deleteDBTMTestGraphList))
                        {
                            deleteDBTMTestGraphList = new List<DBTMTestGraph>();
                        }

                        deleteDBTMTestGraphList.Add(item);
                    }
                }

                if (insertDBTMTestGraphList?.Count > 0)
                {
                    _dBTMTestGraphRepository.Insert(insertDBTMTestGraphList);
                }

                if (deleteDBTMTestGraphList?.Count > 0)
                {
                    _dBTMTestGraphRepository.Delete(deleteDBTMTestGraphList);
                }
            }
            else
            {
                dBTMTestModel.HasError = true;
                dBTMTestModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isdBTMTestUpdated;
        }
          
        //Delete DBTMTest.
        public virtual bool DeleteDBTMTest(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTestMasterId"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("DBTMTestMasterId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteDBTMTest @DBTMTestMasterId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        public virtual DBTMTestParameterListModel GetDBTMTestParameter()
        {
            DBTMTestParameterListModel list = new DBTMTestParameterListModel
            {
                DBTMTestParameterList = (from a in _dBTMTestParameterRepository.Table
                                         select new DBTMTestParameterModel
                                         {
                                             DBTMTestParameterId = a.DBTMTestParameterId,
                                             ParameterName = a.ParameterName,
                                         }).ToList()
            };
            return list;
        }

        public virtual DBTMTestCalculationListModel GetDBTMTestCalculation()
        {
            DBTMTestCalculationListModel list = new DBTMTestCalculationListModel
            {
                DBTMTestCalculationList = (from a in _dBTMTestCalculationRepository.Table
                                           select new DBTMTestCalculationModel
                                           {
                                               DBTMTestCalculationId = a.DBTMTestCalculationId,
                                               CalculationName = a.CalculationName,
                                           }).ToList()
            };
            return list;
        }
        public virtual DBTMGraphMasterListModel GetDBTMGraph()
        {
            DBTMGraphMasterListModel list = new DBTMGraphMasterListModel
            {
                DBTMGraphMasterList = (from a in _dBTMGraphMasterRepository.Table
                                       select new DBTMGraphMasterModel
                                       {
                                           DBTMGraphMasterId = a.DBTMGraphMasterId,
                                           GraphName = a.GraphName,
                                           GraphCode = a.GraphCode,
                                       }).ToList()
            };
            return list;
        }
        public virtual DBTMGraphMasterListModel GetDBTMGraphByDBTMTestMasterId(int dBTMTestMasterId)
        {
            var graphList = (from a in _dBTMTestGraphRepository.Table
                             join b in _dBTMGraphMasterRepository.Table
                             on a.DBTMGraphMasterId equals b.DBTMGraphMasterId
                             where a.DBTMTestMasterId == dBTMTestMasterId
                             select new DBTMGraphMasterModel
                             {
                                 DBTMGraphMasterId = b.DBTMGraphMasterId,
                                 GraphName = b.GraphName,
                                 GraphCode = b.GraphCode
                             })
                             .Distinct()
                             .ToList();

            return new DBTMGraphMasterListModel
            {
                DBTMGraphMasterList = graphList
            };
        }
        public virtual DBTMPerformanceMatrixListModel GetDBTMPerformanceMatrixList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            DBTMPerformanceMatrixListModel list = new DBTMPerformanceMatrixListModel
            {
                DBTMPerformanceMatrixList = (from a in _dBTMPerformanceMatrixRepository.Table
                                             select new DBTMPerformanceMatrixModel
                                             {
                                                 DBTMPerformanceMatrixId = a.DBTMPerformanceMatrixId,
                                                 PerformanceMatrix = a.PerformanceMatrix,
                                             }).ToList()
            };
            return list;
        }

        //Get GetActivityListViewSequence by dBTMTestMasterId.
        public virtual DBTMActivityListViewSequenceListModel GetActivityListViewSequenceList(int dBTMTestMasterId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            List<DBTMTestParameterListViewSequence> activityList = _dBTMActivityListViewSequenceMasterRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestMasterId).OrderBy(x => x.SequenceNumber).ToList();

            // Map the entities to the DBTMActivityListViewSequenceModel list
            List<DBTMActivityListViewSequenceModel> activityViewSequenceList = activityList.Select(x => new DBTMActivityListViewSequenceModel
            {
                DBTMTestParameterListViewSequenceId = x.DBTMTestParameterListViewSequenceId,
                DBTMTestMasterId = x.DBTMTestMasterId,
                ParameterCode = x.ParameterCode,
                IsCalculatedParameter = x.IsCalculatedParameter,
                Recursion = x.Recursion,
                SequenceNumber = x.SequenceNumber,
                ConsecutiveParameterCode = x.ConsecutiveParameterCode,
                IsCalculatedConsecutiveParameterCode = x.IsCalculatedConsecutiveParameterCode,
                ColumnName = x.ColumnName,
                IsActive= x.IsActive
            }).ToList();

            DBTMActivityListViewSequenceListModel listModel = new DBTMActivityListViewSequenceListModel
            {
                DBTMActivityListViewSequenceList = activityViewSequenceList,
                DBTMTestMasterId = dBTMTestMasterId
            };

            return listModel;
        }

        public virtual DBTMActivityListViewSequenceModel GetActivityListViewSequence(int dBTMTestParameterListViewSequenceId)
        {
            if (dBTMTestParameterListViewSequenceId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTestParameterListViewSequenceId"));

            //Get the DBTMTest Details based on id.
            DBTMTestParameterListViewSequence dBTMTestMaster = _dBTMActivityListViewSequenceMasterRepository.Table.Where(x => x.DBTMTestParameterListViewSequenceId == dBTMTestParameterListViewSequenceId)?.FirstOrDefault();
            DBTMActivityListViewSequenceModel dBTMTestModel = dBTMTestMaster?.FromEntityToModel<DBTMActivityListViewSequenceModel>();
            return dBTMTestModel;
        }

        //Update ActivityListViewSequence

        public virtual bool UpdateActivityListViewSequence(DBTMActivityListViewSequenceModel dBTMTestModel)
        {
            if (IsNull(dBTMTestModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (dBTMTestModel.DBTMTestParameterListViewSequenceId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTestParameterListViewSequenceId"));

            DBTMTestParameterListViewSequence dBTMTestMaster = dBTMTestModel.FromModelToEntity<DBTMTestParameterListViewSequence>();

            //Update DBTMTest
            bool isdBTMTestUpdated = _dBTMActivityListViewSequenceMasterRepository.Update(dBTMTestMaster);
            if (!isdBTMTestUpdated)
            {
                dBTMTestModel.HasError = true;
                dBTMTestModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isdBTMTestUpdated;
        }

        public virtual DBTMActivityListViewSequenceModel UpdateSequenceNumber(DBTMActivityListViewSequenceModel dBTMActivityListViewSequenceModel)
        {
            if (IsNull(dBTMActivityListViewSequenceModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (dBTMActivityListViewSequenceModel.DBTMActivityListViewSequenceList == null ||
                !dBTMActivityListViewSequenceModel.DBTMActivityListViewSequenceList.Any())
                return dBTMActivityListViewSequenceModel;

            foreach (var updatedItem in dBTMActivityListViewSequenceModel.DBTMActivityListViewSequenceList)
            {
                var existing = _dBTMActivityListViewSequenceMasterRepository.Table
                    .FirstOrDefault(x => x.DBTMTestParameterListViewSequenceId == updatedItem.DBTMTestParameterListViewSequenceId);

                if (existing != null)
                {
                    existing.SequenceNumber = updatedItem.SequenceNumber;
                    existing.ModifiedDate = DateTime.Now;
                    _dBTMActivityListViewSequenceMasterRepository.Update(existing);
                }
            }

            return dBTMActivityListViewSequenceModel;
        }

        //Delete DBTMActivityListViewSequence.
        public virtual bool DeleteActivityListViewSequence(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTestParameterListViewSequenceId"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("DBTMTestParameterListViewSequenceId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteDBTMActivityListViewSequence @DBTMTestParameterListViewSequenceId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        #region Protected Method
        // Check if Test Name is already present or not.
        protected virtual bool IsDBTMTestNameAlreadyExist(string testCode, int dBTMTestMasterId = 0)
            => _dBTMTestMasterRepository.Table.Any(x => x.TestCode == testCode && (x.DBTMTestMasterId != dBTMTestMasterId || dBTMTestMasterId == 0));
        #endregion
    }
}