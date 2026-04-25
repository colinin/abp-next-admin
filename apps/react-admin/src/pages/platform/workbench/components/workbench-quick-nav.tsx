import React, { useMemo } from "react";
import { Card, Dropdown, Modal } from "antd";
import { DeleteOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { Iconify } from "@/components/icon";
import type { FavoriteMenu } from "../types";

interface Props {
	items?: FavoriteMenu[];
	title: string;
	onClick?: (menu: FavoriteMenu) => void;
	onDelete?: (menu: FavoriteMenu) => void;
	onAdd?: () => void;
}

const WorkbenchQuickNav: React.FC<Props> = ({ items = [], title, onClick, onDelete, onAdd }) => {
	const { t: $t } = useTranslation();

	const [modal, contextHolder] = Modal.useModal();
	const getFavoriteMenus = useMemo(() => {
		const addMenu: FavoriteMenu = {
			id: "addMenu",
			displayName: $t("workbench.content.favoriteMenu.create"),
			icon: "ion:add-outline",
			color: "#00bfff",
			isDefault: true,
		};
		return [...items, addMenu];
	}, [items, $t]);

	const handleClick = (menu: FavoriteMenu) => {
		if (menu.id === "addMenu") {
			onAdd?.();
			return;
		}
		onClick?.(menu);
	};

	const handleDelete = (menu: FavoriteMenu) => {
		modal.confirm({
			centered: true,
			title: $t("AbpUi.AreYouSure"),
			content: $t("AbpUi.ItemWillBeDeletedMessage"),
			onOk: () => onDelete?.(menu),
		});
	};

	return (
		<>
			{contextHolder}
			<Card title={title} className="min-h-[300px] shadow-sm">
				<div className="flex flex-wrap">
					{getFavoriteMenus.map((item, index) => {
						const isAddBtn = item.id === "addMenu";
						// Calculate borders for grid layout (3 columns)
						const borderClasses = `
            flex flex-col items-center justify-center w-1/3 py-8 cursor-pointer
            border-t border-r border-gray-100 hover:shadow-lg transition-shadow duration-300 group
            ${index % 3 === 2 ? "border-r-0" : ""}
            ${index < 3 ? "border-t-0" : ""} 
          `;

						const content = (
							<div className={borderClasses} onClick={() => handleClick(item)}>
								<Iconify
									icon={item.icon || "mdi:circle"}
									style={{ color: item.color, fontSize: "28px" }}
									className="transition-transform duration-300 group-hover:scale-125"
								/>
								<span className="mt-2 text-sm text-gray-600 truncate px-2 w-full text-center">{item.displayName}</span>
							</div>
						);

						if (!item.isDefault && !isAddBtn) {
							return (
								<Dropdown
									key={item.id}
									trigger={["contextMenu"]}
									menu={{
										items: [
											{
												key: "delete",
												label: $t("workbench.content.favoriteMenu.delete"),
												icon: <DeleteOutlined />,
												onClick: () => handleDelete(item),
											},
										],
									}}
								>
									{content}
								</Dropdown>
							);
						}

						return <React.Fragment key={item.id}>{content}</React.Fragment>;
					})}
				</div>
			</Card>
		</>
	);
};

export default WorkbenchQuickNav;
