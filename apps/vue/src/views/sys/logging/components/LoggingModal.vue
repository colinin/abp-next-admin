<template>
  <BasicModal @register="registerModal" :width="800" :height="400" :title="L('Logging')">
    <Form :labelCol="{ span: 4 }" :wrapperCol="{ span: 20 }" layout="horizontal" :model="modelRef">
      <Tabs v-model:activeKey="activeKey">
        <TabPane key="basic" :tab="L('Operation')">
          <FormItem :label="L('TimeStamp')">
            <span>{{ formatDateVal(modelRef.timeStamp) }}</span>
          </FormItem>
          <FormItem :label="L('Level')">
            <Tag :color="LogLevelColor[modelRef.level]">
              {{ LogLevelLabel[modelRef.level] }}
            </Tag>
          </FormItem>
          <FormItem :label="L('Message')">
            <TextArea v-model:value="modelRef.message" readonly :rows="10" />
          </FormItem>
        </TabPane>
        <TabPane v-if="modelRef.fields" key="fields" :tab="L('Fields')">
          <FormItem :label="L('MachineName')">
            <span>{{ modelRef.fields.machineName }}</span>
          </FormItem>
          <FormItem :label="L('Environment')">
            <span>{{ modelRef.fields.environment }}</span>
          </FormItem>
          <FormItem :label="L('Application')">
            <span>{{ modelRef.fields.application }}</span>
          </FormItem>
          <FormItem :label="L('ProcessId')">
            <span>{{ modelRef.fields.processId }}</span>
          </FormItem>
          <FormItem :label="L('ThreadId')">
            <span>{{ modelRef.fields.threadId }}</span>
          </FormItem>
          <FormItem :label="L('Context')">
            <span>{{ modelRef.fields.context }}</span>
          </FormItem>
          <FormItem :label="L('ActionId')">
            <span>{{ modelRef.fields.actionId }}</span>
          </FormItem>
          <FormItem :label="L('ActionName')">
            <span>{{ modelRef.fields.actionName }}</span>
          </FormItem>
          <FormItem :label="L('RequestId')">
            <span>{{ modelRef.fields.requestId }}</span>
          </FormItem>
          <FormItem :label="L('RequestPath')">
            <span>{{ modelRef.fields.requestPath }}</span>
          </FormItem>
          <FormItem :label="L('ConnectionId')">
            <span>{{ modelRef.fields.connectionId }}</span>
          </FormItem>
          <FormItem :label="L('CorrelationId')">
            <span>{{ modelRef.fields.correlationId }}</span>
          </FormItem>
          <FormItem :label="L('ClientId')">
            <span>{{ modelRef.fields.clientId }}</span>
          </FormItem>
          <FormItem :label="L('UserId')">
            <span>{{ modelRef.fields.userId }}</span>
          </FormItem>
        </TabPane>
        <TabPane
          v-if="modelRef.exceptions && modelRef.exceptions.length > 0"
          key="exceptions"
          :tab="L('Exceptions')"
        >
          <Collapse>
            <CollapsePanel
              v-for="(exception, index) in modelRef.exceptions"
              :key="index"
              :header="exception.class"
              :show-arrow="false"
            >
              <FormItem :label="L('Class')">
                <span>{{ exception.class }}</span>
              </FormItem>
              <FormItem :label="L('Message')">
                <span>{{ exception.message }}</span>
              </FormItem>
              <FormItem :label="L('Source')">
                <span>{{ exception.source }}</span>
              </FormItem>
              <FormItem :label="L('StackTrace')">
                <TextArea v-model:value="exception.stackTrace" readonly :rows="10" />
              </FormItem>
              <FormItem :label="L('HResult')">
                <span>{{ exception.hResult }}</span>
              </FormItem>
              <FormItem :label="L('HelpURL')">
                <span>{{ exception.helpURL }}</span>
              </FormItem>
            </CollapsePanel>
          </Collapse>
        </TabPane>
      </Tabs>
    </Form>
  </BasicModal>
</template>

<script lang="ts">
  import { computed, defineComponent, ref } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { Collapse, Form, Tabs, Tag, Input } from 'ant-design-vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { get } from '/@/api/logging/logging';
  import { LogLevelColor, LogLevelLabel } from '../datas/typing';
  import { Log } from '/@/api/logging/model/loggingModel';
  import { formatToDateTime } from '/@/utils/dateUtil';
  export default defineComponent({
    name: 'LoggingModal',
    components: {
      BasicModal,
      Collapse,
      CollapsePanel: Collapse.Panel,
      Form,
      FormItem: Form.Item,
      Tag,
      Tabs,
      TabPane: Tabs.TabPane,
      TextArea: Input.TextArea,
    },
    setup() {
      const { L } = useLocalization('AbpAuditLogging');
      const activeKey = ref('basic');
      const modelRef = ref<Log>({} as Log);
      const [registerModal] = useModalInner((model) => {
        activeKey.value = 'basic';
        modelRef.value = {} as Log;
        get(model.fields.id).then((res) => {
          modelRef.value = res;
        });
      });
      const formatDateVal = computed(() => {
        return (dateVal) => formatToDateTime(dateVal, 'YYYY-MM-DD HH:mm:ss');
      });

      return {
        L,
        modelRef,
        activeKey,
        registerModal,
        formatDateVal,
        LogLevelColor,
        LogLevelLabel,
      };
    },
  });
</script>
