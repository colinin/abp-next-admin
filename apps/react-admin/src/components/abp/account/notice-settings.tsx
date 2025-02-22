import { Card, Empty } from "antd";
import { useTranslation } from "react-i18next";

const BindSettings: React.FC = () => {
	const { t: $t } = useTranslation();

	return (
		<Card bordered={false} title={$t("abp.account.settings.noticeSettings")}>
			<Empty />
		</Card>
	);
};

export default BindSettings;
