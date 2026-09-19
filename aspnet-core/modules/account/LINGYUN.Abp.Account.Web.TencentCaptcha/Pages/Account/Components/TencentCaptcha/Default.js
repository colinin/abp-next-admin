(function ($) {
    let captcha = null;
    let captchaVerified = false;

    const captchaCodeField = 'PasswordLoginInput.CaptchaCode';
    const loginForm = $('#PasswordLoginForm');

    const l = abp.localization.getResource('AbpAccount');

    loginForm.on('submit', async function (e) {
        if (!loginForm.valid()) {
            return;
        }
        if (captchaVerified) {
            return;
        }
        e.preventDefault();
        await initTencentCaptcha();
        if (!captcha) {
            abp.notify.warn(l("CaptchaInitFailed"));
            return;
        }
        captcha.show();
    });

    function captchaCallback(res) {
        if (res.ret === 0) {
            captchaVerified = true;
            appendFormField(captchaCodeField, `${res.randstr};${res.ticket}`);
            loginForm.trigger('submit');
        } else {
            captchaVerified = false;
            removeFormField(captchaCodeField);
            console.warn('captcha valid error, ret:', res.ret);
        }
    }

    async function initTencentCaptcha() {
        try {
            if (captcha == null) {
                const captchaConfig = await labp.account.captcha.config();
                if (captchaConfig.aidEncrypted) {
                    captcha = new TencentCaptcha(captchaConfig.captchaAppId, captchaCallback, {
                        aidEncrypted: captchaConfig.aidEncrypted,
                        aidEncryptedType: captchaConfig.aidEncryptedType,
                        aidEncryptedAad: captchaConfig.aidEncryptedAad
                    });
                } else {
                    captcha = new TencentCaptcha(captchaConfig.captchaAppId, captchaCallback);
                }
            }
        } catch (error) {
            console.warn('captcha init error:', error);
        }
    }

    function appendFormField(name, value) {
        let $input = loginForm.find(`input[name="${name}"]`);
        if ($input.length === 0) {
            $input = $('<input>', { type: 'hidden', name: name }).appendTo(loginForm);
        }
        $input.val(value);
    }

    function removeFormField(name) {
        loginForm.find(`input[name="${name}"]`).remove();
    }

})(jQuery);