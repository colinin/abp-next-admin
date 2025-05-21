import type { RouteDto } from './routes';

interface MenuDto extends RouteDto {
  code: string;
  component: string;
  framework: string;
  isPublic: boolean;
  layoutId: string;
  parentId?: string;
  startup: boolean;
}

interface MenuCreateOrUpdateDto {
  component: string;
  description?: string;
  displayName: string;
  isPublic: boolean;
  meta: Record<string, string>;
  name: string;
  parentId?: string;
  path: string;
  redirect?: string;
}

interface MenuCreateDto extends MenuCreateOrUpdateDto {
  layoutId: string;
}

interface MenuGetInput {
  framework?: string;
}

interface MenuGetByUserInput {
  framework: string;
  userId: string;
}

interface MenuGetByRoleInput {
  framework: string;
  role: string;
}

interface SetUserMenuInput {
  framework?: string;
  menuIds: string[];
  startupMenuId?: string;
  userId: string;
}

interface SetUserMenuStartupInput {
  framework?: string;
  userId: string;
}

interface SetRoleMenuInput {
  framework?: string;
  menuIds: string[];
  roleName: string;
  startupMenuId?: string;
}

interface SetRoleMenuStartupInput {
  framework?: string;
  roleName: string;
}

type MenuUpdateDto = MenuCreateOrUpdateDto;

interface MenuGetAllInput {
  filter?: string;
  framework?: string;
  layoutId?: string;
  parentId?: string;
  sorting?: string;
}

export type {
  MenuCreateDto,
  MenuDto,
  MenuGetAllInput,
  MenuGetByRoleInput,
  MenuGetByUserInput,
  MenuGetInput,
  MenuUpdateDto,
  SetRoleMenuInput,
  SetRoleMenuStartupInput,
  SetUserMenuInput,
  SetUserMenuStartupInput,
};
