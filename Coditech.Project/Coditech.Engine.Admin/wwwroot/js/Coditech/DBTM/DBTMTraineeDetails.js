var DBTMTraineeDetails = {
    Initialize: function () {
        DBTMTraineeDetails.constructor();
    },

    constructor: function () {
    },

    GetTrainerListByCentreCodeAndDepartmentId: function () {

        var selectedCentreCode = $("#SelectedCentreCode").val();
        var selectedDepartmentId = $("#SelectedDepartmentId").val();
        var entityId = $("#EntityId").val();

        if (selectedCentreCode !== "" && selectedDepartmentId !== "" && entityId !== "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMTraineeDetails/GetTrainerList",
                data: {
                    selectedCentreCode: selectedCentreCode,
                    selectedDepartmentId: selectedDepartmentId,
                    entityId: entityId

                },
                contentType: "application/json; charset=utf-8",
                success: function(data) {
                    $("#GeneralTrainerMasterId").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Trainer List", "error");
                    CoditechCommon.HideLodder();
                }
            });
        } else {
            $("#GeneralTrainerMasterId").html("");
        }
    },










}








