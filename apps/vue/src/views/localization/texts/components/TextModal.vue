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
  import { computed, defineComponent, ref, unref, watch } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { message } from 'ant-design-vue';
  import { BasicForm, FormSchema, FormActionType, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { Text } from '/@/api/localization/model/textsModel';
  import { get, getByCulture, create, update } from '/@/api/localization/texts';
  import { getAll as getLanguages } from '/@/api/localization/languages';
  import { getAll as getResources } from '/@/api/localization/resources';
  export default defineComponent({
    name: 'TextModal',
    components: { BasicForm, BasicModal },
    emits: ['change', 'register'],
    setup(_props, { emit }) {
      const { L } = useLocalization('LocalizationManagement', 'AbpUi');
      const idRef = ref<number | undefined>(undefined);
      const modelRef = ref<Nullable<Text>>(null);
      const formElRef = ref<Nullable<FormActionType>>(null);
      const formSchemas: FormSchema[] = [
        {
          field: 'id',
          component: 'Input',
          label: 'id',
          colProps: { span: 24 },
          ifShow: false,
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
                      id: undefined,
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
        },
        {
          field: 'value',
          component: 'InputTextArea',
          label: L('DisplayName:Value'),
          colProps: { span: 24 },
          required: true,
        },
      ];
      const [registerForm] = useForm({
        colon: true,
        labelWidth: 120,
        schemas: formSchemas,
        showActionButtonGroup: false,
        actionColOptions: {
          span: 24,
        },
      });
      const [registerModal, { closeModal, changeOkLoading }] = useModalInner((val) => {
        idRef.value = val.id;
      });
      const formTitle = computed(() => {
        const id = unref(modelRef)?.id;
        if (id && id > 0) {
          return L('EditByName', [unref(modelRef)?.key] as Recordable);
        }
        return L('Text:AddNew');
      });

      watch(
        () => unref(idRef),
        (id) => {
          const formEl = unref(formElRef);
          formEl?.resetFields();
          if (id) {
            get(id).then((res) => {
              modelRef.value = res;
              formEl?.setFieldsValue(res);
            });
          } else {
            modelRef.value = null;
          }
        },
      );

      function handleSubmit() {
        const formEl = unref(formElRef);
        formEl?.validate().then((input) => {
          changeOkLoading(true);
          const model = unref(modelRef);
          const api = model?.id ? update(model!.id, input) : create(input);
          api
            .then(() => {
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
