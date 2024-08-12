<template>
  <BasicModal
    @register="registerModal"
    :title="L('Notifications:Send')"
    :can-fullscreen="false"
    :width="800"
    :height="500"
    @ok="handleSubmit"
  >
    <Form
      v-if="state.notification"
      ref="formRef"
      :model="state.entity"
      :rules="state.entityRules"
      :label-col="{ span: 6 }"
      :wrapper-col="{ span: 18 }"
    >
      <Tabs v-model:active-key="state.activeTab">
        <TabPane key="basic" :tab="L('BasicInfo')">
          <FormItem name="name" :label="L('DisplayName:Name')">
            <Input disabled :value="getDisplayName(state.notification.displayName)" />
          </FormItem>
          <!-- <FormItem v-if="!getIsTemplate" name="useLocalization" :label="L('Notifications:UseLocalization')">
            <Checkbox v-model:checked="state.entity.useLocalization" @change="(e) => handleUseLocalization(e.target.checked)">
              {{ L('Notifications:UseLocalization') }}
            </Checkbox>
          </FormItem> -->
          <FormItem v-if="!getIsTemplate" name="title" :label="L('Notifications:Title')">
            <TextArea v-model:value="state.entity.title" />
          </FormItem>
          <FormItem v-if="!getIsTemplate" name="message" :label="L('Notifications:Message')">
            <TextArea v-model:value="state.entity.message" />
          </FormItem>
          <FormItem
            v-if="!getIsTemplate"
            name="description"
            :label="L('Notifications:Description')"
          >
            <TextArea v-model:value="state.entity.description" />
          </FormItem>
          <FormItem name="culture" :label="L('Notifications:Culture')">
            <Select
              allow-clear
              v-model:value="state.entity.culture"
              :options="getLanguageOptions"
              @change="handleCultureChange"
            />
          </FormItem>
          <FormItem name="toUsers" :label="L('Notifications:ToUsers')">
            <Select
              mode="tags"
              show-search
              allow-clear
              :default-active-first-option="false"
              :filter-option="false"
              :not-found-content="null"
              v-model:value="state.entity.toUsers"
              :options="getUserOptions"
            />
          </FormItem>
          <FormItem name="severity" :label="L('Notifications:Severity')">
            <Select
              allow-clear
              v-model:value="state.entity.severity"
              :options="notificationSeverityOptions"
            />
          </FormItem>
        </TabPane>
        <TabPane key="propertites" :tab="L('Properties')">
          <FormItem name="data" label="" :label-col="{ span: 0 }" :wrapper-col="{ span: 24 }">
            <ExtraPropertyDictionary
              :allow-delete="true"
              :allow-edit="true"
              v-model:value="state.entity.data"
            />
          </FormItem>
        </TabPane>
        <TabPane
          v-if="getIsTemplate && state.templateContent"
          key="content"
          :tab="L('TemplateContent')"
        >
          <TextArea
            readonly
            :auto-size="{ minRows: 15, maxRows: 50 }"
            :value="state.templateContent.content"
          />
        </TabPane>
      </Tabs>
    </Form>
  </BasicModal>
</template>

