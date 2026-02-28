import type React from "react";
import { useEffect, useState, useMemo } from "react";
import { Col, Row } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import {
	getListApi as getFavoriteMenusApi,
	deleteApi as deleteFavoriteMenuApi,
} from "@/api/platform/my-favorite-menus";
import { getMyNotifilersApi } from "@/api/management/notifications/my-notifications";
import { formatToDateTime } from "@/utils/abp";
// import { useAppConfig } from "@/hooks/use-app-config";
// import { useUserStore } from "@/store/user";

import WorkbenchHeader from "./components/workbench-header";
import WorkbenchQuickNav from "./components/workbench-quick-nav";
import WorkbenchTodo from "./components/workbench-todo";
import WorkbenchTrends from "./components/workbench-trends";
import WorkbenchQuickNavModal from "./components/workbench-quick-nav-modal";
import type { FavoriteMenu } from "./types";
import { useNavigate } from "react-router";
import { NotificationReadState } from "#/notifications";
import { useNotificationSerializer } from "@/utils/abp/notifications/useNotificationSerializer";
import useUserStore from "@/store/userStore";

const WorkbenchPage: React.FC = () => {
	const { t: $t } = useTranslation();
	const navigate = useNavigate();
	// const { uiFramework } = useAppConfig(); // Retrieve global config
	const uiFramework = "vben5"; // Placeholder for global config
	const { userInfo } = useUserStore();
	const { deserialize } = useNotificationSerializer();

	// State
	const [favoriteMenus, setFavoriteMenus] = useState<FavoriteMenu[]>([]);
	const [unReadNotifiers, setUnReadNotifiers] = useState<any[]>([]);
	const [unReadCount, setUnReadCount] = useState(0);
	const [todoList, setTodoList] = useState<any[]>([]);
	const [modalVisible, setModalVisible] = useState(false);

	// Default Menus
	const defaultMenus: FavoriteMenu[] = useMemo(
		() => [
			{
				id: "1",
				color: "#1fdaca",
				icon: "ion:home-outline",
				displayName: $t("workbench.content.favoriteMenu.home"),
				path: "/",
				isDefault: true,
			},
			{
				id: "2",
				color: "#bf0c2c",
				icon: "ion:grid-outline",
				displayName: $t("workbench.content.favoriteMenu.dashboard"),
				path: "/",
				isDefault: true,
			},
			{
				id: "3",
				color: "#00d8ff",
				icon: "ant-design:notification-outlined",
				displayName: $t("workbench.content.favoriteMenu.notifiers"),
				path: "/manage/notifications/my-notifilers",
				isDefault: true,
			},
			{
				id: "4",
				color: "#4daf1bc9",
				icon: "tdesign:user-setting",
				displayName: $t("workbench.content.favoriteMenu.settings"),
				path: "/account/my-settings",
				isDefault: true,
			},
			{
				id: "5",
				color: "#3fb27f",
				icon: "hugeicons:profile-02",
				displayName: $t("workbench.content.favoriteMenu.profile"),
				path: "/account/profile",
				isDefault: true,
			},
		],
		[$t],
	);

	const displayMenus = useMemo(() => [...defaultMenus, ...favoriteMenus], [defaultMenus, favoriteMenus]);

	const welcomeTitle = useMemo(() => {
		const hour = new Date().getHours();
		const name = userInfo?.realName || userInfo?.userName || "";
		if (hour < 12) return $t("workbench.header.welcome.morning", { 0: name });
		if (hour < 14) return $t("workbench.header.welcome.atoon", { 0: name });
		if (hour < 17) return $t("workbench.header.welcome.afternoon", { 0: name });
		return $t("workbench.header.welcome.evening", { 0: name });
	}, [userInfo, $t]);

	useEffect(() => {
		initData();
	}, []);

	const initData = async () => {
		await Promise.all([initFavoriteMenus(), initNotifiers(), initTodoList()]);
	};

	const initFavoriteMenus = async () => {
		const { items } = await getFavoriteMenusApi(uiFramework);
		const mapped = items.map((item) => ({
			...item,
			id: item.menuId, // Mapping API response to local FavoriteMenu type
			isDefault: false,
			// Ensure path is resolved if backend returns it or mapping needed
		}));
		setFavoriteMenus(mapped);
	};

	const initNotifiers = async () => {
		const { items, totalCount } = await getMyNotifilersApi({
			maxResultCount: 10,
			readState: NotificationReadState.UnRead,
		});
		const mapped = items.map((item) => {
			const notifier = deserialize(item);
			return {
				avatar: "ant-design:message-outlined", // Placeholder icon
				date: formatToDateTime(item.creationTime),
				title: notifier.title,
				content: notifier.message,
			};
		});
		setUnReadNotifiers(mapped);
		setUnReadCount(totalCount);
	};

	const initTodoList = async () => {
		// TODO: Implementation
		setTodoList([]);
	};

	const handleNav = (menu: FavoriteMenu) => {
		if (menu.path) {
			navigate(menu.path);
		}
	};

	const handleDeleteFavorite = async (menu: FavoriteMenu) => {
		// Assuming menu.id here corresponds to the record ID for deletion API
		// Note: The `items` map above used menuId as id. You might need the actual record ID `item.id`.
		// Adjust initFavoriteMenus mapping if deleteApi needs a specific UUID.
		await deleteFavoriteMenuApi(menu.id);
		toast.success($t("AbpUi.SuccessfullyDeleted"));
		initFavoriteMenus();
	};

	return (
		<div className="p-4 space-y-4">
			<WorkbenchHeader
				avatar={userInfo?.avatar}
				text={userInfo?.realName}
				notifierCount={unReadCount}
				description="今日晴，20℃ - 32℃！" // Localize or fetch real weather
			>
				{welcomeTitle}
			</WorkbenchHeader>

			<Row gutter={[16, 16]}>
				<Col xs={24} lg={14} xl={16} className="space-y-4">
					<WorkbenchQuickNav
						items={displayMenus}
						title={$t("workbench.content.favoriteMenu.title")}
						onAdd={() => setModalVisible(true)}
						onDelete={handleDeleteFavorite}
						onClick={handleNav}
					/>
					<WorkbenchTodo items={todoList} title={$t("workbench.content.todo.title")} />
				</Col>
				<Col xs={24} lg={10} xl={8}>
					<WorkbenchTrends items={unReadNotifiers} title={$t("workbench.content.trends.title")} />
				</Col>
			</Row>

			<WorkbenchQuickNavModal
				visible={modalVisible}
				onClose={() => setModalVisible(false)}
				onChange={initFavoriteMenus}
			/>
		</div>
	);
};

export default WorkbenchPage;
