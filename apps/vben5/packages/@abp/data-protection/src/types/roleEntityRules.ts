import type {
  DataAccessFilterGroup,
  DataAccessOperation,
  EntityRuleDtoBase,
} from './entityRules';
import type { DataAccessStrategy } from './strategys';

interface RoleEntityRuleGetInput {
  entityTypeId: string;
  operation: DataAccessOperation;
  roleName: string;
}

interface EntityRuleCreateOrUpdateDto {
  accessedProperties?: string[];
  filterGroup: DataAccessFilterGroup;
  isEnabled: boolean;
  operation: DataAccessOperation;
}

interface RoleEntityRuleCreateDto extends EntityRuleCreateOrUpdateDto {
  entityTypeId: string;
  roleId: string;
  roleName: string;
}

type RoleEntityRuleUpdateDto = EntityRuleCreateOrUpdateDto;

interface RoleEntityRuleDto extends EntityRuleDtoBase {
  roleId: string;
  roleName: string;
  /** 数据访问策略 */
  strategy: DataAccessStrategy;
}

export type {
  RoleEntityRuleCreateDto,
  RoleEntityRuleDto,
  RoleEntityRuleGetInput,
  RoleEntityRuleUpdateDto,
};
