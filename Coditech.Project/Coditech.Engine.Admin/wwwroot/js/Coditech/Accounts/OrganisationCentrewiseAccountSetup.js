var OrganisationCentrewiseAccountSetup = {
    Initialize: function () {
        OrganisationCentrewiseAccountSetup.constructor();
    },
    constructor: function () {
    },

    GetOrganisationCentrewiseAccountSetup: function () {
        var selectedCentreCode = $("#CentreCode").val();
        if (selectedCentreCode != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/OrganisationCentrewiseAccountSetup/GetOrganisationCentrewiseAccountSetup",
                data: { "centreCode": selectedCentreCode },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#OrganisationCentrewiseAccountSetupDivId").html("").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Currency.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        }
        else {
            $("#OrganisationCentrewiseAccountSetupDivId").html("");
        }
    }
};
