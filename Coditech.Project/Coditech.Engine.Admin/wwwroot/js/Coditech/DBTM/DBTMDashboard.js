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
    },

    OpenTrainerDetail: function (designation, modelPopContentId, numberOfDaysRecord, generalTrainerMasterId, adminRoleMasterId, userMasterId) {
        if (designation.trim().toLowerCase() !== "trainer") {
            CoditechNotification.DisplayNotificationMessage("Details available only for Trainers.", "error");
            return;
        }

        DBTMDashboard.GetTrainerDashboard(
            modelPopContentId,
            numberOfDaysRecord,
            generalTrainerMasterId,
            adminRoleMasterId,
            userMasterId
        );
    },

    GetTrainerDashboard: function (modelPopContentId, numberOfDaysRecord, generalTrainerMasterId, adminRoleMasterId, userMasterId) {
        CoditechCommon.ShowLodder();

        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/DBTMDashboard/GetTrainerDashBoard",
            data: {
                numberOfDaysRecord: numberOfDaysRecord,
                generalTrainerMasterId: generalTrainerMasterId,
                adminRoleMasterId: adminRoleMasterId,
                userMasterId: userMasterId
            },
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                $('#' + modelPopContentId).html(result);

                var modalId = $('#' + modelPopContentId).closest('.modal').attr('id');
                var myModal = new bootstrap.Modal(document.getElementById(modalId));
                myModal.show();

                CoditechCommon.HideLodder();
            },
            error: function (xhr) {
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Failed to load details.", "error");
                CoditechCommon.HideLodder();
            }
        });
    }
};

$(document).ready(function () {
    DBTMDashboard.Initialize();
});
window.addEventListener("pageshow", function (event) {
    if (event.persisted) {
        location.reload();
    }
});