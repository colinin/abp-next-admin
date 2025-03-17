interface RemoteServiceValidationErrorInfo {
  members: string[];
  message: string;
}

interface RemoteServiceErrorInfo {
  code?: string;
  data?: Record<string, any>;
  details?: string;
  message?: string;
  validationErrors?: RemoteServiceValidationErrorInfo[];
}

export type { RemoteServiceErrorInfo };
