import { useState } from "react";
import { Card, Tabs } from "antd";
import { useTranslation } from "react-i18next";
import OrganizationUnitRoleTable from "./organization-unit-role-table";
import OrganizationUnitUserTable from "./organization-unit-user-table";

interface Props {
	selectedKey?: string;
}

type TabKeys = "users" | "roles";

const OrganizationUnitTable: React.FC<Props> = ({ selectedKey }) => {
	const { t: $t } = useTranslation();
	const [activeTab, setActiveTab] = useState<TabKeys>("users");

	return (
		<Card>
			<Tabs activeKey={activeTab} onChange={(key) => setActiveTab(key as TabKeys)}>
				<Tabs.TabPane key="users" tab={$t("AbpIdentity.Users")} />
				<Tabs.TabPane key="roles" tab={$t("AbpIdentity.Roles")} />
			</Tabs>
			{activeTab === "users" ? (
				<OrganizationUnitUserTable selectedKey={selectedKey} />
			) : (
				<OrganizationUnitRoleTable selectedKey={selectedKey} />
			)}
		</Card>
	);
};

export default OrganizationUnitTable;
