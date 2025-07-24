var DBTMReports = {
    Initialize: function () {
        DBTMReports.constructor();
    },
    constructor: function () {
    },

    GetDBTMBatchWiseReports: function () {
        var generalBatchMasterId = $("#GeneralBatchMasterId").val();
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
        var fromdate = $("#FromDate").val();
        var todate = $("#ToDate").val();
        $("#DBTMBatchWiseReportsDivId").html("");
        if (generalBatchMasterId != "" && dBTMTestMasterId != "")
        {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMReports/GetBatchWiseReports",
                data: {
                    "generalBatchMasterId": generalBatchMasterId,
                    "dBTMTestMasterId": dBTMTestMasterId,
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
        else
        {
            CoditechNotification.DisplayNotificationMessage("Please select batch and test.", "error");

        }
    },

    GetDBTMTestListByGeneralBatchMasterId: function () {
        var selectedItem = $("#GeneralBatchMasterId").val();

        if (selectedItem != "") {
            CoditechCommon.ShowLodder();

            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMReports/GetTestByGeneralBatchMasterId",
                data: { generalBatchMasterId: selectedItem },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#DBTMTestMasterId").html(data); 
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status === 401 || xhr.status === 403) {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve DBTM Activity.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        } else {
            $("#DBTMTestMasterId").html(""); 
        }
    },

    GetDBTMTestWiseReports: function () {
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
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
                url: "/DBTMReports/GetTestWiseReports",
                data: {
                    dBTMTestMasterId: dBTMTestMasterId,
                    dBTMTraineeDetailId: dBTMTraineeDetailId,
                    fromdate: fromdate,
                    todate: todate
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
        } else {
            CoditechNotification.DisplayNotificationMessage("Please select activity and trainer.", "error");
        }
    },

    GetDBTMTestWiseGraphReports: function () {
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
        var dBTMTraineeDetailId = $("#DBTMTraineeDetailId").val();
        var fromdate = $("#FromDate").val();
        var todate = $("#ToDate").val();
        var graphType = $("#GraphType").val(); 

        $("#DBTMTestWiseGraphReportsDivId").html("");

        if (dBTMTestMasterId !== "" && dBTMTraineeDetailId && dBTMTraineeDetailId.trim() !== "") {
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
                    graphType: graphType 
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
        } else {
            CoditechNotification.DisplayNotificationMessage("Please select activity and trainer.", "error");
        }
    },

    GetGraphListByDBTMTestMasterId: function () {
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();

        if (dBTMTestMasterId !== "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                url: '/DBTMReports/GetGraphListByDBTMTestMasterId',
                type: 'GET',
                dataType: 'html', 
                data: { dBTMTestMasterId: dBTMTestMasterId },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#GraphType").html(data); 
                    DBTMReports.GetDBTMTestWiseGraphReports(); 
                    CoditechCommon.HideLodder();
                },
                error: function () {
                    CoditechNotification.DisplayNotificationMessage("Failed to load graph list.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        }
    }
};
