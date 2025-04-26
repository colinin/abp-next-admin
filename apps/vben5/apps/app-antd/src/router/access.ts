import type {
  ComponentRecordType,
  GenerateMenuAndRoutesOptions,
} from '@vben/types';

import { generateAccessible } from '@vben/access';
import { useAppConfig } from '@vben/hooks';
import { preferences } from '@vben/preferences';

import { useMenuTransform, useMyMenusApi } from '@abp/platform';
import { message } from 'ant-design-vue';

import { BasicLayout, IFrameView } from '#/layouts';
import { $t } from '#/locales';

const forbiddenComponent = () => import('#/views/_core/fallback/forbidden.vue');

async function generateAccess(options: GenerateMenuAndRoutesOptions) {
  const { getAllApi } = useMyMenusApi();
  const { transformRoutes } = useMenuTransform();
  const pageMap: ComponentRecordType = import.meta.glob('../views/**/*.vue');
  const { uiFramework } = useAppConfig(import.meta.env, import.meta.env.PROD);

  const layoutMap: ComponentRecordType = {
    BasicLayout,
    IFrameView,
  };

  return await generateAccessible(preferences.app.accessMode, {
    ...options,
    fetchMenuListAsync: async () => {
      message.loading({
        content: `${$t('common.loadingMenu')}...`,
        duration: 1.5,
      });
      const { items } = await getAllApi({
        framework: uiFramework,
      });
      return transformRoutes(items);
    },
    // 可以指定没有权限跳转403页面
    forbiddenComponent,
    // 如果 route.meta.menuVisibleWithForbidden = true
    layoutMap,
    pageMap,
  });
}

export { generateAccess };
