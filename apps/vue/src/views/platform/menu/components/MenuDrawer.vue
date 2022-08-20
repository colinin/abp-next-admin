<template>
  <BasicDrawer
    v-bind="$attrs"
    @register="registerDrawer"
    showFooter
    :title="formTitle"
    width="50%"
    @ok="handleSubmit"
  >
    <TabForm
      ref="formElRef"
      :schemas="getFormSchemas"
      :label-width="120"
      :show-action-button-group="false"
      :base-col-props="{
        span: 24,
      }"
      :action-col-options="{
        span: 24,
      }"
    />
  </BasicDrawer>
</template>

<script lang="ts" setup>
  import { nextTick, ref } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { TabForm, FormActionType } from '/@/components/Form';
  import { BasicDrawer, useDrawerInner } from '/@/components/Drawer';
  import { basicProps } from './props';
  import { Menu } from '/@/api/platform/model/menuModel';
  import { useMenuFormContext } from '../hooks/useMenuFormContext';

  const emits = defineEmits(['change', 'register']);
  const props = defineProps(basicProps);

  const { createMessage } = useMessage();
  const { L } = useLocalization(['AppPlatform', 'AbpUi']);
  const menu = ref<Menu>({} as Menu);
  const framework = ref<string  | undefined>('');
  const formElRef = ref<Nullable<FormActionType>>(null);
  const { formTitle, getFormSchemas, handleFormSubmit, fetchLayoutResource } =
    useMenuFormContext({
      menuModel: menu,
      formElRef: formElRef,
      framework: framework,
    });

  const [registerDrawer, { setDrawerProps, closeDrawer }] = useDrawerInner((dataVal) => {
    menu.value = dataVal;
    framework.value = props.framework;
    nextTick(() => {
      setDrawerProps({ confirmLoading: false });
      fetchLayoutResource(dataVal.layoutId);
    });
  });

  function handleSubmit() {
    handleFormSubmit()?.then(() => {
      createMessage.success(L('Successful'));
      closeDrawer();
      emits('change');
    });
  }
</script>
