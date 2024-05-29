<script lang="ts">
  import {
    computed,
    defineComponent,
    reactive,
    ref,
    unref,
    h,
    resolveComponent,
    getCurrentInstance,
  } from 'vue';
  import { Button } from 'ant-design-vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useFlowStoreWithOut } from '/@/store/modules/flow';
  //导入所有节点组件
  import HttpEndPoint from '../nodes/HttpEndPointNode.vue';
  import Approval from '../nodes/ApprovalNode.vue';
  import Cc from '../nodes/CcNode.vue';
  import Concurrent from '../nodes/ConcurrentNode.vue';
  import Condition from '../nodes/ConditionNode.vue';
  import Trigger from '../nodes/TriggerNode.vue';
  import Delay from '../nodes/DelayNode.vue';
  import Empty from '../nodes/EmptyNode.vue';
  import Root from '../nodes/RootNode.vue';
  import Node from '../nodes/Node.vue';

  import DefaultProps, { PrimaryNodes } from './DefaultNodeProps';
  import { cloneDeep } from 'lodash-es';

  export default defineComponent({
    name: 'ProcessTree',
    components: {
      httpendpoint: HttpEndPoint,
      Approval,
      Cc,
      Concurrent,
      Condition,
      Trigger,
      Delay,
      Empty,
      Root,
      Node,
    },
    emits: ['selectedNode'],
    setup: (_, { emit, expose }) => {
      const instance = getCurrentInstance();

      const { createMessage } = useMessage();
      const flowStore = useFlowStoreWithOut();
      const nodeMap = computed(() => {
        return flowStore.nodeMap;
      });
      const dom = computed(() => {
        return flowStore.design.process;
      });
      const state = reactive({
        valid: true,
      });
      const rootRef = ref<any>();

      function getDomTree(node) {
        toMapping(node);
        if (isPrimaryNode(node)) {
          //普通业务节点
          let childDoms = getDomTree(node.children);
          decodeAppendDom(node, childDoms);
          return [h('div', { class: { 'primary-node': true } }, { default: () => childDoms })];
        } else if (isBranchNode(node)) {
          let index = 0;
          //遍历分支节点，包含并行及条件节点
          let branchItems = node.branchs.map((branchNode) => {
            //处理每个分支内子节点
            toMapping(branchNode);
            let childDoms = getDomTree(branchNode.children);
            decodeAppendDom(branchNode, childDoms, { level: index + 1, size: node.branchs.length });
            //插入4条横线，遮挡掉条件节点左右半边线条
            insertCoverLine(index, childDoms, node.branchs);
            //遍历子分支尾部分支
            index++;
            return h('div', { class: { 'branch-node-item': true } }, { default: () => childDoms });
          });
          //插入添加分支/条件的按钮
          branchItems.unshift(
            h(
              'div',
              { class: { 'add-branch-btn': true } },
              {
                default: () => [
                  h(
                    Button,
                    {
                      class: { 'add-branch-btn-el': true },
                      size: 'small',
                      shape: 'round',
                      onClick: () => addBranchNode(node),
                      innerHTML: `添加${isConditionNode(node) ? '条件' : '分支'}`,
                    },
                    { default: () => [] },
                  ),
                ],
              },
            ),
          );
          let bchDom = [
            h('div', { class: { 'branch-node': true } }, { default: () => branchItems }),
          ];
          //继续遍历分支后的节点
          let afterChildDoms = getDomTree(node.children);
          return [h('div', {}, { default: () => [bchDom, afterChildDoms] })];
        } else if (isEmptyNode(node)) {
          //空节点，存在于分支尾部
          let childDoms = getDomTree(node.children);
          decodeAppendDom(node, childDoms);
          return [h('div', { class: { 'empty-node': true } }, { default: () => childDoms })];
        } else {
          //遍历到了末端，无子节点
          return [];
        }
      }

      //解码渲染的时候插入dom到同级
      function decodeAppendDom(node, dom, props = {} as any) {
        props.config = node;
        const component = resolveComponent(node.type.toLowerCase());
        dom?.unshift(
          h(component, {
            ...props,
            ref: node.id,
            key: node.id,
            //定义事件，插入节点，删除节点，选中节点，复制/移动
            onInsertNode: (type) => insertNode(type, node),
            onDelNode: () => delNode(node),
            onSelected: () => selectNode(node),
            onCopy: () => copyBranch(node),
            onLeftMove: () => branchMove(node, -1),
            onRightMove: () => branchMove(node, 1),
          }),
        );
      }

      //id映射到map，用来向上遍历
      function toMapping(node) {
        if (node && node.id) {
          //console.log("node=> " + node.id + " name:" + node.name + " type:" + node.type)
          nodeMap.value.set(node.id, node);
        }
      }

      function insertCoverLine(index, doms, branchs) {
        if (index === 0) {
          //最左侧分支
          doms.unshift(h('div', { class: { 'line-top-left': true } }, { default: () => [] }));
          doms.unshift(h('div', { class: { 'line-bot-left': true } }, { default: () => [] }));
        } else if (index === branchs.length - 1) {
          //最右侧分支
          doms.unshift(h('div', { class: { 'line-top-right': true } }, { default: () => [] }));
          doms.unshift(h('div', { class: { 'line-bot-right': true } }, { default: () => [] }));
        }
      }

      function copyBranch(node) {
        let parentNode = nodeMap.value.get(node.parentId);
        let branchNode = cloneDeep(node);
        branchNode.name = branchNode.name + '-copy';
        forEachNode(parentNode, branchNode, (parent, node) => {
          let id = getRandomId();
          console.log(node, '新id =>' + id, '老nodeId:' + node.id);
          node.id = id;
          node.parentId = parent.id;
        });
        parentNode.branchs.splice(parentNode.branchs.indexOf(node), 0, branchNode);
        instance?.proxy?.$forceUpdate();
      }

      function branchMove(node, offset) {
        let parentNode = nodeMap.value.get(node.parentId);
        let index = parentNode.branchs.indexOf(node);
        let branch = parentNode.branchs[index + offset];
        parentNode.branchs[index + offset] = parentNode.branchs[index];
        parentNode.branchs[index] = branch;
        instance?.proxy?.$forceUpdate();
      }

      //判断是否为主要业务节点
      function isPrimaryNode(node) {
        return node && PrimaryNodes.includes(node.type);
      }

      function isBranchNode(node) {
        return node && (node.type === 'CONDITIONS' || node.type === 'CONCURRENTS');
      }

      function isEmptyNode(node) {
        return node && node.type === 'EMPTY';
      }

      //是分支节点
      function isConditionNode(node) {
        return node.type === 'CONDITIONS';
      }

      //是分支节点
      function isBranchSubNode(node) {
        return node && (node.type === 'CONDITION' || node.type === 'CONCURRENT');
      }

      function isConcurrentNode(node) {
        return node.type === 'CONCURRENTS';
      }

      function getRandomId() {
        return `node_${new Date().getTime().toString().substring(5)}${Math.round(
          Math.random() * 9000 + 1000,
        )}`;
      }

      //选中一个节点
      function selectNode(node) {
        console.log('selectNode', node);
        flowStore.selectNode(node);
        emit('selectedNode', node);
      }

      //处理节点插入逻辑
      function insertNode(type, parentNode) {
        console.log('insertNode', type, parentNode);
        const rootEl = unref(rootRef);
        rootEl?.click();
        //缓存一下后面的节点
        let afterNode = parentNode.children;
        //插入新节点
        parentNode.children = {
          id: getRandomId(),
          parentId: parentNode.id,
          props: {},
          type: type,
        };
        switch (type) {
          case 'APPROVAL':
            insertApprovalNode(parentNode);
            break;
          case 'CC':
            insertCcNode(parentNode);
            break;
          case 'DELAY':
            insertDelayNode(parentNode);
            break;
          case 'TRIGGER':
            insertTriggerNode(parentNode);
            break;
          case 'CONDITIONS':
            insertConditionsNode(parentNode);
            break;
          case 'CONCURRENTS':
            insertConcurrentsNode(parentNode);
            break;
          case 'HTTPENDPOINT':
            insertHttpEndPointNode(parentNode);
            break;
          default:
            break;
        }
        //拼接后续节点
        if (isBranchNode({ type: type })) {
          if (afterNode && afterNode.id) {
            afterNode.parentId = parentNode.children.children.id;
          }
          parentNode.children.children.children = afterNode;
        } else {
          if (afterNode && afterNode.id) {
            afterNode.parentId = parentNode.children.id;
          }
          parentNode.children.children = afterNode;
        }
        instance?.proxy?.$forceUpdate();
      }

      function insertApprovalNode(parentNode) {
        parentNode.children.name = '审批人';
        parentNode.children.props = cloneDeep(DefaultProps.APPROVAL_PROPS);
      }

      function insertCcNode(parentNode) {
        parentNode.children.name = '抄送人';
        parentNode.children.props = cloneDeep(DefaultProps.CC_PROPS);
      }

      function insertDelayNode(parentNode) {
        parentNode.children.name = '延时处理';
        parentNode.children.props = cloneDeep(DefaultProps.DELAY_PROPS);
      }

      function insertTriggerNode(parentNode) {
        parentNode.children.name = '触发器';
        parentNode.children.props = cloneDeep(DefaultProps.TRIGGER_PROPS);
      }

      function insertConditionsNode(parentNode) {
        parentNode.children.name = '条件分支';
        parentNode.children.children = {
          id: getRandomId(),
          parentId: parentNode.children.id,
          type: 'EMPTY',
        };
        parentNode.children.branchs = [
          {
            id: getRandomId(),
            parentId: parentNode.children.id,
            type: 'CONDITION',
            props: cloneDeep(DefaultProps.CONDITION_PROPS),
            name: '条件1',
            children: {},
          },
          {
            id: getRandomId(),
            parentId: parentNode.children.id,
            type: 'CONDITION',
            props: cloneDeep(DefaultProps.CONDITION_PROPS),
            name: '条件2',
            children: {},
          },
        ];
      }

      function insertConcurrentsNode(parentNode) {
        parentNode.children.name = '并行分支';
        parentNode.children.children = {
          id: getRandomId(),
          parentId: parentNode.children.id,
          type: 'EMPTY',
        };
        parentNode.children.branchs = [
          {
            id: getRandomId(),
            name: '分支1',
            parentId: parentNode.children.id,
            type: 'CONCURRENT',
            props: {},
            children: {},
          },
          {
            id: getRandomId(),
            name: '分支2',
            parentId: parentNode.children.id,
            type: 'CONCURRENT',
            props: {},
            children: {},
          },
        ];
      }

      function insertHttpEndPointNode(parentNode) {
        parentNode.children.name = 'HttpEndPoint';
        parentNode.children.props = cloneDeep(DefaultProps.HTTPENDPOINT_PROPS);
      }

      function getBranchEndNode(conditionNode) {
        if (!conditionNode.children || !conditionNode.children.id) {
          return conditionNode;
        }
        return getBranchEndNode(conditionNode.children);
      }

      function addBranchNode(node) {
        if (node.branchs.length < 8) {
          node.branchs.push({
            id: getRandomId(),
            parentId: node.id,
            name: (isConditionNode(node) ? '条件' : '分支') + (node.branchs.length + 1),
            props: isConditionNode(node) ? cloneDeep(DefaultProps.CONDITION_PROPS) : {},
            type: isConditionNode(node) ? 'CONDITION' : 'CONCURRENT',
            children: {},
          });
        } else {
          createMessage.warning('最多只能添加 8 项😥');
        }
      }

      //删除当前节点
      function delNode(node) {
        console.log('删除节点', node);
        //获取该节点的父节点
        let parentNode = nodeMap.value.get(node.parentId);
        if (parentNode) {
          //判断该节点的父节点是不是分支节点
          if (isBranchNode(parentNode)) {
            //移除该分支
            parentNode.branchs.splice(parentNode.branchs.indexOf(node), 1);
            //处理只剩1个分支的情况
            if (parentNode.branchs.length < 2) {
              //获取条件组的父节点
              let ppNode = nodeMap.value.get(parentNode.parentId);
              //判断唯一分支是否存在业务节点
              if (parentNode.branchs[0].children && parentNode.branchs[0].children.id) {
                //将剩下的唯一分支头部合并到主干
                ppNode.children = parentNode.branchs[0].children;
                ppNode.children.parentId = ppNode.id;
                //搜索唯一分支末端最后一个节点
                let endNode = getBranchEndNode(parentNode.branchs[0]);
                //后续节点进行拼接, 这里要取EMPTY后的节点
                endNode.children = parentNode.children.children;
                if (endNode.children && endNode.children.id) {
                  endNode.children.parentId = endNode.id;
                }
              } else {
                //直接合并分支后面的节点，这里要取EMPTY后的节点
                ppNode.children = parentNode.children.children;
                if (ppNode.children && ppNode.children.id) {
                  ppNode.children.parentId = ppNode.id;
                }
              }
            }
          } else {
            //不是的话就直接删除
            if (node.children && node.children.id) {
              node.children.parentId = parentNode.id;
            }
            parentNode.children = node.children;
          }
          instance?.proxy?.$forceUpdate();
        } else {
          createMessage.warning('出现错误，找不到上级节点😥');
        }
      }

      function validateProcess() {
        state.valid = true;
        let err = [];
        validate(err, dom.value);
        return err;
      }

      function validateNode(err, node) {
        const nodeRef = instance?.refs[node.id] as unknown as any;
        if (nodeRef?.validate) {
          state.valid = nodeRef.validate(err);
        }
      }

      //更新指定节点的dom
      function nodeDomUpdate(node) {
        const nodeRef = instance?.refs[node.id] as unknown as any;
        nodeRef?.$forceUpdate();
      }

      //给定一个起始节点，遍历内部所有节点
      function forEachNode(parent, node, callback) {
        if (isBranchNode(node)) {
          callback(parent, node);
          forEachNode(node, node.children, callback);
          node.branchs.map((branchNode) => {
            callback(node, branchNode);
            forEachNode(branchNode, branchNode.children, callback);
          });
        } else if (isPrimaryNode(node) || isEmptyNode(node) || isBranchSubNode(node)) {
          callback(parent, node);
          forEachNode(node, node.children, callback);
        }
      }

      //校验所有节点设置
      function validate(err, node) {
        if (isPrimaryNode(node)) {
          validateNode(err, node);
          validate(err, node.children);
        } else if (isBranchNode(node)) {
          //校验每个分支
          node.branchs.map((branchNode) => {
            //校验条件节点
            validateNode(err, branchNode);
            //校验条件节点后面的节点
            validate(err, branchNode.children);
          });
          validate(err, node.children);
        } else if (isEmptyNode(node)) {
          validate(err, node.children);
        }
      }

      expose({
        validateProcess,
      });

      return () => {
        nodeMap.value.clear();
        let processTrees = getDomTree(dom.value);
        //插入末端节点
        processTrees.push(
          h(
            'div',
            { style: { 'text-align': 'center' } },
            {
              default: () => [h('div', { class: { 'process-end': true }, innerHTML: '流程结束' })],
            },
          ),
        );
        return h('div', { class: { _root: true }, ref: rootRef }, { default: () => processTrees });
      };
    },
  });
