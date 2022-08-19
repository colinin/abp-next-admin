<template>
  <BasicModal
    @register="registerModal"
    :title="L('EditContents')"
    :default-fullscreen="true"
    :can-fullscreen="false"
    :ok-text="L('SaveContent')"
    @ok="handleSubmit"
  >
    <Card :title="getCardTitle">
      <template v-if="buttonEnabled" #extra>
        <Button danger type="primary" style="margin-right: 15px;" @click="handleRestoreToDefault">{{ L('RestoreToDefault') }}</Button>
      </template>
      <BasicForm @register="registerForm" />
    </Card>
  </BasicModal>
</template>

<script lang="ts" setup>
  import { computed, ref, unref, nextTick } from 'vue';
  import { Button, Card } from 'ant-design-vue';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { TextTemplateDefinition } from '/@/api/text-templating/templates/model';
  import { getContent, restoreToDefault, update } from '/@/api/text-templating/templates';
  import { useAbpStoreWithOut } from '/@/store/modules/abp';

  const abpStore = useAbpStoreWithOut();
  const { L } = useLocalization('AbpTextTemplating');
  const { createConfirm, createMessage } = useMessage();
  const { localization } = abpStore.getApplication;
  const textTemplateRef = ref<TextTemplateDefinition>();
  const buttonEnabled = computed(() => {
    const textTemplate = unref(textTemplateRef);
    if (textTemplate && textTemplate.name) {
      return true;
    }
    return false;
  });
  const getCardTitle = computed(() => {
    const textTemplate = unref(textTemplateRef);
    return `${L('DisplayName:Name')}: ${textTemplate?.name}(${textTemplate?.displayName})`
  });
  const [registerForm, { resetFields, setFieldsValue, validateFields, clearValidate }] = useForm({
    layout: 'vertical',
    showActionButtonGroup: false,
    schemas: [
     {
        field: 'baseCultureName',
        component: 'Select',
        label: L('BaseCultureName'),
        colProps: { span: 12 },
        required: true,
        componentProps: {
          options: localization.languages.map((l) => {
            return {
              label: l.displayName,
              value: l.cultureName,
            };
          }),
          onChange: (selectedValue: string) => handleCultureChange('baseContent', selectedValue),
        },
      },
      {
        field: 'targetCultureName',
        component: 'Select',
        label: L('TargetCultureName'),
        colProps: { span: 11, offset: 1 },
        required: true,
        componentProps: {
          options: localization.languages.map((l) => {
            return {
              label: l.displayName,
              value: l.cultureName,
            };
          }),
          onChange: (selectedValue: string) => handleCultureChange('targetContent', selectedValue),
        },
      },
      {
        field: 'baseContent',
        component: 'InputTextArea',
        label: L('BaseContent'),
        colProps: { span: 12 },
        componentProps: {
          readonly: true,
          autoSize: {
            minRows: 15,
            maxRows: 50,
          },
          showCount: true,
        },
      },
      {
        field: 'targetContent',
        component: 'InputTextArea',
        label: L('TargetContent'),
        colProps: { span: 11, offset: 1 },
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
      fetchDefaultContent(data.name);
    });
  });

  function fetchDefaultContent(name: string) {
    getContent({
      name: name,
      culture: localization.currentCulture.name,
    }).then((res) => {
      setFieldsValue({
        baseCultureName: res.culture,
        baseContent: res.content,
      });
    });
  }

  function handleCultureChange(setField: string, selectedValue: string) {
    getContent({
      name: textTemplateRef.value!.name,
      culture: selectedValue,
    }).then((res) => {
      setFieldsValue({
        [setField]: res.content,
      });
    });
  }

  async function handleRestoreToDefault() {
    await clearValidate();
    validateFields(['baseCultureName']).then((input) => {
      createConfirm({
        iconType: 'warning',
        title: L('RestoreToDefault'),
        content: L('RestoreToDefaultMessage'),
        onOk: () => {
          return new Promise((resolve, reject) => {
            const textTemplate = unref(textTemplateRef);
            restoreToDefault({
              name: textTemplate!.name,
              culture: input.baseCultureName,
            }).then(() => {
              createMessage.success(L('TemplateContentRestoredToDefault'));
              return resolve(textTemplate!.name);
            }).catch((error) => {
              return reject(error);
            });
          });
        },
      });
    });
  }

  async function handleSubmit() {
    await clearValidate();
    validateFields(['targetCultureName', 'targetContent']).then((input) => {
      changeOkLoading(true);
      const textTemplate = unref(textTemplateRef);
      update({
        name: textTemplate!.name,
        culture: input.targetCultureName,
        content: input.targetContent,
      }).then(() => {
        createMessage.success(L('TemplateContentUpdated'));
      }).finally(() => {
        changeOkLoading(false);
      });
    });
  }
</script>
