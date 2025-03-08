import { Button, Descriptions, Table, Tag, type TableColumnType } from "antd";
import { useTranslation } from "react-i18next";
import type { IdentitySessionDto } from "#/management/identity/sessions";
import { formatToDateTime } from "@/utils/abp";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { IdentitySessionPermissions } from "@/constants/management/identity/permissions";
import { useApplication } from "@/store/abpCoreStore";
import { Space } from "antd";

interface Props {
	sessions: IdentitySessionDto[];
	onRevoke: (session: IdentitySessionDto) => void;
}

const UserSessionTable: React.FC<Props> = ({ sessions, onRevoke }) => {
	const { t: $t } = useTranslation();
	const application = useApplication();

	const isCurrentSession = (sessionId: string) => {
		return application?.currentUser.sessionId === sessionId;
	};

	const allowRevokeSession = (session: IdentitySessionDto) => {
		if (isCurrentSession(session.sessionId)) {
			return false;
		}
		return hasAccessByCodes([IdentitySessionPermissions.Revoke]);
	};

	const expandedRowRender = (record: IdentitySessionDto) => {
		return (
			<Descriptions bordered size="small" column={2}>
				<Descriptions.Item label={$t("AbpIdentity.DisplayName:SessionId")} span={2}>
					{record.sessionId}
				</Descriptions.Item>
				<Descriptions.Item label={$t("AbpIdentity.DisplayName:Device")}>{record.device}</Descriptions.Item>
				<Descriptions.Item label={$t("AbpIdentity.DisplayName:DeviceInfo")}>{record.deviceInfo}</Descriptions.Item>
				<Descriptions.Item label={$t("AbpIdentity.DisplayName:ClientId")}>{record.clientId}</Descriptions.Item>
				<Descriptions.Item label={$t("AbpIdentity.DisplayName:IpAddresses")}>{record.ipAddresses}</Descriptions.Item>
				<Descriptions.Item label={$t("AbpIdentity.DisplayName:SignedIn")}>
					{formatToDateTime(record.signedIn)}
				</Descriptions.Item>
				<Descriptions.Item label={$t("AbpIdentity.DisplayName:LastAccessed")}>
					{formatToDateTime(record.lastAccessed)}
				</Descriptions.Item>
			</Descriptions>
		);
	};

	const columns: TableColumnType<IdentitySessionDto>[] = [
		{
			title: $t("AbpIdentity.DisplayName:Device"),
			dataIndex: "device",
			width: 150,
			render: (_, record) => (
				<Space>
					<span>{record.device}</span>
					{isCurrentSession(record.sessionId) && <Tag color="#87d068">{$t("AbpIdentity.CurrentSession")}</Tag>}
				</Space>
			),
		},
		{
			title: $t("AbpIdentity.DisplayName:SignedIn"),
			dataIndex: "signedIn",
			render: (value) => formatToDateTime(value),
		},
		{
			title: $t("AbpUi.Actions"),
			width: 120,
			fixed: "right",
			render: (_, record) => (
				<div>
					{allowRevokeSession(record) && (
						<Button danger size="small" onClick={() => onRevoke(record)}>
							{$t("AbpIdentity.RevokeSession")}
						</Button>
					)}
				</div>
			),
		},
	];

	return (
		<Table<IdentitySessionDto>
			columns={columns}
			dataSource={sessions}
			rowKey="sessionId"
			expandable={{
				expandedRowRender,
				expandRowByClick: true,
			}}
			pagination={false}
		/>
	);
};

export default UserSessionTable;
