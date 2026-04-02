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
}
$(document).ready(function () {
    DBTMBatch.Initialize();
    if ($("#GeneralBatchMasterId").val() == 0) {
        DBTMBatch.OnCentreChange();
    }
    //DBTMBatch.Initialize();
    //if ($("#CentreCode").val()) {
    //    DBTMBatch.OnCentreChange();
    //}
});
