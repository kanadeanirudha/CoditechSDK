var DBTMTraineeAssignment = {
    Initialize: function () {
        DBTMTraineeAssignment.constructor();
    },

    constructor: function () {
    },

    GetDBTMTraineeListByCentreCodeAndTrainerMasterId: function () {
        var centreCode = $("#SelectedCentreCode").val();
        var generalTrainerId = $("#GeneralTrainerMasterId").val();

        if (centreCode != "" && generalTrainerId != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMTraineeAssignment/GetTraineeDetailByCentreCodeAndgeneralTrainerId",
                data: { "centreCode": centreCode, "generalTrainerId": generalTrainerId },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#DBTMTraineeDetailId").html("").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Trainee Details List", "error")
                    CoditechCommon.HideLodder();
                }
            });
        }
        else {
            $("#DBTMTraineeDetailId").html("");
        }

    },

    GetDBTMTrainerListByGeneralTrainerMasterId: function () {
        $('#DataTables_SearchById').val("")
        var centreCode = $("#SelectedCentreCode").val();
        var trainerId = $("#SelectedParameter1").val();
        if (!centreCode) {
            CoditechNotification.DisplayNotificationMessage("Please select centre.", "error");
            return;
        }
        if (trainerId === null || trainerId === "") {
            CoditechNotification.DisplayNotificationMessage("Please select trainer.", "error");
            return;
        }
        else {
            CoditechDataTable.LoadList("DBTMTraineeAssignment", "List");
        }
    },

    GetDBTMTrainerListByCentreCode: function () {
        var selectedItem = $("#SelectedCentreCode").val();
        if (selectedItem != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMTraineeAssignment/GetTrainerByCentreCode",
                data: { "centreCode": selectedItem },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#SelectedParameter1").html("").html(data);
                    $("#GeneralTrainerMasterId").html("").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve DBTM Trainer.", "error")
                    CoditechCommon.HideLodder();
                }
            });
        }
        else {
            $("#SelectedParameter1").html("");
        }
    },
}








