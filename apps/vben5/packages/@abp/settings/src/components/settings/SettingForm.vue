<script setup lang="ts">
import type {
  SettingDetail,
  SettingGroup,
  SettingsUpdateInput,
} from '../../types';

import { computed, onMounted, ref, toValue } from 'vue';

import { $t } from '@vben/locales';

import { formatToDate, useFeatures } from '@abp/core';
import {
  Button,
  Card,
  Checkbox,
  Collapse,
  DatePicker,
  Empty,
  Form,
  Input,
  InputNumber,
  InputPassword,
  message,
  Select,
  Tabs,
} from 'ant-design-vue';
import dayjs from 'dayjs';

import { SettingManagementEnable } from '../../constants/features';
import { ValueType } from '../../types';

defineOptions({
  name: 'SettingForm',
});
const props = defineProps<{
  getApi: () => Promise<SettingGroup[]>;
  submitApi: (input: SettingsUpdateInput) => Promise<void>;
}>();
const emits = defineEmits<{
  (event: 'change', data: SettingsUpdateInput): void;
}>();
const FormItem = Form.Item;
const TabPane = Tabs.TabPane;
const CollapsePanel = Collapse.Panel;

const defaultModel: SettingsUpdateInput = {
  settings: [],
};

const { isEnabled } = useFeatures();

const activeTab = ref(0);
const submiting = ref(false);
const settingGroups = ref<SettingGroup[]>([]);
const settingsUpdateInput = ref<SettingsUpdateInput>({ ...defaultModel });

const getExpandedCollapseKeys = computed(() => {
  return (group: SettingGroup) => {
    const keys = group.settings.map((group) => {
      return group.displayName;
    });
    return keys;
  };
});

async function onGet() {
  settingGroups.value = [];
  if (isEnabled(SettingManagementEnable)) {
    settingGroups.value = await props.getApi();
  }
}

async function onSubmit() {
  try {
    submiting.value = true;
    const input = toValue(settingsUpdateInput);
    await props.submitApi(input);
    emits('change', input);
    message.success($t('AbpSettingManagement.SavedSuccessfully'));
  } finally {
    submiting.value = false;
  }
}

function onCheckChange(setting: SettingDetail) {
  setting.value = setting.value?.toLowerCase() === 'true' ? 'false' : 'true';
  onValueChange(setting);
}

function onDateChange(e: any, setting: SettingDetail) {
  setting.value = dayjs.isDayjs(e) ? formatToDate(e) : '';
  onValueChange(setting);
}

function onValueChange(setting: SettingDetail) {
  if (setting.valueType === ValueType.NoSet) {
    return;
  }
  const index = settingsUpdateInput.value.settings.findIndex(
    (s) => s.name === setting.name,
  );
  if (index === -1) {
    settingsUpdateInput.value.settings.push({
      name: setting.name,
      value: String(setting.value),
    });
  } else {
    if (settingsUpdateInput.value.settings[index]) {
      settingsUpdateInput.value.settings[index].value = String(setting.value);
    }
  }
}

function onFilterOption(input: string, option: any) {
  if (option?.value || option?.name) {
    return (
      option?.name?.toLowerCase().includes(input.toLowerCase()) ||
      option?.value?.toLowerCase().includes(input.toLowerCase())
    );
  }
}

onMounted(onGet);
</script>

<template>
  <Card :title="$t('AbpSettingManagement.Settings')">
    <template #extra>
      <div class="flex flex-row gap-1">
        <slot name="toolbar"></slot>
        <Button
          v-if="settingsUpdateInput.settings.length > 0"
          :loading="submiting"
          class="w-[100px]"
          post-icon="ant-design:setting-outlined"
          type="primary"
          @click="onSubmit"
        >
          {{ $t('AbpUi.Submit') }}
        </Button>
      </div>
    </template>
    <Form
      v-if="isEnabled(SettingManagementEnable)"
      :label-col="{ span: 6 }"
      :wrapper-col="{ span: 18 }"
    >
      <Tabs tab-position="left" type="card" v-model="activeTab">
        <TabPane
          v-for="(group, index) in settingGroups"
          :key="index"
          :tab="group.displayName"
        >
          <Collapse :default-active-key="getExpandedCollapseKeys(group)">
            <CollapsePanel
              v-for="setting in group.settings"
              :key="setting.displayName"
              :header="setting.displayName"
            >
              <template v-for="detail in setting.details" :key="detail.name">
                <slot
                  v-if="detail.slot"
                  :change="
                    detail.valueType === ValueType.Boolean
                      ? onCheckChange
                      : onValueChange
                  "
                  :detail="detail"
                  :name="detail.slot"
                ></slot>
                <FormItem
                  v-else
                  :extra="detail.description"
                  :label="detail.displayName"
                >
                  <template v-if="detail.valueType === ValueType.String">
                    <InputPassword
                      v-if="detail.isEncrypted"
                      v-model:value="detail.value"
                      :placeholder="detail.description"
                      @change="onValueChange(detail)"
                    />
                    <Input
                      v-else
                      v-model:value="detail.value"
                      :placeholder="detail.description"
                      type="text"
                      @change="onValueChange(detail)"
                    />
                  </template>
                  <InputNumber
                    v-else-if="
                      detail.valueType === ValueType.Number &&
                      !detail.isEncrypted
                    "
                    v-model:value="detail.value"
                    :placeholder="detail.description"
                    class="w-full"
                    @change="onValueChange(detail)"
                  />
                  <DatePicker
                    v-else-if="detail.valueType === ValueType.Date"
                    :placeholder="detail.description"
                    :value="
                      detail.value ? dayjs(detail.value, 'YYYY-MM-DD') : ''
                    "
                    style="width: 100%"
                    @change="onDateChange($event, detail)"
                  />
                  <Select
                    v-if="detail.valueType === ValueType.Option"
                    v-model:value="detail.value"
                    :filter-option="onFilterOption"
                    :field-names="{ label: 'name', value: 'value' }"
                    :options="detail.options"
                    show-search
                    @change="onValueChange(detail)"
                  />
                  <!-- <SelectOption
                      v-for="option in detail.options"
                      :key="option.value"
                      :disabled="option.value === detail.value"
                    >
                      {{ option.name }}
                    </SelectOption>
                  </Select> -->
                  <Checkbox
                    v-if="detail.valueType === ValueType.Boolean"
                    :checked="detail.value?.toLowerCase() === 'true'"
                    @change="onCheckChange(detail)"
                  >
                    {{ detail.displayName }}
                  </Checkbox>
                </FormItem>
              </template>
            </CollapsePanel>
          </Collapse>
        </TabPane>
      </Tabs>
    </Form>
    <Empty
      v-else
      :description="
        $t('AbpFeature.Volo_Feature:010001', {
          FeatureName: $t(
            'AbpSettingManagement.Feature:SettingManagementEnable',
          ),
        })
      "
    />
  </Card>
</template>

<style lang="scss" scoped>
:deep(.ant-tabs) {
  height: 75vh;

  .ant-tabs-nav {
    width: 14rem;
  }

  .ant-tabs-content-holder {
    overflow: hidden auto !important;
  }
}
</style>
