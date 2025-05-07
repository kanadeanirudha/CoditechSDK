var DBTMTraineeAssignment = {
    Initialize: function () {
        DBTMTraineeAssignment.constructor();
    },
    constructor: function () {
    },

    GetAssociateUnAssociateAssignmentwiseUser: function (modelPopContentId, dBTMTraineeAssignmentUserId, dBTMTraineeAssignmentId, testName, firstName, lastName, dBTMTraineeDetailId) {

        let dBTMTraineeAssignmentToUserViewModel = {
            DBTMTraineeAssignmentUserId: dBTMTraineeAssignmentUserId,
            DBTMTraineeAssignmentId: dBTMTraineeAssignmentId,
            TestName: testName,
            FirstName: firstName,
            LastName:lastName,
            DBTMTraineeDetailId: dBTMTraineeDetailId
            
        };
        CoditechCommon.ShowLodder();
        $.ajax(
            {
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMTraineeAssignment/GetAssociateUnAssociateAssignmentwiseUser",
                data: dBTMTraineeAssignmentToUserViewModel,
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    $('#' + modelPopContentId).html("").html(result);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to display record.", "error");
                    CoditechCommon.HideLodder();
                }
            });
    },
    AssociateUnAssociateAssignmentwiseUser: function () {
        $("#frmAssociateUnAssociateAssignmentwiseUser").submit();
    },
}
