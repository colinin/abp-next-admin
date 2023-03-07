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
    <Card style="height: 100%">
      <div :style="getContentStyle" ref="contentWrapRef">
        <ScrollContainer ref="contentScrollRef">
          <template v-for="securityLog in securityLogs" :key="securityLog.id">
            <CardGrid>
              <Form layout="horizontal" :colon="false" :model="securityLog" :labelCol="{ span: 4 }" :wrapperCol="{ span: 16 }">
                <FormItem labelAlign="left" :label="L('ApplicationName')">
                  <span>{{ securityLog.applicationName }}</span>
                </FormItem>
                <FormItem labelAlign="left" :label="L('Identity')">
                  <span>{{ securityLog.identity }}</span>
                </FormItem>
                <FormItem labelAlign="left" :label="L('Actions')">
                  <span>{{ securityLog.action }}</span>
                </FormItem>
                <FormItem labelAlign="left" :label="L('ClientId')">
                  <span>{{ securityLog.clientId }}</span>
                </FormItem>
                <FormItem labelAlign="left" :label="L('ClientIpAddress')">
                  <span>{{ securityLog.clientIpAddress }}</span>
                </FormItem>
                <FormItem labelAlign="left" :label="L('BrowserInfo')">
                  <span>{{ securityLog.browserInfo }}</span>
                </FormItem>
                <FormItem labelAlign="left" :label="L('CreationTime')">
                  <span>{{ formatToDateTime(securityLog.creationTime) }}</span>
                </FormItem>
              </Form>
            </CardGrid>
          </template>
        </ScrollContainer>
      </div>
    </Card>
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
  import { Card, Form, Pagination } from 'ant-design-vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { ScrollContainer } from '/@/components/Container';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useContentHeight } from '/@/hooks/web/useContentHeight';
  import { useUserStoreWithOut } from '/@/store/modules/user';
  import { getList } from '/@/api/identity/securityLog';
  import { SecurityLog } from '/@/api/identity/model/securityLogModel';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import { formatToDateTime } from '/@/utils/dateUtil';

  const CardGrid = Card.Grid;
  const FormItem = Form.Item;
  const APagination = Pagination;

  const props = defineProps({
    autoContentHeight: {
      type: Boolean,
      default: true,
    }
  });

  const contentWrapRef = ref<any>();
  const contentScrollRef = ref<any>();
  const paginationRef = ref<any>();
  const getContentHeight = computed(() => props.autoContentHeight);
  const { contentHeight } = useContentHeight(
    getContentHeight, contentWrapRef, [paginationRef], []
  );
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
