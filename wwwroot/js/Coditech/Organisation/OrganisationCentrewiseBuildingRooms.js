var OrganisationCentrewiseBuildingRooms = {
    Initialize: function () {
        OrganisationCentrewiseBuildingRooms.constructor();
    },

    constructor: function () {
    },

    GetBuildingRoomsByBuildingMasterId: function () {
        $('#DataTables_SearchById').val("")
        if ($("#SelectedCentreCode").val() == "") {
            CoditechNotification.DisplayNotificationMessage("Please select centre.", "error");
        }
        else if ($("#SelectedParameter1").val() == "") {
            CoditechNotification.DisplayNotificationMessage("Please select building.", "error");
        }
        else {
            CoditechDataTable.LoadList("OrganisationCentrewiseBuildingRooms", "List");
        }
    },

    GetOrganisationCentrewiseBuildingByCentreCode: function () {
        var selectedItem = $("#SelectedCentreCode").val();
        if (selectedItem != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/OrganisationCentrewiseBuildingRooms/GetOrganisationCentrewiseBuildingByCentreCode",
                data: { "centreCode": selectedItem },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#SelectedParameter1").html("").html(data);
                    $("#OrganisationCentrewiseBuildingMasterId").html("").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Building Name.", "error")
                    CoditechCommon.HideLodder();
                }
            });
        }
        else {
            $("#SelectedParameter1").html("");
        }
    },
}
