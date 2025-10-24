var PostalCodeHandler = {
    Initialize: function () {
        this.constructor();
    },

    constructor: function () {
        this.BindPostalCodeForms();
        this.BindModalEvents();
    },

    BindPostalCodeForms: function () {
        const self = this;
        $("form[id^='frmGeneralPersonalAddress_']").each(function () {
            const $form = $(this);
            const formId = $form.attr("id");
            const postalInput = $form.find('input[name="Postalcode"]');
            const saveButton = $form.find(".btn-success");
            const countrySelect = $form.find('select[name="GeneralCountryMasterId"]');
            let popupTimeout = null;

            postalInput.on('keypress', function (e) {
                if (e.key === 'Enter') {
                    e.preventDefault();
                    return false;
                }
            });

            postalInput.on('input', function () {
                let postalVal = $(this).val().trim();
                postalVal = postalVal.replace(/\D/g, '');
                $(this).val(postalVal);
                clearTimeout(popupTimeout);

                const addressTypeEnum = formId.replace("frmGeneralPersonalAddress_", "");

                self.CheckPostalCodeValidity(postalInput, saveButton);

                if (self.IsExplicitlyIndiaSelected(countrySelect) && postalVal.length === 6) {
                    popupTimeout = setTimeout(() => {
                        self.FetchPostalCode(postalVal, addressTypeEnum);
                    }, 300);
                }
            });

            countrySelect.on('change', function () {
                postalInput.trigger('input');
            });

            saveButton.on('click', function (e) {
                const addressTypeEnum = formId.replace("frmGeneralPersonalAddress_", "");
                const postalVal = postalInput.val().trim();
                if (!self.CheckPostalCodeValidity(postalInput, saveButton)) {
                    e.preventDefault();
                    self.ShowPostalCodeError(addressTypeEnum, postalInput);
                    return false;
                }

                postalInput.removeClass("is-invalid");
                if (!$form[0].checkValidity()) {
                    $form[0].reportValidity();
                    return false;
                }
                const selected = $form.find('input[name="SelectedAddress"]:checked').val();
                if (!selected) {
                    self.ShowAddressSelectionError(addressTypeEnum);
                    return;
                }
                const parts = selected.split('|');
                const userAddress = {
                    Name: parts[0],
                    District: parts[1],
                    Division: parts[2],
                    State: parts[3],
                    Country: parts[4],
                    Pincode: parts[5]
                };
                self.PrepareModalForm($form, selected, addressTypeEnum);
                $('#postalCodeModal').modal('show');
            });
            self.CheckPostalCodeValidity(postalInput, saveButton);
        });
    },

    BindModalEvents: function () {
        $('#postalCodeModal').on('hidden.bs.modal', function () {
            const addressTypeEnum = $(this).data('AddressTypeEnum');
            if (addressTypeEnum) {
                const targetForm = $("#frmGeneralPersonalAddress_" + addressTypeEnum);
                targetForm.find('input[name="Postalcode"]').val('');
            }
        });
    },

    IsExplicitlyIndiaSelected: function (countrySelect) {
        const selectedOption = countrySelect.find('option:selected');
        return selectedOption.text()?.toLowerCase().includes('india');
    },

    FetchPostalCode: function (postalCode, addressTypeEnum) {
        CoditechCommon.ShowLodder();

        const formSelector = "#frmGeneralPersonalAddress_" + addressTypeEnum;
        const $targetForm = $(formSelector);

        const personIdVal = $targetForm.find('input[name="PersonId"]').val();
        const entityIdVal = $targetForm.find('input[name="EntityId"]').val();

        $.ajax({
            url: `/GeneralCommon/FetchPostalCode`,
            type: 'GET',
            data: {
                postalCode,
                personId: personIdVal,
                entityId: entityIdVal,
                addressTypeEnum
            },
            success: function (response) {
                $('#postalCodeModalContent').html(response);
                $('#postalCodeModal').data('AddressTypeEnum', addressTypeEnum);
                $('#postalCodeModal').modal('show');
                CoditechCommon.HideLodder();
            },
            error: function (xhr) {
                if (xhr.status == "401" || xhr.status == "403") {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Failed to display record.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },

    CheckPostalCodeValidity: function (postalInput, saveButton) {
        const postalVal = postalInput.val().trim();
        if (postalVal.length !== 6 || !/^\d{6}$/.test(postalVal)) {
            saveButton.prop("disabled", true);
            return false;
        }
        saveButton.prop("disabled", false);
        return true;
    },

    ShowPostalCodeError: function (addressTypeEnum, postalInput) {
        const errorMsg = `Please enter a valid 6-digit postal code for ${this.FormatAddressType(addressTypeEnum)}.`;
        CoditechNotification.DisplayNotificationMessage(errorMsg, "error");
        postalInput.addClass("is-invalid");
    },

    ShowAddressSelectionError: function (addressTypeEnum) {
    },

    FormatAddressType: function (enumString) {
        return enumString.replace(/([A-Z])/g, ' $1').trim();
    },

    PrepareModalForm: function ($form, selected, addressTypeEnum) {
        const backendData = {
            Postalcode: $form.find('input[name="Postalcode"]').val() || '',
            PersonId: $form.find('input[name="PersonId"]').val() || '',
            EntityId: $form.find('input[name="EntityId"]').val() || '',
            EntityType: $form.find('input[name="EntityType"]').val() || '',
            ControllerName: $form.find('input[name="ControllerName"]').val() || '',
            GeneralCountryMasterId: $form.find('select[name="GeneralCountryMasterId"]').val() || '',
            AddressTypeEnum: addressTypeEnum || '',
            AddressLine1: $form.find('textarea[name="AddressLine1"]').val() || '',
            AddressLine2: $form.find('textarea[name="AddressLine2"]').val() || '',
            FirstName: $form.find('input[name="FirstName"]').val() || '',
            MiddleName: $form.find('input[name="MiddleName"]').val() || '',
            LastName: $form.find('input[name="LastName"]').val() || '',
            EmailAddress: $form.find('input[name="EmailAddress"]').val() || '',
            PhoneNumber: $form.find('input[name="PhoneNumber"]').val() || '',
            MobileNumber: $form.find('input[name="MobileNumber"]').val() || ''
        };

        const $modalForm = $('#frmSendDetails');
        for (const key in backendData) {
            if ($modalForm.find(`input[name="${key}"]`).length === 0) {
                $modalForm.append(`<input type="hidden" name="${key}" value="${backendData[key]}" />`);
            } else {
                $modalForm.find(`input[name="${key}"]`).val(backendData[key]);
            }
        }
        $modalForm.find('input[name="SelectedAddress"]').remove();
        $modalForm.append(`<input type="hidden" name="SelectedAddress" value="${selected}" />`);
    }
};
$(function () {
    PostalCodeHandler.Initialize();
});
