var DBTMCamp = {
    OnCentreChange: function () {
        var centreCode = $("#CentreCode").val();
        var selectedActivities = $("#CustomDropdownSelectedValue1").val() || [];
        if (!centreCode) {
            $("#ActivityDropdownDiv").html("");
            return;
        }
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/DBTMCampMaster/GetActivityByCentreCode",
            traditional: true,   
            data: {
                centreCode: centreCode,
                selectedActivities: selectedActivities
            },
            success: function (data) {
                $("#ActivityDropdownDiv").html(data);
                $("#ActivityDropdownDiv .selectpicker")
                    .selectpicker('render')
                    .selectpicker('refresh');
            }
        });
    }
};

$(document).ready(function () {
    if ($("#DBTMCampMasterId").val() == 0) {
        DBTMCamp.OnCentreChange();
    }
    $("#CentreCode").change(function () {
        DBTMCamp.OnCentreChange();
    });
});