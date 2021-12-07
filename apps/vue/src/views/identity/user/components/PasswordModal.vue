<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :title="L('SetPassword')"
    @ok="handleSubmit"
  >
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts">
  import { defineComponent, ref, unref } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicForm, useForm } from '/@/components/Form';
  import { usePassword } from '../hooks/usePassword';
  import { changePassword } from '/@/api/identity/user';

  export default defineComponent({
    name: 'PasswordModal',
    components: { BasicModal, BasicForm },
    setup() {
      const { L } = useLocalization('AbpIdentity');
      const userIdRef = ref('');
      const { formSchemas } = usePassword();
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

      return {
        L,
        registerModal,
        closeModal,
        registerForm,
        getFieldsValue,
        validate,
        userIdRef,
      };
    },
    methods: {
      handleSubmit() {
        const userId = unref(this.userIdRef);
        if (userId) {
          this.validate().then((res) => {
            changePassword(userId, {
              newPassword: res.password,
            }).then(() => {
              this.closeModal();
            });
          });
        }
      },
    },
  });
</script>
