var AddressSubmitHandler = {
    Initialize: function () {
        this.BindSubmitEvent();
    },

    BindSubmitEvent: function () {
        const self = this;
        const $form = $('#frmSendDetails');

        $form.on('submit', function (e) {
            e.preventDefault();
            self.HandleSubmit($form);
        });
    },

    HandleSubmit: function ($form) {
        const selected = $form.find('input[name="SelectedAddress"]:checked').val();
        if (!selected) {
            alert("Please select an address before saving.");
            return;
        }

        CoditechCommon.ShowLodder();
        $('#postalCodeModal').modal('hide');
        const parts = selected.split('|');
        const userAddress = {
            Name: parts[0] || '',
            District: parts[1] || '',
            Division: parts[2] || '',
            State: parts[3] || '',
            Country: parts[4] || '',
            Pincode: parts[5] || ''
        };
        const addressTypeEnum = $form.find('input[name="AddressTypeEnum"]').val();
        const $originForm = $(`#frmGeneralPersonalAddress_${addressTypeEnum}`);

        const backendData = {
            Postalcode: $originForm.find('input[name="Postalcode"]').val() || '',
            PersonId: $originForm.find('input[name="PersonId"]').val() || '',
            EntityId: $originForm.find('input[name="EntityId"]').val() || '',
            EntityType: $originForm.find('input[name="EntityType"]').val() || '',
            ControllerName: $originForm.find('input[name="ControllerName"]').val() || '',
            GeneralCountryMasterId: $originForm.find('select[name="GeneralCountryMasterId"]').val() || '',
            AddressTypeEnum: addressTypeEnum || '',
            AddressLine1: $originForm.find('textarea[name="AddressLine1"]').val() || '',
            AddressLine2: $originForm.find('textarea[name="AddressLine2"]').val() || '',
            FirstName: $originForm.find('input[name="FirstName"]').val() || '',
            MiddleName: $originForm.find('input[name="MiddleName"]').val() || '',
            LastName: $originForm.find('input[name="LastName"]').val() || '',
            EmailAddress: $originForm.find('input[name="EmailAddress"]').val() || '',
            PhoneNumber: $originForm.find('input[name="PhoneNumber"]').val() || '',
            MobileNumber: $originForm.find('input[name="MobileNumber"]').val() || ''
        };

        const payload = Object.assign({}, backendData, userAddress);

        this.SubmitAjax(payload);
    },

    SubmitAjax: function (payload) {
        $.ajax({
            url: '/GeneralCommon/ValidateAddress',
            type: 'POST',
            data: { addressData: JSON.stringify(payload) },
            success: function (response) {
                CoditechCommon.HideLodder();
                $('#addressContainer').html(response);
                location.reload();
            },
            error: function (xhr) {
                if (xhr.status == "401" || xhr.status == "403") {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Failed to display record.", "error");
                CoditechCommon.HideLodder();
            }
        });
    }
};
$(function () {
    AddressSubmitHandler.Initialize();
});
