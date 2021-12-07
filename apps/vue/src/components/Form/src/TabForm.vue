<template>
  <Form
    v-bind="{ ...$attrs, ...$props, ...getProps }"
    :class="getFormClass"
    ref="formElRef"
    :model="formModel"
    @keypress.enter="handleEnterPress"
  >
    <Row v-bind="{ ...getRow }">
      <slot name="formHeader"></slot>
      <Tabs v-model="activedTabKey" style="width: 100%">
        <!-- fix bug: forceRender 必须强制渲染，否则form验证会失效 -->
        <TabPane
          v-for="tabSchema in getTabSchema"
          :key="tabSchema.key"
          :tab="tabSchema.key"
          :forceRender="true"
        >
          <template v-for="schema in tabSchema.schemas" :key="schema.field">
            <FormItem
              :tableAction="tableAction"
              :formActionType="formActionType"
              :schema="schema"
              :formProps="getProps"
              :allDefaultValues="defaultValueRef"
              :formModel="formModel"
              :setFormModel="setFormModel"
            >
              <template #[item]="data" v-for="item in Object.keys($slots)">
                <slot :name="item" v-bind="data"></slot>
              </template>
            </FormItem>
          </template>
        </TabPane>
      </Tabs>

      <FormAction v-bind="{ ...getProps, ...advanceState }" @toggle-advanced="handleToggleAdvanced">
        <template
          #[item]="data"
          v-for="item in ['resetBefore', 'submitBefore', 'advanceBefore', 'advanceAfter']"
        >
          <slot :name="item" v-bind="data"></slot>
        </template>
      </FormAction>
      <slot name="formFooter"></slot>
    </Row>
  </Form>
</template>

