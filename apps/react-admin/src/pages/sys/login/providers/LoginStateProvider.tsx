import { type PropsWithChildren, createContext, useContext, useMemo, useState } from "react";

export enum LoginStateEnum {
	LOGIN = 0,
	REGISTER = 1,
	RESET_PASSWORD = 2,
	MOBILE = 3,
	QR_CODE = 4,
}

interface LoginStateContextType {
	loginState: LoginStateEnum;
	isExternalLoginState: boolean;
	setLoginState: (loginState: LoginStateEnum) => void;
	setIsExternalLoginState: (isExternalLogin: boolean) => void;
	backToLogin: () => void;
}
const LoginStateContext = createContext<LoginStateContextType>({
	loginState: LoginStateEnum.LOGIN,
	isExternalLoginState: false,
	setLoginState: () => {},
	setIsExternalLoginState: () => {},
	backToLogin: () => {},
});

export function useLoginStateContext() {
	const context = useContext(LoginStateContext);
	return context;
}

export function LoginStateProvider({ children }: PropsWithChildren) {
	const [loginState, setLoginState] = useState(LoginStateEnum.LOGIN);
	const [isExternalLoginState, setIsExternalLoginState] = useState(false);

	function backToLogin() {
		setLoginState(LoginStateEnum.LOGIN);
	}

	const value: LoginStateContextType = useMemo(
		() => ({ loginState, setLoginState, backToLogin, isExternalLoginState, setIsExternalLoginState }),
		[loginState, isExternalLoginState],
	);

	return <LoginStateContext.Provider value={value}>{children}</LoginStateContext.Provider>;
}
