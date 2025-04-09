import type { SettingsUpdateInput } from "#/management/settings/settings";
import { getUserSettingsApi, setUserSettingsApi } from "@/api/management/settings/settings";
import SettingForm from "./setting-form";

const UserSetting: React.FC = () => {
	const handleGet = async () => {
		const { items } = await getUserSettingsApi();
		return items;
	};

	const handleSubmit = async (input: SettingsUpdateInput) => {
		await setUserSettingsApi(input);
	};

	return <SettingForm getApi={handleGet} submitApi={handleSubmit} onChange={() => {}} />;
};

export default UserSetting;
