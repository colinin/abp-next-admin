<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { IdentityRoleDto } from '../../types/roles';

import { defineEmits, defineOptions, nextTick, ref, toValue } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useVbenVxeGrid } from '@abp/ui';

import { useOrganizationUnitsApi } from '../../api/useOrganizationUnitsApi';

defineOptions({
  name: 'SelectRoleModal',
});
const emits = defineEmits<{
  (event: 'confirm', items: IdentityRoleDto[]): void;
}>();
const { cancel, getUnaddedRoleListApi } = useOrganizationUnitsApi();

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
  onClosed() {
    cancel();
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
  columns: [
    {
      align: 'center',
      type: 'checkbox',
      width: 80,
    },
    {
      align: 'left',
      field: 'name',
      sortable: true,
      title: $t('AbpIdentity.DisplayName:RoleName'),
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }, formValues) => {
        const sorting = sort.order ? `${sort.field} ${sort.order}` : undefined;
        const state = modalApi.getData<Record<string, any>>();
        return await getUnaddedRoleListApi({
          id: state.id,
          maxResultCount: page.pageSize,
          sorting,
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
  toolbarConfig: {
    refresh: {
      code: 'query',
    },
  },
};

const gridEvents: VxeGridListeners<IdentityRoleDto> = {
  checkboxAll: (e) => {
    selectedRoles.value = e.records;
  },
  checkboxChange: (e) => {
    selectedRoles.value = e.records;
  },
  sortChange: () => {
    gridApi.query();
  },
};
const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

function onRefresh() {
  nextTick(gridApi.query);
}
</script>

<template>
  <Modal>
    <Grid />
  </Modal>
</template>

<style scoped></style>
