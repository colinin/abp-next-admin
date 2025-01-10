<script setup lang="ts">
import type { FormInstance } from 'ant-design-vue';

import type { IdentityRoleDto } from '../../types/roles';

import { defineEmits, defineOptions, ref, toValue } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { Checkbox, Form, Input, message } from 'ant-design-vue';

import { useRolesApi } from '../../api/useRolesApi';

defineOptions({
  name: 'RoleModal',
});
const emits = defineEmits<{
  (event: 'change', data: IdentityRoleDto): void;
}>();

const FormItem = Form.Item;

const defaultModel = {
  isDefault: false,
  isPublic: true,
  isStatic: false,
} as IdentityRoleDto;

const form = ref<FormInstance>();
const formModel = ref<IdentityRoleDto>({ ...defaultModel });

const { cancel, createApi, getApi, updateApi } = useRolesApi();
const [Modal, modalApi] = useVbenModal({
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onClosed() {
    cancel('Role modal has closed!');
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
      formModel.value = { ...defaultModel };
      modalApi.setState({
        title: $t('AbpIdentity.NewRole'),
      });
      const roleDto = modalApi.getData<IdentityRoleDto>();
      if (roleDto?.id) {
        try {
          modalApi.setState({ loading: true });
          const dto = await getApi(roleDto.id);
          formModel.value = dto;
          modalApi.setState({
            title: $t('AbpIdentity.RoleSubject', [dto.name]),
          });
        } finally {
          modalApi.setState({ loading: false });
        }
      }
    }
  },
  title: 'Roles',
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
      <FormItem :label="$t('AbpIdentity.DisplayName:IsDefault')">
        <Checkbox v-model:checked="formModel.isDefault">
          {{ $t('AbpIdentity.DisplayName:IsDefault') }}
        </Checkbox>
      </FormItem>
      <FormItem :label="$t('AbpIdentity.DisplayName:IsPublic')">
        <Checkbox v-model:checked="formModel.isPublic">
          {{ $t('AbpIdentity.DisplayName:IsPublic') }}
        </Checkbox>
      </FormItem>
      <FormItem
        :label="$t('AbpIdentity.DisplayName:RoleName')"
        name="name"
        required
      >
        <Input v-model:value="formModel.name" />
      </FormItem>
    </Form>
  </Modal>
</template>

<style scoped></style>
