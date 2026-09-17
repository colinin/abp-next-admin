$(function () {
    let sendEmailVerifyCodeTimer;
    let sendEmailVerifyCodeCountDown = 0;
    var authService = labp.account.account;

    var l = abp.localization.getResource('AbpAccount');
    var il = abp.localization.getResource('AbpIdentity');

    $("#SendVerifyCodeButton")?.click(function (e) {
        const button = $(this);
        e.preventDefault();

        var isValid = $('#RegisterForm').validate().element('#EmailAddressInput');
        if (!isValid) {
            return false;
        }

        var formModel = $('#RegisterForm').serializeFormToObject();
        var sendEmailVerifyCodeInternal = (Number)($('#SendEmailVerifyCodeInternal').val());
        if (!sendEmailVerifyCodeInternal) {
            sendEmailVerifyCodeInternal = 1;
        }

        abp.ui.setBusy({ busy: true });
        authService.sendEmailRegisterCode({
            emailAddress: formModel.input.emailAddress,
        }).then(function () {
            abp.ui.clearBusy();
            sendEmailVerifyCodeCountDown = sendEmailVerifyCodeInternal * 60;
            sendEmailVerifyCodeTimer = setInterval(function () {
                button.prop('disabled', true);
                button.text(`${sendEmailVerifyCodeCountDown}`);
                if (sendEmailVerifyCodeCountDown === 0) {
                    clearInterval(sendEmailVerifyCodeTimer);
                    button.prop('disabled', false);
                    button.text(l('SendVerifyCode'));
                }
                sendEmailVerifyCodeCountDown--;
            }, 1000);
        }).catch(function () {
            abp.ui.clearBusy();
        });
    });
});
