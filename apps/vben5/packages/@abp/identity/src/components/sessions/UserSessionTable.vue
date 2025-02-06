<script setup lang="ts">
import type { VxeGridProps } from '@abp/ui';

import type { IdentitySessionDto } from '../../types/sessions';

import { computed, nextTick, reactive, watch } from 'vue';

import { useAccess } from '@vben/access';
import { $t } from '@vben/locales';

import { useAbpStore } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import { Button, Descriptions, Tag } from 'ant-design-vue';

import { IdentitySessionPermissions } from '../../constants/permissions';

const props = defineProps<{
  sessions: IdentitySessionDto[];
}>();
const emits = defineEmits<{
  (event: 'revoke', session: IdentitySessionDto): void;
}>();
const DescriptionItem = Descriptions.Item;

const { hasAccessByCodes } = useAccess();
const abpStore = useAbpStore();
/** 获取登录用户会话Id */
const getMySessionId = computed(() => {
  return abpStore.application?.currentUser.sessionId;
});
/** 获取是否允许撤销会话 */
const getAllowRevokeSession = computed(() => {
  return (session: IdentitySessionDto) => {
    if (getMySessionId.value === session.sessionId) {
      return false;
    }
    return hasAccessByCodes([IdentitySessionPermissions.Revoke]);
  };
});

const gridOptions = reactive<VxeGridProps<IdentitySessionDto>>({
  columns: [
    {
      align: 'left',
      slots: { content: 'deviceInfo' },
      type: 'expand',
      width: 50,
    },
    {
      align: 'left',
      field: 'device',
      slots: { default: 'device' },
      title: $t('AbpIdentity.DisplayName:Device'),
      width: 150,
    },
    {
      align: 'left',
      field: 'signedIn',
      minWidth: 200,
      title: $t('AbpIdentity.DisplayName:SignedIn'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 120,
    },
  ],
  expandConfig: {
    padding: true,
    trigger: 'default',
  },
  exportConfig: {},
  keepSource: true,
  pagerConfig: {
    autoHidden: true,
  },
  toolbarConfig: {},
});

const [Grid, gridApi] = useVbenVxeGrid({ gridOptions });

watch(
  () => props.sessions,
  (sessions) => {
    nextTick(() => {
      gridApi.setGridOptions({
        data: sessions,
      });
    });
  },
  {
    immediate: true,
  },
);

function onDelete(session: IdentitySessionDto) {
  emits('revoke', session);
}
</script>

<template>
  <Grid>
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
    <template #deviceInfo="{ row }">
      <Descriptions :colon="false" :column="2" bordered size="small">
        <DescriptionItem
          :label="$t('AbpIdentity.DisplayName:SessionId')"
          :span="2"
        >
          {{ row.sessionId }}
        </DescriptionItem>
        <DescriptionItem :label="$t('AbpIdentity.DisplayName:Device')">
          {{ row.device }}
        </DescriptionItem>
        <DescriptionItem :label="$t('AbpIdentity.DisplayName:DeviceInfo')">
          {{ row.deviceInfo }}
        </DescriptionItem>
        <DescriptionItem :label="$t('AbpIdentity.DisplayName:ClientId')">
          {{ row.clientId }}
        </DescriptionItem>
        <DescriptionItem :label="$t('AbpIdentity.DisplayName:IpAddresses')">
          {{ row.ipAddresses }}
        </DescriptionItem>
        <DescriptionItem :label="$t('AbpIdentity.DisplayName:SignedIn')">
          {{ row.signedIn }}
        </DescriptionItem>
        <DescriptionItem :label="$t('AbpIdentity.DisplayName:LastAccessed')">
          {{ row.lastAccessed }}
        </DescriptionItem>
      </Descriptions>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          v-if="getAllowRevokeSession(row)"
          danger
          size="small"
          @click="onDelete(row)"
        >
          {{ $t('AbpIdentity.RevokeSession') }}
        </Button>
      </div>
    </template>
  </Grid>
</template>

<style scoped></style>
