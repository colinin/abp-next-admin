/** 数据访问策略 */
enum DataAccessStrategy {
  /** 可以访问所有数据 */
  All = 0,
  /** 仅当前用户组织机构及下级机构 */
  CurrentAndSubOrganizationUnits = 5,
  /** 仅当前用户组织机构 */
  CurrentOrganizationUnits = 4,
  /** 仅当前用户角色 */
  CurrentRoles = 3,
  /** 仅当前用户 */
  CurrentUser = 2,
  /** 自定义规则 */
  Custom = 1,
}

interface SubjectStrategyGetInput {
  subjectId: string;
  subjectName: string;
}

interface SubjectStrategySetInput {
  isEnabled: boolean;
  strategy: DataAccessStrategy;
  subjectId: string;
  subjectName: string;
}

interface SubjectStrategyDto {
  isEnabled: boolean;
  strategy: DataAccessStrategy;
  subjectId: string;
  subjectName: string;
}

export { DataAccessStrategy };

export type {
  SubjectStrategyDto,
  SubjectStrategyGetInput,
  SubjectStrategySetInput,
};
