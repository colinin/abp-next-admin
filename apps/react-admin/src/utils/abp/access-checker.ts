import userStore from "@/store/userStore";
import type React from "react";

export const withAccessChecker = (element: React.ReactElement, requiredCodes: string[]) => {
	const accessCodes = userStore.getState().accessCodes;

	const canAccess = requiredCodes.every((code) => accessCodes.includes(code));

	return canAccess ? element : null;
};

/**
 *
 * @param codes 具有至少一个传入的访问权限代码
 * @returns
 */
export const hasAccessByCodes = (codes: string[]) => {
	const userCodesSet = new Set(userStore.getState().accessCodes);
	const intersection = codes.filter((item) => userCodesSet.has(item));
	return intersection.length > 0;
};
