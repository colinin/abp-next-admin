<script setup lang="ts">
import type { FormInstance } from 'ant-design-vue/es/form/Form';

import type { FeatureGroupDto, UpdateFeaturesDto } from '../../types/features';

import { computed, ref, toValue, useTemplateRef } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { Form, message, Tabs } from 'ant-design-vue';

import { useFeaturesApi } from '../../api/useFeaturesApi';
import FeatureGroup from './FeatureGroup.vue';
import {
  buildFeatureTree,
  onFeatureValueChange,
  processAllFeatures,
  updateVisibility,
} from './useFeatureTree';

const TabPane = Tabs.TabPane;

interface ModalState {
  displayName?: string;
  providerKey?: string;
  providerName: string;
  readonly?: boolean;
}

interface FormModel {
  groups: FeatureGroupDto[];
}

const activeTabKey = ref('');
const modelState = ref<ModalState>();
const formModel = ref<FormModel>({ groups: [] });
const form = useTemplateRef<FormInstance>('form');

const getModalTitle = computed(() => {
  if (modelState.value?.displayName) {
    return `${$t('AbpFeatureManagement.Features')} - ${modelState.value.displayName}`;
  }
  return $t('AbpFeatureManagement.Features');
});

const { getApi, updateApi } = useFeaturesApi();

const [Modal, modalApi] = useVbenModal({
  centered: true,
  class: 'w-1/2',
  async onConfirm() {
    await form.value?.validate();
    await onSubmit();
  },
  async onOpenChange(isOpen) {
    if (isOpen) {
      formModel.value = { groups: [] };
      await onGet();
      if (formModel.value?.groups.length > 0) {
        activeTabKey.value = formModel.value.groups[0]?.name!;
      }
    }
  },
});

function mapFeatures(groups: FeatureGroupDto[]): FeatureGroupDto[] {
  groups.forEach((group) => {
    // 构建树形结构
    const treeRoots = buildFeatureTree(group.features);

    // 存储树形结构到 group 中供模板使用
    (group as any)._treeRoots = treeRoots;
    (group as any)._allFeatures = [...group.features];

    // 初始化可见性
    treeRoots.forEach((root) => updateVisibility(root, true));

    // 处理特性值类型转换
    processAllFeatures(treeRoots);
  });
  return groups;
}

function getFeatureInput(groups: FeatureGroupDto[]): UpdateFeaturesDto {
  const input: UpdateFeaturesDto = {
    features: [],
  };
  groups.forEach((g) => {
    const features = (g as any)._allFeatures || g.features;
    features.forEach((f: any) => {
      if (f.value !== null && f.value !== undefined && f.value !== '') {
        input.features.push({
          name: f.name,
          value: String(f.value),
        });
      }
    });
  });
  return input;
}

async function onGet() {
  try {
    modalApi.setState({ loading: true });
    const state = modalApi.getData<ModalState>();
    const { groups } = await getApi({
      providerKey: state.providerKey,
      providerName: state.providerName,
    });
    formModel.value = {
      groups: mapFeatures(groups),
    };
    modelState.value = state;
  } finally {
    modalApi.setState({ loading: false });
  }
}

function handleFeatureChange(feature: any, groupIndex: number) {
  const group = formModel.value.groups[groupIndex];
  const treeRoots = (group as any)._treeRoots;
  if (treeRoots) {
    onFeatureValueChange(feature, treeRoots);
    // 强制更新视图
    formModel.value = { ...formModel.value };
  }
}

async function onSubmit() {
  try {
    modalApi.setState({ submitting: true });
    const model = toValue(formModel);
    const state = modalApi.getData<ModalState>();
    const input = getFeatureInput(model.groups);
    await updateApi(
      {
        providerKey: state.providerKey,
        providerName: state.providerName,
      },
      input,
    );
    message.success($t('AbpUi.SavedSuccessfully'));
    await onGet();
  } finally {
    modalApi.setState({ submitting: false });
  }
}
</script>

<template>
  <Modal :title="getModalTitle">
    <Form :model="formModel" ref="form">
      <Tabs tab-position="left" type="card" v-model:active-key="activeTabKey">
        <TabPane
          v-for="(group, gi) in formModel.groups"
          :key="group.name"
          :tab="group.displayName"
        >
          <FeatureGroup
            :group="group"
            :group-index="gi"
            :base-indent-size="8"
            @change="handleFeatureChange"
          />
        </TabPane>
      </Tabs>
    </Form>
  </Modal>
</template>

<style lang="scss" scoped>
:deep(.ant-tabs) {
  height: 34rem;

  .ant-tabs-nav {
    width: 14rem;
  }

  .ant-tabs-content-holder {
    overflow: hidden auto !important;
  }
}

:deep(.ant-form-item) {
  margin-bottom: 16px;
}
</style>
