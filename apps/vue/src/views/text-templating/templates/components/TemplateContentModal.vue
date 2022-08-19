<template>
  <div>
    <BasicModal
      @register="registerModal"
      :title="L('EditContents')"
      :default-fullscreen="true"
      :can-fullscreen="false"
      :ok-text="L('SaveContent')"
      @ok="handleSubmit"
    >
      <Alert v-if="isInlineLocalized" style="margin-bottom: 15px;" type="warning">
        <template #message>
          <MarkdownViewer :value="L('InlineContentDescription')" />
        </template>
      </Alert>
      <Card :title="getCardTitle">
        <template v-if="buttonEnabled" #extra>
          <Button danger type="primary" style="margin-right: 15px;" @click="handleRestoreToDefault">{{ L('RestoreToDefault') }}</Button>
          <Button type="dashed" @click="handleCustomizePerCulture">{{ L('CustomizePerCulture') }}</Button>
        </template>
        <BasicForm @register="registerForm" />
      </Card>
    </BasicModal>
    <TemplateContentCultureModal @register="registerCultureModal" />
  </div>
</template>

<script lang="ts" setup>
  import { computed, ref, unref, nextTick } from 'vue';
  import { Alert, Button, Card } from 'ant-design-vue';
  import { BasicForm, useForm } from '/@/components/Form';
  import { MarkdownViewer } from '/@/components/Markdown';
  import { BasicModal, useModal, useModalInner } from '/@/components/Modal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { TextTemplateDefinition } from '/@/api/text-templating/templates/model';
  import { getContent, restoreToDefault, update } from '/@/api/text-templating/templates';
  import TemplateContentCultureModal from './TemplateContentCultureModal.vue';

  const { L } = useLocalization('AbpTextTemplating');
  const { createConfirm, createMessage } = useMessage();
  const textTemplateRef = ref<TextTemplateDefinition>();
  const buttonEnabled = computed(() => {
    const textTemplate = unref(textTemplateRef);
    if (textTemplate && textTemplate.name) {
      return true;
    }
    return false;
  });
  const isInlineLocalized = computed(() => {
    const textTemplate = unref(textTemplateRef);
    return textTemplate?.isInlineLocalized === true;
  });
  const getCardTitle = computed(() => {
    const textTemplate = unref(textTemplateRef);
    return `${L('DisplayName:Name')}: ${textTemplate?.name}(${textTemplate?.displayName})`
  });
  const [registerForm, { resetFields, setFieldsValue, validate }] = useForm({
    layout: 'vertical',
    showActionButtonGroup: false,
    schemas: [
     {
        field: 'content',
        component: 'InputTextArea',
        label: L('DisplayName:Content'),
        colProps: { span: 24 },
        required: true,
        componentProps: {
          autoSize: {
            minRows: 15,
            maxRows: 50,
          },
          showCount: true,
        },
      },
    ],
  });
  const [registerModal, { changeOkLoading }] = useModalInner((data) => {
    textTemplateRef.value = data;
    nextTick(() => {
      resetFields();
      fetchContent(data.name);
    });
  });
  const [registerCultureModal, { openModal: openCultureModal }] = useModal();

  function fetchContent(name: string) {
    getContent({
      name: name,
    }).then((res) => {
      setFieldsValue(res);
    });
  }

  function handleCustomizePerCulture() {
    openCultureModal(true, unref(textTemplateRef));
  }

  function handleRestoreToDefault() {
    createConfirm({
      iconType: 'warning',
      title: L('RestoreToDefault'),
      content: L('RestoreToDefaultMessage'),
      onOk: () => {
        return new Promise((resolve, reject) => {
          const textTemplate = unref(textTemplateRef);
          restoreToDefault({ name: textTemplate!.name }).then(() => {
            createMessage.success(L('TemplateContentRestoredToDefault'));
            fetchContent(textTemplate!.name);
            return resolve(textTemplate!.name);
          }).catch((error) => {
            return reject(error);
          });
        });
      },
    });
  }

  function handleSubmit() {
    validate().then((input) => {
      changeOkLoading(true);
      const textTemplate = unref(textTemplateRef);
      update({
        name: textTemplate!.name,
        content: input.content,
      }).then(() => {
        createMessage.success(L('TemplateContentUpdated'));
      }).finally(() => {
        changeOkLoading(false);
      });
    });
  }
</script>
