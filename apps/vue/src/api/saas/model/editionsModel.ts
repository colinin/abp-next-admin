import {
  IHasConcurrencyStamp,
  AuditedEntityDto,
  PagedAndSortedResultRequestDto,
} from '../../model/baseModel';

export interface Edition extends AuditedEntityDto {
  id: string;
  displayName: string;
}

interface EditionCreateOrUpdate {
  displayName: string;
}

export type EditionCreate = EditionCreateOrUpdate;

export interface EditionUpdate extends EditionCreateOrUpdate, IHasConcurrencyStamp {
}

export interface EditionGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}
