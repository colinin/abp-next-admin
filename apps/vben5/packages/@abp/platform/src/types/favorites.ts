import type { AuditedEntityDto, IHasConcurrencyStamp } from '@abp/core';

interface UserFavoriteMenuDto extends AuditedEntityDto<string> {
  aliasName?: string;
  color?: string;
  displayName: string;
  framework: string;
  icon?: string;
  menuId: string;
  name: string;
  path?: string;
  userId: string;
}

interface UserFavoriteMenuCreateOrUpdateDto {
  aliasName?: string;
  color?: string;
  icon?: string;
  menuId: string;
}

interface UserFavoriteMenuCreateDto extends UserFavoriteMenuCreateOrUpdateDto {
  framework: string;
}

interface UserFavoriteMenuUpdateDto
  extends IHasConcurrencyStamp,
    UserFavoriteMenuCreateOrUpdateDto {}

export type {
  UserFavoriteMenuCreateDto,
  UserFavoriteMenuDto,
  UserFavoriteMenuUpdateDto,
};
