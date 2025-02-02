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
    public class DBTMDeviceRegistrationDetailsService : BaseService, IDBTMDeviceRegistrationDetailsService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMDeviceRegistrationDetails> _dBTMDeviceRegistrationDetailsRepository;
        private readonly ICoditechRepository<DBTMDeviceMaster> _dBTMDeviceMasterRepository;
        private readonly ICoditechRepository<DBTMSubscriptionPlan> _dBTMSubscriptionPlanRepository;
        private readonly ICoditechRepository<DBTMSubscriptionPlanAssociatedToUser> _dBTMSubscriptionPlanAssociatedToUserRepository;


        public DBTMDeviceRegistrationDetailsService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMDeviceRegistrationDetailsRepository = new CoditechRepository<DBTMDeviceRegistrationDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMDeviceMasterRepository = new CoditechRepository<DBTMDeviceMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMSubscriptionPlanRepository = new CoditechRepository<DBTMSubscriptionPlan>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMSubscriptionPlanAssociatedToUserRepository = new CoditechRepository<DBTMSubscriptionPlanAssociatedToUser>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        public virtual DBTMDeviceRegistrationDetailsListModel GetDBTMDeviceRegistrationDetailsList(long userMasterId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMDeviceRegistrationDetailsModel> objStoredProc = new CoditechViewRepository<DBTMDeviceRegistrationDetailsModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@UserId", userMasterId, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMDeviceRegistrationDetailsModel> dBTMDeviceRegistrationDetailsList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMDeviceRegistrationDetailsList @UserId,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 5, out pageListModel.TotalRowCount)?.ToList();
            DBTMDeviceRegistrationDetailsListModel listModel = new DBTMDeviceRegistrationDetailsListModel();

            listModel.RegistrationDetailsList = dBTMDeviceRegistrationDetailsList?.Count > 0 ? dBTMDeviceRegistrationDetailsList : new List<DBTMDeviceRegistrationDetailsModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }

        //Create DBTMDeviceRegistrationDetails.
        public virtual DBTMDeviceRegistrationDetailsModel CreateRegistrationDetails(DBTMDeviceRegistrationDetailsModel dBTMDeviceRegistrationDetailsModel)
        {
            if (IsNull(dBTMDeviceRegistrationDetailsModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (string.IsNullOrEmpty(dBTMDeviceRegistrationDetailsModel.DeviceSerialCode))
                throw new CoditechException(ErrorCodes.InvalidData, "Device Serial Code is required.");

            DBTMDeviceMaster dBTMDeviceMaster = new DBTMDeviceMasterService(_coditechLogging, _serviceProvider).GetDBTMDeviceMasterDetailsByCode(dBTMDeviceRegistrationDetailsModel.DeviceSerialCode);
           
            if (dBTMDeviceMaster == null || dBTMDeviceMaster.DBTMDeviceMasterId <= 0)
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format("Invalid Device Serial Code."));

            if (IsDeviceSerialCodeAlreadyExist(dBTMDeviceMaster.DBTMDeviceMasterId))
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "Device Already Added"));

            int subscriptionPlanTypeEnumId = GetEnumIdByEnumCode("DBTMDeviceRegistrationPlan", DropdownCustomTypeEnum.DBTMSubscriptionPlanType.ToString());

            DBTMSubscriptionPlan dBTMSubscriptionPlan = _dBTMSubscriptionPlanRepository.Table.Where(x => x.SubscriptionPlanTypeEnumId == subscriptionPlanTypeEnumId && x.IsActive)?.FirstOrDefault();

            if (IsNull(dBTMSubscriptionPlan))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ErrorMessage_PleaseContactYourAdministrator);

            DBTMDeviceRegistrationDetails dBTMDeviceRegistrationDetails = new DBTMDeviceRegistrationDetails()
            {
                DBTMDeviceMasterId = dBTMDeviceMaster.DBTMDeviceMasterId,
                EntityId = dBTMDeviceRegistrationDetailsModel.EntityId,
                UserType = UserTypeEnum.Employee.ToString(),
                PurchaseDate = DateTime.Now,
                WarrantyExpirationDate = DateTime.Now.AddMonths(dBTMDeviceMaster.WarrantyExpirationPeriodInMonth),
            };

            //Create new DBTMDeviceRegistrationDetails and return it.
            DBTMDeviceRegistrationDetails dBTMDeviceRegistrationDetailsData = _dBTMDeviceRegistrationDetailsRepository.Insert(dBTMDeviceRegistrationDetails);
            if (dBTMDeviceRegistrationDetailsData?.DBTMDeviceRegistrationDetailId > 0)
            {
                dBTMDeviceRegistrationDetailsModel.DBTMDeviceRegistrationDetailId = dBTMDeviceRegistrationDetailsData.DBTMDeviceRegistrationDetailId;

                DBTMSubscriptionPlanAssociatedToUser dBTMSubscriptionPlanAssociatedToUser = new DBTMSubscriptionPlanAssociatedToUser()
                {
                    DBTMSubscriptionPlanId = dBTMSubscriptionPlan.DBTMSubscriptionPlanId,
                    UserType = UserTypeEnum.Employee.ToString(),
                    EntityId = dBTMDeviceRegistrationDetailsModel.EntityId,
                    DBTMDeviceMasterId = dBTMDeviceRegistrationDetails.DBTMDeviceMasterId,
                    DurationInDays = dBTMSubscriptionPlan.DurationInDays,
                    PlanCost = dBTMSubscriptionPlan.PlanCost,
                    PlanDiscount = dBTMSubscriptionPlan.PlanDiscount,
                    IsExpired = false,
                    PlanDurationExpirationDate = DateTime.Now.AddMonths(dBTMDeviceMaster.WarrantyExpirationPeriodInMonth),
                    SalesInvoiceMasterId = 0,
                };

                dBTMSubscriptionPlanAssociatedToUser = _dBTMSubscriptionPlanAssociatedToUserRepository.Insert(dBTMSubscriptionPlanAssociatedToUser);
            }
            else
            {
                dBTMDeviceRegistrationDetailsModel.HasError = true;
                dBTMDeviceRegistrationDetailsModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return dBTMDeviceRegistrationDetailsModel;
        }

        //Get DBTMDeviceRegistrationDetails by dBTMDeviceRegistrationDetailId.
        public virtual DBTMDeviceRegistrationDetailsModel GetRegistrationDetails(long dBTMDeviceRegistrationDetailId)
        {
            if (dBTMDeviceRegistrationDetailId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMDeviceRegistrationDetailId"));

            //Get the DBTMDeviceRegistrationDetails Details based on id.
            DBTMDeviceRegistrationDetails dBTMDeviceRegistrationDetails = _dBTMDeviceRegistrationDetailsRepository.Table.Where(x => x.DBTMDeviceRegistrationDetailId == dBTMDeviceRegistrationDetailId)?.FirstOrDefault();
            DBTMDeviceRegistrationDetailsModel dBTMDeviceRegistrationDetailsModel = dBTMDeviceRegistrationDetails?.FromEntityToModel<DBTMDeviceRegistrationDetailsModel>();
            return dBTMDeviceRegistrationDetailsModel;
        }

        //Update DBTMDeviceRegistrationDetails.
        public virtual bool UpdateRegistrationDetails(DBTMDeviceRegistrationDetailsModel dBTMDeviceRegistrationDetailsModel)
        {
            if (IsNull(dBTMDeviceRegistrationDetailsModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (dBTMDeviceRegistrationDetailsModel.DBTMDeviceRegistrationDetailId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMDeviceRegistrationDetailID"));

            DBTMDeviceRegistrationDetails dBTMDeviceRegistrationDetails = dBTMDeviceRegistrationDetailsModel.FromModelToEntity<DBTMDeviceRegistrationDetails>();

            //Update DBTMDeviceRegistrationDetails
            bool isdBTMDeviceRegistrationDetailsUpdated = _dBTMDeviceRegistrationDetailsRepository.Update(dBTMDeviceRegistrationDetails);
            if (!isdBTMDeviceRegistrationDetailsUpdated)
            {
                dBTMDeviceRegistrationDetailsModel.HasError = true;
                dBTMDeviceRegistrationDetailsModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isdBTMDeviceRegistrationDetailsUpdated;
        }

        //Delete DBTMDeviceRegistrationDetails.
        public virtual bool DeleteRegistrationDetails(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "DBTMDeviceRegistrationDetailId"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("DBTMDeviceRegistrationDetailId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteDBTMDeviceRegistrationDetails @DBTMDeviceRegistrationDetailId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        #region Protected Method
        //Check if DeviceSerialCode is already present or not.
        public virtual bool IsDeviceSerialCodeAlreadyExist(long dBTMDeviceMasterId)
        {
            return _dBTMDeviceRegistrationDetailsRepository.Table.Any(x => x.DBTMDeviceMasterId == dBTMDeviceMasterId);
        }

        #endregion
    }
}
