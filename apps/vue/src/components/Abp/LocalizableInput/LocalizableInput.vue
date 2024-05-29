<template>
  <div style="width: 100%">
    <ItemReset>
      <InputGroup>
        <Row :gutter="4">
          <Col :span="8">
            <Select
              :class="`${prefixCls}__resource`"
              :disabled="props.disabled"
              :allow-clear="props.allowClear"
              :placeholder="t('component.localizable_input.placeholder')"
              v-model:value="state.resourceName"
              :options="getResources"
              @change="(value) => handleResourceChange(value?.toString(), true)"
            />
          </Col>
          <Col :span="16">
            <Input
              v-if="getIsFixed"
              :class="`${prefixCls}__name`"
              :disabled="props.disabled"
              :allow-clear="props.allowClear"
              :placeholder="t('component.localizable_input.resources.fiexed.placeholder')"
              :value="state.displayName"
              @change="(e) => handleDisplayNameChange(e.target.value)"
            />
            <Select
              v-else
              :class="`${prefixCls}__name`"
              :disabled="props.disabled"
              :allow-clear="props.allowClear"
              :placeholder="t('component.localizable_input.resources.localization.placeholder')"
              :value="state.displayName"
              :options="state.displayNames"
              @change="handleDisplayNameChange"
            />
          </Col>
        </Row>
      </InputGroup>
    </ItemReset>
  </div>
</template>

<script setup lang="ts">
  import type { DefaultOptionType } from 'ant-design-vue/lib/select';
  import { computed, watch, shallowReactive } from 'vue';
  import { Form, Row, Col, Input, Select } from 'ant-design-vue';
  import { propTypes } from '/@/utils/propTypes';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { useDesign } from '/@/hooks/web/useDesign';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import { useAbpStoreWithOut } from '/@/store/modules/abp';
  import { isNullOrWhiteSpace } from '/@/utils/strings';

  const ItemReset = Form.ItemRest;
  const InputGroup = Input.Group;
  interface State {
    resourceName?: string;
    displayName?: string;
    displayNames: DefaultOptionType[];
  }

  const emits = defineEmits(['update:value', 'change']);
  const props = defineProps({
    value: propTypes.string,
    allowClear: propTypes.bool.def(false),
    disabled: propTypes.bool.def(false),
  });
  const { t } = useI18n();
  const abpStore = useAbpStoreWithOut();
  const { prefixCls } = useDesign('localizable-input');
  const formItemContext = Form.useInjectFormItemContext();
  const { values: resources } = abpStore.getApplication.localization;
  const { deserialize, serialize } = useLocalizationSerializer();
  const state = shallowReactive<State>({
    displayNames: [] as any[],
  });
  const getIsFixed = computed(() => {
    return state.resourceName === 'Fixed';
  });
  const getResources = computed(() => {
    const sources = Object.keys(resources).map((key) => {
      return {
        label: key,
        value: key,
      };
    });
    return [
      {
        label: t('component.localizable_input.resources.fiexed.group'),
        value: 'F',
        options: [
          {
            label: t('component.localizable_input.resources.fiexed.label'),
            value: 'Fixed',
          },
        ],
      },
      {
        label: t('component.localizable_input.resources.localization.group'),
        value: 'R',
        options: sources,
      },
    ];
  });
  watch(
    () => props.value,
    (value) => {
      const info = deserialize(value);
      if (state.resourceName !== info.resourceName) {
        state.resourceName = isNullOrWhiteSpace(info.resourceName) ? undefined : info.resourceName;
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
    if (value && resources[value]) {
      state.displayNames = Object.keys(resources[value]).map((key) => {
        return {
          label: resources[value][key] ? String(resources[value][key]) : key,
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
    const inputValue = value === undefined ? '' : value;
    let updateValue = '';
    if (getIsFixed.value) {
      updateValue = !isNullOrWhiteSpace(value) ? `F:${value}` : 'F:';
    } else if (!isNullOrWhiteSpace(state.resourceName)) {
      const info: LocalizableStringInfo = {
        resourceName: state.resourceName ?? '',
        name: inputValue,
      };
      updateValue = serialize(info);
    }
    emits('change', updateValue);
    emits('update:value', updateValue);
    formItemContext.onFieldChange();
  }
</script>

<style lang="less" scoped>
  @prefix-cls: ~'@{namespace}-localizable-input';

  .@{prefix-cls} {
    &__resource {
      width: 100% !important;
    }

    &__name {
      width: 100% !important;
    }
  }
</style>
