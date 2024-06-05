<template>
    <BasicDrawer
      v-bind="$attrs"
      @register="registerDrawer"
      width="50%"
      :title="L('EntitiesChanged')"
      :showFooter="false"
    >
      <Skeleton :loading="state.entityChangeLoading">
        <Empty v-if="state.entityChanges.length === 0" />
        <Timeline v-else mode="left">
          <TimelineItem v-for="(entityChange) in state.entityChanges" :color="changeTypeColorMap[entityChange.entityChange.changeType]">
            <Alert :type="changeTypeTypeMap[entityChange.entityChange.changeType]" :message="entityChange.entityChange.changeTime" />
            <div class="entity-change-wrap">
              <Card :title="L('Operation')">
                <template #extra>
                  <slot name="toolbar" :entityChange="entityChange"></slot>
                </template>
                <Descriptions bordered :labelStyle="{ width: 150 }">
                  <DescriptionsItem :label="L('UserName')" :span="3">{{ entityChange.userName }}</DescriptionsItem>
                  <DescriptionsItem :label="L('ChangeTime')" :span="3">{{ entityChange.entityChange.changeTime }}</DescriptionsItem>
                  <DescriptionsItem :label="L('ChangeType')" :span="3">
                    <Tag :color="changeTypeColorMap[entityChange.entityChange.changeType]">
                      {{ changeTypeMessageMap[entityChange.entityChange.changeType] }}
                    </Tag>
                  </DescriptionsItem>
                  <DescriptionsItem :label="L('EntityId')" :span="3">{{ entityChange.entityChange.entityId }}</DescriptionsItem>
                  <DescriptionsItem :label="L('EntityTypeFullName')" :span="3">{{ entityChange.entityChange.entityTypeFullName }}</DescriptionsItem>
                </Descriptions>
              </Card>
              <Card
                v-if="entityChange.entityChange.propertyChanges && entityChange.entityChange.propertyChanges.length > 0"
                :title="L('PropertyChanges')"
              >
                <BasicTable
                  row-key="id"
                  :columns="propertiesColumns"
                  :data-source="entityChange.entityChange.propertyChanges"
                  :pagination="false"
                  :striped="false"
                  :use-search-form="false"
                  :show-table-stting="false"
                  :bordered="true"
                  :show-index-column="false"
                  :can-resize="false"
                  :immediate="false"
                >
                  <template #bodyCell="{ column, record }">
                    <template v-if="column.key === 'propertyName'">
                      <span>{{ `${L('DisplayName:' + record.propertyName)}` }}</span>
                      <span>{{ `(${record.propertyName})` }}</span>
                    </template>
                  </template>
                </BasicTable>
              </Card>
            </div>
          </TimelineItem>
        </Timeline>
      </Skeleton>
    </BasicDrawer>
  </template>
  
  <script lang="ts" setup>
    import { reactive } from 'vue';
    import {
      Alert,
      Card,
      Descriptions,
      DescriptionsItem,
      Empty,
      Skeleton,
      Tag,
      Timeline,
      TimelineItem
    } from 'ant-design-vue';
    import { BasicDrawer, useDrawerInner } from '/@/components/Drawer';
    import { BasicTable, BasicColumn } from '/@/components/Table';
    import { useLocalization } from '/@/hooks/abp/useLocalization';
    import { ChangeType, EntityChangeWithUsernameDto } from '/@/api/auditing/entity-changes/model';
    import { GetWithUsernameAsyncByInput } from '/@/api/auditing/entity-changes';
  
    const props = defineProps({
      entityTypeFullName: {
        type: String,
        required: true,
      },
      entityId: {
        type: [String, Number],
      },
    });
    const { L } = useLocalization(['AbpAuditLogging']);
    const [registerDrawer] = useDrawerInner(refreshChanges);
    const state = reactive({
      entityChangeLoading: false,
      entityChanges: [] as EntityChangeWithUsernameDto[],
    });
    const changeTypeColorMap: {[key: number]: string} = {
      [ChangeType.Created]: '#87d068',
      [ChangeType.Updated]: '#108ee9',
      [ChangeType.Deleted]: 'red',
    };
    const changeTypeTypeMap: {[key: number]: 'success' | 'info' | 'error'} = {
      [ChangeType.Created]: 'success',
      [ChangeType.Updated]: 'info',
      [ChangeType.Deleted]: 'error',
    };
    const changeTypeMessageMap: {[key: number]: string} = {
      [ChangeType.Created]: L('Created'),
      [ChangeType.Updated]: L('Updated'),
      [ChangeType.Deleted]: L('Deleted'),
    };
    const propertiesColumns: BasicColumn[] = [
      {
        title: L('PropertyName'),
        dataIndex: 'propertyName',
        align: 'left',
        width: 280,
        sorter: true,
      },
      {
        title: L('NewValue'),
        dataIndex: 'newValue',
        align: 'left',
        width: 200,
        sorter: true,
      },
      {
        title: L('OriginalValue'),
        dataIndex: 'originalValue',
        align: 'left',
        width: 200,
        sorter: true,
      },
      {
        title: L('PropertyTypeFullName'),
        dataIndex: 'propertyTypeFullName',
        align: 'left',
        width: 300,
        sorter: true,
      },
    ];
  
    function refreshChanges() {
      if (!props.entityId) {
        return;
      }
      state.entityChangeLoading = true;
      GetWithUsernameAsyncByInput({
        entityId: String(props.entityId),
        entityTypeFullName: props.entityTypeFullName,
      }).then((res) => {
        state.entityChanges = res.items;
      }).finally(() => {
        state.entityChangeLoading = false;
      });
    }
  
    defineExpose({
      refreshChanges,
    });
  </script>
  
  <style lang="less" scoped>
    .entity-change-wrap {
      background: #ececec;
      padding: 10px
    }
  </style>