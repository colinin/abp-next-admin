import type React from "react";
import { Drawer, Descriptions } from "antd";
import { formatToDateTime } from "@/utils/abp";
import { useTranslation } from "react-i18next";
import { useQuery } from "@tanstack/react-query";
import { getApi } from "@/api/management/identity/security-logs";

interface Props {
	visible: boolean;
	onClose: () => void;
	securityLogId?: string;
}

const SecurityLogDrawer: React.FC<Props> = ({ visible, onClose, securityLogId }) => {
	const { t: $t } = useTranslation();

	const { data: formModel, isLoading } = useQuery({
		queryKey: ["securityLog", securityLogId],
		queryFn: () => {
			if (!securityLogId) {
				return Promise.reject(new Error("securityLogId is undefined"));
			}
			return getApi(securityLogId);
		},
		enabled: visible && !!securityLogId,
	});

	return (
		<Drawer
			title={$t("AbpAuditLogging.SecurityLog")}
			open={visible}
			onClose={onClose}
			width={800}
			loading={isLoading}
			destroyOnClose
		>
			<Descriptions bordered size="small" column={2} labelStyle={{ width: "110px" }} colon={false}>
				<Descriptions.Item label={$t("AbpAuditLogging.ApplicationName")}>
					{formModel?.applicationName}
				</Descriptions.Item>
				<Descriptions.Item label={$t("AbpAuditLogging.CreationTime")}>
					{formModel?.creationTime ? formatToDateTime(formModel.creationTime) : ""}
				</Descriptions.Item>
				<Descriptions.Item label={$t("AbpAuditLogging.Identity")}>{formModel?.identity}</Descriptions.Item>
				<Descriptions.Item label={$t("AbpAuditLogging.TenantName")}>{formModel?.tenantName}</Descriptions.Item>
				<Descriptions.Item label={$t("AbpAuditLogging.Actions")}>{formModel?.action}</Descriptions.Item>
				<Descriptions.Item label={$t("AbpAuditLogging.CorrelationId")}>{formModel?.correlationId}</Descriptions.Item>
				<Descriptions.Item label={$t("AbpAuditLogging.UserId")}>{formModel?.userId}</Descriptions.Item>
				<Descriptions.Item label={$t("AbpAuditLogging.UserName")}>{formModel?.userName}</Descriptions.Item>
				<Descriptions.Item label={$t("AbpAuditLogging.ClientId")}>{formModel?.clientId}</Descriptions.Item>
				<Descriptions.Item label={$t("AbpAuditLogging.ClientIpAddress")}>
					{formModel?.clientIpAddress}
				</Descriptions.Item>
				<Descriptions.Item label={$t("AbpAuditLogging.BrowserInfo")} span={2}>
					{formModel?.browserInfo}
				</Descriptions.Item>

				<Descriptions.Item label={$t("AbpAuditLogging.Additional")} span={2}>
					{formModel?.extraProperties ? JSON.stringify(formModel.extraProperties, null, 2) : ""}
				</Descriptions.Item>
			</Descriptions>
		</Drawer>
	);
};

export default SecurityLogDrawer;
