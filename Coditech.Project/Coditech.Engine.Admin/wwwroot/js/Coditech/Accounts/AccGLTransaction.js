var AccGLTransaction = {
    SelectedXmlData: null,
    map: {},
    map2: {},
    flag: true,
    rowCount: 0,
    valuTransactionType: null,

    Initialize: function () {
        this.BindEvents();

    },

    GetFinancialYearListByCentreCode: function () {
        var selectedCentreCode = $("#SelectedCentreCode").val();
        if (selectedCentreCode !== "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/AccGLTransaction/GetFinancialYearListByCentreCode",
                data: { "selectedCentreCode": selectedCentreCode },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#GeneralFinancialYearId").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr) {
                    if (xhr.status === 401 || xhr.status === 403) {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve BalanceSheet.", "error");
                    CoditechCommon.HideLodder();
                }
            });
        } else {
            CoditechNotification.DisplayNotificationMessage("Please select Centre and Balance Sheet Type.", "error");
        }
    },

    getData: function (query, process, valuTransactionType) {
        console.log("✅ Selected Transaction Type Code:", valuTransactionType);

        $.ajax({
            url: "/AccGLTransaction/GetAccSetupGLAccountList",
            type: "POST",
            data: {
                term: query,
                maxResults: 10,
                accountId: 0,
                personType: "",
                transactionTypeCode: valuTransactionType
            },
            dataType: "json",
            beforeSend: function () {
                console.log("🚀 Sending Data:", {
                    term: query,
                    transactionTypeCode: valuTransactionType
                });
            },
            success: function (response) {
                console.log("✅ Full Response:", response);

                if (!response || !response.Value || !response.Value.data) {
                    console.error("❌ Error: Response structure is incorrect", response);
                    return;
                }

                var actualData = response.Value.data;
                console.log("🔄 Processed Data Array:", actualData);

                if (!Array.isArray(actualData) || actualData.length === 0) {
                    console.warn("⚠️ No data found in response.Value.data!");
                    return;
                }

                var suggestions = [];

                $.each(actualData, function (i, transaction) {
                    console.log("🛠 Debugging Each Item:", transaction);

                    if (transaction && transaction.GLName) {
                        console.log("🟢 Found GLName:", transaction.GLName);
                        AccGLTransaction.map[transaction.GLName, transaction.AccSetupGLId] = transaction;
                        suggestions.push(transaction.GLName, transaction.AccSetupGLId);

                    } else {
                        console.warn("⚠️ No GLName found in this item:", transaction);
                    }
                });

                console.log("✅ Final Suggestions:", suggestions);
                process(suggestions);
            }
        });
    },
    AddRow: function () {
        var tableLength = $('#example tbody tr').length;
        var newRowCount = tableLength + 1;
        var valuTransactionType = $('#AccSetupTransactionTypeId').val();
        console.log("✅ Inside AddRow: Selected Transaction Type Code:", valuTransactionType);

        if (tableLength === 0) {
            $("#debitBal").val(0);
            $("#creditBal").val(0);
        }

        $('#tableDebitCredit').show();
        $('#example tbody tr td input[type=text]').attr('disabled', true);

        $("#example tbody").append(
            `
            <tr id="row${newRowCount}">
    <!-- Account Name -->
    <td>
        <input id="AccGlName${newRowCount}" class="form-control input-sm typeahead" placeholder="Search Account*" type="text" maxlength="200" />
        <div class="cheque-fields" style="display: none; margin-top: 5px;">
            <input class="form-control input-sm" type="text" id="AccBranchName${newRowCount}" placeholder="Branch Name" />
        </div>
    </td>

    <!-- Narration -->
    <td>
        <input class="form-control input-sm" type="text" maxlength="500" placeholder="Narration" />
        <div class="cheque-fields" style="display: none; margin-top: 5px;">
            <input class="form-control input-sm" type="text" id="AccChequeNumber${newRowCount}" placeholder="Cheque Number" />
            <input class="form-control input-sm" type="date" id="AccChequeDate${newRowCount}" />
        </div>
    </td>

    <!-- Debit & Credit Fields -->
    <td><input class="form-control input-sm debit-field validate-number" type="text" id="debitBal${newRowCount}" maxlength="15" value="0" /></td>
    <td><input class="form-control input-sm credit-field validate-number" type="text" id="creditBal${newRowCount}" maxlength="15" value="0" /></td>

    <!-- Actions -->
    <td>
        <a href="#" class="btn btn-sm btn-soft-success edit-row" title="Edit"><i class="fas fa-edit"></i></a>
        <a href="#" class="btn btn-sm btn-soft-danger remove-row" title="Delete"><i class="fas fa-trash-alt"></i></a>
    </td>
</tr>`
        );

        AccGLTransaction.valuTransactionType = valuTransactionType;
        AccGLTransaction.InitializeAutocomplete("#AccGlName" + newRowCount, valuTransactionType);
        AccGLTransaction.calculateTotals();
    },
    SaveData: function () {
        var data = [];

        $('#example tbody tr').each(function () { // Loop through each row
            var row = $(this);
            var rowId = row.attr('id').replace("row", ""); // Extract row number dynamically

            var debitAmount = parseFloat(row.find(`#debitBal${rowId}`).val()) || 0; // Get debit amount
            var creditAmount = parseFloat(row.find(`#creditBal${rowId}`).val()) || 0; // Get credit amount

            var rowData = {
                AccGlName: row.find(`#AccGlName${rowId}`).val() || "", // Correct input selector
                Narration: row.find("td:eq(1) input").val() || "", // Get narration field
                DebitAmount: debitAmount,
                CreditAmount: creditAmount,
                TransactionAmount: debitAmount + creditAmount // Corrected calculation
            };

            data.push(rowData);
        });

        var jsonData = JSON.stringify(data);

        $("#TransactionDetailsData").val(jsonData);
        console.log('🚀 JSON Data:', jsonData);

        $("#frmWorkoutPlanDetails").submit();
    },

    editRow: function (row) {
        row.find("input").prop("disabled", false);
    },

    calculateTotals: function () {
        let totalDebit = 0, totalCredit = 0;

        $(".debit-field").each(function () {
            let debitValue = parseFloat($(this).val()) || 0;
            totalDebit += debitValue;
        });

        $(".credit-field").each(function () {
            let creditValue = parseFloat($(this).val()) || 0;
            totalCredit += creditValue;
        });

        console.log("🔢 Total Debit:", totalDebit, " | Total Credit:", totalCredit);

        $("#debitBal").val(totalDebit.toFixed(2));
        $("#creditBal").val(totalCredit.toFixed(2));

        // Check if debit equals credit
        if (totalDebit !== totalCredit) {
            $("#debitBal, #creditBal").css("border", "2px solid red");
            console.warn("⚠️ Debit and Credit do not match!");
        } else {
            $("#debitBal, #creditBal").css("border", "");
        }
    },

    //calculateTotals: function () {
    //    var totalDebit = 0, totalCredit = 0;

    //    $(".debit-field").each(function () {
    //        totalDebit += parseFloat($(this).val()) || 0;
    //    });

    //    $(".credit-field").each(function () {
    //        totalCredit += parseFloat($(this).val()) || 0;
    //    });

    //    $("#debitBal").val(totalDebit.toFixed(2));
    //    $("#creditBal").val(totalCredit.toFixed(2));
    //},

    InitializeAutocomplete: function (selector, transactionTypeCode) {
        transactionTypeCode = transactionTypeCode || AccGLTransaction.valuTransactionType;
        console.log("✅ Using Stored TransactionTypeCode:", transactionTypeCode);

        $(selector).autocomplete({
            source: function (request, response) {
                console.log("🔍 Sending AJAX Request with TransactionType:", transactionTypeCode);

                $.ajax({
                    url: "/AccGLTransaction/GetAccounts",
                    type: "POST",
                    data: {
                        term: request.term,
                        maxResults: 10,
                        accountId: 0,
                        personType: "",
                        transactionTypeCode: transactionTypeCode
                    },
                    dataType: "json",
                    success: function (data) {
                        console.log("✅ Received Data:", data);

                        let actualData = [];
                        if (Array.isArray(data.Value)) {
                            actualData = data.Value;
                        } else if (data.Value && Array.isArray(data.Value.data)) {
                            actualData = data.Value.data;
                        } else {
                            console.error("❌ Invalid Data Format:", data);
                            response([]);
                            return;
                        }

                        var suggestions = $.map(actualData, function (item) {
                            console.log("🟢 Adding Suggestion:", item.GLName);
                            return {
                                label: item.GLName,
                                value: item.GLName,
                                id: item.AccSetupGLId, // If you need the ID
                                typeId: item.AccSetupGLTypeId,
                                parentId: item.ParentAccSetupGLId
                            };
                        });

                        console.log("✅ Final Suggestions:", suggestions);

                        if (suggestions.length > 0) {
                            console.log("🔽 Setting Autocomplete Source:", suggestions);
                        } else {
                            console.warn("⚠️ No suggestions found.");
                        }

                        response(suggestions);
                        $(selector).autocomplete("option", "source", suggestions);
                    }
                });
            },
            select: function (event, ui) {
                console.log("✅ Selected Account:", ui.item);

                // Store selected account data
                $(selector).data("selected-account", ui.item);

                // Find the closest row to apply the logic correctly
                var row = $(selector).closest("tr");

                // Check if typeId is 5
                if (ui.item.typeId === 5) {
                    console.log("🔄 Showing extra fields for typeId 5");

                    // Show cheque-related fields
                    row.find(".cheque-fields").show();
                } else {
                    console.log("🔄 Hiding extra fields for other types");

                    // Hide cheque-related fields
                    row.find(".cheque-fields").hide();
                }
            }
        });
    },

    BindEvents: function () {
        $(document).keydown(function (e) {
            if (e.altKey && e.shiftKey) {
                const keyMap = {
                    113: 'N ',  // Alt + Shift + F2 → New Transaction
                    114: 'D ',  // Alt + Shift + F3 → Delete Transaction
                    115: 'C ',  // Alt + Shift + F4 → Credit Transaction
                    116: 'P ',  // Alt + Shift + F5 → Payment Transaction
                    117: 'R ',  // Alt + Shift + F6 → Receipt Transaction
                    118: 'J ',  // Alt + Shift + F7 → Journal Transaction
                    119: 'S ',  // Alt + Shift + F8 → Sales Transaction
                    120: 'H '   // Alt + Shift + F9 → History
                };

                if (keyMap[e.keyCode]) {
                    $("#SelectedTransactionType").val(keyMap[e.keyCode]);
                    AccGLTransaction.valuTransactionType();
                    return false; // Prevent default browser action
                }
            }

            if (e.altKey) {
                const altKeyMap = {
                    65: "#addRowButton",           // Alt + A → Add Row
                    83: "#btnSave",          // Alt + S → Save Transaction
                    114: "#remove-row",        // Alt + Delete → Delete Row
                    69: "#edit-row",          // Alt + E → Edit Row
                    80: "#btnPrint",         // Alt + P → Print Transaction
                    76: "#btnClear",         // Alt + L → Clear Form
                    82: "#btnRefresh",       // Alt + R → Refresh Page
                    78: "#btnNewTransaction",// Alt + N → New Transaction
                    84: "#btnSubmit",        // Alt + T → Submit Transaction
                    72: "#btnHelp"           // Alt + H → Help
                };

                if (altKeyMap[e.keyCode]) {
                    $(altKeyMap[e.keyCode]).click();
                    return false;
                }
            }
        });
        $(document).on("input", ".debit-field", function () {
            let row = $(this).closest("tr");
            row.find(".credit-field").val(0); // Reset credit field
            AccGLTransaction.calculateTotals();
        });

        $(document).on("input", ".credit-field", function () {
            let row = $(this).closest("tr");
            row.find(".debit-field").val(0); // Reset debit field
            AccGLTransaction.calculateTotals();
        });

        $(document).on("click", ".remove-row", function () {
            var rowId = $(this).data("rowid");
            $("#" + rowId).remove();
            AccGLTransaction.calculateTotals();
        });

        $('#btnAdd').on('click', function () {
            $('#ResetAccountTransactionMasterRecord').show();
            $('#CreateAccountTransactionMasterRecord').show();
            AccGLTransaction.AddRow();
        });


        $(document).on("keypress", ".validate-number", function (e) {
            var charCode = e.which ? e.which : e.keyCode;
            if ((charCode < 48 || charCode > 57) && charCode !== 46) e.preventDefault();
            if (charCode === 46 && $(this).val().includes(".")) e.preventDefault();
        });

        $(document).on("click", ".edit-row", function (e) {
            e.preventDefault();
            var row = $(this).closest("tr");
            console.log("🛠 Editing Row:", row.attr("id"));
            row.find("input").prop("disabled", false);
        });
    }
};

// ✅ Initialize on Page Load
$(document).ready(function () {
    AccGLTransaction.InitializeAutocomplete(".typeahead");
    AccGLTransaction.Initialize();
});