$(function () {
    var ul = abp.localization.getResource("AbpUi");
    $("#TwoFactorEnabled").change(function () {
        var isChecked = $(this).is(':checked');
        abp.ui.setBusy({ busy: true });
        labp.account.myProfile
            .changeTwoFactorEnabled({
                enabled: isChecked
            })
            .then(function () {
                abp.notify.success(ul("SavedSuccessfully"));
            })
            .done(function () {
                abp.ui.clearBusy();
            });
    });
});
