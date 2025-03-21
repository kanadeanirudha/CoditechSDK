var GeneralRunningNumbers = {
    Initialize: function () {
        GeneralRunningNumbers.constructor();
    },
    constructor: function () {
    },

    GetFinancialYearListByCentreCode: function () {
        var centreCode = $("#CentreCode").val();
        if (centreCode !== "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/GeneralRunningNumbers/GetFinancialYearListByCentreCode",
                data: { "centreCode": centreCode },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#GeneralFinancialYearId").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr) {
                    if (xhr.status == 401 || xhr.status == 403) {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Financial Year.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        } else {
            CoditechNotification.DisplayNotificationMessage("Please select Centre.", "error");
        }
    },

}