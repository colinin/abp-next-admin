<template>
  <Modal
    :width="width"
    :title="title"
    :mask-closable="clickClose"
    :destroy-on-close="closeFree"
    v-model:visible="state.visible"
    @ok="selectOk"
  >
    <div class="picker">
      <div class="candidate" v-loading="state.loading">
        <div v-if="type !== 'role'">
          <InputSearch
            v-model="state.search"
            @search="searchUser"
            style="width: 95%"
            size="small"
            allow-clear
            placeholder="搜索人员，支持拼音、姓名"
          />
          <div v-show="!showUsers">
            <Ellipsis
              hoverTip
              style="height: 18px; color: #8c8c8c; padding: 5px 0 0"
              :row="1"
              :content="deptStackStr"
            >
              <!-- <i slot="pre" class="el-icon-office-building"></i> -->
              <BuildOutlined>
                <slot name="pre"></slot>
              </BuildOutlined>
            </Ellipsis>
            <div style="margin-top: 5px">
              <Checkbox
                v-model="state.checkAll"
                @change="handleCheckAllChange"
                :disabled="!multiple"
                >全选</Checkbox
              >
              <span v-show="state.deptStack.length > 0" class="top-dept" @click="beforeNode"
                >上一级</span
              >
            </div>
          </div>
        </div>
        <div class="role-header" v-else>
          <div>系统角色</div>
        </div>
        <div class="org-items" :style="type === 'role' ? 'height: 350px' : ''">
          <Empty :image-size="100" description="似乎没有数据" v-show="orgs.length === 0" />
          <div
            v-for="(org, index) in orgs"
            :key="index"
            :class="orgItemClass(org)"
            @click="selectChange(org)"
          >
            <Checkbox v-model="org.selected" :disabled="disableDept(org)"></Checkbox>
            <div v-if="org.type === 'dept'">
              <!-- <i class="el-icon-folder-opened"></i> -->
              <FolderOpenOutlined />
              <span class="name">{{ org.name }}</span>
              <span
                @click.stop="nextNode(org)"
                :class="`next-dept${org.selected ? '-disable' : ''}`"
              >
                <!-- <i class="iconfont icon-map-site"></i>下级 -->
                <ApartmentOutlined />
                下级
              </span>
            </div>
            <div v-else-if="org.type === 'user'" style="display: flex; align-items: center">
              <Avatar :size="35" :src="org.avatar" v-if="!isNullOrWhiteSpace(org.avatar)" />
              <span v-else class="avatar">{{ getShortName(org.name) }}</span>
              <span class="name">{{ org.name }}</span>
            </div>
            <div style="display: inline-block" v-else>
              <!-- <i class="iconfont icon-bumen"></i> -->
              <TeamOutlined />
              <span class="name">{{ org.name }}</span>
            </div>
          </div>
        </div>
      </div>
      <div class="selected">
        <div class="count">
          <span>已选 {{ state.select.length }} 项</span>
          <span @click="clearSelected">清空</span>
        </div>
        <div class="org-items" style="height: 350px">
          <Empty
            :image-size="100"
            description="请点击左侧列表选择数据"
            v-show="state.select.length === 0"
          />
          <div v-for="(org, index) in state.select" :key="index" :class="orgItemClass(org)">
            <div v-if="org.type === 'dept'">
              <!-- <i class="el-icon-folder-opened"></i> -->
              <FolderOpenOutlined />
              <span style="position: static" class="name">{{ org.name }}</span>
            </div>
            <div v-else-if="org.type === 'user'" style="display: flex; align-items: center">
              <Avatar :size="35" :src="org.avatar" v-if="!isNullOrWhiteSpace(org.avatar)" />
              <span v-else class="avatar">{{ getShortName(org.name) }}</span>
              <span class="name">{{ org.name }}</span>
            </div>
            <div v-else>
              <!-- <i class="iconfont icon-bumen"></i> -->
              <TeamOutlined />
              <span class="name">{{ org.name }}</span>
            </div>
            <!-- <i class="el-icon-close" @click="noSelected(index)"></i> -->
            <CloseOutlined @click="noSelected(index)" />
          </div>
        </div>
      </div>
    </div>
  </Modal>
</template>

