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
    public class DBTMOrganisationCentreMasterService : BaseService, IDBTMOrganisationCentreMasterService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<DBTMTestMaster> _dBTMTestMasterRepository;
        private readonly ICoditechRepository<DBTMTestParameterListViewSequence> _dBTMActivityListViewSequenceMasterRepository;
        private readonly ICoditechRepository<DBTMCentrewiseTestParameterListView> _dDBTMCentrewiseTestParameterListViewMasterRepository;
        public DBTMOrganisationCentreMasterService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _dBTMTestMasterRepository = new CoditechRepository<DBTMTestMaster>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dBTMActivityListViewSequenceMasterRepository = new CoditechRepository<DBTMTestParameterListViewSequence>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _dDBTMCentrewiseTestParameterListViewMasterRepository = new CoditechRepository<DBTMCentrewiseTestParameterListView>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        //Get GetActivityListViewSequence by dBTMOrganisationCentreMasterId.
        public virtual DBTMActivityListViewSequenceListModel GetActivityListViewSequenceList(int dBTMOrganisationCentreMasterId, string centreCode, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            if (string.IsNullOrWhiteSpace(centreCode) && dBTMOrganisationCentreMasterId > 0)
            {
                centreCode = GetOrganisationCentreCodeByOrganisationCentreMasterId(dBTMOrganisationCentreMasterId);
            }

            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMActivityListViewSequenceModel> objStoredProc = new CoditechViewRepository<DBTMActivityListViewSequenceModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@CentreCode", centreCode, ParameterDirection.Input, DbType.String);
            List<DBTMActivityListViewSequenceModel> activityList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMCentrewiseActivityListViewSequenceList @CentreCode")?.ToList();
            // Bind List Model
            DBTMActivityListViewSequenceListModel listModel = new DBTMActivityListViewSequenceListModel();
            listModel.DBTMActivityListViewSequenceList = activityList?.Count > 0 ? activityList : new List<DBTMActivityListViewSequenceModel>();
            listModel.TestName = activityList?.FirstOrDefault()?.TestName;
            listModel.CentreCode = centreCode;
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }

        public virtual DBTMCentrewiseTestParameterListViewModel GetDBTMCentrewiseTestParameterListView(int dBTMOrganisationCentreParameterListViewSequenceId, string centreCode)
        {
            DBTMCentrewiseTestParameterListView centrewiseEntity = _dDBTMCentrewiseTestParameterListViewMasterRepository.Table.Where(x => x.DBTMTestParameterListViewSequenceId == dBTMOrganisationCentreParameterListViewSequenceId && x.CentreCode == centreCode).FirstOrDefault();
            if (centrewiseEntity != null)
            {
                return centrewiseEntity.FromEntityToModel<DBTMCentrewiseTestParameterListViewModel>();
            }
            DBTMTestParameterListViewSequence sequenceEntity = _dBTMActivityListViewSequenceMasterRepository.Table.Where(x => x.DBTMTestParameterListViewSequenceId == dBTMOrganisationCentreParameterListViewSequenceId)?.FirstOrDefault();
            if (sequenceEntity == null)
                return null;
            DBTMCentrewiseTestParameterListViewModel dBTMCentrewiseTestParameterListViewModel = new DBTMCentrewiseTestParameterListViewModel
            {
                DBTMCentrewiseTestParameterListViewId = dBTMOrganisationCentreParameterListViewSequenceId,
                DBTMTestParameterListViewSequenceId = sequenceEntity.DBTMTestParameterListViewSequenceId,
                ColumnName = sequenceEntity.ColumnName,
                DisplayOn = sequenceEntity.DisplayOn,
                IsColumnCellBold = sequenceEntity.IsColumnCellBold ?? false
            };
            return dBTMCentrewiseTestParameterListViewModel;
        }

        //Update DBTM Centrewise Test Parameter List View.
        public virtual DBTMCentrewiseTestParameterListViewModel UpdateDBTMCentrewiseTestParameterListView(DBTMCentrewiseTestParameterListViewModel model)
        {
            if (IsNull(model))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);
            DBTMCentrewiseTestParameterListView existingEntity = _dDBTMCentrewiseTestParameterListViewMasterRepository.Table.FirstOrDefault(x => x.DBTMTestParameterListViewSequenceId == model.DBTMTestParameterListViewSequenceId && x.CentreCode == model.CentreCode);
            if (existingEntity != null)
            {
                existingEntity.DisplayOn = model.DisplayOn;
                existingEntity.IsColumnCellBold = model.IsColumnCellBold;
                existingEntity.ModifiedDate = DateTime.Now;
                _dDBTMCentrewiseTestParameterListViewMasterRepository.Update(existingEntity);
            }
            else
            {
                DBTMCentrewiseTestParameterListView newEntity = model.FromModelToEntity<DBTMCentrewiseTestParameterListView>();
                newEntity.CreatedDate = DateTime.Now;
                newEntity.CentreCode = model.CentreCode;
                _dDBTMCentrewiseTestParameterListViewMasterRepository.Insert(newEntity);
            }
            return model;
        }
        #region Protected Method
        #endregion
    }
}