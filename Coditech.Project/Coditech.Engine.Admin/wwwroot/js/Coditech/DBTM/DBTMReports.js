var DBTMReports = {
    Initialize: function () {
        DBTMReports.constructor();
    },
    constructor: function () {
    },

    GetDBTMBatchWiseReports: function () {
        var generalBatchMasterId = $("#GeneralBatchMasterId").val();
        var fromdate = $("#FromDate").val();
        var todate = $("#ToDate").val();
        $("#DBTMBatchWiseReportsDivId").html("");
        if (generalBatchMasterId != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMReports/GetBatchWiseReports",
                data: {
                    "generalBatchMasterId": generalBatchMasterId,
                    "FromDate": fromdate,  
                    "ToDate": todate    
                },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#DBTMBatchWiseReportsDivId").html(data);
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
        else {
            $("#DBTMBatchWiseReportsDivId").html("");
        }
    },

    GetDBTMTestWiseReports: function () {
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
        var dBTMTraineeDetailId = $("#DBTMTraineeDetailId").val();
        var fromdate = $("#FromDate").val();
        var todate = $("#ToDate").val();

        $("#DBTMTestWiseReportsDivId").html("");
        if (dBTMTestMasterId != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMReports/GetTestWiseReports",
                data: {
                    "dBTMTestMasterId": dBTMTestMasterId, dBTMTraineeDetailId: dBTMTraineeDetailId,
                    "fromdate": fromdate, "todate": todate
                },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#DBTMTestWiseReportsDivId").html(data);
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
        }
        else {
            $("#DBTMTestWiseReportsDivId").html("");
        }
    }
};
