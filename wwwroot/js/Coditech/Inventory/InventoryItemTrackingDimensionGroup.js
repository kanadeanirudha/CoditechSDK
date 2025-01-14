var InventoryItemTrackingDimensionGroup = {
    Initialize: function () {
        InventoryItemTrackingDimensionGroup.constructor();
    },
    constructor: function () {
    },

    SaveData: function () {
        var data = [];

        // Loop through each row in the table
        $('#makeEditable tbody tr').each(function () {
            var row = $(this);
            var rowData = {
                //InventoryItemTrackingDimensionId: row.attr('id').replace('row_', ''),
                InventoryItemTrackingDimensionGroupMapperId: parseInt(row.find('input[id^="InventoryItemTrackingDimensionGroupMapperId_"]').val()),
                InventoryItemTrackingDimensionId: parseInt(row.find('input[id^="InventoryItemTrackingDimensionId_"]').val()),
                Active: row.find('input[id^="Active_"]').prop('checked'),
                ActiveInSalesProcess: row.find('input[id^="ActiveInSalesProcess_"]').prop('checked'),
                PrimaryStocking: row.find('input[id^="PrimaryStocking_"]').prop('checked'),
                BlankReceiptAllowed: row.find('input[id^="BlankReceiptAllowed_"]').prop('checked'),
                BlankIssueAllowed: row.find('input[id^="BlankIssueAllowed_"]').prop('checked'),
                PhysicalInventory: row.find('input[id^="PhysicalInventory_"]').prop('checked'),
                FinancialInventory: row.find('input[id^="FinancialInventory_"]').prop('checked'),
                CoveragePlanByDimension: row.find('input[id^="CoveragePlanByDimension_"]').prop('checked'),
                ForPurchasePrices: row.find('input[id^="ForPurchasePrices_"]').prop('checked'),
                ForSalePrices: row.find('input[id^="ForSalePrices_"]').prop('checked'),
                Transfer: row.find('input[id^="Transfer_"]').prop('checked'),
                DisplayOrder: parseInt(row.find('input[id^="DisplayOrder_"]').val()),
                // Retrieve text content of each <td> in the row
                ItemTrackingDimensionName: row.find('td:eq(0)').text().trim(),
            };
            data.push(rowData);
        });

        // Stringify the data array
        var jsonData = JSON.stringify(data);

        //console.log('JSON data:', jsonData);
        $("#ItemTrackingDimensionGroupMapperData").val(jsonData);

        $("#frmInventoryItemTrackingDimensionGroup").submit();

    }
};
