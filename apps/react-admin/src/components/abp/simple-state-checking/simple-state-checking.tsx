import type React from "react";
import { useState, useMemo, useEffect } from "react";
import { Button, Card, Table, Tag, Space, Row, Col, Popconfirm } from "antd";
import { EditOutlined, DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import { useSimpleStateCheck } from "@/hooks/abp/fake-hooks/use-simple-state-check"; //  TODO re hooks in this chain
import type { SimplaCheckStateBase } from "./interface";
import SimpleStateCheckingModal from "./simple-state-checking-modal";
import type { ColumnsType } from "antd/es/table";

interface SimpleStateCheckingProps {
	state: SimplaCheckStateBase;
	value?: string; // Serialized string
	onChange?: (value: string | undefined) => void;
	disabled?: boolean;
	allowEdit?: boolean;
	allowDelete?: boolean;
}

const SimpleStateChecking: React.FC<SimpleStateCheckingProps> = ({
	state,
	value,
	onChange,
	disabled = false,
	allowEdit = false,
	allowDelete = false,
}) => {
	const { t: $t } = useTranslation();
	const { deserializeArray, serializeArray, deserialize } = useSimpleStateCheck();

	// Local state to manage the list of checkers
	const [checkers, setCheckers] = useState<any[]>([]);
	const [modalVisible, setModalVisible] = useState(false);
	const [editingRecord, setEditingRecord] = useState<any>(null);

	// Sync prop value with local state
	useEffect(() => {
		if (!value || value.length === 0) {
			setCheckers([]);
		} else {
			const deserialized = deserializeArray(value, state);
			setCheckers(deserialized);
		}
	}, [value, state]);

	// Map for friendly names
	const simpleCheckerMap: Record<string, string> = {
		A: $t("component.simple_state_checking.requireAuthenticated.title"),
		F: $t("component.simple_state_checking.requireFeatures.title"),
		G: $t("component.simple_state_checking.requireGlobalFeatures.title"),
		P: $t("component.simple_state_checking.requirePermissions.title"),
	};

	const handleAddNew = () => {
		setEditingRecord(null);
		setModalVisible(true);
	};

	const handleEdit = (record: any) => {
		setEditingRecord(record);
		setModalVisible(true);
	};

	const handleUpdateCheckers = (newCheckers: any[]) => {
		setCheckers(newCheckers);
		const serialized = serializeArray(newCheckers);
		onChange?.(serialized); //TODO
	};

	const handleModalConfirm = (data: any) => {
		// 'data' comes in format { T: 'type', A: bool, N: [] } from the modal

		// Deserialize the simple object from modal back into the complex class instance used by the list
		const deserializedItem = deserialize(data, state);
		if (!deserializedItem) return;

		const updatedList = [...checkers];
		const existingIndex = updatedList.findIndex((x) => x.name === data.T);

		if (existingIndex > -1) {
			updatedList[existingIndex] = deserializedItem;
		} else {
			updatedList.push(deserializedItem);
		}

		handleUpdateCheckers(updatedList);
	};

	const handleDelete = (record: any) => {
		const updatedList = checkers.filter((x) => x.name !== record.name);
		handleUpdateCheckers(updatedList);
	};

	const handleClean = () => {
		handleUpdateCheckers([]);
	};

	const options = useMemo(() => {
		return Object.keys(simpleCheckerMap).map((key) => ({
			label: simpleCheckerMap[key],
			value: key,
			// Disable if this type already exists in the list (assuming 1 per type based on Vue logic findIndex)
			disabled: checkers.some((x) => x.name === key),
		}));
	}, [checkers, $t]);

	const columns: ColumnsType<any> = [
		{
			title: $t("component.simple_state_checking.table.name"),
			dataIndex: "name",
			key: "name",
			width: 150,
			render: (name) => simpleCheckerMap[name] || name,
		},
		{
			title: $t("component.simple_state_checking.table.properties"),
			key: "properties",
			render: (_, record) => {
				if (record.name === "F") {
					return (
						<Space wrap>
							{record.featureNames?.map((f: string) => (
								<Tag key={f}>{f}</Tag>
							))}
						</Space>
					);
				}
				if (record.name === "G") {
					return (
						<Space wrap>
							{record.globalFeatureNames?.map((f: string) => (
								<Tag key={f}>{f}</Tag>
							))}
						</Space>
					);
				}
				if (record.name === "P") {
					const permissions = record.model?.permissions || record.permissions || [];
					return (
						<Space wrap>
							{permissions.map((p: string) => (
								<Tag key={p}>{p}</Tag>
							))}
						</Space>
					);
				}
				if (record.name === "A") {
					return $t("component.simple_state_checking.requireAuthenticated.title");
				}
				return null;
			},
		},
	];

	if (!disabled) {
		columns.push({
			title: $t("component.simple_state_checking.table.actions"),
			key: "action",
			width: 180,
			render: (_, record) => (
				<Space>
					{allowEdit && (
						<Button type="link" icon={<EditOutlined />} onClick={() => handleEdit(record)}>
							{$t("component.simple_state_checking.actions.update")}
						</Button>
					)}
					{allowDelete && (
						<Popconfirm title="Are you sure?" onConfirm={() => handleDelete(record)}>
							<Button type="link" danger icon={<DeleteOutlined />}>
								{$t("component.simple_state_checking.actions.delete")}
							</Button>
						</Popconfirm>
					)}
				</Space>
			),
		});
	}

	return (
		<div className="w-full">
			<Card
				title={
					<Row justify="space-between" align="middle">
						<Col>{$t("component.simple_state_checking.title")}</Col>
						<Col>
							{!disabled && (
								<Space>
									<Button type="primary" onClick={handleAddNew} icon={<PlusOutlined />}>
										{$t("component.simple_state_checking.actions.create")}
									</Button>
									<Button danger onClick={handleClean}>
										{$t("component.simple_state_checking.actions.clean")}
									</Button>
								</Space>
							)}
						</Col>
					</Row>
				}
			>
				<Table rowKey="name" columns={columns} dataSource={checkers} pagination={false} size="small" />
			</Card>

			<SimpleStateCheckingModal
				visible={modalVisible}
				onClose={() => setModalVisible(false)}
				onConfirm={handleModalConfirm}
				record={editingRecord}
				options={options}
			/>
		</div>
	);
};

export default SimpleStateChecking;
