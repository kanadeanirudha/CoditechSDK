using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Resources;
using System.Diagnostics;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.Admin.Agents
{
    public class DBTMTrainerAgent : GeneralTrainerAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IGeneralTrainerClient _generalTrainerClient;
        private readonly IUserClient _userClient;
        #endregion

        #region Public Constructor
        public DBTMTrainerAgent(ICoditechLogging coditechLogging, IGeneralTrainerClient generalTrainerClient, IUserClient userClient) : base(coditechLogging, generalTrainerClient, userClient)
        {
            _coditechLogging = coditechLogging;
            _generalTrainerClient = GetClient<IGeneralTrainerClient>(generalTrainerClient);
            _userClient = userClient;
        }
        #endregion

        #region Public Methods
        public override GeneralTrainerListViewModel GetTrainerList(string selectedCentreCode, short selectedDepartmentId, bool isAssociated, DataTableViewModel dataTableModel)
        {
            FilterCollection filters = new FilterCollection();
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters.Add("FirstName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("LastName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("MobileNumber", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("EmailId", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("PersonCode", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("UniqueCode", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }
            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);
            GeneralTrainerListResponse response = _generalTrainerClient.List(selectedCentreCode, selectedDepartmentId, true, null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            GeneralTrainerListModel trainerList = new GeneralTrainerListModel { GeneralTrainerList = response?.GeneralTrainerList };
            GeneralTrainerListViewModel listViewModel = new GeneralTrainerListViewModel();
            listViewModel.GeneralTrainerList = trainerList?.GeneralTrainerList?.ToViewModel<GeneralTrainerViewModel>().ToList();
            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.GeneralTrainerList.Count, BindColumns());
            return listViewModel;
        }
        #endregion
        #region Private Methods
        protected override List<DatatableColumns> BindColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Image",
                ColumnCode = "Image",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Trainer Code",
                ColumnCode = "PersonCode",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "First Name",
                ColumnCode = "FirstName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Last Name",
                ColumnCode = "LastName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Contact",
                ColumnCode = "MobileNumber",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Email Id",
                ColumnCode = "EmailId",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Trainer Specialization",
                ColumnCode = "TrainerSpecializationEnumId",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Number Of Trainee Associated",
                ColumnCode = "NumberOfTraineeAssociated",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Unique Code",
                ColumnCode = "UniqueCode",
                IsSortable = true,
            });
            return datatableColumnList;
        }
        #endregion
    }
}
