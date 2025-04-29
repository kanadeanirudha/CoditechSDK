var AccGLOpeningBalance = {
    Initialize: function () {
    },

    constructor: function () {
    },
    GetAccSetUpCategoryListByGLCategory: function () {
        var accSetupCategoryId = $("#AccSetupCategoryId").val();
        $('#AccGLOpeningBalanceDivId').html('');
        if (accSetupCategoryId != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/AccGLOpeningBalance/UpdateNonControlHeadType",
                data: { "accSetupCategoryId": accSetupCategoryId },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#AccGLOpeningBalanceDivId").html("").html(data);

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
        }
    },

    SaveDataDeatils: function () {
        var data = [];

        // Loop through each row in the table
        $('#makeEditable tbody tr').each(function () {
            var row = $(this);
            var rowData = {
                AccSetupGLId: parseInt(row.find('input[id^="AccSetupGLId"]').val()),
                OpeningBalance: parseFloat(row.find('input[id^="OpeningBalance_"]').val()),
            };
            data.push(rowData);
        });

        // Stringify the data array
        var jsonData = JSON.stringify(data);

        //console.log('JSON data:', jsonData);
        $("#AccGLOpeningBalanceData").val(jsonData);
        console.log('JSON data:', jsonData);
        $("#frmAccGLOpeningBalanceDetails").submit();
    },

    GetAccSetUpCategoryList: function () {
        var accSetupCategoryId = $("#AccSetupCategoryId").val();
        $('#AccGLOpeningBalanceControlHeadDivId').html('');
        if (accSetupCategoryId != "") {
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
        }
    },

    SaveIndividualOpeningBalanceData: function () {
        var data = [];

        // Loop through each row in the table
        $('#makeEditable tbody tr').each(function () {
            var row = $(this);
            var rowData = {
                AccSetupGLId: parseInt(row.find('input[id^="AccSetupGLId_"]').val()),
                //GeneralFinancialYearId: parseInt(row.find('input[id^="GeneralFinancialYearId_"]').val()),
                //UserTypeId: parseInt(row.find('input[id^="UserTypeId_"]').val()),
                PersonId: parseInt(row.find('input[id^="PersonId_"]').val()),
                OpeningBalance: parseFloat(row.find('input[id^="OpeningBalance_"]').val()),
            };
            data.push(rowData);
        });

        // Stringify the data array
        var jsonData = JSON.stringify(data);

        //console.log('JSON data:', jsonData);
        $("#IndividualOpeningBalanceData").val(jsonData);
        console.log('JSON data:', jsonData);
        $("#frmIndividualOpeningBalanceDetails").submit();
    },
}



