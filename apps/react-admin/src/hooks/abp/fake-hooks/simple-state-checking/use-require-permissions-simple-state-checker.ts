import type {
	IHasSimpleStateCheckers,
	ISimpleBatchStateChecker,
	ISimpleStateChecker,
	SimpleBatchStateCheckerContext,
	SimpleStateCheckerContext,
} from "#/abp-core/global";
import type { IPermissionChecker } from "#/abp-core/permissions";

import { useAuthorization } from "../use-abp-authorization";

export class RequirePermissionsSimpleBatchStateCheckerModel<TState extends IHasSimpleStateCheckers<TState>> {
	permissions: string[];
	requiresAll: boolean;
	state: TState;
	constructor(state: TState, permissions: string[], requiresAll = true) {
		this.state = state;
		this.permissions = permissions;
		this.requiresAll = requiresAll;
	}
}

export interface RequirePermissionsStateChecker<TState extends IHasSimpleStateCheckers<TState>> {
	model: RequirePermissionsSimpleBatchStateCheckerModel<TState>;
	name: string;
}

export class RequirePermissionsSimpleStateChecker<TState extends IHasSimpleStateCheckers<TState>>
	implements ISimpleStateChecker<TState>, RequirePermissionsStateChecker<TState>
{
	_permissionChecker: IPermissionChecker;
	model: RequirePermissionsSimpleBatchStateCheckerModel<TState>;
	name = "P";
	constructor(permissionChecker: IPermissionChecker, model: RequirePermissionsSimpleBatchStateCheckerModel<TState>) {
		this.model = model;
		this._permissionChecker = permissionChecker;
	}
	isEnabled(_context: SimpleStateCheckerContext<TState>): boolean {
		return this._permissionChecker.isGranted(this.model.permissions, this.model.requiresAll);
	}

	serialize(): string {
		return JSON.stringify({
			A: this.model.requiresAll,
			N: this.model.permissions,
			T: this.name,
		});
	}
}

export class RequirePermissionsSimpleBatchStateChecker<TState extends IHasSimpleStateCheckers<TState>>
	implements ISimpleBatchStateChecker<TState>
{
	_permissionChecker: IPermissionChecker;
	models: RequirePermissionsSimpleBatchStateCheckerModel<TState>[];
	name = "P";
	constructor(permissionChecker: IPermissionChecker, models: RequirePermissionsSimpleBatchStateCheckerModel<TState>[]) {
		this.models = models;
		this._permissionChecker = permissionChecker;
	}
	isEnabled(context: SimpleBatchStateCheckerContext<TState>) {
		// 1. Initialize a Map instead of a plain object
		const result = new Map<TState, boolean>();

		context.states.forEach((state) => {
			const model = this.models.find((x) => x.state === state);
			if (model) {
				// 2. Use .set() to map the object key to the boolean value
				result.set(model.state, this._permissionChecker.isGranted(model.permissions, model.requiresAll));
			}
		});

		return result;
	}

	serialize(): string | undefined {
		return undefined;
	}
}

export function useRequirePermissionsSimpleStateChecker<TState extends IHasSimpleStateCheckers<TState>>(
	model: RequirePermissionsSimpleBatchStateCheckerModel<TState>,
): ISimpleStateChecker<TState> {
	const permissionChecker = useAuthorization();
	return new RequirePermissionsSimpleStateChecker<TState>(permissionChecker, model);
}

export function useRequirePermissionsSimpleBatchStateChecker<TState extends IHasSimpleStateCheckers<TState>>(
	models: RequirePermissionsSimpleBatchStateCheckerModel<TState>[],
): ISimpleBatchStateChecker<TState> {
	const permissionChecker = useAuthorization();
	return new RequirePermissionsSimpleBatchStateChecker<TState>(permissionChecker, models);
}
