import type { IHasSimpleStateCheckers, ISimpleStateChecker } from '@abp/core';

export interface SimplaCheckStateBase
  extends IHasSimpleStateCheckers<SimplaCheckStateBase> {
  stateCheckers: ISimpleStateChecker<SimplaCheckStateBase>[];
}
