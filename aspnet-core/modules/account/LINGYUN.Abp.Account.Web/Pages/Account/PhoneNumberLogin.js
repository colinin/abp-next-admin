$(function () {
    let sendSmsCodeTimer;
    let sendSmsCodeCountDown = 0;
    var authService = labp.account.account;

    var l = abp.localization.getResource('AbpAccount');

    $("#SendVerifyCodeButton").click(function (e) {
        const button = $(this);
        e.preventDefault();

        var isValid = $('#InputForm').validate().element('#PhoneNumberInput');
        if (!isValid) {
            return false;
        }

        var input = $('#InputForm').serializeFormToObject();

        abp.ui.setBusy({ busy: true });
        authService.sendPhoneSigninCode({
            phoneNumber: input.input.phoneNumber,
        }).then(function () {
            abp.ui.clearBusy();
            sendSmsCodeCountDown = 60;
            sendSmsCodeTimer = setInterval(function () {
                button.prop('disabled', true);
                button.text(`${sendSmsCodeCountDown}`);
                if (sendSmsCodeCountDown === 0) {
                    clearInterval(sendSmsCodeTimer);
                    button.prop('disabled', false);
                    button.text(l('SendVerifyCode'));
                }
                sendSmsCodeCountDown--;
            }, 1000);
        }).catch(function () {
            abp.ui.clearBusy();
        });
    });
});
