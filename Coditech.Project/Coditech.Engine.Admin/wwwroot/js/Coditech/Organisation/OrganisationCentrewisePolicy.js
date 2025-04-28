var OrganisationCentrewisePolicy = {
    Initialize: function () {
        OrganisationCentrewisePolicy.constructor();
    },
    constructor: function () {
    },
    LoadListByCentreCode: function (controllerName, methodName) {
        var centreCode = $("#CentreCode").val();
        if (centreCode === "") {
            CoditechNotification.DisplayNotificationMessage("Please select centre.", "error");
        } else {
            $.ajax({
                type: "GET",
                url: "/" + controllerName + "/" + methodName,
                data: { centreCode: centreCode },
                success: function (result) {
                    $("#DataTablesDivId").html(result);
                },
                error: function () {
                    CoditechNotification.DisplayNotificationMessage("Failed to load list.", "error");
                }
            });
        }
    },
    GetCentrewisePolicyDetails: function (modelPopContentId, generalPolicyRulesId) {
        var centreCode = $("#CentreCode").val();
        var centreName = $("#CentreCode option:selected").text();
        CoditechCommon.ShowLodder();
        $.ajax(
            {
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/OrganisationCentrewisePolicy/GetCentrewisePolicyDetails",
                data: {
                    centreCode: centreCode,
                    generalPolicyRulesId: generalPolicyRulesId
                },
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
    CentrewisePolicy: function () {
        $("#frmCentrewisePolicy").submit();
    },
}
