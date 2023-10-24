import { PermissionChecker, useAuthorization } from '/@/hooks/abp/useAuthorization';

export interface RequirePermissionsStateChecker<TState extends IHasSimpleStateCheckers<TState>> {
  name: string;
  model: RequirePermissionsSimpleBatchStateCheckerModel<TState>;
}

export class RequirePermissionsSimpleBatchStateCheckerModel<TState extends IHasSimpleStateCheckers<TState>> {
  state: TState;
  requiresAll: boolean;
  permissions: string[];
  constructor(
    state: TState,
    permissions: string[],
    requiresAll: boolean = true,
  ) {
    this.state = state;
    this.permissions = permissions;
    this.requiresAll = requiresAll;
  }
}

export class RequirePermissionsSimpleStateChecker<TState extends IHasSimpleStateCheckers<TState>> implements RequirePermissionsStateChecker<TState>, ISimpleStateChecker<TState> {
  name: string = 'P';
  model: RequirePermissionsSimpleBatchStateCheckerModel<TState>;
  _permissionChecker: PermissionChecker;
  constructor(
    permissionChecker: PermissionChecker,
    model: RequirePermissionsSimpleBatchStateCheckerModel<TState>) {
    this.model = model;
    this._permissionChecker = permissionChecker;
  }
  isEnabled(_context: SimpleStateCheckerContext<TState>): boolean {
    return this._permissionChecker.isGranted(this.model.permissions, this.model.requiresAll);
  }

  serialize(): string {
    return JSON.stringify({
      "T": this.name,
      "A": this.model.requiresAll,
      "N": this.model.permissions,
    });
  }
}

export class RequirePermissionsSimpleBatchStateChecker<TState extends IHasSimpleStateCheckers<TState>> implements ISimpleBatchStateChecker<TState> {
  name: string = 'P';
  models: RequirePermissionsSimpleBatchStateCheckerModel<TState>[];
  _permissionChecker: PermissionChecker;
  constructor(
    permissionChecker: PermissionChecker,
    models: RequirePermissionsSimpleBatchStateCheckerModel<TState>[]) {
    this.models = models;
    this._permissionChecker = permissionChecker;
  }
  isEnabled(context: SimpleBatchStateCheckerContext<TState>) {
    const result: SimpleStateCheckerResult<TState> = {};
    context.states.forEach((state) => {
      const model = this.models.find(x => x.state === state);
      if (model) {
        result[model.state as TState] = this._permissionChecker.isGranted(model.permissions, model.requiresAll);
      }
    });
    return result;
  }

  serialize(): string | undefined {
    return undefined;
  }
}

export function useRequirePermissionsSimpleStateChecker<TState extends IHasSimpleStateCheckers<TState>>(
  model: RequirePermissionsSimpleBatchStateCheckerModel<TState>): ISimpleStateChecker<TState> {
  const permissionChecker = useAuthorization();
  return new RequirePermissionsSimpleStateChecker<TState>(permissionChecker, model);
}

export function useRequirePermissionsSimpleBatchStateChecker<TState extends IHasSimpleStateCheckers<TState>>(
  models: RequirePermissionsSimpleBatchStateCheckerModel<TState>[]): ISimpleBatchStateChecker<TState> {
  const permissionChecker = useAuthorization();
  return new RequirePermissionsSimpleBatchStateChecker<TState>(permissionChecker, models);
}
