var DBTMTestWisePerformanceStandard = {
    Initialize: function () {
        DBTMTestWisePerformanceStandard.constructor();
    },
    constructor: function () {
    },
    EditRow: function (ageGroupEnumId, genderEnumId) {
        var row = $("#row_" + ageGroupEnumId + "_" + genderEnumId);
        row.find(".view-mode").hide();
        row.find(".edit-mode").show();
        row.find(".edit-btn").hide();
        row.find(".save-btn").show();
        row.find(".cancel-btn").show();
    },
    CancelRow: function (ageGroupEnumId, genderEnumId) {
        var row = $("#row_" + ageGroupEnumId + "_" + genderEnumId);
        row.find(".view-mode").show();
        row.find(".edit-mode").hide();
        row.find(".edit-btn").show();
        row.find(".save-btn").hide();
        row.find(".cancel-btn").hide();
    },
    SaveRow: function (
        dBTMTestWisePerformanceStandardId,
        dBTMTestMasterId,
        ageGroupEnumId,
        genderEnumId) {
        var row = $("#row_" + ageGroupEnumId + "_" + genderEnumId);
        var excellentValue = row.find(".excellent-input").val();
        var goodValue = row.find(".good-input").val();
        var averageValue = row.find(".average-input").val();
        var poorValue = row.find(".poor-input").val();
        CoditechCommon.ShowLodder();
        $.ajax({
            type: "POST",
            url: "/DBTMTestMaster/SaveDBTMTestWisePerformanceStandard",
            data: {
                DBTMTestWisePerformanceStandardId: dBTMTestWisePerformanceStandardId,
                DBTMTestMasterId: dBTMTestMasterId,
                AgeGroupEnumId: ageGroupEnumId,
                GenderEnumId: genderEnumId,
                ExcellentValue: excellentValue,
                GoodValue: goodValue,
                AverageValue: averageValue,
                PoorValue: poorValue
            },
            success: function (response) {
                CoditechCommon.HideLodder();
                if (response.success) {
                    location.reload();
                }
                else {
                    CoditechNotification.DisplayNotificationMessage("Failed to save record.", "error");
                }
            },
            error: function (xhr) {
                CoditechCommon.HideLodder();
                CoditechNotification.DisplayNotificationMessage("Error occured while saving record.", "error");
            }
        });
    },
   DBTMTestwisePerformanceStandardCategoryList = function () {

        var dBTMTestwisePerformanceStandardCategoryId = $("#DBTMTestwisePerformanceStandardCategoryId").val();
        // var dBTMTestMasterId = $("#DBTMTestMasterId").val();

        if (dBTMTestwisePerformanceStandardCategoryId) {

            CoditechCommon.ShowLodder();

            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMTestMaster/DBTMTestWisePerformanceStandardList",
                data: {
                    dBTMTestMasterId: dBTMTestMasterId,
                    dBTMTestwisePerformanceStandardCategoryId: dBTMTestwisePerformanceStandardCategoryId
                },
                success: function (data) {

                    $("#DataTablesDivId").html(data);

                    CoditechCommon.HideLodder();
                },
                error: function (xhr) {

                    if (xhr.status == 401 || xhr.status == 403) {
                        location.reload();
                    }

                    CoditechNotification.DisplayNotificationMessage(
                        "Failed to retrieve Performance Standard List",
                        "error"
                    );

                    CoditechCommon.HideLodder();
                }
            });

        }
    },
};