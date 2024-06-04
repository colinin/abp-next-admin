<template>
  <div :class="{ node: true, 'node-error-state': state.showError }">
    <div :class="{ 'node-body': true, error: state.showError }">
      <div class="node-body-left" @click="$emit('leftMove')" v-if="level > 1">
        <LeftOutlined />
      </div>
      <div class="node-body-main" @click="$emit('selected')">
        <div class="node-body-main-header">
          <Ellipsis class="title" hover-tip :content="config.name ? config.name : '条件' + level" />
          <span class="level">优先级{{ level }}</span>
          <span class="option">
            <Tooltip effect="dark" content="复制条件" placement="top">
              <CopyOutlined @click.stop="$emit('copy')" />
            </Tooltip>
            <CloseOutlined class="icon-close" @click.stop="$emit('delNode')" />
          </span>
        </div>
        <div class="node-body-main-content">
          <span class="placeholder" v-if="(content || '').trim() === ''">{{
            state.placeholder
          }}</span>
          <Ellipsis hoverTip :row="4" :content="content" v-else />
        </div>
      </div>
      <div class="node-body-right" @click="$emit('rightMove')" v-if="level < size">
        <RightOutlined />
      </div>
      <div class="node-error" v-if="state.showError">
        <Tooltip effect="dark" :content="state.errorInfo" placement="top">
          <WarningOutlined />
        </Tooltip>
      </div>
    </div>
    <div class="node-footer">
      <div class="btn">
        <InsertButton @insertNode="(type) => $emit('insertNode', type)"></InsertButton>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
  export default {
    name: 'Condition',
  };
</script>

<script setup lang="ts">
  import { computed, reactive } from 'vue';
  import { Tooltip } from 'ant-design-vue';
  import {
    RightOutlined,
    CloseOutlined,
    CopyOutlined,
    LeftOutlined,
    WarningOutlined,
  } from '@ant-design/icons-vue';
  import Ellipsis from '../Ellipsis.vue';
  import InsertButton from '../InsertButton.vue';

  defineEmits(['delNode', 'insertNode', 'leftMove', 'rightMove', 'selected']);
  const props = defineProps({
    config: {
      type: Object,
      default: () => {
        return {};
      },
    },
    //索引位置
    level: {
      type: Number,
      default: 1,
    },
    //条件数
    size: {
      type: Number,
      default: 0,
    },
  });
  const content = computed(() => {
    console.log('props', props);
    const groups = props.config.props.groups;
    let confitions: string[] = [];
    groups.forEach((group) => {
      let subConditions: string[] = [];
      group.conditions.forEach((subCondition) => {
        let subConditionStr = '';
        switch (subCondition.valueType) {
          case 'dept':
          case 'user':
            subConditionStr = `${subCondition.title}属于[${String(
              subCondition.value.map((u) => u.name),
            ).replaceAll(',', '. ')}]之一`;
            break;
          case 'number':
          case 'string':
            subConditionStr = getOrdinaryConditionContent(subCondition);
            break;
        }
        subConditions.push(subConditionStr);
      });
      //根据子条件关系构建描述
      let subConditionsStr = String(subConditions).replaceAll(
        ',',
        subConditions.length > 1
          ? group.groupType === 'AND'
            ? ') 且 ('
            : ') 或 ('
          : group.groupType === 'AND'
          ? ' 且 '
          : ' 或 ',
      );
      confitions.push(subConditions.length > 1 ? `(${subConditionsStr})` : subConditionsStr);
    });
    //构建最终描述
    return String(confitions).replaceAll(
      ',',
      props.config.props.groupsType === 'AND' ? ' 且 ' : ' 或 ',
    );
  });
  const state = reactive({
    groupNames: [] as string[],
    placeholder: '请设置条件',
    errorInfo: '',
    showError: false,
  });

  function getDefault(val, df) {
    return val && val !== '' ? val : df;
  }

  function getOrdinaryConditionContent(subCondition: any) {
    switch (subCondition.compare) {
      case 'IN':
        return `${subCondition.title}为[${String(subCondition.value).replaceAll(',', '、')}]中之一`;
      case 'B':
        return `${subCondition.value[0]} < ${subCondition.title} < ${subCondition.value[1]}`;
      case 'AB':
        return `${subCondition.value[0]} ≤ ${subCondition.title} < ${subCondition.value[1]}`;
      case 'BA':
        return `${subCondition.value[0]} < ${subCondition.title} ≤ ${subCondition.value[1]}`;
      case 'ABA':
        return `${subCondition.value[0]} ≤ ${subCondition.title} ≤ ${subCondition.value[1]}`;
      case '<=':
        return `${subCondition.title} ≤ ${getDefault(subCondition.value[0], ' ?')}`;
      case '>=':
        return `${subCondition.title} ≥ ${getDefault(subCondition.value[0], ' ?')}`;
      default:
        return `${subCondition.title}${subCondition.compare}${getDefault(
          subCondition.value[0],
          ' ?',
        )}`;
    }
  }

  function validate(err: string[]) {
    const thatProps = props.config.props;
    if (thatProps.groups.length <= 0) {
      state.showError = true;
      state.errorInfo = '请设置分支条件';
      err.push(`${props.config.name} 未设置条件`);
    } else {
      for (let i = 0; i < thatProps.groups.length; i++) {
        if (thatProps.groups[i].cids.length === 0) {
          state.showError = true;
          state.errorInfo = `请设置条件组${thatProps.groupNames[i]}内的条件`;
          err.push(`条件 ${props.config.name} 条件组${thatProps.groupNames[i]}内未设置条件`);
          break;
        } else {
          let conditions = thatProps.groups[i].conditions;
          for (let ci = 0; ci < conditions.length; ci++) {
            let subc = conditions[ci];
            if (subc.value.length === 0) {
              state.showError = true;
            } else {
              state.showError = false;
            }
            if (state.showError) {
              state.errorInfo = `请完善条件组${thatProps.groupNames[i]}内的${subc.title}条件`;
              err.push(
                `条件 ${props.config.name} 条件组${thatProps.groupNames[i]}内${subc.title}条件未完善`,
              );
              return false;
            }
          }
        }
      }
    }
    return !state.showError;
  }

  defineExpose({
    validate,
  });
