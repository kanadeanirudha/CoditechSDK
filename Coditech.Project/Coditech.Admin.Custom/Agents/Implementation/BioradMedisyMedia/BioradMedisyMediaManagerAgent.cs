using Coditech.Admin.Utilities;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper;
using Coditech.Common.Logger;
using Coditech.Resources;
using System.Diagnostics;
using static Coditech.Common.Helper.HelperUtility;

namespace Coditech.Admin.Agents
{
    public class BioradMedisyMediaManagerAgent : MediaManagerFolderAgent, IBioradMedisyMediaManagerAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IBioradMedisyMediaManagerClient _bioradMedisyMediaManagerClient;
        #endregion

        #region Public Constructor
        public BioradMedisyMediaManagerAgent(ICoditechLogging coditechLogging, IBioradMedisyMediaManagerClient mediaSettingMasterClient, IMediaManagerClient mediaManagerClient) : base(coditechLogging, mediaManagerClient)
        {
            _coditechLogging = coditechLogging;
            _bioradMedisyMediaManagerClient = GetClient<IBioradMedisyMediaManagerClient>(mediaSettingMasterClient);
        }
        #endregion

        #region Public Methods
        public new BioradMedisyMediaModel GetMediaDetails(long mediaId)
        {
            UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
            BioradMedisyMediaResponse response = _bioradMedisyMediaManagerClient.GetMediaDetails(mediaId, userModel.EntityId);
            return response?.MediaModel;
        }

        //UpdateFileApprovalFlow
        public BioradMedisyMediaModel UpdateFileApprovalFlow(BioradMedisyMediaModel bioradMedisyMediaModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "BioradMedisyMediaManager", TraceLevel.Info);
                BioradMedisyMediaResponse response = _bioradMedisyMediaManagerClient.UpdateFileApprovalFlow(bioradMedisyMediaModel);
                bioradMedisyMediaModel = response?.MediaModel;
                _coditechLogging.LogMessage("Agent method execution done.", "BioradMedisyMediaManager", TraceLevel.Info);
                return IsNotNull(bioradMedisyMediaModel) ? bioradMedisyMediaModel : new BioradMedisyMediaModel();
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "BioradMedisyMediaManager", TraceLevel.Error);
                bioradMedisyMediaModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
                return bioradMedisyMediaModel;
            }
        }
        #endregion

        #region protected
        protected override List<DatatableColumns> BindColumns()
        {
            List<DatatableColumns> datatableColumnList = base.BindColumns();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Status",
                ColumnCode = "Status",
                IsSortable = false,
            });
            return datatableColumnList;
        }
        #endregion
    }
}
