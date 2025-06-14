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
        #endregion
    }
}
