function Initialize() {
}
var AccSetupGL = {

    Initialize: function () {
        this.constructor();
    },

    constructor: function () {

        $(document).ready(function () {
            // Call a global Initialize function if defined
            if (typeof Initialize === 'function') {
                Initialize();
            } else {
            }
            $(document).ready(function () {
                // Live validation + auto-uppercase
                $(document).on('input', '#IFSCCode', function () {
                    const regex = /^[A-Z]{4}0[A-Z0-9]{6}$/;
                    const $input = $(this);
                    const $error = $("[data-valmsg-for='IFSCCode']");

                    // Convert to uppercase
                    $input.val($input.val().toUpperCase());

                    const ifscCode = $input.val();

                    // Validation messages
                    if (!ifscCode) {
                        $error.text("IFSC code is required.");
                    } else if (!regex.test(ifscCode)) {
                        $error.text("Invalid IFSC code format.");
                    } else {
                        $error.text(""); // Valid
                    }
                });
            });


            $('#addChildForm').on('submit', function (e) {
                e.preventDefault();
                let mode = $(this).data('mode'); // ✅ Correct data attribute
                console.log('Form Mode:', mode);

                // Get form action from the form's 'action' attribute
                let actionUrl = $(this).attr('action');

                if (mode === 'edit') {

                    // 👉 Call SaveAccountSetupGL when editing 
                    AccSetupGL.SaveAccountSetupGL();

                } else if (mode === 'create') {
                    AccSetupGL.AddChild();
                    // 👉 Call AddChild when editing
                }
            });
            $('#addForm').on('submit', function (e) {
                e.preventDefault();
                let formData = $(this).serialize();
                let mode = $(this).data('mode'); // ✅ Correct data attribute

                // Get form action from the form's 'action' attribute
                let actionUrl = $(this).attr('action');
                if (mode === 'edit') {

                    // 👉 Call AddChild when creating

                    $.post('/AccSetupGL/SaveAccountSetupGL', formData, function (response) {
                        if (response.success) {
                            alert(response.message, "asgag");
                        } else {
                        }
                    });
                }
                else if (mode === 'create') {
                }
            });
            function handleResponse(response) {
                $(".text-danger").text("");

                if (response.success) {
                    $('#addChildModal').modal('hide');
                    CoditechNotification.DisplayNotificationMessage(response.message, "success");

                    let url = window.location.origin + window.location.pathname +
                        '?selectedCentreCode=' + (response.selectedCentreCode || '') +
                        '&accSetupBalanceSheetTypeId=' + (response.accSetupBalanceSheetTypeId || '') +
                        '&accSetupBalancesheetId=' + (response.accSetupBalancesheetId || '');

                    setTimeout(function () {
                        window.location.href = url;
                    }, 1000);
                } else {
                    if (response.errors && response.errors.length > 0) {
                        response.errors.forEach(function (error) {
                            $("[data-valmsg-for='" + error.Field + "']").text(error.Message);
                        });
                    } else {
                        CoditechNotification.DisplayNotificationMessage(response.message, "error");
                    }
                }
            }
            // If global variable actionMode is set (from Razor view) and equals "Create", hide buttons
            if (typeof actionMode !== 'undefined' && actionMode === "Create") {
                $(".add-child-btn, .del-child-btn").hide();
            }
            // Hide delete buttons for system-generated records
            $(".del-child-btn").each(function () {
                var isSystemGenerated = $(this).data("systemgenerated");
                if (isSystemGenerated === true || isSystemGenerated === "true") {
                    $(this).hide();
                }
            });
            $('#addChildForm').modal('hide').on('hidden.bs.modal', function () {
                $('#addChildForm')[0].reset();
                $('#AddForm').empty();
                $('#bankContainer').empty();
                $(".text-danger").text("");
                console.log("🧹 3Modal fully reset on close");
                let url = window.location.origin + window.location.pathname +
                    '?selectedCentreCode=' + ($("#SelectedCentreCode").val() || '') +
                    '&accSetupBalanceSheetTypeId=' + ($("#AccSetupBalanceSheetTypeId").val() || '') +
                    '&accSetupBalancesheetId=' + ($("#AccSetupBalancesheetId").val() || '');

                setTimeout(function () {
                    window.location.href = url;
                }, 100);
                $(this).off('hidden.bs.modal'); // Remove handler to avoid multiple bindings
            });
            $('#addChildForm').on('data-bs-dismiss', function () {
                $('#addChildForm')[0].reset();
                $('#bankContainer').empty();
                $('#AddForm').empty();
                $(".text-danger").text("");
                //location.reload();
                console.log("🧹 4Modal reset on cancel");
            });
            // Bind change event for the GL Type dropdown
            $("#AccSetupGLTypeId").on("change", function () {
                let value = $(this).val();
                if (!value || value === "0") {
                    $("[data-valmsg-for='AccSetupGLTypeId']").text("Please select a GL Type.");
                } else {
                    $("[data-valmsg-for='AccSetupGLTypeId']").text("");
                }
            });
            // Bind click event for adding a child account
            $(document).off("click", ".add-child-btn").on("click", ".add-child-btn", function () {
                var parentId = $(this).data('parentid');
                var categoryId = $(this).data('categoryid');
                AccSetupGL.OpenAddChildModal(parentId, categoryId);
            });
            // Bind click event for editing an account
            $(document).off("click", ".edit-btn").on("click", ".edit-btn", function () {
                var glId = $(this).data('glid');
                var parentId = $(this).data('parentid');
                var categoryId = $(this).data('categoryid');
                AccSetupGL.OpenEditChildModal(glId, parentId, categoryId);
            });
            // Bind click event for submitting a new child account
            $(document).off("click", "#submitAddChild").on("click", "#submitAddChild", function () {
            });
            // Bind click event for delete button
            $(document).off("click", ".del-child-btn").on("click", ".del-child-btn", function () {
                var glId = $(this).data("glid");
                var subAccounts = $("#gl-subaccounts-" + glId);
                if (subAccounts.length > 0 && subAccounts.children().length > 0) {
                    CoditechNotification.DisplayNotificationMessage("This record has sub records. Please delete the sub records first.", "error");
                    setTimeout(function () { $("#notificationDivId").fadeOut(); }, 3000);
                    return;
                }
                // Remove any existing confirmation modal
                $("#confirmDeleteModal").remove();
                var modalHtml = `
                    <div class="modal fade" id="confirmDeleteModal" tabindex="-1" role="dialog" aria-labelledby="confirmDeleteModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-dialog-centered" style="max-width: 500px;" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Confirm Delete</h5>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                </div>
                                <div class="modal-body">
                                    <span style="font-size: 24px; margin-right: 10px;">
                                        <i class="fas fa-exclamation-triangle text-danger"></i>
                                    </span>
                                    <span style="font-size: 16px;">Are you sure you want to delete this GL account?</span>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                    <button type="button" class="btn btn-outline-danger" id="confirmDelete" data-glid="${glId}">Delete</button>
                                </div>
                            </div>
                        </div>
                    </div>`;
                $("body").append(modalHtml);
                $("#confirmDeleteModal").modal("show");
            });

            // Bind click event for confirmation of delete
            $(document).off("click", "#confirmDelete").on("click", "#confirmDelete", function () {
                var glId = $(this).data("glid");
                $.ajax({
                    type: "POST",
                    url: "/AccSetupGL/Delete",
                    data: { accSetupGLIds: glId },
                    success: function (response) {
                        var notificationStyle = response.success ? "bg-success" : "bg-danger";
                        if (response.success) {
                            $("#gl-" + glId).remove();
                        }
                        $("#confirmDeleteModal").modal("hide");
                        setTimeout(function () {
                            $("#confirmDeleteModal").remove();
                        }, 500);
                        $("#notificationDivId")
                            .removeClass("bg-success bg-danger bg-info bg-warning")
                            .addClass(notificationStyle)
                            .show()
                            .delay(3000)
                            .fadeOut(1000);
                        CoditechNotification.DisplayNotificationMessage(response.message, response.success ? "success" : "error");
                    },
                    error: function () {
                        $("#confirmDeleteModal").modal("hide");
                        setTimeout(function () {
                            $("#confirmDeleteModal").remove();
                        }, 500);
                        $("#notificationDivId")
                            .removeClass("bg-success bg-danger bg-info bg-warning")
                            .addClass("bg-danger")
                            .show()
                            .delay(3000)
                            .fadeOut(1000);
                        CoditechNotification.DisplayNotificationMessage("An error occurred while deleting.", "error");
                    }
                });
            });
        });
    },

    SaveAccountSetupGL: function () {
        let formData = new FormData();
        let accSetupGLTypeId = $('select[name="AccSetupGLTypeId"]').val();

        // ✅ Always append common fields
        formData.append('AccSetupGLId', $('input[name="AccSetupGLId"]').val());
        formData.append('ParentAccSetupGLId', $('input[name="ParentAccSetupGLId"]').val());
        formData.append('AccSetupCategoryId', $('input[name="AccSetupCategoryId"]').val());
        formData.append('GLName', $('input[name="GLName"]').val());
        formData.append('GLCode', $('input[name="GLCode"]').val());
        formData.append('AccSetupGLTypeId', accSetupGLTypeId);
        formData.append('IsGroup', $('input[name="IsGroup"]').prop('checked'));
        formData.append('UserTypeId', $('select[name="UserTypeId"]').val());
        formData.append('AccSetupBalancesheetId', $('input[name="AccSetupBalancesheetId"]').val());
        formData.append('AccSetupBalanceSheetTypeId', $('input[name="AccSetupBalanceSheetTypeId"]').val());
        formData.append('AccSetupChartOfAccountTemplateId', $('input[name="AccSetupChartOfAccountTemplateId"]').val());
        // ✅ If AccSetupGLTypeId is 5 (Bank Type), include bank fields
        if (accSetupGLTypeId == 5) {
            formData.append('BankAccountName', $('input[name="BankAccountName"]').val());
            formData.append('BankAccountNumber', $('input[name="BankAccountNumber"]').val());
            formData.append('BankBranchName', $('input[name="BankBranchName"]').val());
            formData.append('IFSCCode', $('input[name="IFSCCode"]').val());
        }
        $.ajax({
            url: '/AccSetupGL/SaveAccountSetupGL',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    $('#addChildModal').modal('hide');
                    CoditechNotification.DisplayNotificationMessage(response.message, "success");
                    let url = window.location.origin + window.location.pathname +
                        '?selectedCentreCode=' + ($("#SelectedCentreCode").val() || '') +
                        '&accSetupBalanceSheetTypeId=' + ($("#AccSetupBalanceSheetTypeId").val() || '') +
                        '&accSetupBalancesheetId=' + ($("#AccSetupBalancesheetId").val() || '');
                    setTimeout(function () {
                        window.location.href = url;
                    }, 100);

                } else {
                    CoditechNotification.DisplayNotificationMessage("error");
                }
            },
            error: function (xhr) {
                alert("An error occurred while saving data.");
            }
        });
    },
    // Opens the modal to add a child GL account
    OpenAddChildModal: function (parentId, categoryId) {
        $('#addChildModal').attr('data-mode', 'create'); // ✅ Set mode to 'create'

        $('input[name="ParentAccSetupGLId"]').val(parentId);
        $('input[name="AccSetupCategoryId"]').val(categoryId);
        $('#childGLName, #childGLCode').val("");
        $('#addChildModal').modal('show');
        var balanceSheetId = $("#AccSetupBalancesheetId").val();
        var balanceSheetTypeId = $("#AccSetupBalanceSheetTypeId").val();
        var chartTemplateId = $("#AccSetupChartOfAccountTemplateId").val();
        var selectedCentreCode = $("#SelectedCentreCode").val();
        $('input[name="AccSetupBalancesheetId"]').val(balanceSheetId);
        $('input[name="AccSetupBalanceSheetTypeId"]').val(balanceSheetTypeId);
        $('input[name="AccSetupChartOfAccountTemplateId"]').val(chartTemplateId);
        $('input[name="SelectedCentreCode"]').val(selectedCentreCode);
        $('#addChildModal').data('mode');
    },

    // Adds a child GL account via AJAX
    AddChild: function () {
        $(".text-danger").text(""); // Clear previous validation messages
        let isValid = true;
        let accSetupGLTypeId = $("#AccSetupGLTypeId").val().trim();
        // ✅ Validate AccSetupGLTypeId
        if (!accSetupGLTypeId || accSetupGLTypeId === "0" || accSetupGLTypeId === "-------AccSetup GL Type-------") {
            $("[data-valmsg-for='AccSetupGLTypeId']").text("Please select a GL Type.");
            isValid = false;
        } else {
            $("[data-valmsg-for='AccSetupGLTypeId']").text(""); // Clear error message if valid
        }
        // ✅ Validate GL Name
        let glName = $("#GLName").val().trim();
        if (!glName) {
            $("[data-valmsg-for='GLName']").text("Ledger name is required.");
            isValid = false;
        } else {
            $("[data-valmsg-for='GLName']").text("");
        }
        // ✅ Validate GL Code
        let glCode = $("#GLCode").val().trim();
        if (!glCode) {
            $("[data-valmsg-for='GLCode']").text("Code is required.");
            isValid = false;
        } else {
            $("[data-valmsg-for='GLCode']").text("");
        }
        // ✅ If AccSetupGLTypeId is 5, apply extra bank-related validation
        let bankModel = null;
        if (accSetupGLTypeId === "5") {
            let bankAccountName = $('#BankAccountName').val().trim();
            let bankBranchName = $('#BankBranchName').val().trim();
            let ifscCode = $('#IFSCCode').val().trim().toUpperCase();  // Force uppercase here
            let bankAccountNumber = $('#BankAccountNumber').val().trim();

            let regex = /^[A-Z]{4}0[A-Z0-9]{6}$/;  // IFSC regex (case-sensitive, so input forced uppercase)

            let isValid = true;  // You should initialize isValid if not already

            if (!bankAccountName) {
                $("[data-valmsg-for='BankAccountName']").text("Bank account name is required.");
                isValid = false;
            } else {
                $("[data-valmsg-for='BankAccountName']").text("");
            }

            if (!bankBranchName) {
                $("[data-valmsg-for='BankBranchName']").text("Bank branch name is required.");
                isValid = false;
            } else {
                $("[data-valmsg-for='BankBranchName']").text("");
            }

            if (!ifscCode) {
                $("[data-valmsg-for='IFSCCode']").text("IFSC code is required.");
                isValid = false;
            } else if (!regex.test(ifscCode)) {
                $("[data-valmsg-for='IFSCCode']").text("Invalid IFSC code format.");
                isValid = false;
            } else {
                $("[data-valmsg-for='IFSCCode']").text("");
            }

            if (!bankAccountNumber) {
                $("[data-valmsg-for='BankAccountNumber']").text("Bank account number is required.");
                isValid = false;
            } else {
                $("[data-valmsg-for='BankAccountNumber']").text("");
            }

            if (isValid) {
                // Store bank details if valid
                bankModel = {
                    AccSetupBalanceSheetId: $('#AccSetupBalanceSheetId').val(),
                    AccSetupGLId: $('#AccSetupGLId').val(),
                    BankAccountName: bankAccountName,
                    BankBranchName: bankBranchName,
                    IFSCCode: ifscCode,
                    BankAccountNumber: bankAccountNumber
                };
            }
        }

        // ✅ Store JSON Data in Hidden Field Only If AccSetupGLTypeId is 5
        if (bankModel) {
            let jsonData = JSON.stringify(bankModel);
            $("#BankModelData").val(jsonData);
        } else {
            $("#BankModelData").val(""); // Clear if bank details are not required
        }
        // ✅ Final Check Before Submitting
        if (!isValid) return;
        // ✅ Proceed with Form Submission
        var formData = $("#addChildForm").serialize();

        $.ajax({
            type: "POST",
            url: "/AccSetupGL/AddChild",
            data: formData,
            success: function (response) {
                $(".text-danger").text("");
                if (response.success) {
                    $('#addChildModal').modal('hide');
                    CoditechNotification.DisplayNotificationMessage(response.message, "success");
                    let url = window.location.origin + window.location.pathname +
                        '?selectedCentreCode=' + (response.selectedCentreCode || '') +
                        '&accSetupBalanceSheetTypeId=' + (response.accSetupBalanceSheetTypeId || '') +
                        '&accSetupBalancesheetId=' + (response.accSetupBalancesheetId || '');
                    setTimeout(function () {
                        window.location.href = url;
                    }, 1000);
                } else {
                    $('#addChildModal').modal('hide');
                    if (response.errors && response.errors.length > 0) {
                        response.errors.forEach(function (error) {
                            $("[data-valmsg-for='" + error.Field + "']").text(error.Message);
                        });
                    } else {
                        $('#addChildModal').modal('hide');
                        CoditechNotification.DisplayNotificationMessage(response.message, "error");
                    }
                }
            },
            error: function () {
                $('#addChildModal').modal('hide');
                CoditechNotification.DisplayNotificationMessage("An error occurred while adding the record. Please try again.", "error");
            }
        });
    },

    OpenCreateChildModal: function (parentId, categoryId) {
        $('#addChildForm')[0].reset();
        $('#addChildForm').attr('data-mode', 'create'); // Set mode to Create
        $('#addChildModal').modal('show');
    },
    OpenEditChildModal: function (glId, parentId, categoryId) {
        $('input[name="AccSetupGLId"]').val(glId);
        $('input[name="ParentAccSetupGLId"]').val(parentId);
        $('input[name="AccSetupCategoryId"]').val(categoryId);
        AccSetupGL.loadAccountSetupGL(glId);
    },

    loadAccountSetupGL: function (accSetupGLId) {
        $.ajax({
            type: "GET",
            url: "/AccSetupGL/LoadAccountSetupGL",
            data: { accSetupGLId: accSetupGLId },
            success: function (response) {
                if (response.success) {
                    // ✅ Populate general fields
                    $('input[name="GLName"]').val(response.data.glName || '');
                    $('input[name="GLCode"]').val(response.data.glCode || '');
                    $('select[name="AccSetupGLTypeId"]').val(response.data.accSetupGLTypeId || '').change();
                    $('select[name="UserTypeId"]').val(response.data.userTypeId || '').change();
                    $('input[name="IsGroup"]').prop('checked', !!response.data.isGroup);
                    $('input[name="IsGroup"]').prop('checked', response.data.isGroup);
                    $('#addChildForm').attr('data-mode', 'edit'); // Set mode to Edit

                    if (response.data.accSetupGLTypeId !== 5 && response.data.accSetupGLTypeId !== 4) {
                        $('select[name="UserTypeId"]').val(response.data.userTypeId || $('select[name="UserTypeId"] option:first').val()).change();
                    } else {
                        $('select[name="UserTypeId"]').val(response.data.userTypeId || '').change();
                    }

                    $('input[name="AccSetupBalancesheetId"]').val(response.data.accSetupBalancesheetId || '');
                    $('input[name="AccSetupBalanceSheetTypeId"]').val(response.data.accSetupBalanceSheetTypeId || '');
                    $('input[name="AccSetupChartOfAccountTemplateId"]').val(response.data.accSetupChartOfAccountTemplateId || '');
                    $('input[name="AccSetupGLTypeId"]').val(response.data.accSetupGLTypeId || '');

                    setTimeout(() => {
                        $('input[name="BankAccountName"]').val(response.data['bankAccountName'] || '');
                        $('input[name="BankAccountNumber"]').val(response.data['bankAccountNumber'] || '')
                            .attr('disabled', 'disabled');
                        $('input[name="BankBranchName"]').val(response.data['bankBranchName'] || '');
                        $('input[name="IFSCCode"]').val(response.data['iFSCCode'] || '').attr('disabled', 'disabled');
                    }, 100);

                    // ✅ Show Bank Fields only if accSetupGLTypeId is 5
                    if (response.data.accSetupGLTypeId === 5) {
                        $('#bankContainer').empty().html(`
                    <div class="flex flex-col space-y-2">
                        <label class="form-label required">Bank Account Name</label>
                        <input type="text" name="BankAccountName" class="form-control" value="${response.data['bankAccountName'] || ''}" />
                    </div>
                    <div class="flex flex-col space-y-2">
                        <label class="form-label required">Bank Account Number</label>
                        <input type="text" name="BankAccountNumber" class="form-control" value="${response.data['bankAccountNumber'] || ''}" disabled />
                    </div>
                    <div class="flex flex-col space-y-2">
                        <label class="form-label required">Bank Branch Name</label>
                        <input type="text" name="BankBranchName" class="form-control" value="${response.data['bankBranchName'] || ''}" />
                    </div>
                    <div class="flex flex-col space-y-2">
                        <label class="form-label required">IFSC Code</label>
                        <input type="text" name="IFSCCode" class="form-control" value="${response.data['iFSCCode'] || ''}" />
                    </div>
                `);
                    } else {
                        // Clear bank fields if not a bank
                        $('#bankContainer').html('');
                    }

                    // Call renderChildModel after loading data
                    AccSetupGL.renderChildModel(response.data);

                } else {
                    CoditechNotification.DisplayNotificationMessage(response.message, "error");
                }
            },
            error: function (xhr, status, error) {
                CoditechNotification.DisplayNotificationMessage("An error occurred while loading data.", "error");
            }
        });
    },

    // ✅ Move renderChildModel OUTSIDE the loadAccountSetupGL function
    renderChildModel: function (model) {
        $.ajax({
            url: '/AccSetupGL/RenderChildModel',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(model),
            success: function (html) {
                $('#addChildForm').html(html);
                if (model.accSetupGLTypeId === 5) {
                    $('#bankContainer').empty().html(`
                                        <div class="flex flex-col space-y-2">
                                            <label class="form-label required">Account Name</label>
                                            <input type="text" name="BankAccountName" class="form-control" value="${model.bankAccountName || ''}" disabled  />
                                        </div>
                                        <div class="flex flex-col space-y-2">
                                            <label class="form-label required">Account Number</label>
                                            <input type="text" name="BankAccountNumber" class="form-control" value="${model.bankAccountNumber || ''}" disabled />
                                        </div>
                                        <div class="flex flex-col space-y-2">
                                            <label class="form-label required">Branch Name</label>
                                            <input type="text" name="BankBranchName" class="form-control" value="${model.bankBranchName || ''}" />
                                        </div>
                                        <div class="flex flex-col space-y-2">
                                            <label class="form-label required">IFSC Code</label>
                                            <input type="text" name="IFSCCode" class="form-control" value="${model.iFSCCode || ''}" disabled />
                                        </div>
                                    `);
                    $('#addChildModal').modal('show');
                } else {
                    $('#addChildModal').modal('show');
                    // ✅ Clear bank fields if not a bank
                    $('#bankContainer').html('');

                }// ✅ Open modal after rendering
            },
            error: function () {
                alert('Failed to render form.');
            }
        });
    },

    GetAccSetupBalanceSheetId: function () {
        $('#DataTables_SearchById').val("");
        if ($("#SelectedCentreCode").val() == "") {
            CoditechNotification.DisplayNotificationMessage("Please select centre.", "error");
        } else if ($("#AccSetupBalanceSheetTypeId").val() == "") {
            CoditechNotification.DisplayNotificationMessage("Please select balance sheet type.", "error");
        } else {
            CoditechDataTable.LoadList("AccSetupGL", "GetAccSetupGL");
        }
    },

    GetAccSetupBalanceSheetByCentreCodeAndTypeId: function () {
        var selectedCentreCode = $("#SelectedCentreCode").val();
        var accSetupBalanceSheetTypeId = $("#AccSetupBalanceSheetTypeId").val();
        $("#AccSetupGLTreeDivId").html("");
        if (selectedCentreCode != "" && accSetupBalanceSheetTypeId != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/AccSetupGL/GetAccSetupBalanceSheetByCentreCodeAndTypeId",
                data: {
                    "selectedCentreCode": selectedCentreCode,
                    "accSetupBalanceSheetTypeId": accSetupBalanceSheetTypeId
                },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#AccSetupBalanceSheetId").html("").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
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
    GetAccSetupGLTree: function () {
        var selectedCentreCode = $("#SelectedCentreCode").val();
        var accSetupBalanceSheetTypeId = $("#AccSetupBalanceSheetTypeId").val();
        var accSetupBalanceSheetId = $("#AccSetupBalanceSheetId").val();

        $("#AccSetupGLTreeDivId").html("");

        if (selectedCentreCode != "" && accSetupBalanceSheetTypeId != "" && accSetupBalanceSheetId != "") {
            CoditechCommon.ShowLodder();
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/AccSetupGL/GetAccSetupGLTree",
                data: {
                    "selectedCentreCode": selectedCentreCode,
                    "accSetupBalanceSheetTypeId": accSetupBalanceSheetTypeId,
                    "accSetupBalanceSheetId": accSetupBalanceSheetId
                },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#AccSetupGLTreeDivId").html("").html(data);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }

                    CoditechNotification.DisplayNotificationMessage("Failed to retrieve BalanceSheet.", "error");

                    // Automatically fade out the error message after 10 seconds
                    setTimeout(function () {
                        $("#notificationDivId").fadeOut(1000);
                    }, 1000);

                    CoditechCommon.HideLodder();
                }
            });
        } else {
            CoditechNotification.DisplayNotificationMessage("Please select Centre and Balance Sheet Type and BalanceSheet.", "error");

            // Automatically fade out the error message after 10 seconds
            setTimeout(function () {
                $("#notificationDivId").fadeOut(1000);
            }, 1000);
        }
    }
};
function handleGLTypeChange() {
    let glTypeId = parseInt($('#AccSetupGLTypeId').val());
    if (glTypeId === 4 || glTypeId === 5) {
        $('#UserTypeId').prop('disabled', true);
        $('#UserTypeId').val('').trigger('change');
    } else {
        $('#UserTypeId').prop('disabled', false);
    }
};
// Toggles sub-accounts visibility
window.toggleSubAccounts = function (type, id) {
    var sub = document.getElementById(type === "cat" ? "cat-gl-" + id : "gl-subaccounts-" + id);
    var chev = document.getElementById(type === "cat" ? "cat-chevron-" + id : "gl-chevron-" + id);
    var actions = document.getElementById(type === "cat" ? "cat-buttons-" + id : "gl-buttons-" + id);
    $('#AccSetupGLTypeId').on('change', handleGLTypeChange);
    if (sub) {
        var isExpanded = sub.style.display === "block";
        sub.style.display = isExpanded ? "none" : "block";
        if (actions) actions.style.display = isExpanded ? "none" : "inline";
    }

    if (chev) {
        var currentRotation = chev.style.transform === "rotate(90deg)" ? "rotate(0deg)" : "rotate(90deg)";
        chev.style.transform = currentRotation;
        chev.style.display = "inline-block";
        chev.style.transition = "transform 0.3s ease-in-out";
    }
};
// Loads the Bank Form if GL Type equals 5
function InitializeBankForm() {
    let value = $("#AccSetupGLTypeId").val();
    let accSetupBalanceSheetId = $("#AccSetupBalanceSheetId").val();
    if (value === "5") {
        $.get("/AccSetupGLBank/GetBankForm", { accSetupBalanceSheetId: accSetupBalanceSheetId })
            .done(function (data) {
                $("#bankContainer").html(data);
                if (typeof Initialize === 'function') {
                    Initialize();
                } else {
                    console.error("Initialize function not defined");
                }
            })
            .fail(function () {
                CoditechNotification.DisplayNotificationMessage("Failed to load Bank form.", "error");
            });
    } else {
        $("#bankContainer").html("");
        if (typeof Initialize === 'function') {
            Initialize();
        }
    }
};

$(document).ready(function () {
    $(document).on("change", "#AccSetupGLTypeId", function () {
        let value = $(this).val();
        let accSetupBalanceSheetId = $("#AccSetupBalanceSheetId").val();

        if (value === "5") { // Assuming "5" represents Bank
            $.get("/AccSetupGLBank/GetBankForm", { accSetupBalanceSheetId: accSetupBalanceSheetId })
                .done(function (data) {
                    $("#bankContainer").html(data);
                })
                .fail(function () {
                    CoditechNotification.DisplayNotificationMessage("Failed to load Bank form.", "error");
                });
        } else {
            $("#bankContainer").html("");
        }
    });

    // Initialize AccSetupGL when the document is ready
    if (typeof AccSetupGL !== "undefined" && typeof AccSetupGL.Initialize === "function") {
        AccSetupGL.Initialize();
    }

    // Use delegated event for dynamic IFSCCode input
    $(document).on('input', '#IFSCCode', function () {
        this.value = this.value.toUpperCase();
    });
});
