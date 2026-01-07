import { defineConfig } from '@vben/vite-config';

export default defineConfig(async () => {
  return {
    application: {},
    vite: {
      server: {
        proxy: {
          '/.well-known': {
            changeOrigin: true,
            target: 'http://localhost:30000/',
          },
          '/api': {
            changeOrigin: true,
            target: 'http://localhost:30000/',
          },
          '/connect': {
            changeOrigin: true,
            target: 'http://localhost:30000/',
          },
          '/signalr-hubs': {
            changeOrigin: true,
            target: 'http://localhost:30000/',
            ws: true,
          },
        },
      },
    },
  };
});
