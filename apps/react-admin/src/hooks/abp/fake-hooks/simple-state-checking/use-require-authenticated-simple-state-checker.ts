import type {
	CurrentUser,
	IHasSimpleStateCheckers,
	ISimpleStateChecker,
	SimpleStateCheckerContext,
} from "#/abp-core/global";
import useAbpStore from "@/store/abpCoreStore";

export interface RequireAuthenticatedStateChecker {
	name: string;
}

export class RequireAuthenticatedSimpleStateChecker<TState extends IHasSimpleStateCheckers<TState>>
	implements ISimpleStateChecker<TState>, RequireAuthenticatedStateChecker
{
	_currentUser?: CurrentUser;
	name = "A";
	constructor(currentUser?: CurrentUser) {
		this._currentUser = currentUser;
	}
	isEnabled(_context: SimpleStateCheckerContext<TState>): boolean {
		return this._currentUser?.isAuthenticated ?? false;
	}

	serialize(): string {
		return JSON.stringify({
			T: this.name,
		});
	}
}

export function requireAuthenticatedSimpleStateChecker<
	TState extends IHasSimpleStateCheckers<TState>,
>(): ISimpleStateChecker<TState> {
	const { application } = useAbpStore.getState();
	return new RequireAuthenticatedSimpleStateChecker<TState>(application?.currentUser);
}
