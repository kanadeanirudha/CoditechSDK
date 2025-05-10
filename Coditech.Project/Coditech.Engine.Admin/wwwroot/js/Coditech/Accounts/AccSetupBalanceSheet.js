var AccSetupBalanceSheet = {
    Initialize: function () {
        AccSetupBalanceSheet.constructor();
    },

    constructor: function () {
    },

    GetBalanceSheetTypeByBalanceSheetTypeId: function () {
        $('#DataTables_SearchById').val("");

        if ($("#SelectedCentreCode").val() === "") {
            CoditechNotification.DisplayNotificationMessage("Please select centre.", "error");
        } else if ($("#SelectedParameter1").val() === "") {
            CoditechNotification.DisplayNotificationMessage("Please select balance sheet.", "error");
        } else {
            CoditechDataTable.LoadList("AccSetupBalanceSheet", "List");
        }
    },

    GetBalanceSheetByCentreCode: function () {
        var selectedItem = $("#SelectedCentreCode").val();

        if (selectedItem !== "") {
            CoditechCommon.ShowLodder();

            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/AccSetupBalanceSheet/GetBalanceSheetByCentreCode",
                data: { "centreCode": selectedItem },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#SelectedParameter1").html(data);
                    $("#AccSetupBalanceSheetTypeId").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status === 401 || xhr.status === 403) {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Balance Sheet.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        } else {
            $("#SelectedParameter1").html("");
        }
    },

}
