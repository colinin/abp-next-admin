<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :title="t('routes.dashboard.workbench.menus.manager')"
    :width="500"
    @ok="handleSubmit"
  >
    <Form :model="formModel" ref="formElRef">
      <Form.Item :label-col="{ span: 3 }" :wrapper-col="{ span: 20 }" :label="t('routes.dashboard.workbench.menus.selectMenu')" name="menus">
        <ATree
          checkable
          checkStrictly
          :tree-data="menuTreeData"
          :fieldNames="{
            title: 'displayName',
            key: 'id',
          }"
          v-model:checkedKeys="formModel.menus"
          :selectedKeys="defaultCheckedKeys"
          @check="handleMenuCheck" />
      </Form.Item>
    </Form>
  </BasicModal>
</template>

<script lang="ts" setup>
  import { reactive, ref, unref } from 'vue';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { Form, Tree } from 'ant-design-vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { getMenuList } from '/@/api/sys/menu';
  import { listToTree } from '/@/utils/helper/treeHelper';

  const ATree = Tree;

  const emits = defineEmits(['change', 'change:keys', 'register']);
  const { t } = useI18n();
  const formModel = reactive({
    menus: [] as string[],
  });
  const formElRef = ref<any>(null);
  const menuTreeData = ref<any[]>([]);
  const radio = ref(false);
  const defaultCheckedKeys = ref<string[]>([]);
  const checkedMenus = ref<any[]>([]);
  const [registerModal, { closeModal }] = useModalInner((props) => {
    init(props);
    fetchMyMenus();
  });

  function init(props) {
    formModel.menus = [];
    checkedMenus.value = [];
    radio.value = props.radio ?? false;
    defaultCheckedKeys.value = props.defaultCheckedKeys ?? [];
  }

  function fetchMyMenus() {
    getMenuList().then((res) => {
      const treeData = listToTree(res.items, { id: 'id', pid: 'parentId' });
      menuTreeData.value = treeData;
    })
  }

  function handleSubmit() {
    const formEl = unref(formElRef);
    formEl?.validate().then(() => {
      emits('change:keys', formModel.menus);
      emits('change', checkedMenus.value);
      closeModal();
    });
  }

  function handleMenuCheck(checkedKeys, e) {
    if (checkedKeys.checked.length > 1 && radio.value) {
      const menu = checkedKeys.checked.pop();
      formModel.menus = [menu!];
    }
    checkedMenus.value = e.checkedNodes;
  }
</script>

<style lang="less" scoped>

</style>