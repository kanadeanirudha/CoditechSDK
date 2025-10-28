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

                DBTMActivityListViewSequence.BindSave();
            },
            error: function () {
                CoditechCommon.HideLodder();
                CoditechNotification.DisplayNotificationMessage("Failed to load popup.", "error");
            }
        });
    },

    BindSave: function () {
        $("#saveSequenceButton").off("click").on("click", function () {
            var data = [];

            $("#sequenceTable tbody tr").each(function () {
                var row = $(this);
                var id = row.find('input[name*="DBTMTestParameterListViewSequenceId"]').val();
                var seq = row.find('input[name*="SequenceNumber"]').val();

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
                data: $("#frmUpdateSequence").serialize(),
                success: function (response) {
                    if (response.success) {
                        CoditechNotification.DisplayNotificationMessage("Sequence Number Saved Successfully.", "success");
                        location.reload();
                    } else {
                        CoditechNotification.DisplayNotificationMessage("Failed to save sequence number.", "error");
                    }
                },
                error: function () {
                    CoditechNotification.DisplayNotificationMessage("Error while saving sequence number.", "error");
                }
            });
        });
    }
};