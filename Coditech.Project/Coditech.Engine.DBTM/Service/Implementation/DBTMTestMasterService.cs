using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Resources;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Specialized;
using System.Data;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMTestMasterService : IDBTMTestMasterService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMTestMaster> _dBTMTestMasterRepository;
        private readonly ICoditechRepository<DBTMTestParameter> _dBTMTestParameterRepository;
        private readonly ICoditechRepository<DBTMParametersAssociatedToTest> _dBTMParametersAssociatedToTestRepository;
        private readonly ICoditechRepository<DBTMTestCalculation> _dBTMTestCalculationRepository;
        private readonly ICoditechRepository<DBTMCalculationAssociatedToTest> _dBTMCalculationAssociatedToTestRepository;
        public DBTMTestMasterService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMTestMasterRepository = new CoditechRepository<DBTMTestMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestParameterRepository = new CoditechRepository<DBTMTestParameter>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMParametersAssociatedToTestRepository = new CoditechRepository<DBTMParametersAssociatedToTest>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMTestCalculationRepository = new CoditechRepository<DBTMTestCalculation>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMCalculationAssociatedToTestRepository = new CoditechRepository<DBTMCalculationAssociatedToTest>(_serviceProvider.GetService<CoditechCustom_Entities>());
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
        //public virtual DBTMTestModel CreateDBTMTest(DBTMTestModel dBTMTestModel)
        //{
        //    if (IsNull(dBTMTestModel))
        //        throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

        //    if (IsDBTMTestNameAlreadyExist(dBTMTestModel.TestCode, dBTMTestModel.DBTMTestMasterId))
        //        throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "Test Code"));


        //    DBTMTestMaster dBTMTestMaster = dBTMTestModel.FromModelToEntity<DBTMTestMaster>();

        //    //Create new DBTMTest and return it.
        //    DBTMTestMaster dBTMTestData = _dBTMTestMasterRepository.Insert(dBTMTestMaster);
        //    if (dBTMTestData?.DBTMTestMasterId > 0)
        //    {
        //        dBTMTestModel.DBTMTestMasterId = dBTMTestData.DBTMTestMasterId;
        //        List<DBTMParametersAssociatedToTest> parametersAssociatedToTestlist = new List<DBTMParametersAssociatedToTest>();
        //        foreach (string item in dBTMTestModel.DBTMSelectedTestParameter)
        //        {
        //            parametersAssociatedToTestlist.Add(new DBTMParametersAssociatedToTest()
        //            {
        //                DBTMTestMasterId = dBTMTestModel.DBTMTestMasterId,
        //                DBTMTestParameterId = Convert.ToByte(item)
        //            });
        //        }

        //        _dBTMParametersAssociatedToTestRepository.InsertAsync(parametersAssociatedToTestlist);

        //        List<DBTMCalculationAssociatedToTest> calculationAssociatedToTestlist = new List<DBTMCalculationAssociatedToTest>();
        //        foreach (string item in dBTMTestModel.DBTMSelectedTestCalculation)
        //        {
        //            calculationAssociatedToTestlist.Add(new DBTMCalculationAssociatedToTest()
        //            {
        //                DBTMTestMasterId = dBTMTestModel.DBTMTestMasterId,
        //                DBTMTestCalculationId = Convert.ToByte(item)
        //            });
        //        }

        //        _dBTMCalculationAssociatedToTestRepository.InsertAsync(calculationAssociatedToTestlist);
        //    }

        //    else
        //    {
        //        dBTMTestModel.HasError = true;
        //        dBTMTestModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
        //    }
        //    return dBTMTestModel;
        //}

        //Create DBTMTest.
        public virtual DBTMTestModel CreateDBTMTest(DBTMTestModel dBTMTestModel)
        {
            if (dBTMTestModel == null)
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (IsDBTMTestNameAlreadyExist(dBTMTestModel.TestCode, dBTMTestModel.DBTMTestMasterId))
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "Test Code"));

            DBTMTestMaster dBTMTestMaster = dBTMTestModel.FromModelToEntity<DBTMTestMaster>();

            //Create new DBTMTest and return it.
            DBTMTestMaster dBTMTestData = _dBTMTestMasterRepository.Insert(dBTMTestMaster);

            if (dBTMTestData?.DBTMTestMasterId > 0)
            {
                dBTMTestModel.DBTMTestMasterId = dBTMTestData.DBTMTestMasterId;

                if (dBTMTestModel.DBTMSelectedTestParameter?.Count > 0)
                {
                    var parametersList = dBTMTestModel.DBTMSelectedTestParameter
                        .Select(item => new DBTMParametersAssociatedToTest
                        {
                            DBTMTestMasterId = dBTMTestModel.DBTMTestMasterId,
                            DBTMTestParameterId = Convert.ToByte(item)
                        }).ToList();

                    _dBTMParametersAssociatedToTestRepository.Insert(parametersList);
                }

                if (dBTMTestModel.DBTMSelectedTestCalculation?.Count > 0)
                {
                    var calculationsList = dBTMTestModel.DBTMSelectedTestCalculation
                        .Select(item => new DBTMCalculationAssociatedToTest
                        {
                            DBTMTestMasterId = dBTMTestModel.DBTMTestMasterId,
                            DBTMTestCalculationId = Convert.ToByte(item)
                        }).ToList();

                    _dBTMCalculationAssociatedToTestRepository.Insert(calculationsList);
                }
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
            }
            return dBTMTestModel;
        }

        //Update DBTMTest.
        //public virtual bool UpdateDBTMTest(DBTMTestModel dBTMTestModel)
        //{
        //    if (IsNull(dBTMTestModel))
        //        throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

        //    if (dBTMTestModel.DBTMTestMasterId < 1)
        //        throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTestMasterID"));

        //    DBTMTestMaster dBTMTestMaster = dBTMTestModel.FromModelToEntity<DBTMTestMaster>();

        //    //Update DBTMTest
        //    bool isdBTMTestUpdated = _dBTMTestMasterRepository.Update(dBTMTestMaster);
        //    if (isdBTMTestUpdated)
        //    {
        //        List<DBTMParametersAssociatedToTest> deleteDBTMParametersAssociatedToTest = null;
        //        List<DBTMParametersAssociatedToTest> insertDBTMParametersAssociatedToTest = null;
        //        List<DBTMParametersAssociatedToTest> parametersAssociatedToTestList = _dBTMParametersAssociatedToTestRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestModel.DBTMTestMasterId)?.ToList();

        //        foreach (string item in dBTMTestModel.DBTMSelectedTestParameter)
        //        {
        //            if (!parametersAssociatedToTestList.Any(x => x.DBTMTestParameterId.ToString() == item))
        //            {
        //                if (IsNull(insertDBTMParametersAssociatedToTest))
        //                {
        //                    insertDBTMParametersAssociatedToTest = new List<DBTMParametersAssociatedToTest>();
        //                }
        //                insertDBTMParametersAssociatedToTest.Add(new DBTMParametersAssociatedToTest()
        //                {
        //                    DBTMTestMasterId = dBTMTestModel.DBTMTestMasterId,
        //                    DBTMTestParameterId = Convert.ToByte(item)
        //                });
        //            }
        //        }

        //        foreach (DBTMParametersAssociatedToTest item in parametersAssociatedToTestList)
        //        {
        //            if (!dBTMTestModel.DBTMSelectedTestParameter.Any(x => x == item.DBTMTestParameterId.ToString()))
        //            {
        //                if (IsNull(deleteDBTMParametersAssociatedToTest))
        //                {
        //                    deleteDBTMParametersAssociatedToTest = new List<DBTMParametersAssociatedToTest>();
        //                }
        //                deleteDBTMParametersAssociatedToTest.Add(item);
        //            }
        //        }

        //        if (insertDBTMParametersAssociatedToTest?.Count > 0)
        //        {
        //            _dBTMParametersAssociatedToTestRepository.Insert(insertDBTMParametersAssociatedToTest);
        //        }

        //        if (deleteDBTMParametersAssociatedToTest?.Count > 0)
        //        {
        //            _dBTMParametersAssociatedToTestRepository.Delete(deleteDBTMParametersAssociatedToTest);
        //        }
        //    }


        //    List<DBTMCalculationAssociatedToTest> deleteDBTMCalculationAssociatedToTest = null;
        //    List<DBTMCalculationAssociatedToTest> insertDBTMCalculationAssociatedToTest = null;
        //    List<DBTMCalculationAssociatedToTest> calculationAssociatedToTestList = _dBTMCalculationAssociatedToTestRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestModel.DBTMTestMasterId)?.ToList();

        //    foreach (string item in dBTMTestModel.DBTMSelectedTestCalculation)
        //    {
        //        if (!calculationAssociatedToTestList.Any(x => x.DBTMTestCalculationId.ToString() == item))
        //        {
        //            if (IsNull(insertDBTMCalculationAssociatedToTest))
        //            {
        //                insertDBTMCalculationAssociatedToTest = new List<DBTMCalculationAssociatedToTest>();
        //            }
        //            insertDBTMCalculationAssociatedToTest.Add(new DBTMCalculationAssociatedToTest()
        //            {
        //                DBTMTestMasterId = dBTMTestModel.DBTMTestMasterId,
        //                DBTMTestCalculationId = Convert.ToByte(item)
        //            });
        //        }
        //    }

        //    foreach (DBTMCalculationAssociatedToTest item in calculationAssociatedToTestList)
        //    {
        //        if (!dBTMTestModel.DBTMSelectedTestParameter.Any(x => x == item.DBTMTestCalculationId.ToString()))
        //        {
        //            if (IsNull(deleteDBTMCalculationAssociatedToTest))
        //            {
        //                deleteDBTMCalculationAssociatedToTest = new List<DBTMCalculationAssociatedToTest>();
        //            }
        //            deleteDBTMCalculationAssociatedToTest.Add(item);
        //        }
        //    }

        //    if (insertDBTMCalculationAssociatedToTest?.Count > 0)
        //    {
        //        _dBTMParametersAssociatedToTestRepository.Insert(insertDBTMCalculationAssociatedToTest);
        //    }

        //    if (deleteDBTMCalculationAssociatedToTest?.Count > 0)
        //    {
        //        _dBTMParametersAssociatedToTestRepository.Delete(deleteDBTMCalculationAssociatedToTest);
        //    }
        //}



        //  else
        //    {
        //        dBTMTestModel.HasError = true;
        //        dBTMTestModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
        //    }
        //    return isdBTMTestUpdated;
        //}

        //Update DBTMTest.
        public virtual bool UpdateDBTMTest(DBTMTestModel dBTMTestModel)
        {
            if (IsNull(dBTMTestModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (dBTMTestModel.DBTMTestMasterId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTestMasterID"));

            DBTMTestMaster dBTMTestMaster = dBTMTestModel.FromModelToEntity<DBTMTestMaster>();

            // Update DBTMTest
            bool isdBTMTestUpdated = _dBTMTestMasterRepository.Update(dBTMTestMaster);
            if (isdBTMTestUpdated)
            {
                List<DBTMParametersAssociatedToTest> parametersAssociatedToTestList = _dBTMParametersAssociatedToTestRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestModel.DBTMTestMasterId)?.ToList();
                List<DBTMParametersAssociatedToTest> insertDBTMParametersAssociatedToTest = new List<DBTMParametersAssociatedToTest>();
                List<DBTMParametersAssociatedToTest> deleteDBTMParametersAssociatedToTest = new List<DBTMParametersAssociatedToTest>();

                foreach (string item in dBTMTestModel.DBTMSelectedTestParameter)
                {
                    if (!parametersAssociatedToTestList.Any(x => x.DBTMTestParameterId.ToString() == item))
                    {
                        insertDBTMParametersAssociatedToTest.Add(new DBTMParametersAssociatedToTest()
                        {
                            DBTMTestMasterId = dBTMTestModel.DBTMTestMasterId,
                            DBTMTestParameterId = Convert.ToByte(item)
                        });
                    }
                }
                foreach (DBTMParametersAssociatedToTest item in parametersAssociatedToTestList)
                {
                    if (!dBTMTestModel.DBTMSelectedTestParameter.Any(x => x == item.DBTMTestParameterId.ToString()))
                    {
                        deleteDBTMParametersAssociatedToTest.Add(item);
                    }
                }

                // Insert new parameters
                if (insertDBTMParametersAssociatedToTest.Count > 0)
                {
                    _dBTMParametersAssociatedToTestRepository.Insert(insertDBTMParametersAssociatedToTest);
                }

                if (deleteDBTMParametersAssociatedToTest.Count > 0)
                {
                    _dBTMParametersAssociatedToTestRepository.Delete(deleteDBTMParametersAssociatedToTest);
                }

                List<DBTMCalculationAssociatedToTest> calculationAssociatedToTestList = _dBTMCalculationAssociatedToTestRepository.Table.Where(x => x.DBTMTestMasterId == dBTMTestModel.DBTMTestMasterId)?.ToList();
                List<DBTMCalculationAssociatedToTest> insertDBTMCalculationAssociatedToTest = new List<DBTMCalculationAssociatedToTest>();
                List<DBTMCalculationAssociatedToTest> deleteDBTMCalculationAssociatedToTest = new List<DBTMCalculationAssociatedToTest>();

                foreach (string item in dBTMTestModel.DBTMSelectedTestCalculation)
                {
                    if (!calculationAssociatedToTestList.Any(x => x.DBTMTestCalculationId.ToString() == item))
                    {
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
                        deleteDBTMCalculationAssociatedToTest.Add(item);
                    }
                }

                // Insert new calculations
                if (insertDBTMCalculationAssociatedToTest.Count > 0)
                {
                    _dBTMCalculationAssociatedToTestRepository.Insert(insertDBTMCalculationAssociatedToTest);
                }

                // Delete old calculations
                if (deleteDBTMCalculationAssociatedToTest.Count > 0)
                {
                    _dBTMCalculationAssociatedToTestRepository.Delete(deleteDBTMCalculationAssociatedToTest);
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

        #region Protected Method
        // Check if Test Name is already present or not.
        protected virtual bool IsDBTMTestNameAlreadyExist(string testCode, int dBTMTestMasterId = 0)
            => _dBTMTestMasterRepository.Table.Any(x => x.TestCode == testCode && (x.DBTMTestMasterId != dBTMTestMasterId || dBTMTestMasterId == 0));
        #endregion
    }
}
