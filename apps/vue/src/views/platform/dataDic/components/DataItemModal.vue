<template>
  <BasicModal v-bind="$attrs" @register="register" :title="title" :width="600" @ok="handleSubmit">
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { computed, ref, unref, watch } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { DataItem } from '/@/api/platform/datas/model';
  import { getDataItemFormSchemas } from './ModalData';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicForm, useForm } from '/@/components/Form/index';
  import { createItem, updateItem } from '/@/api/platform/datas';

  const emits = defineEmits(['change', 'register']);

  const dataItem = ref<DataItem>();
  const { createMessage } = useMessage();
  const { L } = useLocalization(['AppPlatform', 'AbpUi']);
  const schemas = getDataItemFormSchemas();

  watch(
    () => unref(dataItem),
    (item) => {
      resetFields();
      setFieldsValue(item!);
    },
    {
      immediate: false,
    },
  );

  const [registerForm, { validate, setFieldsValue, resetFields }] = useForm({
    labelWidth: 120,
    schemas,
    showActionButtonGroup: false,
    actionColOptions: {
      span: 24,
    },
  });

  const [register, { changeLoading, closeModal }] = useModalInner((dataVal) => {
    dataItem.value = dataVal;
  });

  const title = computed(() => {
    if (unref(dataItem)?.id) {
      return L('Data:EditItem');
    }
    return L('Data:AppendItem');
  });

  function handleSubmit() {
    validate().then((input) => {
      changeLoading(true);
      const api = input.id
        ? updateItem(input.dataId, input.name, input)
        : createItem(input.dataId, input);
      api
        .then(() => {
          createMessage.success(L('Successful'));
          emits('change', input.dataId);
          closeModal();
        })
        .finally(() => {
          changeLoading(false);
        });
    });
  }
</script>
