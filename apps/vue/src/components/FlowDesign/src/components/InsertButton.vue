<template>
  <Popover placement="bottomLeft" title="添加流程节点" width="350" trigger="click">
    <template #content>
      <div class="node-select">
        <div @click="addApprovalNode">
          <CheckOutlined class="icon" style="color: rgb(255, 148, 62)" />
          <span>审批人</span>
        </div>
        <div @click="addCcNode">
          <SendOutlined class="icon" style="color: rgb(50, 150, 250)" />
          <span>抄送人</span>
        </div>
        <div @click="addConditionsNode">
          <ShareAltOutlined class="icon" style="color: rgb(21, 188, 131)" />
          <span>条件分支</span>
        </div>
        <div @click="addConcurrentsNode">
          <ThunderboltOutlined class="icon" style="color: #718dff" />
          <span>并行分支</span>
        </div>
        <div @click="addDelayNode">
          <ClockCircleOutlined class="icon" style="color: #f25643" />
          <span>延迟等待</span>
        </div>
        <div @click="addTriggerNode">
          <SettingOutlined class="icon" style="color: #15bc83" />
          <span>触发器</span>
        </div>
        <div @click="addHttpEndPointNode">
          <GlobalOutlined class="icon" style="color: #2f65c3" />
          <span>HttpEndPoint</span>
        </div>
      </div>
    </template>
    <Button type="primary" size="small" shape="circle">
      <template #icon>
        <PlusOutlined />
      </template>
    </Button>
  </Popover>
</template>

<script lang="ts" setup>
  //import { computed } from 'vue';
  import { Button, Popover } from 'ant-design-vue';
  import {
    CheckOutlined,
    SendOutlined,
    ShareAltOutlined,
    ThunderboltOutlined,
    ClockCircleOutlined,
    SettingOutlined,
    PlusOutlined,
    GlobalOutlined,
  } from '@ant-design/icons-vue';
  //import { useFlowStoreWithOut } from '/@/store/modules/flow';

  const emits = defineEmits(['insertNode']);

  //const flowStore = useFlowStoreWithOut();

  // const selectedNode = computed(() => {
  //   return flowStore.selectedNode;
  // });

  function addApprovalNode() {
    emits('insertNode', 'APPROVAL');
  }

  function addCcNode() {
    emits('insertNode', 'CC');
  }

  function addDelayNode() {
    emits('insertNode', 'DELAY');
  }

  function addConditionsNode() {
    emits('insertNode', 'CONDITIONS');
  }

  function addConcurrentsNode() {
    emits('insertNode', 'CONCURRENTS');
  }

  function addTriggerNode() {
    emits('insertNode', 'TRIGGER');
  }

  function addHttpEndPointNode() {
    emits('insertNode', 'HTTPENDPOINT');
  }
</script>

<style lang="less" scoped>
  .node-select {
    div {
      display: inline-block;
      margin: 5px 5px;
      cursor: pointer;
      padding: 10px 15px;
      border: 1px solid #f8f9f9;
      background-color: #f8f9f9;
      border-radius: 10px;
      width: 130px;
      position: relative;

      span {
        position: absolute;
        left: 65px;
        top: 18px;
      }

      &:hover {
        background-color: #fff;
        box-shadow: 0 0 8px 2px #d6d6d6;
      }

      .icon {
        font-size: 25px;
        padding: 5px;
        border: 1px solid #dedfdf;
        border-radius: 14px;
      }
    }
  }
</style>
