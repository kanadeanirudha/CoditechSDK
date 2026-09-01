var DBTMTestWisePerformanceStandardConfiguration = {
    Initialize: function () {
        DBTMTestWisePerformanceStandardConfiguration.constructor();
    },
    constructor: function () {
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
                url: "/DBTMTestMaster/DBTMTestWisePerformanceStandardConfigurationList",
                data: {
                    dBTMTestMasterId: dBTMTestMasterId,
                    dBTMTestwisePerformanceStandardCategoryId: dBTMTestwisePerformanceStandardCategoryId
                },
                success: function (data) {
                    $("#DataTablesDivId").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr) {
                    if (xhr.status == 401 || xhr.status == 403) { location.reload(); }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Performance Standard Configuration List", "error");
                    CoditechCommon.HideLodder();
                }
            });
        }
    },
    EditConfigurationRow: function (typeId) {
        var row = $("#row_" + typeId);
        row.find(".view-mode").hide();
        row.find(".edit-mode").show();
        row.find(".edit-btn").hide();
        row.find(".save-btn").show();
        row.find(".cancel-btn").show();
    },
    CancelConfigurationRow: function (typeId) {
        var row = $("#row_" + typeId);
        row.find(".edit-mode").hide();
        row.find(".view-mode").show();
        row.find(".save-btn").hide();
        row.find(".cancel-btn").hide();
        row.find(".edit-btn").show();
    },
    SaveConfigurationRow: function (typeId, configurationId) {
        var row = $("#row_" + typeId);
        var isConfigured = row.find(".configuration-dropdown").val() === "true";
        var priorityValue = row.find(".priority-input").val().trim();
        var priority = 0;
        if (isConfigured) {
            if (!priorityValue) {
                CoditechNotification.DisplayNotificationMessage("Priority is required when Configured is Yes.", "error");
                return;
            }
            priority = parseInt(priorityValue);
            if (isNaN(priority) || priority < 1) {
                CoditechNotification.DisplayNotificationMessage("Priority must be greater than 0.", "error");
                return;
            }
        }
        var priorities = [];
        $("#datatable tbody tr").each(function () {
            var currentRow = $(this);
            var configured = currentRow.find(".configuration-dropdown").is(":visible") ? currentRow.find(".configuration-dropdown").val() === "true" : currentRow.find(".configuration-text").text().trim() === "Yes";
            if (configured) {
                var currentPriority = currentRow.find(".priority-input").is(":visible") ? parseInt(currentRow.find(".priority-input").val()) : parseInt(currentRow.find(".priority-text").text().trim());
                if (!isNaN(currentPriority)) {
                    priorities.push(currentPriority);
                }
            }
        });
        var duplicatePriorities = priorities.filter(function (value, index) {
            return priorities.indexOf(value) !== index;
        });
        if (duplicatePriorities.length > 0) {
            CoditechNotification.DisplayNotificationMessage("Duplicate priority is not allowed.", "error");
            return;
        }
        var configuredPriorityCount = priorities.length;
        var invalidContinuousPriority = false;
        for (var i = 1; i <= configuredPriorityCount; i++) {
            if (priorities.indexOf(i) === -1) {
                invalidContinuousPriority = true;
                break;
            }
        }
        if (invalidContinuousPriority) {
            CoditechNotification.DisplayNotificationMessage("Priority must be in sequence.", "error");
            return;
        }
        CoditechCommon.ShowLodder();
        $.ajax({
            type: "POST",
            url: "/DBTMTestMaster/SaveDBTMTestWisePerformanceStandardConfiguration",
            data: {
                DBTMTestWisePerformanceStandardConfigurationId: configurationId,
                DBTMTestMasterId: $("#DBTMTestMasterId").val(),
                DBTMTestwisePerformanceStandardCategoryId: $("#DBTMTestwisePerformanceStandardCategoryId").val(),
                DBTMTestWisePerformanceStandardTypeId: typeId,
                IsConfigured: isConfigured,
                Priority: priority
            },
            success: function (response) {
                if (response.success) {
                    window.location.reload();
                }
                else {
                    CoditechNotification.DisplayNotificationMessage(response.message || "Failed to save Performance Standard Configuration.", "error");
                }
            },
            error: function (xhr) {
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                    return;
                }
                CoditechNotification.DisplayNotificationMessage("Error while saving Performance Standard Configuration.", "error");
            },
            complete: function () {
                CoditechCommon.HideLodder();
            }
        });
    }
};
$(document).ready(function () {
    DBTMTestWisePerformanceStandardConfiguration.Initialize();
});