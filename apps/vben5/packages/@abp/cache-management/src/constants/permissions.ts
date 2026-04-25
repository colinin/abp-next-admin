const CachePrefix = 'AbpCachingManagement.Cache';

export const CachingManagementPermissions = {
  Refresh: `${CachePrefix}.Refresh`,
  Default: CachePrefix,
  Delete: `${CachePrefix}.Delete`,
  ManageValue: `${CachePrefix}.ManageValue`,
};
