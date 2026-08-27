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
        private readonly ICoditechRepository<MediaDetail> _mediaDetailRepository;
        private readonly ICoditechRepository<DBTMGraphMaster> _dBTMGraphMasterRepository;
        private readonly ICoditechRepository<DBTMTestGraph> _dBTMTestGraphRepository;
        private readonly ICoditechRepository<DBTMPerformanceMatrix> _dBTMPerformanceMatrixRepository;
        private readonly ICoditechRepository<DBTMTestParameterVerticalViewSequence> _dBTMActivityVerticalViewSequenceMasterRepository;
        private readonly ICoditechRepository<DBTMCentreWiseTest> _dBTMCentreWiseTestRepository;
        private readonly ICoditechRepository<DBTMTestWisePerformanceStandard> _dBTMTestWisePerformanceStandardRepository;
        private readonly ICoditechRepository<DBTMTraineeDetails> _dBTMTraineeDetailsRepository;
        private readonly ICoditechRepository<DBTMDeviceData> _dBTMDeviceDataRepository;
        private readonly ICoditechRepository<DBTMTestwisePerformanceStandardCategory> _dBTMTestwisePerformanceStandardCategoryRepository;
        public DBTMTestMasterService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMTestMasterRepository = new CoditechRepository<DBTMTestMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMActivityListViewSequenceMasterRepository = new CoditechRepository<DBTMTestParameterListViewSequence>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMActivityVerticalViewSequenceMasterRepository = new CoditechRepository<DBTMTestParameterVerticalViewSequence>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _mediaDetailRepository = new CoditechRepository<MediaDetail>(_serviceProvider.GetService<Coditech_Entities>());
            _dBTMGraphMasterRepository = new CoditechRepository<DBTMGraphMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestGraphRepository = new CoditechRepository<DBTMTestGraph>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMPerformanceMatrixRepository = new CoditechRepository<DBTMPerformanceMatrix>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMCentreWiseTestRepository = new CoditechRepository<DBTMCentreWiseTest>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestWisePerformanceStandardRepository = new CoditechRepository<DBTMTestWisePerformanceStandard>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTraineeDetailsRepository = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMDeviceDataRepository = new CoditechRepository<DBTMDeviceData>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestwisePerformanceStandardCategoryRepository = new CoditechRepository<DBTMTestwisePerformanceStandardCategory>(_serviceProvider.GetService<CoditechCustom_Entities>());
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

        //Centrewise Test List
        public virtual DBTMCentreWiseTestListModel GetTestsByCentreCode(string centreCode)
        {
            var testList = (from test in _dBTMTestMasterRepository.Table
                            join centreTest in _dBTMCentreWiseTestRepository.Table on test.DBTMTestMasterId equals centreTest.DBTMTestMasterId
                            join performanceMatrix in _dBTMPerformanceMatrixRepository.Table on test.DBTMPerformanceMatrixId equals performanceMatrix.DBTMPerformanceMatrixId
                            where centreTest.CentreCode == centreCode
                            select new DBTMCentreWiseTestModel
                            {
                                DBTMTestMasterId = test.DBTMTestMasterId,
                                TestName = test.TestName,
                                CentreCode = $"{centreTest.CentreCode} ({performanceMatrix.PerformanceMatrix})",
                                DBTMCentreWiseTestId = centreTest.DBTMCentreWiseTestId,
                                IsAssociated = true
                            })
                   .ToList();
            DBTMCentreWiseTestListModel model = new DBTMCentreWiseTestListModel
            {
                DBTMCentreWiseTestList = testList
            };
            return model;
        }

        public virtual DBTMCentreWiseTestListModel GetTestsByCentreCodeV2(string centreCode, long? entityId, string userType)
        {
            List<DBTMCentreWiseTestModel> testList;

            if (entityId.HasValue && entityId.Value > 0 && string.Equals(userType, CustomConstants.Trainee, StringComparison.OrdinalIgnoreCase))
            {
                //performed tests
                testList = (
                    from trainee in _dBTMTraineeDetailsRepository.Table
                    join deviceData in _dBTMDeviceDataRepository.Table
                        on trainee.PersonCode equals deviceData.PersonCode
                    join test in _dBTMTestMasterRepository.Table
                        on deviceData.TestCode equals test.TestCode
                    join centreTest in _dBTMCentreWiseTestRepository.Table
                        on test.DBTMTestMasterId equals centreTest.DBTMTestMasterId
                    join performanceMatrix in _dBTMPerformanceMatrixRepository.Table
                        on test.DBTMPerformanceMatrixId equals performanceMatrix.DBTMPerformanceMatrixId
                    where trainee.DBTMTraineeDetailId == entityId.Value
                          && trainee.CentreCode == centreCode
                          && deviceData.IsValidRecord
                    select new DBTMCentreWiseTestModel
                    {
                        DBTMTestMasterId = test.DBTMTestMasterId,
                        TestName = $"{test.TestName}({performanceMatrix.PerformanceMatrix})",
                        CentreCode = centreTest.CentreCode,
                        DBTMCentreWiseTestId = centreTest.DBTMCentreWiseTestId,
                        IsAssociated = true
                    }
                )
                .GroupBy(x => x.DBTMTestMasterId)
                .Select(g => g.FirstOrDefault())
                .ToList();
            }
            else
            {
                // Centre-wise tests
                testList = (
                    from test in _dBTMTestMasterRepository.Table
                    join centreTest in _dBTMCentreWiseTestRepository.Table
                        on test.DBTMTestMasterId equals centreTest.DBTMTestMasterId
                    join performanceMatrix in _dBTMPerformanceMatrixRepository.Table
                        on test.DBTMPerformanceMatrixId equals performanceMatrix.DBTMPerformanceMatrixId
                    where centreTest.CentreCode == centreCode
                    select new DBTMCentreWiseTestModel
                    {
                        DBTMTestMasterId = test.DBTMTestMasterId,
                        TestName = $"{test.TestName}({performanceMatrix.PerformanceMatrix})",
                        CentreCode = centreTest.CentreCode,
                        DBTMCentreWiseTestId = centreTest.DBTMCentreWiseTestId,
                        IsAssociated = true
                    }
                ).ToList();
            }

            return new DBTMCentreWiseTestListModel
            {
                DBTMCentreWiseTestList = testList
            };
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
            if (dBTMTestModel.ActivityProtocolImage > 0)
            {
                var mediaDetail = _mediaDetailRepository.Table.Where(x => x.MediaId == dBTMTestModel.ActivityProtocolImage).FirstOrDefault();
                if (mediaDetail != null)
                {
                    dBTMTestModel.ActivityProtocolImagePath = $"{GetMediaUrl()}{mediaDetail.Path}";
                    dBTMTestModel.ActivityProtocolImageFileName = mediaDetail.FileName;
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

        public virtual DBTMGraphMasterListModel GetDBTMGraph(int dBTMTestMasterId)
        {
            string testCode = dBTMTestMasterId.ToString();
            var graphList = _dBTMGraphMasterRepository.Table
                            .Where(g => ("," + g.TestCode + ",").Contains("," + testCode + ","))
                            .Select(g => new DBTMGraphMasterModel
                            {
                                DBTMGraphMasterId = g.DBTMGraphMasterId,
                                GraphName = g.GraphName,
                                GraphCode = g.GraphCode,
                                GraphMode = g.GraphMode,
                                GraphType = g.GraphType,
                                IsActive = g.IsActive,
                                OrderBy = g.OrderBy
                            })
                            .OrderBy(g => g.OrderBy)
                            .ToList();

            return new DBTMGraphMasterListModel
            {
                DBTMGraphMasterList = graphList
            };
        }

        public virtual DBTMGraphMasterListModel GetDBTMGraphByDBTMTestMasterId(int dBTMTestMasterId, string graphMode)
        {
            var graphList = (from a in _dBTMTestGraphRepository.Table
                             join b in _dBTMGraphMasterRepository.Table
                             on a.DBTMGraphMasterId equals b.DBTMGraphMasterId
                             where a.DBTMTestMasterId == dBTMTestMasterId
                             select new DBTMGraphMasterModel
                             {
                                 DBTMGraphMasterId = b.DBTMGraphMasterId,
                                 GraphName = b.GraphName,
                                 GraphCode = b.GraphCode,
                                 GraphMode = b.GraphMode,
                                 GraphType = b.GraphType
                             })
                             .Distinct()
                             .ToList();

            if (!string.IsNullOrEmpty(graphMode))
            {
                graphList = graphList.Where(x => x.GraphMode == graphMode).ToList();
            }

            var graphListResult = graphList
                .Distinct()
                .OrderBy(x => x.GraphName)
                .ToList();

            return new DBTMGraphMasterListModel
            {
                DBTMGraphMasterList = graphListResult
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

            DBTMActivityListViewSequenceListModel listModel = new DBTMActivityListViewSequenceListModel
            {
                DBTMActivityListViewSequenceList = activityViewSequenceList,
                DBTMTestMasterId = dBTMTestMasterId
            };
            if (dBTMTestMasterId > 0)
            {
                listModel.TestName = _dBTMTestMasterRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestMasterId).Select(x => x.TestName).FirstOrDefault();
            }
            listModel.DBTMTestMasterId = dBTMTestMasterId;
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

        //Create Activity List View Sequence.
        public virtual DBTMActivityListViewSequenceModel CreateActivityListViewSequence(DBTMActivityListViewSequenceModel dBTMActivityListViewSequenceModel)
        {
            if (IsNull(dBTMActivityListViewSequenceModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            //if (IsParameterCodeAlreadyExist(dBTMActivityListViewSequenceModel.ParameterCode))
            //    throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "Parameter Code"));

            DBTMTestParameterListViewSequence dBTMTestParameterListViewSequence = dBTMActivityListViewSequenceModel.FromModelToEntity<DBTMTestParameterListViewSequence>();

            //Create new DBTM Activity List View Sequence and return it.
            DBTMTestParameterListViewSequence dBTMTestParameterListViewSequenceData = _dBTMActivityListViewSequenceMasterRepository.Insert(dBTMTestParameterListViewSequence);
            if (dBTMTestParameterListViewSequenceData?.DBTMTestParameterListViewSequenceId > 0)
            {
                dBTMActivityListViewSequenceModel.DBTMTestParameterListViewSequenceId = dBTMTestParameterListViewSequenceData.DBTMTestParameterListViewSequenceId;
            }
            else
            {
                dBTMActivityListViewSequenceModel.HasError = true;
                dBTMActivityListViewSequenceModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
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

        //Get GetActivityVerticalViewSequence by dBTMTestMasterId.
        public virtual DBTMActivityVerticalViewSequenceListModel GetActivityVerticalViewSequenceList(int dBTMTestMasterId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            List<DBTMTestParameterVerticalViewSequence> ActivityVertical = _dBTMActivityVerticalViewSequenceMasterRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestMasterId).OrderBy(x => x.SequenceNumber).ToList();

            // Map the entities to the DBTMActivityVerticalViewSequenceModel list
            List<DBTMActivityVerticalViewSequenceModel> activityViewSequenceList = ActivityVertical.Select(x => new DBTMActivityVerticalViewSequenceModel
            {
                DBTMTestParameterVerticalViewSequenceId = x.DBTMTestParameterVerticalViewSequenceId,
                DBTMTestMasterId = x.DBTMTestMasterId,
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

            DBTMActivityVerticalViewSequenceListModel listModel = new DBTMActivityVerticalViewSequenceListModel
            {
                DBTMActivityVerticalViewSequenceList = activityViewSequenceList,
                DBTMTestMasterId = dBTMTestMasterId
            };
            if (dBTMTestMasterId > 0)
            {
                listModel.TestName = _dBTMTestMasterRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestMasterId).Select(x => x.TestName).FirstOrDefault();
            }
            listModel.DBTMTestMasterId = dBTMTestMasterId;
            return listModel;
        }

        public virtual DBTMActivityVerticalViewSequenceModel GetActivityVerticalViewSequence(int dBTMTestParameterVerticalViewSequenceId)
        {
            if (dBTMTestParameterVerticalViewSequenceId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTestParameterVerticalViewSequenceId"));

            //Get the DBTMTest Details based on id.
            DBTMTestParameterVerticalViewSequence dBTMTestMaster = _dBTMActivityVerticalViewSequenceMasterRepository.Table.Where(x => x.DBTMTestParameterVerticalViewSequenceId == dBTMTestParameterVerticalViewSequenceId)?.FirstOrDefault();
            DBTMActivityVerticalViewSequenceModel dBTMTestModel = dBTMTestMaster?.FromEntityToModel<DBTMActivityVerticalViewSequenceModel>();
            return dBTMTestModel;
        }

        //Update ActivityVerticalViewSequence

        public virtual bool UpdateActivityVerticalViewSequence(DBTMActivityVerticalViewSequenceModel dBTMTestModel)
        {
            if (IsNull(dBTMTestModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (dBTMTestModel.DBTMTestParameterVerticalViewSequenceId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTestParameterVerticalViewSequenceId"));

            DBTMTestParameterVerticalViewSequence dBTMTestMaster = dBTMTestModel.FromModelToEntity<DBTMTestParameterVerticalViewSequence>();

            //Update DBTMTest
            bool isdBTMTestUpdated = _dBTMActivityVerticalViewSequenceMasterRepository.Update(dBTMTestMaster);
            if (!isdBTMTestUpdated)
            {
                dBTMTestModel.HasError = true;
                dBTMTestModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isdBTMTestUpdated;
        }

        public virtual DBTMActivityVerticalViewSequenceModel UpdateVerticalSequenceNumber(DBTMActivityVerticalViewSequenceModel dBTMActivityVerticalViewSequenceModel)
        {
            if (IsNull(dBTMActivityVerticalViewSequenceModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (dBTMActivityVerticalViewSequenceModel.DBTMActivityVerticalViewSequenceList == null ||
                !dBTMActivityVerticalViewSequenceModel.DBTMActivityVerticalViewSequenceList.Any())
                return dBTMActivityVerticalViewSequenceModel;

            foreach (var updatedItem in dBTMActivityVerticalViewSequenceModel.DBTMActivityVerticalViewSequenceList)
            {
                var existing = _dBTMActivityVerticalViewSequenceMasterRepository.Table.FirstOrDefault(x => x.DBTMTestParameterVerticalViewSequenceId == updatedItem.DBTMTestParameterVerticalViewSequenceId);

                if (existing != null)
                {
                    existing.SequenceNumber = updatedItem.SequenceNumber;
                    existing.ModifiedDate = DateTime.Now;
                    _dBTMActivityVerticalViewSequenceMasterRepository.Update(existing);
                }
            }

            return dBTMActivityVerticalViewSequenceModel;
        }

        //Create Activity List View Sequence.
        public virtual DBTMActivityVerticalViewSequenceModel CreateActivityVerticalViewSequence(DBTMActivityVerticalViewSequenceModel dBTMActivityVerticalViewSequenceModel)
        {
            if (IsNull(dBTMActivityVerticalViewSequenceModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            DBTMTestParameterVerticalViewSequence dBTMTestParameterVerticalViewSequence = dBTMActivityVerticalViewSequenceModel.FromModelToEntity<DBTMTestParameterVerticalViewSequence>();

            //Create new DBTM Activity Vertical View Sequence and return it.
            DBTMTestParameterVerticalViewSequence dBTMTestParameterVerticalViewSequenceData = _dBTMActivityVerticalViewSequenceMasterRepository.Insert(dBTMTestParameterVerticalViewSequence);
            if (dBTMTestParameterVerticalViewSequenceData?.DBTMTestParameterVerticalViewSequenceId > 0)
            {
                dBTMActivityVerticalViewSequenceModel.DBTMTestParameterVerticalViewSequenceId = dBTMTestParameterVerticalViewSequenceData.DBTMTestParameterVerticalViewSequenceId;
            }
            else
            {
                dBTMActivityVerticalViewSequenceModel.HasError = true;
                dBTMActivityVerticalViewSequenceModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return dBTMActivityVerticalViewSequenceModel;
        }

        //Delete DBTMActivityVerticalViewSequence.
        public virtual bool DeleteActivityVerticalViewSequence(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTestParameterVerticalViewSequenceId"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("DBTMTestParameterVerticalViewSequenceId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteDBTMActivityVerticalViewSequence @DBTMTestParameterVerticalViewSequenceId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        public virtual DBTMTestWisePerformanceStandardListModel GetDBTMTestWisePerformanceStandardList(int dBTMTestMasterId, short dBTMTestwisePerformanceStandardCategoryId)
        {
            CoditechViewRepository<DBTMTestWisePerformanceStandardModel> objStoredProc = new CoditechViewRepository<DBTMTestWisePerformanceStandardModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@DBTMTestMasterId", dBTMTestMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@DBTMTestwisePerformanceStandardCategoryId", dBTMTestwisePerformanceStandardCategoryId, ParameterDirection.Input, DbType.Int16);
            List<DBTMTestWisePerformanceStandardModel> list = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMTestwisePerformanceStandard @DBTMTestMasterId, @DBTMTestwisePerformanceStandardCategoryId")?.ToList();
            DBTMTestWisePerformanceStandardListModel dBTMTestWisePerformanceStandardList = new DBTMTestWisePerformanceStandardListModel
            {
                DBTMTestWisePerformanceStandardList = list,
                DBTMTestMasterId = dBTMTestMasterId,
                DBTMTestwisePerformanceStandardCategoryId = dBTMTestwisePerformanceStandardCategoryId,
                TestName = _dBTMTestMasterRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestMasterId).Select(x => x.TestName).FirstOrDefault()
            };
            return dBTMTestWisePerformanceStandardList;
        }

        public virtual DBTMTestWisePerformanceStandardModel CreateDBTMTestWisePerformanceStandard(DBTMTestWisePerformanceStandardModel dBTMTestWisePerformanceStandardModel)
        {
            if (IsNull(dBTMTestWisePerformanceStandardModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);
            DBTMTestWisePerformanceStandard entity = dBTMTestWisePerformanceStandardModel.FromModelToEntity<DBTMTestWisePerformanceStandard>();
            DBTMTestWisePerformanceStandard dBTMTestWisePerformanceStandard = _dBTMTestWisePerformanceStandardRepository.Insert(entity);
            if (dBTMTestWisePerformanceStandard?.DBTMTestWisePerformanceStandardId > 0)
            {
                dBTMTestWisePerformanceStandardModel.DBTMTestWisePerformanceStandardId = dBTMTestWisePerformanceStandard.DBTMTestWisePerformanceStandardId;
            }
            else
            {
                dBTMTestWisePerformanceStandardModel.HasError = true;
                dBTMTestWisePerformanceStandardModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return dBTMTestWisePerformanceStandardModel;
        }
        public virtual bool UpdateDBTMTestWisePerformanceStandard(DBTMTestWisePerformanceStandardModel dBTMTestWisePerformanceStandardModel)
        {
            if (IsNull(dBTMTestWisePerformanceStandardModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);
            if (dBTMTestWisePerformanceStandardModel.DBTMTestWisePerformanceStandardId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTestWisePerformanceStandardId"));
            DBTMTestWisePerformanceStandard dBTMTestWisePerformanceStandard = dBTMTestWisePerformanceStandardModel.FromModelToEntity<DBTMTestWisePerformanceStandard>();
            //Update DBTMTestWisePerformanceStandard
            bool isdBTMTestWisePerformanceStandardUpdated = _dBTMTestWisePerformanceStandardRepository.Update(dBTMTestWisePerformanceStandard);
            if (!isdBTMTestWisePerformanceStandardUpdated)
            {
                dBTMTestWisePerformanceStandardModel.HasError = true;
                dBTMTestWisePerformanceStandardModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isdBTMTestWisePerformanceStandardUpdated;
        }
        public virtual DBTMTestwisePerformanceStandardCategoryListModel GetDBTMTestwisePerformanceStandardCategoryList(short dBTMTestwisePerformanceStandardCategoryId)
        {
            List<DBTMTestwisePerformanceStandardCategoryModel> list;
            if (dBTMTestwisePerformanceStandardCategoryId > 0)
            {
                list = _dBTMTestwisePerformanceStandardCategoryRepository.Table.Where(x => x.DBTMTestwisePerformanceStandardCategoryId == dBTMTestwisePerformanceStandardCategoryId)
                    .Select(x => new DBTMTestwisePerformanceStandardCategoryModel
                    {
                        DBTMTestwisePerformanceStandardCategoryId = x.DBTMTestwisePerformanceStandardCategoryId,
                        Name = x.Name
                    }).ToList();
            }
            else
            {
                list = _dBTMTestwisePerformanceStandardCategoryRepository.Table
                    .Select(x => new DBTMTestwisePerformanceStandardCategoryModel
                    {
                        DBTMTestwisePerformanceStandardCategoryId = x.DBTMTestwisePerformanceStandardCategoryId,
                        Name = x.Name
                    }).ToList();
            }
            DBTMTestwisePerformanceStandardCategoryListModel model = new DBTMTestwisePerformanceStandardCategoryListModel
            {
                DBTMTestwisePerformanceStandardCategoryList = list
            };
            return model;
        }

        #region Protected Method
        // Check if Test Name is already present or not.
        protected virtual bool IsDBTMTestNameAlreadyExist(string testCode, int dBTMTestMasterId = 0)
            => _dBTMTestMasterRepository.Table.Any(x => x.TestCode == testCode && (x.DBTMTestMasterId != dBTMTestMasterId || dBTMTestMasterId == 0));

        //Check if Parameter Code is already present or not.
        protected virtual bool IsParameterCodeAlreadyExist(string parameterCode, int dBTMTestParameterListViewSequenceId = 0)
        => _dBTMActivityListViewSequenceMasterRepository.Table.Any(x => x.ParameterCode == parameterCode && (x.DBTMTestParameterListViewSequenceId != dBTMTestParameterListViewSequenceId || dBTMTestParameterListViewSequenceId == 0));
        #endregion
    }
}