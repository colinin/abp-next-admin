import {
  PagedAndSortedResultRequestDto,
  ListResultDto,
  PagedResultDto,
} from '../../model/baseModel';
import { Route } from './basicModel';

export class Layout extends Route {
  framework!: string;
  dataId!: string;
}

export class LayoutListResult extends ListResultDto<Layout> {}

export class LayoutPagedResult extends PagedResultDto<Layout> {}

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

export class GetLayoutPagedRequest extends PagedAndSortedResultRequestDto {
  filter = '';
  reverse = false;
  framework = '';
}
