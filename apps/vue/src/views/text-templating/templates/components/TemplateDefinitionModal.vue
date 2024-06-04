<template>
  <BasicModal
    @register="registerModal"
    :title="L('TextTemplates')"
    :width="800"
    :min-height="400"
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
          <FormItem hidden name="concurrencyStamp" label="concurrencyStamp">
            <Input v-model:value="state.entity.concurrencyStamp" />
          </FormItem>
          <FormItem
            hidden
            name="localizationResourceName"
            label="L('DisplayName:LocalizationResourceName')"
          >
            <Input v-model:value="state.entity.localizationResourceName" />
          </FormItem>
          <FormItem name="isInlineLocalized" :label="L('DisplayName:Name')">
            <Checkbox
              :disabled="state.entityEditFlag && !state.allowedChange"
              v-model:checked="state.entity.isInlineLocalized"
              >{{ L('DisplayName:IsInlineLocalized') }}
            </Checkbox>
          </FormItem>
          <FormItem name="defaultCultureName" :label="L('DisplayName:DefaultCultureName')">
            <Select
              :disabled="!state.allowedChange"
              :allow-clear="true"
              v-model:value="state.entity.defaultCultureName"
              :options="state.languages"
            />
          </FormItem>
          <FormItem name="isLayout" :label="L('DisplayName:IsLayout')">
            <Checkbox
              :disabled="state.entityEditFlag && !state.allowedChange"
              v-model:checked="state.entity.isLayout"
              >{{ L('DisplayName:IsLayout') }}
            </Checkbox>
          </FormItem>
          <FormItem :hidden="state.entity.isLayout" name="layout" :label="L('DisplayName:Layout')">
            <Select
              :disabled="!state.allowedChange"
              :allow-clear="true"
              v-model:value="state.entity.layout"
              :options="state.layouts"
            />
          </FormItem>
          <FormItem name="name" :label="L('DisplayName:Name')">
            <Input :disabled="!state.allowedChange" v-model:value="state.entity.name" />
          </FormItem>
          <FormItem name="displayName" :label="L('DisplayName:DisplayName')">
            <LocalizableInput
              :disabled="!state.allowedChange"
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

<script lang="ts" setup>
  import type { Rule } from 'ant-design-vue/lib/form';
  import type { DefaultOptionType } from 'ant-design-vue/lib/select';
  import { reactive, ref, unref, nextTick, onMounted, watch, watchEffect } from 'vue';
  import { Checkbox, Form, Input, Select, Tabs } from 'ant-design-vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { LocalizableInput, ExtraPropertyDictionary } from '/@/components/Abp';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { ValidationEnum, useValidation } from '/@/hooks/abp/useValidation';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import {
    GetByNameAsyncByName,
    CreateAsyncByInput,
    UpdateAsyncByNameAndInput,
    GetListAsyncByInput,
  } from '/@/api/text-templating/definitions';
  import { getList as getLanguages } from '/@/api/localization/languages';

  const FormItem = Form.Item;
  const TabPane = Tabs.TabPane;

  const emits = defineEmits(['register', 'change']);

  interface State {
    activeTab: string;
    allowedChange: boolean;
    entity: Recordable;
    entityRules?: Dictionary<string, Rule>;
    entityChanged: boolean;
    entityEditFlag: boolean;
    layouts: DefaultOptionType[];
    languages: DefaultOptionType[];
  }

  const { ruleCreator } = useValidation();
  const { createConfirm, createMessage } = useMessage();
  const { deserialize, validate } = useLocalizationSerializer();
  const { L, Lr } = useLocalization(['AbpTextTemplating']);
  const [registerModal, { changeLoading, changeOkLoading, closeModal }] = useModalInner((data) => {
    nextTick(() => fetch(data?.name));
  });
  const formRef = ref<any>();
  const state = reactive<State>({
    activeTab: 'basic',
    allowedChange: true,
    entityChanged: false,
    entityEditFlag: false,
    entity: {},
    entityRules: {
      name: ruleCreator.fieldRequired({
        name: 'Name',
        prefix: 'DisplayName',
        resourceName: 'AbpTextTemplating',
        trigger: 'blur',
      }),
      displayName: ruleCreator.defineValidator({
        required: true,
        trigger: 'blur',
        validator(_rule, value) {
          if (!validate(value)) {
            return Promise.reject(
              Lr('AbpValidation', ValidationEnum.FieldRequired, [L('DisplayName:DisplayName')]),
            );
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
    layouts: [],
    languages: [],
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
  watchEffect(watchLayoutChange);
  onMounted(() => {
    fetchLayouts();
    fetchLanguages();
  });

  function watchLayoutChange() {
    if (state.entity.isLayout) {
      state.entity.layout = '';
    }
  }

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
    GetByNameAsyncByName(name)
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

  function fetchLanguages() {
    getLanguages({}).then((res) => {
      state.languages = res.items.map((item) => {
        return {
          label: item.displayName,
          value: item.cultureName,
        };
      });
    });
  }

  function fetchLayouts() {
    GetListAsyncByInput({
      isLayout: true,
    }).then((res) => {
      state.layouts = res.items.map((item) => {
        const info = deserialize(item.displayName);
        return {
          label: Lr(info.resourceName, info.name),
          value: item.name,
        };
      });
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
      });
    });
  }

  function handleSubmit() {
    if (!state.allowedChange) {
      closeModal();
      return;
    }
    const form = unref(formRef);
    form?.validate().then((input) => {
      changeLoading(true);
      changeOkLoading(true);
      const submitApi = state.entityEditFlag
        ? UpdateAsyncByNameAndInput(input.name, input)
        : CreateAsyncByInput(input);
      submitApi
        .then((res) => {
          createMessage.success(L('Successful'));
          emits('change', res);
          form.resetFields();
          closeModal();
        })
        .finally(() => {
          changeLoading(false);
          changeOkLoading(false);
        });
    });
  }
</script>

<style lang="less" scoped></style>
