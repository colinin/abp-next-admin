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
                                                abp.notify.info(
                                                    ul('DeletedSuccessfully')
                                                );
                                                dataTable.ajax.reload();
                                            });
                                    }
                                }
                            ]
                    }
                },
                {
                    title: l('DisplayName:CreationTime'),
                    data: "creationTime",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toLocaleString(luxon.DateTime.DATETIME_FULL_WITH_SECONDS);
                    }
                },
                {
                    title: l('DisplayName:Identity'),
                    data: "identity"
                },
                {
                    title: l('DisplayName:ClientId'),
                    data: "clientId"
                },
                {
                    title: l('DisplayName:ClientIpAddress'),
                    data: "clientIpAddress"
                },
                {
                    title: l('DisplayName:ApplicationName'),
                    data: "applicationName"
                },
                {
                    title: l('DisplayName:Actions'),
                    data: "action"
                },
                {
                    title: l('DisplayName:BrowserInfo'),
                    data: "browserInfo"
                }]
        })
    );
});
