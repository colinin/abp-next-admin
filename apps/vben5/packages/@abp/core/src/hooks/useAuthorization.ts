import type { IPermissionChecker } from '../types/permissions';

import { computed } from 'vue';

import { useAbpStore } from '../store/abp';

export function useAuthorization(): IPermissionChecker {
  const abpStore = useAbpStore();
  const getGrantedPolicies = computed(() => {
    return abpStore.application?.auth.grantedPolicies ?? {};
  });

  function isGranted(name: string | string[], requiresAll?: boolean): boolean {
    const grantedPolicies = getGrantedPolicies.value;
    if (Array.isArray(name)) {
      if (requiresAll === undefined || requiresAll === true) {
        return name.every((name) => grantedPolicies[name]);
      }
      return name.some((name) => grantedPolicies[name]);
    }
    return grantedPolicies[name] ?? false;
  }

  function authorize(name: string | string[]): void {
    if (!isGranted(name)) {
      throw new Error(
        `Authorization failed! Given policy has not granted: ${name}`,
      );
    }
  }

  return {
    authorize,
    isGranted,
  };
}
