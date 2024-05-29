<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :title="t('routes.dashboard.workbench.menus.manager')"
    :width="500"
    @ok="handleSubmit"
  >
    <Form
      ref="formElRef"
      :model="formModel"
      :rules="formRules"
      :label-col="{ span: 6 }"
      :wrapper-col="{ span: 17 }"
    >
      <Form.Item :label="t('routes.dashboard.workbench.menus.selectMenu')" name="menuId">
        <VTreeSelect
          allowClear
          treeIcon
          :tree-data="menuTreeData"
          :fieldNames="{
            label: 'displayName',
            value: 'id',
          }"
          v-model:value="formModel.menuId"
          @select="handleMenuChange"
        >
          <template #title="item">
            <VIcon v-if="item.meta?.icon" :icon="item.meta.icon" />
            <span>{{ item.meta?.title ?? item.displayName }}</span>
          </template>
        </VTreeSelect>
      </Form.Item>
      <Form.Item :label="t('routes.dashboard.workbench.menus.selectColor')" name="color">
        <VColorPicker v-model:pureColor="formModel.color" format="hex" />
        <span>({{ formModel.color }})</span>
      </Form.Item>
      <Form.Item :label="t('routes.dashboard.workbench.menus.defineAliasName')" name="aliasName">
        <VInput v-model:value="formModel.aliasName" autocomplete="off" />
      </Form.Item>
      <Form.Item :label="t('routes.dashboard.workbench.menus.defineIcon')" name="icon">
        <IconPicker v-model:value="formModel.icon" :onlyDefineIcons="false" />
      </Form.Item>
    </Form>
  </BasicModal>
</template>

<script lang="ts" setup>
  import { reactive, ref, unref } from 'vue';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { Form, Input, TreeSelect } from 'ant-design-vue';
  import { ColorPicker } from 'vue3-colorpicker';
  import { Icon } from '/@/components/Icon';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { IconPicker } from '/@/components/Icon';
  import { getMenuList } from '/@/api/sys/menu';
  import { listToTree } from '/@/utils/helper/treeHelper';

  const VTreeSelect = TreeSelect;
  const VColorPicker = ColorPicker;
  const VIcon = Icon;
  const VInput = Input;

  const emits = defineEmits(['change', 'register']);
  const { t } = useI18n();
  const formModel = reactive({
    menuId: '',
    icon: '',
    color: '#000000',
    aliasName: '',
  });
  const formRules = reactive({
    menuId: {
      required: true,
    },
  });
  const formElRef = ref<any>(null);
  const menuTreeData = ref<any[]>([]);
  const radio = ref(false);
  const defaultCheckedKeys = ref<string[]>([]);
  const [registerModal, { closeModal }] = useModalInner((props) => {
    init(props);
    fetchMyMenus();
  });

  function init(props) {
    formModel.icon = '';
    formModel.menuId = '';
    formModel.aliasName = '';
    formModel.color = '#000000';
    radio.value = props.radio ?? false;
    defaultCheckedKeys.value = props.defaultCheckedKeys ?? [];
  }

  function fetchMyMenus() {
    getMenuList().then((res) => {
      const treeData = listToTree(res.items, { id: 'id', pid: 'parentId' });
      menuTreeData.value = treeData;
    });
  }

  function handleMenuChange(_, item) {
    formModel.icon = item.meta?.icon;
    formModel.aliasName = item.meta?.title ?? item.displayName;
  }

  function handleSubmit() {
    const formEl = unref(formElRef);
    formEl?.validate().then(() => {
      emits('change', formModel);
      closeModal();
    });
  }
</script>

<style lang="less">
  @import 'vue3-colorpicker/style.css';
</style>
