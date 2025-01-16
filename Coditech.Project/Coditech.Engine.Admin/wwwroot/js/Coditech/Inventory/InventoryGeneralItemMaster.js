var InventoryGeneralItemMaster = {
    Initialize: function () {
        InventoryGeneralItemMaster.constructor();
    },
    constructor: function () {
    },


}

$("#ProductTypeEnumId").change(function () {
    var selectedProductType = $("#ProductTypeEnumId").val();
    var productSubtypeDropdown = $('#ProductSubTypeEnumId');
    productSubtypeDropdown.prop('selectedIndex', 0);
});