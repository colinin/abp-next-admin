import { PagedAndSortedResultRequestDto, PagedResultDto } from '/@/api/model/baseModel';

export interface Text {
  id?: number;
  key: boolean;
  value: string;
  cultureName: string;
  resourceName: string;
}

export interface TextDifference {
  id: number;
  key: boolean;
  value: string;
  cultureName: string;
  resourceName: string;
  targetCultureName: string;
  targetValue: string;
}

export interface TextCreateOrUpdate {
  value: string;
}

export interface TextCreate extends TextCreateOrUpdate {
  key: boolean;
  cultureName: string;
  resourceName: string;
}

export type TextUpdate = TextCreateOrUpdate;

export class TextPagedResult extends PagedResultDto<TextDifference> {}

export interface GetTextByKey {
  key: boolean;
  cultureName: string;
  resourceName: string;
}

export class GetTextPagedRequest extends PagedAndSortedResultRequestDto {
  filter = '';
  cultureName = '';
  targetCultureName = '';
  resourceName = '';
  onlyNull = false;
}
