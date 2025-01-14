var GeneralBatchMaster = {
    Initialize: function () {
        GeneralBatchMaster.constructor();
    },
    constructor: function () {
    },

    GetAssociateUnAssociateBatchwiseUser: function (modelPopContentId, generalBatchUserId, generalBatchMasterId, batchName, firstName, lastName, entityId) {

        let generalBatchUserViewModel = {
            GeneralBatchUserId: generalBatchUserId,
            GeneralBatchMasterId: generalBatchMasterId,
            BatchName: batchName,
            FirstName: firstName,
            LastName:lastName,
            EntityId:entityId
            
        };
        CoditechCommon.ShowLodder();
        $.ajax(
            {
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/GeneralBatchMaster/GetAssociateUnAssociateBatchwiseUser",
                data: generalBatchUserViewModel,
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
    AssociateUnAssociateBatchwiseUser: function () {
        $("#frmAssociateUnAssociateBatchwiseUser").submit();
    },
}
