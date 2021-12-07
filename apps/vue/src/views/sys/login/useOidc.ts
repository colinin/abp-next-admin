import { mgr } from '/@/utils/auth/oidc';
import { useUserStoreWithOut } from '/@/store/modules/user';

export function useOidc() {
  const userStore = useUserStoreWithOut();

  function login() {
    mgr.signinRedirect();
  }

  async function logout() {
    await mgr.signoutRedirect({ id_token_hint: userStore.getUserInfo.idToken });
  }

  return {
    login,
    logout,
  };
}
