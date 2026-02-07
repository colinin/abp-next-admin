import type { IGlobalFeatureChecker } from "#/features";
import type { IHasSimpleStateCheckers, ISimpleStateChecker, SimpleStateCheckerContext } from "#/abp-core/global";

import { useGlobalFeatures } from "../use-abp-global-feature";

export interface RequireGlobalFeaturesStateChecker {
	globalFeatureNames: string[];
	name: string;
	requiresAll: boolean;
}

export class RequireGlobalFeaturesSimpleStateChecker<TState extends IHasSimpleStateCheckers<TState>>
	implements ISimpleStateChecker<TState>, RequireGlobalFeaturesStateChecker
{
	_globalFeatureChecker: IGlobalFeatureChecker;
	globalFeatureNames: string[];
	name = "G";
	requiresAll: boolean;
	constructor(globalFeatureChecker: IGlobalFeatureChecker, globalFeatureNames: string[], requiresAll = false) {
		this._globalFeatureChecker = globalFeatureChecker;
		this.globalFeatureNames = globalFeatureNames;
		this.requiresAll = requiresAll;
	}
	isEnabled(_context: SimpleStateCheckerContext<TState>): boolean {
		return this._globalFeatureChecker.isEnabled(this.globalFeatureNames, this.requiresAll);
	}

	serialize(): string {
		return JSON.stringify({
			A: this.requiresAll,
			N: this.globalFeatureNames,
			T: this.name,
		});
	}
}

export function useRequireGlobalFeaturesSimpleStateChecker<TState extends IHasSimpleStateCheckers<TState>>(
	globalFeatureNames: string[],
	requiresAll = false,
): ISimpleStateChecker<TState> {
	const globalFeatureChecker = useGlobalFeatures();
	return new RequireGlobalFeaturesSimpleStateChecker(globalFeatureChecker, globalFeatureNames, requiresAll);
}
