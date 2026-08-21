var DBTMGraphListViewSequence = {
    Initialize: function () {
        DBTMGraphListViewSequence.constructor();
    },
    constructor: function () {
    },
    UpdateGraphVerticalSequenceNumber: function (modelPopContentId, dBTMGraphMasterId) {
        CoditechCommon.ShowLodder();
        $("#" + modelPopContentId).html("");
        $.ajax({
            type: "GET",
            url: "/DBTMGraphMaster/UpdateGraphVerticalSequenceNumber",
            data: { dBTMGraphMasterId: dBTMGraphMasterId },
            success: function (result) {
                $("#" + modelPopContentId).html(result);
                CoditechCommon.HideLodder();
                var modal = new bootstrap.Modal(document.getElementById('AddGraphVerticalSequenceNumberPopupId'));
                modal.show();
            },
            error: function (xhr) {
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                }
                CoditechCommon.HideLodder();
                CoditechNotification.DisplayNotificationMessage("Failed to load popup.", "error");
            }
        });
    },
    SaveVerticalData: function () {
        $("#saveGraphVerticalSequenceButton").prop("disabled", true);
        var data = [];
        $("#sequenceVerticalTable tbody tr").each(function () {
            var row = $(this);
            var id = row.find('.DBTMGraphVerticalViewSequenceId').val();
            var seq = row.find('.SequenceNumber').val();
            data.push({
                DBTMGraphVerticalViewSequenceId: parseInt(id),
                SequenceNumber: parseInt(seq)
            });
        });
        var jsonData = JSON.stringify(data);
        $("#DBTMSequenceData").val(jsonData);
        $.ajax({
            type: "POST",
            url: "/DBTMGraphMaster/UpdateGraphVerticalSequenceNumber",
            data: {
                DBTMGraphMasterId: $("#DBTMGraphMasterId").val(),
                DBTMGraphVerticalViewSequenceId: $("#DBTMGraphVerticalViewSequenceId").val(),
                DBTMSequenceData: jsonData
            },
            success: function (response) {
                if (response.success) {
                    $('#AddGraphVerticalSequenceNumberPopupId').modal('hide');
                    location.reload();
                }
                else {
                    CoditechNotification.DisplayNotificationMessage( response.message);
                }
            },
            error: function (xhr) {
                CoditechNotification.DisplayNotificationMessage("Error while saving sequence number. " + xhr.statusText, "error");
            },
            complete: function () {
                $("#saveGraphVerticalSequenceButton").prop("disabled", false);
            }
        });
    },
};