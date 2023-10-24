import { IFeatureChecker, useFeatures } from '/@/hooks/abp/useFeatures';

export interface RequireFeaturesStateChecker {
  name: string;
  requiresAll: boolean;
  featureNames: string[];
}

export class RequireFeaturesSimpleStateChecker<TState extends IHasSimpleStateCheckers<TState>> implements RequireFeaturesStateChecker, ISimpleStateChecker<TState> {
  name: string = 'F';
  _featureChecker: IFeatureChecker;
  featureNames: string[];
  requiresAll: boolean;
  constructor(
    featureChecker: IFeatureChecker,
    featureNames: string[],
    requiresAll: boolean = false) {
    this._featureChecker = featureChecker;
    this.featureNames = featureNames;
    this.requiresAll = requiresAll;
  }
  isEnabled(_context: SimpleStateCheckerContext<TState>): boolean {
    return this._featureChecker.isEnabled(this.featureNames, this.requiresAll);
  }
  
  serialize(): string {
    return JSON.stringify({
      "T": this.name,
      "A": this.requiresAll,
      "N": this.featureNames,
    });
  }
}

export function useRequireFeaturesSimpleStateChecker<TState extends IHasSimpleStateCheckers<TState>>(
  featureNames: string[],
  requiresAll: boolean = false,
): ISimpleStateChecker<TState> {
  const { featureChecker } = useFeatures();
  return new RequireFeaturesSimpleStateChecker(featureChecker, featureNames, requiresAll);
}