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
        dBTMTestwisePerformanceStandardCategoryId,
        ageGroupEnumId,
        genderEnumId) {
        var row = $("#row_" + ageGroupEnumId + "_" + genderEnumId);
        var excellentValue = row.find(".excellent-value-input").val();
        var excellentScore = row.find(".excellent-score-input").val();
        var veryGoodValue = row.find(".verygood-value-input").val();
        var veryGoodScore = row.find(".verygood-score-input").val();
        var goodValue = row.find(".good-value-input").val();
        var goodScore = row.find(".good-score-input").val();
        var averageValue = row.find(".average-value-input").val();
        var averageScore = row.find(".average-score-input").val();
        var lowValue = row.find(".low-value-input").val();
        var lowScore = row.find(".low-score-input").val();
        var poorValue = row.find(".poor-value-input").val();
        var poorScore = row.find(".poor-score-input").val();
        CoditechCommon.ShowLodder();   
        $.ajax({
            type: "POST",
            url: "/DBTMTestMaster/SaveDBTMTestWisePerformanceStandard",
            data: {
                DBTMTestWisePerformanceStandardId: dBTMTestWisePerformanceStandardId,
                DBTMTestMasterId: dBTMTestMasterId,
                DBTMTestwisePerformanceStandardCategoryId: dBTMTestwisePerformanceStandardCategoryId,
                AgeGroupEnumId: ageGroupEnumId,
                GenderEnumId: genderEnumId,
                ExcellentValue: excellentValue,
                ExcellentScore: excellentScore,
                VeryGoodValue: veryGoodValue,
                VeryGoodScore: veryGoodScore,
                GoodValue: goodValue,
                GoodScore: goodScore,
                AverageValue: averageValue,
                AverageValueScore: averageScore,
                LowValue: lowValue,
                LowScore: lowScore,
                PoorValue: poorValue,
                PoorScore: poorScore
            },
            success: function (response) {
                CoditechCommon.HideLodder();
                if (response.success) {
                    var categoryId = $("#DBTMTestwisePerformanceStandardCategoryId").val();
                    var testId = $("#DBTMTestMasterId").val();
                    window.location.href =
                        "/DBTMTestMaster/DBTMTestWisePerformanceStandardList" +
                        "?dBTMTestMasterId=" + testId +
                        "&dBTMTestwisePerformanceStandardCategoryId=" + categoryId;
                }
            },
            error: function (xhr) {
                CoditechCommon.HideLodder();
                CoditechNotification.DisplayNotificationMessage("Error occured while saving record.", "error");
            }
        });
    },
    DBTMTestwisePerformanceStandardCategoryList: function () {
        var dBTMTestwisePerformanceStandardCategoryId = $("#DBTMTestwisePerformanceStandardCategoryId").val();
        var dBTMTestMasterId = $("#DBTMTestMasterId").val();
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