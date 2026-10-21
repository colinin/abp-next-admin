$(function () {
    var l = abp.localization.getResource('AbpAccount');
    var authenticatorUri = $('#hdnAuthenticatorUri').val();
    var isAuthenticated = $('#hdnIsAuthenticated').val() === 'true';

    if (isAuthenticated) {
        $('#btnResetAuthenticator').click(function () {
            abp.message.confirm(
                l('ResetAuthenticatorWarning'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        abp.ui.setBusy({ busy: true });
                        labp.account.myProfile.resetAuthenticator().then(function () {
                            abp.notify.success(l('YourAuthenticatorIsSuccessfullyReset'));
                            abp.ui.clearBusy();
                            window.location.reload();
                        }).catch(function () {
                            abp.ui.clearBusy();
                        });
                    }
                }
            );
        });
        return;
    }

    var $step1Content = $('#step1-content');
    var $step2Content = $('#step2-content');
    var $step3Content = $('#step3-content');

    var $step1Indicator = $('#step1-indicator');
    var $step2Indicator = $('#step2-indicator');
    var $step3Indicator = $('#step3-indicator');

    if (authenticatorUri) {
        var container = document.getElementById('qrCodeContainer');
        if (container) {
            container.innerHTML = '';
            new QRCode(container, {
                text: authenticatorUri,
                width: 200,
                height: 200,
                correctLevel: QRCode.CorrectLevel.M
            });
        }
    }

    function copyToClipboard(text, $btnElement) {
        if (!text) return;
        navigator.clipboard.writeText(text).then(function () {
            var originalText = $btnElement.text();
            $btnElement.text(l('CopiedToTheClipboard'));
            setTimeout(function () { $btnElement.text(originalText); }, 2000);
        });
    }

    function renderRecoveryCodes(recoveryCodes) {
        var $codeContainer = $('#recoveryCode code');
        if (recoveryCodes && recoveryCodes.length > 0) {
            var codesText = Array.isArray(recoveryCodes)
                ? recoveryCodes.join('\r')
                : recoveryCodes;
            $codeContainer.text(codesText);
        }
    }

    function goToStep(step) {
        $step1Content.hide();
        $step2Content.hide();
        $step3Content.hide();

        $step1Indicator.removeClass('active');
        $step2Indicator.removeClass('active');
        $step3Indicator.removeClass('active');

        if (step === 1) {
            $step1Content.show();
            $step1Indicator.addClass('active');
        } else if (step === 2) {
            $step2Content.show();
            $step1Indicator.addClass('active');
            $step2Indicator.addClass('active');
        } else if (step === 3) {
            $step3Content.show();
            $step1Indicator.addClass('active');
            $step2Indicator.addClass('active');
            $step3Indicator.addClass('active');
        }
    }

    $('#btnCopyKey').click(function () {
        var sharedKey = $('#sharedKeyText').text().trim();
        copyToClipboard(sharedKey, $(this));
    });

    $('#btnCopyRecovery').click(function () {
        var recoveryText = $('#recoveryCode code').text().trim();
        copyToClipboard(recoveryText, $(this));
    });

    $('#btnGoToStep2').click(function () { goToStep(2); });
    $('#btnBackToStep1').click(function () { goToStep(1); });
    $('#btnGoToStep3').click(function () { goToStep(3); });

    $('#verifyForm').submit(function (e) {
        e.preventDefault();
        var authenticatorCode = $('#AuthenticatorCode').val();
        abp.ui.setBusy({ busy: true });
        labp.account.myProfile.verifyAuthenticatorCode({ authenticatorCode })
            .then(function (result) {
                abp.ui.clearBusy();
                abp.notify.success(l('AuthenticatorCodeValidSuccessfully'));
                renderRecoveryCodes(result.recoveryCodes);
                goToStep(3);
            }).catch(function () {
                abp.ui.clearBusy();
            });
    });

    $('#btnFinish').click(function () {
        window.location.reload();
    });

    goToStep(1);
});