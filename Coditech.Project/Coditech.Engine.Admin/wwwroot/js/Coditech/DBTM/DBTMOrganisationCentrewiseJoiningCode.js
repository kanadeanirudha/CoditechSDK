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
        if ($("#JoiningCodeTypeEnumId").val() == "324") {
            $("#custom1Div").show();
            $("#custom3Div").show();
        }
        else {
            $("#custom1Div").hide();
            $("#custom3Div").hide();
        }
    },

    ValidateForm: function () {
        if ($("#JoiningCodeTypeEnumId").val() == "324") {
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
                data: {
                    "centreCode": centreCode,
                    selectedTrainerId: $("#SelectedTrainer").val()
                },
                success: function (data) {
                    var selectedTrainer = $("#SelectedTrainer").val();
                    $("#Custom1").html(data);
                    if (selectedTrainer) {
                        $("#Custom1").val(selectedTrainer).trigger("change");
                    }
                    CoditechCommon.HideLodder();
                    $(document).trigger("DBTMTrainerListLoaded");
                },
                error: function (xhr) {
                    if (xhr.status == 401 || xhr.status == 403) {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Trainee Details List", "error");
                    CoditechCommon.HideLodder();
                    $(document).trigger("DBTMTrainerListLoaded");
                }
            });
        } else {
            $("#Custom1").html("");
            $(document).trigger("DBTMTrainerListLoaded");
        }
    },
    GetDBTMBatchListByCentreCodeAndTrainer: function () {

        var centreCode = $("#CentreCode").val();
        var generalTrainerMasterId = $("#Custom1").val(); // trainer dropdown
        var joiningCodeTypeEnumId = $("#JoiningCodeTypeEnumId").val(); // joining type

        if (centreCode && generalTrainerMasterId && joiningCodeTypeEnumId) {

            CoditechCommon.ShowLodder();

            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMDashboard/GetDBTMBatchListByCentreCodeAndTrainer",
                data: {
                    centreCode: centreCode,
                    generalTrainerMasterId: generalTrainerMasterId,
                    joiningCodeTypeEnumId: joiningCodeTypeEnumId
                },
                success: function (data) {
                    $("#Custom3").html("").html(data); // batch dropdown
                    $("#custom3Div").show(); // optional
                    CoditechCommon.HideLodder();
                },
                error: function (xhr) {
                    if (xhr.status == 401 || xhr.status == 403) {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Batch List", "error");
                    CoditechCommon.HideLodder();
                }
            });

        } else {
            $("#Custom3").html(""); // reset batch
            $("#custom3Div").hide();
        }
    },
    DownloadTraineeJoiningCode: function () {
        var centreCode = $("#SelectedCentreCode").val();
        if (!centreCode) {
            CoditechNotification.DisplayNotificationMessage("Please select Centre.", "error");
            return;
        }
        CoditechCommon.ShowLodder();
        var downloadUrl = "/DBTMOrganisationCentrewiseJoiningCode/DownloadTraineeJoiningCode"
            + "?centreCode=" + encodeURIComponent(centreCode);
        $("#hiddenDownloader").attr("src", downloadUrl);
        setTimeout(function () {
            CoditechCommon.HideLodder();
        }, 1500);
    },
};