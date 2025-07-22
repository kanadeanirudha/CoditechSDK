var OrganisationTestModel = {
    Initialize: function () {
        OrganisationCentrewiseJoiningCode.constructor();
    },

    constructor: function () { },

    SendTestEmailModel: function (modelPopContentId, centreCode, organisationCentreMasterId) {
        CoditechCommon.ShowLodder();
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/OrganisationCentreMaster/SendTestEmailModel",
            data: {
                centreCode: centreCode,
                organisationCentreMasterId: organisationCentreMasterId
            },
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                $('#' + modelPopContentId).html(result); // Inject HTML into modal content container
                CoditechCommon.HideLodder();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                if (xhr.status === 401 || xhr.status === 403) {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Failed to load details.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },

    SendWhatsAppTestModel: function (modelPopContentId, centreCode, organisationCentreMasterId) {
        CoditechCommon.ShowLodder();
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/OrganisationCentreMaster/SendWhatsAppTestModel",
            data: {
                centreCode: centreCode,
                organisationCentreMasterId: organisationCentreMasterId
            },
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                $('#' + modelPopContentId).html(result); // Inject HTML into modal content container
                CoditechCommon.HideLodder();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                if (xhr.status === 401 || xhr.status === 403) {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Failed to load details.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },
    SendSmsTestModel: function (modelPopContentId, centreCode, organisationCentreMasterId) {
        CoditechCommon.ShowLodder();
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/OrganisationCentreMaster/SendSmsTestModel",
            data: {
                centreCode: centreCode,
                organisationCentreMasterId: organisationCentreMasterId
            },
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                $('#' + modelPopContentId).html(result); // Inject HTML into modal content container
                CoditechCommon.HideLodder();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                if (xhr.status === 401 || xhr.status === 403) {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Failed to load details.", "error");
                CoditechCommon.HideLodder();
            }
        });
    }
};
