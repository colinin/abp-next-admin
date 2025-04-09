$(function () {
    var ul = abp.localization.getResource('AbpUi');
    var il = abp.localization.getResource('AbpIdentity');
    var dataTable = $('#SessionsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(labp.account.myProfile.getSessions),
            columnDefs: [
                {
                    title: ul('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: il('RevokeSession'),
                                    confirmMessage: function () {
                                        return il('SessionWillBeRevokedMessage');
                                    },
                                    visible: function (data) {
                                        return data.sessionId !== abp.currentUser?.sessionId;
                                    },
                                    action: function (data) {
                                        labp.account.myProfile
                                            .revokeSession(data.record.sessionId)
                                            .then(function () {
                                                abp.notify.info(
                                                    il('SuccessfullyRevoked')
                                                );
                                                dataTable.ajax.reload();
                                            });
                                    }
                                }
                            ]
                    }
                },
                {
                    title: il('DisplayName:Device'),
                    data: "device"
                },
                {
                    title: il('DisplayName:IpAddresses'),
                    data: "ipAddresses"
                },
                {
                    title: il('DisplayName:SignedIn'),
                    data: "signedIn",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toLocaleString(luxon.DateTime.DATETIME_FULL_WITH_SECONDS);
                    }
                }]
        })
    );
});
