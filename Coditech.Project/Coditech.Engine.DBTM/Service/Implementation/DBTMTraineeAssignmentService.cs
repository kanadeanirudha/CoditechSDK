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
    public class DBTMTraineeAssignmentService : BaseService, IDBTMTraineeAssignmentService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMTraineeAssignment> _dBTMTraineeAssignmentRepository;
        private readonly ICoditechRepository<GeneralTrainerMaster> _generalTrainerRepository;
        private readonly ICoditechRepository<EmployeeMaster> _employeeMasterRepository;
        private readonly ICoditechRepository<GeneralPerson> _generalPersonRepository;
        private readonly ICoditechRepository<DBTMTraineeDetails> _dBTMTraineeDetailsRepository;
        private readonly ICoditechRepository<GeneralTraineeAssociatedToTrainer> _generalTraineeAssociatedToTrainerRepository;

        public DBTMTraineeAssignmentService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMTraineeAssignmentRepository = new CoditechRepository<DBTMTraineeAssignment>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _generalTrainerRepository = new CoditechRepository<GeneralTrainerMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _employeeMasterRepository = new CoditechRepository<EmployeeMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _generalPersonRepository = new CoditechRepository<GeneralPerson>(_serviceProvider.GetService<Coditech_Entities>());
            _dBTMTraineeDetailsRepository = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _generalTraineeAssociatedToTrainerRepository = new CoditechRepository<GeneralTraineeAssociatedToTrainer>(_serviceProvider.GetService<Coditech_Entities>());
        }

        public virtual DBTMTraineeAssignmentListModel GetDBTMTraineeAssignmentList(long generalTrainerMasterId,FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {

            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMTraineeAssignmentModel> objStoredProc = new CoditechViewRepository<DBTMTraineeAssignmentModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@GeneralTrainerMasterId", generalTrainerMasterId, ParameterDirection.Input, DbType.Int64);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMTraineeAssignmentModel> dBTMTraineeAssignmentList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMTraineeAssignmentList @GeneralTrainerMasterId,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 5, out pageListModel.TotalRowCount)?.ToList();
            DBTMTraineeAssignmentListModel listModel = new DBTMTraineeAssignmentListModel();

            listModel.DBTMTraineeAssignmentList = dBTMTraineeAssignmentList?.Count > 0 ? dBTMTraineeAssignmentList : new List<DBTMTraineeAssignmentModel>();
            listModel.BindPageListModel(pageListModel);
            listModel.GeneralTrainerMasterId = generalTrainerMasterId;
            return listModel;
        }

        //Create DBTMTraineeAssignment
        public virtual DBTMTraineeAssignmentModel CreateDBTMTraineeAssignment(DBTMTraineeAssignmentModel dBTMTraineeAssignmentModel)
        {
            if (IsNull(dBTMTraineeAssignmentModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            DBTMTraineeAssignment dBTMTraineeAssignment = dBTMTraineeAssignmentModel.FromModelToEntity<DBTMTraineeAssignment>();

            //Create new DBTMTraineeAssignment and return it.
            DBTMTraineeAssignment dBTMTraineeAssignmentData = _dBTMTraineeAssignmentRepository.Insert(dBTMTraineeAssignment);
            if (dBTMTraineeAssignmentData?.DBTMTraineeAssignmentId > 0)
            {
                dBTMTraineeAssignmentModel.DBTMTraineeAssignmentId = dBTMTraineeAssignmentData.DBTMTraineeAssignmentId;
            }
            else
            {
                dBTMTraineeAssignmentModel.HasError = true;
                dBTMTraineeAssignmentModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return dBTMTraineeAssignmentModel;
        }

        //Get DBTMTraineeAssignment by dBTMTraineeAssignment id.
        public virtual DBTMTraineeAssignmentModel GetDBTMTraineeAssignment(long dBTMTraineeAssignmentId)
        {
            if (dBTMTraineeAssignmentId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTraineeAssignmentId"));

            //Get the DBTMTraineeAssignment Details based on id.
            DBTMTraineeAssignment dBTMTraineeAssignment = _dBTMTraineeAssignmentRepository.Table.Where(x => x.DBTMTraineeAssignmentId == dBTMTraineeAssignmentId)?.FirstOrDefault();
            DBTMTraineeAssignmentModel dBTMTraineeAssignmentModel = dBTMTraineeAssignment?.FromEntityToModel<DBTMTraineeAssignmentModel>();
            if (dBTMTraineeAssignmentModel?.GeneralTrainerMasterId > 0)
            {
                long employeeId = _generalTrainerRepository.Table.Where(x => x.GeneralTrainerMasterId == dBTMTraineeAssignmentModel.GeneralTrainerMasterId).Select(y => y.EmployeeId).FirstOrDefault();
                GeneralPersonModel generalTrainerDetails = GetGeneralPersonDetailsByEntityType(employeeId, UserTypeEnum.Employee.ToString());
                if (IsNotNull(generalTrainerDetails))
                {
                    dBTMTraineeAssignmentModel.SelectedCentreCode = generalTrainerDetails.SelectedCentreCode;
                }
            }
            return dBTMTraineeAssignmentModel;
        }

        //Update DBTMTraineeAssignment.
        public virtual bool UpdateDBTMTraineeAssignment(DBTMTraineeAssignmentModel dBTMTraineeAssignmentModel)
        {
            if (IsNull(dBTMTraineeAssignmentModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (dBTMTraineeAssignmentModel.DBTMTraineeAssignmentId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMTraineeAssignmentID"));

            DBTMTraineeAssignment dBTMTraineeAssignment = dBTMTraineeAssignmentModel.FromModelToEntity<DBTMTraineeAssignment>();

            //Update DBTMTraineeAssignment
            bool isdBTMTraineeAssignmentUpdated = _dBTMTraineeAssignmentRepository.Update(dBTMTraineeAssignment);
            if (!isdBTMTraineeAssignmentUpdated)
            {
                dBTMTraineeAssignmentModel.HasError = true;
                dBTMTraineeAssignmentModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isdBTMTraineeAssignmentUpdated;
        }

        //Delete DBTMTraineeAssignment.
        public virtual bool DeleteDBTMTraineeAssignment(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMDeviceID"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("DBTMTraineeAssignmentId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteDBTMTraineeAssignment @DBTMTraineeAssignmentId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        public virtual bool SendAssignmentReminder(string dBTMTraineeAssignmentId)
        {
            return true;
        }

        public virtual GeneralTrainerListModel GetTrainerByCentreCode(string centreCode)
        {
            var list = new GeneralTrainerListModel();

            list.GeneralTrainerList = (from a in _generalTrainerRepository.Table
                                       join b in _employeeMasterRepository.Table
                                        on a.EmployeeId equals b.EmployeeId
                                       join c in _generalPersonRepository.Table
                                       on b.PersonId equals c.PersonId

                                       where (b.CentreCode == centreCode || centreCode == null)

                                       select new GeneralTrainerModel()
                                       {
                                           GeneralTrainerMasterId = a.GeneralTrainerMasterId,
                                           FirstName = c.FirstName,
                                           LastName = c.LastName,
                                       }).ToList();

            return list;
        }

        public virtual DBTMTraineeDetailsListModel GetTraineeDetailByCentreCodeAndgeneralTrainerId(string centreCode, long generalTrainerId)
        {
            DBTMTraineeDetailsListModel listModel = new DBTMTraineeDetailsListModel();

            listModel.DBTMTraineeDetailsList = (from a in _dBTMTraineeDetailsRepository.Table
                                                join b in _generalPersonRepository.Table on a.PersonId equals b.PersonId
                                                join c in _generalTraineeAssociatedToTrainerRepository.Table on a.DBTMTraineeDetailId equals c.EntityId
                                                join d in _generalTrainerRepository.Table on c.GeneralTrainerMasterId equals d.GeneralTrainerMasterId
                                                where (a.CentreCode == centreCode || centreCode == null)
                                                && (c.GeneralTrainerMasterId == generalTrainerId) && a.IsActive
                                                select new DBTMTraineeDetailsModel
                                                {
                                                    DBTMTraineeDetailId = a.DBTMTraineeDetailId,
                                                    FirstName = b.FirstName,
                                                    LastName = b.LastName,
                                                }).ToList();

            return listModel;
        }
        #region Protected Method

        #endregion
    }
}
