import { Card, Pagination, Radio, Space, Typography } from "antd";

import { SvgIcon } from "@/components/icon";
// import useLocale from "@/locales/useLocale";

import { themeVars } from "@/theme/theme.css";
import { LocalEnum } from "#/enum";
import { useLanguage, useLocale, useSetLocale } from "@/store/localeI18nStore";
import { useTranslation } from "react-i18next";

export default function MultiLanguagePage() {
	// const {
	// 	setLocale,
	// 	locale,
	// 	language: { icon, label },
	// } = useLocale();
	const locale = useLocale(); // 当前语言标识
	const { icon, label } = useLanguage(); // 当前语言对象
	const { i18n } = useTranslation();
	const setLocale = useSetLocale(); // 切换语言的方法

	return (
		<Space direction="vertical" size="middle" style={{ display: "flex" }}>
			<Typography.Link href="https://www.i18next.com/" style={{ color: themeVars.colors.palette.primary.default }}>
				https://www.i18next.com
			</Typography.Link>
			<Typography.Link
				href="https://ant.design/docs/react/i18n-cn"
				style={{ color: themeVars.colors.palette.primary.default }}
			>
				https://ant.design/docs/react/i18n-cn
			</Typography.Link>
			<Card title="Flexible">
				<Radio.Group onChange={(e) => setLocale(e.target.value, i18n)} value={locale}>
					<Radio value={LocalEnum.en_US}>English</Radio>
					<Radio value={LocalEnum.zh_CN}>Chinese</Radio>
				</Radio.Group>

				<div className="flex items-center text-4xl">
					<SvgIcon icon={icon} className="mr-4 rounded-md" size="30" />
					{label}
				</div>
			</Card>

			<Card title="System">
				<Pagination defaultCurrent={1} total={50} showSizeChanger showQuickJumper />
			</Card>
		</Space>
	);
}
