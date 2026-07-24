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
    public class DBTMPrintQRService : BaseService, IDBTMPrintQRService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<GeneralPerson> _generalPersonRepository;
        private readonly ICoditechRepository<DBTMTraineeDetails> _dBTMTraineeDetailsRepository;
        public DBTMPrintQRService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _generalPersonRepository = new CoditechRepository<GeneralPerson>(_serviceProvider.GetService<Coditech_Entities>());
            _dBTMTraineeDetailsRepository = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        //DBTMPrintQR.
        public virtual DBTMPrintQRListModel GetDBTMPrintQR(ParameterModel parameterModel)
        {        
            if (parameterModel == null || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);
            List<long> personIds = parameterModel.Ids.Split(',').Select(x => Convert.ToInt64(x)).ToList();
            List<GeneralPerson> persons = _generalPersonRepository.Table.Where(x => personIds.Contains(x.PersonId)).ToList();
            List<DBTMTraineeDetails> traineeDetails = _dBTMTraineeDetailsRepository.Table.Where(x => personIds.Contains(x.PersonId)).ToList();
            DBTMPrintQRListModel listModel = new DBTMPrintQRListModel();
            listModel.DBTMPrintQRList = new List<DBTMPrintQRModel>();
            foreach (GeneralPerson person in persons)
            {
                DBTMTraineeDetails trainee = traineeDetails.FirstOrDefault(x => x.PersonId == person.PersonId);
                string personCode = trainee?.PersonCode;
                listModel.DBTMPrintQRList.Add(new DBTMPrintQRModel
                {
                    PersonId = person.PersonId,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    PersonCode = personCode,
                    QRCode = DBTMCustomHelper.GenerateQRCode(personCode, string.Empty)
                });
            }
            return listModel;
        }


        public virtual DBTMPrintQRListModel GetDBTMPrintQRTraineeList(int generalBatchMasterId, string userType, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMPrintQRModel> objStoredProc = new CoditechViewRepository<DBTMPrintQRModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@GeneralBatchMasterId", generalBatchMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@UserType", userType, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@WhereClause", pageListModel.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMPrintQRModel> list = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMPrintQRTraineeList @GeneralBatchMasterId,@UserType,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 6, out pageListModel.TotalRowCount)?.ToList();
            DBTMPrintQRListModel model = new DBTMPrintQRListModel();
            model.DBTMPrintQRList = list ?? new List<DBTMPrintQRModel>();
            model.BindPageListModel(pageListModel);
            return model;
        }
    }
}
