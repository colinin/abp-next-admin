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
      title: $t('AbpIdentity.DisplayName:UserName'),
    },
    {
      field: 'email',
      minWidth: '120px',
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
      query: async ({ page }, formValues) => {
        if (!props.selectedKey) {
          return {
            totalCount: 0,
            items: [],
          };
        }
        return await getUserListApi(props.selectedKey!, {
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
const [Grid, { query, setLoading }] = useVbenVxeGrid({
  gridEvents,
  gridOptions,
});
const [MemberModal, userModalApi] = useVbenModal({
  connectedComponent: SelectMemberModal,
});

const onRefresh = () => {
  nextTick(query);
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
    onOk: () => {
      setLoading(true);
      return removeOrganizationUnitApi(row.id, props.selectedKey!)
        .then(onRefresh)
        .finally(() => setLoading(false));
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

const onCreateMember = (users: IdentityUserDto[]) => {
  userModalApi.setState({
    submitting: true,
  });
  addMembers(props.selectedKey!, {
    userIds: users.map((item) => item.id),
  })
    .then(() => {
      userModalApi.close();
      query();
    })
    .finally(() => {
      userModalApi.setState({
        submitting: false,
      });
    });
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
