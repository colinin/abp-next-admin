export type AbpRecord = Record<string, any>;

export type AbpId = number | string;

export type AbpIdOf<TDto extends AbpRecord> = TDto extends { id: infer TId }
  ? TId
  : AbpId;
