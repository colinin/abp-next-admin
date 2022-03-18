import { useModal } from '/@/components/Modal';

export function useFeatureModal() {
  const [registerModal, { openModal }] = useModal();

  function handleManageFeature(record) {
    openModal(true, { providerName: 'E', providerKey: record.id });
  }

  return {
    registerModal,
    handleManageFeature,
  };
}
