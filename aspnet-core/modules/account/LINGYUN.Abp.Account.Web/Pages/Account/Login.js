$(function () {
    let checkQrCodeTimer;
    let isQrCodeInitialized = false;
    var qrCodeService = labp.account.qrCodeLogin;

    let sendSmsCodeTimer;
    let sendSmsCodeCountDown = 0;
    var authService = labp.account.account;

    var l = abp.localization.getResource('AbpAccount');
    var il = abp.localization.getResource('AbpIdentity');

    $("#SendVerifyCodeButton").click(function (e) {
        const button = $(this);
        e.preventDefault();

        var isValid = $('#PhoneNumberForm').validate().element('#PhoneNumberInput');
        if (!isValid) {
            return false;
        }

        var input = $('#PhoneNumberForm').serializeFormToObject();

        abp.ui.setBusy({ busy: true });
        authService.sendPhoneSigninCode({
            phoneNumber: input.phoneLoginInput.phoneNumber,
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

    $('#LoginFormTabs').on('shown.bs.tab', function (e) {
        const tabName = e.target.name;
        if (tabName === 'QrCodeLogin') {
            initQrCode();
        } else {
            releaseQrCodeTimer();
        }
        // ÇÐ»»tabÒÆ³ý´íÎóÌáÊ¾
        $('#AbpPageAlerts').remove();
    });

    function initQrCode() {
        if (isQrCodeInitialized) {
            return;
        }
        if (checkQrCodeTimer) {
            clearInterval(checkQrCodeTimer);
            checkQrCodeTimer = undefined;
        }
        abp.ui.setBusy({ busy: true });
        qrCodeService.generate().then(function (result) {
            abp.ui.clearBusy();
            $('#QrCodeKey').val(result.key);
            const qrCodeUrl = 'QRCODE_LOGIN:' + result.key;
            $('#QrCode').empty();
            new QRCode(document.getElementById("QrCode"), {
                text: qrCodeUrl,
                width: 150,
                height: 150
            });
            $('#QrCodeStatus').text(il('QrCode:NotScaned'));
            checkQrCodeTimer = setInterval(function () {
                checkQrCode(result.key);
            }, 5000);
        }).catch(function () {
            abp.ui.clearBusy();
        });

        isQrCodeInitialized = true;
    }

    function checkQrCode(key) {
        qrCodeService.check(key, {
            abpHandleError: false
        }).then(function (result) {
            switch (result.status) {
                case 10:
                    releaseQrCodeTimer();
                    $('#QrCodeForm').submit();
                    break;
                case 5:
                    $('#QrCodeStatus').text(il('QrCode:Scaned'));
                    // TODO: Ìæ»»ÓÃ»§Í·Ïñ?
                    if (result.picture) {
                        $('#QrCode').html('<img src="' + result.picture + '" alt="User Avatar" style="width: 150px; height: 150px; border-radius: 50%;">');
                    }
                    break;
                case 0:
                    $('#QrCodeStatus').text(il('QrCode:NotScaned'));
                    break;
                case -1:
                    $('#QrCodeStatus').text(il('QrCode:Invalid'));
                    releaseQrCodeTimer();
                    initQrCode();
                    break;
            }
        }).catch(function () {
            console.warn('Check for QR code errors');
            releaseQrCodeTimer();
        });
    }

    function releaseQrCodeTimer() {
        if (checkQrCodeTimer) {
            clearInterval(checkQrCodeTimer);
            checkQrCodeTimer = undefined;
            isQrCodeInitialized = false;
        }
    }
});