<script setup lang="ts">
  import type { Rule } from 'ant-design-vue/lib/form';
  import { computed, reactive, ref, unref, onMounted } from 'vue';
  import { Form, Input, Select, Tabs } from 'ant-design-vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { ExtraPropertyDictionary } from '/@/components/Abp';
  import { useAbpStoreWithOut } from '/@/store/modules/abp';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { ValidationEnum, useValidation } from '/@/hooks/abp/useValidation';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import { getList as getLanguages } from '/@/api/localization/languages';
  import { Language } from '/@/api/localization/languages/model';
  import { getList as getUsers } from '/@/api/identity/users';
  import { User } from '/@/api/identity/users/model';
  import { send, sendTemplate } from '/@/api/realtime/notifications';
  import { NotificationSendDto } from '/@/api/realtime/notifications/model';
  import { NotificationDefinitionDto } from '/@/api/realtime/notifications/definitions/notifications/model';
  import { GetAsyncByInput as getTemplateContent } from '/@/api/text-templating/contents';
  import { TextTemplateContentDto } from '/@/api/text-templating/contents/model';
  import { isNullOrWhiteSpace } from '/@/utils/strings';
  import { useNotificationDefinition } from '../hooks/useNotificationDefinition';
  import { formatToDateTime } from '/@/utils/dateUtil';

  const FormItem = Form.Item;
  const TabPane = Tabs.TabPane;
  const TextArea = Input.TextArea;
  interface State {
    activeTab: string;
    entity: Recordable;
    entityRules?: Dictionary<string, Rule>;
    notification?: NotificationDefinitionDto;
    templateContent?: TextTemplateContentDto;
    languages: Language[];
    users: User[];
  }

  const formRef = ref<any>();
  const { getApplication } = useAbpStoreWithOut();
  const { ruleCreator } = useValidation();
  const { deserialize, validate } = useLocalizationSerializer();
  const { createMessage } = useMessage();
  const { L, Lr } = useLocalization(['Notifications', 'AbpValidation', 'AbpUi']);
  const { notificationSeverityOptions } = useNotificationDefinition();
  const state = reactive<State>({
    activeTab: 'basic',
    entity: {},
    entityRules: {
      title: ruleCreator.defineValidator({
        required: true,
        trigger: 'blur',
        validator(_rule, value) {
          if (state.entity.useLocalization) {
            if (!validate(value)) {
              return Promise.reject(L(ValidationEnum.FieldRequired, [L('Notifications:Title')]));
            }
          } else {
            if (isNullOrWhiteSpace(value)) {
              return Promise.reject(L(ValidationEnum.FieldRequired, [L('Notifications:Title')]));
            }
          }
          return Promise.resolve();
        },
      }),
      message: ruleCreator.defineValidator({
        trigger: 'blur',
        required: true,
        validator(_rule, value) {
          if (state.entity.useLocalization) {
            if (!validate(value)) {
              return Promise.reject(L(ValidationEnum.FieldRequired, [L('Notifications:Message')]));
            }
          } else {
            if (isNullOrWhiteSpace(value)) {
              return Promise.reject(L(ValidationEnum.FieldRequired, [L('Notifications:Message')]));
            }
          }
          return Promise.resolve();
        },
      }),
      description: ruleCreator.defineValidator({
        trigger: 'blur',
        validator(_rule, value) {
          if (state.entity.useLocalization) {
            if (!validate(value, { required: false })) {
              return Promise.reject(
                L(ValidationEnum.FieldRequired, [L('Notifications:Description')]),
              );
            }
          }
          return Promise.resolve();
        },
      }),
    },
    notification: undefined,
    languages: [],
    users: [],
  });
  const [registerModal, { changeOkLoading, closeModal }] = useModalInner(
    (data: NotificationDefinitionDto) => {
      state.activeTab = 'basic';
      state.entity = {
        data: {} as Recordable,
      };
      state.notification = data;
      state.templateContent = undefined;
      if (!isNullOrWhiteSpace(data.template)) {
        fetchTemplateContent(data.template!);
      }
    },
  );
  const getIsTemplate = computed(() => {
    if (!state.notification) return false;
    return !isNullOrWhiteSpace(state.notification.template);
  });
  const getUserOptions = computed(() => {
    return state.users.map((item) => {
      return {
        label: item.userName,
        value: item.id,
      };
    });
  });
  const getLanguageOptions = computed(() => {
    return state.languages.map((item) => {
      return {
        label: item.displayName,
        value: item.cultureName,
      };
    });
  });
  onMounted(() => {
    fetchUsers();
    fetchLanguages();
  });

  const getDisplayName = (displayName?: string) => {
    if (!displayName) return displayName;
    const info = deserialize(displayName);
    return Lr(info.resourceName, info.name);
  };

  function fetchLanguages() {
    getLanguages({}).then((res) => {
      state.languages = res.items;
    });
  }

  function fetchUsers(filter?: string) {
    getUsers({
      filter: filter,
    }).then((res) => {
      state.users = res.items;
    });
  }

  function fetchTemplateContent(template: string) {
    getTemplateContent({
      name: template,
      culture: state.entity.culture,
    }).then((content) => {
      state.templateContent = content;
    });
  }

  function handleCultureChange() {
    if (!isNullOrWhiteSpace(state.notification?.template)) {
      fetchTemplateContent(state.notification!.template!);
    }
  }

  function handleSubmit() {
    if (!state.notification) return;
    const formEl = unref(formRef);
    formEl?.validate().then(() => {
      let input: NotificationSendDto;
      let toUsers: { userId: string }[] = [];
      if (state.entity.toUsers && Array.isArray(state.entity.toUsers)) {
        toUsers = state.entity.toUsers.map((id) => {
          return { userId: id };
        });
      }
      if (getIsTemplate.value) {
        input = {
          name: state.notification!.template!,
          severity: state.entity.severity,
          culture: state.entity.culture,
          data: state.entity.data,
          toUsers: toUsers,
        };
        changeOkLoading(true);
        sendTemplate(input)
          .then(() => {
            createMessage.success(L('Successful'));
            closeModal();
          })
          .finally(() => {
            changeOkLoading(false);
          });
      } else {
        let title: any = state.entity.title;
        let message: any = state.entity.message;
        let description: any = state.entity.description;
        // if (state.entity.useLocalization) {
        //   const titleInfo = deserialize(title);
        //   title = {
        //     resourceName: titleInfo.resourceName,
        //     name: titleInfo.name,
        //   };
        // }
        // if (state.entity.useLocalization) {
        //   const messageInfo = deserialize(message);
        //   message = {
        //     resourceName: messageInfo.resourceName,
        //     name: messageInfo.name,
        //   };
        // }
        // if (state.entity.useLocalization && !isNullOrUnDef(description)) {
        //   const descriptionInfo = deserialize(description);
        //   description = {
        //     resourceName: descriptionInfo.resourceName,
        //     name: descriptionInfo.name,
        //   };
        // }
        input = {
          name: state.notification!.name,
          severity: state.entity.severity,
          culture: state.entity.culture,
          data: {
            ...state.entity.data,
            title: title,
            message: message,
            description: description,
            formUser: getApplication.currentUser.userName,
            createTime: formatToDateTime(new Date()),
          },
          toUsers: toUsers,
        };
        changeOkLoading(true);
        send(input)
          .then(() => {
            createMessage.success(L('Successful'));
            closeModal();
          })
          .finally(() => {
            changeOkLoading(false);
          });
      }
    });
  }
</script>

<style scoped></style>
