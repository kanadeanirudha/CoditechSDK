var AccountBalanceSheetReport = {
    Initialize: function () {
        this.GetFinancialYearListByCentreCode
    },

    constructor: function () {
    },

    GetFinancialYearListByCentreCode: function () {
        var selectedCentreCode = $("#SelectedCentreCode").val();
        if (selectedCentreCode !== "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/AccGLTransaction/GetFinancialYearListByCentreCode",
                data: { "selectedCentreCode": selectedCentreCode },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#GeneralFinancialYearId").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr) {
                    if (xhr.status === 401 || xhr.status === 403) {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve BalanceSheet.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        } else {
            CoditechNotification.DisplayNotificationMessage("Please select Centre and Balance Sheet Type.", "error");
        }
    },
};
$(document).ready(function () {
    
    AccountBalanceSheetReport.Initialize();
});