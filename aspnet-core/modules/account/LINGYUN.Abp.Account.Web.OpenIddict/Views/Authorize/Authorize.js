$(function () {
    // 初始化选择的 Scope
    updateSelectedScopes();

    // 监听 Scope 选择变化
    $('.scope-checkbox').change(function () {
        updateSelectedScopes();
    });

    function updateSelectedScopes() {
        var selected = [];

        // 获取必需的 Scope
        $('input[name="selectedScopeValues"]').each(function () {
            selected.push($(this).val());
        });

        // 获取用户选择的 Scope
        $('.scope-checkbox:checked').each(function () {
            var value = $(this).data('scope-value');
            if (!selected.includes(value)) {
                selected.push(value);
            }
        });

        // 更新隐藏字段
        $('#selected_scopes').val(selected.join(' '));
    }
});
