import { IGlobalFeatureChecker, useGlobalFeatures } from '/@/hooks/abp/useGlobalFeatures';

export interface RequireGlobalFeaturesStateChecker {
  name: string;
  requiresAll: boolean;
  featureNames: string[];
}

export class RequireGlobalFeaturesSimpleStateChecker<TState extends IHasSimpleStateCheckers<TState>> implements RequireGlobalFeaturesStateChecker, ISimpleStateChecker<TState> {
  name: string = 'G';
  _globalFeatureChecker: IGlobalFeatureChecker;
  featureNames: string[];
  requiresAll: boolean;
  constructor(
    globalFeatureChecker: IGlobalFeatureChecker,
    featureNames: string[],
    requiresAll: boolean = false) {
    this._globalFeatureChecker = globalFeatureChecker;
    this.featureNames = featureNames;
    this.requiresAll = requiresAll;
  }
  isEnabled(_context: SimpleStateCheckerContext<TState>): boolean {
    return this._globalFeatureChecker.isEnabled(this.featureNames, this.requiresAll);
  }

  serialize(): string {
    return JSON.stringify({
      "T": this.name,
      "A": this.requiresAll,
      "N": this.featureNames,
    });
  }
}

export function useRequireGlobalFeaturesSimpleStateChecker<TState extends IHasSimpleStateCheckers<TState>>(
  featureNames: string[],
  requiresAll: boolean = false,
): ISimpleStateChecker<TState> {
  const globalFeatureChecker = useGlobalFeatures();
  return new RequireGlobalFeaturesSimpleStateChecker(globalFeatureChecker, featureNames, requiresAll);
}
