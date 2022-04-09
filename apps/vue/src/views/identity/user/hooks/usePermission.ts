import { useModal } from '/@/components/Modal';
import { useUserStoreWithOut } from '/@/store/modules/user';

export function usePermission() {
  const [registerModel, { openModal }] = useModal();

  function showPermissionModal(userId: string, userName: string) {
    const userStore = useUserStoreWithOut();
    const props = {
      providerName: 'U',
      providerKey: userId,
      readonly: userStore.getUserInfo.userId === userId,
      identity: userName,
    };
    openModal(true, props, true);
  }

  return {
    registerModel,
    showPermissionModal,
  };
}
