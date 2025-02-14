<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { IdentityUserDto } from '../../types/users';

import { defineEmits, defineOptions, nextTick, ref, toValue } from 'vue';

import { useVbenModal, type VbenFormProps } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useVbenVxeGrid } from '@abp/ui';

import { useOrganizationUnitsApi } from '../../api/useOrganizationUnitsApi';

defineOptions({
  name: 'SelectMemberModal',
});
const emits = defineEmits<{
  (event: 'confirm', items: IdentityUserDto[]): void;
}>();
const { cancel, getUnaddedUserListApi } = useOrganizationUnitsApi();

const selectedUsers = ref<IdentityUserDto[]>([]);

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: false,
  resetButtonOptions: {
    show: false,
  },
  schema: [
    {
      component: 'Input',
      componentProps: {
        allowClear: true,
        autocomplete: 'off',
      },
      fieldName: 'filter',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('AbpUi.Search'),
    },
  ],
  // 控制表单是否显示折叠按钮
  showCollapseButton: false,
  // 按下回车时是否提交表单
  submitOnEnter: true,
};
const [Modal, modalApi] = useVbenModal({
  closeOnClickModal: false,
  closeOnPressEscape: false,
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onClosed() {
    cancel();
  },
  onConfirm: async () => {
    emits('confirm', toValue(selectedUsers));
  },
  onOpenChange: async (isOpen: boolean) => {
    isOpen && nextTick(onRefresh);
  },
  title: $t('AbpIdentity.Users'),
});

const gridOptions: VxeGridProps<IdentityUserDto> = {
  checkboxConfig: {
    highlight: true,
    labelField: 'userName',
  },
  columns: [
    {
      align: 'left',
      field: 'userName',
      minWidth: '100px',
      title: $t('AbpIdentity.DisplayName:UserName'),
      type: 'checkbox',
    },
    {
      align: 'left',
      field: 'email',
      minWidth: '120px',
      title: $t('AbpIdentity.DisplayName:Email'),
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page }, formValues) => {
        const state = modalApi.getData<Record<string, any>>();
        return await getUnaddedUserListApi({
          id: state.id,
          maxResultCount: page.pageSize,
          skipCount: (page.currentPage - 1) * page.pageSize,
          ...formValues,
        });
      },
    },
    autoLoad: false,
    response: {
      total: 'totalCount',
      list: 'items',
    },
  },
  toolbarConfig: {},
};

const gridEvents: VxeGridListeners<IdentityUserDto> = {
  checkboxAll: (e) => {
    selectedUsers.value = e.records;
  },
  checkboxChange: (e) => {
    selectedUsers.value = e.records;
  },
};
const [Grid, { query }] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

function onRefresh() {
  nextTick(query);
}
</script>

<template>
  <Modal>
    <Grid />
  </Modal>
</template>

<style scoped></style>
