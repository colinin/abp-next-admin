<script setup lang="ts">
import type {
  FeatureDefinitionDto,
  FeatureGroupDefinitionDto,
} from '../../types';

import { computed, onMounted, ref } from 'vue';

import {
  listToTree,
  useLocalization,
  useLocalizationSerializer,
} from '@abp/core';
import { Checkbox, FormItemRest, TreeSelect } from 'ant-design-vue';

import { useFeatureDefinitionsApi } from '../../api/useFeatureDefinitionsApi';
import { useFeatureGroupDefinitionsApi } from '../../api/useFeatureGroupDefinitionsApi';

interface FeatureTreeData {
  checkable?: boolean;
  children: FeatureTreeData[];
  displayName: string;
  groupName: string;
  name: string;
}

const emits = defineEmits(['blur', 'change']);

const { getListApi: getFeaturesApi } = useFeatureDefinitionsApi();
const { getListApi: getFeatureGroupsApi } = useFeatureGroupDefinitionsApi();
const { deserialize } = useLocalizationSerializer();
const { Lr } = useLocalization();

const features = ref<FeatureDefinitionDto[]>([]);
const featureGroups = ref<FeatureGroupDefinitionDto[]>([]);
const modelValue = defineModel<{
  featureNames: string[];
  requiresAll: boolean;
}>({
  default: {
    featureNames: [],
    requiresAll: false,
  },
});

const getFeatureOptions = computed(() => {
  const featureOptions: FeatureTreeData[] = [];
  featureGroups.value.forEach((group) => {
    featureOptions.push({
      checkable: false,
      displayName: group.displayName,
      groupName: group.name,
      name: group.name,
      children: listToTree(
        features.value.filter((x) => x.groupName === group.name),
        {
          id: 'name',
          pid: 'parentName',
        },
      ),
    });
  });
  return featureOptions;
});
const getRequiredFeatures = computed(() => {
  const featureNames = modelValue.value?.featureNames ?? [];
  const requiredFeatures = features.value
    .filter((feature) => featureNames.includes(feature.name))
    .map((feature) => {
      return {
        label: feature.displayName,
        value: feature.name,
      };
    });
  return requiredFeatures;
});

function onChange() {
  emits('change', modelValue.value);
}

function onFeaturesChange(value: any[]) {
  modelValue.value.featureNames = value.map((item) => item.value);
  emits('change', modelValue.value);
}

async function onInit() {
  const [featureGroupRes, featuresRes] = await Promise.all([
    getFeatureGroupsApi(),
    getFeaturesApi(),
  ]);
  featureGroups.value = featureGroupRes.items.map((item) => {
    const displayName = deserialize(item.displayName);
    return {
      ...item,
      displayName: Lr(displayName.resourceName, displayName.name),
    };
  });
  features.value = featuresRes.items.map((item) => {
    const displayName = deserialize(item.displayName);
    return {
      ...item,
      displayName: Lr(displayName.resourceName, displayName.name),
    };
  });
}

onMounted(onInit);
</script>

<template>
  <div class="flex w-full flex-col gap-5">
    <FormItemRest>
      <Checkbox
        v-model:checked="modelValue.requiresAll"
        @blur="emits('blur')"
        @change="onChange"
      >
        {{ $t('component.simple_state_checking.requireFeatures.requiresAll') }}
      </Checkbox>
      <TreeSelect
        :tree-data="getFeatureOptions"
        allow-clear
        tree-checkable
        tree-check-strictly
        :field-names="{ label: 'displayName', value: 'name' }"
        :value="getRequiredFeatures"
        @blur="emits('blur')"
        @change="onFeaturesChange"
      />
    </FormItemRest>
  </div>
</template>

<style scoped></style>
