import { Helmet } from "react-helmet-async";

import Logo from "@/assets/images/logo.png";
import Router from "@/router/index";

import { MotionLazy } from "./components/animate/motion-lazy";
import Toast from "./components/toast";
import { AntdAdapter } from "./theme/adapter/antd.adapter";
import { ThemeProvider } from "./theme/theme-provider";
import { useEffect } from "react";

import { useSetLocale } from "./store/localeI18nStore";
import { useTranslation } from "react-i18next";
import { getStringItem } from "./utils/storage";
import { LocalEnum, StorageEnum } from "#/enum";
import { useSessions } from "./hooks/abp/use-sessions";

function App() {
	const { i18n } = useTranslation();
	const setLocale = useSetLocale();
	const defaultLng = getStringItem(StorageEnum.I18N) || LocalEnum.en_US;
	useSessions();
	useEffect(() => {
		async function initializeI18n() {
			await setLocale(defaultLng as LocalEnum, i18n);
		}
		initializeI18n(); //触发abp语言包加载
	}, [defaultLng, i18n, setLocale]);
	return (
		<ThemeProvider adapters={[AntdAdapter]}>
			<MotionLazy>
				<Helmet>
					<title>Slash Admin</title>
					<link rel="icon" href={Logo} />
				</Helmet>
				<Toast />

				<Router />
			</MotionLazy>
		</ThemeProvider>
	);
}

export default App;
