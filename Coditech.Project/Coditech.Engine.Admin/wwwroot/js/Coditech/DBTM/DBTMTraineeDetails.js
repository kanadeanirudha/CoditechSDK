var DBTMTraineeDetails = {
    Initialize: function () {
        DBTMTraineeDetails.constructor();
    },

    constructor: function () {
    },

    GetTrainerListByCentreCodeAndDepartmentId: function () {

        var selectedCentreCode = $("#SelectedCentreCode").val();
        var selectedDepartmentId = $("#SelectedDepartmentId").val();
        var entityId = $("#EntityId").val();

        if (selectedCentreCode !== "" && selectedDepartmentId !== "" && entityId !== "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMTraineeDetails/GetTrainerList",
                data: {
                    selectedCentreCode: selectedCentreCode,
                    selectedDepartmentId: selectedDepartmentId,
                    entityId: entityId

                },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#GeneralTrainerMasterId").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Trainer List", "error");
                    CoditechCommon.HideLodder();
                }
            });
        } else {
            $("#GeneralTrainerMasterId").html("");
        }
    },

    GetDBTMTrainerListByCentreCode: function (listType) {
        var selectedItem = $("#SelectedCentreCode").val();
        if (selectedItem != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMTraineeDetails/GetTrainerByCentreCode",
                data: { "centreCode": selectedItem },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#SelectedParameter1").html("").html(data);
                    $("#GeneralTrainerMasterId").html("").html(data);
                    DBTMTraineeDetails.GetDBTMTrainerListByGeneralTrainerMasterId(listType);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve DBTM Trainer.", "error")
                    CoditechCommon.HideLodder();
                }
            });
        }
        else {
            $("#SelectedParameter1").html("");
        }
    },

    GetDBTMTrainerListByGeneralTrainerMasterId: function (listType) {
        $('#DataTables_SearchById').val("");

        var centreCode = $("#SelectedCentreCode").val();
        var trainerId = $("#SelectedParameter1").val();

        if (!centreCode) {
            CoditechNotification.DisplayNotificationMessage("Please select centre.", "error");
            return;
        }
        if (trainerId === null || trainerId === "") {
            CoditechNotification.DisplayNotificationMessage("Please select trainer.", "error");
            return;
        }
        if (listType === "Active") {
            CoditechDataTable.LoadList("DBTMTraineeDetails", "ActiveMemberList");
        } else if (listType === "InActive") {
            CoditechDataTable.LoadList("DBTMTraineeDetails", "InActiveMemberList");
        } else {
            CoditechDataTable.LoadList("DBTMTraineeDetails", "List");
        }
    },

    GetActivityDetails: function (contentId, deviceDataId, trainerId) {
        $("#" + contentId).html("");
        CoditechCommon.ShowLodder();
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/DBTMTraineeDetails/GetActivityDetailsPopup",
            data: { dBTMDeviceDataId: deviceDataId, trainerId: trainerId },
            success: function (result) {
                $("#" + contentId).html(result);
                CoditechCommon.HideLodder();
            },
            error: function () {
                CoditechNotification.DisplayNotificationMessage("Failed to load Activity details.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },

    DownloadAthleteReportPdf: function (traineeId, remarks) {
        CoditechCommon.ShowLodder();
        $.ajax({
            url: "/DBTMTraineeDetails/CheckAthleteReportAvailability",
            type: "GET",
            data: {
                dBTMTraineeDetailId: traineeId,
                remarks: remarks
            },
            success: function (response) {
                if (response.success) {
                    var downloadUrl =
                        "/DBTMTraineeDetails/DownloadAthleteReportPdf"
                        + "?dBTMTraineeDetailId=" + encodeURIComponent(traineeId)
                        + "&remarks=" + encodeURIComponent(remarks);

                    CoditechCommon.HideLodder();

                    $("#hiddenDownloader").attr("src", downloadUrl);
                }
                else {
                    CoditechNotification.DisplayNotificationMessage(response.message, "error");
                    CoditechCommon.HideLodder();
                }
            },
            error: function () {
                CoditechNotification.DisplayNotificationMessage("Error while downloading profile.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },

    GetRemarks: function (contentId, dBTMTraineeDetailId, remarks) {
        $("#" + contentId).html("");
        CoditechCommon.ShowLodder();
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/DBTMTraineeDetails/GetRemarksPopup",
            data: { dBTMTraineeDetailId: dBTMTraineeDetailId, remarks: remarks },
            success: function (result) {
                $("#" + contentId).html(result);
                CoditechCommon.HideLodder();
            },
            error: function () {
                CoditechNotification.DisplayNotificationMessage("Failed to load.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },

    ConfirmDownloadPdf: function (traineeId) {
        var remarks = $("#RemarksText").val() || "";
        $("#RemarkPopupId").modal("hide");
        DBTMTraineeDetails.DownloadAthleteReportPdf(
            traineeId,
            remarks
        );
    },
    GetUploadTraineePopup: function (contentId) {
        $("#" + contentId).html("");
        CoditechCommon.ShowLodder();
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/DBTMTraineeDetails/GetUploadTraineePopup",
            success: function (result) {
                $("#" + contentId).html(result);
                CoditechCommon.HideLodder();
            },
            error: function () {
                CoditechNotification.DisplayNotificationMessage("Failed to load upload popup.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },
    UploadTraineeFile: function () {
        var fileInput = $("#TraineeFile")[0];
        if (!fileInput || fileInput.files.length === 0) {
            CoditechNotification.DisplayNotificationMessage("Please select file.", "error");
            return;
        }
        var file = fileInput.files[0];
        var formData = new FormData();
        formData.append("file", file);
        CoditechCommon.ShowLodder();
        $.ajax({
            url: "/DBTMTraineeDetails/UploadTrainee",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.success) {
                    CoditechNotification.DisplayNotificationMessage(res.message, "success");
                    $("#TraineePopupId").modal("hide");
                } else {
                    CoditechNotification.DisplayNotificationMessage(res.message, "error");
                }
                CoditechCommon.HideLodder();
            },
            error: function () {
                CoditechNotification.DisplayNotificationMessage("Upload failed.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },
}
