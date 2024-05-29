<template>
  <Layout class="main">
    <div class="scale">
      <Button
        class="button"
        size="small"
        @click="state.scale += 10"
        :disabled="state.scale >= 150"
        shape="circle"
      >
        <template #icon>
          <PlusOutlined />
        </template>
      </Button>
      <span>{{ state.scale }}%</span>
      <Button
        class="button"
        size="small"
        @click="state.scale -= 10"
        :disabled="state.scale <= 40"
        shape="circle"
      >
        <template #icon>
          <MinusOutlined />
        </template>
      </Button>
      <Button @click="validate">校验流程</Button>
    </div>
    <div class="design" :style="'transform: scale(' + state.scale / 100 + ');'">
      <ProcessTree ref="processTreeRef" @selectedNode="nodeSelected" />
    </div>
    <Drawer
      :width="500"
      :title="selectedNode.name"
      :visible="state.showConfig"
      destroy-on-close
      @close="() => (state.showConfig = false)"
    >
      <div slot="title">
        <Input
          v-model="selectedNode.name"
          size="small"
          v-show="state.showInput"
          style="width: 300px"
          @blur="state.showInput = false"
        >
        </Input>
      </div>
      <div class="node-config-content">
        <NodeConfig />
      </div>
    </Drawer>
  </Layout>
</template>

<script setup lang="ts">
  import { computed, reactive, ref, unref, onMounted } from 'vue';
  import { Button, Drawer, Input, Layout } from 'ant-design-vue';
  import { PlusOutlined, MinusOutlined } from '@ant-design/icons-vue';
  import { useFlowStoreWithOut } from '/@/store/modules/flow';
  import ProcessTree from './process/ProcessTree.vue';
  import NodeConfig from './config/NodeConfig.vue';

  const flowStore = useFlowStoreWithOut();
  const state = reactive({
    scale: 100,
    selected: {} as any,
    showInput: false,
    showConfig: false,
  });
  const processTreeRef = ref<any>();
  const selectedNode = computed(() => flowStore.selectedNode);

  onMounted(loadInitFrom);

  function loadInitFrom() {
    flowStore.loadForm({
      formId: null,
      formName: '未命名表单',
      logo: {
        icon: 'home',
        background: '#1e90ff',
      },
      settings: {
        commiter: [],
        admin: [],
        sign: false,
        notify: {
          types: ['APP'],
          title: '消息通知标题',
        },
      },
      groupId: undefined,
      formItems: [],
      process: {
        id: 'root',
        parentId: null,
        type: 'ROOT',
        name: '发起人',
        desc: '任何人',
        props: {
          assignedUser: [],
          formPerms: [],
        },
        children: {},
      },
      remark: '备注说明',
    });
  }

  function validate() {
    const processTree = unref(processTreeRef);
    return processTree?.validateProcess();
  }

  function nodeSelected(node) {
    console.log('配置节点', node);
    state.showConfig = true;
  }

  defineExpose({
    validate,
  });
</script>

<style lang="less" scoped>
  .main {
    background: white;
    height: 100%;
  }

  .design {
    margin-top: 100px;
    display: flex;
    transform-origin: 50% 0px 0px;
  }

  .scale {
    z-index: 999;
    position: fixed;
    top: 80px;
    right: 40px;

    .button {
      margin: 0 10px;
    }

    span {
      font-size: 15px;
      color: #7a7a7a;
    }
  }

  .node-config-content {
    padding: 0 20px 20px;
  }

  :deep(.ant-drawer-body) {
    overflow-y: auto;
  }
</style>