</script>

<style lang="less" scoped>
  ._root {
    margin: 0 auto;
  }

  .process-end {
    width: 100px;
    margin: 0 auto;
    margin-bottom: 20px;
    border-radius: 15px;
    padding: 5px 10px;
    font-size: small;
    color: #747474;
    background-color: #f2f2f2;
    box-shadow: 0 0 10px 0 #bcbcbc;
  }

  .primary-node {
    display: flex;
    align-items: center;
    flex-direction: column;
  }

  .branch-node {
    display: flex;
    justify-content: center;
    /*border-top: 2px solid #cccccc;
    border-bottom: 2px solid #cccccc;*/
  }

  .branch-node-item {
    position: relative;
    display: flex;
    background: white;
    flex-direction: column;
    align-items: center;
    border-top: 2px solid #cccccc;
    border-bottom: 2px solid #cccccc;

    &:before {
      content: '';
      position: absolute;
      top: 0;
      left: calc(50% - 1px);
      margin: auto;
      width: 2px;
      height: 100%;
      background-color: #cacaca;
    }

    .line-top-left,
    .line-top-right,
    .line-bot-left,
    .line-bot-right {
      position: absolute;
      width: 50%;
      height: 4px;
      background-color: white;
    }

    .line-top-left {
      top: -2px;
      left: -1px;
    }

    .line-top-right {
      top: -2px;
      right: -1px;
    }

    .line-bot-left {
      bottom: -2px;
      left: -1px;
    }

    .line-bot-right {
      bottom: -2px;
      right: -1px;
    }
  }

  .add-branch-btn {
    position: absolute;
    width: 80px;

    .add-branch-btn-el {
      z-index: 999;
      position: absolute;
      top: -15px;
    }
  }

  .empty-node {
    display: flex;
    justify-content: center;
    flex-direction: column;
    align-items: center;
  }
</style>
