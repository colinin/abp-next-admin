<template>
  <BasicModal
    @register="registerModal"
    :title="L('NotificationDefinitions')"
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
          <FormItem
            name="allowSubscriptionToClients"
            :label="L('DisplayName:AllowSubscriptionToClients')"
            :extra="L('Description:AllowSubscriptionToClients')"
          >
            <Checkbox
              :disabled="!state.allowedChange"
              v-model:checked="state.entity.allowSubscriptionToClients"
              >{{ L('DisplayName:AllowSubscriptionToClients') }}
            </Checkbox>
          </FormItem>
          <FormItem
            name="notificationType"
            :label="L('DisplayName:NotificationType')"
            :extra="L('Description:NotificationType')"
          >
            <Select
              :disabled="!state.allowedChange"
              :allow-clear="true"
              v-model:value="state.entity.notificationType"
              :options="notificationTypeOptions"
            />
          </FormItem>
          <FormItem
            name="notificationLifetime"
            :label="L('DisplayName:NotificationLifetime')"
            :extra="L('Description:NotificationLifetime')"
          >
            <Select
              :disabled="!state.allowedChange"
              :allow-clear="true"
              v-model:value="state.entity.notificationLifetime"
              :options="notificationLifetimeOptions"
            />
          </FormItem>
          <FormItem
            name="contentType"
            :label="L('DisplayName:ContentType')"
            :extra="L('Description:ContentType')"
          >
            <Select
              :disabled="!state.allowedChange"
              :allow-clear="true"
              v-model:value="state.entity.contentType"
              :options="notificationContentTypeOptions"
            />
          </FormItem>
          <FormItem
            name="providers"
            :label="L('DisplayName:Providers')"
            :extra="L('Description:Providers')"
          >
            <Select
              :disabled="!state.allowedChange"
              :allow-clear="true"
              mode="tags"
              v-model:value="state.entity.providers"
              :options="notificationPushProviderOptions"
            />
          </FormItem>
          <FormItem
            name="template"
            :label="L('DisplayName:Template')"
            :extra="L('Description:Template')"
          >
            <Select
              :disabled="!state.allowedChange"
              :allow-clear="true"
              v-model:value="state.entity.template"
              :options="getAvailableTemplateOptions"
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
  import { computed, ref, reactive, unref, nextTick, watch } from 'vue';
  import { Checkbox, Form, Input, Select, Tabs } from 'ant-design-vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { LocalizableInput, ExtraPropertyDictionary } from '/@/components/Abp';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { ValidationEnum, useValidation } from '/@/hooks/abp/useValidation';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import { useNotificationDefinition } from '../hooks/useNotificationDefinition';
  import {
    GetAsyncByName,
    CreateAsyncByInput,
    UpdateAsyncByNameAndInput,
  } from '/@/api/realtime/notifications/definitions/notifications';
  import {
    NotificationDefinitionUpdateDto,
    NotificationDefinitionCreateDto,
  } from '/@/api/realtime/notifications/definitions/notifications/model';
  import { NotificationGroupDefinitionDto } from '/@/api/realtime/notifications/definitions/groups/model';
  import { GetListAsyncByInput as getGroupDefinitions } from '/@/api/realtime/notifications/definitions/groups';
  import {
    NotificationContentType,
    NotificationLifetime,
    NotificationType,
  } from '/@/api/realtime/notifications/types';

  const FormItem = Form.Item;
  const TabPane = Tabs.TabPane;
  interface State {
    activeTab: string;
    allowedChange: boolean;
    entity: Recordable;
    entityRules?: Dictionary<string, Rule>;
    entityChanged: boolean;
    entityEditFlag: boolean;
    defaultGroup?: string;
    availableGroups: NotificationGroupDefinitionDto[];
  }

  const emits = defineEmits(['register', 'change']);

  const { ruleCreator } = useValidation();
  const { validate } = useLocalizationSerializer();
  const { createConfirm, createMessage } = useMessage();
  const { L } = useLocalization(['Notifications', 'AbpValidation', 'AbpUi']);
  const {
    formatDisplayName,
    notificationTypeOptions,
    notificationLifetimeOptions,
    notificationContentTypeOptions,
    notificationPushProviderOptions,
    getAvailableTemplateOptions,
  } = useNotificationDefinition();

  const formRef = ref<any>();
  const state = reactive<State>({
    activeTab: 'basic',
    entity: {},
    allowedChange: false,
    entityChanged: false,
    entityEditFlag: false,
    availableGroups: [],
    entityRules: {
      groupName: ruleCreator.fieldRequired({
        name: 'GroupName',
        prefix: 'DisplayName',
        resourceName: 'Notifications',
        trigger: 'change',
      }),
      name: ruleCreator.fieldRequired({
        name: 'Name',
        prefix: 'DisplayName',
        resourceName: 'Notifications',
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
  const getGroupOptions = computed(() => {
    return state.availableGroups
      .filter((group) => !group.isStatic)
      .map((group) => {
        return {
          label: group.displayName,
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
        groupName: state.defaultGroup,
        allowSubscriptionToClients: false,
        notificationLifetime: NotificationLifetime.OnlyOne,
        notificationType: NotificationType.User,
        contentType: NotificationContentType.Text,
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
      formatDisplayName(res.items);
      state.availableGroups = res.items;
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
            cloneDeep(state.entity) as NotificationDefinitionUpdateDto,
          )
        : CreateAsyncByInput(cloneDeep(state.entity) as NotificationDefinitionCreateDto);
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
