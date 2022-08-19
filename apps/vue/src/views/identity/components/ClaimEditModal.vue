<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :title="L('ManageClaim')"
    :width="600"
    @ok="handleSubmit"
  >
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { nextTick } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { getActivedList } from '/@/api/identity/claim';
  import { isFunction } from '/@/utils/is';

  const emits = defineEmits(['register', 'change']);
  const props = defineProps({
    identity: {
      type: String,
      required: true,
    },
    createApi: {
      type: Function as PropType<(...args: any) => Promise<void>>,
      required: true,
    },
    updateApi: {
      type: Function as PropType<(...args: any) => Promise<void>>,
      required: true,
    },
  });
  const { createMessage } = useMessage();
  const { L } = useLocalization('AbpIdentity');
  const [registerForm, { validate, setFieldsValue, resetFields }] = useForm({
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
        field: 'claimType',
        component: 'ApiSelect',
        label: L('DisplayName:ClaimType'),
        colProps: { span: 24 },
        required: true,
        componentProps: {
          api: () => getActivedList(),
          resultField: 'items',
          labelField: 'name',
          valueField: 'name',
        },
      },
      {
        field: 'claimValue',
        component: 'Input',
        label: L('DisplayName:ClaimValue'),
        colProps: { span: 24 },
        required: true,
        dynamicDisabled: ({ values }) => {
          return values.id ? true : false;
        },
      },
      {
        field: 'newClaimValue',
        component: 'Input',
        label: L('IdentityClaim:NewValue'),
        colProps: { span: 24 },
        required: true,
        ifShow: ({ values }) => {
          return values.id ? true : false;
        },
      },
    ]
  });
  const [registerModal, { changeLoading, closeModal }] = useModalInner((claim) => {
    console.log(claim);
    nextTick(() => {
      resetFields();
      setFieldsValue(claim);
    });
  });

  function handleSubmit() {
    validate().then((input) => {
      changeLoading(true);
      if (isFunction(props.createApi) && isFunction(props.updateApi)) {
        const api = !input.id ? props.createApi(props.identity, input) : props.updateApi(props.identity, input);
        api.then(() => {
          createMessage.success(L('Successful'));
          closeModal();
          emits('change');
        }).finally(() => {
          changeLoading(false);
        });
      }
    });
  }
</script>
