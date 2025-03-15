var CoditechCommon = {
    Initialize: function () {
        CoditechCommon.constructor();
    },
    constructor: function () {
    },

    ShowLodder: function () {
        $('.spinner').css('display', 'block');
    },

    HideLodder: function () {
        $('.spinner').css('display', 'none');
    },

    LoadListByCentreCode: function (controllerName, methodName) {
        $('#DataTables_SearchById').val("")
        if ($("#SelectedCentreCode").val() == "") {
            CoditechNotification.DisplayNotificationMessage("Please select centre.", "error");
        }
        else {
            CoditechDataTable.LoadList(controllerName, methodName);
        }
    },
    GetDepartmentByCentreCode: function () {
        var selectedItem = $("#SelectedCentreCode").val();
        $('#DataTablesDivId tbody').html('');
        if (selectedItem != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/GeneralCommon/GetDepartmentsByCentreCode",
                data: { "centreCode": selectedItem },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#SelectedDepartmentId").html("").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Departments.", "error")
                    CoditechCommon.HideLodder();
                }
            });
        }
        else {
            $('#DataTablesDivId tbody').html('');
            $("#SelectedDepartmentId").html("");
        }
    },
    LoadListByCentreCodeAndDepartmentId: function (controllerName, methodName) {
        $('#DataTables_SearchById').val("")
        if ($("#SelectedCentreCode").val() == "") {
            CoditechNotification.DisplayNotificationMessage("Please select centre.", "error");
        }
        else if ($("#SelectedDepartmentId").val() == "") {
            CoditechNotification.DisplayNotificationMessage("Please select department.", "error");
        }
        else {
            CoditechDataTable.LoadList(controllerName, methodName);
        }
    },

    GetRegionListByCountryId: function () {
        var selectedItem = $("#GeneralCountryMasterId").val();
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
                    $("#GeneralRegionMasterId").html("").html(data);
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
            $("#GeneralRegionMasterId").html("");
        }
    },

    GetCityListByRegionId: function () {
        var selectedItem = $("#GeneralRegionMasterId").val();
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
                    $("#GeneralCityMasterId").html("").html(data);
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
            $("#GeneralRegionMasterId").html("");
        }
    },
    GetDistrictListByRegionId: function () {
        var selectedItem = $("#GeneralRegionMasterId").val();
        if (selectedItem != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/GeneralCommon/GetDistrictListByRegionId",
                data: { "generalRegionMasterId": selectedItem },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#GeneralDistrictMasterId").html("").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve District.", "error")
                    CoditechCommon.HideLodder();
                }
            });
        }
        else {
            $("#GeneralDistrictMasterId").html("");
        }
    },
    GetTermsAndCondition: function (modelPopContentId) {
        CoditechCommon.ShowLodder(); // Show the loader
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/GeneralCommon/GetTermsAndCondition",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                // Populate the modal content with the fetched data
                $('#' + modelPopContentId).html(result);
                CoditechCommon.HideLodder();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                if (xhr.status === 401 || xhr.status === 403) {
                    location.reload(); // Reload page for unauthorized errors
                }
                CoditechNotification.DisplayNotificationMessage("Failed to display record.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },

    AcceptedTermsAndConditions: function () {
        $("#IsTermsAndCondition").prop("checked", true);
    },

    SendOTP: function (sendOTPOn) {
        var mobileNumber = $("#MobileNumber").val();
        var callingCode = $("#CallingCode").val();
        var emailId = $("#EmailId").val();
        if (sendOTPOn == "") {
            CoditechNotification.DisplayNotificationMessage("Invalid data send.", "error");
        }
        else if (sendOTPOn == "mobile" && $("#CallingCode").val() == "") {
            CoditechNotification.DisplayNotificationMessage("Please select calling code.", "error");
        }
        else if (sendOTPOn == "mobile" && $("#MobileNumber").val() == "") {
            CoditechNotification.DisplayNotificationMessage("Please enter mobile number.", "error");
        }
        else if (sendOTPOn == "email" && $("#EmailId").val() == "") {
            CoditechNotification.DisplayNotificationMessage("Please enter EmailId.", "error");
        }
        else {
            CoditechCommon.ShowLodder(); // Show the loader
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "json",
                url: "/GeneralCommon/SendOTP?sendOTPOn=" + sendOTPOn + "&mobileNumber=" + mobileNumber + "&callingCode=" + callingCode + "&emailId=" + emailId,
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    if (result.success == true) {
                        if (sendOTPOn == "mobile") {
                            $("#MobileNumberSendOTPDivId").hide();
                            $("#MobileNumberTokenDivId").show();
                        }
                        else if (sendOTPOn == "email") {
                            $("#EmailIdSendOTPDivId").hide();
                            $("#EmailIdTokenDivId").show();
                        }
                        CoditechNotification.DisplayNotificationMessage(result.message, "success");
                    }
                    else {
                        CoditechNotification.DisplayNotificationMessage(result.message, "error");
                    }
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status === 401 || xhr.status === 403) {
                        location.reload(); // Reload page for unauthorized errors
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to display record.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        }
    },

    VerifySendOTP: function (sendOTPOn) {
        var token = "";
        if (sendOTPOn == "") {
            CoditechNotification.DisplayNotificationMessage("Invalid data send.", "error");
        }
        else if (sendOTPOn == "mobile" && $("#MobileNumberToken").val() != "") {
            token = $("#MobileNumberToken").val();
        }
        else if (sendOTPOn == "email" && $("#EmailIdToken").val() != "") {
            token = $("#EmailIdToken").val();
        }
        else {
            CoditechNotification.DisplayNotificationMessage("Please enter OTP.", "error");
        }
        if (token != "") {
            CoditechCommon.ShowLodder(); // Show the loader
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "json",
                url: "/GeneralCommon/VerifySendOTP?sendOTPOn=" + sendOTPOn + "&otp=" + token,
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    if (result.success == true) {
                        if (sendOTPOn == "mobile") {
                            $("#MobileNumberSendOTPDivId").hide();
                            $("#MobileNumberTokenDivId").hide();
                            $("#IsMobileNumberVerifed").prop("checked", true);
                            $("#MobileNumberVerifiedOTPDivId").show();
                        }
                        else if (sendOTPOn == "email") {
                            $("#EmailIdSendOTPDivId").hide();
                            $("#EmailIdTokenDivId").hide();
                            $("#IsEmailIdVerifed").prop("checked", true);
                            $("#EmailIdVerifiedOTPDivId").show();
                        }
                        CoditechNotification.DisplayNotificationMessage(result.message, "success");
                    }
                    else {
                        CoditechNotification.DisplayNotificationMessage(result.message, "error");
                    }
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status === 401 || xhr.status === 403) {
                        location.reload(); // Reload page for unauthorized errors
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to display record.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        }
    },
    ValidNumeric: function () {
        var charCode = (event.which) ? event.which : event.keyCode;
        if (charCode >= 48 && charCode <= 57) { return true; }
        else { return false; }
    },

    ValidDecimalNumeric: function () {
        var charCode = (event.which) ? event.which : event.keyCode;
        if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
            return false;

        return true;
    },

    AvoidSpacing: function () {
        var charCode = (event.which) ? event.which : event.keyCode;
        if (charCode != 32) {
            return true;
        }
        else {
            return false;
        }
    },
    AllowOnlyAlphabetWithouSpacing: function () {
        const charCode = event.which || event.keyCode;
        if ((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122) || charCode === 8) {
            return true;
        }
        return false;
    },

    SearchDatatableData: function () {
        var charCode = (event.which) ? event.which : event.keyCode;
        if (charCode == 13) {
            $("#DataTables_SearchButton").click();
        }
        else {
            return false;
        }
    },
    LoadListByBalanceSheet: function (controllerName, methodName) {
        $('#DataTables_SearchById').val("")
        if ($("#SelectedParameter1").val() == "") {
            CoditechNotification.DisplayNotificationMessage("Please select centre.", "error");
        }
        else {
            CoditechDataTable.LoadList(controllerName, methodName);
        }
    },

    DashboardDays: function () {
        CoditechCommon.ShowLodder();
        var numberOfDaysRecord = $("#DashboardDaysDropDown option:selected").val();
        let url = window.location.origin + window.location.pathname.replace("index", "GetDashboard") + '?numberOfDaysRecord=' + numberOfDaysRecord;
        window.location.href = url;
        CoditechCommon.HideLodder();
    },
}
