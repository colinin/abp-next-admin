<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    @ok="handleSubmit"
    :title="formTitle"
    :width="660"
    :min-height="280"
  >
    <BasicForm ref="formElRef" @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts">
  import { computed, defineComponent, ref, unref, nextTick } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { message } from 'ant-design-vue';
  import { BasicForm, FormSchema, FormActionType, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { Text } from '/@/api/localization/model/textsModel';
  import { getByCulture, setText } from '/@/api/localization/texts';
  import { getList as getLanguages } from '/@/api/localization/languages';
  import { getList as getResources } from '/@/api/localization/resources';
  export default defineComponent({
    name: 'TextModal',
    components: { BasicForm, BasicModal },
    emits: ['change', 'register'],
    setup(_props, { emit }) {
      const { L } = useLocalization(['LocalizationManagement', 'AbpUi']);
      const modelRef = ref<Nullable<Text>>(null);
      const formElRef = ref<Nullable<FormActionType>>(null);
      const formSchemas: FormSchema[] = [
        {
          field: 'id',
          component: 'Input',
          label: 'id',
          colProps: { span: 24 },
          show: false,
        },
        {
          field: 'cultureName',
          component: 'ApiSelect',
          label: L('DisplayName:TargetCultureName'),
          colProps: { span: 24 },
          required: true,
          componentProps: {
            api: () => getLanguages(),
            resultField: 'items',
            labelField: 'uiCultureName',
            valueField: 'cultureName',
            onChange: (key) => {
              // 当文化变更时，检查键值是否有目标值
              const model = unref(modelRef);
              if (model?.key) {
                getByCulture({
                  key: model!.key,
                  cultureName: key,
                  resourceName: model!.resourceName,
                }).then((res) => {
                  if (res) {
                    modelRef.value = res;
                  } else {
                    modelRef.value = {
                      value: '',
                      key: model!.key,
                      cultureName: key,
                      resourceName: model!.resourceName,
                    };
                  }
                  const formEl = unref(formElRef);
                  formEl?.setFieldsValue(modelRef.value);
                });
              }
            },
          },
        },
        {
          field: 'resourceName',
          component: 'ApiSelect',
          label: L('DisplayName:ResourceName'),
          colProps: { span: 24 },
          required: true,
          componentProps: {
            api: () => getResources(),
            resultField: 'items',
            labelField: 'displayName',
            valueField: 'name',
          },
          dynamicDisabled: ({ values }) => {
            return values.id ? true : false;
          },
        },
        {
          field: 'key',
          component: 'Input',
          label: L('DisplayName:Key'),
          colProps: { span: 24 },
          required: true,
          dynamicDisabled: ({ values }) => {
            return values.id ? true : false;
          },
        },
        {
          field: 'value',
          component: 'InputTextArea',
          label: L('DisplayName:Value'),
          colProps: { span: 24 },
          required: true,
          componentProps: {
            rows: 5,
            showCount: true,
          },
        },
      ];
      const [registerForm, { resetFields, setFieldsValue }] = useForm({
        colon: true,
        labelWidth: 120,
        schemas: formSchemas,
        showActionButtonGroup: false,
        actionColOptions: {
          span: 24,
        },
      });
      const [registerModal, { closeModal, changeOkLoading }] = useModalInner((val) => {
        modelRef.value = val;
        nextTick(() => {
          resetFields();
          setFieldsValue(val);
        });
      });
      const formTitle = computed(() => {
        const key = unref(modelRef)?.key;
        if (key) {
          return L('EditByName', [unref(modelRef)?.key] as Recordable);
        }
        return L('Text:AddNew');
      });

      function handleSubmit() {
        const formEl = unref(formElRef);
        formEl?.validate().then((input) => {
          changeOkLoading(true);
          setText(input).then(() => {
            emit('change');
            message.success(L('Successful'));
            formEl?.resetFields();
            closeModal();
          })
          .finally(() => {
            changeOkLoading(false);
          });
        });
      }

      return {
        L,
        formElRef,
        formTitle,
        registerForm,
        registerModal,
        handleSubmit,
      };
    },
  });
</script>
