var GeneralBatchMaster = {
    Initialize: function () {
        GeneralBatchMaster.constructor();
    },
    constructor: function () {
    },

    GetAssociateUnAssociateBatchwiseUser: function (modelPopContentId, generalBatchUserId, generalBatchMasterId, batchName, firstName, lastName, entityId, custom4) {

        let generalBatchUserViewModel = {
            GeneralBatchUserId: generalBatchUserId,
            GeneralBatchMasterId: generalBatchMasterId,
            BatchName: batchName,
            FirstName: firstName,
            LastName: lastName,
            EntityId: entityId,
            Custom4: custom4
        };
        CoditechCommon.ShowLodder();
        $.ajax({
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
            error: function (xhr) {
                if (xhr.status == "401" || xhr.status == "403") {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage( "Failed to display record.", "error" );
                CoditechCommon.HideLodder();
            }
        });
    },
    AssociateUnAssociateBatchwiseUser: function () {
        $("#frmAssociateUnAssociateBatchwiseUser").submit();
    },
    EditBatchUserRow: function (entityId) {
        var row = $("#row_" + entityId);
        row.find(".view-mode").hide();
        row.find(".edit-mode").show();
        row.find(".edit-btn").hide();
        row.find(".save-btn").show();
        row.find(".cancel-btn").show();
    },
    CancelBatchUserRow: function (entityId) {
        var row = $("#row_" + entityId);
        row.find(".edit-mode").hide();
        row.find(".view-mode").show();
        row.find(".save-btn").hide();
        row.find(".cancel-btn").hide();
        row.find(".edit-btn").show();
    },
    SaveBatchUserRow: function (entityId, generalBatchUserId, generalBatchMasterId, firstName, lastName, custom4) {
        var row = $("#row_" + entityId);
        var isAssociated = row.find(".association-dropdown").val() === "true";
        var currentIsAssociated = row.find(".association-text").text().trim() === "Yes";
        if (isAssociated === currentIsAssociated) {
            GeneralBatchMaster.CancelBatchUserRow(entityId);
            return;
        }
        CoditechCommon.ShowLodder();
        $.ajax({
            cache: false,
            type: "POST",
            url: "/DBTMGeneralBatchMaster/AssociateUnAssociateBatchwiseUser",
            data: {
                GeneralBatchUserId: generalBatchUserId,
                GeneralBatchMasterId: generalBatchMasterId,
                EntityId: entityId,
                FirstName: firstName,
                LastName: lastName,
                IsAssociated: isAssociated,
                UserType: "Trainee",
                Custom4: custom4
            },
            success: function (result) {
                if (result.success) {
                    window.location.reload();
                }
            },
            error: function (xhr) {
                if (xhr.status == 401 || xhr.status == 403) {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Update failed.", "error" );
            },
            complete: function () {
                CoditechCommon.HideLodder();
            }
        });
    },
};