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
                console.error("Initialize function is not defined on page load");
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

            // Bind click event for submitting a new child account
            $(document).off("click", "#submitAddChild").on("click", "#submitAddChild", function () {
                AccSetupGL.AddChild();
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

    // Opens the modal to add a child GL account
    OpenAddChildModal: function (parentId, categoryId) {
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
    },

    // Adds a child GL account via AJAX
    AddChild: function () {
        $(".text-danger").text(""); // Clear previous validation messages

        let isValid = true;
        let accSetupGLTypeId = $("#AccSetupGLTypeId").val();
        if (!accSetupGLTypeId || accSetupGLTypeId === "0" || accSetupGLTypeId === "-------AccSetup GL Type-------") {
            $("[data-valmsg-for='AccSetupGLTypeId']").text("Please select a GL Type.");
            isValid = false;
        }

        let glName = $("#GLName").val().trim();
        if (!glName) {
            $("[data-valmsg-for='GLName']").text("Ledger name is required.");
            isValid = false;
        }

        let glCode = $("#GLCode").val().trim();
        if (!glCode) {
            $("[data-valmsg-for='GLCode']").text("Code is required.");
            isValid = false;
        }

        // If AccSetupGLTypeId is 5, apply extra validation
        if (accSetupGLTypeId === "5") {
            let bankAccountName = $('#BankAccountName').val().trim();
            if (!bankAccountName) {
                $("[data-valmsg-for='BankAccountName']").text("Bank account name is required.");
                isValid = false;
            }

            let bankBranchName = $('#BankBranchName').val().trim();
            if (!bankBranchName) {
                $("[data-valmsg-for='BankBranchName']").text("Bank branch name is required.");
                isValid = false;
            }

            let ifscCode = $('#IFSCCode').val().trim();
            let regex = /^[A-Z]{4}0[A-Z0-9]{6}$/i; // Case-insensitive matching

            if (!ifscCode) {
                $("[data-valmsg-for='IFSCCode']").text("IFSC code is required.");
                isValid = false;
            } else if (!regex.test(ifscCode)) {
                $("[data-valmsg-for='IFSCCode']").text("Invalid IFSC code format.");
                isValid = false;
            } else {
                $("[data-valmsg-for='IFSCCode']").text(""); // Clear error if valid
            }

            let bankAccountNumber = $('#BankAccountNumber').val().trim();
            if (!bankAccountNumber) {
                $("[data-valmsg-for='BankAccountNumber']").text("Bank account number is required.");
                isValid = false;
            }
        }

        if (!isValid) return;

        let bankModel = {
            AccSetupBalanceSheetId: $('#AccSetupBalanceSheetId').val(),
            AccSetupGLId: $('#AccSetupGLId').val(),
            BankAccountName: $('#BankAccountName').val(),
            BankBranchName: $('#BankBranchName').val(),
            BankLimitAmount: $('#BankLimitAmount').val(),
            RateOfInterest: $('#RateOfInterest').val(),
            InterestMode: $('#InterestMode').val(),
            IFSCCode: $('#IFSCCode').val(),
            BankAccountNumber: $('#BankAccountNumber').val()
        };

        let jsonData = JSON.stringify(bankModel);
        $("#BankModelData").val(jsonData);

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
                    if (response.errors && response.errors.length > 0) {
                        response.errors.forEach(function (error) {
                            $("[data-valmsg-for='" + error.Field + "']").text(error.Message);
                        });
                    } else {
                        CoditechNotification.DisplayNotificationMessage(response.message, "error");
                    }
                }
            },
            error: function () {
                CoditechNotification.DisplayNotificationMessage("An error occurred while adding the record. Please try again.", "error");
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
                    CoditechCommon.HideLodder();
                }
            });
        } else {
            CoditechNotification.DisplayNotificationMessage("Please select Centre and Balance Sheet Type and BalanceSheet.", "error");
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
}

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
});
