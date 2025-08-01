<script setup lang="ts">
import type { FormInstance } from 'ant-design-vue';
import type { DefaultOptionType } from 'ant-design-vue/es/select';

import type { IdentityClaimTypeDto } from '../../types/claim-types';

import { defineEmits, defineOptions, reactive, ref, toValue } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import {
  Checkbox,
  Form,
  Input,
  message,
  Select,
  Textarea,
} from 'ant-design-vue';

import { useClaimTypesApi } from '../../api/useClaimTypesApi';
import { ValueType } from '../../types/claim-types';

defineOptions({
  name: 'ClaimTypeModal',
});
const emits = defineEmits<{
  (event: 'change', data: IdentityClaimTypeDto): void;
}>();

const FormItem = Form.Item;

const defaultModel = {
  required: false,
} as IdentityClaimTypeDto;

const form = ref<FormInstance>();
const formModel = ref<IdentityClaimTypeDto>({ ...defaultModel });
const valueTypeOptions = reactive<DefaultOptionType[]>([
  {
    label: 'String',
    value: ValueType.String,
  },
  {
    label: 'Int',
    value: ValueType.Int,
  },
  {
    label: 'Boolean',
    value: ValueType.Boolean,
  },
  {
    label: 'DateTime',
    value: ValueType.DateTime,
  },
]);

const { cancel, createApi, getApi, updateApi } = useClaimTypesApi();
const [Modal, modalApi] = useVbenModal({
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onClosed() {
    cancel('ClaimType Modal has closed!');
  },
  onConfirm: async () => {
    await form.value?.validate();
    const api = formModel.value.id
      ? updateApi(formModel.value.id, toValue(formModel))
      : createApi(toValue(formModel));
    modalApi.setState({ submitting: true });
    api
      .then((res) => {
        message.success($t('AbpUi.Success'));
        emits('change', res);
        modalApi.close();
      })
      .finally(() => {
        modalApi.setState({ submitting: false });
      });
  },
  onOpenChange: async (isOpen: boolean) => {
    if (isOpen) {
      formModel.value = { ...defaultModel };
      modalApi.setState({
        showConfirmButton: false,
        title: $t('AbpIdentity.IdentityClaim:New'),
      });
      const claimTypeDto = modalApi.getData<IdentityClaimTypeDto>();
      if (claimTypeDto?.id) {
        modalApi.setState({ loading: true });
        try {
          const dto = await getApi(claimTypeDto.id);
          formModel.value = dto;
          modalApi.setState({
            showConfirmButton: !dto.isStatic,
            title: `${$t('AbpIdentity.DisplayName:ClaimType')} - ${dto.name}`,
          });
        } finally {
          modalApi.setState({ loading: false });
        }
      } else {
        modalApi.setState({
          showConfirmButton: true,
        });
      }
    }
  },
  title: 'ClaimType',
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
      <FormItem
        :label="$t('AbpIdentity.IdentityClaim:Name')"
        name="name"
        required
      >
        <Input v-model:value="formModel.name" :disabled="formModel.isStatic" />
      </FormItem>
      <FormItem :label="$t('AbpIdentity.IdentityClaim:Required')">
        <Checkbox
          v-model:checked="formModel.required"
          :disabled="formModel.isStatic"
        >
          {{ $t('AbpIdentity.IdentityClaim:Required') }}
        </Checkbox>
      </FormItem>
      <FormItem :label="$t('AbpIdentity.IdentityClaim:Regex')">
        <Input v-model:value="formModel.regex" :disabled="formModel.isStatic" />
      </FormItem>
      <FormItem :label="$t('AbpIdentity.IdentityClaim:RegexDescription')">
        <Input
          v-model:value="formModel.regexDescription"
          :disabled="formModel.isStatic"
        />
      </FormItem>
      <FormItem :label="$t('AbpIdentity.IdentityClaim:ValueType')">
        <Select
          v-model:value="formModel.valueType"
          :disabled="formModel.isStatic"
          :options="valueTypeOptions"
        />
      </FormItem>
      <FormItem :label="$t('AbpIdentity.IdentityClaim:Description')">
        <Textarea
          v-model:value="formModel.description"
          :auto-size="{ minRows: 2 }"
          :disabled="formModel.isStatic"
        />
      </FormItem>
    </Form>
  </Modal>
</template>

<style scoped></style>
