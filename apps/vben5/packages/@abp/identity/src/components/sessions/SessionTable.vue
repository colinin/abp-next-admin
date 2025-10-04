<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';
import type { SelectValue } from 'ant-design-vue/es/select';

import type { VbenFormProps } from '@vben/common-ui';

import type { IdentityUserDto } from '../../types';
import type { IdentitySessionDto } from '../../types/sessions';

import { computed, h, onMounted, ref } from 'vue';

import { useAccess } from '@vben/access';
import { $t } from '@vben/locales';

import { useAbpStore } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import { DeleteOutlined } from '@ant-design/icons-vue';
import { Button, message, Modal, Select, Tag } from 'ant-design-vue';
import debounce from 'lodash.debounce';

import { useUsersApi } from '../../api/useUsersApi';
import { useUserSessionsApi } from '../../api/useUserSessionsApi';
import { IdentitySessionPermissions } from '../../constants/permissions';

defineOptions({
  name: 'SessionTable',
});
const { hasAccessByCodes } = useAccess();
const { getPagedListApi: getUserListApi } = useUsersApi();
const { cancel, getSessionsApi, revokeSessionApi } = useUserSessionsApi();

const abpStore = useAbpStore();

const users = ref<IdentityUserDto[]>([]);

/** 获取登录用户会话Id */
const getMySessionId = computed(() => {
  return abpStore.application?.currentUser.sessionId;
});
/** 获取是否允许撤销会话 */
const getAllowRevokeSession = computed(() => {
  return (session: IdentitySessionDto) => {
    return getMySessionId.value !== session.sessionId;
  };
});

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: false,
  schema: [
    {
      component: 'Select',
      fieldName: 'userId',
      label: $t('AbpIdentity.DisplayName:UserName'),
    },
    {
      component: 'Input',
      fieldName: 'clientId',
      label: $t('AbpIdentity.DisplayName:ClientId'),
    },
    {
      component: 'Input',
      fieldName: 'device',
      label: $t('AbpIdentity.DisplayName:Device'),
    },
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

const gridOptions: VxeGridProps<IdentitySessionDto> = {
  columns: [
    {
      align: 'left',
      field: 'sessionId',
      minWidth: 150,
      sortable: true,
      title: $t('AbpIdentity.DisplayName:SessionId'),
    },
    {
      align: 'left',
      field: 'device',
      minWidth: 120,
      slots: { default: 'device' },
      sortable: true,
      title: $t('AbpIdentity.DisplayName:Device'),
    },
    {
      align: 'left',
      field: 'deviceInfo',
      sortable: true,
      title: $t('AbpIdentity.DisplayName:DeviceInfo'),
      width: 'auto',
    },
    {
      align: 'left',
      field: 'clientId',
      minWidth: 120,
      sortable: true,
      title: $t('AbpIdentity.DisplayName:ClientId'),
    },
    {
      align: 'left',
      field: 'ipAddresses',
      minWidth: 120,
      sortable: true,
      title: $t('AbpIdentity.DisplayName:IpAddresses'),
    },
    {
      align: 'left',
      field: 'signedIn',
      minWidth: 120,
      sortable: true,
      title: $t('AbpIdentity.DisplayName:SignedIn'),
    },
    {
      align: 'left',
      field: 'lastAccessed',
      minWidth: 120,
      sortable: true,
      title: $t('AbpIdentity.DisplayName:LastAccessed'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      visible: hasAccessByCodes([
        IdentitySessionPermissions.Default,
        IdentitySessionPermissions.Revoke,
      ]),
      width: 150,
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }, formValues) => {
        const sorting = sort.order ? `${sort.field} ${sort.order}` : undefined;
        return await getSessionsApi({
          sorting,
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
    refresh: {
      code: 'query',
    },
    zoom: true,
  },
};

const gridEvents: VxeGridListeners<IdentitySessionDto> = {
  cellClick: () => {},
  sortChange: () => {
    gridApi.query();
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const onDelete = (row: IdentitySessionDto) => {
  Modal.confirm({
    centered: true,
    content: $t('AbpIdentity.SessionWillBeRevokedMessage'),
    onCancel: () => {
      cancel();
    },
    onOk: async () => {
      await revokeSessionApi(row.sessionId);
      message.success($t('AbpIdentity.SuccessfullyRevoked'));
      await gridApi.query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
};

const onGetUsers = debounce(async (filter?: string) => {
  const { items } = await getUserListApi({ filter });
  users.value = items;
}, 500);

function onFieldChange(field: string, value?: SelectValue) {
  gridApi.formApi.setFieldValue(field, value);
}

onMounted(onGetUsers);
</script>

<template>
  <Grid :table-title="$t('AbpIdentity.IdentitySessions')">
    <template #form-userId="{ modelValue }">
      <Select
        :default-active-first-option="false"
        :field-names="{ label: 'userName', value: 'id' }"
        :filter-option="false"
        :options="users"
        :placeholder="$t('ui.placeholder.select')"
        :value="modelValue"
        allow-clear
        class="w-full"
        show-search
        @change="(val) => onFieldChange('userId', val)"
        @search="onGetUsers"
      />
    </template>
    <template #device="{ row }">
      <div class="flex flex-row">
        <span>{{ row.device }}</span>
        <div class="pl-[5px]">
          <Tag v-if="row.sessionId === getMySessionId" color="#87d068">
            {{ $t('AbpIdentity.CurrentSession') }}
          </Tag>
        </div>
      </div>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          v-if="getAllowRevokeSession(row)"
          :icon="h(DeleteOutlined)"
          block
          danger
          type="link"
          v-access:code="[IdentitySessionPermissions.Revoke]"
          @click="onDelete(row)"
        >
          {{ $t('AbpIdentity.RevokeSession') }}
        </Button>
      </div>
    </template>
  </Grid>
</template>

<style lang="scss" scoped></style>
