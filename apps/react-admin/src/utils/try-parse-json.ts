import { tryCatch } from "ramda";

export const tryParseJson = tryCatch(JSON.parse, (_) => {
	return {};
});
