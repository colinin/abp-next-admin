<script setup lang="ts">
import type { IdentityRoleDto } from '../../types/roles';

import { computed, defineAsyncComponent, h, nextTick, watch } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import {
  useVbenVxeGrid,
  type VxeGridListeners,
  type VxeGridProps,
} from '@abp/ui';
import { DeleteOutlined, PlusOutlined } from '@ant-design/icons-vue';
import { Button, Modal } from 'ant-design-vue';

import { useOrganizationUnitsApi } from '../../api/useOrganizationUnitsApi';
import { useRolesApi } from '../../api/useRolesApi';
import { OrganizationUnitPermissions } from '../../constants/permissions';

defineOptions({
  name: 'OrganizationUnitRoleTable',
});

const props = defineProps<{
  selectedKey?: string;
}>();

const SelectRoleModal = defineAsyncComponent(
  () => import('./SelectRoleModal.vue'),
);

const { hasAccessByCodes } = useAccess();
const { addRoles, getRoleListApi } = useOrganizationUnitsApi();
const { cancel, removeOrganizationUnitApi } = useRolesApi();

const getAddRoleEnabled = computed(() => {
  return (
    props.selectedKey &&
    hasAccessByCodes([OrganizationUnitPermissions.ManageRoles])
  );
});

const gridOptions: VxeGridProps<IdentityRoleDto> = {
  columns: [
    {
      field: 'name',
      minWidth: '100px',
      title: $t('AbpIdentity.DisplayName:RoleName'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'actions' },
      title: $t('AbpUi.Actions'),
      width: 180,
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page }, formValues) => {
        if (!props.selectedKey) {
          return {
            totalCount: 0,
            items: [],
          };
        }
        return await getRoleListApi(props.selectedKey!, {
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

const gridEvents: VxeGridListeners<IdentityRoleDto> = {
  cellClick: () => {},
};
const [Grid, { query, setLoading }] = useVbenVxeGrid({
  gridEvents,
  gridOptions,
});

const [RoleModal, roleModalApi] = useVbenModal({
  connectedComponent: SelectRoleModal,
});

const onRefresh = () => {
  return nextTick(query);
};

const onDelete = (row: IdentityRoleDto) => {
  Modal.confirm({
    centered: true,
    content: $t('AbpIdentity.OrganizationUnit:AreYouSureRemoveRole', [
      row.name,
    ]),
    onCancel: () => {
      cancel('User closed cancel delete modal.');
    },
    onOk: async () => {
      try {
        setLoading(true);
        await removeOrganizationUnitApi(row.id, props.selectedKey!);
        await onRefresh();
      } finally {
        setLoading(false);
      }
    },
    title: $t('AbpUi.AreYouSure'),
  });
};

const onShowRole = () => {
  roleModalApi.setData({
    id: props.selectedKey,
  });
  roleModalApi.open();
};

const onCreateRole = async (roles: IdentityRoleDto[]) => {
  try {
    roleModalApi.setState({
      closable: false,
      confirmLoading: true,
    });
    await addRoles(props.selectedKey!, {
      roleIds: roles.map((item) => item.id),
    });
    roleModalApi.close();
    await query();
  } finally {
    roleModalApi.setState({
      closable: true,
      confirmLoading: false,
    });
  }
};
watch(() => props.selectedKey, onRefresh);
</script>

<template>
  <Grid :table-title="$t('AbpIdentity.Roles')">
    <template #toolbar-tools>
      <Button
        v-if="getAddRoleEnabled"
        :icon="h(PlusOutlined)"
        type="primary"
        @click="onShowRole"
      >
        {{ $t('AbpIdentity.OrganizationUnit:AddRole') }}
      </Button>
    </template>
    <template #actions="{ row }">
      <Button
        :icon="h(DeleteOutlined)"
        danger
        type="link"
        v-access:code="[OrganizationUnitPermissions.ManageRoles]"
        @click="onDelete(row)"
      >
        {{ $t('AbpUi.Delete') }}
      </Button>
    </template>
  </Grid>
  <RoleModal @confirm="onCreateRole" />
</template>

<style scoped></style>
