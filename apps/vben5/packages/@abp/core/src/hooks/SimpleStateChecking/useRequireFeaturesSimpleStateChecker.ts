import type { IFeatureChecker } from '../../types/features';
import type {
  IHasSimpleStateCheckers,
  ISimpleStateChecker,
  SimpleStateCheckerContext,
} from '../../types/global';

import { useFeatures } from '../useFeatures';

export interface RequireFeaturesStateChecker {
  featureNames: string[];
  name: string;
  requiresAll: boolean;
}

export class RequireFeaturesSimpleStateChecker<
    TState extends IHasSimpleStateCheckers<TState>,
  >
  implements RequireFeaturesStateChecker, ISimpleStateChecker<TState>
{
  _featureChecker: IFeatureChecker;
  featureNames: string[];
  name: string = 'F';
  requiresAll: boolean;
  constructor(
    featureChecker: IFeatureChecker,
    featureNames: string[],
    requiresAll: boolean = false,
  ) {
    this._featureChecker = featureChecker;
    this.featureNames = featureNames;
    this.requiresAll = requiresAll;
  }
  isEnabled(_context: SimpleStateCheckerContext<TState>): boolean {
    return this._featureChecker.isEnabled(this.featureNames, this.requiresAll);
  }

  serialize(): string {
    return JSON.stringify({
      A: this.requiresAll,
      N: this.featureNames,
      T: this.name,
    });
  }
}

export function useRequireFeaturesSimpleStateChecker<
  TState extends IHasSimpleStateCheckers<TState>,
>(
  featureNames: string[],
  requiresAll: boolean = false,
): ISimpleStateChecker<TState> {
  const { featureChecker } = useFeatures();
  return new RequireFeaturesSimpleStateChecker(
    featureChecker,
    featureNames,
    requiresAll,
  );
}
