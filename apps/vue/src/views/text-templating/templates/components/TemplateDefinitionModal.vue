<template>
  <BasicModal
    @register="registerModal"
    :title="L('TextTemplates')"
    :width="800"
    :min-height="400"
    @ok="handleSubmit"
  >
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { reactive, nextTick } from 'vue';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { GetByNameAsyncByName, CreateAsyncByInput, UpdateAsyncByNameAndInput } from '/@/api/text-templating/definitions';
  import { getModalFormSchemas } from '../datas/ModalData';

  const emits = defineEmits(['register', 'change']);

  const state = reactive({
    isEdit: false
  });
  const { createMessage } = useMessage();
  const { L } = useLocalization(['AbpTextTemplating']);
  const [registerForm, { resetFields, setFieldsValue, validate, updateSchema }] = useForm({
    labelWidth: 150,
    showActionButtonGroup: false,
    schemas: getModalFormSchemas(),
  });
  const [registerModal, { changeLoading, changeOkLoading, closeModal }] = useModalInner((data) => {
    nextTick(() => {
      fetch(data?.name);
    });
  });

  function fetch(name?: string) {
    state.isEdit = false;
    resetFields();
    if (!name) {
      updateSchema({
        field: 'name',
        dynamicDisabled: state.isEdit,
      });
      return;
    }
    changeLoading(true);
    changeOkLoading(true);
    GetByNameAsyncByName(name).then((res) => {
      state.isEdit = true;
      updateSchema({
        field: 'name',
        dynamicDisabled: state.isEdit,
      });
      setFieldsValue(res);
      if (res.formatedDisplayName) {
        // L:XXX,YYY
        const splitChars = res.formatedDisplayName.split(',');
        if (splitChars.length >= 2 && splitChars[0].startsWith('L:')) {
          const resource = splitChars[0].substring(2);
          setFieldsValue({
            resource: resource,
            text: splitChars[1],
          });
        }
      }
    }).finally(() => {
      changeLoading(false);
      changeOkLoading(false);
    });
  }
  
  function handleSubmit() {
    validate().then((input) => {
      input.displayName = `L:${input.resource},${input.text}`;
      changeLoading(true);
      changeOkLoading(true);
      const submitApi = state.isEdit
        ? UpdateAsyncByNameAndInput(input.name, input)
        : CreateAsyncByInput(input);
      submitApi.then((res) => {
        setFieldsValue(res);
        createMessage.success(L('Successful'));
        emits('change', res);
        closeModal();
      }).finally(() => {
        changeLoading(false);
        changeOkLoading(false);
      });
    });
  }
</script>

<style lang="less" scoped>

</style>