import { useState } from "react";
import OrganizationUnitTree from "./organization-unit-tree";
import OrganizationUnitTable from "./organization-unit-table";

const OrganizationUnitPage = () => {
	const [selectedKey, setSelectedKey] = useState<string>();

	return (
		<div className="flex flex-row gap-2">
			<div className="basis-1/3">
				<OrganizationUnitTree onSelected={setSelectedKey} />
			</div>
			<div className="basis-2/3">
				<OrganizationUnitTable selectedKey={selectedKey} />
			</div>
		</div>
	);
};

export default OrganizationUnitPage;
