import { useModal } from '/@/components/Modal';

export function usePermission() {
  const [registerModel, { openModal }] = useModal();

  function showPermissionModal(userId: string) {
    const props = {
      providerName: 'R',
      providerKey: userId,
    };
    openModal(true, props, true);
  }

  return {
    registerModel,
    showPermissionModal,
  };
}