<script lang="ts">
  import type { FormActionType, FormProps, FormSchema, TabFormSchema } from './types/form';
  import type { AdvanceState } from './types/hooks';
  import type { CSSProperties, Ref } from 'vue';

  import { defineComponent, reactive, ref, computed, unref, onMounted, watch, nextTick } from 'vue';
  import { Form, Row, Tabs } from 'ant-design-vue';
  import FormItem from './components/FormItem.vue';
  import FormAction from './components/FormAction.vue';

  import { dateItemType } from './helper';
  import { dateUtil } from '/@/utils/dateUtil';
  import { groupBy } from '/@/utils/array';

  // import { cloneDeep } from 'lodash-es';
  import { deepMerge } from '/@/utils';

  import { useFormValues } from './hooks/useFormValues';
  import useAdvanced from './hooks/useAdvanced';
  import { useFormEvents } from './hooks/useFormEvents';
  import { createFormContext } from './hooks/useFormContext';
  import { useAutoFocus } from './hooks/useAutoFocus';
  import { useModalContext } from '/@/components/Modal';

  import { tabProps } from './props';
  import { useDesign } from '/@/hooks/web/useDesign';

  import type { RowProps } from 'ant-design-vue/lib/grid/Row';

  export default defineComponent({
    name: 'TabForm',
    components: {
      Form,
      FormItem,
      FormAction,
      Row,
      Tabs,
      TabPane: Tabs.TabPane,
    },
    props: tabProps,
    emits: ['advanced-change', 'reset', 'submit', 'register'],
    setup(props, { emit }) {
      const formModel = reactive<Recordable>({});
      const modalFn = useModalContext();

      const advanceState = reactive<AdvanceState>({
        isAdvanced: true,
        hideAdvanceBtn: false,
        isLoad: false,
        actionSpan: 6,
      });

      const defaultValueRef = ref<Recordable>({});
      const isInitedDefaultRef = ref(false);
      const activedTabKey = ref('');
      const propsRef = ref<Partial<FormProps>>({});
      const schemaRef = ref<Nullable<TabFormSchema[]>>(null);
      const formElRef = ref<Nullable<FormActionType>>(null);

      const { prefixCls } = useDesign('basic-form');

      // Get the basic configuration of the form
      const getProps = computed((): FormProps => {
        return { ...props, ...unref(propsRef) } as FormProps;
      });

      const getFormClass = computed(() => {
        return [
          prefixCls,
          {
            [`${prefixCls}--compact`]: unref(getProps).compact,
          },
        ];
      });

      // Get uniform row style and Row configuration for the entire form
      const getRow = computed((): CSSProperties | RowProps => {
        const { baseRowStyle = {}, rowProps } = unref(getProps);
        return {
          style: baseRowStyle,
          ...rowProps,
        };
      });

      const getSchema = computed((): TabFormSchema[] => {
        const schemas: TabFormSchema[] = unref(schemaRef) || (unref(getProps).schemas as any);
        for (const schema of schemas) {
          const { defaultValue, component } = schema;
          // handle date type
          if (defaultValue && dateItemType.includes(component)) {
            if (!Array.isArray(defaultValue)) {
              schema.defaultValue = dateUtil(defaultValue);
            } else {
              const def: moment.Moment[] = [];
              defaultValue.forEach((item) => {
                def.push(dateUtil(item));
              });
              schema.defaultValue = def;
            }
          }
        }
        return schemas as TabFormSchema[];
      });

      const getTabSchema = computed((): { key: string; schemas: FormSchema[] }[] => {
        // const schemas = unref(getSchema);
        const tabSchemas: { key: string; schemas: FormSchema[] }[] = [];
        const group = groupBy(getSchema.value, 'tab');
        Object.keys(group).forEach((key) => {
          tabSchemas.push({
            key: key,
            schemas: group[key],
          });
        });
        return tabSchemas;
      });

      const { handleToggleAdvanced } = useAdvanced({
        advanceState,
        emit,
        getProps,
        getSchema,
        formModel,
        defaultValueRef,
      });

      const { handleFormValues, initDefault } = useFormValues({
        getProps,
        defaultValueRef,
        getSchema,
        formModel,
      });

      useAutoFocus({
        getSchema,
        getProps,
        isInitedDefault: isInitedDefaultRef,
        formElRef: formElRef as Ref<FormActionType>,
      });

      const {
        handleSubmit,
        setFieldsValue,
        clearValidate,
        validate,
        validateFields,
        getFieldsValue,
        updateSchema,
        resetSchema,
        appendSchemaByField,
        removeSchemaByFiled,
        resetFields,
        scrollToField,
      } = useFormEvents({
        emit,
        getProps,
        formModel,
        getSchema,
        defaultValueRef,
        formElRef: formElRef as Ref<FormActionType>,
        schemaRef: schemaRef as Ref<TabFormSchema[]>,
        handleFormValues,
      });

      createFormContext({
        resetAction: resetFields,
        submitAction: handleSubmit,
      });

      watch(
        () => unref(getProps).model,
        () => {
          const { model } = unref(getProps);
          if (!model) return;
          setFieldsValue(model);
          activedTabKey.value = '';
        },
        {
          immediate: true,
        },
      );

      watch(
        () => unref(getProps).schemas,
        (schemas) => {
          resetSchema(schemas ?? []);
        },
      );

      watch(
        () => getSchema.value,
        (schema) => {
          nextTick(() => {
            //  Solve the problem of modal adaptive height calculation when the form is placed in the modal
            modalFn?.redoModalHeight?.();
          });
          if (unref(isInitedDefaultRef)) {
            return;
          }
          if (schema?.length) {
            initDefault();
            isInitedDefaultRef.value = true;
          }
        },
      );

      async function setProps(formProps: Partial<FormProps>): Promise<void> {
        propsRef.value = deepMerge(unref(propsRef) || {}, formProps);
      }

      function setFormModel(key: string, value: any) {
        formModel[key] = value;
      }

      function handleEnterPress(e: KeyboardEvent) {
        const { autoSubmitOnEnter } = unref(getProps);
        if (!autoSubmitOnEnter) return;
        if (e.key === 'Enter' && e.target && e.target instanceof HTMLElement) {
          const target: HTMLElement = e.target as HTMLElement;
          if (target && target.tagName && target.tagName.toUpperCase() == 'INPUT') {
            handleSubmit();
          }
        }
      }

      const formActionType: Partial<FormActionType> = {
        getFieldsValue,
        setFieldsValue,
        resetFields,
        updateSchema,
        resetSchema,
        setProps,
        removeSchemaByFiled,
        appendSchemaByField,
        clearValidate,
        validateFields,
        validate,
        submit: handleSubmit,
        scrollToField: scrollToField,
      };

      onMounted(() => {
        initDefault();
        emit('register', formActionType);
      });

      return {
        handleToggleAdvanced,
        handleEnterPress,
        activedTabKey,
        formModel,
        defaultValueRef,
        advanceState,
        getRow,
        getProps,
        formElRef,
        getSchema,
        getTabSchema,
        formActionType,
        setFormModel,
        prefixCls,
        getFormClass,
        ...formActionType,
      };
    },
  });
</script>
