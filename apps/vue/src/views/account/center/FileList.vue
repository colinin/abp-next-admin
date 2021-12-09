<template>
  <div>
    <BasicTable @register="registerTable">
      <template #action="{ record }">
        <TableAction
          :stop-button-propagation="true"
          :actions="[
            {
              color: 'error',
              label: L('Delete'),
              icon: 'ant-design:delete-outlined',
              ifShow: deleteEnabled,
              onClick: handleDelete.bind(null, record),
            },
            {
              label: L('Share'),
              icon: 'ant-design:share-alt-outlined',
              ifShow: shareEnabled,
              onClick: handleShare.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
    <BasicModal @register="registerShareModal" :title="L('Share')" @ok="handleShareFile">
      <BasicForm @register="registershareForm" />
    </BasicModal>
  </div>
</template>

<script lang="ts" setup>
  import { computed, defineProps, defineEmits, watchEffect, nextTick } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModal } from '/@/components/Modal';

  import { getDataColumns, getShareModalSchemas } from './data';
  import { ListResultDto } from '/@/api/model/baseModel';
  import { OssObject } from '/@/api/oss-management/model/ossModel';

  import { getList as getPrivates } from '/@/api/oss-management/private';
  import { getList as getPublices } from '/@/api/oss-management/public';
  import { share } from '/@/api/oss-management/private';

  const props = defineProps({
    selectGroup: {
      type: String,
      required: true,
      default: 'private',
    },
    selectPath: {
      type: String,
      required: true,
      default: '/',
    },
    deleteEnabled: {
      type: Boolean,
      required: true,
      default: false,
    },
  });
  const emit = defineEmits(['delete:file:private', 'delete:file:public', 'append:folder']);

  const { L } = useLocalization('AbpOssManagement', 'AbpUi');
  const { createConfirm, createMessage } = useMessage();
  const [registerTable, { setTableData }] = useTable({
    rowKey: 'name',
    columns: getDataColumns(),
    title: L('FileList'),
    pagination: false,
    striped: false,
    useSearchForm: false,
    showTableSetting: true,
    tableSetting: {
      redo: false,
    },
    bordered: true,
    showIndexColumn: false,
    canResize: false,
    immediate: false,
    rowSelection: { type: 'checkbox' },
    actionColumn: {
      width: 180,
      title: L('Actions'),
      dataIndex: 'action',
      slots: { customRender: 'action' },
    },
  });

  const shareEnabled = computed(() => {
    return props.selectGroup === 'private';
  });

  const [registershareForm, { validate, setFieldsValue, resetFields }] = useForm({
    labelAlign: 'left',
    labelWidth: 120,
    showActionButtonGroup: false,
    schemas: getShareModalSchemas(),
  });
  const [registerShareModal, { openModal, closeModal }] = useModal();

  watchEffect(() => {
    props.selectGroup === 'private' && _fetchFileList(getPrivates);
    props.selectGroup === 'public' && _fetchFileList(getPublices);
  });

  function _fetchFileList(api: (...args: any) => Promise<ListResultDto<OssObject>>) {
    api({
      path: props.selectPath,
      maxResultCount: 100,
    }).then((res) => {
      const folders = res.items.filter((item) => item.isFolder);
      emit('append:folder', folders);
      setTableData(res.items.filter((item) => !item.isFolder));
    });
  }

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
      onOk: () => {
        props.selectGroup === 'private' && emit('delete:file:private', record);
        props.selectGroup === 'public' && emit('delete:file:public', record);
      },
    });
  }

  function handleShare(record) {
    openModal(true);
    nextTick(() => {
      setFieldsValue(record);
    });
  }

  function handleShareFile() {
    validate().then((input) => {
      share(input).then(() => {
        createMessage.success(L('Successful'));
        resetFields();
        closeModal();
      });
    });
  }
</script>
