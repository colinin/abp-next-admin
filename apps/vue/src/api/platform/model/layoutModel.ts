import { Route } from './basicModel';

export class Layout extends Route {
  framework!: string;
  dataId!: string;
}

export interface LayoutListResult extends ListResultDto<Layout> {}

export interface LayoutPagedResult extends PagedResultDto<Layout> {}

export class CreateOrUpdateLayout {
  name!: string;
  path!: string;
  displayName!: string;
  description?: string;
  redirect?: string;
}

export class CreateLayout extends CreateOrUpdateLayout {
  dataId!: string;
  framework!: string;
}

export class UpdateLayout extends CreateOrUpdateLayout {}

export interface GetLayoutPagedRequest extends PagedAndSortedResultRequestDto {
  filter?: string;
  reverse?: boolean;
  framework?: string;
}
