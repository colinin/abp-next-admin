$(function () {
    var ul = abp.localization.getResource('AbpUi');
    var gl = abp.localization.getResource('AbpGdpr');

    var dataTable = $('#PersonalDataTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(labp.gdpr.gdprRequest.getList),
            columnDefs: [
                {
                    title: ul('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: gl('Download'),
                                    visible: function (data) {
                                        const now = luxon.DateTime.now();
                                        const readyTime = luxon.DateTime.fromISO(data.readyTime, {
                                            locale: abp.localization.currentCulture.name
                                        });
                                        return now >= readyTime;
                                    },
                                    action: function (data) {
                                        var downloadWindow = window.open(
                                            abp.appPath + 'api/gdpr/requests/personal-data/download/' + data.record.id + '',
                                            "_blank"
                                        );
                                        downloadWindow.focus();
                                    }
                                },
                                {
                                    text: ul('Delete'),
                                    confirmMessage: function () {
                                        return ul('ItemWillBeDeletedMessage');
                                    },
                                    action: function (data) {
                                        labp.gdpr.gdprRequest
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
                    }
                },
                {
                    title: gl('DisplayName:ReadyTime'),
                    data: "readyTime",
                    dataFormat: "datetime",
                },
                {
                    title: gl('DisplayName:CreationTime'),
                    data: "creationTime",
                    dataFormat: "datetime",
                }]
        })
    );

    $('#RequestPersonalDataButton').click(function (e) {
        e.preventDefault();
        labp.gdpr.gdprRequest
            .preparePersonalData()
            .then(function () {
                abp.notify.success(
                    gl('PersonalDataPrepareRequestReceived')
                );
                dataTable.ajax.reload();
            });
    });
    $('#DeletePersonalDataButton').click(function (e) {
        e.preventDefault();
        abp.message.confirm(
            gl('DeletePersonalDataWarning'),
            function (confirm) {
                if (confirm) {
                    labp.gdpr.gdprRequest
                        .deletePersonalData()
                        .then(function () {
                            abp.notify.success(
                                gl('PersonalDataDeleteRequestReceived')
                            );
                            dataTable.ajax.reload();
                        });
                }
            }
        );
    });
});
