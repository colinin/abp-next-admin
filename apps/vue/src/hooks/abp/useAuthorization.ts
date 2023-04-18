import { computed } from 'vue';
import { useAbpStoreWithOut } from '/@/store/modules/abp';

interface PermissionChecker {
  isGranted(name: string | string[]): boolean;
  authorize(name: string | string[]): void;
}

export function useAuthorization(): PermissionChecker {
  const getGrantedPolicies = computed(() => {
    const abpStore = useAbpStoreWithOut();
    return abpStore.getApplication.auth.grantedPolicies ?? {};
  });

  function isGranted(name: string | string[]): boolean {
    const grantedPolicies = getGrantedPolicies.value;
    if (Array.isArray(name)) {
      return name.every((name) => grantedPolicies[name]);
    }
    return grantedPolicies[name]
  }

  function authorize(name: string | string[]): void {
    if (!isGranted(name)) {
      throw Error(`Authorization failed! Given policy has not granted: ${name}`);
    }
  }

  return {
    isGranted,
    authorize,
  }
}