<template>
  <div>
    <BasicTable @register="registerTable">
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
          <TableAction
            :stop-button-propagation="true"
            :actions="[
              {
                auth: 'AbpCachingManagement.Cache',
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
              {
                auth: 'AbpCachingManagement.Cache.Delete',
                label: L('Delete'),
                color: 'error',
                icon: 'ant-design:delete-outlined',
                onClick: handleDelete.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <CacheModal @register="registerModal" />
  </div>
</template>

<script lang="ts" setup>
  import { onMounted } from 'vue';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getDataColumns } from '../datas/TableData';
  import { getSearchFormSchemas } from '../datas/ModalData';
  import { useModal } from '/@/components/Modal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { getKeys, remove } from '/@/api/caching-management/cache';
  import CacheModal from './CacheModal.vue';

  const { L } = useLocalization('CachingManagement');
  const { createConfirm, createMessage } = useMessage();
  const [registerModal, { openModal }] = useModal();
  const [registerTable, { getForm, setTableData }] = useTable({
    rowKey: 'key',
    title: L('Caches'),
    columns: getDataColumns(),
    pagination: true,
    striped: false,
    useSearchForm: true,
    showIndexColumn: false,
    showTableSetting: true,
    formConfig: {
      labelWidth: 100,
      submitFunc: fetchCacheKeys,
      schemas: getSearchFormSchemas(),
    },
    bordered: true,
    canResize: true,
    immediate: false,
    actionColumn: {
      width: 150,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });

  onMounted(fetchCacheKeys);

  function fetchCacheKeys() {
    const form = getForm();
    return form.validate().then((input) => {
      onFetch(input);
    });
  }

  function onFetch(input) {
    getKeys(input).then((res) => {
      setTableData(res.keys.map((key) => {
        return {
          key: key,
        };
      }));
    });
  }

  function handleEdit(record) {
    openModal(true, record);
  }

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
      onOk: () => {
        return new Promise((resolve, reject) => {
          remove(record.key).then(() => {
            createMessage.success(L('Successful'));
            fetchCacheKeys();
            return resolve(record.key);
          }).catch((error) => {
            return reject(error);
          });
        });
      },
    });
  }
</script>
