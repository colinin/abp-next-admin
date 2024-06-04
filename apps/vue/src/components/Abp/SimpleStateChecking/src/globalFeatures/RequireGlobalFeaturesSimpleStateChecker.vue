<template>
  <div>
    <FormItem
      :name="['stateChecker', 'requiresAll']"
      :label="t('component.simple_state_checking.requireFeatures.requiresAll')"
      :extra="t('component.simple_state_checking.requireFeatures.requiresAllDesc')"
    >
      <Checkbox :checked="state.stateChecker.requiresAll" @change="handleChangeRequiresAll">
        {{ t('component.simple_state_checking.requireFeatures.requiresAll') }}
      </Checkbox>
    </FormItem>
    <FormItem
      :name="['stateChecker', 'featureNames']"
      required
      :label="t('component.simple_state_checking.requireGlobalFeatures.featureNames')"
    >
      <TextArea
        allow-clear
        :value="getRequiredFeatures"
        @change="(e) => handleChangeFeatures(e.target.value)"
      />
    </FormItem>
  </div>
</template>

<script setup lang="ts">
  import type { CheckboxChangeEvent } from 'ant-design-vue/lib/checkbox/interface';
  import { computed, reactive, watchEffect } from 'vue';
  import { Checkbox, Form, Input } from 'ant-design-vue';
  import { useI18n } from '/@/hooks/web/useI18n';

  const FormItem = Form.Item;
  const TextArea = Input.TextArea;
  interface StateChecker {
    name: string;
    requiresAll: boolean;
    featureNames: string[];
  }
  interface State {
    stateChecker: StateChecker;
  }

  const emits = defineEmits(['change', 'update:value']);
  const props = defineProps({
    value: {
      type: Object as PropType<StateChecker>,
      required: true,
    },
  });

  const { t } = useI18n();
  const state = reactive<State>({
    stateChecker: {
      name: 'G',
      requiresAll: true,
      featureNames: [],
    },
  });
  const getRequiredFeatures = computed(() => {
    let features = state.stateChecker.featureNames.join(',');
    return features.length > 0 ? features.substring(0, features.length - 1) : features;
  });

  watchEffect(() => {
    state.stateChecker = props.value;
  });

  function handleChangeRequiresAll(e: CheckboxChangeEvent) {
    state.stateChecker.requiresAll = e.target.checked;
    emits('change', state.stateChecker);
    emits('update:value', state.stateChecker);
  }

  function handleChangeFeatures(value?: string) {
    if (!value) {
      state.stateChecker.featureNames = [];
      return;
    }
    state.stateChecker.featureNames = value.split(',');
    emits('change', state.stateChecker);
    emits('update:value', state.stateChecker);
  }
</script>

<style scoped></style>
