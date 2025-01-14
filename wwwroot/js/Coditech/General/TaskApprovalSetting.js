var TaskApprovalSetting = {
    Initialize: function () {
        TaskApprovalSetting.constructor();
    },
    constructor: function () {
    },
    BindDropdownEvents: function () {
        $(document).on("change", ".employee-dropdown", function () {
            TaskApprovalSetting.ValidateDropdownValues();
        });
    },

    GetEmployeeListByCentreCode: function (centreCode, countNumber) {
        CoditechCommon.ShowLodder();
        countNumber = $("#CountNumber").val();
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/TaskApprovalSetting/GetEmployeeListByCentreCode?centreCode=" + centreCode + "&countNumber=" + countNumber,
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                $("#EmployeeListId").html("").html(result);
                CoditechCommon.HideLodder();
                 TaskApprovalSetting.BindDropdownEvents();

            },
            error: function (xhr, ajaxOptions, thrownError) {
                if (xhr.status == "401" || xhr.status == "403") {
                    {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to load TaskApprovalSetting details.", "error");
                    CoditechCommon.HideLodder();
                }
            }
        });
    },

     ValidateDropdownValues: function () {
        var selectedValues = [];
        var isDuplicate = false;

        $(".employee-dropdown").each(function () {
            var selectedValue = $(this).val();
            if (selectedValue) {
                if (selectedValues.includes(selectedValue)) {
                    isDuplicate = true;

                    CoditechNotification.DisplayNotificationMessage(
                        "Duplicate value selected. Please choose a unique employee.",
                        "error"
                    );
                    $(this).addClass("duplicate-entry");
                } else {
                    $(this).removeClass("duplicate-entry"); 
                    selectedValues.push(selectedValue);
                }
            }
        });

        return !isDuplicate; 
    },


    SaveData: function () {
        if (!TaskApprovalSetting.ValidateDropdownValues()) {

        CoditechNotification.DisplayNotificationMessage(
            "Cannot save. Resolve duplicate entries first.",
            "error"
        );
        return false;
    }

        var dropdownValues = [];
        $('#makeEditable tbody tr').each(function () {
            var employeeId = $(this).find('select').val();
            if (employeeId) {
                dropdownValues.push(employeeId); 
            }
        });

        $('#EmployeeIds').val(dropdownValues.join(","));
        console.log('Prepared JSON Data:', jsonData);

        $("#frmTaskApprovalSetting").submit();
    }
}
