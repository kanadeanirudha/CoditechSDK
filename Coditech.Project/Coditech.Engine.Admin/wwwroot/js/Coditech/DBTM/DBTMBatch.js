var DBTMBatch = {
    Initialize: function () {
        DBTMBatch.constructor();
    },
    constructor: function () {
    },
    OnCentreChange: function () {
        var centreCode = $("#CentreCode").val();
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
            data: { centreCode: centreCode },
            success: function (data) {
                $("#ActivityDropdownDiv").html(data);
                $("#ActivityDropdownDiv .selectpicker").selectpicker();
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
