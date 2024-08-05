<template>
    <BasicModal
      v-bind="$attrs"
      @register="registerModal"
      :width="800"
      :height="500"
      :title="L('IdentitySessions')"
      :mask-closable="false"
      :can-fullscreen="false"
      :show-ok-btn="false"
    >
      <div v-if="identitySessions.length <= 0">
        <Empty />
      </div>
      <div v-else :style="getContentStyle" ref="contentWrapRef" class="session">
        <ScrollContainer ref="contentScrollRef">
          <template v-for="identitySession in identitySessions" :key="identitySession.id">
            <Card style="height: 100%"> 
              <template #title>
                <div class="session__tile">
                  <span>{{ identitySession.device }}</span>
                  <div style="padding-left: 5px;">
                    <Tag
                      v-if="identitySession.sessionId === abpStore.getApplication.currentUser.sessionId"
                      color="#87d068"
                    >{{ L('CurrentSession') }}</Tag>
                  </div>
                </div>
              </template>
              <Descriptions bordered size="small" :column="1">
                <DescriptionItem :label="L('DisplayName:SessionId')">{{ identitySession.sessionId }}</DescriptionItem>
                <DescriptionItem :label="L('DisplayName:Device')">{{ identitySession.device }}</DescriptionItem>
                <DescriptionItem :label="L('DisplayName:DeviceInfo')">{{ identitySession.deviceInfo }}</DescriptionItem>
                <DescriptionItem :label="L('DisplayName:ClientId')">{{ identitySession.clientId }}</DescriptionItem>
                <DescriptionItem :label="L('DisplayName:IpAddresses')">{{ identitySession.ipAddresses }}</DescriptionItem>
                <DescriptionItem :label="L('DisplayName:SignedIn')">{{ formatToDateTime(identitySession.signedIn) }}</DescriptionItem>
                <DescriptionItem v-if="identitySession.lastAccessed" :label="L('DisplayName:LastAccessed')">{{ formatToDateTime(identitySession.lastAccessed) }}</DescriptionItem>
              </Descriptions>
              <template #extra>
                <Button
                  v-auth="['AbpIdentity.IdentitySessions.Revoke']"
                  v-if="identitySession.sessionId !== abpStore.getApplication.currentUser.sessionId"
                  danger
                  @click="handleRevokeSession(identitySession)"
                >{{ L('RevokeSession') }}</Button>
              </template>
            </Card>
          </template>
        </ScrollContainer>
      </div>
      <template #footer>
        <APagination
          v-if="identitySessions.length > 0"
          ref="paginationRef"
          :pageSizeOptions="['10', '25', '50', '100']"
          :total="identitySessionTotal"
          @change="fetchSessions"
          @showSizeChange="fetchSessions"
        />
      </template>
    </BasicModal>
  </template>
  
  <script lang="ts" setup>
    import type { CSSProperties } from 'vue';
    import { computed, ref } from 'vue';
    import { Button, Card, Descriptions, Empty, Pagination, Tag } from 'ant-design-vue';
    import { BasicModal, useModalInner } from '/@/components/Modal';
    import { ScrollContainer } from '/@/components/Container';
    import { useMessage } from '/@/hooks/web/useMessage';
    import { useLocalization } from '/@/hooks/abp/useLocalization';
    import { useContentHeight } from '/@/hooks/web/useContentHeight';
    import { useAbpStoreWithOut } from '/@/store/modules/abp';
    import { getSessions, revokeSession } from '/@/api/identity/sessions';
    import { IdentitySessionDto } from '/@/api/identity/sessions/model';
    import { formatPagedRequest } from '/@/utils/http/abp/helper';
    import { formatToDateTime } from '/@/utils/dateUtil';
  
    const DescriptionItem = Descriptions.Item;
    const APagination = Pagination;
  
    const props = defineProps({
      autoContentHeight: {
        type: Boolean,
        default: true,
      },
    });
  
    const userId = ref('');
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
    const identitySessions = ref<IdentitySessionDto[]>([]);
    const identitySessionTotal = ref(0);
    const abpStore = useAbpStoreWithOut();
    const { createConfirm, createMessage } = useMessage();
    const { L } = useLocalization(['AbpIdentity', 'AbpUi']);
    const [registerModal] = useModalInner((data: { userId: string }) => {
      userId.value = data.userId;
      identitySessions.value = [];
      fetchSessions();
    });
  
    function fetchSessions(page: number = 1, pageSize: number = 10) {
      const request = {
        skipCount: page,
        maxResultCount: pageSize,
      };
      formatPagedRequest(request);
      getSessions({
        userId: userId.value,
        skipCount: request.skipCount,
        maxResultCount: request.maxResultCount,
      }).then((res) => {
        identitySessions.value = res.items;
        identitySessionTotal.value = res.totalCount;
      });
    }

    function handleRevokeSession(session: IdentitySessionDto) {
      createConfirm({
        iconType: 'warning',
        title: L('AreYouSure'),
        content: L('SessionWillBeRevokedMessage'),
        onOk: async () => {
          await revokeSession(session.sessionId);
          createMessage.success(L('SuccessfullyRevoked'));
          fetchSessions();
        },
      });
    }
  </script>
  
  <style lang="less" scoped>
  .session {
    .session__tile {
      display: flex;
      flex-direction: row;
    }
  }
  </style>
  