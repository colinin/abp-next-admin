<template>
  <div class="file-list-wrap">
    <BasicTable @register="registerTable">
      <template #toolbar>
        <Button
          v-if="hasPermission('AbpOssManagement.OssObject.Create')"
          v-feature="'AbpOssManagement.OssObject.UploadFile'"
          type="primary"
          @click="handleUpload"
          >{{ L('Objects:UploadFile') }}</Button>
        <Button
          v-if="hasPermission('AbpOssManagement.OssObject.Delete')"
          :disabled="selectCount <= 0"
          type="primary"
          danger
          @click="handleBulkDelete"
          >{{ L('Objects:BulkDelete') }}</Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
          <TableAction
            :stop-button-propagation="true"
            :actions="[
              {
                color: 'success',
                label: L('Objects:Preview'),
                icon: 'ant-design:search-outlined',
                onClick: handlePreview.bind(null, record),
              },
              {
                auth: 'AbpOssManagement.OssObject.Delete',
                color: 'error',
                label: L('Delete'),
                icon: 'ant-design:delete-outlined',
                onClick: handleDelete.bind(null, record),
              },
              {
                auth: 'AbpOssManagement.OssObject.Download',
                ifShow: !record.isFolder,
                label: L('Objects:Download'),
                icon: 'ant-design:download-outlined',
                onClick: handleDownload.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <OssUploadModal @register="registerUploadModal" />
    <OssPreviewModal @register="registerPreviewModal" />
  </div>
</template>

<script lang="ts" setup>
  import { computed, watch, nextTick } from 'vue';
  import { Button } from 'ant-design-vue';
  import { useModal } from '/@/components/Modal';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { getObjects, generateOssUrl, bulkDeleteObject, deleteObject } from '/@/api/oss-management/oss';
  import { getDataColumns } from '../datas/TableData';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import OssUploadModal from './OssUploadModal.vue';
  import OssPreviewModal from './OssPreviewModal.vue';

  const props = defineProps({
    bucket: {
      type: String,
      default: '',
    },
    path: {
      type: String,
      default: '',
    },
  });
  const { hasPermission } = usePermission();
  const { createConfirm, createMessage } = useMessage();
  const { L } = useLocalization(['AbpOssManagement', 'AbpUi']);
  const [registerUploadModal, { openModal: openUploadModal }] = useModal();
  const [registerPreviewModal, { openModal: openPreviewModal }] = useModal();
  const [registerTable, { reload, getSelectRowKeys, setTableData, setPagination }] = useTable({
    rowKey: 'name',
    title: L('DisplayName:OssObject'),
    columns: getDataColumns(),
    api: getObjects,
    fetchSetting: {
      pageField: 'skipCount',
      sizeField: 'maxResultCount',
      listField: 'objects',
      totalField: 'maxKeys',
    },
    beforeFetch: beforeFetch,
    pagination: true,
    striped: false,
    useSearchForm: false,
    showTableSetting: true,
    // 启用批量删除功能, 防止误选
    clickToRowSelect: false,
    clearSelectOnPageChange: true,
    tableSetting: {
      redo: false,
    },
    bordered: true,
    showIndexColumn: false,
    canResize: false,
    immediate: false,
    rowSelection: { type: 'checkbox' },
    scroll: { x: 'max-content', y: '100%' },
    actionColumn: {
      width: 240,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });
  const selectCount = computed(() => {
    return getSelectRowKeys().length;
  });

  watch(
    () => props.path,
    (path) => {
      if (!path) {
        nextTick(() => {
          setTableData([]);
          setPagination({
            current: 1,
          });
        });
        return;
      }
      nextTick(() => {
        setPagination({
          current: 1,
        });
        reload();
      });
    },
    {
      immediate: true,
    },
  )

  function beforeFetch(request: any) {
    request.bucket = props.bucket;
    request.prefix = props.path;
    request.delimiter = '';
    formatPagedRequest(request);
  }

  function handleUpload() {
    let path = props.path;
    // 去掉头部的./标记
    if (path.startsWith('./')) {
      path = path.substring(2);
    }
    openUploadModal(true, {
      bucket: props.bucket,
      path: path,
    });
  }

  function handleBulkDelete() {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('Objects:WillBeBulkDeletedMessage'),
      okCancel: true,
      onOk: () => {
        bulkDeleteObject({
          bucket: props.bucket,
          path: props.path,
          objects: getSelectRowKeys(),
        }).then(() => {
          createMessage.success(L('Successful'));
          reload();
        });
      },
    });
  }

  function handlePreview(record) {
     openPreviewModal(true, {
      bucket: props.bucket,
      objects: [record],
    });
  }

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
      okCancel: true,
      onOk: () => {
        deleteObject({
          bucket: props.bucket,
          path: props.path,
          object: record.name,
        }).then(() => {
          createMessage.success(L('Successful'));
          reload();
        });
      },
    });
  }

  function handleDownload(record) {
    const link = document.createElement('a');
    link.style.display = 'none';
    link.href = generateOssUrl(props.bucket, record.path, record.name);
    link.setAttribute('download', record.name);
    document.body.appendChild(link);
    link.click();
  }

</script>
