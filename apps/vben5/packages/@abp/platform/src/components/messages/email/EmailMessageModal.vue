<script setup lang="ts">
import type { EmailMessageDto } from '../../../types/messages';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { Tinymce } from '@abp/components/tinymce';
import { toDate } from '@abp/core';

import { useEmailMessagesApi } from '../../../api/useEmailMessagesApi';

const { getApi } = useEmailMessagesApi();

const [Form, formApi] = useVbenForm({
  commonConfig: {
    // 所有表单项
    componentProps: {
      class: 'w-full',
    },
  },
  layout: 'vertical',
  schema: [
    {
      fieldName: 'subject',
      component: 'Input',
      disabled: true,
      label: $t('AppPlatform.DisplayName:Subject'),
    },
    {
      fieldName: 'receiver',
      component: 'Input',
      disabled: true,
      label: $t('AppPlatform.DisplayName:Receiver'),
    },
    {
      fieldName: 'sender',
      component: 'Input',
      disabled: true,
      label: $t('AppPlatform.DisplayName:From'),
    },
    {
      fieldName: 'creationTime',
      component: 'DatePicker',
      disabled: true,
      componentProps: {
        format: 'YYYY-MM-DD HH:mm:ss',
      },
      label: $t('AppPlatform.DisplayName:CreationTime'),
    },
    {
      fieldName: 'content',
      component: 'Input',
      label: $t('AppPlatform.DisplayName:Content'),
    },
  ],
  showDefaultActions: false,
});

const [Modal, modalApi] = useVbenModal({
  class: 'w-[800px]',
  onOpenChange(isOpen) {
    if (isOpen) {
      formApi.resetForm();
      onInit();
    }
  },
  showConfirmButton: false,
});

async function onInit() {
  const { id } = modalApi.getData<EmailMessageDto>();
  const dto = await getApi(id);
  await formApi.setValues({
    subject: dto.subject,
    receiver: dto.receiver,
    sender: dto.sender,
    creationTime: toDate(dto.creationTime),
    content: dto.content,
  });
}
</script>

<template>
  <Modal :title="$t('AppPlatform.EmailMessages')">
    <Form>
      <template #content="{ modelValue }">
        <Tinymce
          v-if="modelValue"
          width="100%"
          :value="modelValue"
          :plugins="[]"
          :toolbar="[]"
          readonly
          menubar="''"
        />
      </template>
    </Form>
  </Modal>
</template>

<style scoped></style>
