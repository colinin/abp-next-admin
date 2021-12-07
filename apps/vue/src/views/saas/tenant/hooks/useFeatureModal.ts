import { useModal } from '/@/components/Modal';

export function useFeatureModal() {
  const [registerModal, { openModal }] = useModal();

  function handleManageHostFeature() {
    openModal(true, { providerName: 'T', providerKey: '' });
  }

  function handleManageTenantFeature(record) {
    openModal(true, { providerName: 'T', providerKey: record.id });
  }

  return {
    registerModal,
    handleManageHostFeature,
    handleManageTenantFeature,
  };
}
