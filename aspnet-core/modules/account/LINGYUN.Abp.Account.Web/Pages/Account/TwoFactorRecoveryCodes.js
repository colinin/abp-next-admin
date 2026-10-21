$(function () {
    var isQrCodeInitialized = false;

    var l = abp.localization.getResource('AbpUi');

    $("#copyRecoveryCode").click(function (e) {
        navigator.clipboard.writeText($('#recoveryCode code').html());
        abp.notify.success(l("CopiedToTheClipboard"));
    });

    $("#printRecoverCodes").click(function (e) {
        var printWindow = window.open();
        printWindow.document.write('<html><body>');
        printWindow.document.write($('#recoveryCode code').html());
        printWindow.document.write('</body></html>');
        printWindow.document.close();
        printWindow.print();
    });
});
