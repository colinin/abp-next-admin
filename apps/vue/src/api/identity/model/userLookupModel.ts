import { ListResultDto, PagedAndSortedResultRequestDto } from '../../model/baseModel';

export interface IUserData {
  id: string;
  tenantId?: string;
  userName: string;
  name: string;
  surname: string;
  email: string;
  emailConfirmed: boolean;
  phoneNumber: string;
  phoneNumberConfirmed: boolean;
}

export interface UserLookupSearchRequest extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface UserLookupCountRequest extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface UserLookupSearchResult extends ListResultDto<IUserData> {}
