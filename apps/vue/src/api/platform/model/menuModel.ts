import {
  ISortedResultRequest,
  ListResultDto,
  PagedAndSortedResultRequestDto,
  PagedResultDto,
} from '../../model/baseModel';
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

export class GetAllMenuRequest implements ISortedResultRequest {
  filter = '';
  sorting = '';
  parentId?: string;
  layoutId?: string;
  framework = '';
}

export class GetMenuPagedRequest extends PagedAndSortedResultRequestDto {
  filter = '';
  reverse = false;
  layoutId?: string;
  parentId?: string;
  framework = '';
}

export class Menu extends Route {
  code!: string;
  layoutId!: string;
  component!: string;
  framework = '';
  parentId?: string;
  isPublic!: boolean;
  children = new Array<Menu>();
}

export class MenuListResult extends ListResultDto<Menu> {}

export class MenuPagedResult extends PagedResultDto<Menu> {}

export class RoleMenu {
  roleName!: string;
  menuIds = new Array<string>();
}

export class UserMenu {
  userId!: string;
  menuIds = new Array<string>();
}
