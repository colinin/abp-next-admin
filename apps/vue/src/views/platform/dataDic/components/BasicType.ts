import { ValueType } from '/@/api/platform/model/dataItemModel';

export const valueTypeMaps: { [key: number]: string } = {
  [ValueType.String]: 'String',
  [ValueType.Numeic]: 'Number',
  [ValueType.Date]: 'Date',
  [ValueType.DateTime]: 'DateTime',
  [ValueType.Boolean]: 'Boolean',
  [ValueType.Array]: 'Array',
  [ValueType.Object]: 'Object',
};

export const valueTypeOptions = [
  {
    label: 'String',
    value: ValueType.String,
    key: ValueType.String,
  },
  {
    label: 'Number',
    value: ValueType.Numeic,
    key: ValueType.Numeic,
  },
  {
    label: 'Date',
    value: ValueType.Date,
    key: ValueType.Date,
  },
  {
    label: 'DateTime',
    value: ValueType.DateTime,
    key: ValueType.DateTime,
  },
  {
    label: 'Boolean',
    value: ValueType.Boolean,
    key: ValueType.Boolean,
  },
  {
    label: 'Array',
    value: ValueType.Array,
    key: ValueType.Array,
  },
  {
    label: 'Object',
    value: ValueType.Object,
    key: ValueType.Object,
  },
];
