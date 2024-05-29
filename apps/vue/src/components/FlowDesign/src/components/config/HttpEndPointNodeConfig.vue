<template>
  <div class="end-point">
    <Form layout="vertical">
      <Tabs v-model:activeKey="state.activeKey">
        <TabPane key="common" tab="常用">
          <FormItem label="名称" extra="活动的名称。" name="name">
            <Input v-model:value="nodeProps.name" />
          </FormItem>
          <FormItem label="显示名称" extra="活动的显示名称。" name="displayName">
            <Input v-model:value="nodeProps.displayName" />
          </FormItem>
          <FormItem label="描述" extra="活动的描述。" name="description">
            <TextArea v-model:value="nodeProps.description" show-count :autoSize="{ minRows: 3 }" />
          </FormItem>
        </TabPane>
        <TabPane key="properties" tab="属性">
          <FormItem label="路径" extra="触发此活动的相对路径" name="path">
            <Input v-model:value="nodeProps.path" />
          </FormItem>
          <FormItem label="请求方法" extra="触发此活动的HTTP方法。" name="methods">
            <Select v-model:value="nodeProps.methods" mode="multiple">
              <SelectOption value="GET">GET</SelectOption>
              <SelectOption value="POST">POST</SelectOption>
              <SelectOption value="PUT">PUT</SelectOption>
              <SelectOption value="PATCH">PATCH</SelectOption>
              <SelectOption value="DELETE">DELETE</SelectOption>
              <SelectOption value="OPTIONS">OPTIONS</SelectOption>
              <SelectOption value="HEAD">HEAD</SelectOption>
            </Select>
          </FormItem>
          <FormItem
            label="读取内容"
            extra="指示是否应将HTTP请求内容体作为HTTP请求模型的一部分读取和存储。存储的格式取决于内容类型头。"
            name="readContent"
            class="user-type"
          >
            <Checkbox v-model:checked="nodeProps.readContent" />
          </FormItem>
        </TabPane>
        <TabPane key="advanced" tab="高级">
          <FormItem label="数据类型" extra="表单数据类型" name="targetType">
            <Input v-model:value="nodeProps.targetType" />
          </FormItem>
          <FormItem label="架构" extra="Json格式数据架构" name="schema">
            <CodeEditor v-model:value="nodeProps.schema" />
          </FormItem>
        </TabPane>
        <TabPane key="security" tab="安全">
          <FormItem label="身份认证" extra="选中以只允许经过身份验证的请求" name="authorize">
            <Checkbox v-model:checked="nodeProps.authorize" />
          </FormItem>
          <FormItem
            label="策略"
            extra="提供一个要评估的策略。如果策略失败，请求将被禁止。"
            name="policy"
          >
            <Input v-model:value="nodeProps.policy" />
          </FormItem>
        </TabPane>
      </Tabs>
    </Form>
  </div>
</template>

<script setup lang="ts">
  import { computed, reactive } from 'vue';
  import { Checkbox, Form, Input, Select, Tabs } from 'ant-design-vue';
  import { CodeEditor } from '/@/components/CodeEditor';
  import { useFlowStoreWithOut } from '/@/store/modules/flow';

  const FormItem = Form.Item;
  const SelectOption = Select.Option;
  const TabPane = Tabs.TabPane;
  const TextArea = Input.TextArea;

  defineProps({
    config: {
      type: Object,
      default: () => {
        return {};
      },
    },
  });
  const flowStore = useFlowStoreWithOut();
  const nodeProps = computed(() => {
    return flowStore.selectedNode.props;
  });
  const state = reactive({
    activeKey: 'common',
  });
</script>

<style scoped></style>
