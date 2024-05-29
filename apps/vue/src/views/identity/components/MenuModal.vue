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
        <FormItem :label="L('Menu:SetStartup')">
          <TreeSelect
            :tree-data="menuTreeRef"
            :field-names="{
              label: 'displayName',
              children: 'children',
              value: 'id',
            }"
            :allow-clear="true"
            v-model:value="startupMenuRef"
          />
        </FormItem>
        <FormItem :label="L('DisplayName:Menus')">
          <BasicTree
            :checkable="true"
            :check-strictly="true"
            :clickRowToExpand="true"
            :tree-data="menuTreeRef"
            :field-names="replaceFields"
            :checked-keys="defaultCheckedRef"
            @check="handleCheck"
          />
        </FormItem>
      </Form>
    </Card>
  </BasicModal>
</template>

<script lang="ts" setup>
  import { ref, unref, watch } from 'vue';
  import { Card, Form, Select, TreeSelect } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicTree } from '/@/components/Tree';
  import { getByName } from '/@/api/platform/datas';
  import { Menu } from '/@/api/platform/menus/model';
  import { getAll } from '/@/api/platform/menus';
  import { listToTree } from '/@/utils/helper/treeHelper';

  const FormItem = Form.Item;

  const emits = defineEmits(['change', 'change:startup', 'register']);
  const props = defineProps({
    loading: {
      type: Boolean,
      default: false,
    },
    getMenuApi: {
      type: Function as PropType<(...args) => Promise<ListResultDto<Menu>>>,
      required: true,
      default: new Promise<ListResultDto<Menu>>((resolve) => {
        return resolve({
          items: [],
        });
      }),
    },
  });

  const { L } = useLocalization('AppPlatform');
  const identityRef = ref('');
  const frameworkRef = ref('');
  const startupMenuRef = ref('');
  const menuTreeRef = ref<any[]>([]);
  const defaultCheckedRef = ref<any[]>([]);
  const checkedRef = ref<string[]>([]);
  const replaceFields = {
    key: 'id',
    title: 'displayName',
    children: 'children',
    value: 'id',
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
    startupMenuRef.value = '';
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
      const startupMenu = res.items.find((item) => item.startup);
      if (startupMenu) {
        startupMenuRef.value = startupMenu.id;
      }
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
    emits('change', unref(identityRef), unref(checkedRef));
    if (unref(startupMenuRef)) {
      emits('change:startup', unref(identityRef), unref(startupMenuRef));
    }
  }
</script>
