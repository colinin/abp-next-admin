<template>
  <div :class="{ node: true, root: isRoot || !show, 'node-error-state': showError }">
    <div v-if="show" @click="$emit('selected')" :class="{ 'node-body': true, error: showError }">
      <div>
        <div class="node-body-header" :style="{ 'background-color': headerBgc }">
          <div style="margin-right: 5px">
            <slot name="headerIcon"></slot>
          </div>
          <Ellipsis class="name" hover-tip :content="title" />
          <CloseOutlined
            v-if="!isRoot"
            class="icon-close"
            style="float: right"
            @click="$emit('delNode')"
          />
        </div>
        <div class="node-body-content">
          <slot name="leftIcon"></slot>
          <span class="placeholder" v-if="(content || '').trim() === ''">{{ placeholder }}</span>
          <Ellipsis :row="3" :content="content" v-else />
          <RightOutlined class="i" />
        </div>
        <div class="node-error" v-if="showError">
          <Tooltip placement="right">
            <WarningOutlined />
            <template #title>
              <span>{{ errorInfo }}</span>
            </template>
          </Tooltip>
        </div>
      </div>
    </div>
    <div class="node-footer">
      <div class="btn">
        <InsertButton @insertNode="(type) => $emit('insertNode', type)"></InsertButton>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { Tooltip } from 'ant-design-vue';
  import { RightOutlined, CloseOutlined, WarningOutlined } from '@ant-design/icons-vue';
  import Ellipsis from '../Ellipsis.vue';
  import InsertButton from '../InsertButton.vue';

  defineEmits(['delNode', 'insertNode', 'selected']);
  defineProps({
    //是否为根节点
    isRoot: {
      type: Boolean,
      default: false,
    },
    //是否显示节点体
    show: {
      type: Boolean,
      default: true,
    },
    //节点内容区域文字
    content: {
      type: String,
      default: '',
    },
    title: {
      type: String,
      default: '标题',
    },
    placeholder: {
      type: String,
      default: '请设置',
    },
    //头部背景色
    headerBgc: {
      type: String,
      default: '#576a95',
    },
    //是否显示错误状态
    showError: {
      type: Boolean,
      default: false,
    },
    errorInfo: {
      type: String,
      default: '无信息',
    },
  });
</script>

<style lang="less" scoped>
  .root {
    &:before {
      display: none !important;
    }
  }

  .node-error-state {
    .node-body {
      box-shadow: 0px 0px 5px 0px #f56c6c !important;
    }
  }

  .node {
    padding: 0 10px;
    //width: 220px;
    position: relative;

    &:before {
      content: '';
      position: absolute;
      top: -12px;
      left: 50%;
      -webkit-transform: translateX(-50%);
      transform: translateX(-50%);
      width: 0;
      border-style: solid;
      border-width: 8px 6px 4px;
      border-color: #cacaca transparent transparent;
      background: #f5f5f7;
    }

    .node-body {
      cursor: pointer;
      max-height: 120px;
      position: relative;
      border-radius: 5px;
      background-color: white;
      box-shadow: 0px 0px 5px 0px #d8d8d8;

      &:hover {
        box-shadow: 0px 0px 3px 0px @primary-color;

        .node-body-header {
          .icon-close {
            display: inline;
            font-size: medium;
          }
        }
      }

      .node-body-header {
        display: flex;
        border-top-left-radius: 5px;
        border-top-right-radius: 5px;
        padding: 5px 15px;
        color: white;
        font-size: xx-small;

        .icon-close {
          display: none;
        }

        .name {
          height: 14px;
          width: 150px;
          display: inline-block;
        }
      }

      .node-body-content {
        padding: 18px;
        color: #656363;
        font-size: 14px;

        .i {
          position: absolute;
          top: 55%;
          right: 5px;
          font-size: medium;
        }

        .placeholder {
          color: #8c8c8c;
        }
      }

      .node-error {
        position: absolute;
        right: -40px;
        top: 20px;
        font-size: 25px;
        color: #f56c6c;
      }
    }

    .node-footer {
      position: relative;

      .btn {
        width: 100%;
        display: flex;
        padding: 20px 0 32px;
        justify-content: center;
      }

      :deep(.a-button) {
        height: 32px;
      }

      &::before {
        content: '';
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        z-index: -1;
        margin: auto;
        width: 2px;
        height: 100%;
        background-color: #cacaca;
      }
    }
  }
</style>
