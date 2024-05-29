import { Route } from "../../layouts/model";

export interface CreateOrUpdateMenu {
  name: string;
  path: string;
  component: string;
  displayName: string;
  description?: string;
  redirect?: string;
  isPublic: boolean;
  meta: { [key: string]: any };
}

export interface CreateMenu extends CreateOrUpdateMenu {
  layoutId: string;
  parentId?: string;
}

export type UpdateMenu = CreateOrUpdateMenu;

export interface GetAllMenuRequest extends SortedResultRequest {
  filter?: string;
  sorting?: string;
  parentId?: string;
  layoutId?: string;
  framework?: string;
}

export interface GetMenuPagedRequest extends PagedAndSortedResultRequestDto {
  filter?: string;
  reverse?: boolean;
  layoutId?: string;
  parentId?: string;
  framework?: string;
}

export interface Menu extends Route {
  code: string;
  layoutId: string;
  component: string;
  framework: string;
  parentId?: string;
  isPubli: boolean;
  startup: boolean;
  children?: Menu[];
}

export interface RoleMenu {
  roleName: string;
  menuIds: string[];
}

export interface UserMenu {
  userId: string;
  menuIds: string[];
}
