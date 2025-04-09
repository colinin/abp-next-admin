interface GetTextByKeyInput {
  cultureName: string;
  key: string;
  resourceName: string;
}

interface GetTextsInput {
  cultureName: string;
  filter?: string;
  onlyNull?: boolean;
  resourceName?: string;
  targetCultureName: string;
}

interface SetTextInput {
  cultureName: string;
  key: string;
  resourceName: string;
  value: string;
}

interface RestoreDefaultTextInput {
  cultureName: string;
  key: string;
  resourceName: string;
}

interface TextDifferenceDto {
  cultureName: string;
  key: string;
  resourceName: string;
  targetCultureName: string;
  targetValue: string;
  value: string;
}

interface TextDto {
  cultureName: string;
  key: string;
  resourceName: string;
  value: string;
}

export type {
  GetTextByKeyInput,
  GetTextsInput,
  RestoreDefaultTextInput,
  SetTextInput,
  TextDifferenceDto,
  TextDto,
};
