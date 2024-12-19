<script setup lang="ts">
import type { FormInstance } from 'ant-design-vue';

import type { IdentityRoleDto } from '../../types/roles';
import type { IdentityUserDto } from '../../types/users';

import { defineEmits, defineOptions, ref, toValue } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import {
  Checkbox,
  Form,
  Input,
  InputPassword,
  message,
  Tabs,
  Transfer,
} from 'ant-design-vue';

import { createApi, getApi, updateApi } from '../../api/users';

defineOptions({
  name: 'UserModal',
});
const emits = defineEmits<{
  (event: 'change', data: IdentityUserDto): void;
}>();

const FormItem = Form.Item;
const TabPane = Tabs.TabPane;

const defaultModel = {
  isActive: true,
} as IdentityUserDto;

const assignableRoles = ref<IdentityRoleDto[]>([]);

const activedTab = ref('info');
const form = ref<FormInstance>();
const formModel = ref<IdentityUserDto>({ ...defaultModel });

const { hasAccessByCodes } = useAccess();
const [Modal, modalApi] = useVbenModal({
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onConfirm: async () => {
    await form.value?.validate();
    const api = formModel.value.id
      ? updateApi(formModel.value.id, toValue(formModel))
      : createApi(toValue(formModel));
    modalApi.setState({ loading: true });
    api
      .then((res) => {
        message.success($t('AbpUi.Success'));
        emits('change', res);
        modalApi.close();
      })
      .finally(() => {
        modalApi.setState({ loading: false });
      });
  },
  onOpenChange: async (isOpen: boolean) => {
    if (isOpen) {
      const { values } = modalApi.getData<Record<string, any>>();
      if (values?.id) {
        modalApi.setState({ loading: true });
        return getApi(values.id)
          .then((dto) => {
            formModel.value = dto;
            modalApi.setState({
              title: `${$t('AbpIdentity.Users')} - ${dto.userName}`,
            });
          })
          .finally(() => {
            modalApi.setState({ loading: false });
          });
      }
      formModel.value = { ...defaultModel };
      modalApi.setState({
        title: $t('NewUser'),
      });
    }
  },
  title: $t('AbpIdentity.Users'),
});
</script>

<template>
  <Modal>
    <Form
      ref="form"
      :label-col="{ span: 6 }"
      :model="formModel"
      :wrapper-col="{ span: 18 }"
    >
      <Tabs v-model:active-key="activedTab">
        <TabPane key="info" :tab="$t('AbpIdentity.UserInformations')">
          <FormItem :label="$t('AbpIdentity.DisplayName:IsActive')">
            <Checkbox v-model:checked="formModel.isActive">
              {{ $t('AbpIdentity.DisplayName:IsActive') }}
            </Checkbox>
          </FormItem>
          <FormItem
            :label="$t('AbpIdentity.UserName')"
            name="userName"
            required
          >
            <Input v-model:value="formModel.userName" />
          </FormItem>
          <FormItem
            v-if="!formModel.id"
            :label="$t('AbpIdentity.Password')"
            name="password"
            required
          >
            <InputPassword v-model:value="formModel.password" />
          </FormItem>
          <FormItem
            :label="$t('AbpIdentity.DisplayName:Surname')"
            name="surname"
          >
            <Input v-model:value="formModel.surname" />
          </FormItem>
          <FormItem :label="$t('AbpIdentity.DisplayName:Name')" name="name">
            <Input v-model:value="formModel.name" />
          </FormItem>
          <FormItem
            :label="$t('AbpIdentity.DisplayName:Email')"
            name="email"
            required
          >
            <Input v-model:value="formModel.email" />
          </FormItem>
          <FormItem
            :label="$t('AbpIdentity.DisplayName:PhoneNumber')"
            name="phoneNumber"
          >
            <Input v-model:value="formModel.phoneNumber" />
          </FormItem>
          <FormItem
            :label="$t('AbpIdentity.DisplayName:LockoutEnabled')"
            :label-col="{ span: 10 }"
          >
            <Checkbox v-model:checked="formModel.lockoutEnabled">
              {{ $t('AbpIdentity.DisplayName:LockoutEnabled') }}
            </Checkbox>
          </FormItem>
        </TabPane>
        <TabPane key="role" :tab="$t('Roles')">
          <Transfer
            :data-source="assignableRoles"
            :disabled="hasAccessByCodes(['AbpIdentity.Users.ManageRoles'])"
            :list-style="{
              width: '47%',
              height: '338px',
            }"
            :target-keys="formModel.roleNames"
            :titles="[$t('AbpIdentity.Assigned'), $t('AbpIdentity.Available')]"
            class="tree-transfer"
          />
        </TabPane>
      </Tabs>
    </Form>
  </Modal>
</template>

<style scoped></style>
