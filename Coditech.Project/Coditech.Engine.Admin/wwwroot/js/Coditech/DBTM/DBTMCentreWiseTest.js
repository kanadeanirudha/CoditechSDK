var DBTMCentreWiseTest = {
    Initialize: function () {
        DBTMCentreWiseTest.constructor();
    },
    constructor: function () {
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
            url: "/DBTMCentreWiseSetting/GetAssociateUnAssociateCentreTest",
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
        $("#frmAssociateUnAssociateCentreTest").submit();
    },
}
$(document).ready(function () {
    $('#datatable').DataTable();
});
