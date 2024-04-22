import { computed } from 'vue';
import { useAbpStoreWithOut } from '/@/store/modules/abp';

export interface PermissionChecker {
  isGranted(name: string | string[], requiresAll?: boolean): boolean;
  authorize(name: string | string[]): void;
}

export function useAuthorization(): PermissionChecker {
  const getGrantedPolicies = computed(() => {
    const abpStore = useAbpStoreWithOut();
    return abpStore.getApplication.auth.grantedPolicies ?? {};
  });

  function isGranted(name: string | string[], requiresAll?: boolean): boolean {
    const grantedPolicies = getGrantedPolicies.value;
    if (Array.isArray(name)) {
      if (requiresAll === undefined || requiresAll === true) {
        return name.every((name) => grantedPolicies[name]);
      }
      return name.some((name) => grantedPolicies[name]);
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