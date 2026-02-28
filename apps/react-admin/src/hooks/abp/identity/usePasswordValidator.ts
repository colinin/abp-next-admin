import { useAbpSettings } from "@/store/abpSettingStore";
import { ValidationEnum } from "@/constants/abp-core";
import { getUnique, isNullOrWhiteSpace } from "@/utils/string";
import { isDigit, isLetterOrDigit, isLower, isUpper } from "@/utils/abp/regex";
import { useLocalizer } from "../use-localization";
import { useMemo } from "react";

export function usePasswordValidator() {
	const { getNumber, isTrue } = useAbpSettings();
	const { L } = useLocalizer(["AbpIdentity", "AbpValidation", "AbpUi"]);

	const passwordSetting = useMemo(() => {
		return {
			requiredDigit: isTrue("Abp.Identity.Password.RequireDigit"),
			requiredLength: getNumber("Abp.Identity.Password.RequiredLength"),
			requiredLowercase: isTrue("Abp.Identity.Password.RequireLowercase"),
			requiredUniqueChars: getNumber("Abp.Identity.Password.RequiredUniqueChars"),
			requireNonAlphanumeric: isTrue("Abp.Identity.Password.RequireNonAlphanumeric"),
			requireUppercase: isTrue("Abp.Identity.Password.RequireUppercase"),
		};
	}, [getNumber, isTrue]);

	function validate(password: string): Promise<void> {
		return new Promise((resolve, reject) => {
			// 1. Check Empty
			if (isNullOrWhiteSpace(password)) {
				return reject(L(ValidationEnum.FieldRequired, [L("DisplayName:Password")]));
			}

			const setting = passwordSetting;

			// 2. Check Length
			if (setting.requiredLength > 0 && password.length < setting.requiredLength) {
				return reject(L("Volo.Abp.Identity:PasswordTooShort", [String(setting.requiredLength)]));
			}

			// 3. Check Non-Alphanumeric
			if (setting.requireNonAlphanumeric && isLetterOrDigit(password)) {
				return reject(L("Volo.Abp.Identity:PasswordRequiresNonAlphanumeric"));
			}

			// 4. Check Digit
			if (setting.requiredDigit && !isDigit(password)) {
				return reject(L("Volo.Abp.Identity:PasswordRequiresDigit"));
			}

			// 5. Check Lowercase
			if (setting.requiredLowercase && !isLower(password)) {
				return reject(L("Volo.Abp.Identity:PasswordRequiresLower"));
			}

			// 6. Check Uppercase
			if (setting.requireUppercase && !isUpper(password)) {
				return reject(L("Volo.Abp.Identity:PasswordRequiresUpper"));
			}

			// 7. Check Unique Chars
			if (setting.requiredUniqueChars >= 1 && getUnique(password).length < setting.requiredUniqueChars) {
				return reject(L("Volo.Abp.Identity:PasswordRequiredUniqueChars", [String(setting.requiredUniqueChars)]));
			}

			return resolve();
		});
	}

	return {
		validate,
	};
}
