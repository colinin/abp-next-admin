$(function () {
    let timer;
    let countDown = 0;
    var l = abp.localization.getResource('AbpAccount');
    var authService = labp.account.account;

    $("#SendVerifyCodeButton").click(function (e) {
        const button = $(this);
        e.preventDefault();

        var isValid = $('#PhoneNumberForm').validate().element('#PhoneNumberInput');
        if (!isValid) {
            return false;
        }

        var input = $('#PhoneNumberForm').serializeFormToObject();

        authService.sendPhoneSigninCode({
            phoneNumber: input.phoneLoginInput.phoneNumber,
        }).then(function () {
            countDown = 60;
            timer = setInterval(function () {
                button.prop('disabled', true);
                button.text(`${countDown}`);
                if (countDown === 0) {
                    clearInterval(timer);
                    button.prop('disabled', false);
                    button.text(l('SendVerifyCode'));
                }
                countDown--;
            }, 1000);
        });
    });

    $("#PasswordVisibilityButton").click(function (e) {
        let button = $(this);
        let passwordInput = button.parent().find("input");
        if (!passwordInput) {
            return;
        }

        if (passwordInput.attr("type") === "password") {
            passwordInput.attr("type", "text");
        }
        else {
            passwordInput.attr("type", "password");
        }

        let icon = button.find("i");
        if (icon) {
            icon.toggleClass("fa-eye-slash").toggleClass("fa-eye");
        }
    });
});
