import { useState } from "react";
import { Button, Input, Modal } from "antd";
import { useTranslation } from "react-i18next";
import type { SettingsUpdateInput } from "#/management/settings/settings";

import {
	getGlobalSettingsApi,
	getTenantSettingsApi,
	sendTestEmailApi,
	setGlobalSettingsApi,
	setTenantSettingsApi,
} from "@/api/management/settings/settings";
import SettingForm from "./setting-form";
import { toast } from "sonner";
import { isEmail } from "@/utils/abp/regex";
import { useApplication } from "@/store/abpCoreStore";

const SystemSetting: React.FC = () => {
	const { t: $t } = useTranslation();
	// const [form] = Form.useForm();
	const [sending, setSending] = useState(false);
	const application = useApplication();

	const [modal, contextHolder] = Modal.useModal();
	const handleGet = async () => {
		const api = application?.currentTenant.isAvailable ? getTenantSettingsApi : getGlobalSettingsApi;
		const { items } = await api();
		return items;
	};

	const handleSubmit = async (input: SettingsUpdateInput) => {
		const api = application?.currentTenant.isAvailable ? setTenantSettingsApi : setGlobalSettingsApi;
		await api(input);
	};

	const handleSendMail = async (email: string) => {
		if (!isEmail(email)) {
			modal.warning({
				title: $t("AbpValidation.ThisFieldIsNotValid"),
				content: $t("AbpValidation.The {0} field is not a valid e-mail address", {
					0: $t("AbpSettingManagement.TargetEmailAddress"),
				}),
				centered: true,
			});
			55;
			return;
		}

		try {
			setSending(true);
			await sendTestEmailApi(email);
			toast.success($t("AbpSettingManagement.SuccessfullySent"));
		} finally {
			setSending(false);
		}
	};

	const SendTestEmailSlot: React.FC<{
		detail: any;
		onChange: (setting: any) => void;
	}> = ({ detail, onChange }) => (
		<Input.Search
			// value={detail.value}
			loading={sending}
			placeholder={$t("AbpSettingManagement.TargetEmailAddress")}
			onSearch={handleSendMail}
			onChange={(e) => {
				detail.value = e.target.value;
				onChange(detail);
			}}
			enterButton={
				<Button loading={sending} type="primary">
					{$t("AbpSettingManagement.Send")}
				</Button>
			}
		/>
	);

	return (
		<>
			{contextHolder}
			<SettingForm
				getApi={handleGet}
				submitApi={handleSubmit}
				onChange={() => {}}
				slots={{
					"send-test-email": SendTestEmailSlot,
				}}
			/>
		</>
	);
};

export default SystemSetting;
