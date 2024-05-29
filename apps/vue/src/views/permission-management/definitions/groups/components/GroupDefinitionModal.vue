<template>
  <BasicModal
    @register="registerModal"
    :title="L('GroupDefinitions')"
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
          <FormItem name="displayName" :label="L('DisplayName:DisplayName')">
            <LocalizableInput
              :disabled="!state.allowedChange"
              :allow-clear="true"
              v-model:value="state.entity.displayName"
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
  import { ref, reactive, unref, nextTick, watch } from 'vue';
  import { Form, Input, Tabs } from 'ant-design-vue';
  import { ExtraPropertyDictionary } from '/@/components/Abp';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { LocalizableInput } from '/@/components/Abp';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { ValidationEnum, useValidation } from '/@/hooks/abp/useValidation';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import {
    GetAsyncByName,
    CreateAsyncByInput,
    UpdateAsyncByNameAndInput,
  } from '/@/api/permission-management/definitions/groups';
  import {
    PermissionGroupDefinitionUpdateDto,
    PermissionGroupDefinitionCreateDto,
  } from '/@/api/permission-management/definitions/groups/model';

  const FormItem = Form.Item;
  const TabPane = Tabs.TabPane;
  interface State {
    activeTab: string;
    allowedChange: boolean;
    entity: Recordable;
    entityRules?: Dictionary<string, Rule>;
    entityChanged: boolean;
    entityEditFlag: boolean;
  }

  const emits = defineEmits(['register', 'change']);

  const { ruleCreator } = useValidation();
  const { validate } = useLocalizationSerializer();
  const { createConfirm, createMessage } = useMessage();
  const { L } = useLocalization(['AbpPermissionManagement', 'AbpUi']);

  const formRef = ref<any>();
  const state = reactive<State>({
    activeTab: 'basic',
    entity: {},
    allowedChange: false,
    entityChanged: false,
    entityEditFlag: false,
    entityRules: {
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
      state.entity = {};
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
            cloneDeep(state.entity) as PermissionGroupDefinitionUpdateDto,
          )
        : CreateAsyncByInput(cloneDeep(state.entity) as PermissionGroupDefinitionCreateDto);
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
