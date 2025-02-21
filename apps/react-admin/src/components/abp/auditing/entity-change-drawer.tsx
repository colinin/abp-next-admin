import { getListWithUsernameApi } from "@/api/management/auditing/entity-changes";
import { EntityChangeTable } from "./entity-change-table";
import type { EntityChangeGetWithUsernameInput } from "#/management/auditing/entity-changes";
import { Drawer } from "antd";
import { useQuery } from "@tanstack/react-query";
import { useTranslation } from "react-i18next";

interface EntityChangeDrawerProps {
	open: boolean;
	onClose: () => void;
	input?: EntityChangeGetWithUsernameInput;
	subject?: string;
}

export const EntityChangeDrawer: React.FC<EntityChangeDrawerProps> = ({ open, onClose, input, subject }) => {
	const { t } = useTranslation();

	const { data: entityChanges = [] } = useQuery({
		queryKey: ["entityChanges", input],
		queryFn: async () => {
			if (!input) return [];
			const { items } = await getListWithUsernameApi(input);
			return items.map((item) => ({
				...item.entityChange,
				userName: item.userName,
			}));
		},
		enabled: open && !!input,
	});

	return (
		<Drawer title={`${t("AbpAuditLogging.EntitiesChanged")}: ${subject}`} open={open} onClose={onClose} width={800}>
			<EntityChangeTable data={entityChanges} showUserName />
		</Drawer>
	);
};
