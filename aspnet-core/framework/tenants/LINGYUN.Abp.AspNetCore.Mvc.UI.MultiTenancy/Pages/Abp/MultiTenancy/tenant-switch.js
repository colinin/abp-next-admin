(function ($) {
    var tenantSwitchModal = new abp.ModalManager(abp.appPath + 'Abp/MultiTenancy/TenantSwitchModal');

    $(function () {
        $('#AbpTenantSwitchLink').click(function (e) {
            e.preventDefault();
            tenantSwitchModal.open();
        });

        function initTenantInputToggle($form) {
            var $tenantSelect = $form.find('#TenantSelect_Name');
            var $tenantInput = $form.find('#TenantSelect_InputName');

            if ($tenantSelect.length === 0 || $tenantInput.length === 0) {
                return;
            }

            if ($tenantSelect.data('tenantInputBound')) {
                return;
            }
            $tenantSelect.data('tenantInputBound', true);

            var $tenantInputGroup = $tenantInput.closest('.mb-3');

            function toggleTenantInput() {
                var isInputTenant = $tenantSelect.val() === '';

                $tenantInputGroup.toggle(isInputTenant);
                $tenantInput.prop('disabled', !isInputTenant);

                if (!isInputTenant) {
                    $tenantInput.val('');
                }
            }

            toggleTenantInput();

            $tenantSelect.on('change', toggleTenantInput);
        }

        tenantSwitchModal.onOpen(function () {
            const form = tenantSwitchModal.getForm();
            initTenantInputToggle(form);
        });

        tenantSwitchModal.onResult(function () {
            location.assign(location.href);
        });
    });

})(jQuery);