export interface Text {
    key: string;
    value: string;
    cultureName: string;
    resourceName: string;
  }
  
  export interface TextDifference {
    key: string;
    value: string;
    cultureName: string;
    resourceName: string;
    targetCultureName: string;
    targetValue: string;
  }
  
  export interface SetTextInput {
    key: string;
    value: string;
    cultureName: string;
    resourceName: string;
  }
  
  export interface GetTextByKey {
    key: string;
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
  