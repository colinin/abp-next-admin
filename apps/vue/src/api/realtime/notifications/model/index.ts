import { NotificationReadState, NotificationSeverity } from "../types";

export interface GetNotificationPagedRequest extends PagedAndSortedResultRequestDto {
  reverse?: boolean;
  readState?: NotificationReadState;
}

export interface UserIdentifier {
  userId: string;
  userName?: string;
}

export interface NotificationSendDto {
  name: string;
  culture?: string;
  toUsers?: UserIdentifier[];
  severity?: NotificationSeverity;
  data: Dictionary<string, any>;
}
