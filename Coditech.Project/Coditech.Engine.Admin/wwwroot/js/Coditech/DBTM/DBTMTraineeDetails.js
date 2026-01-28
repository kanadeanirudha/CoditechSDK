var DBTMTraineeDetails = {
    Initialize: function () {
        DBTMTraineeDetails.bindEvents();
    },
    bindEvents: function () {
        $('#TraineePopupId').on('hidden.bs.modal', function () {
            $("#TraineeFile").val("");
            $("#UploadErrorTableContainer").html("");
            $("#ErrorHeader").hide();
        });
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
            error: function (xhr) {
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                }
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
            error: function (xhr) {
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                }
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
            error: function (xhr) {
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                }
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
            error: function (xhr) {
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Failed to load upload popup.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },
    RenderFailedTable: function (rows) {
        if (!rows || rows.length === 0) {
            $("#UploadErrorTableContainer").html("");
            $("#ErrorHeader").hide();
            return;
        }
        $("#ErrorHeader").show();
        var cols = Object.keys(rows[0]);
        var html = `<hr/><table class="table table-bordered"><thead><tr>`;
        cols.forEach(function (c) {
            html += `<th>${c}</th>`;
        });
        html += `</tr></thead><tbody>`;
        rows.forEach(function (r) {
            html += `<tr>`;
            cols.forEach(function (c) {
                var val = r[c] == null ? "" : r[c];
                if (c.toLowerCase().includes("error"))
                    html += `<td style="color:red">${val}</td>`;
                else
                    html += `<td>${val}</td>`;
            });
            html += `</tr>`;
        });
        html += `</tbody></table>`;
        $("#UploadErrorTableContainer").html(html);
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
        $("#UploadErrorTableContainer").html("");
        $("#ErrorHeader").hide();
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
                    if (res.failedRows && res.failedRows.length > 0) {
                        DBTMTraineeDetails.RenderFailedTable(res.failedRows);
                    } else {
                        CoditechNotification.DisplayNotificationMessage(res.message, "error");
                    }
                }
                CoditechCommon.HideLodder();
            },
            error: function (xhr) {
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Upload failed.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },
    DownloadTemplatePopup: function () {
        $("#DownloadTemplateContentId").html("");
        CoditechCommon.ShowLodder();

        $.ajax({
            type: "GET",
            url: "/DBTMTraineeDetails/GetDownloadTemplatePopup",
            success: function (html) {
                $("#DownloadTemplateContentId").html(html);
                $("#DownloadTemplatePopupId").modal("show");
                CoditechCommon.HideLodder();
            },
            error: function () {
                CoditechNotification.DisplayNotificationMessage(
                    "Failed to load download template popup",
                    "error"
                );
                CoditechCommon.HideLodder();
            }
        });
    },
    ConfirmDownloadTemplate: function () {
        var count = $("#TraineeCount").val();
        clearFieldError("TraineeCount");
        if (!count || isNaN(count) || parseInt(count) <= 0) {
            showFieldError("TraineeCount", "Please enter a valid number of trainees.");
            return;
        }
        DBTMTraineeDetails.CheckAndDownloadTemplate(parseInt(count));
    },
    CheckAndDownloadTemplate: function (count) {

        var centreCode = $("#SelectedCentreCode").val();
        var trainerId = $("#SelectedParameter1").val();
        var userType = $("#UserType").val();

        CoditechCommon.ShowLodder();

        $.ajax({
            url: "/DBTMTraineeDetails/CheckTraineeTemplateAvailability",
            type: "GET",
            data: {
                centreCode: centreCode,
                trainerId: trainerId,
                userType: userType,
                count: count
            },
            success: function (response) {
                clearFieldError("TraineeCount");
                if (response.success) {
                    $("#DownloadTemplatePopupId").modal("hide");
                    var downloadUrl =
                        "/DBTMTraineeDetails/DownloadTraineeTemplate"
                        + "?centreCode=" + encodeURIComponent(centreCode)
                        + "&trainerId=" + encodeURIComponent(trainerId)
                        + "&userType=" + encodeURIComponent(userType || "")
                        + "&count=" + encodeURIComponent(count);
                    CoditechCommon.HideLodder();
                    $("#hiddenDownloader").attr("src", downloadUrl);

                } else {
                    showFieldError("TraineeCount", response.message);
                    CoditechCommon.HideLodder();
                }
            },
            error: function (xhr) {
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Error while downloading template.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },
};
function showFieldError(fieldName, message) {
    const span = $('[data-valmsg-for="' + fieldName + '"]');
    span
        .text(message)
        .removeClass('field-validation-valid')
        .addClass('field-validation-error');
}

function clearFieldError(fieldName) {
    const span = $('[data-valmsg-for="' + fieldName + '"]');
    span
        .text('')
        .removeClass('field-validation-error')
        .addClass('field-validation-valid');
}
