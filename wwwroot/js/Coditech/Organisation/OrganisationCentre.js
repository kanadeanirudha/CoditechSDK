var OrganisationCentre = {
    Initialize: function () {
        OrganisationCentre.constructor();
    },
    constructor: function () {
    },

    GetEmailTemplateByCentreCode: function (organisationCentreMasterId, templateType) {
        var selectedItem = $("#EmailTemplateCode").val();
        if (selectedItem != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/OrganisationCentreMaster/GetEmailTemplateByCentreCode",
                data: { "organisationCentreId": organisationCentreMasterId, "emailTemplateCode": selectedItem, "templateType": templateType },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#emailTemplateId").html("").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Email.", "error")
                    CoditechCommon.HideLodder();
                }
            });
        }
        else {
            $("#emailTemplateId").html("");
        }
    },
    GetSMSSettingByCentreCode: function (organisationCentreMasterId) {
        var selectedItem = $("#GeneralSmsProviderId").val();
        if (selectedItem != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/OrganisationCentreMaster/CentrewiseSmsSetup",
                data: { "organisationCentreId": organisationCentreMasterId, "generalSmsProviderId": selectedItem },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#smsProviderDivId").html("").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve SMS Setting.", "error")
                    CoditechCommon.HideLodder();
                }
            });
        }
        else {
            $("#smsProviderDivId").html("");
        }
    },

    GetWhatsAppSettingByCentreCode: function (organisationCentreMasterId) {
        var selectedItem = $("#GeneralWhatsAppProviderId").val();
        if (selectedItem != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/OrganisationCentreMaster/CentrewiseWhatsAppSetup",
                data: { "organisationCentreId": organisationCentreMasterId, "generalWhatsAppProviderId": selectedItem },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#whatsAppProviderDivId").html("").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve SMS Setting.", "error")
                    CoditechCommon.HideLodder();
                }
            });
        }
        else {
            $("#whatsAppProviderDivId").html("");
        }
    },
}