var DBTMOrganisationCentrewiseJoiningCode = {
    Initialize: function () {
        $("#custom1Div").hide();
        DBTMOrganisationCentrewiseJoiningCode.BindEvents();
        DBTMOrganisationCentrewiseJoiningCode.ToggleTrainerDropdown();
    },

    BindEvents: function () {
        $(document).on("change", "#JoiningCodeTypeEnumId", function () {
            DBTMOrganisationCentrewiseJoiningCode.ToggleTrainerDropdown();
        });

        $(document).on("DBTMTrainerListLoaded", function () {
            DBTMOrganisationCentrewiseJoiningCode.ToggleTrainerDropdown();
        });

        // Form submit validation
        $("form").on("submit", function (e) {
            if (!DBTMOrganisationCentrewiseJoiningCode.ValidateForm()) {
                e.preventDefault(); // stop form submit
            }
        });
    },

    ToggleTrainerDropdown: function () {
        var selectedText = ($("#JoiningCodeTypeEnumId option:selected").text() || "").trim().toLowerCase();

        if (selectedText.indexOf("trainee") !== -1) {
            $("#custom1Div").show();
        } else {
            $("#custom1Div").hide();
            $("#Custom1").val("").trigger("change");
        }
    },

    ValidateForm: function () {
        var selectedText = ($("#JoiningCodeTypeEnumId option:selected").text() || "").trim().toLowerCase();

        if (selectedText.indexOf("trainee") !== -1) {
            var trainerVal = $("#Custom1").val();
            if (!trainerVal) {
                // error message dikhado
                CoditechNotification.DisplayNotificationMessage("Please select a Trainer", "error");
                return false; // invalid
            }
        }
        return true; // valid
    },

    GetDBTMTrainerListByCentreCode: function () {
        var centreCode = $("#CentreCode").val();
        if (centreCode != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMDashboard/GetDBTMTrainerListByCentreCode",
                data: { "centreCode": centreCode },
                success: function (data) {
                    $("#Custom1").html("").html(data);
                    CoditechCommon.HideLodder();
                    $(document).trigger("DBTMTrainerListLoaded");
                },
                error: function () {
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Trainee Details List", "error");
                    CoditechCommon.HideLodder();
                    $(document).trigger("DBTMTrainerListLoaded");
                }
            });
        } else {
            $("#Custom1").html("");
            $(document).trigger("DBTMTrainerListLoaded");
        }
    }
};
