import type React from "react";
import { useEffect, useState } from "react";
import { Modal, Form, Tree, TreeSelect } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { getAllApi as getAllMenusApi } from "@/api/platform/menus";
import { getAllApi as getUserMenusApi, setMenusApi as setUserMenusApi } from "@/api/platform/user-menus";
import { getAllApi as getRoleMenusApi, setMenusApi as setRoleMenusApi } from "@/api/platform/role-menus";
import { getByNameApi as getDataDictionaryByNameApi } from "@/api/platform/data-dictionaries";
import type { MenuSubject } from "./types";
import { listToTree } from "@/utils/tree";
import ApiSelect from "@/components/abp/adapter/api-select";

interface Props {
	visible: boolean;
	onClose: () => void;
	subject: MenuSubject;
	identity: string; // userId or roleName
}

const MenuAllotModal: React.FC<Props> = ({ visible, onClose, subject, identity }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();
	const [submitting, setSubmitting] = useState(false);
	const [treeData, setTreeData] = useState<any[]>([]);
	const [checkedKeys, setCheckedKeys] = useState<React.Key[]>([]);

	// Watch framework to trigger tree data reload
	const framework = Form.useWatch("framework", form);

	useEffect(() => {
		if (visible) {
			form.resetFields();
			setTreeData([]);
			setCheckedKeys([]);
		}
	}, [visible, identity]);

	useEffect(() => {
		if (visible && framework && identity) {
			loadMenus(framework);
		}
	}, [framework, identity, visible]);

	const loadMenus = async (fw: string) => {
		try {
			// Fetch all menus for the framework to build the tree structure
			const allMenusRes = await getAllMenusApi({ framework: fw });
			const tree = listToTree(allMenusRes.items, { id: "id", pid: "parentId" });
			setTreeData(tree);

			// Fetch currently assigned menus for the user/role
			const assignedRes =
				subject === "user"
					? await getUserMenusApi({ framework: fw, userId: identity })
					: await getRoleMenusApi({ framework: fw, role: identity });

			const assignedIds = assignedRes.items.map((item) => item.id);
			setCheckedKeys(assignedIds);

			// Set startup menu
			const startupMenu = assignedRes.items.find((item) => item.startup);
			if (startupMenu) {
				form.setFieldValue("startupMenuId", startupMenu.id);
			}
		} catch (error) {
			console.error(error);
		}
	};

	const handleCheck = (checked: React.Key[] | { checked: React.Key[]; halfChecked: React.Key[] }) => {
		// AntD Tree checkStrictly returns object, normally array
		if (Array.isArray(checked)) {
			setCheckedKeys(checked);
		} else {
			setCheckedKeys(checked.checked);
		}
	};

	const handleSubmit = async () => {
		try {
			const values = await form.validateFields();
			setSubmitting(true);

			const payload = {
				framework: values.framework,
				menuIds: checkedKeys as string[],
				startupMenuId: values.startupMenuId,
			};

			if (subject === "user") {
				await setUserMenusApi({ ...payload, userId: identity });
			} else {
				await setRoleMenusApi({ ...payload, roleName: identity });
			}

			toast.success($t("AbpUi.SavedSuccessfully"));
			onClose();
		} catch (error) {
			console.error(error);
		} finally {
			setSubmitting(false);
		}
	};

	// Helper for ApiSelect
	const uiFrameworkApi = () => getDataDictionaryByNameApi("UI Framework");

	return (
		<Modal
			title={$t("AppPlatform.Menu:Manage")}
			open={visible}
			onCancel={onClose}
			onOk={handleSubmit}
			confirmLoading={submitting}
			destroyOnClose
			width={600}
		>
			<Form form={form} layout="vertical">
				<Form.Item
					name="framework"
					label={$t("AppPlatform.DisplayName:UIFramework")}
					rules={[{ required: true, message: $t("AbpUi.ThisFieldIsRequired") }]}
				>
					<ApiSelect api={uiFrameworkApi} resultField="items" labelField="displayName" valueField="name" allowClear />
				</Form.Item>

				{framework && (
					<>
						<Form.Item name="startupMenuId" label={$t("AppPlatform.Menu:SetStartup")}>
							<TreeSelect
								treeData={treeData}
								fieldNames={{ label: "displayName", value: "id", children: "children" }}
								allowClear
								treeDefaultExpandAll
								placeholder={$t("ui.placeholder.select")}
							/>
						</Form.Item>

						<Form.Item label={$t("AppPlatform.DisplayName:Menus")} required>
							<div className="border rounded p-2 max-h-[400px] overflow-auto">
								<Tree
									checkable
									checkStrictly
									treeData={treeData}
									fieldNames={{ title: "displayName", key: "id", children: "children" }}
									checkedKeys={checkedKeys}
									onCheck={handleCheck}
									defaultExpandAll
								/>
							</div>
						</Form.Item>
					</>
				)}
			</Form>
		</Modal>
	);
};

export default MenuAllotModal;
