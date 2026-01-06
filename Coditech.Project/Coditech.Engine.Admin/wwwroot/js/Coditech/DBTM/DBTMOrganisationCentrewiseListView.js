var DBTMOrganisationCentrewiseListView = {

    GetActivityListViewPopup: function (contentId, sequenceId, testName, centreCode) {
        $("#" + contentId).html("");
        CoditechCommon.ShowLodder();
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/DBTMOrganisationCentreMaster/GetActivityListViewEditPopup",
            data: { dBTMTestParameterListViewSequenceId: sequenceId, testName: testName, centreCode: centreCode },
            success: function (result) {
                $("#" + contentId).html(result);
                CoditechCommon.HideLodder();
            },
            error: function () {
                CoditechNotification.DisplayNotificationMessage(
                    "Failed to load popup.",
                    "error"
                );
                CoditechCommon.HideLodder();
            }
        });
    },

    ConfirmActivityListViewUpdate: function () {
        var formData = $("#ActivityListViewEditForm").serialize();
        CoditechCommon.ShowLodder();
        $.ajax({
            cache: false,
            type: "POST",
            url: "/DBTMOrganisationCentreMaster/GetActivityListViewEditPopup",
            data: formData,
            success: function (result) {
                if (result.success === true) {
                    $("#ActivityListViewPopupId").modal("hide");
                    location.reload();
                } else {
                    CoditechNotification.DisplayNotificationMessage(
                        "Update failed.",
                        "error"
                    );
                }
            },
            complete: function () {
                CoditechCommon.HideLodder();
            }
        });
    },
};
