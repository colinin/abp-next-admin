import { mergeWith } from 'lodash-es';
import { computed, CSSProperties } from 'vue';
import { useAppStoreWithOut } from '/@/store/modules/app';

interface TabsStyle {
  style?: CSSProperties,
  tabBarStyle?: CSSProperties,
}

interface Tabs {
  [key: string]: TabsStyle;
}

export function useTabsStyle(
  position: string = 'top',
  style?: CSSProperties,
  tabBarStyle?: CSSProperties) {
  const defaultStyle: Tabs = {
    top: {
      style: {
        overflow: 'unset !important',
      },
      tabBarStyle: {
        position: 'sticky',
        top: '0',
        zIndex: 999,
      },
    }
  };

  const tabsStyle = computed((): TabsStyle => {
    if (!position) return {};
    const appStore = useAppStoreWithOut();
    const dark = appStore.getDarkMode;
    let tabs = defaultStyle[position];
    if (!tabs) return {};
    tabs = mergeWith(
      tabs, 
      {
        tabBarStyle: {
          background: dark === 'dark' ? '#1f1f1f' : '#fff'
        }
      }
    );
    if (style) {
      tabs = mergeWith(tabs, { style: style });
    }
    if (tabBarStyle) {
      tabs = mergeWith(tabs, { tabBarStyle: tabBarStyle });
    }
    return tabs;
  });

  return tabsStyle;
}
