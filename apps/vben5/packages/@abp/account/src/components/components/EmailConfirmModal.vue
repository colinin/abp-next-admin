<script setup lang="ts">
import { ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { Form, Input, message } from 'ant-design-vue';

import { useProfileApi } from '../../api/useProfileApi';

const FormItem = Form.Item;

interface State {
  confirmToken: string;
  email: string;
  returnUrl?: string;
  userId: string;
}
const formModel = ref({} as State);

const { cancel, confirmEmailApi } = useProfileApi();
const [Modal, modalApi] = useVbenModal({
  onClosed: cancel,
  onConfirm: onSubmit,
  onOpenChange(isOpen) {
    if (isOpen) {
      const state = modalApi.getData<State>();
      formModel.value = state;
    }
  },
  title: $t('AbpAccount.EmailConfirm'),
});
async function onSubmit() {
  try {
    modalApi.setState({ confirmLoading: true });
    await confirmEmailApi({
      confirmToken: encodeURIComponent(formModel.value.confirmToken),
    });
    message.success($t('AbpAccount.YourEmailIsSuccessfullyConfirm'));
    modalApi.close();
    if (formModel.value.returnUrl) {
      window.location.href = formModel.value.returnUrl;
    }
  } finally {
    modalApi.setState({ confirmLoading: false });
  }
}
</script>

<template>
  <Modal>
    <Form :model="formModel">
      <FormItem :label="$t('AbpAccount.DisplayName:Email')">
        <Input v-model:value="formModel.email" readonly />
      </FormItem>
    </Form>
  </Modal>
</template>

<style scoped></style>