</script>

<style lang="less" scoped>
  .node-error-state {
    .node-body {
      box-shadow: 0px 0px 5px 0px #f56c6c !important;
    }
  }

  .node {
    padding: 30px 55px 0;
    width: 330px;

    .node-body {
      cursor: pointer;
      min-height: 80px;
      max-height: 120px;
      position: relative;
      border-radius: 5px;
      background-color: white;
      box-shadow: 0px 0px 5px 0px #d8d8d8;

      &:hover {
        box-shadow: 0px 0px 3px 0px @primary-color;

        .node-body-left,
        .node-body-right {
          i {
            display: block !important;
          }
        }

        .node-body-main {
          .level {
            display: none !important;
          }

          .option {
            display: inline-block !important;
          }
        }
      }

      .node-body-left,
      .node-body-right {
        display: flex;
        align-items: center;
        position: absolute;
        height: 100%;

        i {
          display: none;
        }

        &:hover {
          background-color: #ececec;
        }
      }

      .node-body-left {
        left: 0;
      }

      .node-body-right {
        right: 0;
        top: 0;
      }

      .node-body-main {
        position: absolute;
        width: 188px;
        margin-left: 17px;
        display: inline-block;

        .node-body-main-header {
          padding: 10px 0px 5px;
          font-size: xx-small;
          position: relative;

          .title {
            color: #15bca3;
            display: inline-block;
            height: 14px;
            width: 125px;
          }

          .level {
            position: absolute;
            right: 15px;
            color: #888888;
          }

          .option {
            position: absolute;
            right: 0;
            display: none;
            font-size: medium;

            i {
              color: #888888;
              padding: 0 3px;
            }
          }
        }

        .node-body-main-content {
          padding: 6px;
          color: #656363;
          font-size: 14px;

          i {
            position: absolute;
            top: 55%;
            right: 10px;
            font-size: medium;
          }

          .placeholder {
            color: #8c8c8c;
          }
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
        height: 70px;
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
