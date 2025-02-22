import path from "node:path";

import { vanillaExtractPlugin } from "@vanilla-extract/vite-plugin";
import react from "@vitejs/plugin-react";
import { visualizer } from "rollup-plugin-visualizer";
import { defineConfig, loadEnv } from "vite";
import { createSvgIconsPlugin } from "vite-plugin-svg-icons";
import tsconfigPaths from "vite-tsconfig-paths";

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
	const env = loadEnv(mode, process.cwd(), "");
	const base = env.VITE_APP_BASE_PATH || "/";
	const isProduction = mode === "production";
	const proxyTarget = env.VITE_PROXY_API || "http://localhost:30001";

	return {
		base,
		plugins: [
			react(),
			vanillaExtractPlugin({
				identifiers: ({ debugId }) => `${debugId}`,
			}),
			tsconfigPaths(),
			createSvgIconsPlugin({
				iconDirs: [path.resolve(process.cwd(), "src/assets/icons")],
				symbolId: "icon-[dir]-[name]",
			}),
			isProduction &&
				visualizer({
					open: true,
					gzipSize: true,
					brotliSize: true,
				}),
		],
		server: {
			open: false,
			host: true,
			port: 3100,
			proxy: {
				"/.well-known": {
					changeOrigin: true,
					target: proxyTarget,
					ws: true,
				},
				"/api": {
					changeOrigin: true,
					target: proxyTarget,
					ws: true,
				},
				"/connect": {
					changeOrigin: true,
					target: proxyTarget,
					ws: true,
				},
				"/signalr-hubs": {
					changeOrigin: true,
					target: proxyTarget,
					ws: true,
				},
			},
		},
		optimizeDeps: {
			include: ["react", "react-dom", "react-router", "antd"],
		},
		esbuild: {
			drop: isProduction ? ["console", "debugger"] : [],
		},
		build: {
			target: "esnext",
			minify: "esbuild",
			sourcemap: false,
			cssCodeSplit: true,
			chunkSizeWarningLimit: 1000, // 提高警告阈值到 1000 KB

			rollupOptions: {
				output: {
					manualChunks: {
						"vendor-react": ["react", "react-dom", "react-router"],
						"vendor-antd": ["antd", "@ant-design/icons", "@ant-design/cssinjs"],
						"vendor-charts": ["apexcharts", "react-apexcharts"],
						"vendor-utils": ["axios", "dayjs", "i18next", "zustand"],
						"vendor-ui": ["framer-motion", "styled-components", "@iconify/react"],
					},
				},
			},
		},
	};
});
