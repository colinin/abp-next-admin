(function () {
    'use strict';

    $(function () {
        var l = abp.localization.getResource('AbpUi');
        var qrCodeInstance = null;
        var resizeTimer = null;
        var resizeObserver = null;
        var lastRenderedSize = 0;

        var QR_MIN = 140;
        var QR_MAX = 260;

        $(".password-visibility-button").click(function (e) {
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

        $("#copySharedKey").on("click", function () {
            var text = $("#sharedKey code").text().trim();
            if (!text) return;

            if (navigator.clipboard && window.isSecureContext) {
                navigator.clipboard.writeText(text).then(function () {
                    abp.notify.success(l("CopiedToTheClipboard"));
                });
            } else {
                var $temp = $("<textarea>").val(text).css({
                    position: "fixed", top: "-9999px"
                }).appendTo("body");
                $temp[0].select();
                try {
                    document.execCommand("copy");
                    abp.notify.success(l("CopiedToTheClipboard"));
                } finally {
                    $temp.remove();
                }
            }
        });

        function getContainer() {
            return document.getElementById("QrCode");
        }

        function getQrSize() {
            var container = getContainer();
            if (!container) return QR_MAX;

            var w = container.clientWidth || QR_MAX;
            return Math.min(Math.max(w, QR_MIN), QR_MAX);
        }

        function renderQrCode(force) {
            var container = getContainer();
            var uri = ($("#AuthenticatorUri").val() || "").trim();
            if (!container || !uri) return;

            var size = getQrSize();

            if (!force && lastRenderedSize === size && qrCodeInstance) {
                return;
            }

            container.innerHTML = "";
            qrCodeInstance = new QRCode(container, {
                text: uri,
                width: size,
                height: size,
                correctLevel: QRCode.CorrectLevel.M
            });
            lastRenderedSize = size;

            var el = container.querySelector("img, canvas");
            if (el) {
                el.style.width = "100%";
                el.style.height = "auto";
            }
        }

        function scheduleRender() {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(function () {
                renderQrCode(true);
            }, 120);
        }

        function observe() {
            var container = getContainer();
            if (!container) return;

            if (typeof ResizeObserver !== "undefined") {
                var target = container.closest(".authenticator-columns") || container;
                resizeObserver = new ResizeObserver(scheduleRender);
                resizeObserver.observe(target);
            } else {
                $(window).on("resize", scheduleRender);
            }
        }

        var initialUri = ($("#AuthenticatorUri").val() || "").trim();
        if (initialUri) {
            requestAnimationFrame(function () {
                renderQrCode(true);
                observe();
            });
        }
    });
})();