import type { MenuDto } from '../../types/menus';

type MenuSubject = 'role' | 'user';

type EditMenu = {
  id?: string;
  layoutId?: string;
  parentId?: string;
};

type MenuDrawerState = {
  editMenu?: EditMenu;
  rootMenus: MenuDto[];
};

export type { MenuDrawerState, MenuSubject };
