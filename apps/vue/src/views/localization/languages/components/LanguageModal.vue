<template>
  <BasicModal
    @register="registerModal"
    :title="L('Languages')"
    :can-fullscreen="false"
    :width="800"
    :height="500"
    @ok="handleSubmit"
  >
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { nextTick } from 'vue';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { getModalFormSchemas } from './ModalData';
  import { formatToDateTime } from '/@/utils/dateUtil';
  import { Language } from '/@/api/localization/languages/model';
  import { create, update } from '/@/api/localization/languages';

  const emits = defineEmits(['change', 'register']);

  const { createMessage } = useMessage();
  const { L } = useLocalization(['LocalizationManagement', 'AbpLocalization', 'AbpUi']);
  const [registerForm, { setFieldsValue, resetFields, validate }] = useForm({
    layout: 'vertical',
    showActionButtonGroup: false,
    schemas: getModalFormSchemas(),
    transformDateFunc: (date) => {
      return date ? formatToDateTime(date) : '';
    },
  });
  const [registerModal, { closeModal }] = useModalInner((data: Language) => {
    nextTick(() => {
      resetFields();
      setFieldsValue(data);
    });
  });

  function handleSubmit() {
    validate().then((input) => {
      const api = input.id
        ? update(input.cultureName, input)
        : create(input);
      api.then((dto) => {
        createMessage.success(L('SuccessfullySaved'));
        emits('change', dto);
        closeModal();
      });
    });
  }
</script>
