var UserMainMenu = {
    Initialize: function () {
        UserMainMenu.constructor();
    },

    constructor: function () {
    },

    GetParentMemuCodeByModuleCode: function () {
        var moduleCode = $("#ModuleCode").val();
        if (moduleCode != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/GeneralUserMainMenu/GetParentMemuCodeByModuleCode",
                data: { "moduleCode": moduleCode },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#ParentMenuCode").html("").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve Parent Menu", "error")
                    CoditechCommon.HideLodder();
                }
            });

        }
    },
}








