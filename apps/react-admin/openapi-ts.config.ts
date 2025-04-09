import { defineConfig } from "@hey-api/openapi-ts";
import { defaultPlugins } from "@hey-api/openapi-ts";
export default defineConfig({
	client: "@hey-api/client-axios",
	input: "http://192.168.31.246:30001/swagger/v1/swagger.json",
	output: {
		format: "biome",
		lint: "biome",
		path: "src/api/gen",
	},
	plugins: [
		...defaultPlugins,
		{
			enums: "typescript",
			name: "@hey-api/typescript",
		},
	],
});
