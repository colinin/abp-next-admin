<template>
  <BasicModal @register="registerModal" :width="800" :height="400" :title="L('AuditLog')">
    <Form
      ref="formElRef"
      :colon="false"
      :labelCol="{ span: 6 }"
      :wrapperCol="{ span: 18 }"
      layout="horizontal"
      :model="modelRef"
    >
      <Tabs
        v-model:activeKey="activeKey"
        :style="tabsStyle.style"
        :tabBarStyle="tabsStyle.tabBarStyle"
      >
        <TabPane key="basic" :tab="L('Operation')">
          <FormItem labelAlign="left" :label="L('HttpStatusCode')">
            <Tag :color="httpStatusCodeColor(modelRef.httpStatusCode)">
              {{ modelRef.httpStatusCode }}
            </Tag>
          </FormItem>
          <FormItem labelAlign="left" :label="L('HttpMethod')">
            <Tag :color="httpMethodColor(modelRef.httpMethod)">
              {{ modelRef.httpMethod }}
            </Tag>
          </FormItem>
          <FormItem labelAlign="left" :label="L('RequestUrl')">
            <span>{{ modelRef.url }}</span>
          </FormItem>
          <FormItem labelAlign="left" :label="L('ClientIpAddress')">
            <span>{{ modelRef.clientIpAddress }}</span>
          </FormItem>
          <FormItem labelAlign="left" :label="L('ClientId')">
            <span>{{ modelRef.clientId }}</span>
          </FormItem>
          <FormItem labelAlign="left" :label="L('ClientName')">
            <span>{{ modelRef.clientName }}</span>
          </FormItem>
          <FormItem labelAlign="left" :label="L('UserName')">
            <span>{{ modelRef.userName }}</span>
          </FormItem>
          <FormItem labelAlign="left" :label="L('ExecutionTime')">
            <span>{{ formatDateVal(modelRef.executionTime) }}</span>
          </FormItem>
          <FormItem labelAlign="left" :label="L('ExecutionDuration')">
            <span>{{ modelRef.executionDuration }}</span>
          </FormItem>
          <FormItem labelAlign="left" :label="L('BrowserInfo')">
            <span>{{ modelRef.browserInfo }}</span>
          </FormItem>
          <FormItem labelAlign="left" :label="L('ApplicationName')">
            <span>{{ modelRef.applicationName }}</span>
          </FormItem>
          <FormItem labelAlign="left" :label="L('CorrelationId')">
            <span>{{ modelRef.correlationId }}</span>
          </FormItem>
          <FormItem labelAlign="left" :label="L('Comments')">
            <span>{{ modelRef.comments }}</span>
          </FormItem>
          <FormItem labelAlign="left" :label="L('Additional')">
            <span>{{ modelRef.extraProperties }}</span>
          </FormItem>
          <FormItem labelAlign="left" :label="L('Exception')">
            <span>{{ modelRef.exceptions }}</span>
          </FormItem>
        </TabPane>
        <TabPane
          v-if="modelRef.actions && modelRef.actions.length > 0"
          key="opera"
          :tab="`${L('InvokeMethod')}(${modelRef.actions?.length})`"
        >
          <Collapse>
            <CollapsePanel
              v-for="action in modelRef.actions"
              :key="action.id"
              :header="action.serviceName"
              :show-arrow="false"
            >
              <FormItem labelAlign="left" :label="L('MethodName')">
                <span>{{ action.methodName }}</span>
              </FormItem>
              <FormItem labelAlign="left" :label="L('ExecutionTime')">
                <span>{{ formatDateVal(action.executionTime) }}</span>
              </FormItem>
              <FormItem labelAlign="left" :label="L('ExecutionDuration')">
                <span>{{ action.executionDuration }}</span>
              </FormItem>
              <FormItem labelAlign="left" :label="L('Parameters')">
                <CodeEditor
                  :readonly="true"
                  :mode="MODE.JSON"
                  :value="formatJsonVal(action.parameters ?? '{}')"
                />
              </FormItem>
              <FormItem labelAlign="left" :label="L('Additional')">
                <span>{{ action.extraProperties }}</span>
              </FormItem>
            </CollapsePanel>
          </Collapse>
        </TabPane>
        <TabPane
          v-if="modelRef.entityChanges && modelRef.entityChanges.length > 0"
          key="changes"
          :tab="`${L('EntitiesChanged')}(${modelRef.entityChanges.length})`"
        >
          <Collapse>
            <CollapsePanel
              v-for="entity in modelRef.entityChanges"
              :key="entity.id"
              :header="entity.entityTypeFullName"
              :show-arrow="false"
            >
              <FormItem labelAlign="left" :label="L('ChangeType')">
                <Tag :color="entityChangeTypeColor(entity.changeType)">
                  {{ entityChangeType(entity.changeType) }}
                </Tag>
              </FormItem>
              <FormItem labelAlign="left" :label="L('EntityId')">
                <span>{{ entity.entityId }}</span>
              </FormItem>
              <FormItem labelAlign="left" :label="L('TenantId')">
                <span>{{ entity.entityTenantId }}</span>
              </FormItem>
              <FormItem labelAlign="left" :label="L('StartTime')">
                <span>{{ formatDateVal(entity.changeTime) }}</span>
              </FormItem>
              <FormItem labelAlign="left" :label="L('Additional')">
                <span>{{ entity.extraProperties }}</span>
              </FormItem>
              <BasicTable
                v-if="entity.propertyChanges && entity.propertyChanges.length > 0"
                row-key="id"
                :title="L('PropertyChanges')"
                :columns="columns"
                :data-source="entity.propertyChanges"
                :pagination="false"
                :striped="false"
                :use-search-form="false"
                :show-table-stting="false"
                :bordered="true"
                :show-index-column="false"
                :can-resize="false"
                :immediate="false"
              />
            </CollapsePanel>
          </Collapse>
        </TabPane>
      </Tabs>
    </Form>
  </BasicModal>
