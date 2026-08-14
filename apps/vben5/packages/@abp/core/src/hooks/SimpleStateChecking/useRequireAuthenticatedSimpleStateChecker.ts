import type {
  CurrentUser,
  IHasSimpleStateCheckers,
  ISimpleStateChecker,
  SimpleStateCheckerContext,
} from '../../types/global';

import { useAbpStore } from '../../store/abp';

export interface RequireAuthenticatedStateChecker {
  name: string;
}

export class RequireAuthenticatedSimpleStateChecker<
  TState extends IHasSimpleStateCheckers<TState>,
>
  implements ISimpleStateChecker<TState>, RequireAuthenticatedStateChecker
{
  currentUser?: CurrentUser;
  name = 'A';
  constructor(_currentUser?: CurrentUser) {
    this.currentUser = _currentUser;
  }
  isEnabled(_context: SimpleStateCheckerContext<TState>): boolean {
    return this.currentUser?.isAuthenticated ?? false;
  }

  serialize(): string {
    return JSON.stringify({
      T: this.name,
    });
  }
}

export function useRequireAuthenticatedSimpleStateChecker<
  TState extends IHasSimpleStateCheckers<TState>,
>(): ISimpleStateChecker<TState> {
  const abpStore = useAbpStore();
  return new RequireAuthenticatedSimpleStateChecker<TState>(
    abpStore.application?.currentUser,
  );
}
