import type React from "react";
import { useEffect, useState } from "react";
import { Modal, Form, Input, TreeSelect } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { getAllApi } from "@/api/platform/menus";
import { createApi } from "@/api/platform/my-favorite-menus";
import type { MenuDto } from "#/platform/menus";
import type { UserFavoriteMenuDto } from "#/platform/favorites";
import { ColorPicker } from "antd";
import IconPicker from "@/components/abp/adapter/icon-picker";
import { listToTree } from "@/utils/tree";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: UserFavoriteMenuDto) => void;
}

const WorkbenchQuickNavModal: React.FC<Props> = ({ visible, onClose, onChange }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const [availableMenus, setAvailableMenus] = useState<MenuDto[]>([]);
	const [submitting, setSubmitting] = useState(false);
	// const { uiFramework } = useAppConfig(); // Retrieve global config
	const uiFramework = "react"; // Placeholder for global config

	useEffect(() => {
		if (visible) {
			initMenus();
			form.resetFields();
			// Default color
			form.setFieldValue("color", "#000000");
		}
	}, [visible]);

	const initMenus = async () => {
		try {
			const { items } = await getAllApi({ framework: uiFramework });
			const tree = listToTree(items, { id: "id", pid: "parentId" });
			setAvailableMenus(tree);
		} catch (e) {
			console.error(e);
		}
	};

	const handleSubmit = async () => {
		try {
			const values = await form.validateFields();
			setSubmitting(true);

			const colorHex = typeof values.color === "string" ? values.color : values.color?.toHexString();

			const menuDto = await createApi({
				framework: uiFramework,
				menuId: values.menuId,
				color: colorHex,
				icon: values.icon,
				aliasName: values.aliasName,
			});

			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(menuDto);
			onClose();
		} catch (error) {
			console.error(error);
		} finally {
			setSubmitting(false);
		}
	};

	return (
		<Modal
			title={$t("workbench.content.favoriteMenu.manage")}
			open={visible}
			onCancel={onClose}
			onOk={handleSubmit}
			confirmLoading={submitting}
			destroyOnClose
		>
			<Form form={form} layout="vertical">
				<Form.Item
					name="menuId"
					label={$t("workbench.content.favoriteMenu.select")}
					rules={[{ required: true, message: $t("AbpUi.ThisFieldIsRequired") }]}
				>
					<TreeSelect
						treeData={availableMenus}
						fieldNames={{ label: "displayName", value: "id", children: "children" }}
						allowClear
						showSearch
						treeDefaultExpandAll
						placeholder={$t("ui.placeholder.select")}
					/>
				</Form.Item>

				<Form.Item name="color" label={$t("workbench.content.favoriteMenu.color")}>
					<ColorPicker showText />
				</Form.Item>

				<Form.Item name="aliasName" label={$t("workbench.content.favoriteMenu.alias")}>
					<Input autoComplete="off" />
				</Form.Item>

				<Form.Item name="icon" label={$t("workbench.content.favoriteMenu.icon")}>
					<IconPicker />
				</Form.Item>
			</Form>
		</Modal>
	);
};

export default WorkbenchQuickNavModal;
