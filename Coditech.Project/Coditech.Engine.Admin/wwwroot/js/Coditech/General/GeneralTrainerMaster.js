var GeneralTrainerMaster = {
    Initialize: function () {
        GeneralTrainerMaster.constructor();
    },
    constructor: function () {
    },
    GetEmployeeListByCentreCodeAndDepartmentId: function () {
        var selectedCentreCode = $("#SelectedCentreCode").val();
        var selectedDepartmentId = $("#SelectedDepartmentId").val();

        if (selectedCentreCode != "" && selectedDepartmentId != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/GeneralTrainerMaster/GetEmployeeList",
                data: { "selectedCentreCode": selectedCentreCode, "selectedDepartmentId": selectedDepartmentId },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#EmployeeId").html("").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Employee List", "error")
                    CoditechCommon.HideLodder();
                }
            });

        }
    },
    GetAssociateUnAssociateTrainer: function (modelPopContentId, assocId, traineeId, personId, firstName, lastName, trainerMasterId) {
        let model = {
            GeneralTraineeAssociatedToTrainerId: assocId,   
            GeneralTrainerMasterId: trainerMasterId,      
            DBTMTraineeDetailId: traineeId,
            PersonId: personId, 
            FirstName: firstName,
            LastName: lastName
        };
        CoditechCommon.ShowLodder();
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/DBTMTraineeDetails/GetAssociateUnAssociateTrainer",
            data: model,
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                $('#' + modelPopContentId).html("").html(result);
                CoditechCommon.HideLodder();
            },
            error: function () {
                CoditechNotification.DisplayNotificationMessage("Failed to load.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },
    AssociateUnAssociateTrainer: function () {
        $("#frmAssociateUnAssociateTrainer").submit();
    },
}
