import { useEffect, useState } from "react";
import { Card, Form, Input, Avatar, Upload, Button } from "antd";
import { UploadOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import type { ProfileDto, UpdateProfileDto } from "#/account/profile";
import type { UploadChangeParam, UploadProps } from "antd/es/upload";
import { useUserInfo } from "@/store/userStore";
import { faker } from "@faker-js/faker";
import { useAbpSettings } from "@/hooks/abp/use-abp-settings";

interface Props {
	profile: ProfileDto;
	onSubmit: (profile: UpdateProfileDto) => void;
}

const BasicSettings: React.FC<Props> = ({ profile, onSubmit }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const { isTrue } = useAbpSettings();
	const userInfo = useUserInfo();

	const [formModel, setFormModel] = useState<ProfileDto>({} as ProfileDto);

	useEffect(() => {
		setFormModel({ ...profile });
		form.setFieldsValue(profile);
	}, [profile]);

	const handleAvatarChange = (_param: UploadChangeParam) => {
		// TODO: Wait for OSS module integration
		console.warn("Waiting for OSS module integration...");
	};

	const handleBeforeUpload: UploadProps["beforeUpload"] = () => {
		console.warn("Waiting for OSS module integration...");
		return false;
	};

	const handleSubmit = () => {
		onSubmit(formModel);
	};

	return (
		<Card bordered={false} title={$t("abp.account.settings.basic.title")}>
			<div className="flex flex-row">
				<div className="basis-2/4">
					<Form
						form={form}
						labelCol={{ span: 6 }}
						wrapperCol={{ span: 18 }}
						onValuesChange={(_, values) => setFormModel(values)}
					>
						<Form.Item label={$t("AbpAccount.DisplayName:UserName")} name="userName" rules={[{ required: true }]}>
							<Input disabled={!isTrue("Abp.Identity.User.IsUserNameUpdateEnabled")} autoComplete="off" />
						</Form.Item>
						<Form.Item label={$t("AbpAccount.DisplayName:Email")} name="email" rules={[{ required: true }]}>
							<Input disabled={!isTrue("Abp.Identity.User.IsEmailUpdateEnabled")} autoComplete="off" type="email" />
						</Form.Item>
						<Form.Item label={$t("AbpAccount.DisplayName:Surname")} name="surname">
							<Input autoComplete="off" />
						</Form.Item>
						<Form.Item label={$t("AbpAccount.DisplayName:Name")} name="name">
							<Input autoComplete="off" />
						</Form.Item>
						<Form.Item>
							<div className="flex flex-col items-center">
								<Button style={{ minWidth: 100 }} type="primary" onClick={handleSubmit}>
									{$t("AbpUi.Submit")}
								</Button>
							</div>
						</Form.Item>
					</Form>
				</div>
				<div className="basis-2/4">
					<div className="flex flex-col items-center">
						<p>{$t("AbpUi.ProfilePicture")}</p>
						<Avatar size={100} icon={<img src={userInfo?.avatar ?? faker.image.avatarGitHub()} alt="" />} />
						<Upload beforeUpload={handleBeforeUpload} fileList={[]} name="file" onChange={handleAvatarChange}>
							<Button className="mt-4">
								<UploadOutlined />
								{$t("abp.account.settings.changeAvatar")}
							</Button>
						</Upload>
					</div>
				</div>
			</div>
		</Card>
	);
};

export default BasicSettings;
