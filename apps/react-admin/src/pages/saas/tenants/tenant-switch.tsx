import type React from "react";
import { useState } from "react";
import { Button, Input, Modal, Form } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { findTenantByNameApi } from "@/api/saas/multi-tenancy";
import useAbpStore, { useApplication } from "@/store/abpCoreStore";

interface Props {
	onChange: () => void;
}

const TenantSwitch: React.FC<Props> = ({ onChange }) => {
	const { t: $t } = useTranslation();
	const abpStore = useAbpStore();
	const application = useApplication();
	const currentTenant = application?.currentTenant;

	const [visible, setVisible] = useState(false);
	const [form] = Form.useForm();
	const [submitting, setSubmitting] = useState(false);

	const handleOpen = () => {
		form.setFieldsValue({ name: currentTenant?.name });
		setVisible(true);
	};

	const handleSwitch = async () => {
		try {
			const values = await form.validateFields();
			setSubmitting(true);

			let tenantId: string | undefined = undefined;
			// Clear tenant cookie to avoid conflicts
			// cookies.remove('__tenant');
			document.cookie = "__tenant=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";

			if (values.name) {
				const result = await findTenantByNameApi(values.name);

				if (!result.success) {
					toast.warning($t("AbpUiMultiTenancy.GivenTenantIsNotExist", { 0: values.name }));
					setSubmitting(false);
					return;
				}

				if (!result.isActive) {
					toast.warning($t("AbpUiMultiTenancy.GivenTenantIsNotAvailable", { 0: values.name }));
					setSubmitting(false);
					return;
				}
				tenantId = result.tenantId;
			}

			abpStore.actions.setTenantId(tenantId);
			// console.log("Switching to tenant:", tenantId);
			onChange();

			setVisible(false);
		} catch (e) {
			console.error(e);
		} finally {
			setSubmitting(false);
		}
	};

	return (
		<div className="w-full">
			<Input.Search
				readOnly
				value={currentTenant?.name}
				placeholder={$t("AbpUiMultiTenancy.NotSelected")}
				enterButton={<Button onClick={handleOpen}>({$t("AbpUiMultiTenancy.Switch")})</Button>}
				onSearch={handleOpen}
			/>

			<Modal
				title={$t("AbpUiMultiTenancy.SwitchTenant")}
				open={visible}
				onCancel={() => setVisible(false)}
				onOk={handleSwitch}
				confirmLoading={submitting}
				destroyOnClose
			>
				<Form form={form} layout="vertical">
					<Form.Item name="name" label={$t("AbpUiMultiTenancy.Name")}>
						<Input placeholder={$t("AbpUiMultiTenancy.SwitchTenantHint")} allowClear />
					</Form.Item>
				</Form>
			</Modal>
		</div>
	);
};

export default TenantSwitch;
