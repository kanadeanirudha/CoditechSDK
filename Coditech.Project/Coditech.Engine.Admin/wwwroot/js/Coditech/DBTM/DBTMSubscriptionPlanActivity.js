var DBTMSubscriptionPlanActivity = {
    Initialize: function () {
        DBTMSubscriptionPlanActivity.constructor();
    },
    constructor: function () {
    },

    GetAssociateUnAssociatePlanActivity: function (modelPopContentId, dBTMSubscriptionPlanActivityId, dBTMSubscriptionPlanId, planName, testName, dBTMTestMasterId) {

        let dBTMSubscriptionPlanActivityViewModel = {
            DBTMSubscriptionPlanActivityId: dBTMSubscriptionPlanActivityId,
            DBTMSubscriptionPlanId: dBTMSubscriptionPlanId,
            PlanName: planName,
            TestName: testName,
            
            DBTMTestMasterId: dBTMTestMasterId
            
        };
        CoditechCommon.ShowLodder();
        $.ajax(
            {
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/DBTMSubscriptionPlan/GetAssociateUnAssociatePlanActivity",
                data: dBTMSubscriptionPlanActivityViewModel,
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
    AssociateUnAssociatePlanActivity: function () {
        $("#frmAssociateUnAssociatePlanActivity").submit();
    },
}
