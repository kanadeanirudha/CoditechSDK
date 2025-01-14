var AdminRoleMaster = {
    Initialize: function () {
        AdminRoleMaster.constructor();
    },
    constructor: function () {
    },
}
$("#MonitoringLevel").change(function () {
    var monitoringLevel = $("#MonitoringLevel").val();
    if (monitoringLevel == "Self") {
        $("#SelectedRoleWiseCentresDivId").hide();
        $("#SelectedCentreNameForSelfDivId").show();
    }
    else {
        $("#SelectedCentreNameForSelfDivId").hide();
        $("#SelectedRoleWiseCentresDivId").show();
    }
});

$("#ModuleCode").change(function () {
    CoditechCommon.ShowLodder();
    var moduleCode = $("#ModuleCode option:selected").val();
    if (moduleCode == "") {
        $("#SaveAdminRoleMenuDetailsId").attr('disabled', 'disabled');
    }
    else {
        var adminRoleMasterId = $("#AdminRoleMasterId").val();
        let url = window.location.origin + window.location.pathname + '?adminRoleMasterId=' + adminRoleMasterId + '&moduleCode=' + moduleCode;
        window.location.href = url;
    }
    CoditechCommon.HideLodder();
});