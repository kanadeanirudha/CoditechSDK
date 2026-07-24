var DBTMPrintQR = {
    Initialize: function () {
        DBTMPrintQR.BindDropdownEvents();
    },
    constructor: function () {
        DBTMPrintQR.BindDropdownEvents();
    },
    BindDropdownEvents: function () {
        $(document).on("change", "#SelectedParameter1", function () {
            var batchId = $(this).val();
            CoditechDataTable.prototype.GetData(
                batchId,
                "DBTMPrintQR",
                "GetDBTMPrintQRTraineeList",
                "PrintQRUserListDiv");
        });
    },
    PrintQR: function () {
        var personIds = [];

        $("input[name='chkPerson']:checked").each(function () {
            personIds.push($(this).val());
        });
        $.ajax({
            url: "/DBTMPrintQR/GetDBTMPrintQR",
            type: "POST",
            data: {
                personIds: "11459"
            },
            success: function (response) {
                if (response.success) {
                    location.reload();
                    return;
                }
                CoditechNotification.DisplayNotificationMessage(response.message, "error");
                CoditechCommon.HideLodder();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                if (xhr.status == "401" || xhr.status == "403") {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Failed to transfer batch.", "error");
                CoditechCommon.HideLodder();
            }
        });
    }
};