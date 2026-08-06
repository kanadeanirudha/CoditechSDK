var DBTMCentreWiseTest = {
    Initialize: function () {
        DBTMCentreWiseTest.constructor();  
        DBTMCentreWiseTest.InitializeDataTable();
    },
    constructor: function () {
    },
    InitializeDataTable: function () {
        if ($.fn.DataTable.isDataTable('#datatable')) {
            $('#datatable').DataTable().destroy();
        }
        $('#datatable').DataTable({
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
                    targets: 2,
                    orderable: false,
                    searchable: false
                }
            ]
        });
    },
    GetAssociateUnAssociateCentreTest: function ( modelPopContentId, dBTMCentreWiseTestId, dBTMTestMasterId, organisationCentreMasterId, centreCode, testName) {
        let viewModel = {
            DBTMCentreWiseTestId: dBTMCentreWiseTestId,
            DBTMTestMasterId: dBTMTestMasterId,
            OrganisationCentreMasterId: organisationCentreMasterId,
            CentreCode: centreCode,
            TestName: testName
        };
        CoditechCommon.ShowLodder();
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/DBTMOrganisationCentreMaster/GetAssociateUnAssociateCentreTest",
            data: viewModel,
            success: function (result) {
                $('#' + modelPopContentId).html(result);
                CoditechCommon.HideLodder();
            },
            error: function (xhr) {
                if (xhr.status == "401" || xhr.status == "403") {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage( "Failed to display record.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },
    AssociateUnAssociateCentreTest: function () {
        CoditechCommon.ShowLodder();
        $.ajax({
            url: '/DBTMOrganisationCentreMaster/AssociateUnAssociateCentreTest',
            type: 'POST',
            data: {
                organisationCentreId: $("#OrganisationCentreMasterId").val(),
                centreCode: $("#CentreCode").val(),
                testIds: selectedActivities.join(','),
                actionType: currentActionType
            },
            success: function (response) {
                CoditechCommon.HideLodder();
                if (response.success) {
                    $('#AssociateTestPopup').modal('hide');
                    location.reload();
                }
                else {
                    CoditechNotification.DisplayNotificationMessage(
                        "Failed to update activities.",
                        "error"
                    );
                }
            },
            error: function (xhr) {
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage(
                    "Failed to update activities.",
                    "error"
                );
                CoditechCommon.HideLodder();
            }
        });
        return false;
    },
    OpenAssociatePopup: function () {
        currentActionType = "associate";
        selectedActivities = [];
        $(".activity-checkbox:checked").each(function () {
            selectedActivities.push($(this).val());
        });
        DBTMCentreWiseTest.GetAssociateUnAssociateCentreTest(
            "AssociateTestPopupContent",
            0,
            0,
            $("#OrganisationCentreMasterId").val(),
            $("#CentreCode").val(),
            ""
        );
        $('#AssociateTestPopup').modal('show');
    },
    OpenUnAssociatePopup: function () {
        currentActionType = "unassociate";
        selectedActivities = [];
        $(".activity-checkbox:checked").each(function () {
            selectedActivities.push($(this).val());
        });
        DBTMCentreWiseTest.GetAssociateUnAssociateCentreTest(
            "AssociateTestPopupContent",
            1,
            0,
            $("#OrganisationCentreMasterId").val(),
            $("#CentreCode").val(),
            ""
        );
        $('#AssociateTestPopup').modal('show');
    },
}
var selectedActivities = [];
var currentActionType = "";
$(document).ready(function () {
    DBTMCentreWiseTest.Initialize();
    $(document).on("change", "#chkSelectAll", function () {
        var isChecked = $(this).is(":checked");
        $(".activity-checkbox").prop("checked", isChecked);
    });
    $(document).on("change", ".activity-checkbox", function () {
        $("#chkSelectAll").prop(
            "checked",
            $(".activity-checkbox").length === $(".activity-checkbox:checked").length
        );
    });
});
