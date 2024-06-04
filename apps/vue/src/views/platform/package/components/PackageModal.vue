<template>
  <BasicModal
    :title="L('Packages')"
    :can-fullscreen="false"
    :show-ok-btn="true"
    :width="800"
    :height="500"
    @register="registerModal"
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
  import { getModalFormSchemas } from '../datas/ModalData';
  import { formatToDateTime } from '/@/utils/dateUtil';
  import {
    GetAsyncById,
    CreateAsyncByInput,
    UpdateAsyncByIdAndInput,
  } from '/@/api/platform/package';

  const emits = defineEmits(['change', 'register']);

  const { createMessage } = useMessage();
  const { L } = useLocalization(['Platform', 'AbpUi']);
  const [registerForm, { setFieldsValue, resetFields, validate }] = useForm({
    layout: 'vertical',
    showActionButtonGroup: false,
    schemas: getModalFormSchemas(),
    transformDateFunc: (date) => {
      return date ? formatToDateTime(date) : '';
    },
  });
  const [registerModal, { closeModal }] = useModalInner((data) => {
    nextTick(() => {
      resetFields();
      if (data.id) {
        fetchEntity(data.id);
      }
    });
  });

  function fetchEntity(id: string) {
    GetAsyncById(id).then((dto) => {
      setFieldsValue(dto);
    });
  }

  function handleSubmit() {
    validate().then((input) => {
      const api = input.id ? UpdateAsyncByIdAndInput(input.id, input) : CreateAsyncByInput(input);

      api.then((dto) => {
        createMessage.success(L('SuccessfullySaved'));
        emits('change', dto);
        closeModal();
      });
    });
  }
</script>
