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
            <div class="Person-field" style="display: none; margin-top: 5px;">
                <input class="form-control input-sm" type="text" id="PersonId${newRowCount}" placeholder="Person Name" />
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
        $('#example tbody tr').each(function () {
            var row = $(this);
            var rowId = row.attr('id').replace("row", "");

            // Get debit and credit values
            var debitAmount = parseFloat(row.find(`#debitBal${rowId}`).val()) || 0;
            var creditAmount = parseFloat(row.find(`#creditBal${rowId}`).val()) || 0;

            // Determine flag and amount
            var debitCreditEnum = debitAmount > 0 ? 0 : 1;  // 0 for debit, 1 for credit
            var transactionAmount = debitAmount > 0 ? debitAmount : creditAmount;

            // Get selected account info
            var selectedAccount = row.find(`#AccGlName${rowId}`).data("selected-account");

            // Get UserTypeId from selected account's userTypeId
            var userTypeId = selectedAccount ? selectedAccount.userTypeId : "";  // Assign default value if not selected

            // Get PersonId from the person autocomplete (stored after person selection)
            var personId = row.find(`#PersonId${rowId}`).data("person-id") || ""; // Use `data` to retrieve the selected PersonId

            // Build row data object
            var rowData = {
                TransactionSubId: "",
                AccSetupGLId: selectedAccount?.id || 0,
                DebitCreditEnum: debitCreditEnum,
                TransactionAmount: transactionAmount,
                ChequeNo: row.find(`#AccChequeNumber${rowId}`).val() || "",
                ChequeDatetime: row.find(`#AccChequeDate${rowId}`).val() || "",
                NarrationDescription: row.find("td:eq(1) input").val() || "",
                AccGLName: row.find(`#AccGlName${rowId}`).val() || "",
                BranchName: row.find(`#AccBranchName${rowId}`).val() || "",
                PersonId: personId,  // Use selected PersonId from person autocomplete
                UserType: userTypeId || "",  // Store userTypeId from the selected account
                IsActive: 1
            };

            // Push the row data into the data array
            data.push(rowData);
        });

        // Build XML with enum-based DebitCreditEnum
        const safe = val => (val != null ? val : "");

        // Initialize XML structure
        xmlData = "<rows>";
        data.forEach(item => {
            xmlData += `
    <row>
        <TransactionSubId>${safe(item.TransactionSubId)}</TransactionSubId>
        <AccSetupGLId>${safe(item.AccSetupGLId)}</AccSetupGLId>
        <DebitCreditEnum>${safe(item.DebitCreditEnum)}</DebitCreditEnum>
        <TransactionAmount>${safe(item.TransactionAmount)}</TransactionAmount>
        <ChequeNo>${safe(item.ChequeNo)}</ChequeNo>
        <ChequeDatetime>${safe(item.ChequeDatetime)}</ChequeDatetime>
        <NarrationDescription>${safe(item.NarrationDescription)}</NarrationDescription>
        <AccGLName>${safe(item.AccGLName)}</AccGLName>
        <BranchName>${safe(item.BranchName)}</BranchName>
        <PersonId>${safe(item.PersonId)}</PersonId>  <!-- Store PersonId -->
        <UserTypeId>${safe(item.UserType)}</UserTypeId>  <!-- Store UserTypeId -->
        <IsActive>${safe(item.IsActive)}</IsActive>
    </row>`;
        });
        xmlData += "</rows>";

        // Set hidden field and global var with XML data
        AccGLTransaction.SelectedXmlData = xmlData;
        $("#TransactionDetailsData").val(xmlData);

        // Log final XML Data for debugging
        console.log("✅ Final XML Data:\n", xmlData);

        // Submit the form
        $("#frmWorkoutPlanDetails").submit();
        console.log("✅ Final XML Data submitted:\n", xmlData);
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



    InitializeAutocomplete: function (selector, transactionTypeCode) {
        transactionTypeCode = transactionTypeCode || AccGLTransaction.valuTransactionType;

        $(selector).autocomplete({
            minLength: 1,
            delay: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/AccGLTransaction/GetAccounts",
                    type: "POST",
                    data: {
                        term: request.term, // Server will filter based on this
                        maxResults: 10,
                        accountId: 0,
                        personType: "",
                        transactionTypeCode: transactionTypeCode
                    },
                    dataType: "json",
                    success: function (data) {
                        let actualData = [];

                        if (Array.isArray(data.Value)) {
                            actualData = data.Value;
                        } else if (data.Value && Array.isArray(data.Value.data)) {
                            actualData = data.Value.data;
                        }

                        const term = request.term.toLowerCase();
                        const filtered = actualData.filter(item =>
                            item.GLName.toLowerCase().includes(term)
                        );

                        const suggestions = $.map(filtered, function (item) {
                            return {
                                label: item.GLName,
                                value: item.GLName,
                                id: item.AccSetupGLId,
                                typeId: item.AccSetupGLTypeId,
                                parentId: item.ParentAccSetupGLId,
                                userTypeId: item.UserTypeId
                            };
                        });

                        console.log(suggestions);
                        response(suggestions);
                    },
                    error: function () {
                        response([]);
                    }
                });
            },
            select: function (event, ui) {
                $(selector).data("selected-account", ui.item);

                var row = $(selector).closest("tr");

                // Show/hide cheque fields based on account typeId
                if (ui.item.typeId === 5) {
                    row.find(".cheque-fields").show();
                    row.find(".Person-field").hide();
                } else {
                    row.find(".cheque-fields").hide();
                    row.find(".Person-field").hide();
                }

                // Show person field if userTypeId > 0
                if (ui.item.userTypeId > 0) {
                    console.log("Triggering person fetch...");

                    row.find(".Person-field").show();  // Display Person field
                    var $personInput = row.find(".Person-field input");
                    $personInput.val("");  // Clear previous value

                    // Trigger person autocomplete based on selected account's userTypeId
                    $personInput.autocomplete({
                        minLength: 1,
                        delay: 0,
                        source: function (request, response) {
                            $.ajax({
                                url: "/AccGLTransaction/GetPersonsByUserType",
                                type: "POST",
                                data: {
                                    term: request.term,
                                    userTypeId: ui.item.userTypeId // Use the selected account's userTypeId
                                },
                                dataType: "json",
                                success: function (data) {
                                    let actualData = [];
                                    console.log(data);

                                    if (Array.isArray(data.Value)) {
                                        actualData = data.Value;
                                    } else if (data.Value && Array.isArray(data.Value.data)) {
                                        actualData = data.Value.data;
                                    }

                                    const term = request.term.toLowerCase();
                                    const filtered = actualData.filter(item =>
                                        item.PersonName.toLowerCase().includes(term)
                                    );

                                    const suggestions = $.map(filtered, function (item) {
                                        return {
                                            label: item.PersonName,
                                            value: item.PersonName,
                                            PersonId: item.PersonId, // Store PersonId
                                            userTypeId: item.UserTypeId // Store UserTypeId
                                        };
                                    });

                                    console.log(suggestions);
                                    response(suggestions);
                                },
                                error: function () {
                                    response([]);
                                }
                            });
                        },
                        select: function (event, person) {
                            var selectedPerson = person.item;

                            // Save selected PersonId and UserTypeId on the input field
                            $personInput.data("person-id", selectedPerson.PersonId); // Store PersonId
                            $personInput.data("user-type-id", selectedPerson.userTypeId); // Store UserTypeId

                            console.log("Selected PersonId:", selectedPerson.PersonId);
                            console.log("Selected UserTypeId:", selectedPerson.userTypeId);

                            // You can now access the stored PersonId and UserTypeId like this:
                            var personId = $personInput.data("person-id");
                            var userTypeId = $personInput.data("user-type-id");

                            console.log("Stored PersonId from input field:", personId);
                            console.log("Stored UserTypeId from input field:", userTypeId);
                        }
                    });
                } else {
                    row.find(".Person-field").hide();
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
            $(this).closest("tr").remove();
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