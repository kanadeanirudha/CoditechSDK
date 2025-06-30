var DBTMDashboard = {
    Initialize: function () {
        DBTMDashboard.constructor();
    },

    constructor: function () {
        $(document).on("click", ".send-reminder-btn", function () {
            var dBTMTraineeAssignmentId = $(this).data("assignment-id");
            var dBTMTraineeAssignmentUserId = $(this).data("user-id");

            DBTMDashboard.SendReminder(dBTMTraineeAssignmentId, dBTMTraineeAssignmentUserId);
        });
    },

    SendReminder: function (dBTMTraineeAssignmentId, dBTMTraineeAssignmentUserId) {
        if (!dBTMTraineeAssignmentId) {
            CoditechNotification.DisplayNotificationMessage("Invalid data sent.", "error");
            return;
        }
        CoditechCommon.ShowLodder();
        $.ajax({
            type: "POST",
            url: "/DBTMDashboard/SendAssignmentReminder",
            data: {
                dBTMTraineeAssignmentId: dBTMTraineeAssignmentId,
                dBTMTraineeAssignmentUserId: dBTMTraineeAssignmentUserId
            },
            success: function (result) {
                CoditechCommon.HideLodder();
                if (result.success == true) {
                    CoditechNotification.DisplayNotificationMessage("success");
                } else {
                    CoditechNotification.DisplayNotificationMessage("error");
                }
                let url = window.location.origin + window.location.pathname;
                window.location.href = url;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                CoditechCommon.HideLodder();
                if (xhr.status === 401 || xhr.status === 403) {
                    location.reload();
                } else {
                    CoditechNotification.DisplayNotificationMessage("Something went wrong while sending reminder.", "error");
                }
            }
        });
    }
};

$(document).ready(function () {
    DBTMDashboard.Initialize();
});
