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
        var isValid = true;
        $(".invalid-feedback").remove();
        $(".is-invalid").removeClass("is-invalid");
        var mobileNumber = $("#MobileNumber").val().trim();
        var callingCode = $("#CallingCode").val().trim();
        var emailId = $("#EmailId").val().trim();
        var joiningCode = $("#JoiningCode").val().trim();
        if (!sendOTPOn) {
            CoditechNotification.DisplayNotificationMessage("Invalid data send.", "error");
            return;
        }
        if (sendOTPOn === "mobile") {
            if (!callingCode) {
                isValid = false;
                $("#CallingCode")
                    .addClass("is-invalid")
                    .after('<div class="invalid-feedback">Please select calling code.</div>');
            } else if (!mobileNumber) {
                isValid = false;
                $("#MobileNumber")
                    .addClass("is-invalid")
                    .after('<div class="invalid-feedback">Please enter mobile number.</div>');
            }
            else if (mobileNumber.length < 10) {
                isValid = false;
                $("#MobileNumber")
                    .addClass("is-invalid")
                    .after('<div class="invalid-feedback">Mobile number must be 10 digits.</div>');
            }

        } else if (sendOTPOn === "email") {
            if (!emailId) {
                isValid = false;
                $("#EmailId")
                    .addClass("is-invalid")
                    .after('<div class="invalid-feedback">Please enter Email ID.</div>');
            } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(emailId)) {
                isValid = false;
                $("#EmailId")
                    .addClass("is-invalid")
                    .after('<div class="invalid-feedback">Please enter a valid Email ID.</div>');
            }
        } else {
            isValid = false;
            CoditechNotification.DisplayNotificationMessage("Unsupported method to send OTP.", "error");
        }
        if (!isValid) return;
        CoditechCommon.ShowLodder();
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

    },
    JoiningCodeListByCentreCodeList: function () {
        var selectedCentreCode = $("#SelectedCentreCode").val();
        var selectedParameter1 = $("#JoiningCodeTypeEnumId").val();
        if (selectedCentreCode !== "" && selectedParameter1 !== "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/OrganisationCentrewiseJoiningCode/List",
                data: {
                    "selectedCentreCode": selectedCentreCode,
                    "selectedParameter1": selectedParameter1
                },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#DataTablesDivId").html("").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status === 401 || xhr.status === 403) {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Joining Code list.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        } else {
            CoditechNotification.DisplayNotificationMessage("Please select Centre and Joining Code Type.", "error");
        }
    },

    GetOrganisationCentrewiseJoiningCodeListByCentreCode: function (listType) {
        $('#DataTables_SearchById').val("");
        var selectedCentreCode = $("#SelectedCentreCode").val();
        var selectedParameter1 = $("#JoiningCodeTypeEnumId").val();
        if (listType === "General") {
            CoditechDataTable.LoadList("OrganisationCentrewiseJoiningCode", "JoiningCodeListByCentreCodeList");
        } else {
            CoditechDataTable.LoadList("OrganisationCentrewiseJoiningCode", "List");
        }
    },
}
