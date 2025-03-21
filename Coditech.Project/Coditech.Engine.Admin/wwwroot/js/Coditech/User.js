var User = {
    Initialize: function () {
        User.constructor();
    },
    constructor: function () {
    },


    GetRegionListByCountryId: function (addressType) {
        var selectedItem = $(".GeneralCountryMasterId_" + addressType).val();
        if (selectedItem != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/GeneralCommon/GetRegionListByCountryId",
                data: { "generalCountryMasterId": selectedItem },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $(".GeneralRegionMasterId_" + addressType).html("").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Region.", "error")
                    CoditechCommon.HideLodder();
                }
            });
        }
        else {
            $(".GeneralRegionMasterId_" + addressType).html("");
        }
    },

    GetCityListByRegionId: function (addressType) {
        var selectedItem = $(".GeneralRegionMasterId_" + addressType).val();
        if (selectedItem != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/GeneralCommon/GetCityListByRegionId",
                data: { "generalRegionMasterId": selectedItem },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $(".GeneralCityMasterId_" + addressType).html("").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve City.", "error")
                    CoditechCommon.HideLodder();
                }
            });
        }
        else {
            $(".GeneralRegionMasterId_" + addressType).html("");
        }
    },

    GetGeneralPersonAddressess: function (personId, entityId, entityType, controllerName) {
        CoditechCommon.ShowLodder();
        $.ajax(
            {
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/User/GetGeneralPersonAddressess",
                data: { "personId": personId, "entityId": entityId, "entityType": entityType, "controllerName": controllerName },
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    $('#generalPersonAddressDivId').html("").html(result);
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

    SaveGeneralPersonalAddress: function (addressType) {
        $("#frmGeneralPersonalAddress_" + addressType).validate();
        $("#errorGeneralCountryMasterId_" + addressType).text('').text("").removeClass("field-validation-error").hide();
        $("#errorGeneralRegionMasterId_" + addressType).text('').text("").removeClass("field-validation-error").hide();
        $("#errorGeneralCityMasterId_" + addressType).text('').text("").removeClass("field-validation-error").hide();
        if ($("#frmGeneralPersonalAddress_" + addressType).valid()) {
            var generalCountryMasterId = $(".GeneralCountryMasterId_" + addressType).val();
            if (generalCountryMasterId == "" || generalCountryMasterId.includes("select") || generalCountryMasterId.includes("Select")) {
                $("#errorGeneralCountryMasterId_" + addressType).text('').text("Please Select Country.").addClass("field-validation-error").show();
                return false;
            }

            var generalRegionMasterId = $(".GeneralRegionMasterId_" + addressType).val();
            if (generalRegionMasterId == "" || generalRegionMasterId.includes("select") || generalRegionMasterId.includes("Select")) {
                $("#errorGeneralRegionMasterId_" + addressType).text('').text("Please Select Region.").addClass("field-validation-error").show();
                return false;
            }

            var generalCityMasterId = $(".GeneralCityMasterId_" + addressType).val();
            if (generalCityMasterId == "" || generalCityMasterId.includes("select") || generalCityMasterId.includes("Select")) {
                $("#errorGeneralCityMasterId_" + addressType).text('').text("Please Select City.").addClass("field-validation-error").show();
                return false;
            }
            $("#frmGeneralPersonalAddress_" + addressType).submit();
        }
    },
}
