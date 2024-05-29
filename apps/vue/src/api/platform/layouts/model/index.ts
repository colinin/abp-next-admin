export interface Route {
    id: string;
    name: string;
    path: string;
    displayName: string;
    description?: string;
    redirect?: string;
    meta: { [key: string]: any };
  }
  

export interface Layout extends Route {
  framework: string;
  dataId: string;
}

export interface LayoutListResult extends ListResultDto<Layout> {}

export interface LayoutPagedResult extends PagedResultDto<Layout> {}

export interface CreateOrUpdateLayout {
  name: string;
  path: string;
  displayName: string;
  description?: string;
  redirect?: string;
}

export interface CreateLayout extends CreateOrUpdateLayout {
  dataId: string;
  framework: string;
}

export type UpdateLayout = CreateOrUpdateLayout;

export interface GetLayoutPagedRequest extends PagedAndSortedResultRequestDto {
  filter?: string;
  reverse?: boolean;
  framework?: string;
}
