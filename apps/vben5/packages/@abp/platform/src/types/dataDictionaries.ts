import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/core';

enum ValueType {
  Array = 5,
  Boolean = 2,
  Date = 3,
  DateTime = 4,
  Numeic = 1,
  Object = 6,
  String = 0,
}

interface DataItemDto extends EntityDto<string> {
  allowBeNull: boolean;
  defaultValue?: string;
  description?: string;
  displayName: string;
  name: string;
  valueType: ValueType;
}

interface DataDto extends EntityDto<string> {
  code: string;
  description?: string;
  displayName: string;
  items: DataItemDto[];
  name: string;
  parentId?: string;
}

interface DataCreateOrUpdateDto {
  description?: string;
  displayName: string;
  name: string;
}

interface DataCreateDto extends DataCreateOrUpdateDto {
  parentId?: string;
}

interface DataItemCreateOrUpdateDto {
  allowBeNull: boolean;
  defaultValue?: string;
  description?: string;
  displayName: string;
  valueType: ValueType;
}

interface DataItemCreateDto extends DataItemCreateOrUpdateDto {
  name: string;
}

interface GetDataListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

interface DataMoveDto {
  parentId?: string;
}

type DataUpdateDto = DataCreateOrUpdateDto;

type DataItemUpdateDto = DataItemCreateOrUpdateDto;

export { ValueType };

export type {
  DataCreateDto,
  DataDto,
  DataItemCreateDto,
  DataItemDto,
  DataItemUpdateDto,
  DataMoveDto,
  DataUpdateDto,
  GetDataListInput,
};
