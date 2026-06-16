var DBTMBatch = {
    Initialize: function () {
        DBTMBatch.constructor();
    },
    constructor: function () {
    },
    OnCentreChange: function () {
        var centreCode = $("#CentreCode").val();
        var selectedActivities = $("#CustomDropdownSelectedValue1").val();
        if (selectedActivities && selectedActivities.length > 0) {
            return;
        }
        if (!centreCode) {
            $("#ActivityDropdownDiv").html("");
            return;
        }
        CoditechCommon.ShowLodder();
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/DBTMGeneralBatchMaster/GetActivityByCentreCode",
            data: {
                centreCode: centreCode,
                selectedActivities: selectedActivities
            },
            success: function (data) {
                $("#ActivityDropdownDiv").html(data);
                $("#ActivityDropdownDiv .selectpicker").selectpicker('render');
                $("#ActivityDropdownDiv .selectpicker").selectpicker('refresh');
                CoditechCommon.HideLodder();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                if (xhr.status == "401" || xhr.status == "403") {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Failed to retrieve Activity.", "error")
                CoditechCommon.HideLodder();
            }
        });
    },
    OpenTransferBatchPopup: function (contentId, generalBatchMasterId) {
        $("#" + contentId).html("");
        CoditechCommon.ShowLodder();
        $.ajax({
            type: "GET",
            url: "/DBTMGeneralBatchMaster/GetTransferBatchPopup",
            data: { generalBatchMasterId: generalBatchMasterId },
            success: function (result) {
                $("#" + contentId).html(result);
                CoditechCommon.HideLodder();
            },
            error: function () {
                CoditechCommon.HideLodder();
                CoditechNotification.DisplayNotificationMessage("Failed to load transfer popup.", "error" );
            }
        });
    },
    ConfirmTransfer: function (generalBatchMasterId) {
        var trainerId = $("#GeneralTrainerMasterId").val();
        if (!trainerId || trainerId == "0") {
            CoditechNotification.DisplayNotificationMessage("Please select trainer.", "error");
            return;
        }
        $("#TransferBatchPopupId").modal("hide");
        CoditechCommon.ShowLodder();
        $.ajax({
            type: "POST",
            url: "/DBTMGeneralBatchMaster/TransferBatch",
            data: {
                generalBatchMasterId: generalBatchMasterId,
                trainerId: trainerId
            },
            success: function (response) {
                if (response.success) {
                    location.reload();
                    return;
                }
                CoditechNotification.DisplayNotificationMessage(response.message,"error");
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
    },
}
$(document).ready(function () {
    DBTMBatch.Initialize();
    if ($("#GeneralBatchMasterId").val() == 0) {
        DBTMBatch.OnCentreChange();
    }
});
