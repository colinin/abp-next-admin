<script setup lang="ts">
import type {
  SettingDetail,
  SettingGroup,
  SettingsUpdateInput,
} from '../../types';

import { computed, onMounted, ref, toValue } from 'vue';

import { $t } from '@vben/locales';

import { formatToDate } from '@abp/core';
import {
  Button,
  Card,
  Checkbox,
  Collapse,
  DatePicker,
  Form,
  Input,
  InputNumber,
  InputPassword,
  message,
  Select,
  Tabs,
} from 'ant-design-vue';
import dayjs from 'dayjs';

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
const SelectOption = Select.Option;

const defaultModel: SettingsUpdateInput = {
  settings: [],
};

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
  settingGroups.value = await props.getApi();
}

async function onSubmit() {
  try {
    submiting.value = true;
    const input = toValue(settingsUpdateInput);
    await props.submitApi(input);
    emits('change', input);
    message.success($t('AbpSettingManagement.SuccessfullySaved'));
  } finally {
    submiting.value = false;
  }
}

function onCheckChange(setting: SettingDetail) {
  setting.value = setting.value === 'true' ? 'false' : 'true';
  onValueChange(setting);
}

function onDateChange(e: any, setting: SettingDetail) {
  setting.value = dayjs.isDayjs(e) ? formatToDate(e) : '';
  onValueChange(setting);
}

function onValueChange(setting: SettingDetail) {
  const index = settingsUpdateInput.value.settings.findIndex(
    (s) => s.name === setting.name,
  );
  if (index === -1) {
    settingsUpdateInput.value.settings.push({
      name: setting.name,
      value: String(setting.value),
    });
  } else {
    settingsUpdateInput.value.settings[index]!.value = String(setting.value);
  }
}

onMounted(onGet);
</script>

<template>
  <Card :title="$t('AbpSettingManagement.Settings')">
    <template #extra>
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
    </template>
    <Form :label-col="{ span: 5 }" :wrapper-col="{ span: 15 }">
      <Tabs v-model="activeTab">
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
                      ? onCheckChange(detail)
                      : onValueChange(detail)
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
                    @change="onValueChange(detail)"
                  >
                    <SelectOption
                      v-for="option in detail.options"
                      :key="option.value"
                      :disabled="option.value === detail.value"
                    >
                      {{ option.name }}
                    </SelectOption>
                  </Select>
                  <Checkbox
                    v-if="detail.valueType === ValueType.Boolean"
                    :checked="detail.value === 'true'"
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
  </Card>
</template>

<style scoped></style>
