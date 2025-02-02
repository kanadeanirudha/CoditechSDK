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
            DBTMDeviceListResponse response = new DBTMDeviceClient().List(null, null, null, 1, int.MaxValue);
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

            GeneralTrainerListModel list = new GeneralTrainerListModel();
            if (!string.IsNullOrEmpty(dropdownViewModel.Parameter))
            {
                string centreCode = SpiltCentreCode(dropdownViewModel.Parameter);
                GeneralTrainerListResponse response = new DBTMTraineeAssignmentClient().GetTrainerByCentreCode(centreCode);
                list = new GeneralTrainerListModel { GeneralTrainerList = response?.GeneralTrainerList };
            }
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
                long generalTrainerId = Convert.ToInt16(dropdownViewModel.Parameter.Split("~")[1]);
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
    }
}






