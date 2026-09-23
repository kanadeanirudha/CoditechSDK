var DBTMPrintQR = {
    Initialize: function () {
        DBTMPrintQR.BindDropdownEvents();
    },
    InitializePrintQRTable: function () {
        if ($.fn.DataTable.isDataTable("#datatable-printqr")) {
            $("#datatable-printqr").DataTable().destroy();
        }
        $("#datatable-printqr").DataTable({
            paging: true,
            searching: true,
            ordering: true,
            info: true,
            lengthChange: true,
            pageLength: 10,
            responsive: true,
            autoWidth: false,
            columnDefs: [
                {
                    targets: 0,
                    orderable: false,
                    searchable: false
                },
                {
                    targets: 5,
                    orderable: false,
                    searchable: false
                },
                {
                    targets: 6,
                    orderable: false,
                    searchable: false
                }
            ]
        });
    },
    constructor: function () {
        DBTMPrintQR.BindDropdownEvents();
    },
    BindDropdownEvents: function () {
        $(document).on("change", "#QRPrintingTemplateCode", function () {
            var batchId = $("#SelectedParameter1").val();
            var templateCode = $(this).val();
            if (!batchId || batchId === "0") {
                CoditechNotification.DisplayNotificationMessage("Please select Batch.", "error");
                $(this).val("");
                return;
            }
            if (!templateCode || templateCode === "0") {
                $("#PrintQRUserListContainer").hide();
                $("#PrintQRUserListDiv").empty();
                return;
            }
            DBTMPrintQR.LoadTraineeList();
        });
    },
    ShowPrintQR: function () {
        var batchId = $("#SelectedParameter1").val();
        if (!batchId || batchId === "0") {
            CoditechNotification.DisplayNotificationMessage("Please select Batch.", "error");
            return;
        }
        $("#QRFormatDiv").show();
        var templateCode = $("#QRPrintingTemplateCode").val();
        if (!templateCode || templateCode === "0") {
            $("#QRPrintingTemplateCode").val("DBTMAutoActivityQRCodeFormatVertical");
        }
        DBTMPrintQR.LoadTraineeList();
    },
    LoadTraineeList: function () {
        var batchId = $("#SelectedParameter1").val();
        var templateCode = $("#QRPrintingTemplateCode").val();
        if (!batchId || batchId === "0") {
            CoditechNotification.DisplayNotificationMessage("Please select Batch.", "error");
            return;
        }
        if (!templateCode || templateCode === "0") {
            return;
        }
        CoditechCommon.ShowLodder();
        $.ajax({
            url: "/DBTMPrintQR/GetDBTMPrintQRTraineeList",
            type: "GET",
            data: {
                SelectedParameter1: batchId
            },
            success: function (result) {
                $("#PrintQRUserListDiv").html(result);
                $("#PrintQRUserListContainer").show();
                DBTMPrintQR.InitializePrintQRTable();
                CoditechCommon.HideLodder();
            },
            error: function (xhr) {
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                    return;
                }
                CoditechNotification.DisplayNotificationMessage("Failed to load trainee list.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },
    PrintQR: function (personId) {
        var personIds = [];
        if (personId && personId > 0) {
            personIds.push(personId);
        }
        else {
            $(".person-checkbox:checked").each(function () {
                personIds.push($(this).val());
            });
        }
        if (!$("#SelectedParameter1").val()) {
            CoditechNotification.DisplayNotificationMessage("Please select Batch.", "error");
            return;
        }
        if (!$("#QRPrintingTemplateCode").val()) {
            CoditechNotification.DisplayNotificationMessage("Please select QR format.", "error");
            return;
        }
        if (personIds.length === 0) {
            CoditechNotification.DisplayNotificationMessage("Please select at least one Athlete.", "error");
            return;
        }
        CoditechCommon.ShowLodder();
        $.ajax({
            url: "/DBTMPrintQR/CheckPrintQRAvailability",
            type: "GET",
            data: {
                personIds: personIds.join(',')
            },
            success: function (response) {
                if (response.success) {
                    var downloadUrl =
                        "/DBTMPrintQR/DownloadPrintQR?personIds="
                        + encodeURIComponent(personIds.join(','))
                        + "&generalBatchMasterId="
                        + $("#SelectedParameter1").val()
                        + "&templateCode="
                        + $("#QRPrintingTemplateCode").val();
                    CoditechCommon.DownloadFile(downloadUrl);
                }
                else {
                    CoditechNotification.DisplayNotificationMessage(response.message, "error");
                    CoditechCommon.HideLodder();
                }
            },
            error: function (xhr) {
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                    return;
                }
                CoditechNotification.DisplayNotificationMessage("Error while downloading QR.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },
    OnBatchChange: function () {
        var batchId = $("#SelectedParameter1").val();
        $("#PrintQRUserListContainer").hide();
        $("#PrintQRUserListDiv").empty();
        $("#chkSelectAll").prop("checked", false);
        if (!batchId || batchId === "0") {
            $("#QRFormatDiv").hide();
            return;
        }
        $("#QRFormatDiv").show();
        $("#QRPrintingTemplateCode").val("DBTMAutoActivityQRCodeFormatVertical").trigger("change.select2");
        DBTMPrintQR.LoadTraineeList();
    },
};
$(document).ready(function () {
    DBTMPrintQR.Initialize();
    $(document).on("change", "#chkSelectAll", function () {
        $(".person-checkbox").prop("checked", $(this).is(":checked"));
    });
    $(document).on("change", ".person-checkbox", function () {
        $("#chkSelectAll").prop("checked", $(".person-checkbox").length === $(".person-checkbox:checked").length);
    });
});