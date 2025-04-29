var OrganisationCentrewiseJoiningCode = {
    Initialize: function () {
        OrganisationCentrewiseJoiningCode.constructor();
    },

    constructor: function () { },

    GetOrganisationCentrewiseJoiningCodeSend: function (modelPopContentId, joiningCode) {
        CoditechCommon.ShowLodder();
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/OrganisationCentrewiseJoiningCode/GetOrganisationCentrewiseJoiningCodeSend",
            data: { joiningCode: joiningCode },
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                $('#' + modelPopContentId).html(result);
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

    SendJoiningCode: function (sendOTPOn) {
      
        var mobileNumber = $("#MobileNumber").val();
        var callingCode = $("#CallingCode").val();
        var emailId = $("#EmailId").val();
        var joiningCode = $("#JoiningCode").val();
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
                type: "Post",
                dataType: "json",
                url: "/OrganisationCentrewiseJoiningCode/GetOrganisationCentrewiseJoiningCodeSend?joiningCode=" + joiningCode + "&sendOTPOn=" + sendOTPOn + "&mobileNumber=" + mobileNumber + "&callingCode=" + callingCode + "&emailId=" + emailId,
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
                        let url = window.location.origin + window.location.pathname + '?selectedCentrecode=' + result.centreCode;
                        window.location.href = url;
                    }                  
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

};
   