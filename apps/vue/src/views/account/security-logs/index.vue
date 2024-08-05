<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :width="800"
    :height="500"
    :title="L('SecurityLog')"
    :mask-closable="false"
    :can-fullscreen="false"
    :show-ok-btn="false"
  >
  <div :style="getContentStyle" ref="contentWrapRef">
    <ScrollContainer ref="contentScrollRef">
      <template v-for="securityLog in securityLogs" :key="securityLog.id">
        <Card hoverable style="height: 100%; margin-bottom: 15px;">
          <Descriptions bordered size="small" :column="1">
            <DescriptionItem :label="L('ApplicationName')">{{ securityLog.applicationName }}</DescriptionItem>
            <DescriptionItem :label="L('Identity')">{{ securityLog.identity }}</DescriptionItem>
            <DescriptionItem :label="L('Actions')">{{ securityLog.action }}</DescriptionItem>
            <DescriptionItem :label="L('ClientId')">{{ securityLog.clientId }}</DescriptionItem>
            <DescriptionItem :label="L('ClientIpAddress')">{{ securityLog.clientIpAddress }}</DescriptionItem>
            <DescriptionItem :label="L('BrowserInfo')">{{ formatToDateTime(securityLog.browserInfo) }}</DescriptionItem>
            <DescriptionItem :label="L('CreationTime')">{{ formatToDateTime(securityLog.creationTime) }}</DescriptionItem>
          </Descriptions>
        </Card>
      </template>
    </ScrollContainer>
  </div>
    <template #footer>
      <APagination
        ref="paginationRef"
        :pageSizeOptions="['10', '25', '50', '100']"
        :total="securityLogTotal"
        @change="fetchSecurityLogs"
        @showSizeChange="fetchSecurityLogs"
      />
    </template>
  </BasicModal>
</template>

<script lang="ts" setup>
  import type { CSSProperties } from 'vue';
  import { computed, ref } from 'vue';
  import { Card, Descriptions, Pagination } from 'ant-design-vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { ScrollContainer } from '/@/components/Container';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useContentHeight } from '/@/hooks/web/useContentHeight';
  import { useUserStoreWithOut } from '/@/store/modules/user';
  import { getList } from '/@/api/auditing/security-logs';
  import { SecurityLog } from '/@/api/auditing/security-logs/model';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import { formatToDateTime } from '/@/utils/dateUtil';

  const APagination = Pagination;
  const DescriptionItem = Descriptions.Item;

  const props = defineProps({
    autoContentHeight: {
      type: Boolean,
      default: true,
    },
  });

  const contentWrapRef = ref<any>();
  const contentScrollRef = ref<any>();
  const paginationRef = ref<any>();
  const getContentHeight = computed(() => props.autoContentHeight);
  const { contentHeight } = useContentHeight(getContentHeight, contentWrapRef, [paginationRef], []);
  const getContentStyle = computed((): CSSProperties => {
    return {
      width: '100%',
      height: `${contentHeight.value}px`,
    };
  });
  const securityLogs = ref<SecurityLog[]>([]);
  const securityLogTotal = ref(0);
  const { L } = useLocalization(['AbpAuditLogging', 'AbpIdentity']);
  const userStore = useUserStoreWithOut();
  const [registerModal] = useModalInner(() => {
    console.log('fetchSecurityLogs');
    fetchSecurityLogs();
  });

  function fetchSecurityLogs(page: number = 1, pageSize: number = 10) {
    const request = {
      skipCount: page,
      maxResultCount: pageSize,
    };
    formatPagedRequest(request);
    getList({
      skipCount: request.skipCount,
      maxResultCount: request.maxResultCount,
      sorting: 'creationTime DESC',
      userId: userStore.getUserInfo.userId,
    }).then((res) => {
      securityLogs.value = res.items;
      securityLogTotal.value = res.totalCount;
    });
  }
</script>

<style lang="less" scoped>
  .ant-card-grid {
    width: 100%;
  }
</style>
