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
            CoditechDataTable.prototype.GetData(batchId, "DBTMPrintQR", "GetDBTMPrintQRTraineeList", "PrintQRUserListDiv");
        });
    },

    LoadTraineeList: function () {
        var batchId = $("#SelectedParameter1").val();
        CoditechCommon.ShowLodder();
        $.ajax({
            url: "/DBTMPrintQR/GetDBTMPrintQRTraineeList",
            type: "GET",
            data: {
                SelectedParameter1: batchId
            },
            success: function (result) {
                $("#PrintQRUserListDiv").html(result);
                CoditechCommon.HideLodder();
            },
            error: function (xhr) {

                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                }

                CoditechNotification.DisplayNotificationMessage("Failed to load trainee list.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },
    PrintQR: function (personId) {
        var personIds = [];
        if (personId && personId > 0) {
            personIds.push(personId);
        }
        else {
            $(".person-checkbox:checked").each(function () {
                personIds.push($(this).val());
            });
        }
        if (personIds.length === 0) {
            CoditechNotification.DisplayNotificationMessage("Please select at least one trainee.", "error");
            return;
        }
        CoditechCommon.ShowLodder();
        $.ajax({
            url: "/DBTMPrintQR/CheckPrintQRAvailability",
            type: "GET",
            data: { personIds: personIds.join(',') },
            success: function (response) {
                if (response.success) {
                    var downloadUrl =
                        "/DBTMPrintQR/DownloadPrintQR?personIds="
                        + encodeURIComponent(personIds.join(','));
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
                CoditechNotification.DisplayNotificationMessage("Error while downloading QR.", "error" );
                CoditechCommon.HideLodder();
            }
        });
    }
};
$(document).ready(function () {
    DBTMPrintQR.Initialize();
    $(document).on("change", "#chkSelectAll", function () {
        $(".person-checkbox").prop("checked", $(this).is(":checked"));
    });
    $(document).on("change", ".person-checkbox", function () {
        $("#chkSelectAll").prop(
            "checked",
            $(".person-checkbox").length === $(".person-checkbox:checked").length
        );
    });
});