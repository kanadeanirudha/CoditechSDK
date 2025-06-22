using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Helper.Utilities;
using Coditech.Resources;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Coditech.Admin.Helpers
{
    public static class CoditechCustomDropdownHelper
    {
        public static List<UserAccessibleCentreModel> AccessibleCentreList()
        {
            return SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession)?.AccessibleCentreList;
        }

        public static DropdownViewModel GeneralDropdownList(DropdownViewModel dropdownViewModel)
        {
            List<SelectListItem> dropdownList = new List<SelectListItem>();

            if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.DBTMActivityCategory.ToString()))
            {
                GetDBTMActivityCategoryList(dropdownViewModel, dropdownList);
            }
            else if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.DBTMDeviceRegistrationDetails.ToString()))
            {
                GetDBTMDeviceRegistrationDetailsList(dropdownViewModel, dropdownList);
            }
            else if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.CentrewiseDBTMTrainer.ToString()))
            {
                GetCentrewiseDBTMTrainerList(dropdownViewModel, dropdownList);
            }
            else if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.TraineeDetailsListByDBTMTrainer.ToString()))
            {
                GetTraineeDetailByCentreCodeAndGeneralTrainerList(dropdownViewModel, dropdownList);
            }
            else if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.DBTMTest.ToString()))
            {
                GetDBTMTestList(dropdownViewModel, dropdownList);
            }
            else if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.DBTMBatchActivity.ToString()))
            {
                GetDBTMBatchActivityList(dropdownViewModel, dropdownList);
            }
            else if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.DBTMDeviceSerialCodeByCentreCode.ToString()))
            {
                GetCentrewiseDeviceSerialCodeList(dropdownViewModel, dropdownList);
            }
            else if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.BatchWiseReports.ToString()))
            {
                GetBatchWiseReportsList(dropdownViewModel, dropdownList);
            }
            else if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.DBTMTraineeList.ToString()))
            {
                GetTraineeDetailsList(dropdownViewModel, dropdownList);
            }
            dropdownViewModel.DropdownList = dropdownList;
            return dropdownViewModel;
        }

        private static void GetDBTMActivityCategoryList(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            FilterCollection filters = new FilterCollection();
            filters.Add(FilterKeys.IsActive, ProcedureFilterOperators.Equals, "1");
            DBTMActivityCategoryListResponse response = new DBTMActivityCategoryClient().List(null, filters, null, 1, int.MaxValue);
            if (dropdownViewModel.IsRequired)
                dropdownList.Add(new SelectListItem() { Value = "", Text = GeneralResources.SelectLabel });
            else
                dropdownList.Add(new SelectListItem() { Value = "0", Text = GeneralResources.SelectLabel });

            DBTMActivityCategoryListModel list = new DBTMActivityCategoryListModel { DBTMActivityCategoryList = response.DBTMActivityCategoryList };
            foreach (var item in list.DBTMActivityCategoryList)
            {
                if (!string.IsNullOrEmpty(dropdownViewModel.Parameter) && Convert.ToInt16(dropdownViewModel.Parameter) > 0 && item.DBTMActivityCategoryId == Convert.ToInt16(dropdownViewModel.Parameter))
                {
                    continue;
                }
                dropdownList.Add(new SelectListItem()
                {
                    Text = string.Concat(item.ActivityCategoryName, " (", item.ActivityCategoryCode, ")"),
                    Value = Convert.ToString(item.DBTMActivityCategoryId),
                    Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.DBTMActivityCategoryId)
                });
            }
        }

        private static void GetDBTMDeviceRegistrationDetailsList(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            DBTMDeviceListResponse response = new DBTMDeviceClient().List(0, null, null, null, 1, int.MaxValue);
            dropdownList.Add(new SelectListItem() { Text = "-------Select Registration Details-------" });

            DBTMDeviceListModel list = new DBTMDeviceListModel { DBTMDeviceList = response.DBTMDeviceList };
            foreach (var item in list.DBTMDeviceList)
            {
                if (!string.IsNullOrEmpty(dropdownViewModel.Parameter) && Convert.ToInt16(dropdownViewModel.Parameter) > 0 && item.DBTMDeviceMasterId == Convert.ToInt16(dropdownViewModel.Parameter))
                {
                    continue;
                }
                dropdownList.Add(new SelectListItem()
                {
                    Text = string.Concat(item.DeviceName, " (", item.DeviceSerialCode, ")"),
                    Value = Convert.ToString(item.DBTMDeviceMasterId),
                    Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.DBTMDeviceMasterId)
                });
            }
        }

        private static void GetCentrewiseDBTMTrainerList(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
            GeneralTrainerListModel list = new GeneralTrainerListModel();

            if (!string.IsNullOrEmpty(dropdownViewModel.Parameter))
            {
                string centreCode = SpiltCentreCode(dropdownViewModel.Parameter);
                GeneralTrainerListResponse response = new DBTMTraineeAssignmentClient().GetTrainerByCentreCode(centreCode);
                list = new GeneralTrainerListModel { GeneralTrainerList = response?.GeneralTrainerList };

                // Filter the list if the user is a trainer
                if (userModel?.Custom1 == CustomConstants.DBTMTrainer)
                {
                    list.GeneralTrainerList = list.GeneralTrainerList?.Where(x =>
                        string.Equals(x.FirstName, userModel.FirstName, StringComparison.InvariantCultureIgnoreCase) &&
                        string.Equals(x.LastName, userModel.LastName, StringComparison.InvariantCultureIgnoreCase))?.ToList();
                }
            }

            if (!string.IsNullOrEmpty(dropdownViewModel.SelectedText) && userModel?.Custom1 != CustomConstants.DBTMTrainer)
                dropdownList.Add(new SelectListItem() { Text = dropdownViewModel.SelectedText, Value = dropdownViewModel.SelectedValue });
            else
                dropdownList.Add(new SelectListItem() { Text = "-------Select Trainer-------", Value = "" });

            foreach (var item in list?.GeneralTrainerList)
            {
                dropdownList.Add(new SelectListItem()
                {
                    Text = $"{item.FirstName} {item.LastName}",
                    Value = item.GeneralTrainerMasterId.ToString(),
                    Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.GeneralTrainerMasterId)
                });
            }
        }

        private static void GetTraineeDetailByCentreCodeAndGeneralTrainerList(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {

            DBTMTraineeDetailsListModel list = new DBTMTraineeDetailsListModel();
            if (!string.IsNullOrEmpty(dropdownViewModel.Parameter))
            {
                string centreCode = dropdownViewModel.Parameter.Split("~")[0];
                long generalTrainerId = Convert.ToInt64(dropdownViewModel.Parameter.Split("~")[1]);
                DBTMTraineeDetailsListResponse response = new DBTMTraineeAssignmentClient().GetTraineeDetailByCentreCodeAndgeneralTrainerId(centreCode, generalTrainerId);
                list = new DBTMTraineeDetailsListModel { DBTMTraineeDetailsList = response?.DBTMTraineeDetailsList };
            }
            dropdownList.Add(new SelectListItem() { Text = "-------Select Trainee Details-------", Value = "" });
            foreach (var item in list?.DBTMTraineeDetailsList)
            {
                dropdownList.Add(new SelectListItem()
                {
                    Text = $"{item.FirstName} {item.LastName}",
                    Value = item.DBTMTraineeDetailId.ToString(),
                    Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.DBTMTraineeDetailId)
                });
            }
        }

        private static void GetDBTMTestList(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            DBTMTestListResponse response = new DBTMTestClient().List(null, null, null, 1, int.MaxValue);
            dropdownList.Add(new SelectListItem() { Text = "-------Select Test-------" });

            DBTMTestListModel list = new DBTMTestListModel { DBTMTestList = response.DBTMTestList };
            foreach (var item in list.DBTMTestList)
            {
                if (!string.IsNullOrEmpty(dropdownViewModel.Parameter) && Convert.ToInt16(dropdownViewModel.Parameter) > 0 && item.DBTMTestMasterId == Convert.ToInt16(dropdownViewModel.Parameter))
                {
                    continue;
                }
                dropdownList.Add(new SelectListItem()
                {
                    Text = string.Concat(item.TestName, " (", item.TestCode, ")"),
                    Value = Convert.ToString(item.DBTMTestMasterId),
                    Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.DBTMTestMasterId)
                });
            }
        }
        private static void GetDBTMBatchActivityList(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            dropdownList.Add(new SelectListItem() { Text = "-------Select Activity-------" });

            if (!string.IsNullOrEmpty(dropdownViewModel.Parameter))
            {
                int generalBatchMasterId = Convert.ToInt32(dropdownViewModel.Parameter.Split("~")[0]);
                bool isAssociated = Convert.ToBoolean(dropdownViewModel.Parameter.Split("~")[1]);

                DBTMBatchActivityListResponse response = new DBTMBatchActivityClient().GetDBTMBatchActivityList(generalBatchMasterId, isAssociated, null, null, null, 1, int.MaxValue);
                DBTMBatchActivityListModel list = new DBTMBatchActivityListModel() { DBTMBatchActivityList = response.DBTMBatchActivityList };
                foreach (var item in list?.DBTMBatchActivityList)
                {
                    dropdownList.Add(new SelectListItem()
                    {
                        Text = $"{item.TestName}",
                        Value = item.DBTMTestMasterId.ToString(),
                        Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.DBTMTestMasterId)
                    });
                }
            }
        }

        private static string SpiltCentreCode(string centreCode)
        {
            centreCode = !string.IsNullOrEmpty(centreCode) && centreCode.Contains(":") ? centreCode.Split(':')[0] : centreCode;
            return centreCode;
        }

        private static void GetCentrewiseDeviceSerialCodeList(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            if (string.IsNullOrEmpty(dropdownViewModel.Parameter) && AccessibleCentreList()?.Count == 1)
            {
                dropdownViewModel.Parameter = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession).SelectedCentreCode;
            }
            DBTMDeviceRegistrationDetailsListModel list = new DBTMDeviceRegistrationDetailsListModel();
            if (!string.IsNullOrEmpty(dropdownViewModel.Parameter))
            {
                string centreCode = SpiltCentreCode(dropdownViewModel.Parameter);
                DBTMDeviceRegistrationDetailsListResponse response = new DBTMDeviceRegistrationDetailsClient().GetDeviceSerialCodeByCentreCode(centreCode);
                list = new DBTMDeviceRegistrationDetailsListModel { RegistrationDetailsList = response?.RegistrationDetailsList };
            }
            dropdownList.Add(new SelectListItem() { Text = "-------Select Device Serial Code-------", Value = "" });
            foreach (var item in list?.RegistrationDetailsList)
            {
                dropdownList.Add(new SelectListItem()
                {
                    Text = item.DeviceSerialCode,
                    // Value = item.Custom1.ToString(),
                    Value = Convert.ToString(item.Custom1),
                    Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.Custom1)

                });
            }
        }

        private static void GetBatchWiseReportsList(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            dropdownList.Add(new SelectListItem() { Text = "-------Select Batch-------" , Value = "0" });
            long entityId = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession).EntityId;
            string userType = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession).UserType;
            DBTMBatchListResponse response = new DBTMBatchClient().GetBatchList(entityId, userType);
            DBTMBatchListModel list = new DBTMBatchListModel() { DBTMBatchList = response.DBTMBatchList };
            foreach (var item in list?.DBTMBatchList)
            {
                dropdownList.Add(new SelectListItem()
                {
                    Text = $"{item.BatchName}",
                    Value = item.GeneralBatchMasterId.ToString(),
                    Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.GeneralBatchMasterId)
                });
            }
        }

        private static void GetTraineeDetailsList(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
            string centreCode = userModel.SelectedCentreCode;
            long entityId = 0;
            if (userModel?.Custom1 == CustomConstants.DBTMTrainer)
            {
                entityId = userModel.EntityId;
            }
            DBTMTraineeDetailsListResponse response = new DBTMTraineeDetailsClient().List(centreCode, entityId, null, null, null, 1, int.MaxValue);
            DBTMTraineeDetailsListModel list = new DBTMTraineeDetailsListModel { DBTMTraineeDetailsList = response?.DBTMTraineeDetailsList };
            dropdownList.Add(new SelectListItem() { Text = "All", Value = "0" });
            if (userModel?.Custom1 == CustomConstants.DBTMTrainer)
            {
                list.DBTMTraineeDetailsList = list.DBTMTraineeDetailsList?.Where(x =>
                    string.Equals(x.FirstName, userModel.FirstName, StringComparison.InvariantCultureIgnoreCase) &&
                    string.Equals(x.LastName, userModel.LastName, StringComparison.InvariantCultureIgnoreCase))?.ToList();
            }

            foreach (var item in list?.DBTMTraineeDetailsList)
            {
                dropdownList.Add(new SelectListItem()
                {
                    Text = $"{item.FirstName} {item.LastName}",
                    Value = item.DBTMTraineeDetailId.ToString(),
                    Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.DBTMTraineeDetailId)
                });
            }
        }
    }
}
