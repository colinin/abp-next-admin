<script setup lang="ts">
import type { DefaultOptionType } from 'ant-design-vue/lib/select';

import { computed, shallowReactive, watch } from 'vue';

import { $t } from '@vben/locales';

import {
  isNullOrWhiteSpace,
  type LocalizableStringInfo,
  useAbpStore,
  useLocalizationSerializer,
} from '@abp/core';
import { Form, Input, Select } from 'ant-design-vue';

const props = defineProps<{
  allowClear?: boolean;
  disabled?: boolean;
  value?: string;
}>();
const emits = defineEmits(['update:value', 'change']);
const ItemReset = Form.ItemRest;
const InputGroup = Input.Group;
interface State {
  displayName?: string;
  displayNames: DefaultOptionType[];
  resourceName?: string;
}
const abpStore = useAbpStore();
const formItemContext = Form.useInjectFormItemContext();
const { deserialize, serialize } = useLocalizationSerializer();
const state = shallowReactive<State>({
  displayNames: [] as any[],
});
const getIsFixed = computed(() => {
  return state.resourceName === 'Fixed';
});
const getResources = computed(() => {
  if (!abpStore.localization) {
    return [];
  }
  const sources = Object.keys(abpStore.localization.resources).map((key) => {
    return {
      label: key,
      value: key,
    };
  });
  return [
    {
      label: $t('component.localizable_input.resources.fiexed.group'),
      options: [
        {
          label: $t('component.localizable_input.resources.fiexed.label'),
          value: 'Fixed',
        },
      ],
      value: 'F',
    },
    {
      label: $t('component.localizable_input.resources.localization.group'),
      options: sources,
      value: 'R',
    },
  ];
});
watch(
  () => props.value,
  (value) => {
    const info = deserialize(value);
    if (state.resourceName !== info.resourceName) {
      state.resourceName = isNullOrWhiteSpace(info.resourceName)
        ? undefined
        : info.resourceName;
      handleResourceChange(state.resourceName, false);
    }
    if (state.displayName !== info.name) {
      state.displayName = isNullOrWhiteSpace(info.name) ? undefined : info.name;
    }
  },
  {
    immediate: true,
  },
);

function handleResourceChange(value?: string, triggerChanged?: boolean) {
  state.displayNames = [];
  const resources = abpStore.localization!.resources;
  if (value && resources[value]) {
    state.displayNames = Object.keys(resources[value].texts).map((key) => {
      const labelText = resources[value]?.texts[key];
      return {
        label: labelText ?? key,
        value: key,
      };
    });
  }
  state.displayName = undefined;
  if (triggerChanged === true) {
    triggerDisplayNameChange(state.displayName);
  }
}

function handleDisplayNameChange(value?: string) {
  triggerDisplayNameChange(value);
}

function triggerDisplayNameChange(value?: string) {
  if (!value) {
    return;
  }
  let updateValue = '';
  if (getIsFixed.value) {
    updateValue = `F:${value}`;
  } else if (!isNullOrWhiteSpace(state.resourceName)) {
    const info: LocalizableStringInfo = {
      name: value,
      resourceName: state.resourceName ?? '',
    };
    updateValue = serialize(info);
  }
  emits('change', updateValue);
  emits('update:value', updateValue);
  formItemContext.onFieldChange();
}
</script>

<template>
  <div class="w-full">
    <ItemReset>
      <InputGroup>
        <div class="flex flex-row gap-4">
          <div class="basis-2/5">
            <Select
              v-model:value="state.resourceName"
              :allow-clear="props.allowClear"
              :disabled="props.disabled"
              :options="getResources"
              :placeholder="$t('component.localizable_input.placeholder')"
              class="w-full"
              @change="(value) => handleResourceChange(value?.toString(), true)"
            />
          </div>
          <div class="basis-3/5">
            <Input
              v-if="getIsFixed"
              :allow-clear="props.allowClear"
              :disabled="props.disabled"
              :placeholder="
                $t('component.localizable_input.resources.fiexed.placeholder')
              "
              :value="state.displayName"
              autocomplete="off"
              class="w-full"
              @change="
                (e) => handleDisplayNameChange(e.target.value?.toString())
              "
            />
            <Select
              v-else
              :allow-clear="props.allowClear"
              :disabled="props.disabled"
              :options="state.displayNames"
              :placeholder="
                $t(
                  'component.localizable_input.resources.localization.placeholder',
                )
              "
              :value="state.displayName"
              class="w-full"
              @change="(e) => handleDisplayNameChange(e?.toString())"
            />
          </div>
        </div>
      </InputGroup>
    </ItemReset>
  </div>
</template>
