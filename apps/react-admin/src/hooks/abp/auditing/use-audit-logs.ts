import { useMemo } from "react";
// import { useTranslation } from "react-i18next";
import { ChangeType } from "#/management/auditing/entity-changes";
import { useLocalizer } from "../use-localization";

export function useAuditLogs() {
	const { L } = useLocalizer(["AbpAuditLogging", "AbpUi"]);

	const changeTypeColorMap = useMemo(
		() => ({
			[ChangeType.Created]: { color: "#87d068", value: L("Created") },
			[ChangeType.Deleted]: { color: "red", value: L("Deleted") },
			[ChangeType.Updated]: { color: "#108ee9", value: L("Updated") },
		}),
		[L],
	);
	// const { t } = useTranslation();
	// const changeTypeColorMap = useMemo(
	// 	() => ({
	// 		[ChangeType.Created]: { color: "#87d068", value: t("AbpAuditLogging.Created") },
	// 		[ChangeType.Deleted]: { color: "red", value: t("AbpAuditLogging.Deleted") },
	// 		[ChangeType.Updated]: { color: "#108ee9", value: t("AbpAuditLogging.Updated") },
	// 	}),
	// 	[t],
	// );

	const methodColorMap: { [key: string]: string } = {
		DELETE: "red",
		GET: "blue",
		OPTIONS: "cyan",
		PATCH: "pink",
		POST: "green",
		PUT: "orange",
	};

	const getChangeTypeColor = useMemo(
		() => (changeType: ChangeType) => changeTypeColorMap[changeType].color,
		[changeTypeColorMap],
	);

	const getChangeTypeValue = useMemo(
		() => (changeType: ChangeType) => changeTypeColorMap[changeType].value,
		[changeTypeColorMap],
	);

	const getHttpMethodColor = useMemo(() => (method?: string) => (method ? methodColorMap[method] : ""), []);

	const getHttpStatusCodeColor = useMemo(
		() => (statusCode?: number) => {
			if (!statusCode) {
				return "";
			}
			if (statusCode >= 200 && statusCode < 300) {
				return "#87d068";
			}
			if (statusCode >= 300 && statusCode < 400) {
				return "#108ee9";
			}
			if (statusCode >= 400 && statusCode < 500) {
				return "orange";
			}
			if (statusCode >= 500) {
				return "red";
			}
			return "cyan";
		},
		[],
	);

	return {
		getChangeTypeColor,
		getChangeTypeValue,
		getHttpMethodColor,
		getHttpStatusCodeColor,
	};
}
