import type React from "react";
import { useEffect, useState } from "react";
import { Modal } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import { getConnectionStringsApi, setConnectionStringApi, deleteConnectionStringApi } from "@/api/saas/tenants";
import type { TenantConnectionStringDto, TenantDto } from "#/saas/tenants";
import TenantConnectionStringsList from "./tenant-connection-strings-list";

interface Props {
	visible: boolean;
	onClose: () => void;
	tenant?: TenantDto;
	dataBaseOptions: { label: string; value: string }[];
}

const TenantConnectionStringsModal: React.FC<Props> = ({ visible, onClose, tenant, dataBaseOptions }) => {
	const { t: $t } = useTranslation();
	const [connectionStrings, setConnectionStrings] = useState<TenantConnectionStringDto[]>([]);
	const [loading, setLoading] = useState(false);

	useEffect(() => {
		if (visible && tenant) {
			fetchData(tenant.id);
		} else {
			setConnectionStrings([]);
		}
	}, [visible, tenant]);

	const fetchData = async (id: string) => {
		try {
			setLoading(true);
			const { items } = await getConnectionStringsApi(id);
			setConnectionStrings(items);
		} finally {
			setLoading(false);
		}
	};

	const handleUpdate = async (data: TenantConnectionStringDto) => {
		if (!tenant) return;
		await setConnectionStringApi(tenant.id, data);
		toast.success($t("AbpUi.SavedSuccessfully"));
		await fetchData(tenant.id);
	};

	const handleDelete = async (data: TenantConnectionStringDto) => {
		if (!tenant) return;
		await deleteConnectionStringApi(tenant.id, data.name);
		toast.success($t("AbpUi.DeletedSuccessfully"));
		await fetchData(tenant.id);
	};

	return (
		<Modal
			title={$t("AbpSaas.ConnectionStrings")}
			open={visible}
			onCancel={onClose}
			footer={null}
			width={800}
			destroyOnClose
		>
			<TenantConnectionStringsList
				data={connectionStrings}
				dataBaseOptions={dataBaseOptions}
				onAdd={handleUpdate}
				onDelete={handleDelete}
				loading={loading}
			/>
		</Modal>
	);
};

export default TenantConnectionStringsModal;
