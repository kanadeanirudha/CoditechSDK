var InventoryStorageDimensionGroup = {
    Initialize: function () {
        InventoryStorageDimensionGroup.constructor();
    },
    constructor: function () {
    },
    SaveData: function () {
        var data = [];

        // Loop through each row in the table
        $('#makeEditable tbody tr').each(function () {
            var row = $(this);
            var rowData = {
                InventoryStorageDimensionGroupMapperId: parseInt(row.find('input[id^="InventoryStorageDimensionGroupMapperId_"]').val()),
                InventoryStorageDimensionId: parseInt(row.find('input[id^="InventoryStorageDimensionId_"]').val()),
                Active: row.find('input[id^="Active_"]').prop('checked'),
                BlankReceiptAllowed: row.find('input[id^="BlankReceiptAllowed_"]').prop('checked'),
                BlankIssueAllowed: row.find('input[id^="BlankIssueAllowed_"]').prop('checked'),
                CoveragePlanByDimension: row.find('input[id^="CoveragePlanByDimension_"]').prop('checked'),
                FinancialInventory: row.find('input[id^="FinancialInventory_"]').prop('checked'),
                ForPurchasePrices: row.find('input[id^="ForPurchasePrices_"]').prop('checked'),
                ForSalePrices: row.find('input[id^="ForSalePrices_"]').prop('checked'),
                PhysicalInventory: row.find('input[id^="PhysicalInventory_"]').prop('checked'),
                PrimaryStocking: row.find('input[id^="PrimaryStocking_"]').prop('checked'),
                Reference: row.find('input[id^="Reference_"]').val().trim(),
                Transfer: row.find('input[id^="Transfer_"]').prop('checked'),
                StorageDimensionName: row.find('td:eq(0)').text().trim(),
                DisplayOrder: parseInt(row.find('input[id^="DisplayOrder_"]').val())
               
            };
            data.push(rowData);
        });

        // Stringify the data array
        var jsonData = JSON.stringify(data);

        // Set the JSON data to a form field
        $("#StorageDimensionGroupMapperData").val(jsonData);
        // Submit the form
        $("#frmInventoryStorageDimensionGroup").submit();
    }
};
