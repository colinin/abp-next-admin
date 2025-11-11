<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { IdentityUserDto } from '../../types/users';

import { computed, defineAsyncComponent, h, nextTick, watch } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useVbenVxeGrid } from '@abp/ui';
import { DeleteOutlined, PlusOutlined } from '@ant-design/icons-vue';
import { Button, Modal } from 'ant-design-vue';

import { useOrganizationUnitsApi } from '../../api/useOrganizationUnitsApi';
import { useUsersApi } from '../../api/useUsersApi';
import { OrganizationUnitPermissions } from '../../constants/permissions';

defineOptions({
  name: 'OrganizationUnitUserTable',
});

const props = defineProps<{
  selectedKey?: string;
}>();
const { cancel, removeOrganizationUnitApi } = useUsersApi();
const { addMembers, getUserListApi } = useOrganizationUnitsApi();

const SelectMemberModal = defineAsyncComponent(
  () => import('./SelectMemberModal.vue'),
);

const { hasAccessByCodes } = useAccess();

const getAddMemberEnabled = computed(() => {
  return (
    props.selectedKey &&
    hasAccessByCodes([OrganizationUnitPermissions.ManageUsers])
  );
});

const gridOptions: VxeGridProps<IdentityUserDto> = {
  columns: [
    {
      field: 'userName',
      minWidth: '100px',
      sortable: true,
      title: $t('AbpIdentity.DisplayName:UserName'),
    },
    {
      field: 'email',
      minWidth: '120px',
      sortable: true,
      title: $t('AbpIdentity.DisplayName:Email'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'actions' },
      title: $t('AbpUi.Actions'),
      width: 120,
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }, formValues) => {
        if (!props.selectedKey) {
          return {
            totalCount: 0,
            items: [],
          };
        }
        const sorting = sort.order ? `${sort.field} ${sort.order}` : undefined;
        return await getUserListApi(props.selectedKey!, {
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

const gridEvents: VxeGridListeners<IdentityUserDto> = {
  cellClick: () => {},
  sortChange: () => {
    gridApi.query();
  },
};
const [Grid, gridApi] = useVbenVxeGrid({
  gridEvents,
  gridOptions,
});
const [MemberModal, userModalApi] = useVbenModal({
  connectedComponent: SelectMemberModal,
});

const onRefresh = () => {
  nextTick(gridApi.query);
};

const onDelete = (row: IdentityUserDto) => {
  Modal.confirm({
    centered: true,
    content: $t('AbpIdentity.OrganizationUnit:AreYouSureRemoveUser', [
      row.userName,
    ]),
    onCancel: () => {
      cancel('User closed cancel delete modal.');
    },
    onOk: async () => {
      gridApi.setLoading(true);
      try {
        await removeOrganizationUnitApi(row.id, props.selectedKey!);
        await gridApi.query();
      } finally {
        gridApi.setLoading(false);
      }
    },
    title: $t('AbpUi.AreYouSure'),
  });
};

const onShowMember = () => {
  userModalApi.setData({
    id: props.selectedKey,
  });
  userModalApi.open();
};

const onCreateMember = async (users: IdentityUserDto[]) => {
  userModalApi.setState({
    submitting: true,
  });
  try {
    await addMembers(props.selectedKey!, {
      userIds: users.map((item) => item.id),
    });
    userModalApi.close();
    await gridApi.query();
  } finally {
    userModalApi.setState({
      submitting: false,
    });
  }
};

watch(() => props.selectedKey, onRefresh);
</script>

<template>
  <Grid :table-title="$t('AbpIdentity.Users')">
    <template #toolbar-tools>
      <Button
        v-if="getAddMemberEnabled"
        :icon="h(PlusOutlined)"
        type="primary"
        @click="onShowMember"
      >
        {{ $t('AbpIdentity.OrganizationUnit:AddMember') }}
      </Button>
    </template>
    <template #actions="{ row }">
      <Button
        :icon="h(DeleteOutlined)"
        danger
        type="link"
        v-access:code="[OrganizationUnitPermissions.ManageUsers]"
        @click="onDelete(row)"
      >
        {{ $t('AbpUi.Delete') }}
      </Button>
    </template>
  </Grid>
  <MemberModal @confirm="onCreateMember" />
</template>

<style scoped></style>
