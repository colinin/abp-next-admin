<template>
  <BasicModal
    @register="registerModal"
    :title="L('PermissionDefinitions')"
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
              @change="handleGroupChange"
            />
          </FormItem>
          <FormItem name="parentName" :label="L('DisplayName:ParentName')">
            <TreeSelect
              :disabled="!state.allowedChange"
              :allow-clear="true"
              :tree-data="state.availablePermissions"
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
          <FormItem name="multiTenancySide" :label="L('DisplayName:MultiTenancySide')">
            <Select
              :disabled="!state.allowedChange"
              v-model:value="state.entity.multiTenancySide"
              :options="multiTenancySides"
            />
          </FormItem>
          <FormItem name="providers" :label="L('DisplayName:Providers')">
            <Select
              :disabled="!state.allowedChange"
              mode="multiple"
              :allow-clear="true"
              v-model:value="state.entity.providers"
              :options="providers"
            />
          </FormItem>
        </TabPane>
        <TabPane key="stateCheckers" :tab="L('StateCheckers')" force-render>
          <FormItem
            name="stateCheckers"
            label=""
            :label-col="{ span: 0 }"
            :wrapper-col="{ span: 24 }"
          >
            <SimpleStateChecking
              :allow-delete="true"
              :allow-edit="true"
              :disabled="!state.allowedChange"
              v-model:value="state.entity.stateCheckers"
              :state="state.simpleCheckState"
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
  import { SimplaCheckStateBase, PermissionState } from '/@/components/Abp';
  import { computed, ref, reactive, unref, nextTick, watch } from 'vue';
  import { Checkbox, Form, Input, Select, Tabs, TreeSelect } from 'ant-design-vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import {
    LocalizableInput,
    ExtraPropertyDictionary,
    SimpleStateChecking,
  } from '/@/components/Abp';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { ValidationEnum, useValidation } from '/@/hooks/abp/useValidation';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import {
    GetAsyncByName,
    CreateAsyncByInput,
    UpdateAsyncByNameAndInput,
    GetListAsyncByInput,
  } from '/@/api/permission-management/definitions/permissions';
  import {
    MultiTenancySides,
    PermissionDefinitionUpdateDto,
    PermissionDefinitionCreateDto,
  } from '/@/api/permission-management/definitions/permissions/model';
  import { PermissionGroupDefinitionDto } from '/@/api/permission-management/definitions/groups/model';
  import { multiTenancySides, providers } from '../../typing';
  import { listToTree } from '/@/utils/helper/treeHelper';
  import { groupBy } from '/@/utils/array';

  const FormItem = Form.Item;
  const TabPane = Tabs.TabPane;
  interface PermissionSelectData {
    label: string;
    value: string;
  }
  interface PermissionTreeData {
    name: string;
    groupName: string;
    displayName: string;
    children: PermissionTreeData[];
  }
  interface State {
    activeTab: string;
    allowedChange: boolean;
    entity: Recordable;
    entityRules?: Dictionary<string, Rule>;
    entityChanged: boolean;
    entityEditFlag: boolean;
    defaultGroup?: string;
    availablePermissions: PermissionTreeData[];
    availableGroups: PermissionGroupDefinitionDto[];
    selectionDataSource: PermissionSelectData[];
    simpleCheckState: SimplaCheckStateBase;
  }

  const emits = defineEmits(['register', 'change']);

  const { ruleCreator } = useValidation();
  const { deserialize, validate } = useLocalizationSerializer();
  const { createConfirm, createMessage } = useMessage();
  const { L, Lr } = useLocalization(['AbpPermissionManagement', 'AbpUi']);

  const formRef = ref<any>();
  const state = reactive<State>({
    activeTab: 'basic',
    entity: {},
    allowedChange: false,
    entityChanged: false,
    entityEditFlag: false,
    availablePermissions: [],
    availableGroups: [],
    selectionDataSource: [],
    simpleCheckState: new PermissionState(),
    entityRules: {
      groupName: ruleCreator.fieldRequired({
        name: 'GroupName',
        prefix: 'DisplayName',
        resourceName: 'AbpPermissionManagement',
        trigger: 'blur',
      }),
      name: ruleCreator.fieldRequired({
        name: 'Name',
        prefix: 'DisplayName',
        resourceName: 'AbpPermissionManagement',
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
        isEnabled: true,
        groupName: state.defaultGroup,
        multiTenancySide: MultiTenancySides.Both,
        providers: [],
        extraProperties: {},
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
        nextTick(() => {
          state.entityChanged = false;
        });
      });
  }

  function fetchFeatures(groupName?: string) {
    GetListAsyncByInput({
      groupName: groupName,
    }).then((res) => {
      const permissionGroup = groupBy(
        res.items.filter((def) => !def.isStatic),
        'groupName',
      );
      const permissionTreeData: PermissionTreeData[] = [];
      Object.keys(permissionGroup).forEach((gk) => {
        const permissionTree = listToTree(permissionGroup[gk], {
          id: 'name',
          pid: 'parentName',
        });
        formatDisplayName(permissionTree);
        permissionTreeData.push(...permissionTree);
      });
      state.availablePermissions = permissionTreeData;
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
    GetAsyncByName(value).then((res) => {
      state.entity.groupName = res.groupName;
      const form = unref(formRef);
      form?.clearValidate(['groupName']);
    });
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
      const api = state.entityEditFlag
        ? UpdateAsyncByNameAndInput(
            state.entity.name,
            state.entity as PermissionDefinitionUpdateDto,
          )
        : CreateAsyncByInput(state.entity as PermissionDefinitionCreateDto);
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
