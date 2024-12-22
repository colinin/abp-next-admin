<script setup lang="ts">
import type { VbenFormProps, VxeGridProps } from '@abp/ui';

import type { OpenIddictAuthorizationDto } from '../../types/authorizations';

import { defineAsyncComponent, h } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, message, Modal } from 'ant-design-vue';

import { deleteApi, getPagedListApi } from '../../api/authorizations';
import { ApplicationsPermissions } from '../../constants/permissions';

defineOptions({
  name: 'AuthorizationTable',
});

const CheckIcon = createIconifyIcon('ant-design:check-outlined');
const CloseIcon = createIconifyIcon('ant-design:close-outlined');

const { hasAccessByCodes } = useAccess();

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

const gridOptions: VxeGridProps<OpenIddictAuthorizationDto> = {
  columns: [
    {
      align: 'left',
      field: 'clientId',
      minWidth: 150,
      title: $t('AbpOpenIddict.DisplayName:ClientId'),
    },
    {
      align: 'left',
      field: 'displayName',
      minWidth: 150,
      title: $t('AbpOpenIddict.DisplayName:DisplayName'),
    },
    {
      align: 'center',
      field: 'consentType',
      title: $t('AbpOpenIddict.DisplayName:ConsentType'),
      width: 200,
    },
    {
      align: 'center',
      field: 'clientType',
      title: $t('AbpOpenIddict.DisplayName:ClientType'),
      width: 120,
    },
    {
      align: 'center',
      field: 'applicationType',
      title: $t('AbpOpenIddict.DisplayName:ApplicationType'),
      width: 150,
    },
    {
      align: 'left',
      field: 'clientUri',
      title: $t('AbpOpenIddict.DisplayName:ClientUri'),
      width: 150,
    },
    {
      align: 'left',
      field: 'logoUri',
      title: $t('AbpOpenIddict.DisplayName:LogoUri'),
      width: 120,
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      visible: hasAccessByCodes([
        ApplicationsPermissions.Update,
        ApplicationsPermissions.Delete,
      ]),
      width: 220,
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
const [AuthorizationModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./AuthorizationModal.vue'),
  ),
});

const [Grid, { query }] = useVbenVxeGrid({
  formOptions,
  gridOptions,
});

const onCreate = () => {
  modalApi.setData({});
  modalApi.open();
};

const onUpdate = (row: OpenIddictAuthorizationDto) => {
  modalApi.setData(row);
  modalApi.open();
};

const onDelete = (row: OpenIddictAuthorizationDto) => {
  Modal.confirm({
    centered: true,
    content: `${$t('AbpUi.ItemWillBeDeletedMessageWithFormat')}`,
    onOk: () => {
      return deleteApi(row.id).then(() => {
        message.success($t('AbpUi.SuccessfullyDeleted'));
        query();
      });
    },
    title: $t('AbpUi.AreYouSure'),
  });
};
</script>

<template>
  <Grid :table-title="$t('AbpOpenIddict.Applications')">
    <template #toolbar-tools>
      <Button
        :icon="h(PlusOutlined)"
        type="primary"
        v-access:code="[ApplicationsPermissions.Create]"
        @click="onCreate"
      >
        {{ $t('AbpOpenIddict.Applications:AddNew') }}
      </Button>
    </template>
    <template #required="{ row }">
      <div class="flex flex-row justify-center">
        <CheckIcon v-if="row.required" class="text-green-500" />
        <CloseIcon v-else class="text-red-500" />
      </div>
    </template>
    <template #static="{ row }">
      <div class="flex flex-row justify-center">
        <CheckIcon v-if="row.isStatic" class="text-green-500" />
        <CloseIcon v-else class="text-red-500" />
      </div>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <div class="basis-1/2">
          <Button
            :icon="h(EditOutlined)"
            block
            type="link"
            v-access:code="[ApplicationsPermissions.Update]"
            @click="onUpdate(row)"
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
            v-access:code="[ApplicationsPermissions.Delete]"
            @click="onDelete(row)"
          >
            {{ $t('AbpUi.Delete') }}
          </Button>
        </div>
      </div>
    </template>
  </Grid>
  <AuthorizationModal @change="() => query()" />
</template>

<style lang="scss" scoped></style>
