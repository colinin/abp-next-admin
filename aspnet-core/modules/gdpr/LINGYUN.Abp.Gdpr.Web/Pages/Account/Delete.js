$(function () {
    var l = abp.localization.getResource('AbpGdpr');

    $('#DeletePersonalAccountButton').click(function (e) {
        e.preventDefault();
        abp.message.confirm(
            l('DeletePersonalAccountWarning'),
            function (confirm) {
                if (confirm) {
                    labp.gdpr.gdprRequest
                        .deletePersonalAccount()
                        .then(function () {
                            abp.notify.success(
                                l('PersonalAccountDeleteRequestReceived')
                            );
                            $(location).attr('href', '/Account/Login');
                        });
                }
            }
        );
    });
});
