<template>
  <BasicModal :title="L('ManageSecret')" @register="registerModal" @ok="handleSubmit">
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script setup lang="ts">
  import { reactive } from 'vue';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { update } from '/@/api/openiddict/open-iddict-application';
  import { OpenIddictApplicationUpdateDto } from '/@/api/openiddict/open-iddict-application/model';

  const state = reactive<{
    application: Recordable;
  }>({
    application: {},
  });
  const emits = defineEmits(['change', 'register']);
  const { createMessage } = useMessage();
  const { L } = useLocalization(['AbpOpenIddict', 'AbpUi']);
  const [registerForm, { validate, resetFields }] = useForm({
    layout: 'vertical',
    showActionButtonGroup: false,
    schemas: [
      {
        field: 'clientSecret',
        component: 'Input',
        label: L('DisplayName:ClientSecret'),
        colProps: { span: 24 },
        required: true,
      },
    ],
  });
  const [registerModal, { changeOkLoading, closeModal }] = useModalInner((data) => {
    state.application = data;
  });

  function handleSubmit() {
    validate().then((input) => {
      changeOkLoading(true);
      update(state.application.id, {
        ...(state.application as OpenIddictApplicationUpdateDto),
        clientSecret: input.clientSecret,
      })
        .then((dto) => {
          createMessage.success(L('Successful'));
          resetFields();
          emits('change', dto);
          closeModal();
        })
        .finally(() => {
          changeOkLoading(false);
        });
    });
  }
</script>

<style scoped></style>
