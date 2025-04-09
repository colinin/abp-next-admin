import type { FeatureValue, IFeatureChecker } from "#/features";
import useAbpStore from "@/store/abpCoreStore";
import { useMemo } from "react";

export function useFeatures(): IFeatureChecker {
	const application = useAbpStore((state) => state.application);

	const features = useMemo<FeatureValue[]>(() => {
		if (!application?.features?.values) {
			return [];
		}
		return Object.keys(application.features.values).map((name) => ({
			name,
			value: application.features.values[name] ?? "",
		}));
	}, [application]);

	function get(name: string): FeatureValue | undefined {
		return features.find((feature) => feature.name === name);
	}

	function _isEnabled(name: string): boolean {
		const setting = get(name);
		return setting?.value.toLowerCase() === "true";
	}

	const featureChecker: IFeatureChecker = {
		getOrEmpty(name: string) {
			return get(name)?.value ?? "";
		},

		isEnabled(featureNames: string | string[], requiresAll?: boolean) {
			if (Array.isArray(featureNames)) {
				if (featureNames.length === 0) return true;
				if (requiresAll === undefined || requiresAll === true) {
					return featureNames.every(_isEnabled);
				}
				return featureNames.some(_isEnabled);
			}
			return _isEnabled(featureNames);
		},
	};

	return featureChecker;
}
