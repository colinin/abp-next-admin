$(function () {
    var l = abp.localization.getResource('AbpOpenIddict');

    // 账户选择交互 - 使用 Bootstrap 的 list-group-item-action
    $('.account-item').on('click', function (e) {
        // 如果点击的是链接或按钮，不处理
        if ($(e.target).is('a, button, .btn') ||
            $(e.target).closest('a, button, .btn').length) {
            return;
        }

        // 移除所有 active 状态
        $('.account-item').removeClass('active');

        // 添加当前选中状态
        $(this).addClass('active');

        // 选中对应的 radio button
        var radio = $(this).find('input[type="radio"]');
        radio.prop('checked', true);
    });

    // 表单提交验证
    $('#selectAccountForm').on('submit', function (e) {
        var selectedAccount = $('input[name="Input.SelectedAccountId"]:checked').val();

        if (!selectedAccount) {
            e.preventDefault();
            abp.message.warn(l('SelectedAccountId'));
            return false;
        }

        return true;
    });

    // 初始选中第一个账户（如果不是当前账户）
    if (!$('.account-item.active').length) {
        $('.account-item:first').trigger('click');
    }

    // 自动聚焦到选中的账户
    $('.account-item.active').find('input[type="radio"]').focus();
});