<script setup lang="ts">
  import { computed, reactive } from 'vue';
  import { Avatar, Checkbox, Empty, Modal, Input } from 'ant-design-vue';
  import {
    BuildOutlined,
    CloseOutlined,
    FolderOpenOutlined,
    ApartmentOutlined,
    TeamOutlined,
  } from '@ant-design/icons-vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { isNullOrWhiteSpace } from '/@/utils/strings';
  import { findByUserName } from '/@/api/identity/users-lookup';
  import { getList as getUsers } from '/@/api/identity/users';
  import { getList as getRoles } from '/@/api/identity/roles';
  import { getList as getOrganizationUnits } from '/@/api/identity/organization-units';
  import Ellipsis from './Ellipsis.vue';

  const InputSearch = Input.Search;

  const emits = defineEmits(['ok', 'close']);
  const props = defineProps({
    width: {
      type: String,
      default: '600px',
    },
    clickClose: {
      type: Boolean,
      default: false,
    },
    closeFree: {
      type: Boolean,
      default: false,
    },
    title: {
      default: '请选择',
      type: String,
    },
    type: {
      default: 'dept', //org选择部门/人员  user-选人  dept-选部门 role-选角色
      type: String,
    },
    multiple: {
      //是否多选
      default: false,
      type: Boolean,
    },
    selected: {
      default: () => {
        return [];
      },
      type: Array,
    },
  });
  const deptStackStr = computed(() => {
    return String(state.deptStack.map((v) => v.name)).replaceAll(',', ' > ');
  });
  const orgs = computed(() => {
    return !state.search || state.search.trim() === '' ? state.nodes : state.searchUsers;
  });
  const showUsers = computed(() => {
    return state.search || state.search.trim() !== '';
  });
  const state = reactive({
    visible: false,
    loading: false,
    checkAll: false,
    nowDeptId: null,
    isIndeterminate: false,
    searchUsers: [] as string[],
    nodes: [] as any[],
    select: [] as any[],
    search: '',
    deptStack: [] as any[],
  });
  const { createMessage, createConfirm } = useMessage();

  function show() {
    state.visible = true;
    init();
    initOrgList(props.type);
  }

  function orgItemClass(org) {
    return {
      'org-item': true,
      'org-dept-item': org.type === 'dept',
      'org-user-item': org.type === 'user',
      'org-role-item': org.type === 'role',
    };
  }

  function disableDept(node) {
    return props.type === 'user' && 'dept' === node.type;
  }

  function initOrgList(type) {
    switch (type) {
      case 'dept':
        getOrgList();
        break;
      case 'role':
        getRoleList();
        break;
      case 'user':
        getUserList();
        break;
    }
  }

  function getUserList() {
    state.loading = true;
    getUsers({})
      .then((rsp) => {
        state.loading = false;
        state.nodes = rsp.items.map((item) => {
          return {
            ...item,
            name: item.userName,
          };
        });
        selectToLeft();
      })
      .catch((err) => {
        state.loading = false;
        createMessage.error(err.response.data);
      });
  }

  function getRoleList() {
    state.loading = true;
    getRoles({})
      .then((rsp) => {
        state.loading = false;
        state.nodes = rsp.items;
        selectToLeft();
      })
      .catch((err) => {
        state.loading = false;
        createMessage.error(err.response.data);
      });
  }

  function getOrgList() {
    state.loading = true;
    getOrganizationUnits({})
      .then((rsp) => {
        state.loading = false;
        state.nodes = rsp.items.map((item) => {
          return {
            ...item,
            name: item.displayName,
          };
        });
        selectToLeft();
      })
      .catch((err) => {
        state.loading = false;
        createMessage.error(err.response.data);
      });
  }

  function getShortName(name) {
    if (name) {
      return name.length > 2 ? name.substring(1, 3) : name;
    }
    return '**';
  }

  function searchUser() {
    let userName = state.search.trim();
    state.searchUsers = [];
    state.loading = true;
    findByUserName(userName)
      .then((rsp) => {
        state.loading = false;
        state.searchUsers = [rsp.userName];
        selectToLeft();
      })
      .catch(() => {
        state.loading = false;
        createMessage.error('接口异常');
      });
  }

  function selectToLeft() {
    let nodes = state.search.trim() === '' ? state.nodes : state.searchUsers;
    nodes.forEach((node) => {
      for (let i = 0; i < state.select.length; i++) {
        if (state.select[i].id === node.id) {
          node.selected = true;
          break;
        } else {
          node.selected = false;
        }
      }
    });
  }

  function selectChange(node) {
    if (node.selected) {
      state.checkAll = false;
      for (let i = 0; i < state.select.length; i++) {
        if (state.select[i].id === node.id) {
          state.select.splice(i, 1);
          break;
        }
      }
      node.selected = false;
    } else if (!disableDept(node)) {
      node.selected = true;
      let nodes = state.search.trim() === '' ? state.nodes : state.searchUsers;
      if (!props.multiple) {
        nodes.forEach((nd) => {
          if (node.id !== nd.id) {
            nd.selected = false;
          }
        });
      }
      if (node.type === 'dept') {
        if (!props.multiple) {
          state.select = [node];
        } else {
          state.select.unshift(node);
        }
      } else {
        if (!props.multiple) {
          state.select = [node];
        } else {
          state.select.push(node);
        }
      }
    }
  }

  function noSelected(index) {
    let nodes = state.nodes;
    for (let f = 0; f < 2; f++) {
      for (let i = 0; i < nodes.length; i++) {
        if (nodes[i].id === state.select[index].id) {
          nodes[i].selected = false;
          state.checkAll = false;
          break;
        }
      }
      nodes = state.searchUsers;
    }
    state.select.splice(index, 1);
  }

  function handleCheckAllChange() {
    state.nodes.forEach((node) => {
      if (state.checkAll) {
        if (!node.selected && !disableDept(node)) {
          node.selected = true;
          state.select.push(node);
        }
      } else {
        node.selected = false;
        for (let i = 0; i < state.select.length; i++) {
          if (state.select[i].id === node.id) {
            state.select.splice(i, 1);
            break;
          }
        }
      }
    });
  }

  function nextNode(node) {
    state.nowDeptId = node.id;
    state.deptStack.push(node);
    getOrgList();
  }

  function beforeNode() {
    if (state.deptStack.length === 0) {
      return;
    }
    if (state.deptStack.length < 2) {
      state.nowDeptId = null;
    } else {
      state.nowDeptId = state.deptStack[state.deptStack.length - 2].id;
    }
    state.deptStack.splice(state.deptStack.length - 1, 1);
    getOrgList();
  }

  function recover() {
    state.select = [];
    state.nodes.forEach((nd) => (nd.selected = false));
  }

  function selectOk() {
    emits(
      'ok',
      Object.assign(
        [],
        state.select.map((v) => {
          v.avatar = undefined;
          return v;
        }),
      ),
    );
    state.visible = false;
    recover();
  }

  function clearSelected() {
    createConfirm({
      title: '提示',
      content: '您确定要清空已选中的项?',
      iconType: 'warning',
      onOk: () => {
        recover();
      },
    });
  }

  function close() {
    emits('close');
    recover();
    state.visible = false;
  }

  function init() {
    state.checkAll = false;
    state.nowDeptId = null;
    state.deptStack = [];
    state.nodes = [];
    state.select = Object.assign([], props.selected);
    selectToLeft();
  }

  defineExpose({
    show,
    close,
  });
