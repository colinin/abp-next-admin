<template>
  <BasicModal
    @register="registerModal"
    @visible-change="handleVisibleChange"
    :width="660"
    :height="500"
    :title="L('Menu:Manage')"
    :show-ok-btn="menuTreeRef.length > 0"
    @ok="handleSubmit"
  >
    <Card>
      <Form :labelCol="{ span: 4 }" :wrapperCol="{ span: 18 }" layout="horizontal">
        <FormItem :label="L('DisplayName:UIFramework')">
          <Select v-model:value="frameworkRef" :options="optionsRef" @select="handleSelect" />
        </FormItem>
        <FormItem :label="L('DisplayName:Menus')">
          <BasicTree
            :checkable="true"
            :check-strictly="true"
            :tree-data="menuTreeRef"
            :replace-fields="replaceFields"
            :checked-keys="defaultCheckedRef"
            @check="handleCheck"
          />
        </FormItem>
      </Form>
    </Card>
  </BasicModal>
</template>

<script lang="ts">
  import { defineComponent, ref, unref, watch } from 'vue';
  import { Card, Form, Select } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicTree } from '/@/components/Tree';
  import { getByName } from '/@/api/platform/dataDic';
  import { MenuListResult } from '/@/api/platform/model/menuModel';
  import { getAll } from '/@/api/platform/menu';
  import { listToTree } from '/@/utils/helper/treeHelper';
  export default defineComponent({
    name: 'MenuModal',
    components: {
      BasicModal,
      BasicTree,
      Card,
      Form,
      FormItem: Form.Item,
      Select,
    },
    props: {
      loading: {
        type: Boolean,
        default: false,
      },
      getMenuApi: {
        type: Function as PropType<(...args) => Promise<MenuListResult>>,
        required: true,
        default: new Promise<MenuListResult>((resolve) => {
          return resolve({
            items: [],
          });
        }),
      },
    },
    emits: ['change', 'register'],
    setup(props, { emit }) {
      const { L } = useLocalization('AppPlatform');
      const identityRef = ref('');
      const frameworkRef = ref('');
      const menuTreeRef = ref<any[]>([]);
      const defaultCheckedRef = ref<any[]>([]);
      const checkedRef = ref<string[]>([]);
      const replaceFields = {
        key: 'id',
        title: 'displayName',
        children: 'children',
      };
      const optionsRef = ref<
        {
          key: string;
          label: string;
          value: string;
        }[]
      >([]);
      const [registerModal, { changeOkLoading }] = useModalInner((record) => {
        identityRef.value = record.identity;
        optionsRef.value = [];
        frameworkRef.value = '';
        checkedRef.value = [];
        defaultCheckedRef.value = [];
        menuTreeRef.value = [];
      });

      watch(
        () => unref(props).loading,
        (busy) => {
          changeOkLoading(busy);
        },
      );

      function handleSelect(value) {
        getAll({
          filter: '',
          sorting: '',
          framework: value,
        }).then((res) => {
          menuTreeRef.value = listToTree(res.items, { id: 'id', pid: 'parentId' });
        });
        props.getMenuApi(unref(identityRef), value).then((res) => {
          checkedRef.value = res.items.map((item) => item.id);
          defaultCheckedRef.value = checkedRef.value;
        });
      }

      function handleVisibleChange(visible) {
        if (visible) {
          getByName('UI Framework').then((res) => {
            optionsRef.value = res.items.map((item) => {
              return {
                key: item.id,
                label: item.displayName,
                value: item.name,
              };
            });
          });
        }
      }

      function handleCheck(checkedKeys) {
        checkedRef.value = checkedKeys.checked;
      }

      function handleSubmit() {
        emit('change', unref(identityRef), unref(checkedRef));
      }

      return {
        L,
        menuTreeRef,
        defaultCheckedRef,
        replaceFields,
        frameworkRef,
        optionsRef,
        registerModal,
        handleSelect,
        handleVisibleChange,
        handleCheck,
        handleSubmit,
      };
    },
  });
</script>
