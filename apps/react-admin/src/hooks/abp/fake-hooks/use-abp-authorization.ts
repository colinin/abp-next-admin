import type { IPermissionChecker } from "#/abp-core";
import useAbpStore from "@/store/abpCoreStore";
// import { useMemo } from "react";

export function useAuthorization(): IPermissionChecker {
	// const application = useAbpStore((state) => state.application);
	const { application } = useAbpStore.getState();
	// const grantedPolicies = useMemo(() => {
	// 	return application?.auth.grantedPolicies ?? {};
	// }, [application]);
	const grantedPolicies = application?.auth.grantedPolicies ?? {};

	function isGranted(name: string | string[], requiresAll?: boolean): boolean {
		if (Array.isArray(name)) {
			if (requiresAll === undefined || requiresAll === true) {
				return name.every((n) => grantedPolicies[n]);
			}
			return name.some((n) => grantedPolicies[n]);
		}
		return grantedPolicies[name] ?? false;
	}

	function authorize(name: string | string[]): void {
		if (!isGranted(name)) {
			throw new Error(`Authorization failed! Given policy has not granted: ${name}`);
		}
	}

	return {
		authorize,
		isGranted,
	};
}
