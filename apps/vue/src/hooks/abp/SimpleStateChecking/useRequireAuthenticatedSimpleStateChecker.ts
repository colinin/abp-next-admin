import { useAbpStoreWithOut } from '/@/store/modules/abp';

export interface RequireAuthenticatedStateChecker {
  name: string;
}

export class RequireAuthenticatedSimpleStateChecker<TState extends IHasSimpleStateCheckers<TState>> implements RequireAuthenticatedStateChecker, ISimpleStateChecker<TState> {
  name = "A";
  _currentUser: CurrentUser;
  constructor(currentUser: CurrentUser) {
    this._currentUser = currentUser;
  }
  isEnabled(_context: SimpleStateCheckerContext<TState>): boolean {
    return this._currentUser.isAuthenticated;
  }

  serialize(): string {
    return JSON.stringify({
      "T": this.name,
    });
  }
}

export function useRequireAuthenticatedSimpleStateChecker<TState extends IHasSimpleStateCheckers<TState>>(): ISimpleStateChecker<TState> {
  const abpStore = useAbpStoreWithOut();
  const { currentUser } = abpStore.getApplication;
  return new RequireAuthenticatedSimpleStateChecker<TState>(currentUser);
}