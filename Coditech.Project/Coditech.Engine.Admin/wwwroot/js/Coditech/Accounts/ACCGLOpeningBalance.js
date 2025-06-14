var AccGLOpeningBalance = {
    Initialize: function () {
    },

    constructor: function () {
    },
    GetAccSetUpCategoryListByGLCategory: function () {
        $('#AccGLOpeningBalanceDivId').hide().html('');
        var accSetupCategoryId = $("#AccSetupCategoryId").val();
        if (accSetupCategoryId == "") {
            CoditechNotification.DisplayNotificationMessage("Please select Category.", "error");
            return false;
        }

        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/AccGLOpeningBalance/UpdateNonControlHeadType",
            data: { "accSetupCategoryId": accSetupCategoryId },
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $('#AccGLOpeningBalanceDivId').html(data).show();
                CoditechCommon.HideLodder();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                if (xhr.status == "401" || xhr.status == "403") {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Failed to retrieve Acc SetUp Category.", "error")
                CoditechCommon.HideLodder();
            }
        });
    },

    SaveDataDeatils: function () {
        var data = [];
        var addedIds = new Set();

        $('#makeEditable tbody tr').each(function () {
            var row = $(this);

            var isClosingBalanceUpdated = row.find('input[id^="IsClosingBalanceUpdated_"]').is(':checked');
            var accSetupGLId = parseInt(row.find('input[id^="AccSetupGLId"]').val());
            var openingBalanceInput = row.find('input[id^="OpeningBalance_"]');
            var openingBalance = parseFloat(openingBalanceInput.val());
            var originalBalance = parseFloat(openingBalanceInput.prop("defaultValue"));

            // Only push if checkbox not checked, opening balance > 0, and balance changed
            if (!isClosingBalanceUpdated && !isNaN(openingBalance) && openingBalance > 0 && openingBalance !== originalBalance && !addedIds.has(accSetupGLId)) {
                data.push({
                    AccSetupGLId: accSetupGLId,
                    OpeningBalance: openingBalance,
                    IsClosingBalanceUpdated: 0
                });
                addedIds.add(accSetupGLId);
            }
        });

        if (data.length === 0) {
            CoditechNotification.DisplayNotificationMessage("No changes to save.", "info");
            return false;
        }

        var jsonData = JSON.stringify(data);
        $("#AccGLOpeningBalanceData").val(jsonData);

        $.ajax({
            type: 'POST',
            data: $('#frmAccGLOpeningBalanceDetails').serialize(),
            success: function () {
                var accSetupCategoryId = $("#AccSetupCategoryId").val();
                if (accSetupCategoryId) {
                    window.location.href = "/AccGLOpeningBalance/UpdateNonControlHeadType?accSetupCategoryId=" + accSetupCategoryId;
                } else {
                    location.reload();
                }
            },
            error: function (xhr) {
                if (xhr.status === 401 || xhr.status === 403) {
                    location.reload();
                } else {
                    CoditechNotification.DisplayNotificationMessage("Save failed. Please try again.", "info");
                    CoditechCommon.HideLodder();
                }
            }
        });
    },

    GetAccSetUpCategoryList: function () {
        $('#AccGLOpeningBalanceControlHeadDivId').html('');
        var accSetupCategoryId = $("#AccSetupCategoryId").val();
        if (accSetupCategoryId == "") {
            CoditechNotification.DisplayNotificationMessage("Please select Category.", "error");
            return false;
        }
        CoditechCommon.ShowLodder();
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/AccGLOpeningBalanceControlHead/ControlHeadType",
            data: { "accSetupCategoryId": accSetupCategoryId },
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#AccGLOpeningBalanceControlHeadDivId").html("").html(data);
                CoditechCommon.HideLodder();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                if (xhr.status == "401" || xhr.status == "403") {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Failed to retrieve Acc SetUp Category.", "error")
                CoditechCommon.HideLodder();
            }
        });
    },

    SaveIndividualOpeningBalanceData: function () {
        var data = [];
        var addedIds = new Set();

        $('#makeEditable tbody tr').each(function () {
            var row = $(this);

            var isClosingBalanceUpdated = row.find('input[id^="IsClosingBalanceUpdated_"]').is(':checked');
            var accSetupGLId = parseInt(row.find('input[id^="AccSetupGLId"]').val());
            var personId = parseInt(row.find('input[id^="PersonId_"]').val());
            var openingBalanceInput = row.find('input[id^="OpeningBalance_"]');
            var openingBalance = parseFloat(openingBalanceInput.val());
            var originalBalance = parseFloat(openingBalanceInput.prop("defaultValue"));

            // Only push if checkbox not checked, opening balance > 0, and balance changed
            if (!isClosingBalanceUpdated && !isNaN(openingBalance) && openingBalance > 0 && openingBalance !== originalBalance && !addedIds.has(accSetupGLId)) {
                data.push({
                    AccSetupGLId: accSetupGLId,
                    PersonId: personId,
                    OpeningBalance: openingBalance,
                    IsClosingBalanceUpdated: 0
                });
                addedIds.add(accSetupGLId);
            }
        });

        if (data.length === 0) {
            CoditechNotification.DisplayNotificationMessage("No changes to save.", "info");
            return false;
        }

        var jsonData = JSON.stringify(data);
        $("#IndividualOpeningBalanceData").val(jsonData);

        $.ajax({
            type: 'POST',
            data: $('#frmIndividualOpeningBalanceDetails').serialize(),
            success: function () {
                var accSetupCategoryId = $("#AccSetupCategoryId").val();
                if (accSetupCategoryId) {
                    window.location.href = "/AccGLOpeningBalanceControlHead/ControlHeadType?accSetupCategoryId=" + accSetupCategoryId;
                } else {
                    location.reload();
                }
            },
            error: function (xhr) {
                if (xhr.status === 401 || xhr.status === 403) {
                    location.reload();
                } else {
                    CoditechNotification.DisplayNotificationMessage("Save failed. Please try again.", "error");
                    CoditechCommon.HideLodder();
                }
            }
        });
    },
}



