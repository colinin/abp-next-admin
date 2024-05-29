<template>
  <BasicModal
    @register="registerModal"
    :title="L('Settings')"
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
          <FormItem name="name" :label="L('DisplayName:Name')">
            <Input
              :disabled="state.entityEditFlag && !state.allowedChange"
              :allow-clear="true"
              v-model:value="state.entity.name"
            />
          </FormItem>
          <FormItem name="defaultValue" :label="L('DisplayName:DefaultValue')">
            <TextArea
              :disabled="state.entityEditFlag && !state.allowedChange"
              :allow-clear="true"
              :auto-size="{ minRows: 3 }"
              v-model:value="state.entity.defaultValue"
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
          <FormItem
            name="providers"
            :label="L('DisplayName:Providers')"
            :extra="L('Description:Providers')"
          >
            <Select
              mode="multiple"
              :disabled="!state.allowedChange"
              :allow-clear="true"
              v-model:value="state.entity.providers"
              :options="providers"
            />
          </FormItem>
          <FormItem
            name="isInherited"
            :label="L('DisplayName:IsInherited')"
            :extra="L('Description:IsInherited')"
          >
            <Checkbox :disabled="!state.allowedChange" v-model:checked="state.entity.isInherited"
              >{{ L('DisplayName:IsInherited') }}
            </Checkbox>
          </FormItem>
          <FormItem
            name="isEncrypted"
            :label="L('DisplayName:IsEncrypted')"
            :extra="L('Description:IsEncrypted')"
          >
            <Checkbox :disabled="!state.allowedChange" v-model:checked="state.entity.isEncrypted"
              >{{ L('DisplayName:IsEncrypted') }}
            </Checkbox>
          </FormItem>
          <FormItem
            name="isVisibleToClients"
            :label="L('DisplayName:IsVisibleToClients')"
            :extra="L('Description:IsVisibleToClients')"
          >
            <Checkbox
              :disabled="!state.allowedChange"
              v-model:checked="state.entity.isVisibleToClients"
              >{{ L('DisplayName:IsVisibleToClients') }}
            </Checkbox>
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
  import { cloneDeep } from 'lodash-es';
  import { ref, reactive, unref, nextTick, watch } from 'vue';
  import { Checkbox, Form, Input, Select, Tabs } from 'ant-design-vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { LocalizableInput, ExtraPropertyDictionary } from '/@/components/Abp';
  import { ModalState } from '../types/props';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { ValidationEnum, useValidation } from '/@/hooks/abp/useValidation';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import {
    GetAsyncByName,
    CreateAsyncByInput,
    UpdateAsyncByNameAndInput,
  } from '/@/api/settings-management/definitions';
  import {
    SettingDefinitionDto,
    SettingDefinitionUpdateDto,
    SettingDefinitionCreateDto,
  } from '/@/api/settings-management/definitions/model';

  const FormItem = Form.Item;
  const TabPane = Tabs.TabPane;
  const TextArea = Input.TextArea;

  const emits = defineEmits(['register', 'change']);

  const { ruleCreator } = useValidation();
  const { validate } = useLocalizationSerializer();
  const { createConfirm, createMessage } = useMessage();
  const { L } = useLocalization(['AbpSettingManagement', 'AbpUi']);

  const formRef = ref<any>();
  const providers = reactive([
    { label: L('Providers:Default'), value: 'D' },
    { label: L('Providers:Configuration'), value: 'C' },
    { label: L('Providers:Global'), value: 'G' },
    { label: L('Providers:Tenant'), value: 'T' },
    { label: L('Providers:User'), value: 'U' },
  ]);
  const state = reactive<ModalState>({
    activeTab: 'basic',
    entity: {
      isInherited: true,
    } as SettingDefinitionDto,
    allowedChange: false,
    entityChanged: false,
    entityEditFlag: false,
    entityRules: {
      name: ruleCreator.fieldRequired({
        name: 'Name',
        prefix: 'DisplayName',
        resourceName: 'AbpSettingManagement',
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
  watch(
    () => state.entity,
    () => {
      state.entityChanged = true;
    },
    {
      deep: true,
    },
  );

  const [registerModal, { closeModal, changeLoading, changeOkLoading }] = useModalInner(
    (record) => {
      nextTick(() => fetch(record.name));
    },
  );

  function fetch(name?: string) {
    state.activeTab = 'basic';
    state.entityEditFlag = false;
    if (!name) {
      state.entity = {
        isInherited: true,
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
            cloneDeep(state.entity) as SettingDefinitionUpdateDto,
          )
        : CreateAsyncByInput(cloneDeep(state.entity) as SettingDefinitionCreateDto);
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
