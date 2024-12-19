<script setup lang="ts">
import type { IdentityRoleDto } from '../../types/roles';

import { defineEmits, defineOptions, nextTick, ref, toValue } from 'vue';

import { useVbenModal, type VbenFormProps } from '@vben/common-ui';
import { $t } from '@vben/locales';

import {
  useVbenVxeGrid,
  type VxeGridListeners,
  type VxeGridProps,
} from '@abp/ui';

import { getUnaddedRoleListApi } from '../../api/organization-units';

defineOptions({
  name: 'SelectRoleModal',
});
const emits = defineEmits<{
  (event: 'confirm', items: IdentityRoleDto[]): void;
}>();

const selectedRoles = ref<IdentityRoleDto[]>([]);

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
  onConfirm: async () => {
    emits('confirm', toValue(selectedRoles));
  },
  onOpenChange: async (isOpen: boolean) => {
    isOpen && nextTick(onRefresh);
  },
  title: $t('AbpIdentity.Roles'),
});

const gridOptions: VxeGridProps<IdentityRoleDto> = {
  checkboxConfig: {
    highlight: true,
    labelField: 'name',
  },
  columns: [
    {
      align: 'left',
      field: 'name',
      title: $t('AbpIdentity.DisplayName:RoleName'),
      type: 'checkbox',
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page }, formValues) => {
        const state = modalApi.getData<Record<string, any>>();
        return await getUnaddedRoleListApi({
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

const gridEvents: VxeGridListeners<IdentityRoleDto> = {
  checkboxAll: (e) => {
    selectedRoles.value = e.records;
  },
  checkboxChange: (e) => {
    selectedRoles.value = e.records;
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
