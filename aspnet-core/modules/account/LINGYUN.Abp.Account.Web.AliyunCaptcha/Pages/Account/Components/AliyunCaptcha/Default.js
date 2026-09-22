(function ($) {
    let captcha = null;
    let captchaVerified = false;

    const captchaCodeField = 'Input.CaptchaCode';
    const loginForm = $('#InputForm');

    const l = abp.localization.getResource('AbpAccount');

    loginForm.on('submit', async function (e) {
        if (!loginForm.valid()) {
            return;
        }
        if (captchaVerified) {
            return;
        }
        e.preventDefault();
        captcha.show();
    });

    async function initCaptcha() {
        if (captcha == null) {
            const captchaConfig = await labp.account.captcha.aliyunConfig();
            window.AliyunCaptchaConfig = {
                region: captchaConfig.region,
                prefix: captchaConfig.prefix,
            };
            window.initAliyunCaptcha({
                SceneId: captchaConfig.sceneId,
                EncryptedSceneId: captchaConfig.encryptedSceneId,
                mode: "popup",
                element: "#captcha-element",
                button: '#PasswordLogin_Button',
                success: function (captchaVerifyParam) {
                    captchaVerified = true;
                    appendFormField(captchaCodeField, captchaVerifyParam);
                    loginForm.trigger('submit');
                },
                fail: function (result) {
                    if (!result.success) {
                        captchaVerified = false;
                        removeFormField(captchaCodeField);
                        abp.notify.warn(l("CaptchaInitFailed"));
                    }
                },
                onError: function (errorInfo) {
                    console.warn(`captcha init failed, errorCode: ${errorInfo.code}, errorMessage: ${errorInfo.msg}`);
                    abp.notify.warn(l("CaptchaInitFailed"));
                },
                onClose: function () {
                    captchaVerified = false;
                    removeFormField(captchaCodeField);
                },
                getInstance: function (instance) {
                    captcha = instance;
                },
                slideStyle: {
                    width: 360,
                    height: 40,
                },
            });
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

    initCaptcha();

})(jQuery);