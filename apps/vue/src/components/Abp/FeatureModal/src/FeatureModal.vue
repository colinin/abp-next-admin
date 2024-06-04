<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :title="L('ManageFeatures')"
    :width="800"
    :min-height="400"
    :can-fullscreen="false"
    @ok="handleSubmit"
    @cancel="onGroupChange(0)"
  >
    <Form ref="formRel" :model="featureGroup">
      <Tabs
        :class="`${prefixCls}__tabs`"
        tabPosition="left"
        v-model:activeKey="featureGroupKey"
        @change="onGroupChange"
      >
        <TabPane v-for="(group, gi) in featureGroup.groups" :key="gi" :tab="group.displayName">
          <div v-for="(feature, fi) in group.features" :key="feature.name">
            <FormItem
              v-if="feature.valueType !== null"
              :label="feature.displayName"
              :name="['groups', gi, 'features', fi, 'value']"
              :rules="validator(feature.displayName, feature.valueType.validator)"
              :extra="feature.description"
            >
              <Checkbox
                v-if="
                  feature.valueType.name === 'ToggleStringValueType' &&
                  feature.valueType.validator.name === 'BOOLEAN'
                "
                v-model:checked="feature.value"
                >{{ feature.displayName }}</Checkbox
              >
              <div v-else-if="feature.valueType.name === 'FreeTextStringValueType'">
                <InputNumber
                  v-if="feature.valueType.validator.name === 'NUMERIC'"
                  style="width: 100%"
                  v-model:value="feature.value"
                />
                <BInput v-else v-model:value="feature.value" />
              </div>
              <Select
                v-else-if="feature.valueType.name === 'SelectionStringValueType'"
                :allow-clear="true"
                v-model:value="feature.value"
              >
                <Option
                  v-for="valueItem in (feature.valueType as SelectionStringValueType).itemSource.items"
                  :key="valueItem.value"
                  v-model:value="valueItem.value"
                  :label="Lr(valueItem.displayText.resourceName, valueItem.displayText.name)"
                />
              </Select>
            </FormItem>
          </div>
        </TabPane>
      </Tabs>
    </Form>
  </BasicModal>
</template>

<script lang="ts" setup>
  import { ref } from 'vue';
  import { useDesign } from '/@/hooks/web/useDesign';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { Checkbox, Form, InputNumber, Select, Tabs } from 'ant-design-vue';
  import { Input } from '/@/components/Input';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useFeatures } from '../hooks/useFeatures';

  const FormItem = Form.Item;
  const Option = Select.Option;
  const TabPane = Tabs.TabPane;
  const BInput = Input!;

  const { prefixCls } = useDesign('feature-modal');
  const { L, Lr } = useLocalization(['AbpFeatureManagement']);
  const formRel = ref(null);
  const providerName = ref('');
  const providerKey = ref(null);
  const [registerModal, modalMethods] = useModalInner((data) => {
    providerName.value = data.providerName;
    providerKey.value = data.providerKey;
  });
  const { featureGroup, featureGroupKey, validator, handleSubmit, onGroupChange } = useFeatures({
    providerName,
    providerKey,
    formRel,
    modalMethods,
  });
</script>

<style lang="less" scoped>
  @prefix-cls: ~'@{namespace}-feature-modal';

  .@{prefix-cls} {
    &__tabs {
      height: 500px;

      ::v-deep(.ant-tabs-content-holder) {
        overflow-y: auto !important;
        overflow-x: hidden !important;
      }
    }
  }
</style>
