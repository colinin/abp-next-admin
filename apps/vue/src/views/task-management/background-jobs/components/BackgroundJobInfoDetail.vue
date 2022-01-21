<template>
  <PageWrapper>
    <template #title>
      <ArrowLeftOutlined @click="handleBack" />
      {{ L('BackgroundJobDetail') }}
    </template>
    <Skeleton :loading="jobInfo === undefined">
      <Card class="mt-4" :title="L('BasicInfo')">
        <Description @register="registerDescription" :data="jobInfo" />
      </Card>
      <Card class="mt-4" :title="L('BackgroundJobLogs')">
        <List
          ref="logListElRef"
          item-layout="vertical"
          size="default"
          bordered
          :loading="fetchingLog"
          :pagination="{
            pageSize: fetchLogCount,
            total: maxLogCount,
            showSizeChanger: true,
            onChange: fetchJobLogs,
            onShowSizeChange: handleSizeChange,
          }"
          :data-source="jobLogs"
        >
          <template #renderItem="{ item }">
            <ListItem :key="item.id">
              <ListItemMeta :description="item.message">
                <template #avatar>
                  <Icon v-if="!item.exception" :size="40" icon="grommet-icons:status-good" color="seagreen" />
                  <Icon v-else :size="40" icon="grommet-icons:status-warning" color="orangered" />
                </template>
                <template #title>
                  <span>{{ item.runTime }}</span>
                </template>
              </ListItemMeta>
              {{ item.exception ?? item.message }}
            </ListItem>
          </template>
        </List>
      </Card>
    </Skeleton>
  </PageWrapper>
</template>

<script lang="ts" setup>
  import { onMounted, ref } from 'vue';
  import { Card, List, Skeleton } from 'ant-design-vue';
  import { ArrowLeftOutlined } from '@ant-design/icons-vue';
  import { Icon } from '/@/components/Icon';
  import { PageWrapper } from '/@/components/Page';
  import { Description, useDescription } from '/@/components/Description';
  import { useRoute, useRouter } from 'vue-router';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import { getById } from '/@/api/task-management/backgroundJobInfo';
  import { getList as getJobLogs } from '/@/api/task-management/backgroundJobLog';
  import { BackgroundJobInfo } from '/@/api/task-management/model/backgroundJobInfoModel';
  import { BackgroundJobLog } from '/@/api/task-management/model/backgroundJobLogModel';
  import { getDescriptionSchemas } from '../datas/DescriptionData';

  const ListItem = List.Item;
  const ListItemMeta = List.Item.Meta;

  const { L } = useLocalization('TaskManagement');
  const { back } = useRouter();
  const route = useRoute();
  const maxLogCount = ref(0);
  const fetchLogCount = ref(10);
  const fetchingLog = ref(false);
  const logListElRef = ref<any>();
  const jobInfo = ref<BackgroundJobInfo>();
  const jobLogs = ref<BackgroundJobLog[]>([]);
  
  const [registerDescription] = useDescription({
    bordered: true,
    column: 3,
    schema: getDescriptionSchemas(),
  });

  onMounted(fetchJob);

  function fetchJob() {
    getById(String(route.params.id)).then((res) => {
      jobInfo.value = res;
      jobLogs.value = [];
      fetchJobLogs(1);
    });
  }

  function fetchJobLogs(page: number) {
    const request = {
      skipCount: page,
      maxResultCount: fetchLogCount.value,
    };
    formatPagedRequest(request);
    fetchingLog.value = true;
    getJobLogs({
      jobId: String(route.params.id),
      sorting: '',
      skipCount: request.skipCount,
      maxResultCount: request.maxResultCount,
    }).then((res) => {
      jobLogs.value = res.items;
      maxLogCount.value = res.totalCount;
    }).finally(() => {
      fetchingLog.value = false;
    });
  }

  function handleSizeChange(current: number, size: number) {
    fetchLogCount.value = size;
    fetchJobLogs(current);
  }

  function handleBack() {
    back();
  }
</script>
