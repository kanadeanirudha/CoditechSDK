var DBTMCampMaster = {
    Initialize: function () {
        DBTMCampMaster.constructor();
    },
    constructor: function () {
    },

    GetAssociateUnAssociateCampwiseUser: function (modelPopContentId, dBTMCampUserId, dBTMCampMasterId, campName, firstName, lastName, entityId) {

        let DBTMCampUserViewModel = {
            DBTMCampUserId: dBTMCampUserId,
            DBTMCampMasterId: dBTMCampMasterId,
            CampName: campName,
            FirstName: firstName,
            LastName:lastName,
            EntityId: entityId
            
        };
        CoditechCommon.ShowLodder();
        $.ajax(
            {
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMCampMaster/GetAssociateUnAssociateCampwiseUser",
                data: DBTMCampUserViewModel,
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    $('#' + modelPopContentId).html("").html(result);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to display record.", "error");
                    CoditechCommon.HideLodder();
                }
            });
    },
    AssociateUnAssociateCampwiseUser: function () {
        $("#frmAssociateUnAssociateCampwiseUser").submit();
    },
}
