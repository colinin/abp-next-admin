import { computed, onMounted, ref } from 'vue';
import { cloneDeep } from 'lodash-es';
import { Modal } from 'ant-design-vue';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormSchema } from '/@/components/Form';
import { create, deleteById, getAll, move, update } from '/@/api/identity/organization-units';
import { listToTree } from '/@/utils/helper/treeHelper';
import { useModal } from '/@/components/Modal';

export function useOuTree({ emit }: { emit: EmitType }) {
  const { L } = useLocalization('AbpIdentity');
  const organizationUnitTree = ref([]);

  const formSchemas: FormSchema[] = [
    {
      field: 'id',
      component: 'Input',
      label: 'id',
      colProps: { span: 24 },
      ifShow: false,
    },
    {
      field: 'parentId',
      component: 'Input',
      label: 'parentId',
      colProps: { span: 24 },
      ifShow: false,
    },
    {
      field: 'displayName',
      component: 'Input',
      label: L('OrganizationUnit:DisplayName'),
      colProps: { span: 24 },
      required: true,
    },
  ];

  const [registerModal, { openModal }] = useModal();

  const getContentMenus = computed(() => {
    return (node: any) => {
      return [
        {
          label: L('Edit'),
          handler: () => {
            openModal(true, cloneDeep(node.$attrs), true);
          },
          icon: 'ant-design:edit-outlined',
        },
        {
          label: L('OrganizationUnit:AddChildren'),
          handler: () => {
            handleAddNew(node.$attrs.id);
            // openModal(true, { parentId: node.$attrs.id }, true);
          },
          icon: 'ant-design:plus-outlined',
        },
        {
          label: L('Delete'),
          handler: () => {
            Modal.warning({
              title: L('AreYouSure'),
              content: L('OrganizationUnit:WillDelete', [node.$attrs.displayName] as Recordable),
              okCancel: true,
              onOk: () => {
                deleteById(node.$attrs.id).then(() => {
                  loadTree();
                });
              },
            });
          },
          icon: 'ant-design:delete-outlined',
        },
      ];
    };
  });

  function loadTree() {
    getAll().then((res) => {
      organizationUnitTree.value = listToTree(res.items, {
        id: 'id',
        pid: 'parentId',
      });
    });
  }

  function handleAddNew(parentId?: string) {
    openModal(true, { parentId: parentId }, true);
  }

  function handleSelect(selectedKeys) {
    emit('select', selectedKeys[0]);
  }

  function handleDrop(opt) {
    const api =
      opt.dropPosition === -1
        ? move(opt.dragNode.eventKey) // parent
        : move(opt.dragNode.eventKey, opt.node.eventKey); // children
    api.then(() => loadTree());
  }

  function handleSubmit(val) {
    const api = val.id
      ? update(val.id, {
          displayName: val.displayName,
        })
      : create(val);
    return api.then(() => loadTree());
  }

  onMounted(() => {
    loadTree();
  });
  return {
    organizationUnitTree,
    getContentMenus,
    registerModal,
    formSchemas,
    handleDrop,
    handleAddNew,
    handleSelect,
    handleSubmit,
  };
}
