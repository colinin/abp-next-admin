<template>
  <div class="content">
    <Card :title="L('Objects:FileSystem')">
      <CardGrid style="width: 25%">
        <CardMeta>
          <template #title>
            <Select
              style="width: 100%"
              :placeholder="L('Containers:Select')"
              @change="handleContainerChange"
            >
              <Option
                v-for="container in containers"
                :key="container.name"
                :value="container.name"
                >{{ container.name }}</Option
              >
            </Select>
          </template>
          <template #description>
            <a-button
              v-if="hasPermission('AbpOssManagement.OssObject.Create')"
              :disabled="lockTree"
              style="width: 100%; margin-bottom: 20px"
              ghost
              type="primary"
              @click="handleNewFolder"
              >{{ L('Objects:CreateFolder') }}</a-button
            >
            <DirectoryTree
              v-if="!lockTree"
              :tree-data="folders"
              :expandedKeys="expandedKeys"
              @expand="fetchFolders"
              @select="handleSelectFolder"
            />
          </template>
        </CardMeta>
      </CardGrid>
      <CardGrid style="width: 75%">
        <BasicTable @register="registerTable">
          <template #toolbar>
            <a-button
              v-if="hasPermission('AbpOssManagement.OssObject.Create')"
              v-feature="'AbpOssManagement.OssObject.UploadFile'"
              :disabled="lockTree"
              type="primary"
              @click="handleUpload"
              >{{ L('Objects:UploadFile') }}</a-button
            >
            <a-button
              v-if="hasPermission('AbpOssManagement.OssObject.Delete')"
              :disabled="selectCount <= 0"
              type="primary"
              danger
              @click="handleBulkDelete"
              >{{ L('Objects:BulkDelete') }}</a-button
            >
          </template>
          <template #action="{ record }">
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
        </BasicTable>
      </CardGrid>
    </Card>
    <OssUploadModal @register="registerUploadModal" />
    <OssFolderModal @register="registerFolderModal" />
    <OssPreviewModal @register="registerPreviewModal" />
  </div>
</template>

<script lang="ts">
  import { computed, defineComponent, unref } from 'vue';
  import { Card, Modal, Tree, Select, message } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { useModal } from '/@/components/Modal';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getObjects, deleteObject, bulkDeleteObject, generateOssUrl } from '/@/api/oss-management/oss';
  import { getDataColumns } from '../datas/TableData';
  import { useObjects } from '../hooks/useObjects';
  import OssUploadModal from './OssUploadModal.vue';
  import OssFolderModal from './OssFolderModal.vue';
  import OssPreviewModal from './OssPreviewModal.vue';

  export default defineComponent({
    name: 'OssTable',
    components: {
      Card,
      CardGrid: Card.Grid,
      CardMeta: Card.Meta,
      BasicTable,
      TableAction,
      Select,
      Option: Select.Option,
      DirectoryTree: Tree.DirectoryTree,
      OssUploadModal,
      OssFolderModal,
      OssPreviewModal,
    },
    setup() {
      // 暂时不将逻辑代码移动到hooks，也不算太复杂 Orz...

      const { L } = useLocalization('AbpOssManagement', 'AbpUi');
      const { hasPermission } = usePermission();
      const [registerFolderModal, { openModal: openFolderModal }] = useModal();
      const [registerUploadModal, { openModal: openUploadModal }] = useModal();
      const [registerPreviewModal, { openModal: openPreviewModal }] = useModal();
      const {
        path,
        bucket,
        folders,
        containers,
        expandedKeys,
        fetchFolders,
        beforeFetch,
        handleContainerChange,
      } = useObjects();
      const [registerTable, { reload, setPagination, getSelectRowKeys }] = useTable({
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
        actionColumn: {
          width: 240,
          title: L('Actions'),
          dataIndex: 'action',
          slots: { customRender: 'action' },
        },
      });
      const lockTree = computed(() => {
        return unref(bucket) ? false : true;
      });
      const selectCount = computed(() => {
        return getSelectRowKeys().length;
      })

      function handleSelectFolder(folders, e) {
        path.value = e.node.dataRef.path + folders[0];
        setPagination({
          current: 1,
        });
        reload();
      }

      function handleDelete(record) {
        Modal.warning({
          title: L('AreYouSure'),
          content: L('ItemWillBeDeletedMessage'),
          okCancel: true,
          onOk: () => {
            deleteObject({
              bucket: unref(bucket),
              path: unref(path),
              object: record.name,
            }).then(() => {
              reload();
              message.success(L('Successful'));
            });
          },
        });
      }

      function handleBulkDelete() {
        Modal.warning({
          title: L('AreYouSure'),
          content: L('Objects:WillBeBulkDeletedMessage'),
          okCancel: true,
          onOk: () => {
            bulkDeleteObject({
              bucket: unref(bucket),
              path: unref(path),
              objects: getSelectRowKeys(),
            }).then(() => {
              reload();
              message.success(L('Successful'));
            });
          },
        });
      }

      function handleDownload(record) {
        const link = document.createElement('a');
        link.style.display = 'none';
        link.href = generateOssUrl(unref(bucket), record.path, record.name);
        link.setAttribute('download', record.name);
        document.body.appendChild(link);
        link.click();
      }

      function handleUpload() {
        openUploadModal(true, {
          bucket: unref(bucket),
          // 去掉头部的./标记
          path: unref(path).substring(2),
        });
      }

      function handleNewFolder() {
        openFolderModal(true, {
          bucket: unref(bucket),
          path: unref(path),
        });
      }

      function handlePreview(record) {
        openPreviewModal(true, {
          bucket: unref(bucket),
          objects: [record],
        });
      }

      return {
        L,
        folders,
        lockTree,
        selectCount,
        expandedKeys,
        fetchFolders,
        handleSelectFolder,
        containers,
        handleContainerChange,
        registerUploadModal,
        handleUpload,
        handleNewFolder,
        hasPermission,
        registerTable,
        handleDelete,
        handleBulkDelete,
        handleDownload,
        registerFolderModal,
        registerPreviewModal,
        handlePreview,
      };
    },
  });
</script>
