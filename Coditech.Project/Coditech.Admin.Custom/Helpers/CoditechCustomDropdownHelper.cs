using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Helper.Utilities;
using Coditech.Resources;

using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

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
            else if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.DBTMTrainerCentrewise.ToString()))
            {
                GetDBTMTrainerListByCentreCode(dropdownViewModel, dropdownList);
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
            else if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.BatchWiseMultiReports.ToString()))
            {
                GetDBTMMultiBatchActivityList(dropdownViewModel, dropdownList);
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
            else if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.JoiningCodewiseGeneralTrainer.ToString()))
            {
                GetGeneralTrainerByJoiningCodeList(dropdownViewModel, dropdownList);
            }
            else if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.DBTMGraph.ToString()))
            {
                DBTMGraphByDBTMTestMasterId(dropdownViewModel, dropdownList);
            }
            else if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.GraphType.ToString()))
            {
                DBTMGraphTypeList(dropdownViewModel, dropdownList);
            }
            else if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.DBTMTraineeGraphList.ToString()))
            {
                GetTraineeDetailsGraphList(dropdownViewModel, dropdownList);
            }
            else if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.DBTMPerformanceMatrix.ToString()))
            {
                GetDBTMPerformanceMatrix(dropdownViewModel, dropdownList);
            }
            else if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.DisplayOn.ToString()))
            {
                GetDisplayOn(dropdownViewModel, dropdownList);
            }
            else if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.GraphMode.ToString()))
            {
                GetGraphMode(dropdownViewModel, dropdownList);
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
            foreach (var item in list.DBTMActivityCategoryList.OrderBy(x => x.ActivityCategoryName))
            {
                if (!string.IsNullOrEmpty(dropdownViewModel.Parameter) && Convert.ToInt16(dropdownViewModel.Parameter) > 0 && item.DBTMActivityCategoryId == Convert.ToInt16(dropdownViewModel.Parameter))
                {
                    continue;
                }
                dropdownList.Add(new SelectListItem()
                {
                    Text = item.ActivityCategoryName,
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
            foreach (var item in list.DBTMDeviceList.OrderBy(x => x.DeviceName))
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

            foreach (var item in list?.GeneralTrainerList?.OrderBy(x => x.FirstName))
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
            foreach (var item in list?.DBTMTraineeDetailsList.OrderBy(x => x.FirstName))
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
            if (dropdownViewModel.IsRequired)
                dropdownList.Add(new SelectListItem() { Value = "", Text = GeneralResources.SelectLabel });
            else
                dropdownList.Add(new SelectListItem() { Value = "0", Text = GeneralResources.SelectLabel });
            DBTMTestListModel list = new DBTMTestListModel { DBTMTestList = response.DBTMTestList };
            bool isActive = !string.IsNullOrEmpty(dropdownViewModel.Parameter)
                                && dropdownViewModel.Parameter.Equals("IsActive", StringComparison.OrdinalIgnoreCase);

            if (isActive)
                list.DBTMTestList = list.DBTMTestList.Where(x => x.IsActive).ToList();

            foreach (var item in list?.DBTMTestList.OrderBy(x => x.PerformanceMatrix ?? string.Empty).ThenBy(x => x.TestName ?? string.Empty))
            {
                if (!string.IsNullOrEmpty(dropdownViewModel.Parameter) && short.TryParse(dropdownViewModel.Parameter, out short excludeId) && item.DBTMTestMasterId == excludeId)
                {
                    continue;
                }
                dropdownList.Add(new SelectListItem()
                {
                    Text = $"{item.TestName}",
                    Value = Convert.ToString(item.DBTMTestMasterId),
                    Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.DBTMTestMasterId)
                });
            }
        }
        private static void GetDBTMBatchActivityList(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            if (dropdownViewModel.IsRequired)
                dropdownList.Add(new SelectListItem() { Value = "", Text = GeneralResources.SelectLabel });
            else
                dropdownList.Add(new SelectListItem() { Value = "0", Text = GeneralResources.SelectLabel });

            if (!string.IsNullOrEmpty(dropdownViewModel.Parameter) &&
                dropdownViewModel.Parameter.ToLower() != "0~false")
            {
                int generalBatchMasterId = Convert.ToInt32(dropdownViewModel.Parameter.Split("~")[0]);
                bool isAssociated = Convert.ToBoolean(dropdownViewModel.Parameter.Split("~")[1]);

                DBTMBatchActivityListResponse response = new DBTMBatchActivityClient().GetDBTMBatchActivityList(generalBatchMasterId, isAssociated, null, null, null, 1, int.MaxValue);
                DBTMBatchActivityListModel list = new DBTMBatchActivityListModel() { DBTMBatchActivityList = response.DBTMBatchActivityList };
                foreach (var item in list?.DBTMBatchActivityList.OrderBy(x => x.TestName))
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

        private static void GetDBTMMultiBatchActivityList(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            if (dropdownViewModel.DropdownType == DropdownCustomTypeEnum.BatchWiseMultiReports.ToString())
                if (dropdownViewModel.IsRequired)
                {
                    dropdownList.Add(new SelectListItem { Value = "0", Text = "All" });
                }
                else
                {
                    dropdownList.Add(new SelectListItem { Value = "0", Text = GeneralResources.SelectLabel });
                }

            if (!string.IsNullOrEmpty(dropdownViewModel.Parameter) &&
            dropdownViewModel.Parameter.ToLower() != "0~false")
            {
                int generalBatchMasterId = Convert.ToInt32(dropdownViewModel.Parameter.Split("~")[0]);
                bool isAssociated = Convert.ToBoolean(dropdownViewModel.Parameter.Split("~")[1]);

                DBTMBatchActivityListResponse response = new DBTMBatchActivityClient().GetDBTMBatchActivityList(generalBatchMasterId, isAssociated, null, null, null, 1, int.MaxValue);
                DBTMBatchActivityListModel list = new DBTMBatchActivityListModel() { DBTMBatchActivityList = response.DBTMBatchActivityList };
                foreach (var item in list?.DBTMBatchActivityList.OrderBy(x => x.PerformanceMatrix ?? string.Empty).ThenBy(x => x.TestName ?? string.Empty))
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
            foreach (var item in list?.RegistrationDetailsList.OrderBy(x => x.DeviceSerialCode))
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
            dropdownList.Add(new SelectListItem() { Text = "-------Select Batch-------", Value = "0" });
            long entityId = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession).EntityId;
            string userType = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession).UserType;
            DBTMBatchListResponse response = new DBTMBatchClient().GetBatchList(entityId, userType);
            DBTMBatchListModel list = new DBTMBatchListModel() { DBTMBatchList = response.DBTMBatchList };
            foreach (var item in list?.DBTMBatchList.OrderBy(x => x.BatchName))
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
            DBTMCustomUserModel dBTMCustomUserModel = new DBTMCustomUserModel();
            if (userModel?.Custom1 == CustomConstants.DBTMTrainer)
            {
                dBTMCustomUserModel = JsonConvert.DeserializeObject<DBTMCustomUserModel>(SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession).Custom3);
            }
            DBTMTraineeDetailsListResponse response = new DBTMTraineeDetailsClient().List(centreCode, Convert.ToInt64(dBTMCustomUserModel.GeneralTrainerMasterId), null, null, null, 1, int.MaxValue);
            DBTMTraineeDetailsListModel list = new DBTMTraineeDetailsListModel { DBTMTraineeDetailsList = response?.DBTMTraineeDetailsList };
            if (dropdownViewModel.ExcludedValues == null || !dropdownViewModel.ExcludedValues.Contains("0"))
            {
                dropdownList.Add(new SelectListItem() { Text = "All", Value = "0" });
            }
            //if (userModel?.Custom1 == CustomConstants.DBTMTrainer)
            //{
            //    list.DBTMTraineeDetailsList = list.DBTMTraineeDetailsList?.Where(x =>
            //        string.Equals(x.FirstName, userModel.FirstName, StringComparison.InvariantCultureIgnoreCase) &&
            //        string.Equals(x.LastName, userModel.LastName, StringComparison.InvariantCultureIgnoreCase))?.ToList();
            //}

            foreach (var item in list?.DBTMTraineeDetailsList.OrderBy(x => x.FirstName))
            {
                dropdownList.Add(new SelectListItem()
                {
                    Text = $"{item.FirstName} {item.LastName}",
                    Value = item.DBTMTraineeDetailId.ToString(),
                    Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.DBTMTraineeDetailId)
                });
            }
        }
        private static void GetGeneralTrainerByJoiningCodeList(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            if (dropdownViewModel.IsRequired)
                dropdownList.Add(new SelectListItem() { Value = "", Text = GeneralResources.SelectLabel });
            else
                dropdownList.Add(new SelectListItem() { Value = "0", Text = GeneralResources.SelectLabel });

            if (!string.IsNullOrEmpty(dropdownViewModel.Parameter))
            {
                string joiningCode = (dropdownViewModel.Parameter);
                long generalTrainerMasterId = 0;

                DBTMNewRegistrationListResponse response = new DBTMNewRegistrationClient().GetGeneralTrainerByJoiningCode(joiningCode, generalTrainerMasterId);
                DBTMNewRegistrationListModel list = new DBTMNewRegistrationListModel() { DBTMNewRegistrationList = response.DBTMNewRegistrationList };
                foreach (var item in list?.DBTMNewRegistrationList.OrderBy(x => x.FirstName))
                {
                    dropdownList.Add(new SelectListItem()
                    {
                        Text = string.Concat(item.FirstName, " ", item.LastName, ""),
                        Value = item.GeneralTrainerMasterId.ToString(),
                        Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.GeneralTrainerMasterId)
                    });
                }
            }
        }
        private static void DBTMGraphByDBTMTestMasterId(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            if (dropdownViewModel.IsRequired)
                dropdownList.Add(new SelectListItem() { Value = "", Text = GeneralResources.SelectLabel });
            else
                dropdownList.Add(new SelectListItem() { Value = "0", Text = GeneralResources.SelectLabel });

            if (!string.IsNullOrEmpty(dropdownViewModel.Parameter))
            {
                var parameters = dropdownViewModel.Parameter.Split('|');
                int dBTMTestMasterId = Convert.ToInt32(parameters[0]);
                string graphMode = parameters.Length > 1 ? parameters[1] : string.Empty;

                DBTMGraphMasterListResponse response = new DBTMTestClient().DBTMGraphByDBTMTestMasterId(dBTMTestMasterId, graphMode);
                DBTMGraphMasterListModel list = new DBTMGraphMasterListModel() { DBTMGraphMasterList = response.DBTMGraphMasterList };
                var filteredList = string.IsNullOrEmpty(graphMode)
                    ? list.DBTMGraphMasterList
                    : list.DBTMGraphMasterList.Where(x => x.GraphMode == graphMode).ToList();

                foreach (var item in filteredList.OrderBy(x => x.GraphName))
                {
                    dropdownList.Add(new SelectListItem()
                    {
                        Text = item.GraphName,
                        Value = item.DBTMGraphMasterId.ToString(),
                        Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.DBTMGraphMasterId)
                    });
                }
            }
        }
        private static void GetTraineeDetailsGraphList(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            UserModel userModel = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession);
            string centreCode = userModel.SelectedCentreCode;
            DBTMCustomUserModel dBTMCustomUserModel = new DBTMCustomUserModel();
            if (userModel?.Custom1 == CustomConstants.DBTMTrainer)
            {
                dBTMCustomUserModel = JsonConvert.DeserializeObject<DBTMCustomUserModel>(SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession).Custom3);
            }
            DBTMTraineeDetailsListResponse response = new DBTMTraineeDetailsClient().List(centreCode, Convert.ToInt64(dBTMCustomUserModel.GeneralTrainerMasterId), null, null, null, 1, int.MaxValue);
            DBTMTraineeDetailsListModel list = new DBTMTraineeDetailsListModel { DBTMTraineeDetailsList = response?.DBTMTraineeDetailsList };
            foreach (var item in list?.DBTMTraineeDetailsList.OrderBy(x => x.FirstName))
            {
                dropdownList.Add(new SelectListItem()
                {
                    Text = $"{item.FirstName} {item.LastName}",
                    Value = item.DBTMTraineeDetailId.ToString(),
                    Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.DBTMTraineeDetailId)
                });
            }
        }
        private static void GetDBTMPerformanceMatrix(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            dropdownList.Add(new SelectListItem() { Text = "-------Select-------" });

            DBTMPerformanceMatrixListResponse response = new DBTMTestClient().GetDBTMPerformanceMatrixList(null, null, null, 1, int.MaxValue);
            DBTMPerformanceMatrixListModel list = new DBTMPerformanceMatrixListModel() { DBTMPerformanceMatrixList = response.DBTMPerformanceMatrixList };
            foreach (var item in list?.DBTMPerformanceMatrixList.OrderBy(x => x.PerformanceMatrix))
            {
                dropdownList.Add(new SelectListItem()
                {
                    Text = $"{item.PerformanceMatrix}",
                    Value = item.DBTMPerformanceMatrixId.ToString(),
                    Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.DBTMPerformanceMatrixId)
                });
            }
        }
        private static void DBTMGraphTypeList(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            foreach (DBTMGraphCustomEnum graphType in Enum.GetValues(typeof(DBTMGraphCustomEnum)))
            {
                if (dropdownViewModel.ExcludedValues != null && dropdownViewModel.ExcludedValues.Any(x => x.Contains(graphType.ToString())))
                {
                    continue;
                }
                dropdownList.Add(new SelectListItem()
                {
                    Text = graphType.ToString(),
                    Value = graphType.ToString(),
                    Selected = graphType.ToString() == dropdownViewModel.DropdownSelectedValue
                });
            }
        }
        private static void GetDBTMTrainerListByCentreCode(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            GeneralTrainerListModel list = new GeneralTrainerListModel();
            if (dropdownViewModel.IsRequired)
                dropdownList.Add(new SelectListItem() { Value = "", Text = GeneralResources.SelectLabel });
            else
                dropdownList.Add(new SelectListItem() { Value = "0", Text = GeneralResources.SelectLabel });

            if (!string.IsNullOrEmpty(dropdownViewModel.Parameter))
            {
                string centreCode = SpiltCentreCode(dropdownViewModel.Parameter);

                GeneralTrainerListResponse response = new DBTMTraineeAssignmentClient().GetTrainerByCentreCode(centreCode);
                list = new GeneralTrainerListModel { GeneralTrainerList = response?.GeneralTrainerList };
                foreach (var item in list?.GeneralTrainerList?.OrderBy(x => x.FirstName))
                {
                    dropdownList.Add(new SelectListItem()
                    {
                        Text = $"{item.FirstName} {item.LastName}",
                        Value = item.GeneralTrainerMasterId.ToString(),
                        Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.GeneralTrainerMasterId)
                    });
                }
            }
        }
        private static void GetDisplayOn(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            dropdownList.Add(new SelectListItem
            {
                Text = "Both",
                Value = "Both",
                Selected = "Both" == dropdownViewModel.DropdownSelectedValue
            });

            dropdownList.Add(new SelectListItem
            {
                Text = "Only Web",
                Value = "OnlyWeb",
                Selected = "OnlyWeb" == dropdownViewModel.DropdownSelectedValue
            });

            dropdownList.Add(new SelectListItem
            {
                Text = "Only Mobile App",
                Value = "OnlyMobileApp",
                Selected = "OnlyMobileApp" == dropdownViewModel.DropdownSelectedValue
            });

            dropdownList.Add(new SelectListItem
            {
                Text = "None",
                Value = "None",
                Selected = "None" == dropdownViewModel.DropdownSelectedValue
            });
        }
        private static void GetGraphMode(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            dropdownList.Add(new SelectListItem
            {
                Text = "Instantaneous Chart",
                Value = "InstantaneousChart",
                Selected = "InstantaneousChart" == dropdownViewModel.DropdownSelectedValue
            });

            dropdownList.Add(new SelectListItem
            {
                Text = "Progress Chart",
                Value = "ProgressChart",
                Selected = "ProgressChart" == dropdownViewModel.DropdownSelectedValue
            });
        }
    }
}
