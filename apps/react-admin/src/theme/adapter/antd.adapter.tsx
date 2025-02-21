import { StyleProvider } from "@ant-design/cssinjs";
import type { ThemeConfig } from "antd";
import { App, ConfigProvider, theme } from "antd";
import { ThemeMode } from "#/enum";
import { baseThemeTokens } from "../tokens/base";
import { darkColorTokens, lightColorTokens, presetsColors } from "../tokens/color";
import type { UILibraryAdapter } from "../type";

import { useSettings } from "@/store/settingStore";
import { removePx } from "@/utils/theme";
import { lightShadowTokens } from "../tokens/shadow";
import { darkShadowTokens } from "../tokens/shadow";
import { typographyTokens } from "../tokens/typography";
import { useLanguage } from "@/store/localeI18nStore";

export const AntdAdapter: UILibraryAdapter = ({ mode, children }) => {
	const language = useLanguage();
	const { themeColorPresets } = useSettings();
	const algorithm = mode === ThemeMode.Light ? theme.defaultAlgorithm : theme.darkAlgorithm;

	const colorTokens = mode === ThemeMode.Light ? lightColorTokens : darkColorTokens;
	const shadowTokens = mode === ThemeMode.Light ? lightShadowTokens : darkShadowTokens;

	const primaryColorToken = presetsColors[themeColorPresets];

	const token: ThemeConfig["token"] = {
		colorPrimary: primaryColorToken.default,
		colorSuccess: colorTokens.palette.success.default,
		colorWarning: colorTokens.palette.warning.default,
		colorError: colorTokens.palette.error.default,
		colorInfo: colorTokens.palette.info.default,

		colorBgLayout: colorTokens.background.default,
		colorBgContainer: colorTokens.background.paper,
		colorBgElevated: colorTokens.background.default,

		wireframe: false,

		borderRadiusSM: removePx(baseThemeTokens.borderRadius.sm),
		borderRadius: removePx(baseThemeTokens.borderRadius.default),
		borderRadiusLG: removePx(baseThemeTokens.borderRadius.lg),
	};

	const components: ThemeConfig["components"] = {
		Breadcrumb: {
			fontSize: removePx(typographyTokens.fontSize.xs),
			separatorMargin: removePx(baseThemeTokens.spacing[1]),
		},
		Menu: {
			fontSize: removePx(typographyTokens.fontSize.sm),
			colorFillAlter: "transparent",
			itemColor: colorTokens.text.secondary,
			motionDurationMid: "0.125s",
			motionDurationSlow: "0.125s",
			darkItemBg: darkColorTokens.background.default,
		},
		Layout: {
			siderBg: darkColorTokens.background.default,
		},
		Card: {
			boxShadow: shadowTokens.card,
		},
	};

	return (
		<ConfigProvider
			locale={language.antdLocal}
			theme={{ algorithm, token, components }}
			tag={{
				style: {
					borderRadius: removePx(baseThemeTokens.borderRadius.md),
					fontWeight: 700,
					padding: `0 ${baseThemeTokens.spacing[1]}`,
					margin: `0 ${baseThemeTokens.spacing[1]}`,
					fontSize: removePx(typographyTokens.fontSize.xs),
					borderWidth: 0,
				},
			}}
		>
			<StyleProvider hashPriority="high">
				<App>{children}</App>
			</StyleProvider>
		</ConfigProvider>
	);
};
