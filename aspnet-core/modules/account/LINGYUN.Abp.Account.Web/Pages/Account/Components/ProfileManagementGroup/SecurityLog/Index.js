$(function () {
    var ul = abp.localization.getResource('AbpUi');
    var l = abp.localization.getResource('AbpAuditLogging');
    var dataTable = $('#SecurityLogTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(labp.account.mySecurityLog.getList),
            columnDefs: [
                {
                    title: ul('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: ul('Delete'),
                                    confirmMessage: function () {
                                        return ul('ItemWillBeDeletedMessage');
                                    },
                                    action: function (data) {
                                        labp.account.mySecurityLog
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(
                                                    ul('DeletedSuccessfully')
                                                );
                                                dataTable.ajax.reload();
                                            });
                                    }
                                }
                            ]
                    },
                    width: '100px'
                },
                {
                    title: l('CreationTime'),
                    data: "creationTime",
                    dataFormat: "datetime",
                    width: '180px'
                },
                {
                    title: l('Identity'),
                    data: "identity",
                    width: '150px'
                },
                {
                    title: l('ClientId'),
                    data: "clientId",
                    width: '150px'
                },
                {
                    title: l('ClientIpAddress'),
                    data: "clientIpAddress",
                    width: '150px'
                },
                {
                    title: l('ApplicationName'),
                    data: "applicationName",
                    width: '150px'
                },
                {
                    title: l('ActionName'),
                    data: "action",
                    width: '150px'
                },
                {
                    title: l('BrowserInfo'),
                    data: "browserInfo",
                    width: '300px'
                }]
        })
    );
});
