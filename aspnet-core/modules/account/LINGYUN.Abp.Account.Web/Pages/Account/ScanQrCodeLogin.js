$(function () {
    let checkQrCodeTimer;
    let isQrCodeInitialized = false;
    var qrCodeService = labp.account.qrCodeLogin;

    var il = abp.localization.getResource('AbpIdentity');

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
                    // TODO: 替换用户头像?
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

    initQrCode();
});
