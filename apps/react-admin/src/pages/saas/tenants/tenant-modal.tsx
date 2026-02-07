import type React from "react";
import { useEffect, useState } from "react";
import { Modal, Form, Input, Select, Checkbox, DatePicker, Tabs, Row, Col } from "antd";
import { useTranslation } from "react-i18next";
import { toast } from "sonner";
import dayjs from "dayjs";
import { createApi, updateApi, getApi, checkConnectionString } from "@/api/saas/tenants";
import { getPagedListApi as getEditionsApi } from "@/api/saas/editions";
import type { TenantDto, TenantConnectionStringDto, TenantCreateDto } from "#/saas/tenants";
import type { EditionDto } from "#/saas/editions";
import TenantConnectionStringsList from "./tenant-connection-strings-list";

interface Props {
	visible: boolean;
	onClose: () => void;
	onChange: (data: TenantDto) => void;
	tenantId?: string;
	dataBaseOptions: { label: string; value: string }[];
}

const TenantModal: React.FC<Props> = ({ visible, onClose, onChange, tenantId, dataBaseOptions }) => {
	const { t: $t } = useTranslation();
	const [form] = Form.useForm();

	const [activeTab, setActiveTab] = useState("basic");
	const [loading, setLoading] = useState(false);
	const [submitting, setSubmitting] = useState(false);

	const [editions, setEditions] = useState<EditionDto[]>([]);
	// Local state for connection strings when creating a new tenant
	const [localConnectionStrings, setLocalConnectionStrings] = useState<TenantConnectionStringDto[]>([]);

	useEffect(() => {
		if (visible) {
			setActiveTab("basic");
			form.resetFields();
			setLocalConnectionStrings([]);
			fetchEditions();

			if (tenantId) {
				fetchTenant(tenantId);
			} else {
				form.setFieldsValue({
					isActive: true,
					useSharedDatabase: true,
				});
			}
		}
	}, [visible, tenantId]);

	const fetchTenant = async (id: string) => {
		try {
			setLoading(true);
			const dto = await getApi(id);
			form.setFieldsValue({
				...dto,
				enableTime: dto.enableTime ? dayjs(dto.enableTime) : null,
				disableTime: dto.disableTime ? dayjs(dto.disableTime) : null,
			});
		} finally {
			setLoading(false);
		}
	};

	const fetchEditions = async () => {
		const { items } = await getEditionsApi({ maxResultCount: 100 });
		setEditions(items);
	};

	const handleNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const name = e.target.value;
		// Auto-fill admin email if creating and email not manually modified (simplified logic)
		if (!tenantId) {
			const currentEmail = form.getFieldValue("adminEmailAddress");
			if (!currentEmail || currentEmail.includes("@")) {
				form.setFieldValue("adminEmailAddress", `admin@${name || "domain"}.com`);
			}
		}
	};

	const handleLocalConnectionAdd = async (data: TenantConnectionStringDto) => {
		setLocalConnectionStrings((prev) => {
			const exists = prev.find((x) => x.name === data.name);
			if (exists) {
				return prev.map((x) => (x.name === data.name ? { ...x, value: data.value } : x));
			}
			return [...prev, data];
		});
	};

	const handleLocalConnectionDelete = async (data: TenantConnectionStringDto) => {
		setLocalConnectionStrings((prev) => prev.filter((x) => x.name !== data.name));
	};

	const handleSubmit = async () => {
		try {
			const values = await form.validateFields();
			setSubmitting(true);

			// Check default connection string if not shared
			if (!tenantId && !values.useSharedDatabase) {
				await checkConnectionString({
					connectionString: values.defaultConnectionString,
					provider: values.provider,
				});
			}

			const submitData = {
				...values,
				enableTime: values.enableTime ? values.enableTime.format("YYYY-MM-DD") : null,
				disableTime: values.disableTime ? values.disableTime.format("YYYY-MM-DD") : null,
			};

			// Add local connection strings if creating
			if (!tenantId) {
				(submitData as TenantCreateDto).connectionStrings = localConnectionStrings;
			}

			const api = tenantId ? updateApi(tenantId, submitData) : createApi(submitData);

			const res = await api;
			toast.success($t("AbpUi.SavedSuccessfully"));
			onChange(res);
			onClose();
		} catch (e) {
			console.error(e);
		} finally {
			setSubmitting(false);
		}
	};

	// Watch for useSharedDatabase to conditionally render tab content
	const useSharedDatabase = Form.useWatch("useSharedDatabase", form);

	return (
		<Modal
			title={tenantId ? `${$t("AbpSaas.Tenants")} - ${form.getFieldValue("name") || ""}` : $t("AbpSaas.NewTenant")}
			open={visible}
			onCancel={onClose}
			onOk={handleSubmit}
			confirmLoading={submitting}
			loading={loading}
			width={600}
			destroyOnClose
		>
			<Form form={form} layout="vertical">
				<Form.Item name="isActive" valuePropName="checked">
					<Checkbox>{$t("AbpSaas.DisplayName:IsActive")}</Checkbox>
				</Form.Item>

				<Form.Item name="name" label={$t("AbpSaas.DisplayName:TenantName")} rules={[{ required: true }]}>
					<Input autoComplete="off" onChange={handleNameChange} />
				</Form.Item>

				{!tenantId && (
					<>
						<Form.Item
							name="adminEmailAddress"
							label={$t("AbpSaas.DisplayName:AdminEmailAddress")}
							rules={[{ required: true, type: "email" }]}
						>
							<Input autoComplete="off" />
						</Form.Item>
						<Form.Item
							name="adminPassword"
							label={$t("AbpSaas.DisplayName:AdminPassword")}
							rules={[{ required: true }]}
						>
							<Input.Password autoComplete="off" />
						</Form.Item>
					</>
				)}

				<Form.Item name="editionId" label={$t("AbpSaas.DisplayName:EditionName")}>
					<Select
						allowClear
						showSearch
						optionFilterProp="label"
						options={editions.map((e) => ({ label: e.displayName, value: e.id }))}
					/>
				</Form.Item>

				<Row gutter={16}>
					<Col span={12}>
						<Form.Item name="enableTime" label={$t("AbpSaas.DisplayName:EnableTime")}>
							<DatePicker className="w-full" />
						</Form.Item>
					</Col>
					<Col span={12}>
						<Form.Item name="disableTime" label={$t("AbpSaas.DisplayName:DisableTime")}>
							<DatePicker className="w-full" />
						</Form.Item>
					</Col>
				</Row>

				{!tenantId && (
					<Form.Item name="useSharedDatabase" valuePropName="checked">
						<Checkbox>{$t("AbpSaas.DisplayName:UseSharedDatabase")}</Checkbox>
					</Form.Item>
				)}

				{/* Dedicated Database Configuration Section */}
				{!tenantId && !useSharedDatabase && (
					<div className="border p-4 rounded mb-4 bg-gray-50">
						<Form.Item name="provider" label={$t("AbpSaas.DisplayName:DataBaseProvider")}>
							<Select options={dataBaseOptions} />
						</Form.Item>
						<Form.Item name="defaultConnectionString" label={$t("AbpSaas.DisplayName:DefaultConnectionString")}>
							<Input.TextArea autoSize={{ minRows: 2 }} />
						</Form.Item>

						<div className="mt-4">
							<h4>{$t("AbpSaas.ConnectionStrings")}</h4>
							<TenantConnectionStringsList
								data={localConnectionStrings}
								dataBaseOptions={dataBaseOptions}
								onAdd={handleLocalConnectionAdd}
								onDelete={handleLocalConnectionDelete}
							/>
						</div>
					</div>
				)}
			</Form>
		</Modal>
	);
};

export default TenantModal;
