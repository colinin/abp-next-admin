declare interface LocalizableStringInfo {
  resourceName: string;
  name: string;
}

declare type ExtraPropertyDictionary = { [key: string]: any };

declare interface ISelectionStringValueItem {
  value: string;
  displayText: LocalizableStringInfo;
}

declare interface ISelectionStringValueItemSource {
  items: ISelectionStringValueItem[];
}

declare interface Validator {
  name: string;
  properties: { [key: string]: string };
}

declare interface ValueType {
  name: string;
  validator: Validator;
  properties: { [key: string]: string };
}

declare interface SelectionStringValueType extends ValueType {
  itemSource: ISelectionStringValueItemSource;
}
