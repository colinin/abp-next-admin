<template>
  <BasicModal
    v-bind="$attrs"
    :width="600"
    :height="300"
    :min-height="300"
    @register="registerModal"
    @ok="handleSubmit"
    :title="L('Client:Clone')"
  >
    <BasicForm ref="formElRef" @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { ref, unref } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicForm, useForm, FormActionType, FormSchema } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { clone } from '/@/api/identity-server/clients';

  const emits = defineEmits(['change', 'register']);

  const { createMessage } = useMessage();
  const { L } = useLocalization('AbpIdentityServer');
  const clientIdRef = ref('');
  const formElRef = ref<Nullable<FormActionType>>(null);
  const formSchemas: FormSchema[] = [
    {
      field: 'clientId',
      component: 'Input',
      label: L('Client:Id'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'clientName',
      component: 'Input',
      label: L('Name'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'description',
      component: 'InputTextArea',
      label: L('Description'),
      colProps: { span: 24 },
    },
    {
      field: 'copyAllowedGrantType',
      component: 'Checkbox',
      label: L('Clone:CopyAllowedGrantType'),
      labelWidth: 180,
      colProps: { span: 24 },
      defaultValue: true,
      renderComponentContent: L('Clone:CopyAllowedGrantType'),
    },
    {
      field: 'copyRedirectUri',
      component: 'Checkbox',
      label: L('Clone:CopyRedirectUri'),
      labelWidth: 180,
      colProps: { span: 24 },
      defaultValue: true,
      renderComponentContent: L('Clone:CopyRedirectUri'),
    },
    {
      field: 'copyAllowedScope',
      component: 'Checkbox',
      defaultValue: true,
      label: L('Clone:CopyAllowedScope'),
      labelWidth: 180,
      colProps: { span: 24 },
      renderComponentContent: L('Clone:CopyAllowedScope'),
    },
    {
      field: 'copyClaim',
      component: 'Checkbox',
      label: L('Clone:CopyClaim'),
      labelWidth: 180,
      colProps: { span: 24 },
      defaultValue: true,
      renderComponentContent: L('Clone:CopyClaim'),
    },
    {
      field: 'copySecret',
      component: 'Checkbox',
      label: L('Clone:CopySecret'),
      labelWidth: 180,
      colProps: { span: 24 },
      defaultValue: true,
      renderComponentContent: L('Clone:CopySecret'),
    },
    {
      field: 'copyAllowedCorsOrigin',
      component: 'Checkbox',
      label: L('Clone:CopyAllowedCorsOrigin'),
      labelWidth: 180,
      colProps: { span: 24 },
      defaultValue: true,
      renderComponentContent: L('Clone:CopyAllowedCorsOrigin'),
    },
    {
      field: 'copyPostLogoutRedirectUri',
      component: 'Checkbox',
      label: L('Clone:CopyPostLogoutRedirectUri'),
      labelWidth: 180,
      colProps: { span: 24 },
      defaultValue: true,
      renderComponentContent: L('Clone:CopyPostLogoutRedirectUri'),
    },
    {
      field: 'copyProperties',
      component: 'Checkbox',
      label: L('Clone:CopyProperties'),
      labelWidth: 180,
      colProps: { span: 24 },
      defaultValue: true,
      renderComponentContent: L('Clone:CopyProperties'),
    },
    {
      field: 'copyIdentityProviderRestriction',
      component: 'Checkbox',
      defaultValue: true,
      label: L('Clone:CopyIdentityProviderRestriction'),
      labelWidth: 180,
      colProps: { span: 24 },
      renderComponentContent: L('Clone:CopyIdentityProviderRestriction'),
    },
  ];
  const [registerForm] = useForm({
    labelWidth: 120,
    schemas: formSchemas,
    showActionButtonGroup: false,
  });
  const [registerModal, { closeModal, changeOkLoading }] = useModalInner((data) => {
    clientIdRef.value = data.id;
  });

  function handleSubmit() {
    const formEl = unref(formElRef);
    formEl?.validate().then((input) => {
      changeOkLoading(true);
      clone(unref(clientIdRef), input)
        .then(() => {
          createMessage.success(L('Successful'));
          emits('change');
          closeModal();
        })
        .finally(() => {
          changeOkLoading(false);
        });
    });
  }
</script>
