import { Form, Input, Modal, Select } from "antd";
import { useTranslation } from "react-i18next";
import { useQuery } from "@tanstack/react-query";
import type { IdentityUserDto } from "#/management/identity/user";
import { getApi as getApplication } from "@/api/openiddict/applications";
import { getApi as getAuthorization } from "@/api/openiddict/authorizations";
import { findByIdApi } from "@/api/management/identity/user-lookup";
import { hasAccessByCodes } from "@/utils/abp/access-checker";
import { UserLookupPermissions } from "@/constants/management/identity/permissions";
import JsonEdit from "@/components/abp/common/json-edit";
import { useEffect } from "react";
import { CircleLoading } from "@/components/loading";

interface Props {
	visible: boolean;
	authorizationId?: string;
	onClose: () => void;
}

const AuthorizationModal: React.FC<Props> = ({ visible, authorizationId, onClose }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();

	// 获取授权详情和关联数据
	const { data: authorizationData, isLoading } = useQuery({
		queryKey: ["authorization", authorizationId],
		queryFn: async () => {
			if (!authorizationId) {
				return Promise.reject(new Error("authorizationId is undefined"));
			}

			const authorization = await getAuthorization(authorizationId);
			if (!authorization.applicationId) {
				return Promise.reject(new Error("authorization.applicationId is undefined"));
			}
			const application = await getApplication(authorization.applicationId);

			let subjectInfo: IdentityUserDto | undefined;
			if (hasAccessByCodes([UserLookupPermissions.Default])) {
				if (authorization.subject) {
					subjectInfo = await findByIdApi(authorization.subject);
				}
			}

			return {
				...authorization,
				applicationId: `${application.clientId}(${authorization.applicationId})`,
				subject: subjectInfo?.userName ? `${subjectInfo.userName}(${authorization.subject})` : authorization.subject,
			};
		},
		enabled: visible && !!authorizationId,
	});

	// 更新表单数据
	useEffect(() => {
		if (authorizationData && authorizationData.id === authorizationId) {
			form.setFieldsValue(authorizationData);
		}
	}, [authorizationData]);

	const formItems = [
		{
			name: "applicationId",
			label: $t("AbpOpenIddict.DisplayName:ApplicationId"),
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
			name: "creationDate",
			label: $t("AbpOpenIddict.DisplayName:CreationDate"),
			component: Input,
		},
		{
			name: "status",
			label: $t("AbpOpenIddict.DisplayName:Status"),
			component: Input,
		},
		{
			name: "scopes",
			label: $t("AbpOpenIddict.DisplayName:Scopes"),
			component: Select,
			props: {
				mode: "tags" as const,
				open: false,
			},
		},
		{
			name: "properties",
			label: $t("AbpOpenIddict.DisplayName:Properties"),
			render: (value: string) => <JsonEdit data={value || ""} />,
		},
	];

	return (
		<Modal open={visible} title={$t("AbpOpenIddict.Authorizations")} onCancel={onClose} footer={null} width="50%">
			{!!authorizationId && visible && isLoading ? (
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
								<item.component className="w-full" readOnly {...item.props} />
							)}
						</Form.Item>
					))}
				</Form>
			)}
		</Modal>
	);
};

export default AuthorizationModal;
