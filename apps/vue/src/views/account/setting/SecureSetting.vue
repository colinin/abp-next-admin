<template>
  <div>
    <CollapseContainer :title="L('SecuritySettings')" :canExpan="false">
      <List>
        <template v-for="item in secureSettingList" :key="item.key">
          <ListItem>
            <ListItemMeta>
              <template #title>
                {{ item.title }}
                <Switch
                  v-if="item.switch"
                  class="extra"
                  default-checked
                  v-model:checked="item.switch.checked"
                  @change="handleChange(item, $event)"
                />
                <div v-else-if="item.extra" class="extra" @click="toggleCommand(item)">
                  {{ item.extra }}
                </div>
              </template>
              <template #description>
                <div>
                  <span>{{ item.description }}</span>
                  <Tag v-if="item.tag" :color="item.tag.color">{{ item.tag.title }}</Tag>
                </div>
              </template>
            </ListItemMeta>
          </ListItem>
        </template>
      </List>
    </CollapseContainer>
    <BasicModal @register="registerModal" @ok="handleSumbit">
      <BasicForm ref="formElRef" :schemas="formSechams" :showActionButtonGroup="false" />
    </BasicModal>
  </div>
</template>
<script lang="ts">
  import type { Rule } from 'ant-design-vue/lib/form/interface';
  import type { FormSchema, FormActionType } from '/@/components/Form';
  import type { ListItem } from './useProfile';
  import { List, Switch, Tag } from 'ant-design-vue';
  import { defineComponent, ref, unref, onMounted } from 'vue';
  import { CollapseContainer } from '/@/components/Container/index';
  import { useProfile } from './useProfile';
  import { useValidation } from '/@/hooks/abp/useValidation';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { BasicModal, useModal } from '/@/components/Modal';
  import { BasicForm } from '/@/components/Form';
  import { useAbpStoreWithOut } from '/@/store/modules/abp';
  import {
    changePassword,
    changeTwoFactorEnabled,
    sendChangePhoneNumberCode,
    changePhoneNumber,
  } from '/@/api/account/profiles';

  export default defineComponent({
    components: {
      BasicForm,
      BasicModal,
      CollapseContainer,
      List,
      ListItem: List.Item,
      ListItemMeta: List.Item.Meta,
      Switch,
      Tag,
    },
    setup() {
      const { createMessage } = useMessage();
      const { L } = useLocalization('AbpAccount');
      const { ruleCreator } = useValidation();
      const { getSecureSettingList } = useProfile();
      const [registerModal, { closeModal, openModal, setModalProps }] = useModal();
      const secureSettingList = ref<ListItem[]>([]);
      const formElRef = ref<FormActionType>();
      const formSechams = ref<FormSchema[]>([]);
      const commandApi = ref<(...args: any[]) => Promise<any>>();

      onMounted(() => {
        getSecureSettingList().then((res) => {
          secureSettingList.value = res;
        });
      });

      function toggleCommand(item: ListItem) {
        commandApi.value = undefined;
        formSechams.value = [];
        switch (item.key) {
          case 'password':
            formSechams.value = [
              {
                field: 'currentPassword',
                component: 'Input',
                label: L('DisplayName:CurrentPassword'),
                required: true,
                colProps: { span: 24 },
              },
              {
                field: 'newPassword',
                component: 'StrengthMeter',
                label: L('DisplayName:NewPassword'),
                required: true,
                colProps: { span: 24 },
              },
              {
                field: 'confirmNewPassword',
                component: 'InputPassword',
                label: L('DisplayName:NewPasswordConfirm'),
                required: true,
                colProps: { span: 24 },
                dynamicRules: ({ values }) => {
                  return [
                    ...(ruleCreator.doNotMatch({
                      name: 'NewPasswordConfirm',
                      resourceName: 'AbpAccount',
                      prefix: 'DisplayName',
                      matchField: 'NewPassword',
                      matchValue: values.newPassword,
                      trigger: 'change',
                      required: true,
                    }) as Rule[]),
                  ];
                },
              },
            ];
            commandApi.value = changePassword;
            setModalProps({
              title: item.title,
            });
            openModal();
            break;
          case 'phoneNumber':
            const abpStore = useAbpStoreWithOut();
            const currentPhoneNumber = abpStore.getApplication.currentUser.phoneNumber;
            formSechams.value = [
              {
                field: 'newPhoneNumber',
                component: 'Input',
                label: L('DisplayName:PhoneNumber'),
                required: true,
                colProps: { span: 24 },
                dynamicDisabled: ({ model, field }) => {
                  return currentPhoneNumber !== '' && model[field] === currentPhoneNumber;
                },
              },
              {
                field: 'code',
                component: 'InputCountDown',
                label: L('DisplayName:SmsVerifyCode'),
                required: true,
                colProps: { span: 24 },
                dynamicDisabled: ({ values }) => {
                  return currentPhoneNumber !== '' && values.newPhoneNumber === currentPhoneNumber;
                },
                componentProps: {
                  sendCodeApi: _sendResetPasswordCode,
                },
              },
            ];
            setModalProps({
              title: item.title,
            });
            commandApi.value = changePhoneNumber;
            openModal();
            break;
        }
      }

      function handleChange(item: ListItem, checked: boolean) {
        switch (item.key) {
          case 'twofactor':
            changeTwoFactorEnabled(checked).then(() => {
              createMessage.success(L('Successful'));
            });
            break;
        }
      }

      function _sendResetPasswordCode() {
        const formEl = unref(formElRef);
        if (formEl) {
          const field = formEl.getFieldsValue();
          sendChangePhoneNumberCode(field.newPhoneNumber)
            .then(() => {
              return Promise.resolve(true);
            })
            .catch(() => {
              return Promise.reject(false);
            });
        }
        return Promise.resolve(false);
      }

      function _changeOkButtonLoading(loaing: boolean) {
        setModalProps({
          okButtonProps: {
            loading: loaing,
          },
        });
      }

      function handleSumbit() {
        if (commandApi.value && formElRef.value) {
          formElRef.value.validate().then((input) => {
            _changeOkButtonLoading(true);
            if (commandApi.value) {
              commandApi
                .value(input)
                .then(() => {
                  closeModal();
                  createMessage.success(L('Successful'));
                })
                .finally(() => {
                  _changeOkButtonLoading(false);
                });
            } else {
              _changeOkButtonLoading(false);
            }
          });
        }
      }

      return {
        L,
        secureSettingList,
        handleChange,
        toggleCommand,
        registerModal,
        formElRef,
        formSechams,
        handleSumbit,
      };
    },
  });
</script>
<style lang="less" scoped>
  .extra {
    float: right;
    margin-top: 10px;
    margin-right: 30px;
    font-weight: normal;
    color: #1890ff;
    cursor: pointer;
  }
</style>
