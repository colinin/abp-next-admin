import { Route } from './basicModel';

export class CreateOrUpdateMenu {
  name!: string;
  path!: string;
  component!: string;
  displayName!: string;
  description?: string;
  redirect?: string;
  isPublic!: boolean;
  meta: { [key: string]: any } = {};
}

export class CreateMenu extends CreateOrUpdateMenu {
  layoutId!: string;
  parentId?: string;
}

export class UpdateMenu extends CreateOrUpdateMenu {}

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

export class Menu extends Route {
  code!: string;
  layoutId!: string;
  component!: string;
  framework = '';
  parentId?: string;
  isPublic = false;
  startup = false;
  children = new Array<Menu>();
}

export interface MenuListResult extends ListResultDto<Menu> {}

export interface MenuPagedResult extends PagedResultDto<Menu> {}

export class RoleMenu {
  roleName!: string;
  menuIds = new Array<string>();
}

export class UserMenu {
  userId!: string;
  menuIds = new Array<string>();
}
