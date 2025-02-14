import { defineConfig } from '@vben/vite-config';

export default defineConfig(async () => {
  return {
    application: {},
    vite: {
      server: {
        proxy: {
          '/.well-known': {
            changeOrigin: true,
            target: 'http://127.0.0.1:30001/',
          },
          '/api': {
            changeOrigin: true,
            target: 'http://127.0.0.1:30001/',
          },
          '/connect': {
            changeOrigin: true,
            target: 'http://127.0.0.1:30001/',
          },
          '/signalr-hubs': {
            changeOrigin: true,
            target: 'http://127.0.0.1:30001/',
            ws: true,
          },
        },
      },
    },
  };
});
