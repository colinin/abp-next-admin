export enum JobActionType {
  Failed = -1,
  Successed = 0,
}

export interface BackgroundJobAction {
  id: string;
  jobId: string;
  name: string;
  displayName?: string;
  isEnabled: boolean;
  paramters: ExtraPropertyDictionary;
}

export interface BackgroundJobActionParamter {
  name: string;
  required: boolean;
  displayName: string;
  description?: string;
}

export interface BackgroundJobActionDefinition {
  name: string;
  type: JobActionType;
  displayName: string;
  description?: string;
  paramters: BackgroundJobActionParamter[];
}

export interface CreateBackgroundJobAction {
  name: string;
  isEnabled: boolean;
  paramters: ExtraPropertyDictionary;
}

export interface UpdateBackgroundJobAction {
  isEnabled: boolean;
  paramters: ExtraPropertyDictionary;
}

export interface BackgroundJobActionGetDefinitionsInput {
  type?: JobActionType;
}
