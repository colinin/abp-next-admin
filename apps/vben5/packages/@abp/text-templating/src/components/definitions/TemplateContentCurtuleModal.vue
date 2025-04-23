<script setup lang="ts">
import type { ExtendedFormApi } from '@vben/common-ui';

import type { TextTemplateContentDto } from '../../types';
import type { TextTemplateDefinitionDto } from '../../types/definitions';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useAbpStore } from '@abp/core';
import { Modal as AntdvModal, Button, Card, message } from 'ant-design-vue';

import { useTemplateContentsApi } from '../../api';

defineProps<{
  title: string;
}>();
const emits = defineEmits<{
  (event: 'change', data: TextTemplateContentDto): void;
}>();
const abpStore = useAbpStore();
const { getApi, restoreToDefaultApi, updateApi } = useTemplateContentsApi();

const [SourceForm, sourceFormApi] = useVbenForm({
  commonConfig: {
    colon: true,
    componentProps: {
      class: 'w-full',
    },
    disabled: true,
  },
  layout: 'vertical',
  schema: [
    {
      component: 'Select',
      componentProps: {
        fieldNames: {
          label: 'displayName',
          value: 'cultureName',
        },
        onChange: (val?: string) => onCultureChange(sourceFormApi, val),
      },
      fieldName: 'culture',
      label: $t('AbpTextTemplating.BaseCultureName'),
      rules: 'required',
    },
    {
      component: 'Textarea',
      componentProps: {
        autoSize: {
          minRows: 20,
        },
        showCount: true,
      },
      fieldName: 'content',
      label: $t('AbpTextTemplating.BaseContent'),
      rules: 'required',
    },
  ],
  showDefaultActions: false,
});
const [TargetForm, targetFormApi] = useVbenForm({
  commonConfig: {
    colon: true,
    componentProps: {
      class: 'w-full',
    },
  },
  handleSubmit: onSubmit,
  layout: 'vertical',
  schema: [
    {
      component: 'Select',
      componentProps: {
        fieldNames: {
          label: 'displayName',
          value: 'cultureName',
        },
        onChange: (val?: string) => onCultureChange(targetFormApi, val),
      },
      fieldName: 'culture',
      label: $t('AbpTextTemplating.TargetCultureName'),
      rules: 'required',
    },
    {
      component: 'Textarea',
      componentProps: {
        autoSize: {
          minRows: 20,
        },
        showCount: true,
      },
      fieldName: 'content',
      label: $t('AbpTextTemplating.TargetContent'),
      rules: 'required',
    },
  ],
  showDefaultActions: false,
});
const [Modal, modalApi] = useVbenModal({
  confirmText: $t('AbpTextTemplating.SaveContent'),
  fullscreen: true,
  fullscreenButton: false,
  async onConfirm() {
    await targetFormApi.validateAndSubmitForm();
  },
  async onOpenChange(isOpen) {
    if (isOpen) {
      await onInit();
    }
  },
});

async function onInit() {
  try {
    modalApi.setState({ loading: true });
    const culture =
      abpStore.application?.localization.currentCulture.cultureName;
    const languages = abpStore.application?.localization.languages ?? [];
    sourceFormApi.updateSchema([
      {
        componentProps: {
          options: languages,
        },
        fieldName: 'culture',
      },
    ]);
    targetFormApi.updateSchema([
      {
        componentProps: {
          options: languages,
        },
        fieldName: 'culture',
      },
    ]);
    const { name } = modalApi.getData<TextTemplateDefinitionDto>();
    const { content } = await getApi({
      culture,
      name,
    });
    sourceFormApi.setValues({
      content,
      culture,
    });
  } finally {
    modalApi.setState({ loading: false });
  }
}

async function onCultureChange(formApi: ExtendedFormApi, culture?: string) {
  const { name } = modalApi.getData<TextTemplateDefinitionDto>();
  const { content } = await getApi({
    culture,
    name,
  });
  formApi.setValues({
    content,
    culture,
  });
}

async function onSubmit(values: Record<string, string>) {
  try {
    modalApi.setState({ submitting: true });
    const { name } = modalApi.getData<TextTemplateDefinitionDto>();
    const dto = await updateApi(name, {
      content: values.content!,
      culture: values.culture,
    });
    emits('change', dto);
    message.success($t('AbpTextTemplating.TemplateContentUpdated'));
    modalApi.close();
  } finally {
    modalApi.setState({ submitting: false });
  }
}

function onRestoreToDefault() {
  AntdvModal.confirm({
    centered: true,
    content: $t('AbpTextTemplating.RestoreToDefaultMessage'),
    onOk: async () => {
      const { name } = modalApi.getData<TextTemplateDefinitionDto>();
      const formValues = await sourceFormApi.getValues();
      await restoreToDefaultApi(name, {
        culture: formValues.culture,
      });
      message.success($t('AbpTextTemplating.TemplateContentRestoredToDefault'));
      await onInit();
    },
    title: $t('AbpTextTemplating.RestoreToDefault'),
  });
}
</script>

<template>
  <Modal :title="$t('AbpTextTemplating.EditContents')">
    <Card :title="title">
      <template #extra>
        <Button danger type="primary" @click="onRestoreToDefault">
          {{ $t('AbpTextTemplating.RestoreToDefault') }}
        </Button>
      </template>
      <div class="flex flex-row gap-3">
        <div class="w-1/2">
          <SourceForm />
        </div>
        <div class="w-1/2">
          <TargetForm />
        </div>
      </div>
    </Card>
  </Modal>
</template>

<style scoped></style>
