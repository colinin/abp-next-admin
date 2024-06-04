<template>
  <Card :title="title" size="small">
    <template #extra>
      <a-button type="primary" @click="openDataModal(true, {})">{{ L('Data:AddNew') }}</a-button>
    </template>
    <div :style="getContentStyle" ref="contentWrapRef">
      <ScrollContainer ref="contentScrollRef">
        <BasicTree
          :show-line="false"
          :show-icon="false"
          :block-node="true"
          :tree-data="treeData"
          :field-names="replaceFields"
          defaultExpandLevel="1"
          :before-right-click="getContentMenus"
          @select="handleNodeChange"
        />
      </ScrollContainer>
    </div>
    <DataModal @register="registerDataModal" @change="onLoadAllDataDic" />
    <DataItemModal @register="registerItemModal" @change="(dataId) => handleNodeChange([dataId])" />
  </Card>
</template>

<script lang="ts" setup>
  import type { CSSProperties } from 'vue';
  import { computed, ref, onMounted } from 'vue';
  import { ScrollContainer } from '/@/components/Container';
  import { useContentHeight } from '/@/hooks/web/useContentHeight';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { Card } from 'ant-design-vue';
  import { useModal } from '/@/components/Modal';
  import { BasicTree, ContextMenuItem } from '/@/components/Tree/index';
  import { listToTree } from '/@/utils/helper/treeHelper';
  import { getAll, remove } from '/@/api/platform/datas';
  import DataModal from './DataModal.vue';
  import DataItemModal from './DataItemModal.vue';

  const emits = defineEmits(['change', 'append-item']);

  const { createMessage, createConfirm } = useMessage();
  const { L } = useLocalization(['AppPlatform', 'AbpUi']);
  const title = L('DisplayName:DataDictionary');
  const contentWrapRef = ref<any>();
  const contentScrollRef = ref<any>();
  const treeData = ref<any[]>([]);
  const replaceFields = ref({
    title: 'displayName',
    key: 'id',
    value: 'id',
  });
  const [registerItemModal, { openModal: openItemModal }] = useModal();
  const [registerDataModal, { openModal: openDataModal }] = useModal();
  const getFlag = computed(() => true);
  const { contentHeight } = useContentHeight(getFlag, contentWrapRef, [], []);
  const getContentStyle = computed((): CSSProperties => {
    return {
      width: '100%',
      height: `${contentHeight.value}px`,
    };
  });

  onMounted(onLoadAllDataDic);

  function getContentMenus(node: any): ContextMenuItem[] {
    return [
      {
        label: L('Data:Edit'),
        handler: () => {
          openDataModal(true, { id: node.eventKey });
        },
        icon: 'ant-design:edit-outlined',
      },
      {
        label: L('Data:AddChildren'),
        handler: () => {
          openDataModal(true, { parentId: node.eventKey });
        },
        icon: 'ant-design:plus-outlined',
      },
      {
        label: L('Data:AppendItem'),
        handler: () => {
          openItemModal(true, { dataId: node.eventKey }, true);
        },
        icon: 'ant-design:plus-square-outlined',
      },
      {
        label: L('Data:Delete'),
        handler: () => {
          createConfirm({
            iconType: 'warning',
            title: L('AreYouSure'),
            content: L('ItemWillBeDeletedMessage'),
            okCancel: true,
            onOk: () => {
              return remove(node.eventKey).then(() => {
                createMessage.success(L('SuccessfullyDeleted'));
                onLoadAllDataDic();
              });
            },
          });
        },
        icon: 'ant-design:delete-outlined',
      },
    ];
  }

  function onLoadAllDataDic() {
    getAll().then((res) => {
      treeData.value = listToTree(res.items, {
        pid: 'parentId',
      });
    });
  }

  function handleNodeChange(selectKeys: String[]) {
    if (selectKeys.length > 0) {
      emits('change', selectKeys[0]);
    }
  }
</script>
