<template>
  <Form
    v-bind="getBindValue"
    :class="getFormClass"
    ref="formElRef"
    :model="formModel"
    @keypress.enter="handleEnterPress"
  >
    <Tabs
      v-model:activeKey="activedTabKey"
      :style="tabsStyle.style"
      :tabBarStyle="tabsStyle.tabBarStyle"
    >
      <!-- fix bug: forceRender 必须强制渲染，否则form验证会失效 -->
      <TabPane
        v-for="tabSchema in getTabSchema"
        :key="tabSchema.key"
        :tab="tabSchema.key"
        :forceRender="true"
      >
        <Row v-bind="getRow">
          <slot name="formHeader"></slot>
          <template v-for="schema in tabSchema.schemas" :key="schema.field">
            <slot v-if="schema.tabSlot" :name="schema.tabSlot"></slot>
            <FormItem
              v-else
              :tableAction="tableAction"
              :formActionType="formActionType"
              :schema="schema"
              :formProps="getProps"
              :allDefaultValues="defaultValueRef"
              :formModel="formModel"
              :setFormModel="setFormModel"
            >
              <template #[item]="data" v-for="item in Object.keys($slots)">
                <slot :name="item" v-bind="data || {}"></slot>
              </template>
            </FormItem>
          </template>
        </Row>
      </TabPane>
    </Tabs>

    <Row v-bind="getRow">
      <FormAction v-bind="getFormActionBindProps" @toggle-advanced="handleToggleAdvanced">
        <template
          #[item]="data"
          v-for="item in ['resetBefore', 'submitBefore', 'advanceBefore', 'advanceAfter']"
        >
          <slot :name="item" v-bind="data || {}"></slot>
        </template>
      </FormAction>
      <slot name="formFooter"></slot>
    </Row>
  </Form>
</template>

<script lang="ts">
  import type { TabFormActionType, TabFormProps, TabFormSchema } from './types/form';
  import type { AdvanceState } from './types/hooks';
  import type { Ref } from 'vue';

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
  import { useTabsStyle } from '/@/hooks/component/useStyles';
  import { useModalContext } from '/@/components/Modal';
  import { useDebounceFn } from '@vueuse/core';

  import { tabProps } from './props';
  import { useDesign } from '/@/hooks/web/useDesign';

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
    emits: ['advanced-change', 'reset', 'submit', 'register', 'field-value-change'],
    setup(props, { emit, attrs }) {
      const formModel = reactive<Recordable>({});
      const modalFn = useModalContext();
      const tabsStyle = useTabsStyle();

      const advanceState = reactive<AdvanceState>({
        isAdvanced: true,
        hideAdvanceBtn: false,
        isLoad: false,
        actionSpan: 6,
      });

      const defaultValueRef = ref<Recordable>({});
      const isInitedDefaultRef = ref(false);
      const activedTabKey = ref('');
      const propsRef = ref<Partial<TabFormProps>>({});
      const schemaRef = ref<Nullable<TabFormSchema[]>>(null);
      const formElRef = ref<Nullable<TabFormActionType>>(null);

      const { prefixCls } = useDesign('basic-form');

      // Get the basic configuration of the form
      const getProps = computed((): TabFormProps => {
        return { ...props, ...unref(propsRef) } as TabFormProps;
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
      const getRow = computed((): Recordable => {
        const { baseRowStyle = {}, rowProps } = unref(getProps);
        return {
          style: baseRowStyle,
          ...rowProps,
        };
      });

      const getBindValue = computed(
        () => ({ ...attrs, ...props, ...unref(getProps) } as Recordable),
      );

      const getSchema = computed((): TabFormSchema[] => {
        const schemas: TabFormSchema[] = unref(schemaRef) || (unref(getProps).schemas as any);
        for (const schema of schemas) {
          const { defaultValue, component } = schema;
          // handle date type
          if (defaultValue && dateItemType.includes(component)) {
            if (!Array.isArray(defaultValue)) {
              schema.defaultValue = dateUtil(defaultValue);
            } else {
              const def: any[] = [];
              defaultValue.forEach((item) => {
                def.push(dateUtil(item));
              });
              schema.defaultValue = def;
            }
          }
        }
        if (unref(getProps).showAdvancedButton) {
          return schemas.filter((schema) => schema.component !== 'Divider');
        } else {
          return schemas;
        }
      });

      const getTabSchema = computed((): { key: string; schemas: TabFormSchema[] }[] => {
        // const schemas = unref(getSchema);
        const tabSchemas: { key: string; schemas: TabFormSchema[] }[] = [];
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
        formElRef: formElRef as Ref<TabFormActionType>,
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
        removeSchemaByField,
        resetFields,
        scrollToField,
      } = useFormEvents({
        emit,
        getProps,
        formModel,
        getSchema,
        defaultValueRef,
        formElRef: formElRef as Ref<TabFormActionType>,
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
          const { model, schemas } = unref(getProps);
          if (schemas?.length) {
            activedTabKey.value = schemas[0].tab;
          }
          if (!model) return;
          setFieldsValue(model);
        },
        {
          immediate: true,
        },
      );

      watch(
        () => unref(getProps).schemas,
        (schemas) => {
          resetSchema(schemas ?? []);
          if (schemas?.length) {
            activedTabKey.value = schemas[0].tab;
          }
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
            activedTabKey.value = schema[0].tab;
          }
        },
      );

      watch(
        () => formModel,
        useDebounceFn(() => {
          unref(getProps).submitOnChange && handleSubmit();
        }, 300),
        { deep: true },
      );

      async function setProps(formProps: Partial<TabFormProps>): Promise<void> {
        propsRef.value = deepMerge(unref(propsRef) || {}, formProps);
      }

      function setFormModel(key: string, value: any) {
        formModel[key] = value;
        const { validateTrigger } = unref(getBindValue);
        if (!validateTrigger || validateTrigger === 'change') {
          validateFields([key]).catch((_) => {});
        }
        emit('field-value-change', key, value);
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

      function changeTab(tab: string) {
        activedTabKey.value = tab;
      }

      const formActionType: Partial<TabFormActionType> = {
        getFieldsValue,
        setFieldsValue,
        resetFields,
        updateSchema,
        resetSchema,
        setProps,
        removeSchemaByField,
        appendSchemaByField,
        clearValidate,
        validateFields,
        validate,
        submit: handleSubmit,
        scrollToField: scrollToField,
        changeTab: changeTab,
      };

      onMounted(() => {
        initDefault();
        emit('register', formActionType);
      });

      return {
        getBindValue,
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
        tabsStyle,
        formActionType: formActionType as any,
        setFormModel,
        getFormClass,
        getFormActionBindProps: computed(
          (): Recordable => ({ ...getProps.value, ...advanceState }),
        ),
        ...formActionType,
      };
    },
  });
</script>

<style lang="less">
  @prefix-cls: ~'@{namespace}-basic-form';

  .@{prefix-cls} {
    .ant-form-item {
      &-label label::after {
        margin: 0 6px 0 2px;
      }

      &-with-help {
        margin-bottom: 0;
      }

      &:not(.ant-form-item-with-help) {
        margin-bottom: 20px;
      }

      &.suffix-item {
        .ant-form-item-children {
          display: flex;
        }

        .ant-form-item-control {
          margin-top: 4px;
        }

        .suffix {
          display: inline-flex;
          padding-left: 6px;
          margin-top: 1px;
          line-height: 1;
          align-items: center;
        }
      }
    }

    .ant-form-explain {
      font-size: 14px;
    }

    &--compact {
      .ant-form-item {
        margin-bottom: 8px !important;
      }
    }
  }
</style>
