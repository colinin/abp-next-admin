<template>
  <div class="container">
    <BasicForm @register="registerForm" />
  </div>
</template>

<script lang="ts" setup>
  import { onMounted } from 'vue';
  import { useRoute } from 'vue-router';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { BasicForm, useForm } from '/@/components/Form/index';
  import { confirmEmail } from '/@/api/account/profiles';

  const route = useRoute();
  const { t } = useI18n();
  const [registerForm, { setFieldsValue, getFieldsValue }] = useForm({
    labelWidth: 120,
    showResetButton: false,
    showSubmitButton: true,
    submitButtonOptions: {
      text: t('common.okText'),
    },
    submitFunc: handleSubmit,
    showAdvancedButton: false,
    showActionButtonGroup: true,
    schemas: [
      {
        field: 'userId',
        component: 'Input',
        label: 'userId',
        show: false,
      },
      {
        field: 'confirmToken',
        component: 'Input',
        label: 'confirmToken',
        show: false,
      },
      {
        field: 'returnUrl',
        component: 'Input',
        label: 'returnUrl',
        show: false,
      },
    ],
  });

  onMounted(() => {
    setFieldsValue(route.query);
  });

  function handleSubmit(): Promise<void> {
    var input = getFieldsValue();
    return confirmEmail({
      userId: input.userId,
      confirmToken: input.confirmToken,
    }).then(() => {
      if (input.returnUrl) {
        window.location.href = input.returnUrl;
      }
    });
  }
</script>

<style scoped></style>
