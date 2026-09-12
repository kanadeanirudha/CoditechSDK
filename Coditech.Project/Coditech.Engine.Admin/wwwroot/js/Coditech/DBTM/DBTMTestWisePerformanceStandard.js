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
    SaveRow: function (ageGroupEnumId, genderEnumId) {
        var row = $("#row_" + ageGroupEnumId + "_" + genderEnumId);
        var testMasterId = $("#DBTMTestMasterId").val();
        var categoryId = $("#DBTMTestwisePerformanceStandardCategoryId").val();
        var performanceStandards = [];
        var groupedInputs = {};
        row.find(".performance-input").each(function () {
            var input = $(this);
            var performanceType = input.data("performance-type");
            var fieldType = input.data("field-type");
            if (!groupedInputs[performanceType]) {
                groupedInputs[performanceType] = {
                    DBTMTestWisePerformanceStandardConfigurationId: input.data("configuration-id"),
                    AgeGroupEnumId: ageGroupEnumId,
                    GenderEnumId: genderEnumId,
                    PerformanceStandardType: performanceType,
                    PerformanceStandardTypeValue: "",
                    PerformanceStandardTypeScore: ""
                };
            }
            var rangeType = input.data("range-type");
            if (fieldType === "value") {
                if (rangeType === "min") {
                    groupedInputs[performanceType].ValueMin = input.val();
                }
                else if (rangeType === "max") {
                    groupedInputs[performanceType].ValueMax = input.val();
                }
            }
            if (fieldType === "score") {
                if (rangeType === "min") {
                    groupedInputs[performanceType].ScoreMin = input.val();
                }
                else if (rangeType === "max") {
                    groupedInputs[performanceType].ScoreMax = input.val();
                }
            }
        });
        $.each(groupedInputs, function (key, item) {
            var valueMin = item.ValueMin || "";
            var valueMax = item.ValueMax || "";
            var scoreMin = item.ScoreMin || "";
            var scoreMax = item.ScoreMax || "";
            if (valueMin === "" && valueMax === "" && scoreMin === "" && scoreMax === "") {
                return;
            }
            performanceStandards.push({
                DBTMTestWisePerformanceStandardConfigurationId: item.DBTMTestWisePerformanceStandardConfigurationId,
                AgeGroupEnumId: item.AgeGroupEnumId,
                GenderEnumId: item.GenderEnumId,
                PerformanceStandardType: item.PerformanceStandardType,
                PerformanceStandardTypeValue: valueMin + "-" + valueMax,
                PerformanceStandardTypeScore: scoreMin + "-" + scoreMax
            });
        });
        var formData = {
            DBTMTestMasterId: parseInt(testMasterId),
            DBTMTestwisePerformanceStandardCategoryId: parseInt(categoryId),
            AgeGroupEnumId: parseInt(ageGroupEnumId),
            GenderEnumId: parseInt(genderEnumId)
        };
        $.each(performanceStandards, function (index, item) {
            formData["PerformanceStandards[" + index + "].DBTMTestWisePerformanceStandardConfigurationId"] = item.DBTMTestWisePerformanceStandardConfigurationId;
            formData["PerformanceStandards[" + index + "].AgeGroupEnumId"] = item.AgeGroupEnumId;
            formData["PerformanceStandards[" + index + "].GenderEnumId"] = item.GenderEnumId;
            formData["PerformanceStandards[" + index + "].PerformanceStandardType"] = item.PerformanceStandardType;
            formData["PerformanceStandards[" + index + "].PerformanceStandardTypeValue"] = item.PerformanceStandardTypeValue;
            formData["PerformanceStandards[" + index + "].PerformanceStandardTypeScore"] = item.PerformanceStandardTypeScore;
        });
        CoditechCommon.ShowLodder();
        $.ajax({
            type: "POST",
            url: "/DBTMTestMaster/SaveDBTMTestWisePerformanceStandard",
            data: formData,
            success: function (response) {
                if (response.success) {
                    var categoryId = $("#DBTMTestwisePerformanceStandardCategoryId").val();
                    var testId = $("#DBTMTestMasterId").val();
                    window.location.href =
                        "/DBTMTestMaster/DBTMTestWisePerformanceStandardList" +
                        "?dBTMTestMasterId=" + testId +
                        "&dBTMTestwisePerformanceStandardCategoryId=" + categoryId;
                    DBTMTestWisePerformanceStandard.DBTMTestwisePerformanceStandardCategoryList();
                }
            }, 
            error: function (xhr) {
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Error occured while saving record.", "error");
            },
            complete: function () {
                CoditechCommon.HideLodder();
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
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Performance Standard List", "error");
                    CoditechCommon.HideLodder();
                }
            });
        }
    },
};