</template>

<script lang="ts" setup>
  import { computed, nextTick, ref, unref } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useTabsStyle } from '/@/hooks/component/useStyles';
  import { Collapse, Form, Tabs, Tag } from 'ant-design-vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicTable, BasicColumn } from '/@/components/Table';
  import { CodeEditor, MODE } from '/@/components/CodeEditor';
  import { useAuditLog } from '../hooks/useAuditLog';
  import { get } from '/@/api/auditing/audit-log';
  import { AuditLogDto } from '/@/api/auditing/audit-log/model';
  import { formatToDateTime } from '/@/utils/dateUtil';
  import { tryToJson } from '/@/utils/strings';

  const CollapsePanel = Collapse.Panel;
  const FormItem = Form.Item;
  const TabPane = Tabs.TabPane;

  const { L } = useLocalization('AbpAuditLogging');
  const formElRef = ref<any>();
  const activeKey = ref('basic');
  const tabsStyle = useTabsStyle();
  const modelRef = ref<AuditLogDto>({} as AuditLogDto);
  const [registerModal] = useModalInner((model) => {
    activeKey.value = 'basic';
    nextTick(() => {
      fetchAuditLog(model.id);
    });
  });
  const columns: BasicColumn[] = [
    {
      title: 'id',
      dataIndex: 'id',
      width: 1,
      ifShow: false,
    },
    {
      title: L('PropertyName'),
      dataIndex: 'propertyName',
      align: 'left',
      width: 120,
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
  const { entityChangeTypeColor, entityChangeType, httpMethodColor, httpStatusCodeColor } =
    useAuditLog();
  const formatJsonVal = computed(() => {
    return (jsonString: string) => tryToJson(jsonString);
  });
  const formatDateVal = computed(() => {
    return (dateVal) => formatToDateTime(dateVal, 'YYYY-MM-DD HH:mm:ss');
  });

  function fetchAuditLog(id?: string) {
    const formEl = unref(formElRef);
    formEl?.resetFields();
    if (id) {
      get(id).then((res) => {
        modelRef.value = res;
      });
    }
  }
</script>
