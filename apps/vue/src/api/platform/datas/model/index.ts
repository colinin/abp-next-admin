 export interface DataItem {
    id: string;
    allowBeNull: boolean;
    defaultValue?: string;
    description?: string;
    displayName: string;
    name: string;
    valueType: ValueType;
  }
  
export interface Data {
  id: string;
  name: string;
  code: string;
  displayName: string;
  description?: string;
  parentId?: string;
  items: DataItem[];
}

export enum ValueType {
  String = 0,
  Numeic = 1,
  Boolean = 2,
  Date = 3,
  DateTime = 4,
  Array = 5,
  Object = 6,
}

export interface GetDataByPaged extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface DataPagedResult extends PagedResultDto<Data> {}

export interface DataListResult extends ListResultDto<Data> {}

export class CreateOrUpdateData {
  name = '';
  displayName = '';
  description? = '';
}

export class CreateData extends CreateOrUpdateData {
  parentId?: string;
}

export class UpdateData extends CreateOrUpdateData {}

export class CreateOrUpdateDataItem {
  defaultValue = '';
  displayName = '';
  description? = '';
  allowBeNull = true;
  valueType = ValueType.String;
}

export class UpdateDataItem extends CreateOrUpdateDataItem {}

export class CreateDataItem extends CreateOrUpdateDataItem {
  name = '';
}
