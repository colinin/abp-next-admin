<template>
  <BasicModal
    @register="registerModal"
    :title="L('WebhookDefinitions')"
    :can-fullscreen="false"
    :width="800"
    :height="500"
    :close-func="handleBeforeClose"
    @ok="handleSubmit"
  >
    <Form
      ref="formRef"
      :model="state.entity"
      :rules="state.entityRules"
      :label-col="{ span: 6 }"
      :wrapper-col="{ span: 18 }"
    >
      <Tabs v-model:active-key="state.activeTab">
        <TabPane key="basic" :tab="L('BasicInfo')">
          <FormItem name="isEnabled" :label="L('DisplayName:IsEnabled')">
            <Checkbox :disabled="!state.allowedChange" v-model:checked="state.entity.isEnabled"
              >{{ L('DisplayName:IsEnabled') }}
            </Checkbox>
          </FormItem>
          <FormItem name="groupName" :label="L('DisplayName:GroupName')">
            <Select
              :disabled="!state.allowedChange"
              :allow-clear="true"
              v-model:value="state.entity.groupName"
              :options="getGroupOptions"
            />
          </FormItem>
          <FormItem name="name" :label="L('DisplayName:Name')">
            <Input
              :disabled="state.entityEditFlag && !state.allowedChange"
              :allow-clear="true"
              v-model:value="state.entity.name"
            />
          </FormItem>
          <FormItem name="displayName" :label="L('DisplayName:DisplayName')">
            <LocalizableInput
              :disabled="!state.allowedChange"
              :allow-clear="true"
              v-model:value="state.entity.displayName"
            />
          </FormItem>
          <FormItem name="description" :label="L('DisplayName:Description')">
            <LocalizableInput
              :disabled="!state.allowedChange"
              :allow-clear="true"
              v-model:value="state.entity.description"
            />
          </FormItem>
          <FormItem name="requiredFeatures" :label="L('DisplayName:RequiredFeatures')">
            <TreeSelect
              :disabled="!state.allowedChange"
              :allow-clear="true"
              :tree-checkable="true"
              :tree-check-strictly="true"
              :tree-data="state.availableFeatures"
              :value="getRequiredFeatures"
              :field-names="{
                label: 'displayName',
                value: 'name',
                children: 'children',
              }"
              @change="handleFeatureChange"
            />
          </FormItem>
        </TabPane>
        <TabPane key="propertites" :tab="L('Properties')">
          <FormItem
            name="extraProperties"
            label=""
            :label-col="{ span: 0 }"
            :wrapper-col="{ span: 24 }"
          >
            <ExtraPropertyDictionary
              :disabled="!state.allowedChange"
              :allow-delete="true"
              :allow-edit="true"
              v-model:value="state.entity.extraProperties"
            />
          </FormItem>
        </TabPane>
      </Tabs>
    </Form>
  </BasicModal>
</template>

