import { useModal } from '/@/components/Modal';

export function useTenantModal() {
  const [registerModal, { openModal }] = useModal();

  function handleAddNew() {
    openModal(true, { id: undefined });
  }

  function handleEdit(record) {
    openModal(true, { id: record.id });
  }

  return {
    registerModal,
    handleAddNew,
    handleEdit,
  };
}
