import { ListResultDto } from '/@/api/model/baseModel';

export interface Text {
  key: boolean;
  value: string;
  cultureName: string;
  resourceName: string;
}

export interface TextDifference {
  key: boolean;
  value: string;
  cultureName: string;
  resourceName: string;
  targetCultureName: string;
  targetValue: string;
}

export interface SetTextInput {
  key: boolean;
  value: string;
  cultureName: string;
  resourceName: string;
}

export class TextListResult extends ListResultDto<TextDifference> {}

export interface GetTextByKey {
  key: boolean;
  cultureName: string;
  resourceName: string;
}

export class GetTextRequest {
  filter = '';
  cultureName = '';
  targetCultureName = '';
  resourceName = '';
  onlyNull = false;
}
