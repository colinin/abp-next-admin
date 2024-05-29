<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :title="L('SetPassword')"
    @ok="handleSubmit"
  >
    <BasicForm ref="formElRef" @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { ref, unref } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicForm, useForm, FormActionType } from '/@/components/Form';
  import { usePassword } from '../hooks/usePassword';
  import { changePassword } from '/@/api/identity/users';

  const { createMessage } = useMessage();
  const { L } = useLocalization('AbpIdentity');
  const userIdRef = ref('');
  const formElRef = ref<Nullable<FormActionType>>(null);
  const { formSchemas } = usePassword(formElRef);
  const [registerModal, { changeOkLoading, closeModal }] = useModalInner((val) => {
    userIdRef.value = val;
  });
  const [registerForm, { validate }] = useForm({
    schemas: formSchemas,
    showActionButtonGroup: false,
    actionColOptions: {
      span: 24,
    },
  });

  function handleSubmit() {
    const userId = unref(userIdRef);
    if (userId) {
      validate().then((res) => {
        changeOkLoading(true);
        changePassword(userId, {
          password: res.password,
        })
          .then(() => {
            createMessage.success(L('Successful'));
            closeModal();
          })
          .finally(() => {
            changeOkLoading(true);
          });
      });
    }
  }
</script>
