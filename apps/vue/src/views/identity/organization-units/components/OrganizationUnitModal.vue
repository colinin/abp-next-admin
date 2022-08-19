<template>
  <BasicModal
    @register="registerModal"
    :title="L('OrganizationUnit')"
    @ok="handleSubmit"
  >
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { nextTick } from 'vue';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { get, create, update } from '/@/api/identity/organization-units';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  
  const emits = defineEmits(['register', 'change']);
  const { createMessage } = useMessage();
  const { L } = useLocalization('AbpIdentity');
  const [registerForm, { setFieldsValue, resetFields, validate }] = useForm({
    showActionButtonGroup: false,
    layout: 'vertical',
    schemas: [
      {
        field: 'id',
        component: 'Input',
        label: 'id',
        colProps: { span: 24 },
        show: false,
      },
      {
        field: 'parentId',
        component: 'Input',
        label: 'parentId',
        colProps: { span: 24 },
        defaultValue: undefined,
        show: false,
      },
      {
        field: 'displayName',
        component: 'Input',
        label: L('OrganizationUnit:DisplayName'),
        colProps: { span: 24 },
        required: true,
      },
    ],
  });
  const [registerModal, { changeLoading, closeModal }] = useModalInner((data) => {
    nextTick(() => {
      // reset
      resetFields();
      // parentId
      setFieldsValue(data);
      fetchOrganizationUnit(data.id);
    });
  });

  function fetchOrganizationUnit(id?: string) {
    if (id) {
      get(id).then((res) => {
        setFieldsValue(res);
      });
    }
  }

  function handleSubmit() {
    validate().then((input) => {
      changeLoading(true);
      const api = !input.id ? create(input) : update(input.id, input);
      api.then((ou) => {
        createMessage.success(L('Successful'));
        emits('change', ou);
        closeModal();
      }).finally(() => {
        changeLoading(false);
      });
    })
  }
</script>
