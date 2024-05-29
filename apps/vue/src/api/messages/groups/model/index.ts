export interface Group {
    id: string;
    name: string;
    avatarUrl: string;
    allowAnonymous: boolean;
    allowSendMessage: boolean;
    maxUserLength: number;
    groupUserCount: number;
  }
  
  export interface GroupSearchRequest extends PagedAndSortedResultRequestDto {
    filter: string;
  }
  