import { Form, Input, Modal } from "antd";
import { useTranslation } from "react-i18next";
import { useQuery } from "@tanstack/react-query";
import type { IdentityUserDto } from "#/management/identity/user";
import { getApi as getApplication } from "@/api/openiddict/applications";
import { getApi as getToken } from "@/api/openiddict/tokens";
import { findByIdApi } from "@/api/management/identity/user-lookup";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { UserLookupPermissions } from "@/constants/management/identity/permissions";
import JsonEdit from "@/components/abp/common/json-edit";
import { useEffect } from "react";
import { CircleLoading } from "@/components/loading";

interface Props {
	visible: boolean;
	tokenId?: string;
	onClose: () => void;
}

const TokenModal: React.FC<Props> = ({ visible, tokenId, onClose }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();

	// 获取Token详情和关联数据
	const { data: tokenData, isLoading } = useQuery({
		queryKey: ["token", tokenId],
		queryFn: async () => {
			if (!tokenId) {
				return Promise.reject(new Error("tokenId is undefined"));
			}

			const token = await getToken(tokenId);
			if (!token.applicationId) {
				return Promise.reject(new Error("token.applicationId is undefined"));
			}
			const application = await getApplication(token.applicationId);

			let subjectInfo: IdentityUserDto | undefined;
			if (hasAccessByCodes([UserLookupPermissions.Default])) {
				if (token.subject) {
					subjectInfo = await findByIdApi(token.subject);
				}
			}

			return {
				...token,
				applicationId: `${application.clientId}(${token.applicationId})`,
				subject: subjectInfo?.userName ? `${subjectInfo.userName}(${token.subject})` : token.subject,
			};
		},
		enabled: visible && !!tokenId,
	});

	// 更新表单数据
	useEffect(() => {
		if (tokenData && tokenData.id === tokenId) {
			form.setFieldsValue(tokenData);
		}
	}, [tokenData]);

	const formItems = [
		{
			name: "applicationId",
			label: $t("AbpOpenIddict.DisplayName:ApplicationId"),
			component: Input,
		},
		{
			name: "authorizationId",
			label: $t("AbpOpenIddict.DisplayName:AuthorizationId"),
			component: Input,
		},
		{
			name: "subject",
			label: $t("AbpOpenIddict.DisplayName:Subject"),
			component: Input,
		},
		{
			name: "type",
			label: $t("AbpOpenIddict.DisplayName:Type"),
			component: Input,
		},
		{
			name: "status",
			label: $t("AbpOpenIddict.DisplayName:Status"),
			component: Input,
		},
		{
			name: "creationDate",
			label: $t("AbpOpenIddict.DisplayName:CreationDate"),
			component: Input,
		},
		{
			name: "expirationDate",
			label: $t("AbpOpenIddict.DisplayName:ExpirationDate"),
			component: Input,
		},
		{
			name: "redemptionDate",
			label: $t("AbpOpenIddict.DisplayName:RedemptionDate"),
			component: Input,
		},
		{
			name: "referenceId",
			label: $t("AbpOpenIddict.DisplayName:ReferenceId"),
			component: Input,
		},
		{
			name: "payload",
			label: $t("AbpOpenIddict.DisplayName:Payload"),
			render: (value: string) => <JsonEdit data={value || ""} />,
		},
	];

	return (
		<Modal open={visible} title={$t("AbpOpenIddict.Tokens")} onCancel={onClose} footer={null} width="50%">
			{!!tokenId && visible && isLoading ? (
				<CircleLoading />
			) : (
				<Form
					form={form}
					layout="vertical"
					disabled // 表单只读
				>
					{formItems.map((item) => (
						<Form.Item key={item.name} name={item.name} label={item.label}>
							{item.render ? (
								item.render(form.getFieldValue(item.name))
							) : (
								<item.component className="w-full" readOnly />
							)}
						</Form.Item>
					))}
				</Form>
			)}
		</Modal>
	);
};

export default TokenModal;
