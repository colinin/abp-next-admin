<script setup lang="ts">
import type {
  TextTemplateContentDto,
  TextTemplateDefinitionDto,
} from '../../types';

import { computed, defineAsyncComponent, ref } from 'vue';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { MarkdownViewer } from '@abp/components/vditor';
import { useAbpStore } from '@abp/core';
import {
  Alert,
  Modal as AntdvModal,
  Button,
  Card,
  message,
} from 'ant-design-vue';

import { useTemplateContentsApi } from '../../api/useTemplateContentsApi';

const emits = defineEmits<{
  (event: 'change', data: TextTemplateContentDto): void;
}>();

const { cancel, getApi, restoreToDefaultApi, updateApi } =
  useTemplateContentsApi();

const abpStore = useAbpStore();
const textTemplate = ref<TextTemplateDefinitionDto>();

const getCardTitle = computed(() => {
  if (!textTemplate.value) {
    return '';
  }
  return `${$t('AbpTextTemplating.DisplayName:Name')} - ${textTemplate.value.name}`;
});

const [Form, formApi] = useVbenForm({
  commonConfig: {
    componentProps: {
      class: 'w-full',
    },
  },
  handleSubmit: onSubmit,
  layout: 'vertical',
  schema: [
    {
      component: 'Textarea',
      componentProps: {
        autoSize: {
          minRows: 20,
        },
        showCount: true,
      },
      fieldName: 'content',
      label: $t('AbpTextTemplating.DisplayName:Content'),
      rules: 'required',
    },
  ],
  showDefaultActions: false,
});
const [Modal, modalApi] = useVbenModal({
  confirmText: $t('AbpTextTemplating.SaveContent'),
  fullscreen: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onClosed() {
    cancel('TemplateDefinitionModal has closed!');
  },
  onConfirm: async () => {
    await formApi.validateAndSubmitForm();
  },
  onOpenChange: async (isOpen: boolean) => {
    textTemplate.value = undefined;
    if (isOpen) {
      const textTemplateDefine = modalApi.getData<TextTemplateDefinitionDto>();
      textTemplate.value = textTemplateDefine;
      await onGet();
    }
  },
});
const [TemplateContentCurtuleModal, curtuleModalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./TemplateContentCurtuleModal.vue'),
  ),
});

async function onGet() {
  try {
    modalApi.setState({ loading: true });
    const textTemplateDefine = modalApi.getData<TextTemplateDefinitionDto>();
    const culture = textTemplateDefine.isInlineLocalized
      ? undefined
      : abpStore.application?.localization.currentCulture.cultureName;
    const dto = await getApi({ culture, name: textTemplateDefine.name });
    formApi.setFieldValue('content', dto.content);
  } finally {
    modalApi.setState({ loading: false });
  }
}

function onRestoreToDefault() {
  AntdvModal.confirm({
    centered: true,
    content: $t('AbpTextTemplating.RestoreToDefaultMessage'),
    onOk: async () => {
      await restoreToDefaultApi(textTemplate.value!.name, {});
      message.success($t('AbpTextTemplating.TemplateContentRestoredToDefault'));
      await onGet();
    },
    title: $t('AbpTextTemplating.RestoreToDefault'),
  });
}

function onCustomizePerCulture() {
  curtuleModalApi.setData(textTemplate.value);
  curtuleModalApi.open();
}

async function onSubmit(values: Record<string, string>) {
  try {
    modalApi.setState({ submitting: true });
    const textTemplateDefine = modalApi.getData<TextTemplateDefinitionDto>();
    const culture = textTemplateDefine.isInlineLocalized
      ? undefined
      : abpStore.application?.localization.currentCulture.cultureName;
    const dto = await updateApi(textTemplateDefine.name, {
      content: values.content!,
      culture,
    });
    emits('change', dto);
    message.success($t('AbpTextTemplating.TemplateContentUpdated'));
    modalApi.close();
  } finally {
    modalApi.setState({ submitting: false });
  }
}
</script>

<template>
  <Modal :title="$t('AbpTextTemplating.EditContents')">
    <Alert v-if="textTemplate?.isInlineLocalized" type="warning">
      <template #message>
        <MarkdownViewer
          :value="$t('AbpTextTemplating.InlineContentDescription')"
        />
      </template>
    </Alert>
    <Card :title="getCardTitle">
      <template #extra>
        <div class="flex flex-row gap-2">
          <Button danger type="primary" @click="onRestoreToDefault">
            {{ $t('AbpTextTemplating.RestoreToDefault') }}
          </Button>
          <Button type="dashed" @click="onCustomizePerCulture">
            {{ $t('AbpTextTemplating.CustomizePerCulture') }}
          </Button>
        </div>
      </template>
      <Form />
    </Card>
    <TemplateContentCurtuleModal :title="getCardTitle" />
  </Modal>
</template>

<style scoped></style>
