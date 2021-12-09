export enum ValueType {
  String = 0,
  Numeic = 1,
  Boolean = 2,
  Date = 3,
  DateTime = 4,
  Array = 5,
  Object = 6,
}

export interface DataItem {
  id: string;
  allowBeNull: boolean;
  defaultValue?: string;
  description?: string;
  displayName: string;
  name: string;
  valueType: ValueType;
}
