<script setup lang="ts">
import type { VbenFormProps, VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { IdentityUserDto } from '../../types/user';

import { defineAsyncComponent, h } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { formatToDateTime } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import { DeleteOutlined, EditOutlined } from '@ant-design/icons-vue';
import { Button, Modal } from 'ant-design-vue';

import { deleteApi, getPagedListApi } from '../../api/user';

defineOptions({
  name: 'UserTable',
});

const UserModal = defineAsyncComponent(() => import('./UserModal.vue'));
const CheckIcon = createIconifyIcon('ant-design:check-outlined');
const CloseIcon = createIconifyIcon('ant-design:close-outlined');

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: false,
  schema: [
    {
      component: 'Input',
      fieldName: 'filter',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('AbpUi.Search'),
    },
  ],
  // 控制表单是否显示折叠按钮
  showCollapseButton: true,
  // 按下回车时是否提交表单
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<IdentityUserDto> = {
  columns: [
    {
      field: 'isActive',
      slots: { default: 'active' },
      title: $t('AbpIdentity.DisplayName:IsActive'),
    },
    {
      field: 'userName',
      minWidth: '100px',
      title: $t('AbpIdentity.DisplayName:UserName'),
    },
    {
      field: 'email',
      minWidth: '120px',
      title: $t('AbpIdentity.DisplayName:Email'),
    },
    { field: 'surname', title: $t('AbpIdentity.DisplayName:Surname') },
    { field: 'name', title: $t('AbpIdentity.DisplayName:Name') },
    { field: 'phoneNumber', title: $t('AbpIdentity.DisplayName:PhoneNumber') },
    {
      field: 'lockoutEnd',
      formatter: ({ cellValue }) => {
        return cellValue ? formatToDateTime(cellValue) : '';
      },
      title: $t('AbpIdentity.LockoutEnd'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 180,
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page }, formValues) => {
        return await getPagedListApi({
          maxResultCount: page.pageSize,
          skipCount: (page.currentPage - 1) * page.pageSize,
          ...formValues,
        });
      },
    },
    response: {
      total: 'totalCount',
      list: 'items',
    },
  },
  toolbarConfig: {
    custom: true,
    export: true,
    // import: true,
    refresh: true,
    zoom: true,
  },
};

const gridEvents: VxeGridListeners<IdentityUserDto> = {
  cellClick: () => {},
};
const [UserEditModal, userModalApi] = useVbenModal({
  connectedComponent: UserModal,
});
const [Grid, { query }] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const handleAdd = () => {
  userModalApi.setData({});
  userModalApi.open();
};

const handleEdit = (row: IdentityUserDto) => {
  userModalApi.setData({
    values: row,
  });
  userModalApi.open();
};

const handleDelete = (row: IdentityUserDto) => {
  Modal.confirm({
    centered: true,
    content: $t('AbpIdentity.UserDeletionConfirmationMessage', [row.userName]),
    onOk: () => {
      return deleteApi(row.id).then(() => query());
    },
    title: $t('AbpUi.AreYouSure'),
  });
};
</script>

<template>
  <Grid :table-title="$t('AbpIdentity.Users')">
    <template #toolbar-tools>
      <Button
        type="primary"
        v-access:code="['AbpIdentity.Users.Create']"
        @click="handleAdd"
      >
        {{ $t('AbpIdentity.NewUser') }}
      </Button>
    </template>
    <template #active="{ row }">
      <div class="active-box">
        <CheckIcon v-if="row.isActive" class="actived" />
        <CloseIcon v-else />
      </div>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <div class="basis-1/2">
          <Button
            :icon="h(EditOutlined)"
            block
            type="link"
            v-access:code="['AbpIdentity.Users.Update']"
            @click="handleEdit(row)"
          >
            {{ $t('AbpUi.Edit') }}
          </Button>
        </div>
        <div class="basis-1/2">
          <Button
            :icon="h(DeleteOutlined)"
            block
            danger
            type="link"
            v-access:code="['AbpIdentity.Users.Delete']"
            @click="handleDelete(row)"
          >
            {{ $t('AbpUi.Delete') }}
          </Button>
        </div>
      </div>
    </template>
  </Grid>
  <UserEditModal @change="() => query()" />
</template>

<style lang="scss" scoped>
.active-box {
  display: flex;
  justify-content: center;
  color: red;

  .actived {
    color: green;
  }
}
</style>
