import type { IGlobalFeatureChecker } from '../../types/features';
import type {
  IHasSimpleStateCheckers,
  ISimpleStateChecker,
  SimpleStateCheckerContext,
} from '../../types/global';

import { useGlobalFeatures } from '../useGlobalFeatures';

export interface RequireGlobalFeaturesStateChecker {
  featureNames: string[];
  name: string;
  requiresAll: boolean;
}

export class RequireGlobalFeaturesSimpleStateChecker<
    TState extends IHasSimpleStateCheckers<TState>,
  >
  implements RequireGlobalFeaturesStateChecker, ISimpleStateChecker<TState>
{
  _globalFeatureChecker: IGlobalFeatureChecker;
  featureNames: string[];
  name: string = 'G';
  requiresAll: boolean;
  constructor(
    globalFeatureChecker: IGlobalFeatureChecker,
    featureNames: string[],
    requiresAll: boolean = false,
  ) {
    this._globalFeatureChecker = globalFeatureChecker;
    this.featureNames = featureNames;
    this.requiresAll = requiresAll;
  }
  isEnabled(_context: SimpleStateCheckerContext<TState>): boolean {
    return this._globalFeatureChecker.isEnabled(
      this.featureNames,
      this.requiresAll,
    );
  }

  serialize(): string {
    return JSON.stringify({
      A: this.requiresAll,
      N: this.featureNames,
      T: this.name,
    });
  }
}

export function useRequireGlobalFeaturesSimpleStateChecker<
  TState extends IHasSimpleStateCheckers<TState>,
>(
  featureNames: string[],
  requiresAll: boolean = false,
): ISimpleStateChecker<TState> {
  const globalFeatureChecker = useGlobalFeatures();
  return new RequireGlobalFeaturesSimpleStateChecker(
    globalFeatureChecker,
    featureNames,
    requiresAll,
  );
}