<script setup lang="ts">
  import type { Rule } from 'ant-design-vue/lib/form';
  import { cloneDeep } from 'lodash-es';
  import { computed, ref, reactive, unref, nextTick, watch, onMounted } from 'vue';
  import { Checkbox, Form, Input, Select, Tabs, TreeSelect } from 'ant-design-vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { LocalizableInput, ExtraPropertyDictionary } from '/@/components/Abp';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { ValidationEnum, useValidation } from '/@/hooks/abp/useValidation';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import { getList as getAvailableFeatures } from '/@/api/feature-management/definitions/features';
  import { FeatureDefinitionDto } from '/@/api/feature-management/definitions/features/model';
  import {
    GetAsyncByName,
    CreateAsyncByInput,
    UpdateAsyncByNameAndInput,
  } from '/@/api/webhooks/definitions/webhooks';
  import {
    WebhookDefinitionDto,
    WebhookDefinitionUpdateDto,
    WebhookDefinitionCreateDto,
  } from '/@/api/webhooks/definitions/webhooks/model';
  import { WebhookGroupDefinitionDto } from '/@/api/webhooks/definitions/groups/model';
  import { GetListAsyncByInput as getGroupDefinitions } from '/@/api/webhooks/definitions/groups';
  import { listToTree } from '/@/utils/helper/treeHelper';
  import { groupBy } from '/@/utils/array';
  import { isNullOrUnDef } from '/@/utils/is';

  const FormItem = Form.Item;
  const TabPane = Tabs.TabPane;
  interface FeatureTreeData {
    name: string;
    displayName: string;
    children: FeatureTreeData[];
  }
  interface State {
    activeTab: string;
    allowedChange: boolean;
    entity: Recordable;
    entityRules?: Dictionary<string, Rule>;
    entityChanged: boolean;
    entityEditFlag: boolean;
    defaultGroup?: string;
    features: FeatureDefinitionDto[];
    availableFeatures: FeatureTreeData[];
    availableGroups: WebhookGroupDefinitionDto[];
  }

  const emits = defineEmits(['register', 'change']);

  const { ruleCreator } = useValidation();
  const { deserialize, validate } = useLocalizationSerializer();
  const { createConfirm, createMessage } = useMessage();
  const { L, Lr } = useLocalization(['WebhooksManagement', 'AbpUi']);

  const formRef = ref<any>();
  const state = reactive<State>({
    activeTab: 'basic',
    entity: {
      isEnabled: true,
    } as WebhookDefinitionDto,
    allowedChange: false,
    entityChanged: false,
    entityEditFlag: false,
    availableFeatures: [],
    availableGroups: [],
    features: [],
    entityRules: {
      groupName: ruleCreator.fieldRequired({
        name: 'GroupName',
        prefix: 'DisplayName',
        resourceName: 'WebhooksManagement',
        trigger: 'change',
      }),
      name: ruleCreator.fieldRequired({
        name: 'Name',
        prefix: 'DisplayName',
        resourceName: 'WebhooksManagement',
        trigger: 'blur',
      }),
      displayName: ruleCreator.defineValidator({
        required: true,
        trigger: 'blur',
        validator(_rule, value) {
          if (!validate(value)) {
            return Promise.reject(L(ValidationEnum.FieldRequired, [L('DisplayName:DisplayName')]));
          }
          return Promise.resolve();
        },
      }),
      description: ruleCreator.defineValidator({
        trigger: 'blur',
        validator(_rule, value) {
          if (!validate(value, { required: false })) {
            return Promise.reject(L(ValidationEnum.FieldRequired, [L('DisplayName:Description')]));
          }
          return Promise.resolve();
        },
      }),
    },
  });
  const getRequiredFeatures = computed(() => {
    if (isNullOrUnDef(state.entity.requiredFeatures)) return [];
    return state.features
      .filter((feature) => state.entity.requiredFeatures.includes(feature.name))
      .map((feature) => {
        return {
          label: feature.displayName,
          value: feature.name,
        };
      });
  });
  const getGroupOptions = computed(() => {
    return state.availableGroups
      .filter((group) => !group.isStatic)
      .map((group) => {
        const info = deserialize(group.displayName);
        return {
          label: Lr(info.resourceName, info.name),
          value: group.name,
        };
      });
  });
  watch(
    () => state.entity,
    () => {
      state.entityChanged = true;
    },
    {
      deep: true,
    },
  );
  watch(() => state.defaultGroup, fetchGroups, {
    deep: true,
  });
  onMounted(fetchFeatures);

  const [registerModal, { closeModal, changeLoading, changeOkLoading }] = useModalInner(
    (record) => {
      state.defaultGroup = record.groupName;
      nextTick(() => {
        fetch(record.name);
      });
    },
  );

  function fetch(name?: string) {
    state.activeTab = 'basic';
    state.entityEditFlag = false;
    if (!name) {
      state.entity = {
        isEnabled: true,
        groupName: state.defaultGroup,
        requiredFeatures: [],
      };
      state.allowedChange = true;
      nextTick(() => (state.entityChanged = false));
      return;
    }
    changeLoading(true);
    changeOkLoading(true);
    GetAsyncByName(name)
      .then((record) => {
        state.entity = record;
        state.entityEditFlag = true;
        state.allowedChange = !record.isStatic;
      })
      .finally(() => {
        changeLoading(false);
        changeOkLoading(false);
        nextTick(() => (state.entityChanged = false));
      });
  }

  function fetchGroups() {
    getGroupDefinitions({
      filter: state.defaultGroup,
    }).then((res) => {
      state.availableGroups = res.items;
    });
  }

  function fetchFeatures() {
    getAvailableFeatures({}).then((res) => {
      state.features = res.items;
      formatDisplayName(state.features);
      const featureGroup = groupBy(cloneDeep(res.items), 'groupName');
      const featureGroupTree: FeatureTreeData[] = [];
      Object.keys(featureGroup).forEach((gk) => {
        const featureTree = listToTree(featureGroup[gk], {
          id: 'name',
          pid: 'parentName',
        });
        featureGroupTree.push(...featureTree);
      });
      state.availableFeatures = featureGroupTree;
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

  function handleFeatureChange(value: any[]) {
    state.entity.requiredFeatures = value.map((item) => item.value);
  }

  function handleBeforeClose(): Promise<boolean> {
    return new Promise((resolve) => {
      if (!state.entityChanged) {
        const form = unref(formRef);
        form?.resetFields();
        return resolve(true);
      }
      createConfirm({
        iconType: 'warning',
        title: L('AreYouSure'),
        content: L('AreYouSureYouWantToCancelEditingWarningMessage'),
        onOk: () => {
          const form = unref(formRef);
          form?.resetFields();
          resolve(true);
        },
        onCancel: () => {
          resolve(false);
        },
        afterClose: () => {
          state.allowedChange = false;
          state.defaultGroup = undefined;
        },
      });
    });
  }

  function handleSubmit() {
    if (!state.allowedChange) {
      closeModal();
      return;
    }
    const form = unref(formRef);
    form?.validate().then(() => {
      changeOkLoading(true);
      const api = state.entityEditFlag
        ? UpdateAsyncByNameAndInput(
            state.entity.name,
            cloneDeep(state.entity) as WebhookDefinitionUpdateDto,
          )
        : CreateAsyncByInput(cloneDeep(state.entity) as WebhookDefinitionCreateDto);
      api
        .then((res) => {
          createMessage.success(L('Successful'));
          emits('change', res);
          form.resetFields();
          closeModal();
        })
        .finally(() => {
          changeOkLoading(false);
        });
    });
  }
</script>

<style scoped></style>
