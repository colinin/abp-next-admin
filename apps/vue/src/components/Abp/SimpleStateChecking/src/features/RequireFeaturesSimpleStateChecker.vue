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
      :label="t('component.simple_state_checking.requireFeatures.featureNames')"
    >
      <TreeSelect
        allow-clear
        tree-checkable
        tree-check-strictly
        :tree-data="state.treeData"
        :field-names="{
          label: 'displayName',
          value: 'name',
          children: 'children',
        }"
        :value="getRequiredFeatures"
        @change="handleChangeFeatures"
      />
    </FormItem>
  </div>
</template>

<script setup lang="ts">
  import type { CheckboxChangeEvent } from 'ant-design-vue/lib/checkbox/interface';
  import { cloneDeep } from 'lodash-es';
  import { computed, reactive, onMounted, watchEffect } from 'vue';
  import { Checkbox, Form, TreeSelect } from 'ant-design-vue';
  import { getList } from '/@/api/feature-management/definitions/features';
  import { FeatureDefinitionDto } from '/@/api/feature-management/definitions/features/model';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { listToTree } from '/@/utils/helper/treeHelper';
  import { groupBy } from '/@/utils/array';
  import { valueTypeSerializer } from '../../../StringValueType/valueType';

  const FormItem = Form.Item;
  interface FeatureTreeData {
    name: string;
    groupName: string;
    displayName: string;
    children: FeatureTreeData[];
  }
  interface StateChecker {
    name: string;
    requiresAll: boolean;
    featureNames: string[];
  }
  interface State {
    features: FeatureDefinitionDto[];
    treeData: FeatureTreeData[];
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
  const { Lr } = useLocalization();
  const { deserialize } = useLocalizationSerializer();
  const state = reactive<State>({
    treeData: [],
    features: [],
    stateChecker: {
      name: 'F',
      requiresAll: true,
      featureNames: [],
    },
  });
  const getRequiredFeatures = computed(() => {
    return state.features
      .filter((feature) => state.stateChecker.featureNames.includes(feature.name))
      .map((feature) => {
        return {
          label: feature.displayName,
          value: feature.name,
        };
      });
  });

  watchEffect(() => {
    state.stateChecker = props.value;
  });
  onMounted(fetchFeatures);

  function fetchFeatures() {
    getList({}).then((res) => {
      state.features = res.items;
      formatDisplayName(state.features);
      const featureGroup = groupBy(cloneDeep(res.items), 'groupName');
      const featureTreeData: FeatureTreeData[] = [];
      Object.keys(featureGroup).forEach((gk) => {
        const featuresByGroup = featureGroup[gk].filter((feature) => {
          const valueType = valueTypeSerializer.deserialize(feature.valueType);
          return valueType.validator.name === 'BOOLEAN';
        });
        const featureTree = listToTree(featuresByGroup, {
          id: 'name',
          pid: 'parentName',
        });
        featureTreeData.push(...featureTree);
      });
      state.treeData = featureTreeData;
    });
  }

  function formatDisplayName(list: any[]) {
    if (list && Array.isArray(list)) {
      list.forEach((item) => {
        if (Reflect.has(item, 'displayName')) {
          const info = deserialize(item.displayName);
          item.displayName = Lr(info.resourceName, info.name);
        }
        if (Reflect.has(item, 'children')) {
          formatDisplayName(item.children);
        }
      });
    }
  }

  function handleChangeRequiresAll(e: CheckboxChangeEvent) {
    state.stateChecker.requiresAll = e.target.checked;
    emits('change', state.stateChecker);
    emits('update:value', state.stateChecker);
  }

  function handleChangeFeatures(value: any[]) {
    state.stateChecker.featureNames = value.map((val) => val.value);
    emits('change', state.stateChecker);
    emits('update:value', state.stateChecker);
  }
</script>

<style scoped></style>
