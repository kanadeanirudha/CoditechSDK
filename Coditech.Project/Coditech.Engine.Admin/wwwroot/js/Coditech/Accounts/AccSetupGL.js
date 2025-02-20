var AccSetupGL = {
    Initialize: function () {
        AccSetupGL.constructor();
    },

    constructor: function () {
    },

    GetAccSetupBalanceSheetId: function () {
        $('#DataTables_SearchById').val("");
        if ($("#SelectedCentreCode").val() == "") {
            CoditechNotification.DisplayNotificationMessage("Please select centre.", "error");
        }
        else if ($("#AccSetupBalanceSheetTypeId").val() == "") {
            CoditechNotification.DisplayNotificationMessage("Please select balance sheet type.", "error");
        }
        else {
            CoditechDataTable.LoadList("AccSetupGL", "GetAccSetupGL");
        }
    },

    GetAccSetupBalanceSheetByCentreCodeAndTypeId: function () {
        var selectedCentreCode = $("#SelectedCentreCode").val();
        var accSetupBalanceSheetTypeId = $("#AccSetupBalanceSheetTypeId").val(); // Updated ID
        $("#AccSetupGLTreeDivId").html("");
        if (selectedCentreCode != "" && accSetupBalanceSheetTypeId != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/AccSetupGL/GetAccSetupBalanceSheetByCentreCodeAndTypeId",
                data: {
                    "selectedCentreCode": selectedCentreCode,
                    "accSetupBalanceSheetTypeId": accSetupBalanceSheetTypeId
                },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#AccSetupBalanceSheetId").html("").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve BalanceSheet.", "error")
                    CoditechCommon.HideLodder();
                }
            });
        }
        else {
            CoditechNotification.DisplayNotificationMessage("Please select Centre and Balance Sheet Type.", "error");
        }
    },

    GetAccSetupGLTree: function () {
        var selectedCentreCode = $("#SelectedCentreCode").val();
        var accSetupBalanceSheetTypeId = $("#AccSetupBalanceSheetTypeId").val();
        var accSetupBalanceSheetId = $("#AccSetupBalanceSheetId").val(); // Updated ID
        $("#AccSetupGLTreeDivId").html("");
        if (selectedCentreCode != "" && accSetupBalanceSheetTypeId != "" && accSetupBalanceSheetId != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/AccSetupGL/GetAccSetupGLTree",
                data: {
                    "selectedCentreCode": selectedCentreCode,
                    "accSetupBalanceSheetTypeId": accSetupBalanceSheetTypeId,
                    "accSetupBalanceSheetId": accSetupBalanceSheetId
                },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#AccSetupGLTreeDivId").html("").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve BalanceSheet.", "error")
                    CoditechCommon.HideLodder();
                }
            });
        }
        else {
            CoditechNotification.DisplayNotificationMessage("Please select Centre and Balance Sheet Type and BalanceSheet .", "error");
        }
    },
}
