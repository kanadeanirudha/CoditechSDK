var InventoryProductDimensionGroup = {
    Initialize: function () {
        InventoryProductDimensionGroup.constructor();
    },
    constructor: function () {
    },
    SaveData: function () {
        var data = [];

        // Loop through each row in the table
        $('#makeEditable tbody tr').each(function () {
            var row = $(this);
            var rowData = {
                //InventoryProductDimensionId: row.attr('id').replace('row_', ''),
                InventoryProductDimensionGroupMapperId: parseInt(row.find('input[id^="InventoryProductDimensionGroupMapperId_"]').val()),
                InventoryProductDimensionId: parseInt(row.find('input[id^="InventoryProductDimensionId_"]').val()),
                ForPurchase: row.find('input[id^="ForPurchase_"]').prop('checked'),
                ForSale: row.find('input[id^="ForSale_"]').prop('checked'),
                IsActive: row.find('input[id^="IsActive_"]').prop('checked'),
                DisplayOrder: parseInt(row.find('input[id^="DisplayOrder_"]').val()),
                // Retrieve text content of each <td> in the row
                ProductDimensionName: row.find('td:eq(0)').text().trim(),
            };
            data.push(rowData);
        });

        // Stringify the data array
        var jsonData = JSON.stringify(data);

        //console.log('JSON data:', jsonData);
        $("#ProductDimensionGroupMapperData").val(jsonData);

        $("#frmInventoryProductDimensionGroup").submit();

    }
};