</script>

<style lang="less" scoped>
  @containWidth: 278px;

  .candidate,
  .selected {
    position: absolute;
    display: inline-block;
    width: @containWidth;
    height: 400px;
    border: 1px solid #e8e8e8;
  }

  .picker {
    height: 402px;
    position: relative;
    text-align: left;

    .candidate {
      left: 0;
      top: 0;

      .role-header {
        padding: 10px !important;
        margin-bottom: 5px;
        border-bottom: 1px solid #e8e8e8;
      }

      .top-dept {
        margin-left: 20px;
        cursor: pointer;
        color: #38adff;
      }

      .next-dept {
        float: right;
        color: @primary-color;
        cursor: pointer;
      }

      .next-dept-disable {
        float: right;
        color: #8c8c8c;
        cursor: not-allowed;
      }

      & > div:first-child {
        padding: 5px 10px;
      }
    }

    .selected {
      right: 0;
      top: 0;
    }

    .org-items {
      overflow-y: auto;
      height: 310px;

      .anticon-close {
        position: absolute;
        right: 5px;
        cursor: pointer;
        font-size: larger;
      }

      .org-dept-item {
        padding: 10px 5px;

        & > div {
          display: inline-block;

          & > span:last-child {
            position: absolute;
            right: 5px;
          }
        }
      }

      .org-role-item {
        display: flex;
        align-items: center;
        padding: 10px 5px;
      }

      :deep(.org-user-item) {
        display: flex;
        align-items: center;
        padding: 5px;

        & > div {
          display: inline-block;
        }

        .avatar {
          width: 35px;
          text-align: center;
          line-height: 35px;
          background: @primary-color;
          color: white;
          border-radius: 50%;
        }
      }

      :deep(.org-item) {
        margin: 0 5px;
        border-radius: 5px;
        position: relative;
        display: flex;

        .ant-checkbox {
          margin-right: 10px;
        }

        .name {
          margin-left: 5px;
        }

        &:hover {
          background: #f1f1f1;
        }
      }
    }
  }

  .selected {
    border-left: none;

    .count {
      width: calc(@containWidth - 20px);
      padding: 10px;
      display: inline-block;
      border-bottom: 1px solid #e8e8e8;
      margin-bottom: 5px;

      & > span:nth-child(2) {
        float: right;
        color: #c75450;
        cursor: pointer;
      }
    }
  }

  :deep(.ant-modal-body) {
    padding: 10px 20px;
  }

  .disabled {
    cursor: not-allowed !important;
    color: #8c8c8c !important;
  }

  ::-webkit-scrollbar {
    float: right;
    width: 4px;
    height: 4px;
    background-color: white;
  }

  ::-webkit-scrollbar-thumb {
    border-radius: 16px;
    background-color: #efefef;
  }
</style>
