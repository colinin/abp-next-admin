import type React from "react";
import { JsonEditor, githubDarkTheme, githubLightTheme } from "json-edit-react";
import { useTheme } from "@/theme/hooks";
import { ThemeMode } from "#/enum";
import { tryParseJson } from "@/utils/try-parse-json";

interface JsonEditProps {
	data: object | any[] | string;
}
/**
 * default readonly json viewer
 */
const JsonEdit: React.FC<JsonEditProps> = ({ data }) => {
	const { mode } = useTheme();
	const parseData = typeof data === "string" ? tryParseJson(data) : data;
	return (
		<JsonEditor
			rootName={""}
			data={parseData}
			theme={mode === ThemeMode.Dark ? githubDarkTheme : githubLightTheme}
			restrictEdit={true}
			restrictDelete={true}
			restrictAdd={true}
		/>
	);
};

export default JsonEdit;
