(function ($) {
    $('#CaptchaImg').click(function (e) {
        e.preventDefault();
        refreshCaptchaImage();
    });

    function refreshCaptchaImage() {
        labp.account.captcha.lazyRefresh().then(function (result) {
            if (!result.captchaImage) return;
            document.getElementById('CaptchaImg').src = result.captchaImage;
        });
    }

    refreshCaptchaImage();
})(jQuery);