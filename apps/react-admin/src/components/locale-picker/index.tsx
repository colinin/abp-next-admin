import { Dropdown } from "antd";

import { IconButton, SvgIcon } from "../icon";

import type { MenuProps } from "antd";
import type { LocalEnum } from "#/enum";
import { LANGUAGE_MAP, useLocale, useSetLocale } from "@/store/localeI18nStore";
import { useTranslation } from "react-i18next";

type Locale = keyof typeof LocalEnum;

/**
 * Locale Picker
 */
export default function LocalePicker() {
	const locale = useLocale(); // 当前语言标识
	const setLocale = useSetLocale(); // 切换语言的方法
	const { i18n } = useTranslation();
	const localeList: MenuProps["items"] = Object.values(LANGUAGE_MAP).map((item) => {
		return {
			key: item.locale,
			label: item.label,
			icon: <SvgIcon icon={item.icon} size="20" className="rounded-md" />,
		};
	});

	return (
		<Dropdown
			placement="bottomRight"
			trigger={["click"]}
			menu={{ items: localeList, onClick: (e) => setLocale(e.key as Locale, i18n) }}
		>
			<IconButton className="h-10 w-10 hover:scale-105">
				<SvgIcon icon={`ic-locale_${locale}`} size="24" className="rounded-md" />
			</IconButton>
		</Dropdown>
	);
}
