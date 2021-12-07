<template>
  <BasicModal
    v-bind="$attrs"
    @register="register"
    :title="title()"
    @ok="handleSubmit"
    @visible-change="handleVisible"
  >
    <BasicForm @register="registerForm" :model="data" />
  </BasicModal>
</template>

<script lang="ts">
  import { defineComponent, ref } from 'vue';

  import { useLocalization } from '/@/hooks/abp/useLocalization';

  import { Data } from '/@/api/platform/model/dataModel';
  import { getDateFormSchemas } from './ModalData';

  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicForm, useForm } from '/@/components/Form/index';
  export default defineComponent({
    name: 'DataModal',
    components: {
      BasicForm,
      BasicModal,
    },
    setup() {
      const data = ref<Data>();
      const { L } = useLocalization('AppPlatform');
      const schemas = getDateFormSchemas(data.value);

      const [registerForm, { validate, getFieldsValue, resetFields }] = useForm({
        labelWidth: 120,
        schemas,
        showActionButtonGroup: false,
        actionColOptions: {
          span: 24,
        },
      });
      const [register, { setModalProps }] = useModalInner((dataVal) => {
        console.log(dataVal);
        data.value = dataVal;
      });

      return {
        L,
        registerForm,
        register,
        data,
        setModalProps,
        validate,
        resetFields,
        getFieldsValue,
      };
    },
    computed: {
      title() {
        return () => {
          if (this.data && this.data.id) {
            return this.L('Data:Edit');
          }
          return this.L('Data:AddNew');
        };
      },
    },
    methods: {
      handleSubmit() {
        this.validate().then(() => {
          console.log(this.getFieldsValue());
          this.setModalProps({ loading: true, confirmLoading: true });
          setTimeout(() => {
            this.setModalProps({ loading: false, confirmLoading: false });
          }, 2000);
        });
      },
      handleVisible(visible: Boolean) {
        if (!visible) {
          this.resetFields();
        }
      },
    },
  });
</script>
