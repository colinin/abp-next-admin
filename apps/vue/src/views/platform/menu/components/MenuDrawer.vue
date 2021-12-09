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

<script lang="ts">
  import { defineComponent, ref } from 'vue';

  import { TabForm, FormActionType } from '/@/components/Form';
  import { BasicDrawer, useDrawerInner } from '/@/components/Drawer';

  import { basicProps } from './props';
  import { Menu } from '/@/api/platform/model/menuModel';
  import { useMenuFormContext } from '../hooks/useMenuFormContext';

  export default defineComponent({
    name: 'MenuDrawer',
    components: {
      BasicDrawer,
      TabForm,
    },
    props: basicProps,
    emits: ['change', 'register'],
    setup(props) {
      const menu = ref<Menu>({} as Menu);
      const formElRef = ref<Nullable<FormActionType>>(null);
      const { formTitle, getFormSchemas, handleFormSubmit, warpParentRootMenu } =
        useMenuFormContext({
          menuModel: menu,
          formElRef: formElRef,
        });

      const [registerDrawer, { setDrawerProps, closeDrawer }] = useDrawerInner(async (dataVal) => {
        setDrawerProps({ confirmLoading: false });
        menu.value = dataVal;

        await warpParentRootMenu(props.framework);
      });

      return {
        formTitle,
        formElRef,
        handleFormSubmit,
        getFormSchemas,
        registerDrawer,
        closeDrawer,
      };
    },
    methods: {
      handleSubmit() {
        this.handleFormSubmit()?.then(() => {
          this.closeDrawer();
          this.$emit('change');
        });
      },
    },
  });
</script>
