<template>
  <BasicModal
    @register="registerModal"
    :title="L('FeatureDefinitions')"
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
          <FormItem name="groupName" :label="L('DisplayName:GroupName')">
            <Select
              :disabled="!state.allowedChange"
              :allow-clear="true"
              v-model:value="state.entity.groupName"
              :options="getGroupOptions"
              @change="handleGroupChange"
            />
          </FormItem>
          <FormItem name="parentName" :label="L('DisplayName:ParentName')">
            <TreeSelect
              :disabled="!state.allowedChange"
              :allow-clear="true"
              :tree-data="state.availableFeatures"
              v-model:value="state.entity.parentName"
              :field-names="{
                label: 'displayName',
                value: 'name',
                children: 'children',
              }"
              @change="handleParentChange"
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
          <FormItem name="defaultValue" :label="L('DisplayName:DefaultValue')">
            <Select
              v-if="valueTypeNameRef === 'SelectionStringValueType'"
              :disabled="!state.allowedChange"
              :allow-clear="true"
              v-model:value="state.entity.defaultValue"
              :options="state.selectionDataSource"
            />
            <TextArea
              v-else-if="
                valueTypeNameRef === 'FreeTextStringValueType' &&
                (validatorNameRef === 'NULL' || validatorNameRef === 'STRING')
              "
              :disabled="state.entityEditFlag && !state.allowedChange"
              :allow-clear="true"
              :auto-size="{ minRows: 3 }"
              v-model:value="state.entity.defaultValue"
            />
            <InputNumber
              v-else-if="
                valueTypeNameRef === 'FreeTextStringValueType' && validatorNameRef === 'NUMERIC'
              "
              style="width: 100%"
              :disabled="state.entityEditFlag && !state.allowedChange"
              v-model:value="state.entity.defaultValue"
            />
            <Checkbox
              v-else-if="
                valueTypeNameRef === 'ToggleStringValueType' && validatorNameRef === 'BOOLEAN'
              "
              :disabled="state.entityEditFlag && !state.allowedChange"
              :checked="state.entity.defaultValue === 'true'"
              @change="(e) => (state.entity.defaultValue = String(e.target.checked).toLowerCase())"
            >
              {{ L('DisplayName:DefaultValue') }}
            </Checkbox>
          </FormItem>
          <FormItem
            name="IsVisibleToClients"
            :label="L('DisplayName:IsVisibleToClients')"
            :extra="L('Description:IsVisibleToClients')"
          >
            <Checkbox
              :disabled="!state.allowedChange"
              v-model:checked="state.entity.isVisibleToClients"
              >{{ L('DisplayName:IsVisibleToClients') }}
            </Checkbox>
          </FormItem>
          <FormItem
            name="IsAvailableToHost"
            :label="L('DisplayName:IsAvailableToHost')"
            :extra="L('Description:IsAvailableToHost')"
          >
            <Checkbox
              :disabled="!state.allowedChange"
              v-model:checked="state.entity.isAvailableToHost"
              >{{ L('DisplayName:IsAvailableToHost') }}
            </Checkbox>
          </FormItem>
        </TabPane>
        <TabPane key="valueType" :tab="L('ValueValidator')" force-render>
          <FormItem name="valueType" label="" :label-col="{ span: 0 }" :wrapper-col="{ span: 24 }">
            <StringValueTypeInput
              ref="valueTypeRef"
              :disabled="!state.allowedChange"
              :allow-delete="true"
              :allow-edit="true"
              v-model:value="state.entity.valueType"
              @change:value-type="handleValueTypeNameChange"
              @change:validator="handleValidatorNameChange"
              @change:selection="handleSelectionChange"
            />
          </FormItem>
        </TabPane>
        <TabPane key="propertites" :tab="L('Properties')" force-render>
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
  import { computed, ref, reactive, unref, nextTick, watch } from 'vue';
  import { Checkbox, Form, Input, InputNumber, Select, Tabs, TreeSelect } from 'ant-design-vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import {
    LocalizableInput,
    ExtraPropertyDictionary,
    StringValueTypeInput,
  } from '/@/components/Abp';
  import {
    FreeTextStringValueType,
    SelectionStringValueItem,
    valueTypeSerializer,
  } from '/@/components/Abp/StringValueType/valueType';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { ValidationEnum, useValidation } from '/@/hooks/abp/useValidation';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import {
    getByName,
    getList,
    create,
    update,
  } from '/@/api/feature-management/definitions/features';
  import {
    FeatureDefinitionUpdateDto,
    FeatureDefinitionCreateDto,
  } from '/@/api/feature-management/definitions/features/model';
  import { FeatureGroupDefinitionDto } from '/@/api/feature-management/definitions/groups/model';
  import { listToTree } from '/@/utils/helper/treeHelper';
  import { isBoolean, isNullOrUnDef } from '/@/utils/is';
  import { groupBy } from '/@/utils/array';

  const FormItem = Form.Item;
  const TabPane = Tabs.TabPane;
  const TextArea = Input.TextArea;
  interface FeatureSelectData {
    label: string;
    value: string;
  }
  interface FeatureTreeData {
    name: string;
    groupName: string;
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
    availableFeatures: FeatureTreeData[];
    availableGroups: FeatureGroupDefinitionDto[];
    selectionDataSource: FeatureSelectData[];
  }

  const emits = defineEmits(['register', 'change']);

  const { ruleCreator } = useValidation();
  const { deserialize, validate } = useLocalizationSerializer();
  const { createConfirm, createMessage } = useMessage();
  const { L, Lr } = useLocalization(['AbpFeatureManagement', 'AbpUi']);

  const formRef = ref<any>();
  const valueTypeRef = ref<any>();
  const validatorNameRef = ref<string>('NULL');
  const valueTypeNameRef = ref<string>('FreeTextStringValueType');
  const state = reactive<State>({
    activeTab: 'basic',
    entity: {},
    allowedChange: false,
    entityChanged: false,
    entityEditFlag: false,
    availableFeatures: [],
    availableGroups: [],
    selectionDataSource: [],
    entityRules: {
      groupName: ruleCreator.fieldRequired({
        name: 'GroupName',
        prefix: 'DisplayName',
        resourceName: 'WebhooksManagement',
        trigger: 'blur',
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
      defaultValue: ruleCreator.defineValidator({
        trigger: 'change',
        validator(_rule, value) {
          const valueType = unref(valueTypeRef);
          if (valueType) {
            return valueType.validate(value);
          }
          return Promise.resolve();
        },
      }),
    },
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

  const [registerModal, { closeModal, changeLoading, changeOkLoading }] = useModalInner((data) => {
    state.defaultGroup = data.groupName;
    state.availableGroups = data.groups;
    nextTick(() => {
      fetchFeatures(state.defaultGroup);
      fetch(data.record?.name);
    });
  });

  function fetch(name?: string) {
    state.activeTab = 'basic';
    state.entityEditFlag = false;
    if (!name) {
      state.entity = {
        isVisibleToClients: true,
        isAvailableToHost: true,
        groupName: state.defaultGroup,
        requiredFeatures: [],
        valueType: valueTypeSerializer.serialize(new FreeTextStringValueType()),
      };
      state.allowedChange = true;
      nextTick(() => (state.entityChanged = false));
      return;
    }
    changeLoading(true);
    changeOkLoading(true);
    getByName(name)
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

  function fetchFeatures(groupName?: string) {
    getList({
      groupName: groupName,
    }).then((res) => {
      const featureGroup = groupBy(
        res.items.filter((def) => !def.isStatic),
        'groupName',
      );
      const featureTreeData: FeatureTreeData[] = [];
      Object.keys(featureGroup).forEach((gk) => {
        const featureTree = listToTree(featureGroup[gk], {
          id: 'name',
          pid: 'parentName',
        });
        formatDisplayName(featureTree);
        featureTreeData.push(...featureTree);
      });
      state.availableFeatures = featureTreeData;
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

  function handleGroupChange(value?: string) {
    state.entity.parentName = undefined;
    fetchFeatures(value);
  }

  function handleParentChange(value?: string) {
    if (!value) return;
    getByName(value).then((res) => {
      state.entity.groupName = res.groupName;
      const form = unref(formRef);
      form?.clearValidate(['groupName']);
    });
  }

  function handleValueTypeNameChange(valueTypeName: string) {
    valueTypeNameRef.value = valueTypeName;
    switch (valueTypeName) {
      case 'ToggleStringValueType':
        if (!isBoolean(state.entity.defaultValue)) {
          state.entity.defaultValue = false;
        }
        break;
      default:
        if (isBoolean(state.entity.defaultValue)) {
          state.entity.defaultValue = undefined;
        }
        break;
    }
  }

  function handleValidatorNameChange(validatorName: string) {
    validatorNameRef.value = validatorName;
  }

  function handleSelectionChange(items: SelectionStringValueItem[]) {
    if (items.length === 0) {
      state.entity.defaultValue = undefined;
      state.selectionDataSource = [];
      return;
    }
    state.selectionDataSource = items.map((item) => {
      return {
        label: Lr(item.displayText.resourceName, item.displayText.name),
        value: item.value,
      };
    });
    if (!items.find((item) => item.value === state.entity.defaultValue)) {
      state.entity.defaultValue = undefined;
    }
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
          state.allowedChange = false;
          resolve(true);
        },
        onCancel: () => {
          resolve(false);
        },
        afterClose: () => {
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
      const input = cloneDeep(state.entity);
      if (!isNullOrUnDef(input.defaultValue)) {
        input.defaultValue = String(input.defaultValue);
      }
      const api = state.entityEditFlag
        ? update(state.entity.name, input as FeatureDefinitionUpdateDto)
        : create(input as FeatureDefinitionCreateDto);
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
