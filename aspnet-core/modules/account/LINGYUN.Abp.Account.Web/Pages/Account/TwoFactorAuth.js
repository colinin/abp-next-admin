$(function () {
    var isQrCodeInitialized = false;

    var l = abp.localization.getResource('AbpUi');

    $("#copySharedKey").click(function (e) {
        navigator.clipboard.writeText($('#sharedKey code').html());
        abp.notify.success(l("CopiedToTheClipboard"));
    });

    function initQrCode() {
        if (isQrCodeInitialized) {
            return;
        }

        var authenticatorUri = $('#AuthenticatorUri').val();
        if (!authenticatorUri || authenticatorUri.length <= 0) {
            return;
        }

        new QRCode(document.getElementById("QrCode"), {
            text: authenticatorUri,
            width: 150,
            height: 150
        });
        isQrCodeInitialized = true;
    }

    initQrCode();
});
