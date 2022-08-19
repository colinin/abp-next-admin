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

<script lang="ts">
  import { defineComponent, ref, unref } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicForm, useForm, FormActionType } from '/@/components/Form';
  import { usePassword } from '../hooks/usePassword';
  import { changePassword } from '/@/api/identity/user';

  export default defineComponent({
    name: 'PasswordModal',
    components: { BasicModal, BasicForm },
    setup() {
      const { L } = useLocalization('AbpIdentity');
      const userIdRef = ref('');
      const formElRef = ref<Nullable<FormActionType>>(null);
      const { formSchemas } = usePassword(formElRef);
      const [registerModal, { closeModal }] = useModalInner((val) => {
        userIdRef.value = val;
      });
      const [registerForm, { getFieldsValue, validate }] = useForm({
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
            changePassword(userId, {
              password: res.password,
            }).then(() => {
              closeModal();
            });
          });
        }
      }

      return {
        L,
        formElRef,
        registerModal,
        closeModal,
        registerForm,
        getFieldsValue,
        validate,
        userIdRef,
        handleSubmit,
      };
    },
  });
</script>
