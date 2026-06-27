var activityPerformedDates = [];
var DBTMReports = {
    Initialize: function () {
        DBTMReports.constructor();
    },
    constructor: function () {
    },
    GetDBTMTestWiseMultiReports: function () {
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
        dBTMTestMasterId = dBTMTestMasterId ? dBTMTestMasterId.join(",") : "";

        var dBTMTraineeDetailId = $("#DBTMTraineeDetailId").val();
        var fromdate = $("#FromDate").val();
        var todate = $("#ToDate").val();

        $("#DBTMTestWiseMultiReportsDivId").html("");

        if (dBTMTestMasterId !== "" && dBTMTraineeDetailId && dBTMTraineeDetailId.trim() !== "") {
            DBTMReports.LoadActivityPerformedDates(true);
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMReports/GetTestWiseReports",
                data: {
                    dBTMTestMasterIds: dBTMTestMasterId,
                    dBTMTraineeDetailId: dBTMTraineeDetailId,
                    FromDate: fromdate,
                    ToDate: todate
                },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#DBTMTestWiseMultiReportsDivId").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve activity Reports.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        } if (dBTMTestMasterId.length > 0 && dBTMTraineeDetailId) {
        } else {
            CoditechNotification.DisplayNotificationMessage("Please select activity.", "error");
        }
    },

    GetDBTMTestWiseGraphReports: function () {
        DBTMReports.LoadActivityPerformedDates();
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
        var dBTMTraineeDetailId = $("#DBTMTraineeDetailId").val();
        var fromdate = $("#FromDate").val();
        var todate = $("#ToDate").val();
        var graphMode = $("#GraphMode").val();
        var dBTMGraphMasterId = $("#DBTMSelectedGraph").val();
        dBTMGraphMasterId = dBTMGraphMasterId ? dBTMGraphMasterId.join(",") : "";

        $("#DBTMTestWiseGraphReportsDivId").html("");
        if (!graphMode || graphMode.trim() === "") {
            CoditechNotification.DisplayNotificationMessage("Please select Graph Mode.", "error");
            return;
        }

        if (!dBTMTestMasterId || dBTMTestMasterId.trim() === "") {
            CoditechNotification.DisplayNotificationMessage("Please select Activity.", "error");
            return;
        }

        if (!dBTMTraineeDetailId || dBTMTraineeDetailId.trim() === "") {
            CoditechNotification.DisplayNotificationMessage("Please select Trainer.", "error");
            return;
        }

        CoditechCommon.ShowLodder();
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/DBTMReports/GetTestWiseGraphReports",
            data: {
                dBTMTestMasterId: dBTMTestMasterId,
                dBTMTraineeDetailId: dBTMTraineeDetailId,
                fromdate: fromdate,
                todate: todate,
                dBTMGraphMasterIds: dBTMGraphMasterId,
                graphMode: graphMode
            },
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#DBTMTestWiseGraphReportsDivId").html(data);
                CoditechCommon.HideLodder();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                if (xhr.status == "401" || xhr.status == "403") {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Failed to retrieve Test Reports.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },

    GetGraphListByDBTMTestMasterId: function () {
        $("#DBTMTestWiseGraphReportsDivId").html("");
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
        var graphMode = $("#GraphMode").val();

        if (dBTMTestMasterId !== "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                url: '/DBTMReports/GetGraphListByDBTMTestMasterId',
                type: 'GET',
                dataType: 'json',
                data: { dBTMTestMasterId: dBTMTestMasterId, graphMode: graphMode },
                success: function (data) {
                    var $ddl = $("#DBTMSelectedGraph");
                    $ddl.empty();
                    $.each(data, function (i, item) {
                        $ddl.append($('<option>', {
                            value: item.Value,
                            text: item.Text
                        }));
                    });
                    $ddl.selectpicker('refresh');
                    DBTMReports.LoadActivityPerformedDates();
                    CoditechCommon.HideLodder();
                },
                error: function (xhr) {
                    if (xhr.status == 401 || xhr.status == 403) {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to load graph list.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        }
    },
    GetGraphListByGraphMode: function () {
        $("#DBTMTestWiseGraphReportsDivId").html("");
        applyGraphModeUI();
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
        var graphMode = $("#GraphMode").val();
        if (graphMode === "InstantaneousChart") {
            var fromDate = $("#FromDate").datepicker("getDate");
            if (fromDate) {
                $("#ToDate").datepicker("setDate", fromDate);
            }
            $("#ToDate").prop("readonly", true);
        } else {
            $("#ToDate").prop("readonly", false);
        }
        if (dBTMTestMasterId) {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                url: '/DBTMReports/GetGraphListByDBTMTestMasterId',
                type: 'GET',
                dataType: 'json',
                data: { dBTMTestMasterId: dBTMTestMasterId, graphMode: graphMode },
                success: function (data) {
                    var $ddl = $("#DBTMSelectedGraph");
                    $ddl.empty();

                    $.each(data, function (i, item) {
                        $ddl.append($('<option>', {
                            value: item.Value,
                            text: item.Text
                        }));
                    });
                    $ddl.selectpicker('refresh');
                    if (graphMode === "Progress Chart") {
                        DBTMReports.GetDBTMTestWiseGraphReports();
                    }
                    CoditechCommon.HideLodder();
                },
                error: function (xhr) {
                    if (xhr.status == 401 || xhr.status == 403) {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to load graph list.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        }
    },

    LoadActivityPerformedDates: function (showMessage = false) {
        var dBTMTestMasterIds = $("#DBTMTestMasterId").val();
        var dBTMTraineeDetailId = $("#DBTMTraineeDetailId").val();
        if (!dBTMTraineeDetailId) {
            activityPerformedDates = [];
            $("#FromDate,#ToDate").datepicker("refresh");
            return;
        }
        if (!dBTMTestMasterIds || dBTMTestMasterIds.length === 0) {
            activityPerformedDates = [];
            $("#FromDate,#ToDate").datepicker("refresh");
            return;
        }
        if (!Array.isArray(dBTMTestMasterIds)) {
            dBTMTestMasterIds = [dBTMTestMasterIds];
        }
        $.ajax({
            type: "GET",
            url: "/DBTMReports/GetActivityPerformedDates",
            data: {
                dBTMTestMasterIds: dBTMTestMasterIds.join(","),
                dBTMTraineeDetailId: dBTMTraineeDetailId
            },
            success: function (data) {
                activityPerformedDates = data || [];
                $("#FromDate,#ToDate").datepicker("refresh");
            },
            error: function (xhr) {
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                }
                activityPerformedDates = [];
                CoditechNotification.DisplayNotificationMessage("Failed to load activity dates.", "error");
            }
        });
    },
    LoadBatchActivityPerformedDates: function () {
        var dBTMTestMasterIds = $("#DBTMTestMasterId").val();
        var generalBatchMasterId = $("#GeneralBatchMasterId").val();
        if (!dBTMTestMasterIds || dBTMTestMasterIds.length === 0 || !generalBatchMasterId) {
            activityPerformedDates = [];
            $("#FromDate,#ToDate").datepicker("refresh");
            return;
        }
        if (!Array.isArray(dBTMTestMasterIds)) {
            dBTMTestMasterIds = [dBTMTestMasterIds];
        }
        $.ajax({
            type: "GET",
            url: "/DBTMReports/GetBatchActivityPerformedDates",
            data: {
                dBTMTestMasterIds: dBTMTestMasterIds.join(","),
                generalBatchMasterId: generalBatchMasterId
            },
            success: function (data) {
                activityPerformedDates = data || [];
                if (!activityPerformedDates || activityPerformedDates.length === 0) {
                    CoditechNotification.DisplayNotificationMessage("Batch has never been tested.", "error");
                }
                $("#FromDate,#ToDate").datepicker("refresh");
            },
            error: function () {
                activityPerformedDates = [];
                CoditechNotification.DisplayNotificationMessage(
                    "Failed to load batch activity dates.",
                    "error"
                );
            }
        });
    },

    GetDBTMMultiTestListByGeneralBatchMasterId: function () {
        $("#DBTMBatchWiseMultiReportsDivId").html("");
        var selectedItem = $("#GeneralBatchMasterId").val();
        if (selectedItem != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                url: "/DBTMReports/GetMultiTestByGeneralBatchMasterId",
                data: { generalBatchMasterId: selectedItem },
                success: function (data) {
                    $("#DBTMTestMasterId").html(data);
                    $('#DBTMTestMasterId').selectpicker('refresh');

                    CoditechCommon.HideLodder();
                },
                error: function (xhr) {
                    if (xhr.status == 401 || xhr.status == 403) {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve DBTM Activity.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        } else {
            $("#DBTMTestMasterId").html("");
            $('#DBTMTestMasterId').selectpicker('refresh');
        }
    },

    GetDBTMBatchWiseMultiReports: function () {
        var generalBatchMasterId = $("#GeneralBatchMasterId").val();
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
        dBTMTestMasterId = dBTMTestMasterId ? dBTMTestMasterId.join(",") : "";

        var fromdate = $("#FromDate").val();
        var todate = $("#ToDate").val();
        $("#DBTMBatchWiseMultiReportsDivId").html("");
        if (generalBatchMasterId != "" && dBTMTestMasterId != "") {
            CoditechCommon.ShowLodder();
            if (!activityPerformedDates || activityPerformedDates.length === 0) {
                CoditechNotification.DisplayNotificationMessage("Batch has never been tested.", "error");
                return;
            }
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMReports/GetBatchWiseReports",
                data: {
                    "generalBatchMasterId": generalBatchMasterId,
                    "dBTMTestMasterIds": dBTMTestMasterId,
                    "FromDate": fromdate,
                    "ToDate": todate
                },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#DBTMBatchWiseMultiReportsDivId").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Batch Reports.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        }
        else if (generalBatchMasterId == "" || generalBatchMasterId == "0") {
            CoditechNotification.DisplayNotificationMessage("Please select a batch.", "error");
        }
        else if (dBTMTestMasterId == "") {
            CoditechNotification.DisplayNotificationMessage("Please select an activity.", "error");
        }
        else {
            CoditechNotification.DisplayNotificationMessage("Please select batch and activity.", "error");
        }
    },

    GetTraineeChangeNameWiseReports: function () {
        $("#DBTMNameWiseReportsDivId").html("");
    },

    GetDBTMNameWiseMultiReports: function () {
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
        dBTMTestMasterId = dBTMTestMasterId ? dBTMTestMasterId.join(",") : "";

        var dBTMTraineeDetailId = $("#DBTMTraineeDetailId").val();
        var fromdate = $("#FromDate").val();
        var todate = $("#ToDate").val();

        $("#DBTMNameWiseReportsDivId").html("");
        if (dBTMTraineeDetailId && dBTMTraineeDetailId !== "0" && dBTMTestMasterId !== "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMReports/GetNameWiseReports",
                data: {
                    dBTMTestMasterIds: dBTMTestMasterId,
                    dBTMTraineeDetailId: dBTMTraineeDetailId,
                    FromDate: fromdate,
                    ToDate: todate
                },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#DBTMNameWiseReportsDivId").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve activity Reports.", "error"
                    );
                    CoditechCommon.HideLodder();
                }
            });
        }
        else if (dBTMTestMasterId == "") {
            CoditechNotification.DisplayNotificationMessage("Please select an activity.", "error");
        }
        else {
            CoditechNotification.DisplayNotificationMessage("Please select trainee and activity.", "error");
        }
    },

    GetDBTMTestWiseMultiReportsFile: function () {
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
        dBTMTestMasterId = dBTMTestMasterId ? dBTMTestMasterId.join(",") : "";

        var dBTMTraineeDetailId = $("#DBTMTraineeDetailId").val();
        var fromdate = $("#FromDate").val();
        var todate = $("#ToDate").val();
        var reportType = $("#ReportType").val();

        $("#DBTMTestWiseMultiReportsDivId").html("");

        if (dBTMTestMasterId !== "" && dBTMTraineeDetailId && dBTMTraineeDetailId.trim() !== "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMReports/GetTestWiseReportsFile",
                data: {
                    dBTMTestMasterIds: dBTMTestMasterId,
                    dBTMTraineeDetailId: dBTMTraineeDetailId,
                    FromDate: fromdate,
                    ToDate: todate,
                    ReportType: reportType
                },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#DBTMTestWiseMultiReportsDivId").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Error while downloading report.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        } if (dBTMTestMasterId.length > 0 && dBTMTraineeDetailId) {
        } else {
            CoditechNotification.DisplayNotificationMessage("Please select activity.", "error");
        }
    },

    DownloadExcelReport: function () {
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
        dBTMTestMasterId = dBTMTestMasterId ? dBTMTestMasterId.join(",") : "";
        var dBTMTraineeDetailId = $("#DBTMTraineeDetailId").val();
        var fromdate = $("#FromDate").val();
        var todate = $("#ToDate").val();
        var reportType = $("#ReportType").val() || "excel";

        if (dBTMTestMasterId !== "" && dBTMTraineeDetailId && dBTMTraineeDetailId.trim() !== "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                url: "/DBTMReports/CheckReportAvailability",
                type: "GET",
                data: {
                    dBTMTestMasterIds: dBTMTestMasterId,
                    dBTMTraineeDetailId: dBTMTraineeDetailId,
                    fromDate: fromdate,
                    toDate: todate,
                },
                success: function (response) {
                    if (response.success) {
                        var downloadUrl = "/DBTMReports/DownloadReport"
                            + "?dBTMTestMasterIds=" + encodeURIComponent(dBTMTestMasterId)
                            + "&dBTMTraineeDetailId=" + encodeURIComponent(dBTMTraineeDetailId)
                            + "&fromDate=" + encodeURIComponent(fromdate)
                            + "&toDate=" + encodeURIComponent(todate)
                            + "&reportType=" + encodeURIComponent(reportType);
                        CoditechCommon.HideLodder();
                        $("#hiddenDownloader").attr("src", downloadUrl);
                    } else {
                        CoditechNotification.DisplayNotificationMessage(response.message || "No data available for download.", "error");
                        CoditechCommon.HideLodder();
                    }
                },
                error: function (xhr) {
                    if (xhr.status == 401 || xhr.status == 403) {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Error while checking report availability.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        } else {
            CoditechNotification.DisplayNotificationMessage("Please select activity.", "error");
        }
    },

    DownloadBatchExcelReport: function () {
        var generalBatchMasterId = $("#GeneralBatchMasterId").val();
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
        dBTMTestMasterId = dBTMTestMasterId ? dBTMTestMasterId.join(",") : "";
        var fromdate = $("#FromDate").val();
        var todate = $("#ToDate").val();
        var reportType = $("#ReportType").val() || "excel";

        if (dBTMTestMasterId !== "" && generalBatchMasterId && generalBatchMasterId.trim() !== "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                url: "/DBTMReports/CheckBatchReportAvailability",
                type: "GET",
                data: {
                    dBTMTestMasterIds: dBTMTestMasterId,
                    generalBatchMasterId: generalBatchMasterId,
                    fromDate: fromdate,
                    toDate: todate,
                },
                success: function (response) {
                    if (response.success) {
                        var downloadUrl = "/DBTMReports/DownloadBatchReport"
                            + "?dBTMTestMasterIds=" + encodeURIComponent(dBTMTestMasterId)
                            + "&generalBatchMasterId=" + encodeURIComponent(generalBatchMasterId)
                            + "&fromDate=" + encodeURIComponent(fromdate)
                            + "&toDate=" + encodeURIComponent(todate)
                            + "&reportType=" + encodeURIComponent(reportType);
                        CoditechCommon.HideLodder();
                        $("#hiddenDownloader").attr("src", downloadUrl);
                    } else {
                        CoditechNotification.DisplayNotificationMessage(response.message || "No data available for download.", "error");
                        CoditechCommon.HideLodder();
                    }
                },
                error: function (xhr) {
                    if (xhr.status == 401 || xhr.status == 403) {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Error while checking report availability.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        } else {
            CoditechNotification.DisplayNotificationMessage("Please select activity.", "error");
        }
    },
    ShowActivityDetailPopup: function (modelPopContentId, deviceDataId, dBTMTraineeDetailId, enableStackBlur) {
        if (enableStackBlur === true) {
            var $openModal = $(".modal.show");
            if ($openModal.length > 0) {
                $openModal.addClass("stack-blur");
            }
        }
        CoditechCommon.ShowLodder();
        $("#" + modelPopContentId).html("");
        $.ajax({
            type: "GET",
            url: "/DBTMReports/ViewActivityDetailPopup",
            data: {
                dBTMDeviceDataId: deviceDataId,
                dBTMTraineeDetailId: dBTMTraineeDetailId
            },
            success: function (result) {
                $("#" + modelPopContentId).html(result);
                CoditechCommon.HideLodder();
                var modalEl = document.getElementById("DBTMActivityDetailPopupId");
                var modal = new bootstrap.Modal(modalEl);
                modal.show();
                if (enableStackBlur === true) {
                    modalEl.addEventListener("hidden.bs.modal", function () {
                        $(".stack-blur").removeClass("stack-blur");
                        $(".modal-backdrop").not(":first").remove();
                        if ($(".modal.show").length > 0) {
                            $("body").addClass("modal-open");
                        }
                    }, { once: true });
                }
            },
            error: function (xhr) {
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                }
                CoditechCommon.HideLodder();
                CoditechNotification.DisplayNotificationMessage(
                    "Failed to load activity details.",
                    "error"
                );
            }
        });
    },

    GetBatchUserListByBatchId: function () {

        var selectedItem = $("#GeneralBatchMasterId").val();

        if (selectedItem != "") {

            CoditechCommon.ShowLodder();

            $.ajax({
                cache: false,
                type: "GET",
                url: "/DBTMReports/GetBatchUserListByBatchId",
                data: { generalBatchMasterId: selectedItem },
                success: function (data) {
                    $("#DBTMTraineeDetailId").html("").html(data);
                    $('#DBTMTraineeDetailId').selectpicker('refresh');
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Trainee Details List", "error")
                    CoditechCommon.HideLodder();
                }
            });
        }
        else {
            $("#DBTMTraineeDetailId").html("");
            $('#DBTMTraineeDetailId').selectpicker('refresh');
        }

    },

    GetBatchWiseTraineeProfileDetailsList: function () {
        var generalBatchMasterId = $("#GeneralBatchMasterId").val();
        var dbtmTraineeDetailId = $("#DBTMTraineeDetailId").val();
        var orderBy = $("#OrderBy").val();
        var todate = $("#ToDate").val();
        var fromdate = todate;
        if (Array.isArray(dbtmTraineeDetailId)) {
            dbtmTraineeDetailId = dbtmTraineeDetailId.join(",");
        }
        $("#DBTMBatchWiseTraineeProfileDetailsDivId").html("");
        if (generalBatchMasterId && dbtmTraineeDetailId) {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMReports/GetBatchWiseTraineeProfileDetailsList",
                data: {
                    generalBatchMasterId: generalBatchMasterId,
                    dbtmTraineeDetailIds: dbtmTraineeDetailId,
                    orderBy: orderBy,
                    FromDate: fromdate,
                    ToDate: todate
                },
                success: function (data) {
                    $("#DBTMBatchWiseTraineeProfileDetailsDivId").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr) {
                    if (xhr.status == 401 || xhr.status == 403) {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage(
                        "Failed to retrieve Batch trainees.",
                        "error"
                    );
                    CoditechCommon.HideLodder();
                }
            });

        }
        else if (!generalBatchMasterId || generalBatchMasterId === "0") {

            CoditechNotification.DisplayNotificationMessage(
                "Please select a batch.",
                "error"
            );
        }
        else if (!dbtmTraineeDetailId) {

            CoditechNotification.DisplayNotificationMessage(
                "Please select a trainee.",
                "error"
            );
        }
    },
    GetDBTMMultiTestListByDBTMCampMasterId: function () {
        var campId = $("#DBTMCampMasterId").val();
        if (campId != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                url: "/DBTMReports/GetMultiTestByCampMasterId",
                data: { dBTMCampMasterId: campId },
                success: function (data) {
                    $("#DBTMTestMasterId").html(data);
                    $('#DBTMTestMasterId').selectpicker('refresh');
                    DBTMReports.LoadCampActivityPerformedDates();
                    CoditechCommon.HideLodder();
                },
                error: function () {
                    CoditechNotification.DisplayNotificationMessage("Failed to load activity.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        } else {

            $("#DBTMTestMasterId").html("");
            $('#DBTMTestMasterId').selectpicker('refresh');
        }
    },
    GetDBTMCampWiseMultiReports: function () {
        var campId = $("#DBTMCampMasterId").val();
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
        dBTMTestMasterId = dBTMTestMasterId ? dBTMTestMasterId.join(",") : "";
        var fromdate = $("#FromDate").val();
        var todate = $("#ToDate").val();
        $("#DBTMCampWiseMultiReportsDivId").html("");
        if (campId != "" && dBTMTestMasterId != "") {
            CoditechCommon.ShowLodder();
            if (!activityPerformedDates || activityPerformedDates.length === 0) {
                CoditechNotification.DisplayNotificationMessage("Camp has never been tested.", "error");
                return;
            }
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMReports/GetCampWiseReports",
                data: {
                    dBTMCampMasterId: campId,
                    dBTMTestMasterIds: dBTMTestMasterId,
                    FromDate: fromdate,
                    ToDate: todate
                },
                success: function (data) {
                    $("#DBTMCampWiseMultiReportsDivId").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function () {
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Camp Reports.", "error");
                    CoditechCommon.HideLodder();
                }
            });

        } else if (campId == "" || campId == "0") {
            CoditechNotification.DisplayNotificationMessage("Please select a camp.", "error");
        } else if (dBTMTestMasterId == "") {
            CoditechNotification.DisplayNotificationMessage("Please select an activity.", "error");
        }
    },
    DownloadCampExcelReport: function () {
        var campId = $("#DBTMCampMasterId").val();
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
        dBTMTestMasterId = dBTMTestMasterId ? dBTMTestMasterId.join(",") : "";
        var fromdate = $("#FromDate").val();
        var todate = $("#ToDate").val();
        var reportType = $("#ReportType").val() || "excel";
        if (dBTMTestMasterId !== "" && campId && campId.trim() !== "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                url: "/DBTMReports/CheckCampReportAvailability",
                type: "GET",
                data: {
                    dBTMTestMasterIds: dBTMTestMasterId,
                    dBTMCampMasterId: campId,
                    fromDate: fromdate,
                    toDate: todate
                },
                success: function (response) {
                    if (response.success) {
                        var downloadUrl = "/DBTMReports/DownloadCampReport"
                            + "?dBTMTestMasterIds=" + encodeURIComponent(dBTMTestMasterId)
                            + "&dBTMCampMasterId=" + encodeURIComponent(campId)
                            + "&fromDate=" + encodeURIComponent(fromdate)
                            + "&toDate=" + encodeURIComponent(todate)
                            + "&reportType=" + encodeURIComponent(reportType);
                        CoditechCommon.HideLodder();
                        $("#hiddenDownloader").attr("src", downloadUrl);

                    } else {
                        CoditechNotification.DisplayNotificationMessage(response.message || "No data available.", "error");
                        CoditechCommon.HideLodder();
                    }
                },
                error: function () {
                    CoditechNotification.DisplayNotificationMessage("Error while downloading report.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        } else {
            CoditechNotification.DisplayNotificationMessage("Please select activity.", "error");
        }
    },
    LoadCampActivityPerformedDates: function () {
        var dBTMTestMasterIds = $("#DBTMTestMasterId").val();
        var campId = $("#DBTMCampMasterId").val();
        if (!dBTMTestMasterIds || dBTMTestMasterIds.length === 0 || !campId) {
            activityPerformedDates = [];
            $("#FromDate,#ToDate").datepicker("refresh");
            return;
        }
        if (!Array.isArray(dBTMTestMasterIds)) {
            dBTMTestMasterIds = [dBTMTestMasterIds];
        }
        $.ajax({
            type: "GET",
            url: "/DBTMReports/GetCampActivityPerformedDates",
            data: {
                dBTMTestMasterIds: dBTMTestMasterIds.join(","),
                dBTMCampMasterId: campId
            },
            success: function (data) {
                activityPerformedDates = (data || []).map(function (d) {
                    return d.split('T')[0];
                });
                if (!activityPerformedDates || activityPerformedDates.length === 0) {
                    CoditechNotification.DisplayNotificationMessage("Camp has never been tested.", "error");
                }
                $("#FromDate,#ToDate").datepicker("refresh");
            },
            error: function () {
                activityPerformedDates = [];
            }
        });
    },
    LoadTraineeProfileActivityDates: function () {
        var traineeIds = $("#DBTMTraineeDetailId").val();
        var generalBatchMasterId = $("#GeneralBatchMasterId").val();
        if (!traineeIds || traineeIds.length === 0) {
            activityPerformedDates = [];
            $("#ToDate").datepicker("refresh");
            return;
        }
        if (Array.isArray(traineeIds)) {
            traineeIds = traineeIds.join(",");
        }
        $.ajax({
            type: "GET",
            url: "/DBTMReports/GetTraineeListActivityDates",
            data: {
                traineeIds: traineeIds,
                generalBatchMasterId: generalBatchMasterId
            },
            success: function (data) {
                activityPerformedDates = (data || []).map(d =>
                    d.split("T")[0]
                );
                $("#ToDate").datepicker("refresh");
            },
            error: function () {
                activityPerformedDates = [];
            }
        });
    },
    GetBatchByTrainerId: function () {
        var generalTrainerMasterId = $("#GeneralTrainerMasterId").val();
        $("#DBTMBatchWiseMultiReportsDivId").html("");
        $("#DBTMTestMasterId").html("");
        $('#DBTMTestMasterId').selectpicker('refresh');
        activityPerformedDates = [];
        CoditechCommon.ShowLodder();
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/DBTMReports/GetBatchByTrainerId",
            data: {
                generalTrainerMasterId: generalTrainerMasterId
            },
            success: function (data) {
                $("#GeneralBatchMasterId").html(data);
                $('#GeneralBatchMasterId').selectpicker('refresh');
                CoditechCommon.HideLodder();
            },
            error: function (xhr) {
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Failed to load batches.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },
};