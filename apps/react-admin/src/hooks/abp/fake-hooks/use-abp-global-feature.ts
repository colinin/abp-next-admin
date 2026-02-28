import useAbpStore from "@/store/abpCoreStore";
import { isNullOrWhiteSpace } from "@/utils/string";
// import { useMemo } from "react";

export function useGlobalFeatures() {
	// const application = useAbpStore((state) => state.application);
	const { application } = useAbpStore.getState();

	// const enabledFeatures = useMemo<string[]>(() => {
	// 	if (!application?.globalFeatures?.enabledFeatures) {
	// 		return [];
	// 	}
	// 	return application.globalFeatures.enabledFeatures;
	// }, [application]);

	let enabledFeatures: string[];
	if (!application?.globalFeatures?.enabledFeatures) {
		enabledFeatures = [];
	} else {
		enabledFeatures = application.globalFeatures.enabledFeatures;
	}

	function _isEnabled(name: string): boolean {
		// Find if the feature exists in the enabled list
		const feature = enabledFeatures.find((f) => f === name);
		return !isNullOrWhiteSpace(feature);
	}

	function isEnabled(featureNames: string | string[], requiresAll?: boolean): boolean {
		if (Array.isArray(featureNames)) {
			if (featureNames.length === 0) return true;
			if (requiresAll === undefined || requiresAll === true) {
				return featureNames.every(_isEnabled);
			}
			return featureNames.some(_isEnabled);
		}
		return _isEnabled(featureNames);
	}

	return {
		isEnabled,
	};
}
