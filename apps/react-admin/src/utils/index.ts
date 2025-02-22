import type { LocalEnum } from "#/enum";
import { type ClassValue, clsx } from "clsx";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]) {
	return twMerge(clsx(inputs));
}

export function bindMethods<T extends object>(instance: T): void {
	const prototype = Object.getPrototypeOf(instance);
	const propertyNames = Object.getOwnPropertyNames(prototype);

	propertyNames.forEach((propertyName) => {
		const descriptor = Object.getOwnPropertyDescriptor(prototype, propertyName);
		const propertyValue = instance[propertyName as keyof T];

		if (
			typeof propertyValue === "function" &&
			propertyName !== "constructor" &&
			descriptor &&
			!descriptor.get &&
			!descriptor.set
		) {
			instance[propertyName as keyof T] = propertyValue.bind(instance);
		}
	});
}

// 映射函数：将 LocalEnum 转为 API 格式
export function mapLocaleToAbpLanguageFormat(locale: keyof typeof LocalEnum): string {
	return locale.replace("_", "-");
}
