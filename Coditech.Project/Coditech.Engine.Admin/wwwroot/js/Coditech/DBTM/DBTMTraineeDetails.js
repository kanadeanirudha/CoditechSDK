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
    ConfirmDownloadPdf: function (traineeId, modalId) {
        var remarks = $("#RemarksText").val() || "";
        $("#" + modalId).modal("hide");
        DBTMTraineeDetails.DownloadAthleteReportPdf(traineeId, remarks);
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
    RenderFailedTable: function (rows, headers) {
        if (!rows || rows.length === 0) {
            $("#UploadErrorTableContainer").html("");
            $("#ErrorHeader").hide();
            return;
        }
        $("#ErrorHeader").show();
        var cols = Object.keys(rows[0]).filter(function (c) {
            return c !== "ErrorMessage";
        });
        var headerMap = {};
        headers.forEach(function (h) {
            headerMap[h.HeaderCode] = h;
        });
        var html = `<hr/><table class="table table-bordered"><thead><tr>`;
        cols.forEach(function (c) {
            var header = headerMap[c];
            var title = header ? header.HeaderName : c;
            if (header && header.IsRequired) {
                title += ' <span class="text-danger">*</span>';
            }
            html += `<th>${title}</th>`;
        });
        html += `</tr></thead><tbody>`;
        rows.forEach(function (r) {
            html += `<tr>`;
            cols.forEach(function (c) {
                var val = r[c] == null ? "" : r[c];
                var isError =
                    typeof val === "string" &&
                    (
                        val.includes("required") ||
                        val.includes("empty") ||
                        val.includes("invalid") ||
                        val.includes("exists") ||
                        val.includes("contains") ||
                        val.includes("expired")
                    );

                if (isError) {
                    html += `<td><span class="text-danger">${val}</span></td>`;
                }
                else {
                    html += `<td>${val}</td>`;
                }
            });
            html += `</tr>`;
        });

        html += `</tbody></table>`;
        $("#UploadErrorTableContainer").html(html);
    },
    UploadTraineeFile: function () {
        $("#UploadFileValidationMsg").text("");
        var fileInput = $("#TraineeFile")[0];
        if (!fileInput || fileInput.files.length === 0) {
            $("#UploadFileValidationMsg").text("Please select file.");
            return;
        }
        var file = fileInput.files[0];
        var formData = new FormData();
        formData.append("file", file);
        $("#UploadErrorTableContainer").html("");
        $("#ErrorHeader").hide();
        $.ajax({
            url: "/DBTMTraineeDetails/UploadTrainee",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            beforeSend: function () {
                CoditechCommon.ShowLodder();
            },
            success: function (res) {
                if (res.success) {
                    location.reload();
                    return;
                }
                CoditechCommon.HideLodder();
                if (res.failedRows && res.failedRows.length > 0) {
                    DBTMTraineeDetails.RenderFailedTable(res.failedRows, res.headers);
                }
                else {
                    CoditechNotification.DisplayNotificationMessage("Upload failed.", "error");
                }
            },
            error: function (xhr) {
                CoditechCommon.HideLodder();
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                    return;
                }
                CoditechNotification.DisplayNotificationMessage("Upload failed.", "error");
            }
        });
    },
    DownloadTemplatePopup: function () {
        var $openModal = $(".modal.show").first();
        if ($openModal.length > 0) {
            $openModal.addClass("stack-blur");
        }
        CoditechCommon.ShowLodder();
        $.ajax({
            type: "GET",
            url: "/DBTMTraineeDetails/GetDownloadTemplatePopup",
            success: function (html) {
                $("#DownloadTemplateContentId").html(html);
                var modalEl = document.getElementById("DownloadTemplatePopupId");
                var modal = new bootstrap.Modal(modalEl);
                modal.show();
                CoditechCommon.HideLodder();
                modalEl.addEventListener("hidden.bs.modal", function () {
                    $(".stack-blur").removeClass("stack-blur");
                    if ($(".modal.show").length > 0) {
                        $(".modal-backdrop").slice(1).remove();
                        $("body").addClass("modal-open");
                    } else {
                        $(".modal-backdrop").remove();
                        $("body").removeClass("modal-open");
                    }
                }, { once: true });
            },
            error: function (xhr) {
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Failed to load download template popup", "error");
                CoditechCommon.HideLodder();
            }
        });
    },
    ConfirmDownloadTemplate: function () {
        var count = $("#TraineeCount").val();
        clearFieldError("TraineeCount");
        count = parseInt(count, 10);
        if (!count || isNaN(count) || count < 1 || count > 999) {
            showFieldError("TraineeCount", "Please enter a number between 1 and 999.");
            return;
        }
        DBTMTraineeDetails.CheckAndDownloadTemplate(count);
    },
    CheckAndDownloadTemplate: function (count) {
        var centreCode = $("#SelectedCentreCode").val();
        var trainerId = $("#GeneralTrainerMasterId").val();
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
    OpenConvertPopup: function (contentId, traineeId) {
        $("#" + contentId).html("");
        CoditechCommon.ShowLodder();
        $.ajax({
            type: "GET",
            url: "/DBTMTraineeDetails/GetConvertCampPopup",
            data: { dBTMTraineeDetailId: traineeId },
            success: function (result) {
                $("#" + contentId).html(result);
                CoditechCommon.HideLodder();
            },
            error: function () {
                CoditechNotification.DisplayNotificationMessage("Failed to load popup.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },
    ConfirmConvert: function (traineeId) {
        $("#ConvertCampPopupId").modal("hide");
        CoditechCommon.ShowLodder();
        $.ajax({
            url: "/DBTMTraineeDetails/ConvertCampUserToBatchUser",
            type: "POST",
            data: { dBTMTraineeDetailId: traineeId },
            success: function (response) {
                if (response.success) {
                    CoditechNotification.DisplayNotificationMessage(response.message, "success");
                    CoditechDataTable.LoadList("DBTMTraineeDetails", "List");
                }
                else {

                    CoditechNotification.DisplayNotificationMessage(response.message, "error");
                }
                CoditechCommon.HideLodder();
            },
            error: function () {

                CoditechNotification.DisplayNotificationMessage("Something went wrong.", "error");
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
