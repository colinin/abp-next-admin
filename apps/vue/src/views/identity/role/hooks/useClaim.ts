import type { Ref } from 'vue';

import { useLocalization } from '/@/hooks/abp/useLocalization';
import { Modal } from 'ant-design-vue';
import { FormSchema } from '/@/components/Form';
import { useModal } from '/@/components/Modal';
import { getActivedList } from '/@/api/identity/claim';
import { getClaimColumns } from '../datas/TableData';
import { createClaim, deleteClaim, getClaimList, updateClaim } from '/@/api/identity/role';
import { useTable } from '/@/components/Table';
import { computed, unref, watch } from 'vue';

interface UseClaim {
  roleIdRef: Ref<string>;
}
// TODO: 与UserClaim重复 需要分离组件
export function useClaim({ roleIdRef }: UseClaim) {
  const { L } = useLocalization('AbpIdentity');
  const formSchemas: FormSchema[] = [
    {
      field: 'id',
      component: 'Input',
      label: 'id',
      colProps: { span: 24 },
      ifShow: false,
    },
    {
      field: 'claimType',
      component: 'ApiSelect',
      label: L('DisplayName:ClaimType'),
      colProps: { span: 24 },
      required: true,
      componentProps: {
        api: () => getActivedList(),
        resultField: 'items',
        labelField: 'name',
        valueField: 'name',
      },
    },
    {
      field: 'claimValue',
      component: 'Input',
      label: L('DisplayName:ClaimValue'),
      colProps: { span: 24 },
      required: true,
      ifShow: ({ values }) => {
        return values.id ? false : true;
      },
    },
    {
      field: 'newClaimValue',
      component: 'Input',
      label: L('DisplayName:ClaimValue'),
      colProps: { span: 24 },
      required: true,
      ifShow: ({ values }) => {
        return values.id ? true : false;
      },
    },
  ];
  const search = computed(() => {
    return {
      id: roleIdRef.value,
    };
  });
  const [registerTable, { reload: reloadTable }] = useTable({
    rowKey: 'id',
    title: L('ManageClaim'),
    columns: getClaimColumns(),
    api: getClaimList,
    searchInfo: search,
    pagination: false,
    striped: false,
    useSearchForm: false,
    showTableSetting: true,
    bordered: true,
    showIndexColumn: false,
    canResize: false,
    immediate: false,
    rowSelection: { type: 'checkbox' },
    actionColumn: {
      width: 160,
      title: L('Actions'),
      dataIndex: 'action',
      slots: { customRender: 'action' },
    },
  });

  const [registerClaimForm, { openModal }] = useModal();

  function openClaimForm(model) {
    if (model.id) {
      openModal(
        true,
        Object.assign(model, {
          newClaimValue: model.claimValue,
        }),
        true,
      );
    } else {
      openModal(true, {}, true);
    }
  }

  function handleDelete(claim) {
    Modal.warning({
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessageWithFormat', [claim.claimValue] as Recordable),
      okCancel: true,
      onOk: () => {
        deleteClaim(unref(roleIdRef), claim).then(() => {
          reloadTable();
        });
      },
    });
  }

  function handleSaveChanges(input) {
    const api = input.id
      ? updateClaim(unref(roleIdRef), input)
      : createClaim(unref(roleIdRef), input);
    return api.then(() => {
      reloadTable();
    });
  }

  watch(
    () => unref(roleIdRef),
    (id) => {
      reloadTable({
        searchInfo: {
          id: id,
        },
      });
    },
  );

  return {
    formSchemas,
    registerClaimForm,
    openClaimForm,
    registerTable,
    reloadTable,
    handleSaveChanges,
    handleDelete,
  };
}
