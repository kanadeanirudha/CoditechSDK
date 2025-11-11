var DBTMReports = {
    Initialize: function () {
        DBTMReports.constructor();
    },
    constructor: function () {
    },

    GetDBTMMultiTestListByGeneralBatchMasterId: function () {
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
                error: function () {
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

    GetDBTMTestWiseMultiReports: function () {
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
        dBTMTestMasterId = dBTMTestMasterId ? dBTMTestMasterId.join(",") : "";

        var dBTMTraineeDetailId = $("#DBTMTraineeDetailId").val();
        var fromdate = $("#FromDate").val();
        var todate = $("#ToDate").val();

        $("#DBTMTestWiseMultiReportsDivId").html("");

        if (dBTMTestMasterId !== "" && dBTMTraineeDetailId && dBTMTraineeDetailId.trim() !== "") {
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
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
        var dBTMTraineeDetailId = $("#DBTMTraineeDetailId").val();
        var fromdate = $("#FromDate").val();
        var todate = $("#ToDate").val();
        var dBTMGraphMasterId = $("#DBTMGraphMasterId").val();
        var graphMode = $("#GraphMode").val();

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

        if (!dBTMGraphMasterId || dBTMGraphMasterId.trim() === "") {
            CoditechNotification.DisplayNotificationMessage("Please select Graph Type.", "error");
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
                dBTMGraphMasterId: dBTMGraphMasterId,
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
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
        var graphMode = $("#GraphMode").val();

        if (dBTMTestMasterId !== "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                url: '/DBTMReports/GetGraphListByDBTMTestMasterId',
                type: 'GET',
                dataType: 'html',
                data: { dBTMTestMasterId: dBTMTestMasterId, graphMode: graphMode },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#DBTMGraphMasterId").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function () {
                    CoditechNotification.DisplayNotificationMessage("Failed to load graph list.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        }
    },
    GetGraphListByGraphMode: function () {
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
                dataType: 'html',
                data: {
                    dBTMTestMasterId: dBTMTestMasterId,
                    graphMode: graphMode
                },
                success: function (data) {
                    $("#DBTMGraphMasterId").html(data);
                    DBTMReports.GetDBTMTestWiseGraphReports();
                    CoditechCommon.HideLodder();
                },
                error: function () {
                    CoditechNotification.DisplayNotificationMessage("Failed to load graph list.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        }
    },

    GetDBTMNameWiseReports: function () {
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
        dBTMTestMasterId = dBTMTestMasterId ? dBTMTestMasterId.join(",") : "";

        var dBTMTraineeDetailId = $("#DBTMTraineeDetailId").val();
        var fromdate = $("#FromDate").val();
        var todate = $("#ToDate").val();

        $("#DBTMTestWiseReportsDivId").html("");

        if (dBTMTestMasterId !== "" && dBTMTraineeDetailId && dBTMTraineeDetailId.trim() !== "") {
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
                error: function () {
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
                error: function () {
                    CoditechNotification.DisplayNotificationMessage("Error while checking report availability.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        } else {
            CoditechNotification.DisplayNotificationMessage("Please select activity.", "error");
        }
    }
};