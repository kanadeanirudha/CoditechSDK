var DBTMActivityListViewSequence = {
    Initialize: function () {
        DBTMActivityListViewSequence.constructor();
    },
    constructor: function () { },

    UpdateSequenceNumber: function (modelPopContentId, dBTMTestMasterId) {
        CoditechCommon.ShowLodder();
        $("#" + modelPopContentId).html("");

        $.ajax({
            type: "GET",
            url: "/DBTMTestMaster/UpdateSequenceNumber",
            data: { dBTMTestMasterId: dBTMTestMasterId },
            success: function (result) {
                $("#" + modelPopContentId).html(result);
                CoditechCommon.HideLodder();

                var modal = new bootstrap.Modal(document.getElementById('AddSequenceNumberPopupId'));
                modal.show();
            },
            error: function () {
                CoditechCommon.HideLodder();
                CoditechNotification.DisplayNotificationMessage("Failed to load popup.", "error");
            }
        });
    },

    SaveData: function () {
        $("#saveSequenceButton").prop("disabled", true);

        var data = [];

        $("#sequenceTable tbody tr").each(function () {
            var row = $(this);
            var id = row.find('.DBTMTestParameterListViewSequenceId').val();
            var seq = row.find('.SequenceNumber').val();

            data.push({
                DBTMTestParameterListViewSequenceId: parseInt(id),
                SequenceNumber: parseInt(seq)
            });
        });
        var jsonData = JSON.stringify(data);
        $("#DBTMSequenceData").val(jsonData);

        $.ajax({
            type: "POST",
            url: "/DBTMTestMaster/UpdateSequenceNumber",
            data: {
                DBTMTestMasterId: $("#DBTMTestMasterId").val(),
                DBTMTestParameterListViewSequenceId: $("#DBTMTestParameterListViewSequenceId").val(),
                DBTMSequenceData: jsonData
            },
            success: function (response) {
                if (response.success) {
                    $('#AddSequenceNumberPopupId').modal('hide');
                    location.reload();
                } else {
                    CoditechNotification.DisplayNotificationMessage(response.message);
                }
            },
            error: function (xhr) {
                CoditechNotification.DisplayNotificationMessage("Error while saving sequence number. " + xhr.statusText, "error");
            },
            complete: function () {
                $("#saveSequenceButton").prop("disabled", false);
            }
        });
    },
    UpdateVerticalSequenceNumber: function (modelPopContentId, dBTMTestMasterId) {
        CoditechCommon.ShowLodder();
        $("#" + modelPopContentId).html("");

        $.ajax({
            type: "GET",
            url: "/DBTMTestMaster/UpdateVerticalSequenceNumber",
            data: { dBTMTestMasterId: dBTMTestMasterId },
            success: function (result) {
                $("#" + modelPopContentId).html(result);
                CoditechCommon.HideLodder();

                var modal = new bootstrap.Modal(document.getElementById('AddVerticalSequenceNumberPopupId'));
                modal.show();
            },
            error: function () {
                CoditechCommon.HideLodder();
                CoditechNotification.DisplayNotificationMessage("Failed to load popup.", "error");
            }
        });
    },

    SaveVerticalData: function () {
        $("#saveSequenceButton").prop("disabled", true);

        var data = [];

        $("#sequenceVerticalTable tbody tr").each(function () {
            var row = $(this);
            var id = row.find('.DBTMTestParameterVerticalViewSequenceId').val();
            var seq = row.find('.SequenceNumber').val();

            data.push({
                DBTMTestParameterVerticalViewSequenceId: parseInt(id),
                SequenceNumber: parseInt(seq)
            });
        });
        var jsonData = JSON.stringify(data);
        $("#DBTMSequenceData").val(jsonData);

        $.ajax({
            type: "POST",
            url: "/DBTMTestMaster/UpdateVerticalSequenceNumber",
            data: {
                DBTMTestMasterId: $("#DBTMTestMasterId").val(),
                DBTMTestParameterListViewSequenceId: $("#DBTMTestParameterVerticalViewSequenceId").val(),
                DBTMSequenceData: jsonData
            },
            success: function (response) {
                if (response.success) {
                    $('#AddVerticalSequenceNumberPopupId').modal('hide');
                    location.reload();
                } else {
                    CoditechNotification.DisplayNotificationMessage(response.message);
                }
            },
            error: function (xhr) {
                CoditechNotification.DisplayNotificationMessage("Error while saving sequence number. " + xhr.statusText, "error");
            },
            complete: function () {
                $("#saveVerticalSequenceButton").prop("disabled", false);
            }
        });
    }
};