var CoditechDataTable = {
    Initialize: function () {
        CoditechDataTable.constructor();
    },
    constructor: function () {
    },

    // LoadList method is used to load List page
    LoadList: function (controllerName, methodName) {
        var dataTableModel = BindDataTableModel($('#DataTables_PageIndexId').val());
        CallListPage(controllerName, methodName, dataTableModel);
    },

    // LoadList method is used to load List page
    LoadListFirst: function (controllerName, methodName) {
        var dataTableModel = BindDataTableModel(1);
        CallListPage(controllerName, methodName, dataTableModel);
    },

    LoadListLast: function (controllerName, methodName, pageSize) {
        var dataTableModel = BindDataTableModel(pageSize);
        CallListPage(controllerName, methodName, dataTableModel);
    },

    LoadListPrevious: function (controllerName, methodName, pageIndex) {
        var dataTableModel = BindDataTableModel(pageIndex);
        CallListPage(controllerName, methodName, dataTableModel);
    },
    
    LoadListNext: function (controllerName, methodName, pageIndex) {
        var dataTableModel = BindDataTableModel(pageIndex);
        CallListPage(controllerName, methodName, dataTableModel);
    },

    LoadListSortBy: function (controllerName, methodName, columnName, sortBy) {
        sortBy = sortBy == "asc" ? "desc" : "asc";
        $('#DataTables_SortColumn').val(columnName);
        $('#DataTables_SortBy').val(sortBy);
        var dataTableModel = BindDataTableModel($('#DataTables_PageIndexId').val());
        CallListPage(controllerName, methodName, dataTableModel);
    },
    ReloadLoadList: function (controllerName, methodName) {
        $("#DataTables_SearchById").val("");
        var dataTableModel = BindDataTableModel($('#DataTables_PageIndexId').val());
        CallListPage(controllerName, methodName, dataTableModel);
    },
}

function CallListPage(controllerName, methodName, dataTableModel) {
    CoditechCommon.ShowLodder();
    $.ajax(
        {
            cache: false,
            type: "POST",
            dataType: "html",
            url: "/" + controllerName + "/" + methodName,
            data: dataTableModel,
            success: function (result) {
                //Rebind Grid Data
                $('#DataTablesDivId').html(result);
                $("#DataTables_PageSizeId").attr("disabled", false);
                $("#DataTables_SearchById").attr("disabled", false);
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
}

function BindDataTableModel(PageIndex) {
    $("#notificationDivId").hide();
    let dataTableModel = {
        SearchBy: $('#DataTables_SearchById').val() != undefined ? $('#DataTables_SearchById').val().trim() : "",
        SortByColumn: $('#DataTables_SortColumn').val(),
        SortBy: $('#DataTables_SortBy').val(),
        PageIndex: PageIndex,
        PageSize: $('#DataTables_PageSizeId').val(),
        SelectedCentreCode: $("#SelectedCentreCode").length > 0 ? $("#SelectedCentreCode").val() : "",
        SelectedDepartmentId: $("#SelectedDepartmentId").length > 0 ? $("#SelectedDepartmentId").val() : 0,
        SelectedParameter1: $("#SelectedParameter1").length > 0 ? $("#SelectedParameter1").val() : "",
        SelectedParameter2: $("#SelectedParameter2").length > 0 ? $("#SelectedParameter2").val() : "",
        SelectedParameter3: $("#SelectedParameter3").length > 0 ? $("#SelectedParameter3").val() : "",
        SelectedParameter4: $("#SelectedParameter4").length > 0 ? $("#SelectedParameter4").val() : "",
        SelectedParameter5: $("#SelectedParameter5").length > 0 ? $("#SelectedParameter5").val() : "",

    }
    $("#DataTables_PageSizeId").attr("disabled", true);
    $("#DataTables_SearchById").attr("disabled", true);
    return dataTableModel;
}
