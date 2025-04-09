/// <reference types="vite/client" />

interface ImportMetaEnv {
	readonly VITE_GLOB_APP_TITLE: string;
	readonly VITE_APP_BASE_API: string;
	readonly VITE_APP_HOMEPAGE: string;
	readonly VITE_APP_BASE_PATH: string;
	readonly VITE_APP_ENV: "development" | "production";
	readonly VITE_GLOB_CLIENT_ID: string;
	readonly VITE_GLOB_CLIENT_SECRET: string;
	readonly VITE_GLOB_SCOPE: string;
	readonly VITE_PROXY_API: string;
}

interface ImportMeta {
	readonly env: ImportMetaEnv;
}
