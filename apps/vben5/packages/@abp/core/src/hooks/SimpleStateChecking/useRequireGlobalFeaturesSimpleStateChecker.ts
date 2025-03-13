import type { IGlobalFeatureChecker } from '../../types/features';
import type {
  IHasSimpleStateCheckers,
  ISimpleStateChecker,
  SimpleStateCheckerContext,
} from '../../types/global';

import { useGlobalFeatures } from '../useGlobalFeatures';

export interface RequireGlobalFeaturesStateChecker {
  globalFeatureNames: string[];
  name: string;
  requiresAll: boolean;
}

export class RequireGlobalFeaturesSimpleStateChecker<
    TState extends IHasSimpleStateCheckers<TState>,
  >
  implements ISimpleStateChecker<TState>, RequireGlobalFeaturesStateChecker
{
  _globalFeatureChecker: IGlobalFeatureChecker;
  globalFeatureNames: string[];
  name: string = 'G';
  requiresAll: boolean;
  constructor(
    globalFeatureChecker: IGlobalFeatureChecker,
    globalFeatureNames: string[],
    requiresAll: boolean = false,
  ) {
    this._globalFeatureChecker = globalFeatureChecker;
    this.globalFeatureNames = globalFeatureNames;
    this.requiresAll = requiresAll;
  }
  isEnabled(_context: SimpleStateCheckerContext<TState>): boolean {
    return this._globalFeatureChecker.isEnabled(
      this.globalFeatureNames,
      this.requiresAll,
    );
  }

  serialize(): string {
    return JSON.stringify({
      A: this.requiresAll,
      N: this.globalFeatureNames,
      T: this.name,
    });
  }
}

export function useRequireGlobalFeaturesSimpleStateChecker<
  TState extends IHasSimpleStateCheckers<TState>,
>(
  globalFeatureNames: string[],
  requiresAll: boolean = false,
): ISimpleStateChecker<TState> {
  const globalFeatureChecker = useGlobalFeatures();
  return new RequireGlobalFeaturesSimpleStateChecker(
    globalFeatureChecker,
    globalFeatureNames,
    requiresAll,
  );
}
